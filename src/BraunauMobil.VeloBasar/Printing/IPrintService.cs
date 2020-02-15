using System.Collections.Generic;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Printing
{
    public interface IPrintService
    {
        byte[] Combine(IEnumerable<byte[]> pdfs);
        byte[] CreateTransaction(ProductsTransaction acceptance, PrintSettings settings);
        byte[] CreateLabel(Product product, PrintSettings settings);
    }
}
