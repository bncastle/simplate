using System;
using System.Collections.Generic;

namespace Pixelbyte.CodeGen
{
    public class MethodElement : ITemplateElement
    {
        string methodName;
        ITemplateElement[] methodParams;

        public MethodElement(string methodName, params ITemplateElement[] methodParameters)
        {
            if (string.IsNullOrEmpty(methodName))
                throw new NullReferenceException("methodName cannot be null!");
            this.methodName = methodName;

            methodParams = methodParameters;
        }

        public string GetOutput(Dictionary<string, object> parameters, Functors functors)
        {
            if (methodParams == null || methodParams.Length == 0)
                return functors.Call(methodName);
            else
            {
                string[] stringParams = new string[methodParams.Length];
                for (int i = 0; i < methodParams.Length; i++)
                {
                    stringParams[i] = methodParams[i].GetOutput(parameters, functors);
                }

                //Now that we have transformed the Template Elements into strings, use them as the params
                return functors.Call(methodName, stringParams);

            }
        }
    }
}
