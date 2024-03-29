﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests;

public static class X
{
    public static DateTime FirstContactDay { get; } = new DateTime(2063, 04, 05, 11, 22, 33);

    public static IFormatProvider FormatProvider { get; } = CultureInfo.GetCultureInfo("de-AT");

    public static IStringLocalizer<SharedResources> StringLocalizer { get; } = new StringLocalizerMock(FormatProvider);

    public static Action<ServiceDescriptor> CreateInspector<TServiceType, TImplementationType>(ServiceLifetime lifetime)
        => item =>
        {
            item.ServiceType.Should().Be(typeof(TServiceType));
            item.ImplementationType.Should().Be(typeof(TImplementationType));
            item.Lifetime.Should().Be(lifetime);
        };

    public static T StrictFake<T>()
        where T : class
        => A.Fake<T>(options => options.Strict());

    public static string Line(this string s, string nextLine = "")
    {
        ArgumentNullException.ThrowIfNull(s);
        ArgumentNullException.ThrowIfNull(nextLine);

        return s + Environment.NewLine + nextLine;
    }

    public static IPaginatedList<T> EmptyPaginatedList<T>()
        => new PaginatedList<T>(new List<T>(), 0, 0, 0, 0);
}
