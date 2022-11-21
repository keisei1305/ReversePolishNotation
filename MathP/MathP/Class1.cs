using System;
using System.Collections.Generic;

namespace MathP
{
    public static class MathP
    {
        static private List<string> _globalOperators = new List<string>(new string[]
        {
            "(", ")", "+", "-", "*", "/", "^"
        });
        static private List<string> _biOperators = new List<string>(new string[]
        {
            "+", "-", "*", "/", "^"
        });
        static private List<string> _prefixFunction = new List<string>(new string[]
        {
            "sin", "cos", "tg", "ctg",
            "arcsin", "arccos", "arctg", "arcctg",
            "--"
        });
        static private List<Func<double, double, double>> _operationResult = new List<Func<double, double, double>>(new Func<double, double, double>[] {
            null,(x,y)=>x+y,(x,y)=>x-y, (x,y)=>x*y, (x,y)=>x/y, (x,y)=>Math.Pow(x,y)
        });
        static private List<Func<double, double>> _functionResult = new List<Func<double, double>>(new Func<double, double>[]
        {
            null,
            x => Math.Sin(x),
            x => Math.Cos(x),
            x => Math.Tan(x),
            x => Math.Pow(Math.Tan(x), -1) ,
            x => Math.Asin(x),
            x => Math.Acos(x),
            x => Math.Atan(x),
            x => (Math.PI/2) -Math.Atan(x),
            x => Math.Log(x),
            x => -x
        });

        static public double Parse(string[] s)
        {
            Stack<double> Operands = new Stack<double>();
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i][0] - 48 < 10 && s[i][0] - 48 >= 0) Operands.Push(Convert.ToDouble(s[i]));
                else if (_biOperators.Contains(s[i]))
                {
                    if (Operands.Count > 1)
                    {
                        double operand2 = Operands.Pop();
                        double operand1 = Operands.Pop();
                        int numFunction = GetNumOperator(s[i][0]);
                        Operands.Push(_operationResult[numFunction](operand1, operand2));
                    }
                    else if (s[i] == "-")
                    {
                        Operands.Push(-Operands.Pop());
                    }
                    else if (s[i] == "+") continue;
                    else throw new ArgumentException();
                }
                else if (_prefixFunction.Contains(s[i]))
                {
                    double operand = Operands.Pop();
                    int numFunction = GetNumFunction(s[i]);
                    Operands.Push(_functionResult[numFunction](operand));
                }
            }
            return Operands.Pop();
        }

        static public double Parse(string[] s, double x)
        {
            Stack<double> Operands = new Stack<double>();
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i][0] - 48 < 10 && s[i][0] - 48 >= 0) Operands.Push(Convert.ToDouble(s[i]));
                else if (s[i] == "x") Operands.Push(x);
                else if (_biOperators.Contains(s[i]))
                {
                    if (Operands.Count > 1)
                    {
                        double operand2 = Operands.Pop();
                        double operand1 = Operands.Pop();
                        int numFunction = GetNumOperator(s[i][0]);
                        Operands.Push(_operationResult[numFunction](operand1, operand2));
                    }
                    else if (s[i] == "-")
                    {
                        Operands.Push(-Operands.Pop());
                    }
                    else if (s[i] == "+") continue;
                    else throw new ArgumentException();
                }
                else if (_prefixFunction.Contains(s[i]))
                {
                    double operand = Operands.Pop();
                    int numFunction = GetNumFunction(s[i]);
                    Operands.Push(_functionResult[numFunction](operand));
                }
            }
            return Operands.Pop();
        }

        static public double Parse(string s, bool isPoland = true)
        {
            if (isPoland)
            {
                string[] S = Split(s);
                return Parse(S);
            }
            else
            {
                string[] S = Split(s);
                S = MathP.Polska(S);
                return Parse(S);
            }
        }

        static public double Parse(string s, double x, bool isPoland = true)
        {
            if (isPoland)
            {
                string[] S = Split(s);
                return Parse(S,x);
            }
            else
            {
                string[] S = Split(s);
                S = MathP.Polska(S);
                return Parse(S,x);
            }
        }

        static public string[] Polska(string s)
        {
            string[] S = Split(s);
            return Polska(S);
        }

        static public string[] Polska(string[] s)
        {
            List<string> vs = new List<string>();
            Stack<string> Operators = new Stack<string>();
            for (int i = 0; i < s.Length; i++)
            {
                if ((s[i][0] - 48 >= 0 && s[i][0] - 48 < 10) || s[i] == "x") vs.Add(s[i]);
                else if (_biOperators.Contains(s[i]))
                {
                    while (Operators.Count != 0 && (_prefixFunction.Contains("" + Operators.Peek()) == true || GetPrioritet(Convert.ToChar(Operators.Peek())) >= GetPrioritet(Convert.ToChar(s[i]))) && Operators.Peek() != "(")
                    {
                        vs.Add(Operators.Pop());
                    }
                    Operators.Push(s[i]);
                }
                else if (s[i] == "(") Operators.Push(s[i]);
                else if (s[i] == ")")
                {
                    while (Operators.Peek() != "(") vs.Add(Operators.Pop());
                    Operators.Pop();
                }
                else if (_prefixFunction.Contains(s[i])) Operators.Push(s[i]);
            }
            while (Operators.Count != 0) vs.Add(Operators.Pop());
            string[] vs1 = new string[vs.Count];
            vs.CopyTo(vs1, 0);
            return vs1;
        }

        static public string[] Split(string s)
        {
            List<string> vs = new List<string>();
            for (int i = 0; i < s.Length; i++)
            {
                if ((s[i] - 48 < 10 && s[i] - 48 >= 0) || s[i] == ',')
                {
                    vs.Add("");
                    while ((s[i] - 48 < 10 && s[i] - 48 >= 0) || s[i] == ',')
                    {
                        vs[vs.Count - 1] += s[i];
                        i++;
                        if (i == s.Length)
                        {
                            string[] vss = new string[vs.Count];
                            vs.CopyTo(vss, 0);
                            return vss;
                        }
                    }
                }
                if (_globalOperators.Contains(Convert.ToString(s[i])) == true)
                {
                    if ((s[i] == '-' && i == 0) || (s[i] == '-' && (s[i - 1] - 48 >= 10 || s[i - 1] - 48 < 0) && s[i - 1] != 'x'))
                    {
                        vs.Add("--");
                        continue;
                    }
                    vs.Add("" + s[i]);
                }
                else if (s[i] == 'c' && s[i + 1] == 'o')
                {
                    i += 2;
                    vs.Add("cos");
                }
                else if (s[i] == 's')
                {
                    i += 2;
                    vs.Add("sin");
                }
                else if (s[i] == 't')
                {
                    i += 1;
                    vs.Add("tg");
                }
                else if (s[i] == 'c' && s[i] == 't')
                {
                    i += 2;
                    vs.Add("ctg");
                }
                else if (s[i] == 'a' && s[i + 3] == 's')
                {
                    i += 5;
                    vs.Add("arcsin");
                }
                else if (s[i] == 'a' && s[i + 4] == 'o')
                {
                    i += 5;
                    vs.Add("arccos");
                }
                else if (s[i] == 'a' && s[i + 3] == 't')
                {
                    i += 4;
                    vs.Add("arctg");
                }
                else if (s[i] == 'a' && s[i + 4] == 't')
                {
                    i += 5;
                    vs.Add("arcctg");
                }
                else if (s[i] == 'l')
                {
                    i++;
                    vs.Add("ln");
                }
                else if (s[i] == 'x') vs.Add("x");
            }
            string[] vs1 = new string[vs.Count];
            vs.CopyTo(vs1, 0);
            return vs1;
        }

        static private int GetPrioritet(char s)
        {
            switch (s)
            {
                case '(': return 100;
                case ')': return 100;
                case '+': return 1;
                case '-': return 1;
                case '*': return 2;
                case '/': return 2;
                case '^': return 3;
                default: throw new ArgumentException();
            }
        }

        static private int GetNumOperator(char s)
        {
            switch (s)
            {
                case '(': return 0;
                case ')': return 0;
                case '+': return 1;
                case '-': return 2;
                case '*': return 3;
                case '/': return 4;
                case '^': return 5;
                default: throw new ArgumentException();
            }
        }

        static private int GetNumFunction(string s)
        {
            switch (s)
            {
                case "sin": return 1;
                case "cos": return 2;
                case "tg": return 3;
                case "ctg": return 4;
                case "arcsin": return 5;
                case "arccos": return 6;
                case "arctg": return 7;
                case "arcctg": return 8;
                case "ln": return 9;
                case "--": return 10;
                default: throw new ArgumentException();
            }
        }
    }
}
