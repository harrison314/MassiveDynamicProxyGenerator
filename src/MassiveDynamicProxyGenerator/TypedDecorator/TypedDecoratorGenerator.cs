using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.TypedDecorator
{
    /// <summary>
    /// Generator for decorators.
    /// </summary>
    /// <seealso cref="AbstractTypeBuilder{GenerateUnion}" />
    internal class TypedDecoratorGenerator : AbstractTypeBuilder<GenerateUnion>
    {
        private readonly ITypeNameCreator typeNameGenerator;
        private readonly CallableInterceptorDescriptor callableInterceptorDescriptor;
        private readonly CallableInvocationDescriptor callableInvocationDescriptor;
        private readonly Type actionType;

        private FieldBuilder interceptorField;
        private FieldBuilder parentField;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypedDecoratorGenerator"/> class.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="typeNameGenerator">The type name generator.</param>
        /// <exception cref="ArgumentNullException">typeNameGenerator</exception>
        public TypedDecoratorGenerator(TypeBuilder typeBuilder, ITypeNameCreator typeNameGenerator)
            : base(typeBuilder)
        {
            if (typeNameGenerator == null)
            {
                throw new ArgumentNullException(nameof(typeNameGenerator));
            }

            this.typeNameGenerator = typeNameGenerator;
            this.callableInterceptorDescriptor = new CallableInterceptorDescriptor();
            this.callableInvocationDescriptor = new CallableInvocationDescriptor();

            this.actionType = typeof(Action<ICallableInvocation>);
        }

        /// <summary>
        /// Implements the fields.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        protected override void ImplementFields(TypeBuilder typeBuilder, Type interfaceType)
        {
            this.interceptorField = this.TypeBuilder.DefineField("interceptor", typeof(ICallableInterceptor), FieldAttributes.Private);
            this.parentField = this.TypeBuilder.DefineField("parent", interfaceType, FieldAttributes.Private);

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
                new Type[] { this.callableInterceptorDescriptor.Type, interfaceType });

            ILGenerator il = constructorBuilder.GetILGenerator();
            ConstructorInfo conObj = typeof(object).GetTypeInfo().GetConstructor(new Type[0]);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, conObj);
            il.Emit(OpCodes.Nop);

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, this.interceptorField);

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Stfld, this.parentField);

            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Implements the method.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="interfaceMethod">The interface method.</param>
        /// <param name="context">The context.</param>
        protected override void ImplementMethod(TypeBuilder typeBuilder, Type interfaceType, MethodInfo interfaceMethod, GenerateUnion context)
        {
            MethodBuilder processMethod = this.ImplementParentCallMethod(typeBuilder, interfaceType, interfaceMethod);
            base.ImplementMethod(typeBuilder, interfaceType, interfaceMethod, new GenerateUnion(processMethod));
        }

        /// <summary>
        /// Generates the method.
        /// </summary>
        /// <param name="interfaceMethod">The interface method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="il">The IL generator.</param>
        /// <param name="context">The context.</param>
        protected override void GenerateMethod(MethodInfo interfaceMethod, Type[] parameters, Type interfaceType, ILGenerator il, GenerateUnion context)
        {
            LocalBuilder invocationVar = il.DeclareLocal(this.callableInvocationDescriptor.Type);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldftn, context.ProcessMethod);
            il.Emit(OpCodes.Newobj, this.actionType.GetTypeInfo().GetConstructors().First());

            il.Emit(OpCodes.Newobj, this.callableInvocationDescriptor.Constructor);
            il.Emit(OpCodes.Stloc, invocationVar);

            // invocation.Arguments = new object[params.Length];
            il.Emit(OpCodes.Ldloc, invocationVar);
            il.Emit(OpCodes.Ldc_I4, parameters.Length);
            il.Emit(OpCodes.Newarr, typeof(object));

            for (int i = 0; i < parameters.Length; i++)
            {
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldarg, i + 1);
                if (parameters[i].GetTypeInfo().IsValueType)
                {
                    il.Emit(OpCodes.Box, parameters[i]);
                }

                il.Emit(OpCodes.Stelem_Ref);
            }

            il.Emit(OpCodes.Callvirt, this.callableInvocationDescriptor.Arguments.GetSetMethod());

            // invocation.MethodName = "AnyMethod";
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldloc, invocationVar);
            il.Emit(OpCodes.Ldstr, interfaceMethod.Name);
            il.Emit(OpCodes.Callvirt, this.callableInvocationDescriptor.MethodName.GetSetMethod());

            MethodInfo getTypeFromhandle = typeof(Type).GetTypeInfo().GetMethod(nameof(Type.GetTypeFromHandle), BindingFlags.Static | BindingFlags.Public);

            // invocation.OriginalType = typeof(interfaceType);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldloc, invocationVar);
            il.Emit(OpCodes.Ldtoken, interfaceType);
            il.Emit(OpCodes.Call, getTypeFromhandle);
            il.Emit(OpCodes.Callvirt, this.callableInvocationDescriptor.OriginalType.GetSetMethod());

            // invocation.ArgumentTypes = new Type[] { typeof(....), .... };
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldloc, invocationVar);
            il.Emit(OpCodes.Ldc_I4, parameters.Length);
            il.Emit(OpCodes.Newarr, typeof(Type));

            for (int i = 0; i < parameters.Length; i++)
            {
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldtoken, parameters[i]);
                il.Emit(OpCodes.Call, getTypeFromhandle);
                il.Emit(OpCodes.Stelem_Ref);
            }

            il.Emit(OpCodes.Callvirt, this.callableInvocationDescriptor.ArgumentTypes.GetSetMethod());

            // invocation.ReturnType = typeof(method.ReturnType);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldloc, invocationVar);
            il.Emit(OpCodes.Ldtoken, interfaceMethod.ReturnType);
            il.Emit(OpCodes.Call, getTypeFromhandle);
            il.Emit(OpCodes.Callvirt, this.callableInvocationDescriptor.ReturnType.GetSetMethod());

            // volanie interceptora
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, this.interceptorField);
            il.Emit(OpCodes.Ldloc, invocationVar);

            il.Emit(OpCodes.Callvirt, this.callableInterceptorDescriptor.Intercept);

            if (interfaceMethod.ReturnType == typeof(void))
            {
                il.Emit(OpCodes.Nop);
                il.Emit(OpCodes.Ret);
            }
            else
            {
                il.Emit(OpCodes.Nop);
                il.Emit(OpCodes.Ldloc, invocationVar);
                il.Emit(OpCodes.Callvirt, this.callableInvocationDescriptor.ReturnValue.GetGetMethod());
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
        /// <param name="interfaceProperty">The interface property.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="il">The il.</param>
        /// <param name="context">The context.</param>
        protected override void GenerateGetProperty(PropertyInfo interfaceProperty, Type interfaceType, ILGenerator il, GenerateUnion context)
        {
            MethodInfo methodInfo = interfaceProperty.GetGetMethod();
            Type[] parameters = methodInfo.GetParameters().Select(t => t.ParameterType).ToArray();
            MethodBuilder processMethod = this.ImplementParentCallMethod(this.TypeBuilder, interfaceType, methodInfo);

            this.GenerateMethod(methodInfo, parameters, interfaceType, il, new GenerateUnion(processMethod));
        }

        /// <summary>
        /// Generates the set property.
        /// </summary>
        /// <param name="interfaceProperity">The interface properity.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="il">The il.</param>
        /// <param name="context">The context.</param>
        protected override void GenerateSetProperty(PropertyInfo interfaceProperity, Type interfaceType, ILGenerator il, GenerateUnion context)
        {
            MethodInfo methodInfo = interfaceProperity.GetSetMethod();
            Type[] parameters = methodInfo.GetParameters().Select(t => t.ParameterType).ToArray();
            MethodBuilder processMethod = this.ImplementParentCallMethod(this.TypeBuilder, interfaceType, methodInfo);

            this.GenerateMethod(methodInfo, parameters, interfaceType, il, new GenerateUnion(processMethod));
        }

        private MethodBuilder ImplementParentCallMethod(TypeBuilder typeBuilder, Type interfaceType, MethodInfo originalMethod)
        {
            string methodName = this.typeNameGenerator.CreateMethodName();
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(methodName,
                   MethodAttributes.Private | MethodAttributes.HideBySig,
                   typeof(void),
                   new Type[] { typeof(ICallableInvocation) });

            ParameterInfo[] methodParameters = originalMethod.GetParameters();

            ILGenerator il = methodBuilder.GetILGenerator();

            if (originalMethod.ReturnType != typeof(void))
            {
                LocalBuilder tmpArray = il.DeclareLocal(typeof(object[]));
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Callvirt, this.callableInvocationDescriptor.Arguments.GetGetMethod());
                il.Emit(OpCodes.Stloc, tmpArray);

                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, this.parentField);

                for (int i = 0; i < methodParameters.Length; i++)
                {
                    il.Emit(OpCodes.Ldloc, tmpArray);
                    il.Emit(OpCodes.Ldc_I4, i); // TODO: refaktor
                    il.Emit(OpCodes.Ldelem_Ref);

                    if (methodParameters[i].ParameterType.GetTypeInfo().IsValueType)
                    {
                        il.Emit(OpCodes.Unbox_Any, methodParameters[i].ParameterType);
                    }
                    else
                    {
                        // TODO: nekastovat ak je to objekt
                        il.Emit(OpCodes.Castclass, methodParameters[i].ParameterType);
                    }
                }

                il.Emit(OpCodes.Callvirt, originalMethod);
                if (originalMethod.ReturnType.GetTypeInfo().IsValueType)
                {
                    il.Emit(OpCodes.Box, originalMethod.ReturnType);
                }

                il.Emit(OpCodes.Callvirt, this.callableInvocationDescriptor.ReturnValue.GetSetMethod());

                il.Emit(OpCodes.Nop);
                il.Emit(OpCodes.Ret);
            }
            else
            {
                LocalBuilder tmpArray = il.DeclareLocal(typeof(object[]));
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Callvirt, this.callableInvocationDescriptor.Arguments.GetGetMethod());
                il.Emit(OpCodes.Stloc, tmpArray);

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, this.parentField);

                for (int i = 0; i < methodParameters.Length; i++)
                {
                    il.Emit(OpCodes.Ldloc, tmpArray);
                    il.Emit(OpCodes.Ldc_I4, i); // TODO: refaktor
                    il.Emit(OpCodes.Ldelem_Ref);

                    if (methodParameters[i].ParameterType.GetTypeInfo().IsValueType)
                    {
                        il.Emit(OpCodes.Unbox_Any, methodParameters[i].ParameterType);
                    }
                    else
                    {
                        // TODO: nekastovat ak je to objekt
                        il.Emit(OpCodes.Castclass, methodParameters[i].ParameterType);
                    }
                }

                il.Emit(OpCodes.Callvirt, originalMethod);

                il.Emit(OpCodes.Nop);
                il.Emit(OpCodes.Ret);
            }

            return methodBuilder;
        }
    }
}
