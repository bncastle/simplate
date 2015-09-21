using System;
using System.Collections.Generic;

namespace Pixelbyte.Simplate
{
    /// <summary>
    /// mmm functors. mmm good they are
    /// in it functions insert. use ones you want, mmm yes
    /// string parameters only must they take. return a string also they must
    /// </summary>
    public class Functors
    {
        Dictionary<string, Delegate> table;

        public Functors()
        {
            table = new Dictionary<string, Delegate>();
        }

        public void Add(string key, Delegate d)
        {
            if (d.Method.ReturnType != typeof(string))
                throw new ArgumentException("delegate must return a string!");
            if (string.IsNullOrEmpty(key)) return;
            table.Add(key, d);
        }

        public string Call(string methodName, params object[] parameters)
        {
            Delegate del = null;
            if (table.TryGetValue(methodName, out del))
            {
                if (parameters.Length != del.Method.GetParameters().Length)
                    throw new Exception("Method [ " + methodName + " ]: number of parameters do not match!");
                return del.DynamicInvoke(parameters) as string;
            }
            else
                throw new KeyNotFoundException("Couldn't find the method called: " + methodName + " in the functors table!!");
        }
    }
}
