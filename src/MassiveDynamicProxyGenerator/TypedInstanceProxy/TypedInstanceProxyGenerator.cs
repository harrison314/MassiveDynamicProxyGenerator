using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.TypedInstanceProxy
{
    /// <summary>
    /// Generator for instanced proxy.
    /// </summary>
    /// <seealso cref="AbstractTypeBuilder{Object}" />
    internal class TypedInstanceProxyGenerator : AbstractTypeBuilder<object>
    {
        private readonly InstanceProvicerDescriptor descriptor;
        private FieldBuilder instanceProvicerField;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypedInstanceProxyGenerator"/> class.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        public TypedInstanceProxyGenerator(TypeBuilder typeBuilder)
            : base(typeBuilder)
        {
            this.descriptor = new InstanceProvicerDescriptor();
        }

        /// <summary>
        /// Implements the fields.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        protected override void ImplementFields(TypeBuilder typeBuilder, Type interfaceType)
        {
            this.instanceProvicerField = typeBuilder.DefineField("instanceProvicer", typeof(IInstanceProvicer), FieldAttributes.Private);
        }

        /// <summary>
        /// Implementses the constructor.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        protected override void ImplementConstructor(TypeBuilder typeBuilder, Type interfaceType)
        {
            ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public |
                MethodAttributes.HideBySig |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[] { this.descriptor.Type });

            ILGenerator il = constructorBuilder.GetILGenerator();
            ConstructorInfo conObj = typeof(object).GetTypeInfo().GetConstructor(new Type[0]);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, conObj);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, this.instanceProvicerField);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Generates the get property.
        /// </summary>
        /// <param name="interfaceProperity">The interface properity.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="il">The il.</param>
        /// <param name="context">The context.</param>
        protected override void GenerateGetProperty(PropertyInfo interfaceProperity, Type interfaceType, ILGenerator il, object context)
        {
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, this.instanceProvicerField);
            il.Emit(OpCodes.Callvirt, this.descriptor.GetInstance);
            il.Emit(OpCodes.Castclass, interfaceType);
            il.Emit(OpCodes.Callvirt, interfaceProperity.GetGetMethod());
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Generates the set property.
        /// </summary>
        /// <param name="interfaceProperity">The interface properity.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="il">The il.</param>
        /// <param name="context">The context.</param>
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

        /// <summary>
        /// Generates the method.
        /// </summary>
        /// <param name="interfaceMethod">The interface method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="il">The il.</param>
        /// <param name="context">The context.</param>
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
