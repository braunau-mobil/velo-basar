using System;

namespace BraunauMobil.VeloBasar.Models
{
    public class FileStore
    {
        public int Id { get; set; }

        public string ContentType { get; set; }

        public byte[] Data { get; set; }
    }
}
