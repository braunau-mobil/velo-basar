using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

using Xan.AspNetCore.Rendering;

namespace BraunauMobil.VeloBasar.Rendering;

public class DefaultBoostrapHtmlFactory
    : DefaultHtmlFactory
    , IBootstrapHtmlFactory
{
    private ViewContext? _viewContext;

    public DefaultBoostrapHtmlFactory(IVeloRouter router, VeloTexts txt)
    {

        Router = router ?? throw new ArgumentNullException(nameof(router));
        Txt = txt ?? throw new ArgumentNullException(nameof(txt));
    }

    public virtual void Contextualize(ViewContext viewContext)
    {
        ArgumentNullException.ThrowIfNull(viewContext);

        _viewContext = viewContext;
    }

    public ViewContext ViewContext
    {
        get
        {
            if (_viewContext == null)
            {
                throw new InvalidOperationException($"No {nameof(ViewContext)} set, forgot to call {Contextualize}?");
            }
            return _viewContext;
        }
    }

    protected IVeloRouter Router { get; }

    protected VeloTexts Txt { get; }

    public TagBuilder Alert(MessageType type, LocalizedString text)
    {
        ArgumentNullException.ThrowIfNull(text);

        TagBuilder p = Paragraph();
        p.InnerHtml.SetHtmlContent(text);

        return Alert(type, p);
    }

    public TagBuilder Alert(MessageType type, LocalizedString title, LocalizedString text)
    {
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(text);

        TagBuilder heading = Heading(4);
        heading.AddCssClass("alert-heading");
        heading.InnerHtml.SetHtmlContent(title);

        TagBuilder p = Paragraph();
        p.InnerHtml.SetHtmlContent(text);

        HtmlContentBuilder content = new();
        content.AppendHtml(heading);
        content.AppendHtml(p);

        return Alert(type, content);
    }

    public TagBuilder Badge(BadgeType type)
    {
        string css = $"badge rounded-pill {type.ToCss()}";
        TagBuilder span = Span();
        span.AddCssClass(css);
        return span;
    }

    public TagBuilder CheckBoxField(string name, bool value, LocalizedString title)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(title);

        HtmlContentBuilder content = new();

        TagBuilder input = CheckBox(name, value);
        input.AddCssClass("form-check-input");
        content.AppendHtml(input);

        TagBuilder label = Label();
        label.AddCssClass("form-check-label");
        label.InnerHtml.SetHtmlContent(title);
        content.AppendHtml(label);

        ModelStateEntry? stateEntry = ViewContext.ModelState[name];
        if (stateEntry != null && stateEntry.ValidationState == ModelValidationState.Invalid)
        {
            input.AddCssClass("is-invalid");

            IHtmlContent validationResult = FieldValidationResults(stateEntry);
            TagBuilder validationFeedback = Div();
            validationFeedback.AddCssClass("invalid-feedback");
            validationFeedback.InnerHtml.SetHtmlContent(validationResult);

            content.AppendHtml(validationFeedback);
        }

        TagBuilder div = Div();
        div.AddCssClass("form-check mb-3");
        div.InnerHtml.SetHtmlContent(content);
        return div;
    }

    public TagBuilder DateInputField(string name, DateTime value, LocalizedString title, bool autoFocus = false)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(title);

        TagBuilder input = DateInput(name, value, autoFocus);
        input.AddCssClass("form-control");

        return InputField(name, input, title);
    }

    public string EnabledCss(bool isEnabled)
    {
        if (isEnabled)
        {
            return "";
        }
        return "disabled";
    }

    public TagBuilder EmailInputField(string name, string? value, LocalizedString title, bool autoFocus = false)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(title);

        TagBuilder input = Input("email", name, value, autoFocus);
        input.AddCssClass("form-control");

        return InputField(name, input, title);
    }

    public IHtmlContent NumberInputField(string name, int value, LocalizedString title, bool autoFocus = false)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(title);

        TagBuilder input = NumberInput(name, value, autoFocus);
        input.AddCssClass("form-control");

        return InputField(name, input, title);
    }

    public IHtmlContent NumberInputField(string name, double value, LocalizedString title, bool autoFocus = false)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(title);

        TagBuilder input = NumberInput(name, value, autoFocus);
        input.AddCssClass("form-control");

        return InputField(name, input, title);
    }

    public IHtmlContent NumberInputField(string name, decimal value, LocalizedString title, bool autoFocus = false)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(title);

        TagBuilder input = NumberInput(name, value, autoFocus);
        input.AddCssClass("form-control");

        return InputField(name, input, title);
    }

    public TagBuilder PasswordInputField(string name, string? value, LocalizedString title, bool autoFocus = false)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(title);

        TagBuilder input = PasswordInput(name, value, autoFocus);
        input.AddCssClass("form-control");

        return InputField(name, input, title);
    }

    public TagBuilder PhoneNumberInputField(string name, string? value, LocalizedString title, bool autoFocus = false)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(title);

        TagBuilder input = Input("tel", name, value, autoFocus);
        input.AddCssClass("form-control");

        return InputField(name, input, title);
    }

    public override TagBuilder Select(string name, string? value, SelectList items, bool submitOnChange = false, bool autoFocus = false)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(value);

        TagBuilder select = base.Select(name, value, items, submitOnChange, autoFocus);
        select.AddCssClass("form-select");
        return select;
    }

    public TagBuilder SelectField<TEnum>(string name, TEnum? value, SelectList items, LocalizedString title, bool submitOnChange = false, bool autoFocus = false)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(title);

        TagBuilder input = Select(name, value, items, submitOnChange, autoFocus);

        return InputField(name, input, title);
    }

    public override TagBuilder Table()
    {
        TagBuilder table = base.Table();
        table.AddCssClass("table table-striped m-0");
        return table;
    }

    public TagBuilder TextInputField(string name, string? value, LocalizedString title, bool autoFocus = false)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(title);

        TagBuilder input = TextInput(name, value, autoFocus);
        input.AddCssClass("form-control");

        return InputField(name, input, title);
    }

    public TagBuilder TextAreaField(string name, string? value, LocalizedString title, bool autoFocus = false)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(title);

        TagBuilder input = TextArea(name, value, autoFocus);
        input.AddCssClass("form-control");

        TagBuilder field = InputField(name, input, title);
        return field;
    }

    public IHtmlContent TitleCardHeader()
    {
        LocalizedString? title = ViewContext.ViewData.Title();
        IHtmlContent htmlTitle;
        if (title != null)
        {
            htmlTitle = title.ToHtml();
        }
        else
        {
            htmlTitle = new HtmlString("");
        }

        TagBuilder heading = Heading(4);
        heading.AddCssClass("card-header");
        heading.InnerHtml.SetHtmlContent(htmlTitle);
        return heading;
    }

    public IHtmlContent NonFieldValidationResults()
    {
        HtmlContentBuilder errorMessages = new();
        foreach (KeyValuePair<string, ModelStateEntry> modelState in ViewContext.ModelState)
        {
            if (!string.IsNullOrEmpty(modelState.Key)
                && modelState.Value.ValidationState == ModelValidationState.Invalid)
            {
                foreach (ModelError error in modelState.Value.Errors)
                {
                    errorMessages.AppendHtmlLine(error.ErrorMessage);
                }
            }
        }

        if (errorMessages.Count == 0)
        {
            return new HtmlString(null);
        }
        return Alert(MessageType.Danger, errorMessages);
    }    

    private TagBuilder InputField(string name, TagBuilder input, LocalizedString title)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(title);

        TagBuilder div = Div();
        div.AddCssClass("mb-3");

        TagBuilder label = Label();
        label.AddCssClass("form-label");
        label.InnerHtml.SetHtmlContent(title);
        div.InnerHtml.AppendHtml(label);

        div.InnerHtml.AppendHtml(input);

        ModelStateEntry? stateEntry = ViewContext.ModelState[name];
        if (stateEntry != null && stateEntry.ValidationState == ModelValidationState.Invalid)
        {
            input.AddCssClass("is-invalid");

            IHtmlContent validationResult = FieldValidationResults(stateEntry);
            TagBuilder validationFeedback = Div();
            validationFeedback.AddCssClass("invalid-feedback");
            validationFeedback.InnerHtml.SetHtmlContent(validationResult);

            div.InnerHtml.AppendHtml(validationFeedback);
        }

        return div;
    }

    private TagBuilder Alert(MessageType type, IHtmlContent content)
    {
        TagBuilder div = Div();
        div.AddCssClass("alert");
        div.AddCssClass(type.ToCss());
        div.Attributes.Add("role", "alert");
        div.InnerHtml.SetHtmlContent(content);
        return div;
    }

    private static IHtmlContent FieldValidationResults(ModelStateEntry stateEntry)
    {
        HtmlContentBuilder errorMessages = new();
        if (stateEntry.ValidationState == ModelValidationState.Invalid)
        {
            foreach (ModelError error in stateEntry.Errors)
            {
                errorMessages.AppendHtmlLine(error.ErrorMessage);
            }
        }
        return errorMessages;
    }


}
