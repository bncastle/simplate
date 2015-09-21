using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pixelbyte.Simplate
{
    public class ForEachElement : ITemplateElement
    {
        List<ITemplateElement> elements;
        string collectionName;
        string variableName;

        public ForEachElement(string collectionName, string variableName, List<ITemplateElement> block)
        {
            this.collectionName = collectionName;
            this.variableName = variableName;
            elements = block;
        }

        public string GetOutput(Dictionary<string, object> parameters, Functors functors)
        {
            object data = null;
            if (!parameters.TryGetValue(collectionName, out data))
                throw new Exception("Unable to find: " + collectionName + " in the parameters!");

            ICollection collection = data as ICollection;
            if(collection == null)

            if (data is ICollection)
                    throw new Exception("Item with key: " + collectionName + "must be iterable!");

            var sb = new StringBuilder();
            //int index = 0;
            //var count = collection.Count;
            foreach (var item in collection)
            {
                parameters[variableName] = item;
                foreach (var element in elements)
                {
                    sb.Append(element.GetOutput(parameters, functors));
                }
            }
            //Put the original data back
            parameters[variableName] = null;

            return sb.ToString();
        }
    }
}
