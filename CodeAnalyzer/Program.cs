using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAnalyzer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var targetAssembly = "StackExchange.Redis";
            var totalCounters = new Counters();
            var projectFile = @"C:\panos\repos\CodeAnalyzer\TestClassLibrary1\TestClassLibrary1.csproj";
            
            MSBuildLocator.RegisterDefaults();
            using (var workspace = MSBuildWorkspace.Create())
            {
                var project = await workspace.OpenProjectAsync(projectFile);
                var compilation = await project.GetCompilationAsync();

                foreach (var document in project.Documents)
                {
                    Console.WriteLine($"  Document: {document.Name}");
                    var syntaxTree = await document.GetSyntaxTreeAsync();
                    var model = compilation.GetSemanticModel(syntaxTree, true);
                    var symbols = syntaxTree
                        .GetRoot()
                        .DescendantNodes()
                        .OfType<IdentifierNameSyntax>()
                        .Select(node => model.GetSymbolInfo(node).Symbol)
                        .Where(s => s?.ContainingAssembly?.Name == targetAssembly);
                    symbols
                        .OfType<INamedTypeSymbol>()
                        .Select(s => s.Name)
                        .GroupBy(s => s)
                        .ToList()
                        .ForEach(g => totalCounters.AddTypeReferences(document.Name, g.Key, g.Count()));
                    symbols
                        .OfType<IFieldSymbol>()
                        .Select(s => s.ContainingType.Name)
                        .GroupBy(s => s)
                        .ToList()
                        .ForEach(g => totalCounters.AddFieldReferences(document.Name, g.Key, g.Count()));
                    var unknownSymbols = symbols
                        .Where(s => !(s is INamedTypeSymbol || s is IFieldSymbol || s is INamespaceSymbol));
                    unknownSymbols.ToList().ForEach(s => Console.WriteLine($"    >>> Unknown:  {s}, {s.GetType().Name}"));

                    var documentCounters = totalCounters.GetCounters(document.Name);
                    foreach (var counter in documentCounters)
                        Console.WriteLine($"    * {counter.Name,-64} {counter.TypeRefs,3} {counter.FieldRefs,3}");
                }

                var counters = totalCounters.GetTotals();
                Console.WriteLine(new string('-', 77));
                Console.WriteLine($"{counters.Count()} entries found.");
                foreach (var counter in counters)
                    Console.WriteLine($"  * {counter.Name,-64} {counter.TypeRefs,3} {counter.FieldRefs,3}");
            }
        }
    }
}
