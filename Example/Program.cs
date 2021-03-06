﻿using Pixelbyte.CodeGen;
using System;
using System.Collections.Generic;
using System.IO;

namespace Template_parser
{
    class Nid
    {
        public string name;
        public int id;

        public Nid(string n, int i)
        {
            name = n;
            id = i;
        }
    }

    static class Program
    {
        //Templates
        const string testMethodsUsingSimplates = @"//
//This file was auto-generated by Pixelbyte Studios
//Don't bother messing with it as it can/will change
//
namespace Simplate.Test
{
    {{ColorClass()}}
    {{CarClass()}}
}";

        const string constTemplate = @"//
//This file was auto-generated by Simplate
//Your changes will be changed. You have been warned...
//
public static class {{ className }}
{
    {{foreach item in items}}
    public const {{itemType}} {{ pascal(item) }} = ""{{ item }}"";
    {{end foreach}}

    Great!
}";

        const string testPropAccessTemplate = @"//
public static class TestPropAccess
{
    //Let's try a comment here
    {{foreach item in testItems}}
    public const {{itemType}} {{ pascal({{item.name}}) }} = {{item.id}};
    public const string {{ pascal({{item.name}}) }}_s = ""{{item.id}}"";
    {{Turkey()}}
    {{Turkey()}}
    {{end foreach}}
}";

        public const string testEmptyEnd = @"
Itsa me mario!";
        public const string testIf = @"
//Hello there
{{if diamond}}
We have diamonds!
Somewhere...
{{else}}
No Diamonds here..
Too bad!
{{end if}}
ok that was fun";

        public const string testIfElse = @"
Hello there.
{{if name}}
    Ah your name is {{name}}
{{else}}
    No name then?
{{end if}}
Mergdorious!";

        static Functors functors;
        static Simplate constSimplate;
        static void Main()
        {

            var items = new string[] { "green", "blue", "red", "black", "elvis" };

            constSimplate = Simplate.Compile(constTemplate);

            //setup a global functors for all our tests
            functors = new Functors();
            functors.Add("camel", StringExtensions.ToCamelCase);
            functors.Add("pascal", StringExtensions.ToPascalCase);
            functors.Add("Turkey", new Func<string>(Turkey));
            functors.Add("ColorClass", new Func<string>(ColorClass));
            functors.Add("CarClass", new Func<string>(CarClass));

            //Test accessing properties/fields within foreach
            var nids = new Nid[] { new Nid("me", 1), new Nid("jamerson", 22), new Nid("testability", 147) };
            var par = new Dictionary<string, object>();
            par.Add("itemType", "int");
            par.Add("testItems", nids);
            Simplate fe = Simplate.Compile(testPropAccessTemplate);
            string feout = fe.GetOutput(par, functors);
            Console.WriteLine(feout);

            //Test methods using Simplates
            var testMethods = Simplate.Compile(testMethodsUsingSimplates);
            var tm = Simplate.Compile(testEmptyEnd);

            //var ifSimplate = Simplate.Compile(testIf);
            //par.Add("diamond", false);
            //Console.WriteLine(ifSimplate.GetOutput(par, functors));

            par.Add("name", "turkey");
            var ifElseSimplate = Simplate.Compile(testIfElse);
            Console.WriteLine(ifElseSimplate.GetOutput(par, functors));

            //using (StreamWriter sw = new StreamWriter(@"E:\test.txt"))
            //{
            //    sw.Write(testMethods.GetOutput(null, functors));
            //}
            //Console.WriteLine(testMethods.GetOutput(null, functors));
        }

        static string ColorClass()
        {
            var para = new Dictionary<string, object>();
            string[] items = new string[] { "red", "green", "blue", "black", "brown" };
            para.Add("items", items);
            para.Add("itemType", "string");
            para.Add("className", "Colors");

            var outp = constSimplate.GetOutput(para, functors);
            return outp;
        }

        static string CarClass()
        {
            var para = new Dictionary<string, object>();
            string[] items = new string[] { "ford", "chevrolet", "chrysler", "honda", "hyundai" };
            para.Add("items", items);
            para.Add("itemType", "string");
            para.Add("className", "Cars");

            var outp = constSimplate.GetOutput(para, functors);
            return outp;
        }

        static string Turkey() { return "Gobble gobble!"; }

        const string tinyTemplate = @"//TODO: Mojo Here
    {{ item }}
";
        public static string TinyTemplate(string input)
        {
            var tinyTempl = Simplate.Compile(tinyTemplate);

            var functors = new Functors();
            functors.Add("camel", StringExtensions.ToCamelCase);
            functors.Add("pascal", StringExtensions.ToPascalCase);

            var items = new string[] { "green", "blue", "red", "black", "elvis" };
            var para = new Dictionary<string, object>() { { "item", input } };
            return tinyTempl.GetOutput(para, null);
        }
    }
}
