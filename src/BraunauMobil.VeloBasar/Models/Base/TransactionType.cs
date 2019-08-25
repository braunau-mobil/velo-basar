﻿using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models.Base
{
    public enum TransactionType
    {
        [Display(Name = "Annahme")]
        Acceptance,
        [Display(Name = "Storno")]
        Cancellation,
        [Display(Name = "Verkauf")]
        Sale,
        [Display(Name = "Abrechnung")]
        Settlement,
    };
}
