using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

#if DEBUGGENERATOR
using System.Diagnostics;
#endif

namespace LocalizationConstGenerator;

[Generator]
public class LocalizationConstClassGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
#if DEBUGGENERATOR
        if (!Debugger.IsAttached)
        {
            Debugger.Launch();
        }
#endif

        // Register a factory that can create our custom syntax receiver
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {

    }

    internal class SyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> CandidateClasses { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            // Check if the current node is the .resx Resource file
            //if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax &&
            //    classDeclarationSyntax.AttributeLists.Count > 0 &&
            //    classDeclarationSyntax.AttributeLists[0].Attributes.Count > 0 &&
            //    classDeclarationSyntax.AttributeLists[0].Attributes[0].Name.ToString() == "ResxResource")
            //{
            //    CandidateClasses.Add(classDeclarationSyntax);
            //}

        }
    }
}
