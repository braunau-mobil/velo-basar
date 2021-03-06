﻿using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class ViewModel<T> : BasarViewModel
    {
        public ViewModel(Basar basar, T value) : base(basar)
        {
            Value = value;
        }

        public T Value { get; set; }
    }
}
