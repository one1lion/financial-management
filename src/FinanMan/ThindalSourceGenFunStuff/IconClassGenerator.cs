using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace ThindalSourceGenFunStuff
{
    [Generator]
    public class IconClassGenerator : ISourceGenerator
    {
        private const string _iconIdentifierAttribute = "ThindalIconAttribute";

        public void Execute(GeneratorExecutionContext context)
        {
            //throw new System.NotImplementedException();
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // Register a factory that can create our custom syntax receiver
            context.RegisterForSyntaxNotifications(() => new EventLogEntrySyntaxReceiver());
        }

        private class EventLogEntrySyntaxReceiver : ISyntaxReceiver
        {
            public List<AttributeSyntax> CandidateAttributes { get; } = new List<AttributeSyntax>();
            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                // any attribute with name "EventLogEntryAttribute" is a candidate for property generation
                if (syntaxNode is AttributeSyntax attributeSyntax
                    && attributeSyntax.Name is IdentifierNameSyntax identifierNameSyntax
                    && identifierNameSyntax.Identifier.ValueText == _iconIdentifierAttribute)
                {
                    CandidateAttributes.Add(attributeSyntax);
                }
            }
        }
    }
}
