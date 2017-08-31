using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Pixelbyte.CodeGen
{
    //https://stackoverflow.com/questions/28827705/c-sharp-creating-an-expression-in-runtime-with-a-string
    class IfElement : ITemplateElement
    {
        //string variableName;

        public string GetOutput(Dictionary<string, object> parameters, Functors functors)
        {
            //object data;
            //if (!parameters.TryGetValue(variableName, out data))
            //    throw new Exception("Unable to find: " + variableName + " in the parameters!");

            //Type type = typeof(bool);
            //var p = Expression.Parameter(type, "Variable");
            //var field = Expression.PropertyOrField(p, variableName);
            //Expression.
            //Expression.Condition
            throw new NotImplementedException();
            //Expression.Equal()
            //Expression.Lambda()
            //System.Linq.Expressions.LambdaExpression le = new System.Linq.Expressions.LambdaExpression(
        }
    }
}
