using BraunauMobil.VeloBasar.Models.Interfaces;

namespace BraunauMobil.VeloBasar.Models
{
    public class FileData : IModel
    {
        public int Id { get; set; }

        public string ContentType { get; set; }

        public byte[] Data { get; set; }
    }
}
