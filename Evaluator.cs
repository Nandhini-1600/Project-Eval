namespace Eval;

#region EvalException --------------------------------------------------------------------------
/// <summary>Class EvalException</summary>
public class EvalException : Exception {
   public EvalException (string message) : base (message) { }
}
#endregion

#region Class Evaluator ------------------------------------------------------------------------
/// <summary>Class Evaluator</summary>
public class Evaluator {
   #region Property-----------------------------------------------
   public int BasePriority { get; set; }
   #endregion

   #region Method ------------------------------------------------
   /// <summary>Apply the operator with its corresponding operands</summary>
   void ApplyOperator () {
      var op = mOperators.Pop ();
      var f1 = mOperands.Pop ();
      if (op is TOpArithmetic arith) {
         if (mOperands.Count == 0) throw new EvalException ("Too few operands");
         var f2 = mOperands.Pop ();
         mOperands.Push (arith.Evaluate (f2, f1));
      } else mOperands.Push (op is TOpUnary u ? u.Evaluate (f1) : ((TOpFunction)op).Evaluate (f1));
   }

   /// <summary>Evaluate the expressions</summary>
   /// <param name="input">Expression input</param>
   /// <returns>Returns a double after evaluating</returns>
   public double Evaluate (string text) {
      Reset ();
      List<Token> tokens = new ();
      var tokenizer = new Tokenizer (this, text);
      for (; ; ) {
         var token = tokenizer.Next (tokens);
         if (token is TEnd) break;
         if (token is TError err) throw new EvalException (err.Message);
         tokens.Add (token);
      }
      // Check if this is a variable assignment
      TVariable? tVariable = null;
      if (tokens.Count > 2 && tokens[0] is TVariable tvar && tokens[1] is TOpArithmetic { Op: '=' }) {
         tVariable = tvar;
         tokens.RemoveRange (0, 2);
      }
      foreach (var t in tokens) Process (t);
      while (mOperators.Count > 0) ApplyOperator ();
      if (mOperators.Count > 0) throw new EvalException ("Too many operators");
      if (mOperands.Count != 1) throw new EvalException ("Too many operands");
      if (BasePriority != 0) throw new EvalException ("Invalid Paranthesis");
      double f = mOperands.Pop ();
      if (tVariable != null) mVars[tVariable.Name] = f;
      return f;
   }

   /// <summary>Gets the assigned variable</summary>
   /// <param name="name">Variable name</param>
   /// <returns>Returns the value of the variable</returns>
   /// <exception cref="EvalException">Throws exception if variable is unknown</exception>
   public double GetVariable (string name) {
      if (mVars.TryGetValue (name, out double f)) return f;
      throw new EvalException ($"Unknown variable: {name}");
   }

   /// <summary>Push operands and operators in individual stack based on its priority</summary>
   /// <param name="token">Each token</param>
   void Process (Token token) {
      switch (token) {
         case TNumber num:
            mOperands.Push (num.Value);
            break;
         case TOperator op:
            while (mOperands.Count > 0 && mOperators.Count > 0 && mOperators.Peek ().Priority >= op.Priority)
               ApplyOperator ();
            mOperators.Push (op);
            break;
         case TPunctuation p:
            BasePriority += p.Punct == '(' ? 10 : -10;
            break;
         default:
            throw new EvalException ($"Unknown token: {token}");
      }
   }
   #endregion

   #region implementation ----------------------------------------
   /// <summary>Resets the evaluator</summary>
   void Reset () {
      mOperands.Clear ();
      mOperators.Clear ();
      BasePriority = 0;
   }
   #endregion

   #region private data-------------------------------------------
   readonly Stack<double> mOperands = new ();
   readonly Stack<TOperator> mOperators = new ();
   readonly Dictionary<string, double> mVars = new ();
   #endregion
}
#endregion