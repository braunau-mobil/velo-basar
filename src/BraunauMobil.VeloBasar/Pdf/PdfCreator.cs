using System;
using System.Collections.Generic;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Pdf
{
    public class PdfCreator
    {
        public byte[] Combine(IEnumerable<byte[]> pdfs)
        {
            throw new NotImplementedException();
        }

        public byte[] CreateAcceptance(Acceptance acceptance)
        {
            throw new NotImplementedException();
        }

        public byte[] CreateLabel(Product product)
        {
            throw new NotImplementedException();
        }

        public byte[] CreateSettlement(Settlement settlement)
        {
            throw new NotImplementedException();
        }
    }
}
