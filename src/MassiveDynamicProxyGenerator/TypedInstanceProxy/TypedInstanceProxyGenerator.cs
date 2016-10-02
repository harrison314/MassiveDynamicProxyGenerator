using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.TypedInstanceProxy
{
    internal class TypedInstanceProxyGenerator : AbstractTypeBuilder<object>
    {
        private readonly InstanceProvicerDescriptor descriptor;
        private FieldBuilder instanceProvicerField;

        public TypedInstanceProxyGenerator(TypeBuilder typeBuilder)
            : base(typeBuilder)
        {
            this.descriptor = new InstanceProvicerDescriptor();
        }

        protected override void ImplementFields(TypeBuilder typeBuilder, Type interfaceType)
        {
            this.instanceProvicerField = typeBuilder.DefineField("instanceProvicer", typeof(IInstanceProvicer), FieldAttributes.Private);
        }

        protected override void ImplementsConstructor(TypeBuilder typeBuilder, Type interfaceType)
        {
            ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public |
                MethodAttributes.HideBySig |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[] { this.descriptor.Type });

            ILGenerator il = constructorBuilder.GetILGenerator();
            ConstructorInfo conObj = typeof(object).GetConstructor(new Type[0]);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, conObj);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, this.instanceProvicerField);
            il.Emit(OpCodes.Ret);
        }

        protected override void GenerateGetProperty(PropertyInfo interfaceProperity, Type interfaceType, ILGenerator il, object context)
        {
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, this.instanceProvicerField);
            il.Emit(OpCodes.Callvirt, this.descriptor.GetInstance);
            il.Emit(OpCodes.Castclass, interfaceType);
            il.Emit(OpCodes.Callvirt, interfaceProperity.GetGetMethod());
            il.Emit(OpCodes.Ret);
        }

        protected override void GenerateSetProperty(PropertyInfo interfaceProperity, Type interfaceType, ILGenerator il, object context)
        {
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, this.instanceProvicerField);
            il.Emit(OpCodes.Callvirt, this.descriptor.GetInstance);
            il.Emit(OpCodes.Castclass, interfaceType);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Callvirt, interfaceProperity.GetSetMethod());
            il.Emit(OpCodes.Ret);
        }

        protected override void GenerateMethod(MethodInfo interfaceMethod, Type[] parameters, Type interfaceType, ILGenerator il, object context)
        {
            LocalBuilder realObject = il.DeclareLocal(interfaceType);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, this.instanceProvicerField);
            il.Emit(OpCodes.Callvirt, this.descriptor.GetInstance);
            il.Emit(OpCodes.Castclass, interfaceType);

            // For multithreading.
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldloc_0);

            for (int i = 0; i < parameters.Length; i++)
            {
                il.Emit(OpCodes.Ldarg, i + 1);
            }

            il.Emit(OpCodes.Callvirt, interfaceMethod);

            if (interfaceMethod.Name == nameof(IDisposable.Dispose) && parameters.Length == 0 && interfaceMethod.ReturnType == typeof(void))
            {
                il.Emit(OpCodes.Nop);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Callvirt, this.descriptor.Dispose);
            }

            il.Emit(OpCodes.Ret);
        }
    }
}
