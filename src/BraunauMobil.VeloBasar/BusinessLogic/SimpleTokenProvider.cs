using System.Globalization;
using System.Text;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class SimpleTokenProvider
    : ITokenProvider
{
    public string CreateToken(SellerEntity seller)
    {
        ArgumentNullException.ThrowIfNull(seller);

        StringBuilder token = new();
        AppendPartAsHex(token, seller.FirstName);
        token.AppendFormat(CultureInfo.InvariantCulture, "{0:X}", seller.Id);
        AppendPartAsHex(token, seller.LastName);

        return token.ToString();
    }

    private static void AppendPartAsHex(StringBuilder sb, string name)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(name);
        for (int counter = 0; counter < 2; counter++)
        {
            byte b = 0;
            if (bytes.Length > counter)
            {
                b = bytes[counter];
            }

            sb.AppendFormat(CultureInfo.InvariantCulture, "{0:X}", b);
        }
    }
}
