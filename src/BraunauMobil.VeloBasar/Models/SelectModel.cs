namespace BraunauMobil.VeloBasar.Models;

#nullable disable warnings
public sealed class SelectModel<T>
{
    public SelectModel()
    { }

    public SelectModel(T value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public bool IsSelected { get; set; }

    public T Value { get; set; }
}
