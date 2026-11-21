#!/usr/bin/env bash
# Manages a throwaway PostgreSQL container (Podman) for local development.
# The database is ephemeral: `stop` removes the container including all data.
#
# The port is also referenced by:
#   - src/BraunauMobil.VeloBasar/appsettings.Development.json (ConnectionString)
#   - test/BraunauMobil.VeloBasar.DataGenerator/Program.cs
set -euo pipefail

NAME=velobasar-dev-db
IMAGE=docker.io/library/postgres:18
PORT=${DEV_DB_PORT:-55432}
DB_USER=postgres
DB_PASSWORD=postgres
DB_NAME=velobasar
READY_TIMEOUT_SECONDS=30

cd "$(dirname "$0")/.."

connection_string() {
    echo "User ID=$DB_USER;Password=$DB_PASSWORD;Host=localhost;Port=$PORT;Database=$DB_NAME;Pooling=true;"
}

require_podman() {
    if ! command -v podman >/dev/null 2>&1; then
        echo "podman is not installed." >&2
        echo "  Fedora: sudo dnf install podman" >&2
        echo "  Ubuntu: sudo apt install podman" >&2
        exit 1
    fi
}

is_running() {
    [[ "$(podman container inspect -f '{{.State.Running}}' "$NAME" 2>/dev/null)" == "true" ]]
}

wait_until_ready() {
    # The postgres image runs a temporary, socket-only server during init and then restarts,
    # so only a TCP check tells us that the final server is up.
    local waited=0
    until podman exec "$NAME" pg_isready -q -h 127.0.0.1 -U "$DB_USER" -d "$DB_NAME" 2>/dev/null; do
        if ! is_running; then
            echo "Container stopped unexpectedly." >&2
            exit 1
        fi
        if (( waited >= READY_TIMEOUT_SECONDS )); then
            echo "Database not ready after ${READY_TIMEOUT_SECONDS}s. See: $0 logs" >&2
            exit 1
        fi
        sleep 1
        waited=$((waited + 1))
    done
}

cmd_start() {
    if is_running; then
        echo "Already running."
        cmd_status
        return
    fi

    # Remove leftovers (e.g. a container stuck in 'created' state after a failed start).
    podman rm -f "$NAME" >/dev/null 2>&1 || true

    echo "Starting $IMAGE as '$NAME' on 127.0.0.1:$PORT ..."
    if ! podman run -d --rm \
        --name "$NAME" \
        -p "127.0.0.1:$PORT:5432" \
        -e POSTGRES_USER="$DB_USER" \
        -e POSTGRES_PASSWORD="$DB_PASSWORD" \
        -e POSTGRES_DB="$DB_NAME" \
        "$IMAGE" >/dev/null; then
        echo "Failed to start the container. If port $PORT is in use, choose another one with DEV_DB_PORT=<port>." >&2
        exit 1
    fi

    wait_until_ready
    echo "Ready."
    cmd_status
}

cmd_stop() {
    if ! is_running; then
        echo "Not running."
        return
    fi
    echo "Stopping '$NAME' (all data will be removed) ..."
    podman stop "$NAME" >/dev/null
    echo "Stopped."
}

cmd_status() {
    if is_running; then
        echo "Status:     running ($NAME, $IMAGE)"
        echo "Connection: $(connection_string)"
    else
        echo "Status:     not running"
    fi
}

cmd_logs() {
    podman logs -f "$@" "$NAME"
}

cmd_psql() {
    if ! is_running; then
        echo "Not running. Start it with: $0 start" >&2
        exit 1
    fi
    local tty_flags=-i
    [[ -t 0 && -t 1 ]] && tty_flags=-it
    podman exec "$tty_flags" "$NAME" psql -U "$DB_USER" -d "$DB_NAME" "$@"
}

cmd_seed() {
    is_running || cmd_start
    dotnet run --project test/BraunauMobil.VeloBasar.DataGenerator/BraunauMobil.VeloBasar.DataGenerator.csproj
}

usage() {
    cat <<EOF
Usage: $0 <command> [args]

Manages an ephemeral PostgreSQL development container (Podman).

Commands:
  start    Start the database and wait until it accepts connections
  stop     Stop the database (removes the container and ALL data)
  reset    Stop and start again (fresh, empty database)
  status   Show whether the database is running and its connection string
  logs     Follow the container logs (extra args go to 'podman logs')
  psql     Open a psql shell in the container (extra args go to psql)
  seed     Fill the database with generated test data (DataGenerator)

Environment:
  DEV_DB_PORT   Host port to bind (default: 55432)
EOF
}

command=${1:-}
[[ $# -gt 0 ]] && shift

case "$command" in
    start)  require_podman; cmd_start ;;
    stop)   require_podman; cmd_stop ;;
    reset)  require_podman; cmd_stop; cmd_start ;;
    status) require_podman; cmd_status ;;
    logs)   require_podman; cmd_logs "$@" ;;
    psql)   require_podman; cmd_psql "$@" ;;
    seed)   require_podman; cmd_seed ;;
    -h|--help|help) usage ;;
    *) usage >&2; exit 1 ;;
esac
