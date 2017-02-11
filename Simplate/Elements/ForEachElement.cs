using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pixelbyte.Simplate
{
    public class ForEachElement : ITemplateElement
    {
        List<ITemplateElement> elements;

        /// <summary>
        /// The name of the collection that we will grab from the parameters
        /// </summary>
        string collectionName;

        /// <summary>
        /// The name of the variable to which each item of the collection is assigned
        /// </summary>
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
            if (collection == null)
                throw new Exception("Item with key: " + collectionName + "must be iterable!");

            var sb = new StringBuilder();
            foreach (var item in collection)
            {
                parameters[variableName] = item;

                foreach (var element in elements)
                {
                    sb.Append(element.GetOutput(parameters, functors));
                }
            }
            //Remove the data we added
            parameters[variableName] = null;

            return sb.ToString();
        }
    }
}