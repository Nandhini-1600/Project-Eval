// ------------------------------------------------------------------------------------------------
// Training ~ A training program for interns at Metamation, Batch- July 2023.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// UnitTest1.cs
// Test case for Expression Evaluator.
// ------------------------------------------------------------------------------------------------
using Eval;
namespace TestTraining;

[TestClass]
public class TestTraining {
   [TestMethod]
   public void TestParse () {
      Evaluator eval = new ();
      // Expressions with their expected results
      Dictionary<string, double> exp = new Dictionary<string, double> {
            { "-10 ^ 2", Math.Pow(-10, 2) }, { "a = 4", 4 }, { "b = 3.5", 3.5 }, { "a + b", 4 + 3.5 },
            { "sqrt 25", Math.Sqrt(25) }, { "log 1", Math.Log(1) }, { "-2 -2", -2 - 2 },
            { "10/2+3", 10.0 / 2 + 3 }, { "(+10+3)*2", (10 + 3) * 2 }, { "(a+2) * a", (4 + 2) * 4 },
            { "cos 0", Math.Cos(0) }, { "exp 2", Math.Exp(2) }, { "cos acos 0", Math.Cos(Math.Acos(0)) },
            { "10 -2 -2", 10 - 2 - 2 }, { "---5", -(-(-5)) }, { "-5+10", -5 + 10 }, { "-2-4", -2 - 4 },
            { "---4+5--2+3",-(-(-4)) + 5 - (-2) + 3 }, { "(2+3)*-4", (2 + 3) * -4 }, { "(2+3)*+5", (2 + 3) * 5 },
            { "-4*(3+5)", -4 * (3 + 5) }, { "-4+5--8", -4 + 5 - (-8) }, { "-4+5-(-8)", -4 + 5 - (-8) },
            { "10^(-4+2)", Math.Pow(10, (-4 + 2)) }, { "-10-10^2", -10 - Math.Pow(10, 2) },
            { "---4+5--6-2", -(-(-4)) + 5 - (-6) - 2 }, { "sin 45", Math.Sin(45) }, { "sin -45", Math.Sin(-45) },
            { "-sin 45", -Math.Sin(45) },{ "cos 45", Math.Cos(45) },{ "cos -45", Math.Cos(-45) },
            { "tan 45", Math.Tan(45 * Math.PI / 180) }, { "sin(45+45)", Math.Sin((45 + 45)) }, { "(sin 90)*2", Math.Sin(90) * 2 },
            { "sin --90", Math.Sin(-(-90)) }, { "log 10", Math.Log(10) }, { "sqrt 100", Math.Sqrt(100) },
            { "sqrt(10^2)", Math.Sqrt(Math.Pow(10, 2)) }, { "(sqrt 100) - 10", Math.Sqrt(100) - 10 },
            { "(tan 45)+10-20", Math.Tan(45 * Math.PI / 180) + 10 - 20 }, { "asin 1", Math.Asin(1)* 180 / Math.PI },
            { "acos 0", Math.Acos(0) * 180 / Math.PI}, { "(log 10)+5", Math.Log(10) + 5 }, { "log(10+5)", Math.Log(10 + 5) },
            { "(sin 90)--1", Math.Sin(90) - (-1) }, { "(sin -90)--1", Math.Sin(-90) - (-1) },
            { "sqrt(90+10)", Math.Sqrt(90 + 10) }, { "sqrt(110-10)", Math.Sqrt(110 - 10) },
            { "atan 1", Math.Atan(1) * 180 / Math.PI }, { "asin -1", Math.Asin(-1)* 180 / Math.PI},
            { "atan -1", Math.Atan(-1)* 180 / Math.PI}, { "(atan -1)+45", (Math.Atan(-1)* 180 / Math.PI) + 45 },
            { "exp 1", Math.Exp(1) }, { "exp 1-2", Math.Exp(1) - 2 }, { "exp(2-1)", Math.Exp(2 - 1) },
            { "exp -1", Math.Exp(-1) }, { "sqrt -100", double.NaN }, { "log(-10+5)", double.NaN },
            { "sin(sqrt-1)", double.NaN }, { "sqrt asin-1", double.NaN }
        };
      List<string> exception = new () { "3 + * 5", "(4 + 6", "2 + abc", "6 *", "5 * (3 + 2))", "3 + 2 *" };
      foreach (var input in exp) {
         double expected = Math.Round (input.Value);
         double actual = Math.Round (eval.Evaluate (input.Key));
         Assert.AreEqual (actual, expected);
      }
      foreach (var ip in exception) Assert.ThrowsException<EvalException> (() => eval.Evaluate (ip));
   }
}