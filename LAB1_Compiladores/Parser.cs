using System;
using System.Collections.Generic;
using System.Text;

namespace LAB1_Compiladores
{
    public class Parser
    {
        private Dictionary<string, string> LRTable = new Dictionary<string, string>();
        private Scanner Scanner;
        private Token CurrentToken;
        private Stack<string> StatesStack = new Stack<string>();
        private Stack<string> PrincipalStack = new Stack<string>();
        private Stack<string> AuxiliaryStack = new Stack<string>();
        public string Result;
        private List<List<string>> Rules = new List<List<string>>();
        public string ErrorMessage;
        public string Log;

        public bool ValidateExpression(string exp)
        {
            try
            {
                Scanner = new Scanner(exp);
                CurrentToken = Scanner.GetToken();
                bool acc = false;
                while (!acc)
                {
                    string action = GetAction(StatesStack.Peek(), ConvertTokenTag(Convert.ToString(CurrentToken.Tag)));
                    if (action[0] == 'S')
                    {
                        StatesStack.Push(action.Substring(1));
                        PrincipalStack.Push(ConvertTokenTag(Convert.ToString(CurrentToken.Tag)));
                        SaveAux();
                        CurrentToken = Scanner.GetToken();
                        SaveStep(action);
                    }
                    else if (action[0] == 'R')
                    {
                        SaveStep(action);
                        List<string> rule = GetRule(action.Substring(1));
                        for (int i = rule.Count - 1; i > 0; i--)
                        {
                            if (rule[i] == PrincipalStack.Peek())
                            {
                                PrincipalStack.Pop();
                                StatesStack.Pop();
                            }
                            
                            //Si saca mas de dos elementos por una regla hacer operación, 
                            //De que con que? de lo que esta en el tope de la pila auxiliar, luego el resultado meterlo nuevamente a la pila auxiliar
                        }
                        UpdateAux(rule.Count - 1);
                        PrincipalStack.Push(Convert.ToString(rule[0][1]));                        
                        StatesStack.Push(GetAction(StatesStack.Peek(), PrincipalStack.Peek()));
                    }
                    else if (action == "ACCEPTED")
                    {
                        SaveStep(action);
                        Result = AuxiliaryStack.Pop();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                ErrorMessage = $"ERROR DE SINTAXIS: {SetErrorMessage()}";
                return false;
            }
        }
        public void SaveAux()
        {
            if(CurrentToken.Value != "(" && CurrentToken.Value != ")")
            {
                AuxiliaryStack.Push(CurrentToken.Value);
            }
            
        }
        public void UpdateAux(int count)
        {
            if (AuxiliaryStack.Count == 1) return;
            if (count == 1) return;
            else if(count == 2)
            {
                string num = AuxiliaryStack.Pop();
                string negative_complete = AuxiliaryStack.Pop() + num;
                AuxiliaryStack.Push(negative_complete);
            }
            else if(count >= 3)
            {
                double result = 0.0;
                double right_num = Convert.ToDouble(AuxiliaryStack.Pop());
                string operation = AuxiliaryStack.Pop();
                double left_num = Convert.ToDouble(AuxiliaryStack.Pop());
                switch (operation)
                {
                    case ("+"):
                        result = left_num + right_num;
                        AuxiliaryStack.Push(Convert.ToString(result));
                        break;
                    case ("-"):
                        result = left_num - right_num;
                        AuxiliaryStack.Push(Convert.ToString(result));
                        break;
                    case ("*"):
                        result = left_num * right_num;
                        AuxiliaryStack.Push(Convert.ToString(result));
                        break;
                    case ("/"):
                        result = left_num / right_num;
                        AuxiliaryStack.Push(Convert.ToString(result));
                        break;
                    default:
                        break;
                }
            }  
        }
        private void SaveStep(string action)
        {
            string st_stack = "";
            string[] states = new string[StatesStack.Count];
            StatesStack.CopyTo(states, 0);
            for (int i = states.Length-1; i >= 0; i--)
            {
                st_stack += states[i].PadRight(4, ' ');
            }

            string p_stack = "";
            string[] tokens = new string[PrincipalStack.Count];
            PrincipalStack.CopyTo(tokens, 0);
            for (int i = tokens.Length-1; i >= 0; i--)
            {
                p_stack += tokens[i].PadRight(4, ' ');
            }
            Log += st_stack.PadRight(50, ' ') +"|" +p_stack.PadRight(50, ' ') + "|" + action.PadRight(5, ' ')+"\n";
        }
        public string SetErrorMessage()
        {
            string result = ConvertTokenTag(Convert.ToString(CurrentToken.Tag));
            while (CurrentToken.Tag != TokenType.T_EOF)
            {
                CurrentToken = Scanner.GetToken();
                result += ConvertTokenTag(Convert.ToString(CurrentToken.Tag));
            };
            return result;
        }
        public string GetAction(string state, string tokenValue)
        {
            string key = "{" + state + "," + tokenValue + "}";
            return LRTable[key];
        }
        public List<string> GetRule(string rule)
        {
            int pos = Convert.ToInt32(rule);
            return Rules[pos];

        }
        public string ConvertTokenTag(string t)
        {
            if (t == "T_ADD") return "+";
            if (t == "T_SUB") return "-";
            if (t == "T_MUL") return "*";
            if (t == "T_DIV") return "/";
            if (t == "T_LPAREN") return "(";
            if (t == "T_RPAREN") return ")";
            if (t == "T_EOF") return "$";
            if (t == "T_INT") return "int";
            if (t == "T_UNKNOWN") return "?";
            return "";
        }
        public Parser()
        {
            //INITIAL VALUES
            PrincipalStack.Push("#");
            StatesStack.Push("0");
            Log += "".PadRight(115, '-') + "\n";
            Log += "STATES".PadRight(50, ' ') + "|" + "STACK".PadRight(50, ' ') + "|" + "ACTION".PadRight(5, ' ') + "\n";
            Log += "".PadRight(115, '-')+"\n";

            //------------------PRODUCTION RULES
            Rules.Add(new List<string>() { "{S'}" , "S" }); //Rule 0, not useful

            Rules.Add(new List<string>() { "{S}", "S", "+","T" });
            Rules.Add(new List<string>() { "{S}", "S", "-", "T" });
            Rules.Add(new List<string>() { "{S}", "T" });
            Rules.Add(new List<string>() { "{T}", "T", "/", "F" });
            Rules.Add(new List<string>() { "{T}", "T", "*", "F" });
            Rules.Add(new List<string>() { "{T}", "F" });
            Rules.Add(new List<string>() { "{F}", "(", "S", ")" });
            Rules.Add(new List<string>() { "{F}", "int"});
            Rules.Add(new List<string>() { "{F}", "-", "int" });

            //-------------------LR TABLE
            //-State 0
            LRTable.Add("{0,-}", "S6");
            LRTable.Add("{0,(}", "S4");
            LRTable.Add("{0,int}", "S5");
            LRTable.Add("{0,S}", "1");
            LRTable.Add("{0,T}", "2");
            LRTable.Add("{0,F}", "3");

            //-State 1
            LRTable.Add("{1,+}", "S7");
            LRTable.Add("{1,-}", "S8");
            LRTable.Add("{1,$}", "ACCEPTED");

            //-State 2
            LRTable.Add("{2,+}", "R3");
            LRTable.Add("{2,-}", "R3");
            LRTable.Add("{2,/}", "S9");
            LRTable.Add("{2,*}", "S10");
            LRTable.Add("{2,)}", "R3");
            LRTable.Add("{2,$}", "R3");


            //-State 3
            LRTable.Add("{3,+}", "R6");
            LRTable.Add("{3,-}", "R6");
            LRTable.Add("{3,/}", "R6");
            LRTable.Add("{3,*}", "R6");
            LRTable.Add("{3,)}", "R6");
            LRTable.Add("{3,$}", "R6");


            //-State 4
            LRTable.Add("{4,-}", "S6");
            LRTable.Add("{4,(}", "S4");
            LRTable.Add("{4,int}", "S5");
            LRTable.Add("{4,S}", "11");
            LRTable.Add("{4,T}", "2");
            LRTable.Add("{4,F}", "3");

            //-State 5
            LRTable.Add("{5,+}", "R8");
            LRTable.Add("{5,-}", "R8");
            LRTable.Add("{5,/}", "R8");
            LRTable.Add("{5,*}", "R8");
            LRTable.Add("{5,)}", "R8");
            LRTable.Add("{5,$}", "R8");

            //-State 6
            LRTable.Add("{6,int}", "S12");

            //-State 7
            LRTable.Add("{7,-}", "S6");
            LRTable.Add("{7,(}", "S4");
            LRTable.Add("{7,int}", "S5");
            LRTable.Add("{7,T}", "13");
            LRTable.Add("{7,F}", "3");

            //-State 8
            LRTable.Add("{8,-}", "S6");
            LRTable.Add("{8,(}", "S4");
            LRTable.Add("{8,int}", "S5");
            LRTable.Add("{8,T}", "14");
            LRTable.Add("{8,F}", "3");

            //-State 9
            LRTable.Add("{9,-}", "S6");
            LRTable.Add("{9,(}", "S4");
            LRTable.Add("{9,int}", "S5");
            LRTable.Add("{9,F}", "15");

            //-State 10
            LRTable.Add("{10,-}", "S6");
            LRTable.Add("{10,(}", "S4");
            LRTable.Add("{10,int}", "S5");
            LRTable.Add("{10,F}", "16");

            //-State 11
            LRTable.Add("{11,+}", "S7");
            LRTable.Add("{11,-}", "S8");
            LRTable.Add("{11,)}", "S17");

            //-State 12
            LRTable.Add("{12,+}", "R9");
            LRTable.Add("{12,-}", "R9");
            LRTable.Add("{12,/}", "R9");
            LRTable.Add("{12,*}", "R9");
            LRTable.Add("{12,)}", "R9");
            LRTable.Add("{12,$}", "R9");

            //-State 13
            LRTable.Add("{13,+}", "R1");
            LRTable.Add("{13,-}", "R1");
            LRTable.Add("{13,/}", "S9");
            LRTable.Add("{13,*}", "S10");
            LRTable.Add("{13,)}", "R1");
            LRTable.Add("{13,$}", "R1");

            //-State 14
            LRTable.Add("{14,+}", "R2");
            LRTable.Add("{14,-}", "R2");
            LRTable.Add("{14,/}", "S9");
            LRTable.Add("{14,*}", "S10");
            LRTable.Add("{14,)}", "R2");
            LRTable.Add("{14,$}", "R2");

            //-State 15
            LRTable.Add("{15,+}", "R4");
            LRTable.Add("{15,-}", "R4");
            LRTable.Add("{15,/}", "R4");
            LRTable.Add("{15,*}", "R4");
            LRTable.Add("{15,)}", "R4");
            LRTable.Add("{15,$}", "R4");

            //-State 16
            LRTable.Add("{16,+}", "R5");
            LRTable.Add("{16,-}", "R5");
            LRTable.Add("{16,/}", "R5");
            LRTable.Add("{16,*}", "R5");
            LRTable.Add("{16,)}", "R5");
            LRTable.Add("{16,$}", "R5");

            //-State 17
            LRTable.Add("{17,+}", "R7");
            LRTable.Add("{17,-}", "R7");
            LRTable.Add("{17,/}", "R7");
            LRTable.Add("{17,*}", "R7");
            LRTable.Add("{17,)}", "R7");
            LRTable.Add("{17,$}", "R7");

        }
    }
}
