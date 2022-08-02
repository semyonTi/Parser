using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RndLangParser.Model
{
    public enum DictionaryValueType
    {
        /// <summary>
        /// Объект
        /// </summary>
        OBJECT = 1,
        /// <summary>
        /// Константа
        /// </summary>
        CONSTANT = 2,
        /// <summary>
        /// Функция
        /// </summary>
        FUNCTION = 3,
    }
}
