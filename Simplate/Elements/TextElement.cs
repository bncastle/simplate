using System;
using System.Collections.Generic;

namespace Pixelbyte.Simplate
{
    public class TextElement : ITemplateElement
    {
        /// <summary>
        /// Holds the plain text of this element, or base variable name if this
        /// element is a variable
        /// </summary>
        string name;

        /// <summary>
        /// If this element is a variable and the . notation is used to access
        /// properties or fields, this array contains those access names
        /// </summary>
        string[] specifiers;

        //True if the text is a variable to be found in the parameters dictionary
        bool isVariable;

        public TextElement(string txt, bool isVariable = false)
        {
            this.isVariable = isVariable;

            if (!isVariable)
                name = txt;
            else
                name = GetBaseObjectName(txt);
        }

        string GetBaseObjectName(string name)
        {
            string[] parts = name.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1)
            {
                specifiers = new string[parts.Length - 1];
                Array.Copy(parts, 1, specifiers, 0, parts.Length - 1);
            }

            return parts[0];
        }

        static object GetPropertyOrField(object obj, string name)
        {
            var t = obj.GetType();
            var fi = t.GetField(name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance);
            if (fi != null)
            {
                return fi.GetValue(obj);
            }

            //Or is it a property?
            var pi = t.GetProperty(name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance);
            if (pi != null)
            {
                return pi.GetValue(obj, null);
            }

            //otherwise, just return the object for now
            return obj;
        }

        /// <summary>
        /// This uses reflection to get value from any . calls on this variable
        /// </summary>
        /// <returns></returns>
        string GetStringFromSpecifiers(object original)
        {
            if (!isVariable) return name;
            else if (specifiers == null || specifiers.Length == 0) return original.ToString();

            object obj = original;
            for (int i = 0; i < specifiers.Length; i++)
            {
                obj = GetPropertyOrField(obj, specifiers[i]);
            }
            return obj.ToString();
        }

        public string GetOutput(Dictionary<string, object> parameters, Functors functors)
        {
            if (isVariable)
            {
                object data = null;
                if (!parameters.TryGetValue(name, out data))
                    throw new Exception("Unable to find: " + name + " in the parameters!");
                return GetStringFromSpecifiers(data);
            }
            else
                return name;
        }
    }
}
