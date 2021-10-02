using System;
using System.Collections.Generic;
using System.Text;

namespace LAB1_Compiladores
{
    public class Scanner
    {
        private string _math_exp = "";
        private int _index = 0;
        private int _state = 0;
        public Scanner(string math_exp)
        {
            _math_exp = math_exp + (char)TokenType.T_EOF;
            _index = 0;
            _state = 0;
        }
        public Token GetToken()
        {
            Token result = new Token() { Value = "" };
            bool tokenFound = false;
            while (!tokenFound)
            {
                char peek = _math_exp[_index];
                switch (_state)
                {
                    case 0:
                        while (char.IsWhiteSpace(peek))
                        {
                            _index++;
                            peek = _math_exp[_index];
                        } 
                        
                        switch (peek)
                        {
                            case (char)TokenType.T_ADD:
                            case (char)TokenType.T_SUB:
                            case (char)TokenType.T_MUL:
                            case (char)TokenType.T_DIV:
                            case (char)TokenType.T_LPAREN:
                            case (char)TokenType.T_RPAREN:
                                tokenFound = true;
                                result.Tag = (TokenType)peek;
                                result.Value += peek;
                                break;
                            case (char)'0':
                            case (char)'1':
                            case (char)'2':
                            case (char)'3':
                            case (char)'4':
                            case (char)'5':
                            case (char)'6':
                            case (char)'7':
                            case (char)'8':
                            case (char)'9':
                                char exit = _math_exp[_index + 1];
                                if (exit == '0' || exit == '1' || exit == '2' || exit == '3' || exit == '4' || exit == '5' || exit == '6' || exit == '7' || exit == '8' || exit == '9')
                                {
                                    _state = 1;
                                    result.Value += peek;
                                }
                                else
                                {
                                    result.Tag = TokenType.T_INT;
                                    result.Value += peek;
                                    tokenFound = true;
                                }  
                                break;
                            case (char)TokenType.T_EOF:
                                result.Tag = TokenType.T_EOF;
                                tokenFound = true;
                                result.Value += '$';
                                break;
                            default:
                                result.Tag = TokenType.T_UNKNOWN;
                                tokenFound = true;
                                result.Value += peek;
                                break;
                        }
                        break;                    
                    case 1:
                        switch (peek)
                        {
                            case (char)'0':
                            case (char)'1':
                            case (char)'2':
                            case (char)'3':
                            case (char)'4':
                            case (char)'5':
                            case (char)'6':
                            case (char)'7':
                            case (char)'8':
                            case (char)'9':
                                char exit = _math_exp[_index+1];
                                if (exit == '0' || exit == '1' || exit == '2' || exit == '3' || exit == '4' || exit == '5' || exit == '6' || exit == '7' || exit == '8' || exit == '9')
                                {
                                    _state = 1;
                                    result.Value += peek;
                                }
                                else
                                {
                                    result.Tag = TokenType.T_INT;
                                    result.Value += peek;
                                    tokenFound = true;
                                }
                                break;
                            default:
                                result.Tag = TokenType.T_UNKNOWN;
                                tokenFound = true;
                                result.Value += peek;
                                break;
                        }
                        break;
                    default:
                        break;
                }
                _index++;
            }
            _state = 0;
            return result;          
        }
    }
}
