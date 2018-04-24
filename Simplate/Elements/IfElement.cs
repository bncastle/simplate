using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Pixelbyte.CodeGen
{
    //https://stackoverflow.com/questions/28827705/c-sharp-creating-an-expression-in-runtime-with-a-string
    class IfElement : ITemplateElement
    {
        string variableName;
        List<ITemplateElement> ifDo;
        ITemplateElement elseDo;

        public IfElement(string variableName, List<ITemplateElement> ifDo, ITemplateElement elseDo)
        {
            this.variableName = variableName;
            this.ifDo = ifDo;
            this.elseDo = elseDo;
        }

        public string GetOutput(Dictionary<string, object> parameters, Functors functors)
        {
            object data;
            if (!parameters.TryGetValue(variableName, out data))
            {
                //Currently, if the parameter does not exist, assume that means the if is FALSE
                //throw new Exception("Unable to find: " + variableName + " in the parameters!");
                data = null;
            }

            if (IsTrue(data))
            {
                return ifDo.GetOutput(parameters, functors);
            }
            else
            {
                if (elseDo != null)
                    return elseDo.GetOutput(parameters, functors);
                else
                    return string.Empty;
            }
        }

        bool IsTrue(object data)
        {
            if (data == null) return false;
            Type t = data.GetType();
            if (t == typeof(string)) { return !string.IsNullOrEmpty(data.ToString()); }
            else if (t.IsClass) return data != null;
            else if(t == typeof(bool)) { return (bool)data; }
            else return true;
        }
    }
}
