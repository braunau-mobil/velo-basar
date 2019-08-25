using BraunauMobil.VeloBasar.Models;
using System;
using System.Linq.Expressions;

namespace BraunauMobil.VeloBasar.Data
{
    public static class Expressions
    {
        public static Expression<Func<Seller, bool>> SellerSearch(string searchString)
        {
            return s => s.FirstName.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
              || s.LastName.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
              || s.City.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
              || s.Country.Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
              || (s.BankAccountHolder != null && s.BankAccountHolder.Contains(searchString, StringComparison.InvariantCultureIgnoreCase));
        }

        public static Expression<Func<ProductsTransaction, bool>> TransactionSearch(string searchString)
        {
            return x => (x.Seller == null) ? true : 
                 x.Seller.FirstName.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
              || x.Seller.LastName.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
              || x.Seller.City.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
              || x.Seller.Country.Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
              || (x.Seller.BankAccountHolder != null && x.Seller.BankAccountHolder.Contains(searchString, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
