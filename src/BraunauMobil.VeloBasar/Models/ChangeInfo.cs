using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BraunauMobil.VeloBasar.Models
{
    public class ChangeInfo
    {
        public ChangeInfo()
        {
            Denomination = new Dictionary<decimal, int>();
        }
        public ChangeInfo(decimal amount, IReadOnlyCollection<decimal> nominations)
        {
            Amount = amount;
            IsValid = true;
            
            var denomination = new Dictionary<decimal, int>();
            var remainingAmount = Amount;
            foreach (var nomination in nominations.OrderByDescending(x => x).Distinct())
            {
                if (nomination > Amount)
                {
                    denomination.Add(nomination, 0);
                    continue;
                }

                var count = (int)(remainingAmount / nomination);
                denomination.Add(nomination, count);

                remainingAmount -= (nomination * count);
            }
            Denomination = denomination;
        }

        [BindProperty]
        [DataType(DataType.Currency)]
        [Display(Name = "Wechselgeld")]
        public decimal Amount { get; set; }

        public bool IsValid { get; }
        public bool HasDenomination { get => Denomination.Values.Any(x => x > 0); }

        public IReadOnlyDictionary<decimal, int> Denomination { get; set; }
        
    }
}
