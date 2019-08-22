using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using MassiveDynamicProxyGenerator.Utils;

namespace MassiveDynamicProxyGenerator.TypedProxy
{
    /// <summary>
    /// Generator for proxy.
    /// </summary>
    /// <seealso cref="AbstractTypeBuilder{Object}" />
    internal class TypedProxyGenerator : AbstractTypeBuilder<object>
    {
        private readonly InvocationDescriptor descriptor;
        private readonly bool implementProperty;
        private FieldBuilder interceptorField;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypedProxyGenerator"/> class.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="implementProperty">if set to <c>true</c> [implement property].</param>
        /// <exception cref="ArgumentNullException">typeBuilder</exception>
        public TypedProxyGenerator(TypeBuilder typeBuilder, bool implementProperty)
            : base(typeBuilder)
        {
            if (typeBuilder == null)
            {
                throw new ArgumentNullException(nameof(typeBuilder));
            }

            this.implementProperty = implementProperty;
            this.descriptor = InvocationDescriptor.Create<TypedProxyInvocation>();
        }

        /// <summary>
        /// Implements the fields.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        protected override void ImplementFields(TypeBuilder typeBuilder, Type interfaceType)
        {
            this.interceptorField = this.TypeBuilder.DefineField("interceptor", typeof(IInterceptor), FieldAttributes.Private);
            base.ImplementFields(typeBuilder, interfaceType);
        }

        /// <summary>
        /// Implements the constructor.
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
                new Type[] { typeof(IInterceptor) });

            ILGenerator il = constructorBuilder.GetILGenerator();
            ConstructorInfo conObj = typeof(object).GetTypeInfo().GetConstructor(new Type[0]);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, conObj);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, this.interceptorField);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Generates the method.
        /// </summary>
        /// <param name="interfaceMethod">The interface method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="il">The IL generator.</param>
        /// <param name="context">The context.</param>
        protected override void GenerateMethod(MethodInfo interfaceMethod, Type[] parameters, Type interfaceType, ILGenerator il, object context)
        {
            LocalBuilder invocationVar = il.DeclareLocal(this.descriptor.Type);
            il.Emit(OpCodes.Newobj, this.descriptor.Constructor);
            il.Emit(OpCodes.Stloc, invocationVar);

            // invocation.Arguments = new object[params.Length];
            il.Emit(OpCodes.Ldloc, invocationVar);
            il.EmitFastInt(parameters.Length);
            il.Emit(OpCodes.Newarr, typeof(object));

            for (int i = 0; i < parameters.Length; i++)
            {
                il.Emit(OpCodes.Dup);
                il.EmitFastInt(i);
                il.Emit(OpCodes.Ldarg, i + 1);
                if (parameters[i].GetTypeInfo().IsValueType)
                {
                    il.Emit(OpCodes.Box, parameters[i]);
                }

                il.Emit(OpCodes.Stelem_Ref);
            }

            il.Emit(OpCodes.Callvirt, this.descriptor.Arguments.GetSetMethod());

            // invocation.MethodName = "AnyMethod";
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldloc, invocationVar);
            il.Emit(OpCodes.Ldstr, interfaceMethod.Name);
            il.Emit(OpCodes.Callvirt, this.descriptor.MethodName.GetSetMethod());

            MethodInfo getTypeFromhandle = typeof(Type).GetTypeInfo().GetMethod(nameof(Type.GetTypeFromHandle), BindingFlags.Static | BindingFlags.Public);

            // invocation.OriginalType = typeof(interfaceType);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldloc, invocationVar);
            il.Emit(OpCodes.Ldtoken, interfaceType);
            il.Emit(OpCodes.Call, getTypeFromhandle);
            il.Emit(OpCodes.Callvirt, this.descriptor.OriginalType.GetSetMethod());

            // invocation.ArgumentTypes = new Type[] { typeof(....), .... };
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldloc, invocationVar);
            il.EmitFastInt(parameters.Length);
            il.Emit(OpCodes.Newarr, typeof(Type));

            for (int i = 0; i < parameters.Length; i++)
            {
                il.Emit(OpCodes.Dup);
                il.EmitFastInt(i);
                il.Emit(OpCodes.Ldtoken, parameters[i]);
                il.Emit(OpCodes.Call, getTypeFromhandle);
                il.Emit(OpCodes.Stelem_Ref);
            }

            il.Emit(OpCodes.Callvirt, this.descriptor.ArgumentTypes.GetSetMethod());

            // invocation.ReturnType = typeof(method.ReturnType);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldloc, invocationVar);
            il.Emit(OpCodes.Ldtoken, interfaceMethod.ReturnType);
            il.Emit(OpCodes.Call, getTypeFromhandle);
            il.Emit(OpCodes.Callvirt, this.descriptor.ReturnType.GetSetMethod());

            // volanie interceptora
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, this.interceptorField);
            il.Emit(OpCodes.Ldloc, invocationVar);
            il.Emit(OpCodes.Callvirt, typeof(IInterceptor).GetTypeInfo().GetMethod(nameof(IInterceptor.Intercept)));

            if (interfaceMethod.ReturnType == typeof(void))
            {
                il.Emit(OpCodes.Nop);
                il.Emit(OpCodes.Ret);
            }
            else
            {
                il.Emit(OpCodes.Nop);
                il.Emit(OpCodes.Ldloc, invocationVar);
                il.Emit(OpCodes.Callvirt, this.descriptor.ReturnValue.GetGetMethod());
                if (interfaceMethod.ReturnType.GetTypeInfo().IsValueType)
                {
                    il.Emit(OpCodes.Unbox_Any, interfaceMethod.ReturnType);
                }
                else
                {
                    il.Emit(OpCodes.Castclass, interfaceMethod.ReturnType);
                }

                il.Emit(OpCodes.Ret);
            }
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
            if (this.implementProperty)
            {
                MethodInfo methodInfo = interfaceProperity.GetGetMethod();
                Type[] parameters = methodInfo.GetParameters().Select(t => t.ParameterType).ToArray();

                this.GenerateMethod(methodInfo, parameters, interfaceType, il, context);
            }
            else
            {
                base.GenerateGetProperty(interfaceProperity, interfaceType, il, context);
            }
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
            if (this.implementProperty)
            {
                MethodInfo methodInfo = interfaceProperity.GetSetMethod();
                Type[] parameters = methodInfo.GetParameters().Select(t => t.ParameterType).ToArray();

                this.GenerateMethod(methodInfo, parameters, interfaceType, il, context);
            }
            else
            {
                base.GenerateSetProperty(interfaceProperity, interfaceType, il, context);
            }
        }
    }
}
