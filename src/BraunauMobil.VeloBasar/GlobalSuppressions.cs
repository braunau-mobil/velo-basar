// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
//  @todo This should not be supressed globally, try to configure it with the .editorconfig file (this didnt' work for me at 2019-12-28)
[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "namespaceanddescendants", Target = "BraunauMobil.VeloBasar.Migrations")]
