using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public class SellerContext : ISellerContext
    {
        private readonly VeloRepository _db;

        public SellerContext(VeloRepository db)
        {
            _db = db;
        }

        public async Task CreateAsync(Seller seller)
        {
            _db.Sellers.Add(seller);
            await _db.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id) => await _db.Sellers.ExistsAsync(id);
        public async Task<Seller> GetAsync(int id) => await _db.Sellers.IncludeAll().FirstOrDefaultAsync(s => s.Id == id);
        public IQueryable<Seller> GetMany(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return _db.Sellers.IncludeAll().OrderById();
            }

            return _db.Sellers.Where(SellerSearch(searchString)).IncludeAll().OrderById();
        }
        public IQueryable<Seller> GetMany(string firstName, string lastName)
        {
            return _db.Sellers.Where(SellerSearch(firstName, lastName)).IncludeAll().OrderById();
        }
        public async Task UpdateAsync(Seller seller)
        {
            _db.Attach(seller).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        private static Expression<Func<Seller, bool>> SellerSearch(string searchString)
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
                return s => s.FirstName.StartsWith(firstName, StringComparison.InvariantCultureIgnoreCase);
            }
            else if (string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                return s => s.LastName.StartsWith(lastName, StringComparison.InvariantCultureIgnoreCase);
            }

            return s => s.FirstName.StartsWith(firstName, StringComparison.InvariantCultureIgnoreCase)
              && s.LastName.StartsWith(lastName, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
