using System;
using System.Collections.Generic;

namespace Pixelbyte.Simplate
{
    public class TextElement : ITemplateElement
    {
        string name;

        //True if the text is a variable to be found in the parameters dictionary
        bool isVariable;

        public TextElement(string txt, bool isVariable = false)
        {
            name = txt;
            this.isVariable = isVariable;
        }

        public string GetOutput(Dictionary<string, object> parameters, Functors functors)
        {
            if (isVariable)
            {
                object data = null;
                if (!parameters.TryGetValue(name, out data))
                    throw new Exception("Unable to find: " + name + " in the parameters!");
                return data.ToString();
            }
            else
                return name;
        }
    }
}
