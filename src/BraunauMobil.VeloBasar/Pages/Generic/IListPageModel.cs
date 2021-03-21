using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Pages.Generic
{
    public interface IListPageModel
    {
        IEnumerable<IListItem> Items { get; }
        /// <summary>
        /// @todo this should be implemented on ListPageModel, or inhert IListPageModel : IPaginatable
        /// </summary>
        IPaginatable Paginatable { get; }

        Task<bool> CanDeleteAsync(IListItem item);
        VeloPage CreatePage();
        VeloPage DeletePage(IListItem item);
        VeloPage EditPage(IListItem item);
        VeloPage SetStatePage(IListItem item, ObjectState stateToSet);
        VeloPage SearchPage();
    }
}
