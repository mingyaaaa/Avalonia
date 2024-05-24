using System.Collections.Generic;
using System.Linq;
using Avalonia.Generators.Common.Domain;
using XamlX.TypeSystem;

namespace Avalonia.Generators.NameGenerator;

internal class OnlyPropertiesCodeGenerator : ICodeGenerator
{
    private string _generatorName = typeof(OnlyPropertiesCodeGenerator).FullName;
    private string _generatorVersion = typeof(OnlyPropertiesCodeGenerator).Assembly.GetName().Version.ToString();

    public string GenerateCode(string className, string nameSpace, IXamlType xamlType, IEnumerable<ResolvedName> names)
    {
        var namedControls = names
            .Select(info => "        " +
                            $"[global::System.CodeDom.Compiler.GeneratedCode(\"{_generatorName}\", \"{_generatorVersion}\")]\n" +
                            "        " +
                            $"{info.FieldModifier} {info.TypeName} {info.Name} => " +
                            $"this.FindNameScope()?.Find<{info.TypeName}>(\"{info.Name}\");")
            .ToList();
        var lines = string.Join("\n", namedControls);
        return $@"// <auto-generated />

using Avalonia.Controls;

namespace {nameSpace}
{{
    partial class {className}
    {{
{lines}
    }}
}}
";
    }
}