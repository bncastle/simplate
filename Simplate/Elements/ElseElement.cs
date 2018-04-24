using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Pixelbyte.CodeGen
{
    //https://stackoverflow.com/questions/28827705/c-sharp-creating-an-expression-in-runtime-with-a-string
    class ElseElement : ITemplateElement
    {
        List<ITemplateElement> statements;

        public ElseElement(List<ITemplateElement> statements)
        {
            this.statements = statements;
        }

        public string GetOutput(Dictionary<string, object> parameters, Functors functors)
        {
            return statements.GetOutput(parameters, functors);
        }
    }
}
