# simplate
A very simple WIP template system for C#

Right now simple functions can be called on items placed into the parameters dictionary and a few
command functions are available. It works well enough to process templates like:

```
public class {{classname}}
{
{{foreach item in items}}
    public string {{item}} = {{aMethod(item)}};
{{end foreach}}
}
```

First you compile the template by calling Simplate.Compile(string template) or Simplate.Compile(Textreader template).
Then to get output, call GetOutput(parameters, functorsTable) where parameters is a Dictionary&lt;string, object> and functorsTable is of type Functors and it contains a list of methods you want ot be able to call from the template itself

Simple if/else statements also work (still preliminary):

```
Hello there.
{{if name}}
    Ah your name is {{name}}
{{else}}
    No name then?
{{end if}}
```

More documentation forthcoming.
