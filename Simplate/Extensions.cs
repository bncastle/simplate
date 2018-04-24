using System.Collections.Generic;
using System.Text;

namespace Pixelbyte.CodeGen
{
    internal static class Extensions
    {
        public static string GetOutput(this List<ITemplateElement> elements, Dictionary<string, object> parameters, Functors functors)
        {
            var sb = new StringBuilder();
            foreach (var element in elements)
            {
                sb.Append(element.GetOutput(parameters, functors));
            }
            return sb.ToString();
        }
    }
}
