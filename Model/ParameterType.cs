using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RndLangParser.Model
{
    public enum ParameterType
    {
        /// <summary>
        /// Строка
        /// </summary>
        STRING = 1,
        /// <summary>
        /// Дата/время
        /// </summary>
        DATETIME = 2,
        /// <summary>
        /// Целое число
        /// </summary>
        LONG = 3,
        /// <summary>
        /// Дробное число
        /// </summary>
        DECIMAL = 4,
        /// <summary>
        /// Логическое
        /// </summary>
        BOOL = 5,
        /// <summary>
        /// null
        /// </summary>
        NULL = 6
    }
}
