using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using Xan.AspNetCore.Rendering;

namespace BraunauMobil.VeloBasar.Rendering;

public interface IBootstrapHtmlFactory
    : IHtmlFactory
    , IViewContextAware
{
    TagBuilder Alert(MessageType type, LocalizedString text);

    TagBuilder Alert(MessageType type, LocalizedString title, LocalizedString text);

    TagBuilder Badge(BadgeType type);

    TagBuilder CheckBoxField(string name, bool value, LocalizedString title);

    TagBuilder DateInputField(string name, DateTime value, LocalizedString title, bool autoFocus = false);

    string EnabledCss(bool isEnabled);

    TagBuilder EmailInputField(string name, string? value, LocalizedString title, bool autoFocus = false);

    IHtmlContent NumberInputField(string name, int value, LocalizedString title, bool autoFocus = false);

    IHtmlContent NumberInputField(string name, double value, LocalizedString title, bool autoFocus = false);

    IHtmlContent NumberInputField(string name, decimal value, LocalizedString title, bool autoFocus = false);

    TagBuilder PasswordInputField(string name, string? value, LocalizedString title, bool autoFocus = false);

    TagBuilder PhoneNumberInputField(string name, string? value, LocalizedString title, bool autoFocus = false);

    TagBuilder SelectField<TEnum>(string name, TEnum? value, SelectList items, LocalizedString title, bool submitOnChange = false, bool autoFocus = false);

    TagBuilder TextAreaField(string name, string? value, LocalizedString title, bool autoFocus = false);

    TagBuilder TextInputField(string name, string? value, LocalizedString title, bool autoFocus = false);

    IHtmlContent TitleCardHeader();

    IHtmlContent NonFieldValidationResults();
}
