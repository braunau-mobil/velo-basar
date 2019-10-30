﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace BraunauMobil.VeloBasar
{
    public static class ExtensionMethods
    {
        public static T TakeRandom<T>(this T[] array, Random rand)
        {
            return array[rand.Next(0, array.Length - 1)];
        }

        public static double NextGaussian(this Random rand, double mean, double stdDev)
        {
            //  yehaw random stackoverflow code: https://stackoverflow.com/questions/218060/random-gaussian-variables
            var u1 = 1.0 - rand.NextDouble();
            var u2 = 1.0 - rand.NextDouble();
            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            return mean + stdDev * randStdNormal;
        }

        public static IActionResult RedirectToPage<TPageModel>(this PageModel pageModel, object parameter = null) where TPageModel : PageModel
        {
            return pageModel.RedirectToPage(Utils.GetPageForModel<TPageModel>(), parameter);
        }

        public static VeloPage GetPage<TPageModel>(this PageModel page, object parameter = null)
        {
            return new VeloPage { Page = Utils.GetPageForModel<TPageModel>(), Parameter = parameter };
        }

        public static VeloPage GetPage<TPageModel>(this IRazorPage page, object parameter = null)
        {
            return new VeloPage { Page = Utils.GetPageForModel<TPageModel>(), Parameter = parameter };
        }
    }
}
