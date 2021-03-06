﻿using BraunauMobil.VeloBasar.Models;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public interface IStatisticContext
    {
        Task<BasarStatistic> GetBasarStatisticAsnyc(int basarId);
        Task<SellerStatistics> GetSellerStatisticsAsync(Basar basar, int sellerId);
    }
}
