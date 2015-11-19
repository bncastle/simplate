using System.Collections.Generic;

namespace Pixelbyte.CodeGen
{
    public interface ITemplateElement
    {
        string GetOutput(Dictionary<string, object> parameters, Functors functors);
    }
}
