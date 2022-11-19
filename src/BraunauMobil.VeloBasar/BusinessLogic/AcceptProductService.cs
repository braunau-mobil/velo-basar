using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Xan.AspNetCore.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class AcceptProductService
    : IAcceptProductService
{
    private readonly VeloDbContext _db;
    private readonly VeloTexts _txt;

    public AcceptProductService(VeloDbContext db, VeloTexts txt)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));
    }

    public async Task<AcceptProductModel> CreateNewAsync(int sessionId)
    {
        AcceptSessionEntity session = await GetSession(sessionId);

        if (session.IsCompleted)
        {
            throw new InvalidOperationException(_txt.AcceptSessionIsCompleted(sessionId));
        }

        ProductEntity newProduct = new()
        {
            SessionId = sessionId
        };

        return CreateModel(newProduct, session);
    }

    public async Task CreateAsync(ProductEntity product)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (product.SessionId == 0)
        {
            throw new InvalidOperationException("Cannot create product without session");
        }

        _db.Products.Add(product);

        await _db.SaveChangesAsync();
        await ReloadRelations(product);
    }

    public async Task DeleteAsync(int productId)
    {
        ProductEntity entity = await _db.Products.FirstByIdAsync(productId);
        _db.Products.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<AcceptProductModel> GetAsync(int productId)
    {
        ProductEntity product = await _db.Products
            .Include(product => product.Brand)
            .Include(product => product.Type)
            .FirstByIdAsync(productId);

        AcceptSessionEntity session = await GetSession(product.SessionId);
        return CreateModel(product, session);
    }

    public async Task<AcceptProductModel> GetAsync(int sessionId, ProductEntity product)
    {
        ArgumentNullException.ThrowIfNull(product);

        AcceptSessionEntity session = await GetSession(sessionId);
        return CreateModel(product, session);
    }

    public async Task UpdateAsync(ProductEntity product)
    {
        ArgumentNullException.ThrowIfNull(product);

        _db.Attach(product)
            .State = EntityState.Modified;

        await _db.SaveChangesAsync();
        await ReloadRelations(product);
    }

    private async Task ReloadRelations(ProductEntity product)
    {
        foreach (ReferenceEntry reference in _db.Entry(product).References)
        {
            await reference.LoadAsync();
        }
    }

    private async Task<AcceptSessionEntity> GetSession(int id)
        => await _db.AcceptSessions
            .Include(session => session.Products)
                .ThenInclude(product => product.Brand)
            .Include(session => session.Products)
                .ThenInclude(product => product.Type)
            .FirstByIdAsync(id);

    private static AcceptProductModel CreateModel(ProductEntity product, AcceptSessionEntity session)
        => new()
        {
            CanAccept = session.CanAccept(),
            Entity = product,
            SellerId = session.SellerId,
            SessionId = product.SessionId,
            Products = session.Products.ToArray()
        };
}
