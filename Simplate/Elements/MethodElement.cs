using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pixelbyte.Simplate
{
    public class MethodElement : ITemplateElement
    {
        string methodName;
        string indent;
        ITemplateElement[] methodParams;

        public MethodElement(TokenInfo ti)
        {
            methodName = ti.name;
            indent = ti.indentation;
            methodParams = ti.parameters;

            //Console.WriteLine("{0} '{1}' {2} ",methodName, indent, indent.Length);
            if (string.IsNullOrEmpty(methodName))
                throw new NullReferenceException("methodName cannot be null!");
        }

        public string GetOutput(Dictionary<string, object> parameters, Functors functors)
        {
            string result = string.Empty;

            if (methodParams == null || methodParams.Length == 0)
                result = functors.Call(methodName);
            else
            {
                string[] stringParams = new string[methodParams.Length];
                for (int i = 0; i < methodParams.Length; i++)
                {
                    stringParams[i] = methodParams[i].GetOutput(parameters, functors);
                }

                //Now that we have transformed the Template Elements into strings, use them as the params
                result = functors.Call(methodName, stringParams);
            }

            //Now if this method is indented, then indent all the lines the function call above produced by the set indentation
            if (string.IsNullOrEmpty(indent)) return result;

            StringBuilder sb = new StringBuilder();
            //int lines = 0;
            using (var sr = new StringReader(result))
            {
                string txt = string.Empty;
                while ((txt = sr.ReadLine()) != null)
                {
                    sb.Append(indent);
                    sb.Append(txt);
                    sb.AppendLine();
                    //lines++;
                }

                //no need to append the indent to the first line
                sb.Remove(0, indent.Length);

                //Remove the last newline as it will be added outside the method call
                sb.Remove(sb.Length - 2, 2);
            }
            return sb.ToString();
        }
    }
}
