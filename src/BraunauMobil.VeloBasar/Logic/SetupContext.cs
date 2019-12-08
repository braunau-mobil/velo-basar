using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public class SetupContext : ISetupContext
    {
        private static readonly string[] _brandNames = { "Additive", "Agresti", "Airstreeem", "Akkurad", "Alutech Cycles", "AnthroTech", "AT Zweirad", "Atlanta", "Ave Hyprid Bikes", "Bavaria", "Bellini", "Bergamont", "Bernds", "Bikespace", "Bionicon", "BLACK LABEL", "Boomer", "Böttcher", "Brenner Cycles", "Brothers Bikes", "Bulls", "C 14", "Campus", "Canyon Bicycles", "Carver", "Ccr", "Centurion", "Checker Pig", "Cheetah", "Cinelli Bikes", "Cito", "Cobra", "Cocoon", "Contoura", "Conway", "Corona", "Corratec", "Cresta", "Cube", "Cucuma", "Cyclecraft", "Cyclewolf", "Cyclomanix", "Dalliegerad", "Dancelli", "Definder Cycle Manufaktur", "Diamant", "Dragonfly", "Draisin", "Drössiger", "Druxs", "Düll", "Duo Bike", "Dürkopp", "Dynamics", "Easy Rider", "Egon Rahe", "Electrolyte", "Elfa", "Elom", "EMANON", "Endorfin", "Enik", "Epple", "Express", "Faggin", "Falke", "Falkenjagd", "Falter", "Fatmodul", "Fischer", "Fixie Inc.", "Fleiner", "Flitz Bike", "Flux", "Focus", "Frank Bikes", "Frischauf", "Frosch Rad", "Fxx Cycles", "Gaastra", "Garage 271", "Geier", "German A", "Germans Cycles", "Ghost", "Gigant", "Gmp", "Gobax", "Gold-Rad", "Göricke", "Grace", "Gudereit", "Guylaine", "Haibike", "Hammonia", "Handybike", "Hase Bikes", "Hawk", "Hercules", "Herkelmann", "Hot Chili", "HP Velotechnik", "Hrinkow", "Idworx", "Inchuan", "Indienrad", "Jaguar", "Jakel", "Jan Ullrich Bikes", "Juchem", "Jungherz", "Kalkhoff", "Kania", "Katarga", "Kemper", "Kenhill", "Kettler", "KHEbikes", "Koch Bikes", "Kokua", "Kondor", "Kotter", "Krabo", "Kreidler", "KTM", "Lakes", "Langenberg", "Last", "Leafcycles", "Lehmkuhl", "Leiba", "Liing", "Liteville", "Mars", "Marschall", "Maxcycles", "Maxx", "Mercedes-Benz", "Merida Bikes", "Miele", "MIFA", "Mini", "Mondello", "Morrison", "Möve", "Muli Cycles", "Müsing", "Nakita", "NANSCO", "Nhola", "Nicolai", "Nishiki", "Nöll", "Nordwind", "Norwid", "Nox", "NSU", "Olympus", "Onooka Industries|Onooka", "Opel", "Opus", "Orange Mountain Bikes", "Pakka", "Panther", "Passat", "Patria WKC", "Pearl", "Pedalpower", "Pedersen", "Pegasus", "Pepper", "Peter Green", "Peugeot", "Phänomen", "Pichlerrad", "Poison", "Porsche", "Presto", "Proceed", "Propain Bikes", "Prophete", "Puch", "PUKY", "Pulcro", "Quantec", "Quitmann", "Quix", "Rabbit", "Rabeneick", "Radius", "Radnabel", "Radon Bikes", "Reichmann Engineering", "Rennstahl", "Retovelo", "Reuber Bike", "Rheinfels", "Richi", "Riese und Müller", "Rink", "Rixe", "Roberts", "Rocket Bikes", "Roots", "Rose", "Rose Bikes", "Rotor", "Rotwild", "Rowona", "Ruff Cycles", "Ruhrwerk", "Saliko", "Schauff", "Schindelhauer", "Seidel & Naumann", "Simplon", "Sinovelo", "Smart", "Snake Bikes", "Snake Rides", "Soil", "Solid", "Staiger", "Steinbock", "Steppenwolf", "Stevens", "Stevens Bikes", "Stoewer", "Storck", "Subtil Bikes", "Superior", "Technium", "Texo", "Thorax Fahrzeugentwicklung", "Torpedo", "Tout Terrain", "Toxy", "Trenga De", "Trento", "Trimobil", "Tripendo", "Troytec", "Turnier", "Univega", "Urban E", "Utopia", "VAF Fahrradmanufaktur", "Vaterland", "Velfon", "Velo de Ville", "Veloform Media", "Velomobiles", "Velotraum", "Victoria", "Vital Bike", "Voitl", "Volt", "Voss Spezialrad", "Votec", "Votum", "VSF", "VW Volkswagen", "Walter", "Wanderer", "Wiesmann", "Wildsau", "Winora", "woom", "Work Bikes", "Wulfhorst", "X 4U", "Xyrion", "YT Industries", "Zonenschein", "Zweydingers" };
        private static readonly string[] _productTypeNames = { "Einrad", "Rennrad", "City-Bike Herren", "City-Bike Frauen", "Kinderrad", "Roller", "E-Bike", "Stahlross" };
        private readonly VeloRepository _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ISettingsContext _settingsContext;
        private readonly ICountryContext _countryContext;

        public SetupContext(VeloRepository db, UserManager<IdentityUser> userManager, ISettingsContext settingsContext, ICountryContext countryContext)
        {
            _db = db;
            _userManager = userManager;
            _settingsContext = settingsContext;
            _countryContext = countryContext;
        }

        public async Task InitializeDatabaseAsync(InitializationConfiguration config)
        {
            Contract.Requires(config != null);

            await _db.Database.EnsureCreatedAsync();

            var adminUser = new IdentityUser
            {
                Email = config.AdminUserEMail,
                UserName = config.AdminUserEMail
            };
            await _userManager.CreateAsync(adminUser, "root");

            var settings = new VeloSettings
            {
                IsInitialized = true
            };
            await _settingsContext.UpdateAsync(settings);

            await _settingsContext.UpdateAsync(new PrintSettings());

            if (config.GenerateCountries)
            {
                await _countryContext.CreateAsync(new Country
                {
                    Iso3166Alpha3Code = "AUT",
                    Name = "Österreich"
                });
                await _countryContext.CreateAsync(new Country
                {
                    Iso3166Alpha3Code = "GER",
                    Name = "Deutschland"
                });
            }

            if (config.GenerateBrands)
            {
                await GenerateBrandsAsync();
            }

            if (config.GenerateProductTypes)
            {
                await GenerateProductTypesAsync();
            }

            await _db.SaveChangesAsync();
        }

        private async Task GenerateBrandsAsync()
        {
            foreach (var brandName in _brandNames)
            {
                await _db.Brands.AddAsync(new Brand
                {
                    Name = brandName,
                    State = ObjectState.Enabled
                });
            }
        }
        private async Task GenerateProductTypesAsync()
        {
            foreach (var productTypeName in _productTypeNames)
            {
                await _db.ProductTypes.AddAsync(new ProductType
                {
                    Name = productTypeName,
                    State = ObjectState.Enabled
                });
            }
        }
    }
}
