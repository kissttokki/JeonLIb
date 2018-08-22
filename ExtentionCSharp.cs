using System;
using System.Collections.Generic;
using System.Text;

namespace JeonLib.Extention.CSharp
{
    public static class ExtentionCSharp
    {
        public static void ShowMe(this object obj,bool endline =true)
        {
            if (endline)
                Console.WriteLine(obj);
            else
                Console.Write(obj);
        }
    }
}
