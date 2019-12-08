﻿using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public class FileStoreContext : IFileStoreContext
    {
        private readonly VeloRepository _db;

        public FileStoreContext(VeloRepository db)
        {
            _db = db;
        }

        public Task<bool> ExistsAsync(int id) => _db.Files.ExistsAsync(id);
        public async Task<FileData> GetAsync(int id)
        {
            return await _db.Files.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}