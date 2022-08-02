using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RndLangParser
{
    public class TestTree
    {

        public bool cli { get; set; }
        public bool clic { get; set; }
        public TestTree(bool a)
        {
            cli = a;
            clic = a;
        }

        public static DateTime ColDate(DateTime b, bool a)
        {
            if (a)
            {
                return b;
            }
            return DateTime.Now;
        }
        public static DateTime ColBool(bool a, DateTime b)
        {
            if (a)
            {
                return b;
            }

            return DateTime.Now;
        }
        public static int Col()
        {
            Console.WriteLine("Col");
            return 10;
        }
        public static int ColParametrs(int a)
        {
            if (a > 10)
            {
                return 6;
            }
            return 3;
        }
    }
}
