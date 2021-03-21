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

        public void AddColumn(string name, string width)
        {
            var property = _model.Properties.FirstOrDefault(p => p.Name == name);
            if (property == null)
            {
                throw new InvalidOperationException($"No matching property with name: {name} found on {_model.ModelType.Name}");
            }
            Columns.Add(new DynamicColumnConfiguration
            {
                Property = property,
                Width = width
            });
        }

        public IList<DynamicColumnConfiguration> Columns { get; } = new List<DynamicColumnConfiguration>();
    }
}
