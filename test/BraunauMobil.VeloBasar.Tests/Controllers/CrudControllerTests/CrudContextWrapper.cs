#warning    @todo
//using BraunauMobil.VeloBasar.BusinessLogic;
//using BraunauMobil.VeloBasar.Models.Interfaces;
//using Xan.Core.Collections.Generic;

//namespace BraunauMobil.VeloBasar.Tests.Controllers.CrudControllerTests;

//public class CrudContextWrapper<TModel>
//    : ICrudContext<TModel>
//    where TModel : IModel
//{
//    private readonly ICrudContext<TModel> _context;

//    public CrudContextWrapper(ICrudContext<TModel> contextToWrap)
//    {
//        _context = contextToWrap ?? throw new ArgumentNullException(nameof(contextToWrap));
//    }

//    public Task<bool> CanDeleteAsync(TModel item)
//        => _context.CanDeleteAsync(item);

//    public Task<TModel> CreateAsync(TModel item)
//        => _context.CreateAsync(item);

//    public Task DeleteAsync(int id)
//        => _context.DeleteAsync(id);

//    public Task<bool> ExistsAsync(int id)
//        => _context.ExistsAsync(id);

//    public Task<TModel?> GetAsync(int id)
//        => _context.GetAsync(id);

//    public Task<IPaginatedList<TModel>> GetManyAsync(int pageSize, int pageIndex, string? searchString = null)
//        => _context.GetManyAsync(pageSize, pageIndex, searchString);

//    public Task UpdateAsync(TModel item)
//        => _context.UpdateAsync(item);
//}

