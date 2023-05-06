using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Homework12.Model
{
    public static class EnumHelper
    {
        /// <summary>
        /// Получение описания из Enum
        /// </summary>
        /// <param name="value">значение Enum</param>
        /// <returns>текст описания</returns>
        public static string Description(this Enum value)
        {
            var attributes = value.GetType().GetField(value.ToString())?.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Any())
            {
                var description = (attributes.First() as DescriptionAttribute)?.Description;
                if (description != null)
                    return description;
            }

            // Если описание не найдено, заменяем подчёркивания на пробелы
            var ti = CultureInfo.CurrentCulture.TextInfo;
            return ti.ToTitleCase(ti.ToLower(value.ToString().Replace("_", " ")));
        }

        /// <summary>
        /// Возврат значений и описаний
        /// </summary>
        /// <param name="t"></param>
        /// <returns>значения и описания</returns>
        /// <exception cref="ArgumentException">сообщение об ошибке</exception>
        public static IEnumerable<ValueDescription> GetAllValuesAndDescriptions(Type t)
        {
            if (!t.IsEnum)
                throw new ArgumentException($"{nameof(t)} должно быть типа enum");

            return Enum.GetValues(t).Cast<Enum>().Select((e) => new ValueDescription() { Value = e, Description = e.Description() }).ToList();
        }
    }
}
