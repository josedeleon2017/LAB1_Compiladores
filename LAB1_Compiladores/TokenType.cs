using System;
using System.Collections.Generic;
using System.Text;

namespace LAB1_Compiladores
{
    public enum TokenType
    {
        T_ADD = '+',
        T_SUB = '-',
        T_MUL = '*',
        T_DIV = '/',
        T_LPAREN = '(',
        T_RPAREN = ')',
        T_EOF = '$',
        T_INT = 'I',
        T_UNKNOWN = '?',
        T_INVERSE = 'E'
    }
}
