using Pidgin;
using RndLangParser.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Pidgin.Parser;

namespace RndLangParser
{
    public class ParserT
    {
        public static Dictionary<string, DictionaryValue> dictonary(bool a)
        {
            var dic = new Dictionary<string, DictionaryValue>();
            var test = new DictionaryValue()
            {
                type = DictionaryValueType.FUNCTION,
                returnType = ParameterType.LONG,
                instance = new TestTree(a),
                name = "Col"
            };
            var testParam = new DictionaryValue()
            {
                type = DictionaryValueType.OBJECT,
                returnType = ParameterType.BOOL,
                instance = new TestTree(a),
                name = "cli"
            };
            var testParametrs = new DictionaryValue()
            {
                type = DictionaryValueType.FUNCTION,
                returnType = ParameterType.LONG,
                instance = new TestTree(a),
                name = "ColParametrs",
            };
            var testBool = new DictionaryValue()
            {
                type = DictionaryValueType.FUNCTION,
                returnType = ParameterType.LONG,
                instance = new TestTree(a),
                name = "ColBool",
            };
            var testDate = new DictionaryValue()
            {
                type = DictionaryValueType.FUNCTION,
                returnType = ParameterType.LONG,
                instance = new TestTree(a),
                name = "ColDate",
            };
            dic.Add("Col", test);
            dic.Add("ColParametrs", testParametrs);
            dic.Add("ColBool", testBool);
            dic.Add("ColDate", testDate);
            dic.Add("cli", testParam);
            return dic;
        }

        public static readonly Dictionary<string, DictionaryValue> dic = dictonary(true);

        private static readonly Parser<char, ITree> TreeDate =
           Rec(() => Num).Then(String("."), (day, comma) => (day)).Then(Rec(() => Num), (day, month) => (day, month))
           .Then(String("."), (dayMonth, coma) => (dayMonth)).Then(Rec(() => Num), (dayMounth, years) => (dayMounth.day, dayMounth.month, years))
           .Select<ITree>(x => new DateTree(new DateTime(x.years,x.month,x.day))).Between(SkipWhitespaces);

        private static readonly Parser<char, Expression> testNum =
            Num.Select<Expression>(x => Expression.Constant(x, typeof(int)));

        private static readonly Parser<char, Expression> TreeBool =
            Try(String("true")).ThenReturn(Expression.Constant(true)).Select<Expression>(x=>x)
            .Or(Try((String("false")).ThenReturn(Expression.Constant(false)).Select<Expression>(x=>x)));

        public static Parser<char, ITree> TreeEnity =
           Parser.AtLeastOnceString(Lowercase).Separated(String("."))
           .Select<ITree>(objName =>
           {
               var objectName = string.Empty;
               foreach (var item in objName)
               {
                   objectName += item;
               }
               Func<(Dictionary<string, DictionaryValue> dictonary, string name), object> funcForParser = dictonaryAndNameObj =>
               {
                   var dictionaryValue = dictonaryAndNameObj.dictonary[dictonaryAndNameObj.name];
                   Func<DictionaryValue, object> getObj = dictonary =>
                   {
                       var res = dictonary.GetType().GetProperty("instance").GetValue(dictonary).GetType().GetProperty(objectName).GetValue(dictonary.instance);
                       return (res,dictonaryAndNameObj.name);
                   };

                   return getObj(dictionaryValue);
               };

               Expression<Func<(Dictionary<string, DictionaryValue>, string), object>> expression = res => funcForParser(res);

               return new ObjectTree((expression.Compile(), objectName));
           });

        public static Parser<char, ITree> FuncParce =
        
              Parser.AtLeastOnceString(LetterOrDigit).Between(SkipWhitespaces)
                   .Then(Try(Rec(() => TreeDate))
                   .Or(Try(Rec(() => TreeEnity)))
                       .Separated(String(",")).Between(String("("), String(")")), (nameFunc, parametrs) => (nameFunc, parametrs)).Select<ITree>(x =>
                   {
                       var tmp = string.Empty;
                       foreach (var item in x.nameFunc)
                       {
                           tmp += item;
                       }

                       var parametrsList = new List<ITree>();
                       foreach (var item in x.parametrs)
                       {
                           parametrsList.Add(item);
                       }

                       Func<Dictionary<string, DictionaryValue>, object> funcForParser = dictonary =>
                       {
                           var dictionaryValue = dictonary[tmp];

                           Func<DictionaryValue, object> func = dictionaryValue =>
                           {
                               var parametrs = new Object[parametrsList.Count];
                               var count = 0;
                               foreach (var item in parametrsList)
                               {
                                   if (item is ObjectTree)
                                   {
                                       var objectTree = item as ObjectTree;
                                       var invokeObj = objectTree.Tulp.func.Invoke((dictonary, objectTree.Tulp.nameFunc));
                                       var parametr = (((object, string))invokeObj).Item1;
                                       parametrs[count] = parametr;
                                       count++;
                                       continue;
                                   }

                                   if (item is DateTree)
                                   {
                                       var dateTree = item as DateTree;
                                       parametrs[count] = dateTree.Date;
                                       count++;
                                       continue;
                                   }
                               }
                               var res = dictionaryValue.GetType().GetProperty("instance").GetValue(dictionaryValue).GetType().GetMethod(tmp).Invoke(dictionaryValue.instance, parametrs);
                               return res;
                           };

                           return func(dictionaryValue);
                       };
                       Expression<Func<Dictionary<string, DictionaryValue>, object>> expression = res => funcForParser(res);
                       var result = new FuncTree(expression.Compile());
                       return result;
                   });

        public static Parser<char, Expression> Func =
                  Parser.AtLeastOnceString(LetterOrDigit).Between(SkipWhitespaces)
                 .Then(Try(Rec(() => TreeDate))
                 //.Or(Try(Rec(() => TreeBool)))
                 //.Or(Try(Rec(() => testNum)))
                 //.Or(Try(Rec(() => TreeEnity(Expression.Parameter(typeof(Dictionary<string, DictionaryValue>))))))
                 .Separated(String(",")
                 .Between(SkipWhitespaces))
                 .Between(String("("), String(")")), (name, val) =>
                 {
                     List<Type> tmp = new List<Type>();
                     foreach (var item in val)
                     {
                         tmp.Add(item.GetType());
                     }
                     var parametrs = tmp.ToArray();
                     if (parametrs.Length != 0)
                     {
                         return (Expression.Call(null, dic[name].instance.GetType().GetMethod(dic[name].name, parametrs)));
                     }
                     else return (Expression.Call(null, dic[name].instance.GetType().GetMethod(dic[name].name, parametrs)));

                 }).Select<Expression>(x => x);

        private static readonly Parser<char, Expression> Сomparisonsign =
           String("==").Or(String("<").Or(String(">").Or(String("=>").Or(String("<=").Or(String("!=")))))).Select<Expression>(x=>Expression.Constant(x)).Between(SkipWhitespaces);

        public static Parser<char, ITree> Return = String("Return").Between(SkipWhitespaces).Then(Try(FuncParce).Or(Try(TreeDate)))
            .Select<ITree>(x => 
            {
                Func<Dictionary<string, DictionaryValue>, object> res = r =>
                {
                    if (x is FuncTree)
                    {
                        var res = (FuncTree)x;
                        return res.Func.Invoke(r);
                    }
                    else
                    {
                        var res = (DateTree)x;
                        return res;
                    }
                        
                };
                Expression<Func<Dictionary<string, DictionaryValue>, object>> expression = ress => res(ress);
                return new ReturnTree(expression.Compile());
            });

        public static Parser<char, Expression<Func<Dictionary<string, DictionaryValue>, object>>> Condition = Try(FuncParce).Then(Сomparisonsign.Then(FuncParce, (com, second)
           => (com, second)), (first, secondCom) => (first, secondCom))
            .Select<Expression<Func<Dictionary<string, DictionaryValue>, object>>>(x =>
            {
                Func<Dictionary<string, DictionaryValue>, object> eq = dictonary =>
                {
                    Func<Dictionary<string, DictionaryValue>, object> res = dictonary =>
                     {
                         var funcTree = (FuncTree)x.first;
                         var resFunc = funcTree.Func.Invoke(dictonary);
                         
                         var funcTreeSecond = (FuncTree)x.secondCom.second;
                         var resFuncSecond = funcTreeSecond.Func.Invoke(dictonary);

                         var equalFunc = Expression.Equal(Expression.Constant(resFunc), Expression.Constant(resFuncSecond));
                         var res = Expression.Lambda(equalFunc).Compile().DynamicInvoke();
                         return res;
                     };
                    return res(dictonary);
                };
                Expression<Func<Dictionary<string, DictionaryValue>, object>> expression = ress => eq(ress);
                return expression;
            });

        public static Parser<char, ITree> TreeThen = String("then").Between(SkipWhitespaces).Then(Try(FuncParce.Separated(String(";")))
            .Then(Return, (block, ret) =>
        (block, ret)).Select<ITree>(x =>
        {
            Func<Dictionary<string, DictionaryValue>, object> eq = eq =>
            {
                foreach (var item in x.block)
                {
                    var func = (FuncTree)item;
                    func.Func.Invoke(eq);
                }
                var arsa = (ReturnTree)x.ret;
                var fas = arsa.Return.Invoke(eq);
                return fas;
            };
            
            Func<Dictionary<string, DictionaryValue>, object> expression = ress => eq(ress);
            var r = new ThenTree(expression);
            return r;
        }));

        public static void TreeMemberSS()
        {
            static object Run(string str)
            {
                var ctx = Expression.Parameter(typeof(Dictionary<string, DictionaryValue>));
                var dic2 = new Dictionary<string,DictionaryValue>();
                var testParametrs = new DictionaryValue()
                {
                    type = DictionaryValueType.OBJECT,
                    returnType = ParameterType.BOOL,
                    instance = new TestTree(false),
                    name = "cli",
                };
                var testParametrs2 = new DictionaryValue()
                {
                    type = DictionaryValueType.FUNCTION,
                    returnType = ParameterType.LONG,
                    instance = new TestTree(false),
                    name = "ColBool",
                };
                dic2.Add("cli", testParametrs);
                dic2.Add("ColBool", testParametrs2);
                var woofReturn = Return.Parse("Return ColBool(cli,10.09.2020)").Value;
                var returnTree = (ReturnTree)woofReturn;
                var woofThen = TreeThen.Parse("thenColBool(cli,10.09.2020)ReturnColBool(cli,10.09.2020)").Value;
                var thenTree = (ThenTree)woofThen;
                var resThen = thenTree.Then.Invoke(dic);
                var ye = returnTree.Return.Invoke(dic2);
                var woof = Condition.Parse("ColBool(cli,10.09.2020)==ColBool(cli,10.09.2020)").Value;
                var wooftest = woof.Compile();
                var woofInvoke = wooftest.DynamicInvoke(dic);
                var woofCompile = woof.Compile().DynamicInvoke(dic2);

                return 3;
            }
          
            var result1 = Run("ColBool(cli,10.09.2020)");
            var result2 = Run("Col()");
            var result3 = Run("ColParametrs(1111)");
        }
    }
}
