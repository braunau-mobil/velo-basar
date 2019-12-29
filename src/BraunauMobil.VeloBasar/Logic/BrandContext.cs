﻿using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public class BrandContext : IBrandContext
    {
        private readonly VeloRepository _db;

        public BrandContext(VeloRepository dbContext)
        {
            _db = dbContext;
        }

        public async Task<bool> CanDeleteAsync(Brand item) => !await _db.Products.AnyAsync(p => p.BrandId == item.Id);
        public async Task CreateAsync(Brand brand)
        {
            _db.Brands.Add(brand);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var brand = await GetAsync(id);
            if (brand != null)
            {
                _db.Brands.Remove(brand);
                await _db.SaveChangesAsync();
            }
        }
        public async Task<bool> ExistsAsync(int id) => await _db.Brands.ExistsAsync(id);
        public async Task<Brand> GetAsync(int id) => await _db.Brands.FirstOrDefaultAsync(p => p.Id == id);
        public IQueryable<Brand> GetMany(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return _db.Brands.DefaultOrder();
            }
            return _db.Brands.Where(BrandSearch(searchString)).DefaultOrder();
        }

        public SelectList GetSelectList() => new SelectList(_db.Brands, "Id", "Name");
        public async Task UpdateAsync(Brand brand)
        {
            _db.Attach(brand).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        private static Expression<Func<Brand, bool>> BrandSearch(string searchString)
        {
            if (int.TryParse(searchString, out int id))
            {
                return b => b.Id == id;
            }
            return b => EF.Functions.Like(b.Name, $"%{searchString}%");
        }
    }
}
