using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface IAcceptSessionService
{
    Task<AcceptSessionEntity> CreateAsync(int basarId, int sellerId);

    Task DeleteAsync(int id);

    Task<AcceptSessionEntity?> FindAsync(int id);

    Task<IPaginatedList<AcceptSessionEntity>> GetAllAsync(int pageSize, int pageIndex, int basarId, AcceptSessionState? state);
    
    Task<AcceptSessionEntity> GetAsync(int id);

    Task<bool> IsSessionRunning(int? id);

    Task<int> SubmitAsync(int id);
}
