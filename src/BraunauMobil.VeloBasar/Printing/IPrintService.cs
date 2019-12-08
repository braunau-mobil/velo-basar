﻿using System.Collections.Generic;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Printing
{
    public interface IPrintService
    {
        byte[] Combine(IEnumerable<byte[]> pdfs);
        byte[] CreateAcceptance(ProductsTransaction acceptance, PrintSettings settings);
        byte[] CreateLabel(Basar basar, Product product);
        byte[] CreateSale(ProductsTransaction sale);
        byte[] CreateSettlement(ProductsTransaction settlement);
    }
}