using System;
using System.ComponentModel;
using System.Reflection;
using GraphQL;
using GraphQL.Types;

namespace SandwichClub.Api.GraphQL.Types
{
    public class AutoObjectGraphType<T> : ObjectGraphType<T>
    {
        public AutoObjectGraphType()
        {
            var type = typeof(T);

            Name = ToCamelCase(type.Name);
            Description = type.GetCustomAttribute<DescriptionAttribute>()?.Description;

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propertyType = property.PropertyType;

                var fieldGraphType = propertyType.GetGraphTypeFromType(!propertyType.IsValueType || propertyType.IsNullable());
                var fieldName = ToCamelCase(property.Name);
                var description = property.GetCustomAttribute<DescriptionAttribute>()?.Description;
                var deprecationReason = property.GetCustomAttribute<ObsoleteAttribute>()?.Message;

                Field(fieldGraphType, fieldName, description, deprecationReason: deprecationReason);
            }
        }

        private static string ToCamelCase(string str)
            => $"{char.ToLower(str[0])}{str.Substring(1)}";
    }
}
