using System;

namespace LAB1_Compiladores
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo keyinfo;
            do
            {
                PrintHeader();
                Console.WriteLine("Presione [F5] para ingresar una expresión manualmente");
                Console.WriteLine("Presione [F6] para ver los ejemplos cargados");
                keyinfo = Console.ReadKey();

                string exp = "!";
                if (keyinfo.Key == ConsoleKey.F5)
                {
                    Console.Clear();
                    PrintHeader();
                    Console.WriteLine("Ingrese la expresión aritmética");
                    exp = Console.ReadLine();
                }
                else if (keyinfo.Key == ConsoleKey.F6)
                {
                    Console.Clear();
                    PrintHeader();
                    Console.WriteLine("Seleccione una opción");
                    Console.WriteLine("[1] -2+8*4/(5-3)");
                    Console.WriteLine("[2] 5+20/10*5-5/5");
                    Console.WriteLine("[3] -5-6+30/10*2*6");
                    Console.WriteLine("[4] -50/10/5*10");
                    Console.WriteLine("[5] (-5*3/5)-5/5");
                    keyinfo = Console.ReadKey();
                    if (keyinfo.Key == ConsoleKey.D1) exp = "-2+8*4/(5-3)";
                    if (keyinfo.Key == ConsoleKey.D2) exp = "5+20/10*5-5/5";
                    if (keyinfo.Key == ConsoleKey.D3) exp = "-5-6+30/10*2*6";
                    if (keyinfo.Key == ConsoleKey.D4) exp = "-50/10/5*10";
                    if (keyinfo.Key == ConsoleKey.D5) exp = "(-5*3/5)-5/5";
                    Console.WriteLine("\n"+exp);
                }

                if (exp != "!")
                {
                    Parser p = new Parser();
                    if (p.ValidateExpression(exp))
                    {
                        Console.WriteLine("\t\t\t\t\t  --- EXPRESIÓN VÁLIDA ---");
                        Console.WriteLine($"\t\t\t\t\t       RESULTADO = {p.Result}\n");
                    }
                    else
                    {
                        Console.WriteLine("\t\t\t\t\t  --- EXPRESIÓN INVÁLIDA ---\n");
                        Console.WriteLine("\t\t\t\t\t    " + p.ErrorMessage + "\n");
                    }
                    Console.WriteLine(p.Log);
                }
                else
                {
                    Console.WriteLine("\nNo seleccionó una opción válida");
                }

                Console.WriteLine("Presione [Esc] para salir, para repetir presione cualquier otra tecla");
                keyinfo = Console.ReadKey();
                Console.Clear();
            }
            while (keyinfo.Key != ConsoleKey.Escape);             
            return;
        }

        public static void PrintHeader()
        {
            Console.WriteLine("-------------------------------------------------- LAB01 --------------------------------------------------");
            Console.WriteLine("                                       José Vinicio De León Jiménez");
            Console.WriteLine("                                                  1027169");
        }
    }
}
