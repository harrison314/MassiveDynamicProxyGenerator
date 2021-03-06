﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    /// <summary>
    /// Base type builder.
    /// </summary>
    /// <typeparam name="T">Type of context.</typeparam>
    internal abstract class AbstractTypeBuilder<T>
    {
        private readonly TypeBuilder typeBuilder;
        private readonly HashSet<Type> implementInterfaces;
        private bool isMembersImplements;

        /// <summary>
        /// Gets the implement interfaces.
        /// </summary>
        /// <value>
        /// The implement interfaces.
        /// </value>
        public IEnumerable<Type> ImplementInterfaces
        {
            get
            {
                return this.implementInterfaces;
            }
        }

        /// <summary>
        /// Gets the type builder.
        /// </summary>
        /// <value>
        /// The type builder.
        /// </value>
        protected TypeBuilder TypeBuilder
        {
            get
            {
                return this.typeBuilder;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractTypeBuilder{T}"/> class.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        /// <exception cref="ArgumentNullException">typeBuilder</exception>
        public AbstractTypeBuilder(TypeBuilder typeBuilder)
        {
            if (typeBuilder == null)
            {
                throw new ArgumentNullException(nameof(typeBuilder));
            }

            this.typeBuilder = typeBuilder;
            this.isMembersImplements = false;
            this.implementInterfaces = new HashSet<Type>();
        }

        /// <summary>
        /// Checks the type.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <exception cref="ArgumentNullException">interfaceType</exception>
        /// <exception cref="InvalidOperationException">Type is not interface.</exception>
        public virtual void CheckType(Type interfaceType)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }

            if (!interfaceType.GetTypeInfo().IsInterface)
            {
                throw new InvalidOperationException($"Type {interfaceType.FullName} is not interface.");
            }
        }

        /// <summary>
        /// Implements the interface.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <exception cref="ArgumentNullException">interfaceType</exception>
        public void ImplementInterface(Type interfaceType)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }

            if (!this.isMembersImplements)
            {
                this.ImplementFields(this.TypeBuilder, interfaceType);
                this.ImplementConstructor(this.TypeBuilder, interfaceType);
                this.isMembersImplements = true;
            }

            if (this.implementInterfaces.Add(interfaceType))
            {
                this.ImplementSimpleInterface(interfaceType);
            }

            foreach (Type secundaryInterfaceType in interfaceType.GetTypeInfo().GetInterfaces())
            {
                if (this.implementInterfaces.Add(secundaryInterfaceType))
                {
                    this.ImplementSimpleInterface(secundaryInterfaceType);
                }
            }
        }

        /// <summary>
        /// Implements the fields.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        protected virtual void ImplementFields(TypeBuilder typeBuilder, Type interfaceType)
        {
        }

        /// <summary>
        /// Implements the constructor.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        protected virtual void ImplementConstructor(TypeBuilder typeBuilder, Type interfaceType)
        {
            ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public |
                MethodAttributes.HideBySig |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                Type.EmptyTypes);

            ILGenerator il = constructorBuilder.GetILGenerator();
            ConstructorInfo conObj = typeof(object).GetTypeInfo().GetConstructor(Type.EmptyTypes);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, conObj);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Implements the method.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="interfaceMethod">The interface method.</param>
        /// <param name="context">The context.</param>
        protected virtual void ImplementMethod(TypeBuilder typeBuilder, Type interfaceType, MethodInfo interfaceMethod, T context)
        {
            Type[] parameters = interfaceMethod.GetParameters().Select(t => t.ParameterType).ToArray();
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(interfaceMethod.Name,
                         MethodAttributes.Private | MethodAttributes.Virtual,
                         interfaceMethod.ReturnType,
                         parameters);

            typeBuilder.DefineMethodOverride(methodBuilder, interfaceMethod);

            ILGenerator il = methodBuilder.GetILGenerator();

            this.GenerateMethod(interfaceMethod, parameters, interfaceType, il, context);
        }

        /// <summary>
        /// Generates the method.
        /// </summary>
        /// <param name="interfaceMethod">The interface method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="il">The IL generator.</param>
        /// <param name="context">The context.</param>
        protected virtual void GenerateMethod(MethodInfo interfaceMethod, Type[] parameters, Type interfaceType, ILGenerator il, T context)
        {
            ConstructorInfo ci = typeof(NotImplementedException).GetTypeInfo().GetConstructor(Type.EmptyTypes);

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Newobj, ci);
            il.Emit(OpCodes.Throw);
        }

        /// <summary>
        /// Implements the property.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="interfaceProperty">The interface property.</param>
        /// <param name="context">The context.</param>
        protected virtual void ImplementProperty(TypeBuilder typeBuilder, Type interfaceType, PropertyInfo interfaceProperty, T context)
        {
            PropertyBuilder properityBuilder = typeBuilder.DefineProperty(interfaceProperty.Name,
                PropertyAttributes.HasDefault,
                interfaceProperty.PropertyType,
                null);

            if (interfaceProperty.CanRead)
            {
                MethodBuilder getMethodBuilder = typeBuilder.DefineMethod(interfaceProperty.GetGetMethod().Name,
                    MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                    interfaceProperty.PropertyType,
                    Type.EmptyTypes);

                typeBuilder.DefineMethodOverride(getMethodBuilder, interfaceProperty.GetGetMethod());
                ILGenerator il = getMethodBuilder.GetILGenerator();
                this.GenerateGetProperty(interfaceProperty, interfaceType, il, context);
                properityBuilder.SetGetMethod(getMethodBuilder);
            }

            if (interfaceProperty.CanWrite)
            {
                MethodBuilder setMethodBuilder = typeBuilder.DefineMethod(interfaceProperty.GetSetMethod().Name,
                    MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                    null,
                    new Type[] { interfaceProperty.PropertyType });

                typeBuilder.DefineMethodOverride(setMethodBuilder, interfaceProperty.GetSetMethod());
                ILGenerator il = setMethodBuilder.GetILGenerator();
                this.GenerateSetProperty(interfaceProperty, interfaceType, il, context);
                properityBuilder.SetSetMethod(setMethodBuilder);
            }
        }

        /// <summary>
        /// Generates the set property.
        /// </summary>
        /// <param name="interfacePproperty">The interface property.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="il">The IL generator.</param>
        /// <param name="context">The context.</param>
        protected virtual void GenerateSetProperty(PropertyInfo interfacePproperty, Type interfaceType, ILGenerator il, T context)
        {
            ConstructorInfo ci = typeof(NotImplementedException).GetTypeInfo().GetConstructor(Type.EmptyTypes);

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Newobj, ci);
            il.Emit(OpCodes.Throw);
        }

        /// <summary>
        /// Generates the get property.
        /// </summary>
        /// <param name="interfacePproperty">The interface property.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="il">The IL generator.</param>
        /// <param name="context">The context.</param>
        protected virtual void GenerateGetProperty(PropertyInfo interfacePproperty, Type interfaceType, ILGenerator il, T context)
        {
            ConstructorInfo ci = typeof(NotImplementedException).GetTypeInfo().GetConstructor(Type.EmptyTypes);

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Newobj, ci);
            il.Emit(OpCodes.Throw);
        }

        private void ImplementSimpleInterface(Type interfaceType)
        {
            foreach (MethodInfo methodInfo in interfaceType.GetTypeInfo().GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!methodInfo.IsSpecialName)
                {
                    this.ImplementMethod(this.TypeBuilder, interfaceType, methodInfo, default);
                }
            }

            foreach (PropertyInfo properityInfo in interfaceType.GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                this.ImplementProperty(this.TypeBuilder, interfaceType, properityInfo, default);
            }
        }
    }
}
