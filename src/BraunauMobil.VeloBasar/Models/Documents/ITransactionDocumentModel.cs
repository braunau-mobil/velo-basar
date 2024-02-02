using BraunauMobil.VeloBasar.Configuration;

namespace BraunauMobil.VeloBasar.Models.Documents;

public interface ITransactionDocumentModel
{
    public string Title {get;}

    public string LocationAndDateText {get;}
    
    public string PageNumberFormat {get;}
    
    public string PoweredBy {get;}
    
    public Margins PageMargins { get; }
}
