using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RndLangParser.Model
{
    public class FunctionParameter
    {
        /// <summary>
        /// Номер по порядку
        /// </summary>
        public int order { get; set; }

        /// <summary>
        /// Наименование параметра
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Признак обязательности параметра
        /// </summary>
        public bool required { get; set; }

        /// <summary>
        /// Тип параметра
        /// </summary>
        public ParameterType type { get; set; }
    }
}
