using System;
using System.Collections.Generic;

namespace Pixelbyte.Simplate
{
    public enum EndBlockType { ForEach, If }

    public class EndBlockElement : ITemplateElement
    {
        public EndBlockType type;

        public EndBlockElement(EndBlockType blockType) { type = blockType; }
        public EndBlockElement(string name) { switch(name.ToLower()) { case "foreach": type = EndBlockType.ForEach; break; case "if": type = EndBlockType.If; break; default: throw new Exception("Invalid end block type!"); } }

        public string GetOutput(Dictionary<string, object> parameters, Functors functors)
        {
            throw new NotImplementedException();
        }
    }
}
