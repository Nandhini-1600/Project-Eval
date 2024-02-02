// ------------------------------------------------------------------------------------------------
// Training ~ A training program for interns at Metamation, Batch- July 2023.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// UnitTest1.cs
// Expression Evaluator.
// ------------------------------------------------------------------------------------------------
using static System.Console;
namespace Eval;

#region Class Program --------------------------------------------------------------------------
/// <summary>Expression Evaluator</summary>
class Program {
   #region Method ------------------------------------------------
   /// <summary>Gets the user input and prints the evaluated result</summary>
   static void Main (string[] args) {
      var eval = new Evaluator ();
      for (; ; ) {
         Write ("> ");
         string text = ReadLine () ?? "";
         if (text == "exit") break;
         try {
            double result = eval.Evaluate (text);
            ForegroundColor = ConsoleColor.Green;
            WriteLine (result);
         } catch (Exception e) {
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine (e.Message);
         }
         ResetColor ();
      }
   }
   #endregion
}
#endregion