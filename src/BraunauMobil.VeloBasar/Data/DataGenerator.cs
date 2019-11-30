using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Data
{
    public class DataGenerator
    {
        private const string _basarLocation = "Braunau";
        private static readonly string[] _brandNames = new[] { "Additive", "Agresti", "Airstreeem", "Akkurad", "Alutech Cycles", "AnthroTech", "AT Zweirad", "Atlanta", "Ave Hyprid Bikes", "Bavaria", "Bellini", "Bergamont", "Bernds", "Bikespace", "Bionicon", "BLACK LABEL", "Boomer", "Böttcher", "Brenner Cycles", "Brothers Bikes", "Bulls", "C 14", "Campus", "Canyon Bicycles", "Carver", "Ccr", "Centurion", "Checker Pig", "Cheetah", "Cinelli Bikes", "Cito", "Cobra", "Cocoon", "Contoura", "Conway", "Corona", "Corratec", "Cresta", "Cube", "Cucuma", "Cyclecraft", "Cyclewolf", "Cyclomanix", "Dalliegerad", "Dancelli", "Definder Cycle Manufaktur", "Diamant", "Dragonfly", "Draisin", "Drössiger", "Druxs", "Düll", "Duo Bike", "Dürkopp", "Dynamics", "Easy Rider", "Egon Rahe", "Electrolyte", "Elfa", "Elom", "EMANON", "Endorfin", "Enik", "Epple", "Express", "Faggin", "Falke", "Falkenjagd", "Falter", "Fatmodul", "Fischer", "Fixie Inc.", "Fleiner", "Flitz Bike", "Flux", "Focus", "Frank Bikes", "Frischauf", "Frosch Rad", "Fxx Cycles", "Gaastra", "Garage 271", "Geier", "German A", "Germans Cycles", "Ghost", "Gigant", "Gmp", "Gobax", "Gold-Rad", "Göricke", "Grace", "Gudereit", "Guylaine", "Haibike", "Hammonia", "Handybike", "Hase Bikes", "Hawk", "Hercules", "Herkelmann", "Hot Chili", "HP Velotechnik", "Hrinkow", "Idworx", "Inchuan", "Indienrad", "Jaguar", "Jakel", "Jan Ullrich Bikes", "Juchem", "Jungherz", "Kalkhoff", "Kania", "Katarga", "Kemper", "Kenhill", "Kettler", "KHEbikes", "Koch Bikes", "Kokua", "Kondor", "Kotter", "Krabo", "Kreidler", "KTM", "Lakes", "Langenberg", "Last", "Leafcycles", "Lehmkuhl", "Leiba", "Liing", "Liteville", "Mars", "Marschall", "Maxcycles", "Maxx", "Mercedes-Benz", "Merida Bikes", "Miele", "MIFA", "Mini", "Mondello", "Morrison", "Möve", "Muli Cycles", "Müsing", "Nakita", "NANSCO", "Nhola", "Nicolai", "Nishiki", "Nöll", "Nordwind", "Norwid", "Nox", "NSU", "Olympus", "Onooka Industries|Onooka", "Opel", "Opus", "Orange Mountain Bikes", "Pakka", "Panther", "Passat", "Patria WKC", "Pearl", "Pedalpower", "Pedersen", "Pegasus", "Pepper", "Peter Green", "Peugeot", "Phänomen", "Pichlerrad", "Poison", "Porsche", "Presto", "Proceed", "Propain Bikes", "Prophete", "Puch", "PUKY", "Pulcro", "Quantec", "Quitmann", "Quix", "Rabbit", "Rabeneick", "Radius", "Radnabel", "Radon Bikes", "Reichmann Engineering", "Rennstahl", "Retovelo", "Reuber Bike", "Rheinfels", "Richi", "Riese und Müller", "Rink", "Rixe", "Roberts", "Rocket Bikes", "Roots", "Rose", "Rose Bikes", "Rotor", "Rotwild", "Rowona", "Ruff Cycles", "Ruhrwerk", "Saliko", "Schauff", "Schindelhauer", "Seidel & Naumann", "Simplon", "Sinovelo", "Smart", "Snake Bikes", "Snake Rides", "Soil", "Solid", "Staiger", "Steinbock", "Steppenwolf", "Stevens", "Stevens Bikes", "Stoewer", "Storck", "Subtil Bikes", "Superior", "Technium", "Texo", "Thorax Fahrzeugentwicklung", "Torpedo", "Tout Terrain", "Toxy", "Trenga De", "Trento", "Trimobil", "Tripendo", "Troytec", "Turnier", "Univega", "Urban E", "Utopia", "VAF Fahrradmanufaktur", "Vaterland", "Velfon", "Velo de Ville", "Veloform Media", "Velomobiles", "Velotraum", "Victoria", "Vital Bike", "Voitl", "Volt", "Voss Spezialrad", "Votec", "Votum", "VSF", "VW Volkswagen", "Walter", "Wanderer", "Wiesmann", "Wildsau", "Winora", "woom", "Work Bikes", "Wulfhorst", "X 4U", "Xyrion", "YT Industries", "Zonenschein", "Zweydingers" };
        private static readonly string[] _colors = new[] { "achatgrau", "agavengrün", "ahornrot", "alaskagrau", "alpinaweiss", "altrosa", "altrosafarben", "altrosafarbig", "aluminiumgrau", "amarant", "amarantfarben", "amarantfarbig", "amarantrot", "amazonasgrün", "ameisenrot", "ananasgelb", "anthrazit", "anthrazitfarben", "anthrazitfarbig", "anthrazitgrau", "antikgelb", "antikrosé", "antikrot", "antiktürkis", "antikweiss", "apfelgrün", "apricot", "aquamarin", "aquamarinblau", "aquamarinfarben", "asphaltgrau", "atlantikblau", "atlantisblau", "aubergine", "auberginefarben", "auberginefarbig", "azorenblau", "azurblau", "ballettrosa", "bananengelb", "basaltgrau", "beige", "beigefarben", "beigefarbig", "beigerot", "betongrau", "birkengrün", "blattgrün", "blau", "bläulich", "bleigrau", "bluescreenblau", "blütenweiß", "blutorange", "blutrot", "bordeauxrot", "braun", "braunbeige", "bräunlich", "braunrot", "brillantblau", "brombeerrot", "bronze", "burgunderrot", "calypsorot", "cappuccino", "capriblau", "carrerarot", "cayennerot", "chamois", "cherry", "chromgelb", "creepergrün", "cremeweiß", "currygelb", "cyan", "dahliengelb", "delphinblau", "diamantblau", "diamantgrün", "diamantrot", "diamantschwarz", "dunkelblau", "dunkelbraun", "dunkelgelb", "dunkelgrau", "dunkelgrün", "dunkellila", "dunkelrosa", "dunkelrot", "eisblau", "eisengrau", "elefantengrau", "elfenbein", "enzianblau", "erdbeerrot", "erikarot", "espressobraun", "estorilblau", "farblos", "farngrün", "fehengrau", "fenstergrau", "ferrarirot", "feuerrot", "flamingorosa", "flamingorot", "flaschengrün", "fleischfarben", "fleischfarbig", "fliederfarben", "französischgrün", "froschgrün", "frühlingsgrün", "fuchsienrot", "gelb", "gelbgrün", "gelblich", "gelborange", "geraniumrot", "giftgrün", "gilblich", "ginstergelb", "gletscherblau", "gold", "goldbraun", "golden", "goldfarben", "goldgelb", "granitgrau", "graphitgrau", "grasgrün", "grau", "graubeige", "gräulich", "grün", "grünbeige", "grünlich", "gülden", "haselnussbraun", "hautfarben", "heidelbeerblau", "hellblau", "hellbraun", "hellelfenbein", "hellgelb", "hellgrau", "hellgrün", "hellrosa", "hellrot", "hellrotorange", "himbeerrot", "himmelblau", "honiggelb", "hummerrot", "indianerrot", "indigo", "indigoblau", "indigorot", "indischgelb", "indischrot", "infernorot", "inkarnat", "italienischrot", "jadegrün", "jägergrün", "jerichorot", "johannisbeerrot", "juwelierrot", "kackbraun", "kadmiumgelb", "kaffeebraun", "kamillengelb", "kanariengelb", "karamellbraun", "kardinalrot", "karibikblau", "karminrot", "karminrot", "kastanienbraun", "khakigrau", "khakigrün", "kieferngrün", "kieselgrau", "kirschrot", "kiwigrün", "kobaltblau", "kobaltgrün", "königsblau", "korallenrosa", "korallenrot", "kornblumenblau", "koronagelb", "kosmosschwarz", "kupferrot", "lachsfarben", "lachsorange", "lachsrosa", "lachsrot", "laubgrün", "lavendelblau", "lehmbraun", "lemongrün", "leuchtgelb", "leuchthellorange", "leuchthellrot", "leuchtorange", "leuchtrot", "lichtblau", "lila", "lilafarben", "limonengrün", "lindgrün", "lotusrot", "magenta", "magentafarben", "magnolienrosa", "magnolienrot", "mahagonibraun", "mahagonirot", "maigrün", "maisgelb", "mandelbraun", "marineblau", "marmor", "mauritiusblau", "mausgrau", "meeresgrün", "melonengelb", "mintgrün", "mohnrot", "moosgrau", "moosgrün", "nachtblau", "narzissengelb", "neapelgelb", "neonblau", "neongelb", "neongrün", "neonlila", "neonorange", "neonpink", "neonrosa", "neonrot", "neontürkis", "neonviolett", "nougatbraun", "nussbraun", "ocker", "ockerbraun", "ockergelb", "ockerrot", "olivgelb", "olivgrau", "olivgrün", "orange", "orangefarben", "orangegelb", "orangerot", "orchidee", "orientrot", "oxidrot", "ozeanblau", "papageirot", "paprikarot", "papyrus", "papyrusweiß", "Pariser Blau", "pastell", "pastellgelb", "pastellorange", "patinagrün", "pazifikblau", "pechrabenschwarz", "perlbeige", "perlgold", "perlgrau", "perlorange", "perlrosa", "perlrubinrot", "perlweiß", "permamentrosa", "permamentrot", "persischrot", "petrol", "pfirsichrot", "pflaumenblau", "pigmentgrün", "pink", "pinkfarben", "pinkfarbig", "pistaziengrün", "platingrau", "plazentarot", "porzellanblau", "preußischblau", "pumucklrot", "purpur", "purpurfarben", "purpurlila", "purpurrot", "quarzgrau", "quittegelb", "quittengelb", "rapsgelb", "rehbraun", "reinorange", "reinrot", "resedagrün", "rhabarberrot", "rindenbraun", "ringelblumengelb", "rosa", "rosafarben", "rosé", "roséfarben", "rosenrosa", "rosenrot", "rosig", "rostbraun", "rostrot", "rot", "rotorange", "royalblau", "rubinrot", "safrangelb", "safranrot", "saharagelb", "sandgelb", "sandgelb", "sandsteinrot", "saphirblau", "scharlachrot", "schiefergrau", "schilfgrün", "schneeweiß", "schokoladenbraun", "schwarz", "schwarzbraun", "schwärzlich", "schwarzrot", "schwefelgelb", "seegrün", "seidengrau", "sepiabraun", "siena", "sienabraun", "signalgelb", "signalorange", "signalrot", "silber", "silberfarben", "silbergrau", "silbrig", "smalteblau", "smaragdgrün", "sonnengelb", "stahlblau", "staubgrau", "steingrau", "südseeblau", "tabakbraun", "tannengrün", "taubenblau", "terrabraun", "terracotta", "tieforange", "tiefschwarz", "tintenblau", "tintenrot", "tintenschwarz", "titangrau", "tizianrot", "tomatenrot", "torfbraun", "tumblau", "türkis", "türkischrot", "türkisfarben", "ultramarinblau", "umbra", "universalblau", "va­nil­le", "veilchenblau", "venezianischrot", "verkehrsblau", "verkehrsgelb", "verkehrsgrün", "verkehrsorange", "verkehrsrot", "violett", "violettfarben", "walnussbraun", "wasserblau", "weinrot", "weiß", "weißlich", "wiesengrün", "wüstenrot", "xenon", "xili", "zartgrün", "zartrosa", "zeltgrau", "zementgrau", "ziegelrot", "zimtbraun", "zinkgelb", "zinnoberrot", "zitronengelb", "zitrusgelb", "zyan" };
        private static readonly string[] _tireSizes = new[] { "12", "14", "16", "17", "18", "20", "22", "24", "25", "26", "27", "29" };
        private static readonly string[] _productTypeNames = new string[] { "Einrad", "Rennrad", "City-Bike Herren", "City-Bike Frauen", "Kinderrad", "Roller", "E-Bike", "Stahlross" };
        private static readonly string[] _firstNames = new string[] { "Adaldrida", "Adalgrim", "Adamanta", "Adelard", "Adrahil", "Aegnor", "Aerandir", "Aerin", "Aghan", "Ailinel", "Ainairos", "Alatar", "Aldamir", "Aldor", "Algund", "Almarian", "Almiel", "Alter", "Aluin", "Amandil", "Amaranth", "Amarië", "Amdír", "Amillo", "Amlach", "Amlaith", "Amnon", "Amras", "Amrod", "Amroth", "Anardil", "Anborn", "Ancalagon", "Andróg", "Andweis", "Angamaite", "Angbor", "Angelica", "Angelimir", "Angrim", "Angrod", "Annael", "Ansen", "Anárion", "Ar-Adûnakhôr", "Ar-Gimilzôr", "Ar-Pharazôn", "Ar-Sakalthôr", "Ar-Zimrathôn", "Arador", "Araglas", "Aragorn", "Aragost", "Arahad", "Arahael", "Aranarth", "Arantar", "Aranuir", "Aranwe", "Araphant", "Araphor", "Arassuil", "Aratan", "Arathorn", "Araval", "Aravir", "Aravorn", "Arciryas", "Aredhel", "Argeleb", "Argonui", "Arien", "Arminas", "Arod", "Arroch", "Artamir", "Arthad", "Arvedui", "Arvegil", "Arveleg", "Arwen", "Asfaloth", "Asgon", "Asphodel", "Atanatar", "Aule", "Azaghâl", "Azog", "Bain", "Balbo", "Baldor", "Balin", "Bandobras", "Barach", "Baragund", "Barahir", "Baran", "Baranor", "Bard", "Bauer", "Baumbart", "Belba", "Belecthor", "Beleg", "Belegorn", "Belegund", "Beleth", "Bell", "Belladonna", "Beor", "Beorn", "Bereg", "Beregar", "Beregond", "Beren", "Bergil", "Berilac", "Bert", "Berylla", "Berúthiel", "Bifur", "Bilbo", "Bill", "Bingo", "Bladorthin", "Blanco", "Bodo", "Bofur", "Bogenman", "Bolg", "Bombur", "Borin", "Borlach", "Borlad", "Boromir", "Boron", "Borondir", "Borthand", "Brand", "Brandir", "Brego", "Bregolas", "Bregor", "Brodda", "Bruithwir", "Brytta", "Bucca", "Buchenbein", "Bungo", "Bór", "Calimehtar", "Calimmacil", "Calmacil", "Camellia", "Caranthir", "Carcharoth", "Carl", "Castamir", "Celandine", "Celeborn", "Celebrimbor", "Celebrindor", "Celebrían", "Celegorm", "Celepharn", "Cemendur", "Ceorl", "Chica", "Cirion", "Ciryandil", "Ciryatur", "Ciryon", "Curufin", "Círdan", "Daeron", "Dagnir", "Dairuin", "Damrod", "Danuin", "Daurin", "Denethor", "Derufin", "Dervorin", "Dickes", "Dietmute", "Dinodas", "Dior", "Doderic", "Dodinas", "Donnamira", "Dora", "Dori", "Dorlas", "Draugluin", "Drogo", "Dudo", "Duilin", "Duinhir", "Durin", "Dwalin", "Dáin", "Déagol", "Déor", "Déorwine", "Dírhael", "Dírhavel", "Dís", "Dúnhere", "Earendil", "Earendur", "Earnil", "Earnur", "Earwen", "Ecthelion", "Edrahil", "Egalmoth", "Eilinel", "Elanor", "Elatan", "Eldacar", "Eldarion", "Elemmakil", "Elemmíre", "Elendil", "Elendur", "Elenwe", "Elfhelm", "Elfhild", "Elfwine", "Elladan", "Elmir", "Elmo", "Elrohir", "Elrond", "Elros", "Eltas", "Eluréd", "Elurín", "Elwe", "Elwing", "Emeldir", "Enel", "Enelyë", "Enerdhil", "Eoh", "Eol", "Eonwe", "Eorl", "Eradan", "Erchirion", "Erellont", "Erendis", "Erestor", "Eriol", "Erkenbrand", "Erling", "Ermon", "Eru", "Esmeralda", "Este", "Estella", "Estelmo", "Everard", "Falathar", "Falco", "Familie", "Fang", "Fankil", "Fanuin", "Faramir", "Farin", "Fastolph", "Fastred", "Feanor", "Felaróf", "Fengel", "Ferdibrand", "Ferdinand", "Ferumbras", "Feuerfuß", "Figwit", "Filibert", "Fimbrethil", "Finarfin", "Findegil", "Finduilas", "Finglas", "Fingolfin", "Fingon", "Finrod", "Finwe", "Fladrif", "Flambard", "Flinkbaum", "Flói", "Folca", "Folcred", "Folcwine", "Folko", "Forlong", "Forthwini", "Fortinbras", "Forweg", "Freca", "Freda", "Fredegar", "Frerin", "Frodo", "Frumgar", "Frár", "Fréa", "Fréaláf", "Fréawine", "Frór", "Fuinur", "Fundin", "Fíli", "Galadhon", "Galador", "Galadriel", "Galathil", "Galdor", "Gamil", "Gamling", "Gandalf", "Gelmir", "Gerstenmann", "Gethron", "Ghân-buri-Ghân", "Gil-galad", "Gildor", "Gilfanon", "Gilmith", "Gilraen", "Gimilkhâd", "Gimilzagar", "Gimli", "Glaurung", "Glirhuin", "Glorfindel", "Gléowine", "Glóin", "Glóredhel", "Golasgil", "Goldbeere", "Goldblume", "Goldlöckchen", "Goldwine", "Gollum", "Gorbadoc", "Gorbag", "Gorbulas", "Gorhendad", "Gorlim", "Gormadoc", "Gothmog", "Gram", "Greif", "Griffo", "Grimbeorn", "Grimbold", "Grinnah", "Grischnákh", "Grithnir", "Gríma", "Gróin", "Grór", "Guilin", "Gundabald", "Gundor", "Guthláf", "Gwaihir", "Gwindor", "Gálmód", "Gárulf", "Hador", "Halbarad", "Haldad", "Haldan", "Haldar", "Haldir", "Haleth", "Halfast", "Halfred", "Hallacar", "Hallas", "Hallatan", "Halmir", "Hamfast", "Hamsen", "Handir", "Hanna", "Harding", "Hareth", "Hasufel", "Hathaldir", "Hathol", "Hatholdir", "Heiderose", "Heinrich", "Helm", "Henderch", "Hending", "Herefara", "Herion", "Herumor", "Hild", "Hilda", "Hildibrand", "Hildifons", "Hildigard", "Hildigrim", "Hinz", "Hirgon", "Hirluin", "Hob", "Hobsen", "Holfast", "Holman", "Horn", "Huan", "Hugo", "Humpel", "Hunthor", "Huor", "Hyarmendacil", "Háma", "Höhlenmann", "Húrin", "Hütter", "Hüttner", "Ibun", "Idril", "Ilberic", "Ilmare", "Imin", "Iminyë", "Imlach", "Imrahil", "Imrazôr", "Indis", "Indor", "Ingil", "Ingold", "Ingwe", "Inzilbêth", "Ioreth", "Iorlas", "Irolas", "Isegrim", "Isembard", "Isembold", "Isengar", "Isildur", "Isilme", "Isilmo", "Isumbras", "Ithilbor", "Kankra", "Khamûl", "Khîm", "Kunz", "Kíli", "Labkraut", "Lagduf", "Lalaith", "Landroval", "Langhöhlen", "Langon", "Largo", "Larnach", "Laura", "Legolas", "Lenwe", "Lily", "Linda", "Lindir", "Lindo", "Lindórië", "Lobelia", "Longo", "Lorgan", "Lotho", "Lothíriel", "Lugdusch", "Lurtz", "Lutz", "Léod", "Lóni", "Lórien", "Löffelohr", "Lúthien", "Mablon", "Mablung", "Madoc", "Madril", "Maedhros", "Maeglin", "Maglor", "Magor", "Magsame", "Mahtan", "Maie", "Makar", "Malach", "Malantur", "Malbeth", "Mallor", "Malte", "Malva", "Malvegil", "Mandos", "Manwe", "Marach", "Marcho", "Mardil", "Margerite", "Marhari", "Marhwini", "Marmadas", "Marmadoc", "Marroc", "Mauhúr", "Melian", "Melilot", "Melkor", "Menegilda", "Meneldil", "Meneldor", "Meriadoc", "Meril-i-Turinqi", "Merimac", "Merimas", "Merry", "Meássë", "Milo", "Mimosa", "Minardil", "Minastan", "Minohtar", "Minto", "Minze", "Mirabella", "Mithrellas", "Moro", "Morwen", "Mosco", "Mungo", "Muzgasch", "Myrte", "Míriel", "Mîm", "Nahar", "Narmacil", "Narvi", "Nelke", "Nellas", "Nerdanel", "Nessa", "Nieliqui", "Nienna", "Nienor", "Nimloth", "Nimrodel", "Nori", "Nornore", "Nuin", "Náin", "Náli", "Númendil", "Núneth", "Odo", "Odovacar", "Ohtar", "Olo", "Olwe", "Ondoher", "Orchaldor", "Orgulas", "Ori", "Orleg", "Orodreth", "Orome", "Oropher", "Orophin", "Osse", "Ostoher", "Otho", "Paladin", "Pallando", "Pelendur", "Peregrin", "Perle", "Petunia", "Pimpernel", "Pippin", "Platschfuß", "Polo", "Ponto", "Porto", "Posco", "Primula", "Prisca", "Päonie", "Radagast", "Radbug", "Radhruin", "Ragnir", "Ragnor", "Ranuin", "Reginard", "Robi", "Rochallor", "Roheryn", "Roland", "Rorimac", "Rosa", "Rosamunde", "Rose", "Rowan", "Rubinie", "Rudigar", "Rudolf", "Rufus", "Rían", "Rómendacil", "Rúmil", "Sadoc", "Sador", "Saeros", "Salmar", "Salvia", "Samweis", "Sancho", "Sandheber", "Saradas", "Saradoc", "Saruman", "Sauron", "Scatha", "Schagrat", "Schattenfell", "Schneemähne", "Schnüffelschnauz", "Schönkind", "Seredic", "Sigismund", "Silmarien", "Siriondil", "Smaug", "Soronto", "Streicher", "Stybba", "Tanta", "Tar-Alcarin", "Tar-Aldarion", "Tar-Amandil", "Tar-Ancalime", "Tar-Ancalimon", "Tar-Anducal", "Tar-Anárion", "Tar-Ardamin", "Tar-Atanamir", "Tar-Calmacil", "Tar-Ciryatan", "Tar-Elendil", "Tar-Meneldur", "Tar-Minastir", "Tar-Míriel", "Tar-Palantir", "Tar-Súrion", "Tar-Telemmaite", "Tar-Telperien", "Tar-Vanimelde", "Tarannon", "Tarcil", "Tarciryan", "Tareg", "Targon", "Tarondor", "Tata", "Tatië", "Tauriel", "Telchar", "Telemnar", "Telimektar", "Telumehtar", "Tevildo", "Thengel", "Thorin", "Thorondir", "Thorondor", "Thranduil", "Thráin", "Thrór", "Thuringwethil", "Théoden", "Théodred", "Théodwyn", "Tilion", "Timm", "Tinfang", "Tobold", "Togo", "Tolman", "Tom", "Tulkas", "Tuor", "Turambar", "Turgon", "Túrin", "Túvo", "Ufthak", "Uglúk", "Uin", "Uinen", "Ulbandi", "Ulbar", "Uldor", "Ulfang", "Ulfast", "Ulmo", "Ulrad", "Ulwarth", "Ungoliant", "Uole", "Urthel", "Vaire", "Valacar", "Valandil", "Valandur", "Varda", "Vardamir", "Veantur", "Vidugavia", "Vidumavi", "Viola", "Vorondil", "Voronwe", "Vána", "Walda", "Wedelschwanz", "Weisman", "Wilibald", "Windfola", "Winzigherz", "Wolf", "Wulf", "Wídfara", "Yavanna", "Zamîn", "Zimrahin", "Zwiefuß", "Éofor", "Éomer", "Éomund", "Éothain", "Éowyn", "Îbal", "Óin" };
        private static readonly string[] _cities = new string[] { "Agar", "Aldburg", "Alqualonde", "Andúnië", "Annúminas", "Archet", "Armenelos", "Avallóne", "Balgfurt", "Belegost", "Bockenburg", "Bree", "Brithombar", "Bruchtal", "Calembel", "Caras Galadhon", "Carn Dûm", "Dachsbauten", "Dol Amroth", "Dornbühl", "Dwollingen", "Edhellond", "Edoras", "Eglarest", "Eldalonde", "Esgaroth", "Finkenschlupf", "Forlond", "Fornost", "Froschmoorstetten", "Gamwich", "Gondolin", "Graue Anfurten", "Grimslade", "Grünholm", "Hafergut", "Hagsend", "Harlond (Lindon)", "Hobbingen", "Hochborn", "Häfen des Sirion", "Khazad-dûm", "Kortirion", "Krickloch", "Langcleeve", "Langgrund", "Linhir", "Lützelbinge", "Menegroth", "Michelbinge", "Minas Anor", "Minas Ithil", "Minas Morgul", "Minas Tirith (Gondor)", "Máhanaxar", "Nadelhohl", "Nargothrond", "Neuburg", "Neuhausen", "Nindamos", "Nogrod", "Oberbühl", "Ondosto", "Osgiliath", "Ost-in-Edhil", "Pelargir", "Pen-arduin", "Reepfeld", "Rhosgobel", "Rohrholm", "Rómenna", "Schlucht", "Schären", "Stadel", "Stadt der Corsaren", "Steinbruch", "Steinbüttel", "Steingrube", "Stock", "Tarnost", "Tavrobel", "Thal", "Tharbad", "Thorins Hallen", "Tiefenhain", "Tirion", "Tuckbergen", "Tukhang", "Udul", "Unterbühl", "Unterharg", "Untertürmen", "Valmar", "Vinyamar", "Waldhof", "Wasserau", "Wegscheid", "Weidengrund", "Weißbrunn", "Weißfurchen" };
        private static readonly string[] _streets = new string[] { "Abraham-a-Sancta-Clara-Gasse", "Akademiestraße", "Albertinaplatz", "Alte Walfischgasse", "Am Gestade", "Am Hof", "An der Hülben", "Annagasse", "Auerspergstraße", "Augustinerbastei", "Augustinerstraße", "Auwinkel", "Babenbergerstraße", "Ballgasse", "Ballhausplatz", "Bankgasse", "Barbaragasse", "Bartensteingasse", "Bauernmarkt", "Beethovenplatz", "Bellariastraße", "Biberstraße", "Blumenstockgasse", "Bognergasse", "Brandstätte", "Bruno-Kreisky-Gasse", "Bräunerstraße", "Burgring", "Bäckerstraße", "Börsegasse", "Börseplatz", "Bösendorferstraße", "Canovagasse", "Christinengasse", "Churhausgasse", "Cobdengasse", "Coburgbastei", "Concordiaplatz", "Desider-Friedmann-Platz", "Deutschmeisterplatz", "Doblhoffgasse", "Domgasse", "Dominikanerbastei", "Donnergasse", "Dorotheergasse", "Dr.-Ignaz-Seipel-Platz", "Dr.-Karl-Lueger-Platz", "Dr.-Karl-Renner-Ring", "Drachengasse", "Drahtgasse", "Dumbastraße", "Ebendorferstraße", "Elisabethstraße", "Ertlgasse", "Eschenbachgasse", "Essiggasse", "Fahnengasse", "Falkestraße", "Felderstraße", "Fichtegasse", "Fischerstiege", "Fischhof", "Fleischmarkt", "Franz-Josefs-Kai", "Franziskanerplatz", "Freda-Meissner-Blau-Promenade", "Freisingergasse", "Freyung", "Friedrich-Schmidt-Platz", "Friedrichstraße", "Fritz-Wotruba-Promenade", "Färbergasse", "Führichgasse", "Fütterergasse", "Gartenbaupromenade", "Gauermanngasse", "Georg-Coch-Platz", "Getreidemarkt", "Gluckgasse", "Goethegasse", "Goldschmiedgasse", "Gonzagagasse", "Gottfried-von-Einem-Platz", "Graben", "Grashofgasse", "Griechengasse", "Grillparzerstraße", "Grünangergasse", "Gölsdorfgasse", "Göttweihergasse", "Haarhof", "Habsburgergasse", "Hafnersteig", "Hansenstraße", "Hanuschgasse", "Hegelgasse", "Heidenschuss", "Heinrichsgasse", "Heldenplatz", "Helferstorferstraße", "Helmut-Zilk-Platz", "Herbert-von-Karajan-Platz", "Herrengasse", "Heßgasse", "Himmelpfortgasse", "Hohenstaufengasse", "Hoher Markt", "In der Burg", "Irisgasse", "Jakobergasse", "Jasomirgottstraße", "Jerusalemstiege", "Jesuitengasse", "Johannesgasse", "Jordangasse", "Josef-Meinrad-Platz", "Josefsplatz", "Judengasse", "Judenplatz", "Julius-Raab-Platz", "Jungferngasse", "Kantgasse", "Karlsplatz", "Kleeblattgasse", "Kohlmarkt", "Kramergasse", "Krugerstraße", "Kumpfgasse", "Kupferschmiedgasse", "Kurrentgasse", "Kärntner Durchgang", "Kärntner Ring", "Kärntner Straße", "Köllnerhofgasse", "Körblergasse", "Kühfußgasse", "Landesgerichtsstraße", "Landhausgasse", "Landskrongasse", "Laurenzerberg", "Ledererhof", "Leopold-Figl-Gasse", "Leopold-Gratz-Platz", "Lichtenfelsgasse", "Lichtensteg", "Liebenberggasse", "Liebiggasse", "Liliengasse", "Lobkowitzplatz", "Lothringerstraße", "Lugeck", "Löwelstraße", "Mahlerstraße", "Makartgasse", "Marc-Aurel-Straße", "Marco-d’Aviano-Gasse", "Maria-Theresien-Platz", "Maria-Theresien-Straße", "Marienstiege", "Max-Weiler-Platz", "Maysedergasse", "Metastasiogasse", "Michaelerplatz", "Milchgasse", "Minoritenplatz", "Morzinplatz", "Museumsplatz", "Museumstraße", "Musikvereinsplatz", "Mölker Bastei", "Mölker Steig", "Naglergasse", "Neubadgasse", "Neuer Markt", "Neutorgasse", "Nibelungengasse", "Nikolaigasse", "Operngasse", "Opernring", "Oppolzergasse", "Oskar-Kokoschka-Platz", "Parisergasse", "Parkring", "Passauer Platz", "Pestalozzigasse", "Petersplatz", "Petrarcagasse", "Philharmonikerstraße", "Plankengasse", "Postgasse", "Predigergasse", "Rabensteig", "Rathausplatz", "Rathausstraße", "Rauhensteingasse", "Rechte Wienzeile", "Reichsratsstraße", "Reischachstraße", "Reitschulgasse", "Renngasse", "Riemergasse", "Robert-Stolz-Platz", "Rockhgasse", "Rosenbursenstraße", "Rosengasse", "Rotenturmstraße", "Rotgasse", "Rudolfsplatz", "Ruprechtsplatz", "Ruprechtsstiege", "Salvatorgasse", "Salzgasse", "Salzgries", "Salztorgasse", "Schallautzerstraße", "Schauflergasse", "Schellinggasse", "Schenkenstraße", "Schillerplatz", "Schmerlingplatz", "Schottenbastei", "Schottengasse", "Schottenring", "Schottentor", "Schreyvogelgasse", "Schubertring", "Schulerstraße", "Schulhof", "Schultergasse", "Schwarzenbergplatz", "Schwarzenbergstraße", "Schwedenplatz", "Schwertgasse", "Schönlaterngasse", "Seilergasse", "Seilerstätte", "Seitenstettengasse", "Seitzergasse", "Singerstraße", "Sonnenfelsgasse", "Spiegelgasse", "Stadiongasse", "Stallburggasse", "Steindlgasse", "Stephansplatz", "Sterngasse", "Steyrerhof", "Stock-im-Eisen-Platz", "Stoß im Himmel", "Strauchgasse", "Strobelgasse", "Stubenbastei", "Stubenring", "Tegetthoffstraße", "Teinfaltstraße", "Theodor-Herzl-Platz", "Theodor-Herzl-Stiege", "Tiefer Graben", "Trattnerhof", "Tuchlauben", "Tuchlaubenhof", "Universitätsring", "Universitätsstraße", "Uraniastraße", "Volksgartenstraße", "Vorlaufstraße", "Walfischgasse", "Wallnerstraße", "Weihburggasse", "Weiskirchnerstraße", "Werdertorgasse", "Wiesingerstraße", "Wildpretmarkt", "Windhaaggasse", "Wipplingerstraße", "Wolfengasse", "Wolfgang-Schmitz-Promenade", "Wollzeile", "Wächtergasse", "Zedlitzgasse", "Zelinkagasse" };

        private readonly Random _rand = new Random();
        private readonly VeloBasarContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DataGeneratorConfiguration _config;
        private Brand[] _brands;
        private Country[] _countries;
        private ProductType[] _productTypes;

        public DataGenerator(VeloBasarContext context, UserManager<IdentityUser> userManager , DataGeneratorConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
        }

        public async Task GenerateAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();

            await _context.InitializeDatabase(_userManager, _config);
            var settings = _context.GetVeloSettings();

            await CreateCountriesAsync();
            await CreateBrandsAsync();
            await CreateProductTypesAsync();

            for (var basarNumber = 1; basarNumber <= _config.BasarCount; basarNumber++)
            {
                var basar = await CreateBasarAsync(_config.FirstBasarDate.AddYears(basarNumber - 1), $"{basarNumber}. Fahrradbasar");
                if (settings.ActiveBasarId == null)
                {
                    settings.ActiveBasarId = basar.Id;
                    await _context.SetBasarSettingsAsync(settings);
                }
            }
        }

        private async Task CreateCountriesAsync()
        {
            await _context.Country.AddAsync(new Country
            {
                Iso3166Alpha3Code = "AUT",
                Name = "Österreich"
            });
            await _context.Country.AddAsync(new Country
            {
                Iso3166Alpha3Code = "GER",
                Name = "Deutschland"
            });
            await _context.SaveChangesAsync();
            _countries = await _context.Country.ToArrayAsync();
        }

        private async Task CreateBrandsAsync()
        {
            foreach (var brandName in _brandNames)
            {
                await _context.Brand.AddAsync(new Brand
                {
                    Name = brandName,
                    State = ObjectState.Enabled
                });
            }
            await _context.SaveChangesAsync();
            _brands = await _context.Brand.ToArrayAsync();
        }

        private async Task CreateProductTypesAsync()
        {
            foreach (var productTypeName in _productTypeNames)
            {
                await _context.ProductTypes.AddAsync(new ProductType
                {
                    Name = productTypeName,
                    State = ObjectState.Enabled
                });
            }
            await _context.SaveChangesAsync();
            _productTypes = await _context.ProductTypes.ToArrayAsync();
        }

        private async Task<Basar> CreateBasarAsync(DateTime date, string name)
        {
            var basar = await _context.CreateNewBasarAsync(date, name, _basarLocation, 0.9m, 0.0m, 0.0m);
            await _context.SaveChangesAsync();

            var sellerCount = _rand.Next(_config.MinSellers, _config.MaxSellers);
            for (var sellerNumber = 1; sellerNumber <= sellerCount; sellerNumber++)
            {
                await CreateSellerWithAcceptancesAsync(basar, sellerNumber);
            }

            return basar;
        }

        private async Task CreateSellerWithAcceptancesAsync(Basar basar, int sellerNumber)
        {
            var seller = new Seller();
            SetSellerName(seller);
            SetSellerLocation(seller);
            await _context.Seller.AddAsync(seller);
            await _context.SaveChangesAsync();

            var acceptancePerCustomerCount = _rand.Next(_config.MinAcceptancesPerSeller, _config.MaxAcceptancesPerSeller);
            while (acceptancePerCustomerCount > 0)
            {
                await CreateAcceptanceAsync(basar, seller);
                acceptancePerCustomerCount--;
            }

            await _context.CreateLabelsForSellerAsync(basar, seller.Id);
        }

        private async Task CreateAcceptanceAsync(Basar basar, Seller seller)
        {
            var productCount = NextProductCount();
            var products = new List<Product>();
            for (var count = 0; count < productCount; count++)
            {
                products.Add(CreateProduct());
            }
            await _context.AcceptProductsAsync(basar, seller.Id, new PrintSettings(), products);
        }

        private Product CreateProduct()
        {
            return new Product
            {
                Brand = NextBrand(),
                Color = NextColor(),
                Description = $"Beschreibung für Produkt",
                FrameNumber = NextFrameNumber(),
                Price = NextPrice(),
                StorageState = StorageState.Available,
                ValueState = ValueState.NotSettled,
                TireSize = NextTireSize(),
                Type = NextProductType()
            };
        }

        private void SetSellerName(Seller seller)
        {
            seller.FirstName = _firstNames.TakeRandom(_rand);
            seller.LastName = _firstNames.TakeRandom(_rand);
        }

        private void SetSellerLocation(Seller seller)
        {
            seller.Country = _countries.TakeRandom(_rand);
            seller.City = _cities.TakeRandom(_rand);
            seller.Street = $"{_streets.TakeRandom(_rand)} {_rand.Next(1, 50)}";
            seller.ZIP = $"{_rand.Next(1,9)}{_rand.Next(1, 9)}{_rand.Next(1, 9)}{_rand.Next(1, 9)}";
        }
        private int NextProductCount()
        {
            //  wir wollen keine annahmen mit 0 produkten
            return Math.Max((int)_rand.NextGaussian(_config.MeanProductsPerSeller, _config.StdDevProductsPerSeller), 1);
        }

        private Brand NextBrand()
        {
            return _brands.TakeRandom(_rand);
        }

        private string NextColor()
        {
            return _colors.TakeRandom(_rand);
        }

        private string NextFrameNumber()
        {
            return Guid.NewGuid().ToString();
        }

        private decimal NextPrice()
        {
            return Math.Round((decimal)_rand.NextGaussian((double)_config.MeanPrice, (double)_config.StdDevPrice), 2);
        }

        private string NextTireSize()
        {
            return _tireSizes.TakeRandom(_rand);
        }

        private ProductType NextProductType()
        {
            return _productTypes.TakeRandom(_rand);
        }
    }
}
