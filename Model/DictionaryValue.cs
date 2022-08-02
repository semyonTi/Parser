using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RndLangParser.Model
{
    public class DictionaryValue
    {
        /// <summary>
        /// Тип элемента
        /// </summary>
        public DictionaryValueType type { get; set; }

        /// <summary>
        /// Возвращаемый тип элемента 
        /// </summary>
        public ParameterType returnType { get; set; }

        /// <summary>
        /// Экземпляр класса
        /// </summary>
        public object instance { get; set; }

        /// <summary>
        /// Наименование функции/метода/поля
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Параметры функции/поля/метода
        /// </summary>
        public IEnumerable<FunctionParameter> parameters { get; set; }
    }
}

