using System.Collections.Generic;

namespace Pixelbyte.Simplate
{
    public interface ITemplateElement
    {
        string GetOutput(Dictionary<string, object> parameters, Functors functors);
    }
}
