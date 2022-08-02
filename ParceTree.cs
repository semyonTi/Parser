
using RndLangParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RndLangParser
{
    public interface ITree
    {

    }

    class ObjectTree : ITree
    {
        public (Func<(Dictionary<string, DictionaryValue>, string), object> func, string nameFunc ) Tulp { get; set; }
        public ObjectTree((Func<(Dictionary<string, DictionaryValue>, string), object>, string) tmp)
        {
            Tulp = tmp;
        }
    }
    class DateTree : ITree
    {
        public DateTime Date { get; set; }
        public DateTree(DateTime date)
        {
            Date = date;
        }
    }

    class FuncTree : ITree
    {
        public Func<Dictionary<string, DictionaryValue>, object> Func { get; set; }
        public FuncTree(Func<Dictionary<string, DictionaryValue>, object> func)
        {
            Func = func;
        }
    }

    class ReturnTree : ITree
    {
        public Func<Dictionary<string, DictionaryValue>, object> Return { get; set; }
        public ReturnTree(Func<Dictionary<string, DictionaryValue>, object> func)
        {
            Return = func;
        }
    }

    class ThenTree : ITree
    {
        public Func<Dictionary<string, DictionaryValue>, object> Then { get; set; }
        public ThenTree(Func<Dictionary<string, DictionaryValue>, object> func)
        {
            Then = func;
        }
    }
}
