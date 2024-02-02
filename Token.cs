namespace Eval;

#region Token --------------------------------------------------------------------------------
/// <summary>class Token</summary>
abstract class Token {
}

/// <summary>Token for number</summary>
abstract class TNumber : Token {
   public abstract double Value { get; }
}

/// <summary>Token for literal </summary>
class TLiteral : TNumber {
   public TLiteral (double f) => mValue = f;
   public override double Value => mValue;
   public override string ToString () => $"literal:{Value}";
   readonly double mValue;
}

/// <summary>Token for variable</summary>
class TVariable : TNumber {
   public TVariable (Evaluator eval, string name) => (Name, mEval) = (name, eval);
   public string Name { get; private set; }
   public override double Value => mEval.GetVariable (Name);
   public override string ToString () => $"var:{Name}";
   readonly Evaluator mEval;
}

/// <summary>Token for operator</summary>
abstract class TOperator : Token {
   protected TOperator (Evaluator eval) => mEval = eval;
   public int Priority { get; protected set; }
   readonly protected Evaluator mEval;
}

/// <summary>Token for binary operator</summary>
class TOpArithmetic : TOperator {
   public TOpArithmetic (Evaluator eval, char ch) : base (eval) {
      Op = ch;
      Priority = sPriority[Op] + mEval.BasePriority;
   }
   public char Op { get; set; }
   public override string ToString () => $"op:{Op}:{Priority}";
   static Dictionary<char, int> sPriority = new () {
      ['+'] = 1, ['-'] = 1, ['*'] = 2, ['/'] = 2, ['^'] = 3, ['='] = 4,
   };
   public double Evaluate (double a, double b) {
      return Op switch {
         '+' => a + b, '-' => a - b,
         '*' => a * b, '/' => a / b,
         '^' => Math.Pow (a, b),
         _ => throw new EvalException ($"Unknown Operator: {Op}"),
      };
   }
}

/// <summary>Token for unary operator</summary>
class TOpUnary : TOperator {
   public TOpUnary (Evaluator eval, char op) : base (eval) {
      Op = op;
      Priority = 6 + mEval.BasePriority;
   }
   public char Op { get; private set; }
   public override string ToString () => $"TUnary {Op}";
   public double Evaluate (double a) =>
       Op switch {'+' => a, '-' => -a,_ => throw new EvalException ("Unary Operator not Implemented")};
}

/// <summary>Token for function</summary>
class TOpFunction : TOperator {
   public TOpFunction (Evaluator eval, string name) : base (eval) {
      Func = name;
      Priority = 5 + mEval.BasePriority;
   }
   public string Func { get; private set; }
   public override string ToString () => $"func:{Func}:{Priority}";
   public double Evaluate (double f) {
      return Func switch {
         "sin" => Math.Sin (D2R (f)),
         "cos" => Math.Cos (D2R (f)),
         "tan" => Math.Tan (D2R (f)),
         "sqrt" => Math.Sqrt (f),
         "log" => Math.Log (f),
         "exp" => Math.Exp (f),
         "asin" => R2D (Math.Asin (f)),
         "acos" => R2D (Math.Acos (f)),
         "atan" => R2D (Math.Atan (f)),
         _ => throw new EvalException ($"Unknown function: {Func}")
      };

      double D2R (double f) => f * Math.PI / 180;
      double R2D (double f) => f * 180 / Math.PI;
   }
}

/// <summary>Token for punctuation</summary>
class TPunctuation : Token {
   public TPunctuation (char ch) => Punct = ch;
   public char Punct { get; private set; }
   public override string ToString () => $"punct:{Punct}";
}

/// <summary>Token of end</summary>
class TEnd : Token {
   public override string ToString () => "end";
}

/// <summary>Token of error</summary>
class TError : Token {
   public TError (string message) => Message = message;
   public string Message { get; private set; }
   public override string ToString () => $"error:{Message}";
}
#endregion