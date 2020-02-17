using BraunauMobil.VeloBasar.Models;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;

namespace BraunauMobil.VeloBasar.Logic
{
    public class SimpleTokenProvider : ITokenProvider
    {
        public string CreateToken(Seller seller)
        {
            Contract.Requires(seller != null);

            var token = new StringBuilder();

            token.Append("VB");
            AppendPartAsHex(token, seller.FirstName);
            token.AppendFormat(CultureInfo.InvariantCulture, "{0:X}", seller.Id);
            AppendPartAsHex(token, seller.LastName);

            return token.ToString();
        }

        private static void AppendPartAsHex(StringBuilder sb, string name)
        {
            var bytes = Encoding.UTF8.GetBytes(name);
            for (var counter = 0; counter < 2; counter++)
            {
                var b = 0;
                if (bytes.Length > counter)
                {
                    b = bytes[counter];
                }

                sb.AppendFormat(CultureInfo.InvariantCulture, "{0:X}", b);
            }
        }
    }
}
