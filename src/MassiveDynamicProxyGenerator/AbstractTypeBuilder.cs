using System;
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
                string message = string.Format("Type {0} is not interface.", interfaceType.FullName);
                throw new InvalidOperationException(message);
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
                this.ImplementsConstructor(this.TypeBuilder, interfaceType);
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
        /// Implementses the constructor.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        protected virtual void ImplementsConstructor(TypeBuilder typeBuilder, Type interfaceType)
        {
            ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public |
                MethodAttributes.HideBySig |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[] { });

            ILGenerator il = constructorBuilder.GetILGenerator();
            ConstructorInfo conObj = typeof(object).GetTypeInfo().GetConstructor(new Type[0]);
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
        /// <param name="il">The il.</param>
        /// <param name="context">The context.</param>
        protected virtual void GenerateMethod(MethodInfo interfaceMethod, Type[] parameters, Type interfaceType, ILGenerator il, T context)
        {
            ConstructorInfo ci = typeof(NotImplementedException).GetTypeInfo().GetConstructor(new Type[0]);

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Newobj, ci);
            il.Emit(OpCodes.Throw);
        }

        /// <summary>
        /// Implementeds the properity.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="interfaceProperity">The interface properity.</param>
        /// <param name="context">The context.</param>
        protected virtual void ImplementedProperity(TypeBuilder typeBuilder, Type interfaceType, PropertyInfo interfaceProperity, T context)
        {
            PropertyBuilder properityBuilder = typeBuilder.DefineProperty(interfaceProperity.Name,
                PropertyAttributes.HasDefault,
                interfaceProperity.PropertyType,
                null);

            if (interfaceProperity.CanRead)
            {
                MethodBuilder getMethodBuilder = typeBuilder.DefineMethod(interfaceProperity.GetGetMethod().Name,
                    MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                    interfaceProperity.PropertyType,
                    Type.EmptyTypes);

                typeBuilder.DefineMethodOverride(getMethodBuilder, interfaceProperity.GetGetMethod());
                ILGenerator il = getMethodBuilder.GetILGenerator();
                this.GenerateGetProperty(interfaceProperity, interfaceType, il, context);
                properityBuilder.SetGetMethod(getMethodBuilder);
            }

            if (interfaceProperity.CanWrite)
            {
                MethodBuilder setMethodBuilder = typeBuilder.DefineMethod(interfaceProperity.GetSetMethod().Name,
                    MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                    null,
                    new[] { interfaceProperity.PropertyType });

                typeBuilder.DefineMethodOverride(setMethodBuilder, interfaceProperity.GetSetMethod());
                ILGenerator il = setMethodBuilder.GetILGenerator();
                this.GenerateSetProperty(interfaceProperity, interfaceType, il, context);
                properityBuilder.SetSetMethod(setMethodBuilder);
            }
        }

        /// <summary>
        /// Generates the set property.
        /// </summary>
        /// <param name="interfaceProperity">The interface properity.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="il">The il.</param>
        /// <param name="context">The context.</param>
        protected virtual void GenerateSetProperty(PropertyInfo interfaceProperity, Type interfaceType, ILGenerator il, T context)
        {
            ConstructorInfo ci = typeof(NotImplementedException).GetTypeInfo().GetConstructor(new Type[0]);

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Newobj, ci);
            il.Emit(OpCodes.Throw);
        }

        /// <summary>
        /// Generates the get property.
        /// </summary>
        /// <param name="interfaceProperity">The interface properity.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="il">The il.</param>
        /// <param name="context">The context.</param>
        protected virtual void GenerateGetProperty(PropertyInfo interfaceProperity, Type interfaceType, ILGenerator il, T context)
        {
            ConstructorInfo ci = typeof(NotImplementedException).GetTypeInfo().GetConstructor(new Type[0]);

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
                    this.ImplementMethod(this.TypeBuilder, interfaceType, methodInfo, default(T));
                }
            }

            foreach (PropertyInfo properityInfo in interfaceType.GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                this.ImplementedProperity(this.TypeBuilder, interfaceType, properityInfo, default(T));
            }
        }
    }
}
