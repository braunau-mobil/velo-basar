using BraunauMobil.VeloBasar.Models;
using System;
using System.Linq.Expressions;

namespace BraunauMobil.VeloBasar.Data
{
    public static class Expressions
    {
        public static Expression<Func<Product, bool>> ProductSearch(string searchString)
        {
            return p => p.Brand.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
                || p.Color.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
                || p.Description.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
                || p.FrameNumber.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
                || p.TireSize.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
                || p.Type.Contains(searchString, StringComparison.InvariantCultureIgnoreCase);
        }

        public static Expression<Func<Seller, bool>> SellerSearch(string searchString)
        {
            return s => s.FirstName.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
              || s.LastName.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
              || s.City.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
              || s.Country.Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
              || (s.BankAccountHolder != null && s.BankAccountHolder.Contains(searchString, StringComparison.InvariantCultureIgnoreCase));
        }

        public static Expression<Func<Seller, bool>> SellerSearch(string firstName, string lastName)
        {
            if (!string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            {
                return s => string.Equals(s.FirstName, firstName, StringComparison.InvariantCultureIgnoreCase);
            }
            else if (string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                return s => string.Equals(s.LastName, lastName, StringComparison.InvariantCultureIgnoreCase);
            }

            return s => string.Equals(s.FirstName, firstName, StringComparison.InvariantCultureIgnoreCase)
              && string.Equals(s.LastName, lastName, StringComparison.InvariantCultureIgnoreCase);
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
