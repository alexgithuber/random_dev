using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System;

public class ListHelper
{
    public static void WriteLine<T>(List<T> list, [CallerArgumentExpression("list")] string listStr = "") {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(listStr);
        Console.WriteLine("Elements: {0}", list.Count);
        Console.ResetColor();
        list.ForEach(o => Console.WriteLine(o));

        Console.WriteLine(listStr);

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Elements: {0}", list.Count);
        Console.WriteLine("------- end --------");
        Console.ResetColor();




    } 
}
