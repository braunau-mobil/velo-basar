using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BraunauMobil.VeloBasar.AuthoringTagHelpers.TagHelpers
{
    public class DynamicTableConfiguration
    {
        private readonly ModelMetadata _model;

        public DynamicTableConfiguration(ModelMetadata model)
        {
            _model = model;
        }

        public void AddColumn(DynamicColumnTagHelper tagHelper)
        {
            if (tagHelper.GetPage != null)
            {
                AddColumn(tagHelper.GetPage, tagHelper.PageText, tagHelper.Width);
            }
            else
            {
                AddColumn(tagHelper.PropertyName, tagHelper.Width);
            }
        }
        public void AddColumn(string name, string width)
        {
            var property = _model.Properties.FirstOrDefault(p => p.Name == name);
            if (property == null)
            {
                throw new InvalidOperationException($"No matching property with name: {name} found on {_model.ModelType.Name}");
            }
            Columns.Add(new DynamicPropertyColumnConfiguration
            {
                Property = property,
                Width = width
            });
        }
        public void AddColumn(Func<object, VeloPage> getPage, string pageText, string width)
        {
            Columns.Add(new DynamicPageColumnConfiguration
            {
                GetPage = getPage,
                PageText = pageText,
                Width = width
            });
        }

        public IList<DynamicColumnConfiguration> Columns { get; } = new List<DynamicColumnConfiguration>();
    }
}
