using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using Xan.AspNetCore.EntityFrameworkCore;
using Xan.Extensions;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class AcceptSessionService
    : IAcceptSessionService
{
    private readonly VeloDbContext _db;
    private readonly ITransactionService _transactionService;
    private readonly IClock _clock;

    public AcceptSessionService(VeloDbContext db, ITransactionService transactionService, IClock clock)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }

    public async Task<AcceptSessionEntity> CreateAsync(int basarId, int sellerId)
    {
        SellerEntity seller = await _db.Sellers.FirstByIdAsync(sellerId);

        AcceptSessionEntity newSession = new (basarId, seller, _clock.GetCurrentDateTime());
        _db.AcceptSessions.Add(newSession);
        await _db.SaveChangesAsync();

        return newSession;
    }

    public async Task DeleteAsync(int id)
    {
        AcceptSessionEntity session = await _db.AcceptSessions.FirstByIdAsync(id);

        _db.AcceptSessions.Remove(session);
        await _db.SaveChangesAsync();
    }

    public async Task<AcceptSessionEntity?> FindAsync(int id)
        => await _db.AcceptSessions
            .FirstOrDefaultByIdAsync(id);

    public async Task<IPaginatedList<AcceptSessionEntity>> GetAllAsync(int pageSize, int pageIndex, int basarId, AcceptSessionState? state)
    {
        IQueryable<AcceptSessionEntity> iq = _db.AcceptSessions
            .Include(session => session.Products)
            .Where(session => session.BasarId == basarId);
        if (state.HasValue)
        {
            iq = iq.Where(session => session.State == state.Value);
        }
        iq = iq.OrderBy(session => session.StartTimeStamp);

        return await iq.AsPaginatedAsync(pageSize, pageIndex);
    }

    public async Task<AcceptSessionEntity> GetAsync(int id)
        => await _db.AcceptSessions
            .Include(s => s.Seller)
            .Include(s => s.Basar)
            .Include(s => s.Products)
            .Include(s => s.Products)
                .ThenInclude(p => p.Brand)
            .Include(s => s.Products)
                .ThenInclude(p => p.Type)
            .FirstByIdAsync(id);

    public async Task<bool> IsSessionRunning(int? id)
    {
        if (id.HasValue)
        {
            AcceptSessionEntity? acceptSession = await _db.AcceptSessions.FirstOrDefaultByIdAsync(id.Value);
            if (acceptSession != null)
            {
                return !acceptSession.IsCompleted;
            }
        }
        return false;
    }

    public async Task<int> SubmitAsync(int id)
    {
        AcceptSessionEntity session = await _db.AcceptSessions
            .Include(session => session.Seller)
            .Include(session => session.Products)
            .FirstByIdAsync(id);

        session.Complete(_clock.GetCurrentDateTime());
        return await _transactionService.AcceptAsync(session.BasarId, session.SellerId, session.Products.Ids());
    }
}
