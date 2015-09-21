using Pixelbyte.Simplate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template_parser
{
    static class Program
    {
        static void Main()
        {
            string txt = @"
//Comments
public class NegaSpace
{
	public void Mist(int arg)
	{
		{{foreach item in items}}
        {{test(pascal(item))}}
        {{end foreach}}
	}
    //Take out those stinking {{pascal(freedomFighters!)}} Now!
}";
            Simplate tk = Simplate.Compile(txt);

            var items = new string[] { "green", "blue", "red", "black", "elvis" };
            var functors = new Functors();
            functors.Add("camel", new Func<string, string>(StringExtensions.ToCamelCase));
            functors.Add("pascal", new Func<string, string>(StringExtensions.ToPascalCase));
            functors.Add("test", new Func<string, string>(Program.Test));
            var para = new Dictionary<string, object>();
            para.Add("items", items);
            Console.WriteLine(tk.GetOutput(para, functors));

            using (StreamWriter sw = new StreamWriter(@"E:\test.txt"))
            {
                sw.Write(tk.GetOutput(para, functors));
            }
        }

        public static string Test(string input)
        {
            return input + "-+=";
        }
    }
}
