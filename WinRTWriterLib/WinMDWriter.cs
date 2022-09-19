using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WinRTWriterLib
{

    public class WinMDWriter : IWriter
    {

        private BlobHandle GetBlobHandle(string blob)
        {
            return m_builder.GetOrAddBlobUTF16(blob);
        }

        private StringHandle GetStringHandle(string assemblyName)
        {
            return m_builder.GetOrAddString(assemblyName);
        }

        public void EnterScope()
        {
            
        }

        public void ExitScope()
        {
            
        }

        MetadataBuilder m_builder = new();

        private static readonly Guid s_guid = new Guid("87D4DBE1-1143-4FAD-AAB3-1001F92068E6");
        private static readonly BlobContentId s_contentId = new BlobContentId(s_guid, 0x04030201);

        private static void WritePEImage(
    Stream peStream,
    MetadataBuilder metadataBuilder,
    BlobBuilder ilBuilder
    )
        {
            // Create executable with the managed metadata from the specified MetadataBuilder.
            var peHeaderBuilder = new PEHeaderBuilder(
                imageCharacteristics: Characteristics.ExecutableImage
                );

            var peBuilder = new ManagedPEBuilder(
                peHeaderBuilder,
                new MetadataRootBuilder(metadataBuilder),
                ilBuilder,
                //                entryPoint: entryPointHandle,
                flags: CorFlags.ILLibrary,
                deterministicIdProvider: content => s_contentId); ;

            // Write executable into the specified stream.
            var peBlob = new BlobBuilder();
            BlobContentId contentId = peBuilder.Serialize(peBlob);
            peBlob.WriteContentTo(peStream);
        }

        private static BlobBuilder BuildSignature(Action<BlobEncoder> action)
        {
            var builder = new BlobBuilder();
            action(new BlobEncoder(builder));
            return builder;
        }

        public void Write(IEnumerable<WinRTNamespace> namespaces)
        {
            using var peStream = new FileStream("Test123.winmd", FileMode.OpenOrCreate, FileAccess.ReadWrite
    );
            var assemblyName = namespaces.First().AssemblyName;
            var asm = m_builder.AddAssembly(GetStringHandle(assemblyName), new Version(255, 255, 0, 0), default,
                default, AssemblyFlags.WindowsRuntime, AssemblyHashAlgorithm.None);

            var ilBuilder = new BlobBuilder();
            var module = m_builder.AddModule(0, GetStringHandle("UI"), m_builder.GetOrAddGuid(Guid.NewGuid()), default, default);

            m_builder.AddTypeDefinition(
                default,
                default,
                GetStringHandle("<Module>"),
                default,
                MetadataTokens.FieldDefinitionHandle(1),
                MetadataTokens.MethodDefinitionHandle(1));

            m_builder.AddAssemblyReference(GetStringHandle("mscorlib"), new Version(4, 0), default, default, default, default);

            foreach (var ns in namespaces.Select(x => x.GetTopLevel() as WinRTNamespace).Distinct())
                EmitNamespace(ns!);

            WritePEImage(peStream, m_builder, ilBuilder);

        }

        private void EmitNamespace(WinRTNamespace ns)
        {
            foreach (var entity in ns.Members)
            {
                if (entity is WinRTNamespace n)
                    EmitNamespace(n);
                else if (entity is WinRTEnum e)
                    EmitEnum(ns, e);
                else if (entity is WinRTInterface i)
                    EmitInterface(ns, i);
                else
                {
                 //   throw new NotImplementedException();
                }
            }
        }

        private MethodDefinitionHandle EmitMethod(WinRTMethod m)
        {
            ParameterHandle[] parameterHandles = new ParameterHandle[m.Arguments.Count + 1];
            parameterHandles[0] = m_builder.AddParameter(ParameterAttributes.Retval, GetStringHandle("retval"), 0);

            var i = 1;
            foreach (var a in m.Arguments)
            {
                parameterHandles[i++] = m_builder.AddParameter(ParameterAttributes.In, GetStringHandle(a.Name!), i);
            }

            var signature = new BlobBuilder();
            new BlobEncoder(signature)
                .MethodSignature(SignatureCallingConvention.Default, 0, !m.IsStatic)
                .Parameters(
                parameterHandles.Length,
                _ => EncodeReturnType(_, m.ReturnType),
                _ => EncodeParameters(_, m.Arguments, parameterHandles)
                );

            var mdh = m_builder.AddMethodDefinition(MethodAttributes.Abstract | MethodAttributes.Public | MethodAttributes.Virtual,
    MethodImplAttributes.IL, GetStringHandle(m.Name!), m_builder.GetOrAddBlob(signature), -1, parameterHandles[0]);

            return mdh;

        }

        private BlobHandle GetSignatureBlob(WinRTMethod m)
        {
            throw new NotImplementedException();
        }

        private void EncodeParameters(ParametersEncoder _, List<WinRTArgument> arguments, ParameterHandle[] parameterHandles)
        {
            foreach (var p in arguments)
            {
                var paramTypeEncoder = _.AddParameter();
                // TODO
                paramTypeEncoder.Type().Object();
            }
        }

        private void EncodeReturnType(ReturnTypeEncoder _, WinRTType returnType)
        {
            if (returnType == WinRTBasicType.Void)
            {
                _.Void();
            } else
            {
                // TODO
                _.Type().Object();
            }
        }

        private void EmitInterface(WinRTNamespace ns, WinRTInterface i)
        {
            MethodDefinitionHandle? first = null;
            foreach (var m in i.Methods)
            {
                var mdh = EmitMethod(m);
                if (first == null)
                {
                    first = mdh;
                }
            }

            m_builder.AddTypeDefinition(TypeAttributes.Public | TypeAttributes.Interface | TypeAttributes.WindowsRuntime | TypeAttributes.Abstract,
                GetStringHandle(ns.FullName!), GetStringHandle(i.Name!),
                baseType: default(TypeDefinitionHandle),
                fieldList: MetadataTokens.FieldDefinitionHandle(1),
                methodList: (MethodDefinitionHandle)first!);
        }

        private void EmitEnum(WinRTNamespace? ns, WinRTEnum e)
        {
            FieldDefinitionHandle? first = null;
            foreach (var field in e.Fields)
            {
                var _ = m_builder.AddFieldDefinition(
                    FieldAttributes.Public | FieldAttributes.Literal | FieldAttributes.Static | FieldAttributes.HasDefault,
                    GetStringHandle(field.Name!),
                    m_builder.GetOrAddBlob(BuildSignature(e => e.FieldSignature().Int32())));
                if (first == null)
                {
                    first = _;
                }
                m_builder.AddConstant(_, field.Value);
            }
            var fieldSig = new BlobBuilder();
            var encoder = new BlobEncoder(fieldSig);
            encoder.FieldSignature().Int32();

            m_builder.AddFieldDefinition(FieldAttributes.Private | FieldAttributes.SpecialName | FieldAttributes.RTSpecialName,
                GetStringHandle("value__"),
                m_builder.GetOrAddBlob(fieldSig));

            var enumRef = m_builder.AddTypeReference(default(AssemblyReferenceHandle), GetStringHandle("System"), GetStringHandle("Enum"));
            var td = m_builder.AddTypeDefinition(TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.WindowsRuntime,
                GetStringHandle(ns!.FullName!),
                GetStringHandle(e.Name!),
                enumRef,
                (FieldDefinitionHandle)first!,
                MetadataTokens.MethodDefinitionHandle(1)
                );
        }
    }
}
