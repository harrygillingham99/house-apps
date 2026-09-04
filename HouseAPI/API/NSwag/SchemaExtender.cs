using House.Objects.Attributes;
using House.Objects.Objects;

namespace House.API.NSwag
{
    using System;
    using System.Linq;
    using System.Reflection;
    using API;
    using global::NSwag.Generation.Processors;
    using global::NSwag.Generation.Processors.Contexts;
    using HLL.ServerStats;
    using SignalR;

    /*
     * NSwag Schema Post-Processor to add any classes with the [NSwagInclude] attribute
     * to the schema
     */
    [ScrutorIgnore]
    public class SchemaExtenderDocumentProcessor : IDocumentProcessor
    {
        private const string NamespaceIdentifier = "House.";
        private readonly Type[] _typesToLoadAssembliesOf = { typeof(Startup), typeof(ProcessInfo), typeof(AppHub), typeof(WallboardInfo) };

        public void Process(DocumentProcessorContext context)
        {
            //Only load specific assemblies
            var assemblies = _typesToLoadAssembliesOf.Select(x => x.GetTypeInfo().Assembly);

            //Merge the lists of assemblies and check for any types with the [NSwagInclude] attribute
            var types = assemblies.SelectMany(x => x.ExportedTypes).Where(type =>
                !string.IsNullOrWhiteSpace(type.FullName) && type.FullName.StartsWith(NamespaceIdentifier) &&
                type.GetTypeInfo().CustomAttributes.Any(x => x.AttributeType == typeof(NSwagIncludeAttribute)));

            //Add the types to the schema
            foreach (var type in types)
                if (!context.SchemaResolver.HasSchema(type, type.IsEnum))
                    context.SchemaGenerator.Generate(type, context.SchemaResolver);
        }
    }
}