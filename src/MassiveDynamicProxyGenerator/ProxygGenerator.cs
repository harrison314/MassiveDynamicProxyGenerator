using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using MassiveDynamicProxyGenerator.Utils;
using MassiveDynamicProxyGenerator.TypedDecorator;
using MassiveDynamicProxyGenerator.TypedInstanceProxy;
using MassiveDynamicProxyGenerator.TypedProxy;
using MassiveDynamicProxyGenerator.DynamicProxy;

namespace MassiveDynamicProxyGenerator
{
    /// <summary>
    /// Dynamic proxy generator.
    /// </summary>
    public class ProxygGenerator
    {
        private static int assemblyCount = 1;
        private readonly string assemblyName;
        private readonly ITypeNameCreator typeNameCreator;
        private readonly GeneratedTypeList generatedTypeList;
        private AssemblyBuilder assemblyBuilder;
        private ModuleBuilder moduleBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxygGenerator"/> class.
        /// </summary>
        public ProxygGenerator()
        {
            this.assemblyBuilder = null;
            this.moduleBuilder = null;
            this.typeNameCreator = DefaultInstances.TypeNameCreator;
            int number = Interlocked.Increment(ref assemblyCount);
            this.assemblyName = string.Format(CultureInfo.InvariantCulture, "MassiveDynamic.DynamicProxys{0}", number);
            this.generatedTypeList = DefaultInstances.TypedList;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxygGenerator"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <exception cref="ArgumentNullException">settings</exception>
        /// <seealso cref="ProxygGeneratorSettings"/>
        public ProxygGenerator(ProxygGeneratorSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this.assemblyBuilder = null;
            this.moduleBuilder = null;

            this.typeNameCreator = settings.TypeNameCreator ?? DefaultInstances.TypeNameCreator;

            if (settings.UseLocalCache)
            {
                this.generatedTypeList = new GeneratedTypeList();
            }
            else
            {
                this.generatedTypeList = DefaultInstances.TypedList;
            }

            if (string.IsNullOrEmpty(settings.AssemblyName))
            {
                int number = Interlocked.Increment(ref assemblyCount);
                this.assemblyName = string.Format(CultureInfo.InvariantCulture, "MassiveDynamic.DynamicProxys{0}", number);
            }
            else
            {
                this.assemblyName = settings.AssemblyName;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxygGenerator"/> class.
        /// </summary>
        /// <param name="configure">The configure action.</param>
        /// <exception cref="ArgumentNullException">configure</exception>
        public ProxygGenerator(Action<ProxygGeneratorSettings> configure)
            : this(SettingsUtils.Apply(configure, new ProxygGeneratorSettings()))
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }
        }

        /// <summary>
        /// Generates the proxy instance with interceptor.
        /// </summary>
        /// <typeparam name="T">Type of inteface for generate proxy.</typeparam>
        /// <param name="interceptor">The interceptor.</param>
        /// <returns>Instance of <typeparamref name="T"/> implament as proxy generator.</returns>
        /// <exception cref="ArgumentNullException">interceptor</exception>
        public T GenerateProxy<T>(IInterceptor interceptor)
            where T : class
        {
            return this.GenerateProxy<T>(interceptor, false);
        }

        /// <summary>
        /// Generates the proxy instance with interceptor.
        /// </summary>
        /// <typeparam name="T">Type of inteface for generate proxy.</typeparam>
        /// <param name="interceptor">The interceptor.</param>
        /// <param name="containsProperies">if set to <c>true</c> contains properies to interception.</param>
        /// <returns>Instance of <typeparamref name="T"/> implament as proxy generator.</returns>
        /// <exception cref="ArgumentNullException">interceptor</exception>
        public T GenerateProxy<T>(IInterceptor interceptor, bool containsProperies)
            where T : class
        {
            if (interceptor == null)
            {
                throw new ArgumentNullException(nameof(interceptor));
            }

            Type interfaceType = typeof(T);

            Type proxyType = this.generatedTypeList.EnshureType(interfaceType,
                containsProperies ? TypedDecoratorType.TypedProxyWithParameters : TypedDecoratorType.TypedProxy,
                type => this.BuildProxyType(type, containsProperies));

            return (T)this.CreateTypedProxyInstance(proxyType, interceptor);
        }

        /// <summary>
        /// Generates the proxy instance with interceptor.
        /// </summary>
        /// <param name="interfaceType">Type of the interface fo implementation proxy.</param>
        /// <param name="interceptor">The interceptor.</param>
        /// <returns>Instance of implementator of type <paramref name="interfaceType"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// interfaceType
        /// or
        /// interceptor
        /// </exception>
        public object GenerateProxy(Type interfaceType, IInterceptor interceptor)
        {
            return this.GenerateProxy(interfaceType, interceptor);
        }

        /// <summary>
        /// Generates the proxy instance with interceptor.
        /// </summary>
        /// <param name="interfaceType">Type of the interface fo implementation proxy.</param>
        /// <param name="interceptor">The interceptor.</param>
        /// <param name="containsProperies">if set to <c>true</c> contains properies to interception.</param>
        /// <returns>Instance of implementator of type <paramref name="interfaceType"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// interfaceType
        /// or
        /// interceptor
        /// </exception>
        public object GenerateProxy(Type interfaceType, IInterceptor interceptor, bool containsProperies)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }

            if (interceptor == null)
            {
                throw new ArgumentNullException(nameof(interceptor));
            }

            Type proxyType = this.generatedTypeList.EnshureType(interfaceType,
                containsProperies ? TypedDecoratorType.TypedProxyWithParameters : TypedDecoratorType.TypedProxy,
                type => this.BuildProxyType(type, containsProperies));

            return this.CreateTypedProxyInstance(proxyType, interceptor);
        }

        /// <summary>
        /// Generates the proxy with multiple interfaces implementation.
        /// </summary>
        /// <typeparam name="T">Type of base interface.</typeparam>
        /// <param name="interceptor">The interceptor.</param>
        /// <param name="additionalTypes">The additional interface types.</param>
        /// <returns>Instance of prxy class generatet with multiple interfaces.</returns>
        /// <exception cref="ArgumentNullException">
        /// interceptor
        /// or
        /// additionalTypes
        /// </exception>
        public T GenerateProxy<T>(IInterceptor interceptor, params Type[] additionalTypes)
            where T : class
        {
            if (interceptor == null)
            {
                throw new ArgumentNullException(nameof(interceptor));
            }

            if (additionalTypes == null)
            {
                throw new ArgumentNullException(nameof(additionalTypes));
            }

            Type[] interfaceTypes = new Type[additionalTypes.Length + 1];
            for (int i = 0; i < additionalTypes.Length; i++)
            {
                interfaceTypes[i] = additionalTypes[i];
            }

            interfaceTypes[interfaceTypes.Length - 1] = typeof(T);

            Type proxyType = this.generatedTypeList.EnshureType(interfaceTypes, TypedDecoratorType.TypedProxy, this.GenerateType);   // this.GenerateType(interfaceTypes);

            return (T)this.CreateTypedProxyInstance(proxyType, interceptor);
        }

        /// <summary>
        /// Generates the proxy with multiple interfaces implementation.
        /// </summary>
        /// <param name="interceptor">The interceptor.</param>
        /// <param name="additionalTypes">The additional interface types.</param>
        /// <returns>
        /// Instance of prxy class generatet with multiple interfaces.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// interceptor
        /// or
        /// additionalTypes
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">additionalTypes</exception>
        public object GenerateProxy(IInterceptor interceptor, params Type[] additionalTypes)
        {
            if (interceptor == null)
            {
                throw new ArgumentNullException(nameof(interceptor));
            }

            if (additionalTypes == null)
            {
                throw new ArgumentNullException(nameof(additionalTypes));
            }

            if (additionalTypes.Length < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(additionalTypes)); // TODO: doplnit lepsiu spravu
            }

            Type proxyType = this.generatedTypeList.EnshureType(additionalTypes, TypedDecoratorType.TypedProxy, this.GenerateType);   // this.GenerateType(interfaceTypes);

            return this.CreateTypedProxyInstance(proxyType, interceptor);
        }

        /// <summary>
        /// Generates the proxy with multiple interfaces implementation.
        /// </summary>
        /// <param name="interfaceType">Type of the interface for decorator.</param>
        /// <param name="interceptor">The interceptor.</param>
        /// <param name="additionalTypes">The additional interface types.</param>
        /// <returns>
        /// Instance of prxy class generatet with multiple interfaces.
        /// </returns>
        /// <exception cref="ArgumentNullException">interceptor</exception>
        public object GenerateProxy(Type interfaceType, IInterceptor interceptor, params Type[] additionalTypes)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }

            if (interceptor == null)
            {
                throw new ArgumentNullException(nameof(interceptor));
            }

            Type[] interfaceTypes = new Type[additionalTypes.Length + 1];
            for (int i = 0; i < additionalTypes.Length; i++)
            {
                interfaceTypes[i] = additionalTypes[i];
            }

            interfaceTypes[interfaceTypes.Length - 1] = interfaceType;

            Type proxyType = this.generatedTypeList.EnshureType(interfaceTypes, TypedDecoratorType.TypedProxy, this.GenerateType);   // this.GenerateType(interfaceTypes);

            return this.CreateTypedProxyInstance(proxyType, interceptor);
        }

        /// <summary>
        /// Generates the instance proxy.
        /// </summary>
        /// <typeparam name="T">Type of proxy.</typeparam>
        /// <param name="instanceProvider">The instance provider.</param>
        /// <returns>Instance of proxy class with instance provider.</returns>
        /// <exception cref="ArgumentNullException">instanceProvider</exception>
        public T GenerateInstanceProxy<T>(IInstanceProvicer instanceProvider)
            where T : class
        {
            if (instanceProvider == null)
            {
                throw new ArgumentNullException(nameof(instanceProvider));
            }

            Type interfaceType = typeof(T);

            Type proxyType = this.generatedTypeList.EnshureType(interfaceType,
                TypedDecoratorType.TypedInstancedProxy,
                this.BuildInstanceProxyType);

            return (T)this.CreateGenerateInstanceProxy(proxyType, instanceProvider);
        }

        /// <summary>
        /// Generates the decorator.
        /// </summary>
        /// <typeparam name="T">Type of decorator.</typeparam>
        /// <param name="interceptor">The interceptor.</param>
        /// <param name="parent">The parent.</param>
        /// <returns>Instance of decorator.</returns>
        /// <exception cref="ArgumentNullException">
        /// interceptor
        /// or
        /// parent
        /// </exception>
        public T GenerateDecorator<T>(ICallableInterceptor interceptor, T parent)
          where T : class
        {
            if (interceptor == null)
            {
                throw new ArgumentNullException(nameof(interceptor));
            }

            if (object.ReferenceEquals(parent, null))
            {
                throw new ArgumentNullException(nameof(parent));
            }

            Type interfaceType = typeof(T);
            Type proxyType = this.generatedTypeList.EnshureType(interfaceType,
                TypedDecoratorType.TypedDecorator,
                this.BuildDecoratorType);

            return (T)this.CreateDecoratorInstance(interceptor, parent, interfaceType, proxyType);
        }

        ///// <summary>
        ///// Generate <c>dynamic</c> object proxy with inteceptor.
        ///// </summary>
        ///// <param name="interceptor">The interceptor.</param>
        ///// <returns>Dynamic proxy object.</returns>
        ///// <exception cref="System.ArgumentNullException">interceptor</exception>
        //public dynamic GenerateDynamicObjectProxy(IInterceptor interceptor)
        //{
        //    if (interceptor == null)
        //    {
        //        throw new ArgumentNullException(nameof(interceptor));
        //    }
        //
        //    DynamicProxyObject dynamicObject = new DynamicProxyObject(interceptor);
        //
        //    return dynamicObject;
        //}

        /// <summary>
        /// Saves the assembly with file name for testing.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <exception cref="ArgumentNullException">fileName</exception>
        internal void Save(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (!fileName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
            {
                fileName = string.Concat(fileName.TrimEnd(), ".dll");
            }

            this.assemblyBuilder.Save(fileName);
        }

        private Type GenerateType(Type[] interfaceTypes)
        {
            TypeBuilder typeBuilder = this.CreateEmptyType(interfaceTypes);
            TypedProxyGenerator generator = new TypedProxyGenerator(typeBuilder, true);

            for (int i = 0; i < interfaceTypes.Length; i++)
            {
                generator.CheckType(interfaceTypes[i]);
            }

            for (int i = 0; i < interfaceTypes.Length; i++)
            {
                generator.ImplementInterface(interfaceTypes[i]);
            }

#if COREFX
            TypeInfo proxyType = typeBuilder.CreateTypeInfo();
            return proxyType.AsType();
#else
            Type proxyType = typeBuilder.CreateType();
            return proxyType;
#endif

        }

        private Type BuildProxyType(Type interfaceType, bool containsProperies)
        {
            TypeBuilder typeBuilder = this.CreateEmptyType(interfaceType);
            TypedProxyGenerator generator = new TypedProxyGenerator(typeBuilder, containsProperies);
            generator.CheckType(interfaceType);
            generator.ImplementInterface(interfaceType);
            Type proxyType = typeBuilder.CreateType();

            return proxyType;
        }

        private object CreateTypedProxyInstance(Type proxyType, IInterceptor interceptor)
        {
            ConstructorInfo constructor = proxyType.GetTypeInfo().GetConstructor(new Type[] { typeof(IInterceptor) });
            ParameterExpression param = Expression.Parameter(typeof(IInterceptor), "interceptor");
            return Expression.Lambda<Func<IInterceptor, object>>(Expression.New(constructor, new Expression[] { param }), param).Compile().Invoke(interceptor);
        }

        private Type BuildInstanceProxyType(Type interfaceType)
        {
            TypeBuilder typeBuilder = this.CreateEmptyType(interfaceType);
            TypedInstanceProxyGenerator generator = new TypedInstanceProxyGenerator(typeBuilder);
            generator.CheckType(interfaceType);
            generator.ImplementInterface(interfaceType);
            Type proxyType = typeBuilder.CreateType();

            return proxyType;
        }

        private object CreateGenerateInstanceProxy(Type proxyType, IInstanceProvicer instanceProvider)
        {
            ConstructorInfo constructor = proxyType.GetTypeInfo().GetConstructor(new Type[] { typeof(IInstanceProvicer) });
            ParameterExpression param = Expression.Parameter(typeof(IInstanceProvicer), "instanceProvider");
            return Expression.Lambda<Func<IInstanceProvicer, object>>(Expression.New(constructor, new Expression[] { param }), param).Compile().Invoke(instanceProvider);
        }

        private Type BuildDecoratorType(Type interfaceType)
        {
            TypeBuilder typeBuilder = this.CreateEmptyType(interfaceType);
            TypedDecoratorGenerator generator = new TypedDecoratorGenerator(typeBuilder, this.typeNameCreator);
            generator.CheckType(interfaceType);
            generator.ImplementInterface(interfaceType);
            Type proxyType = typeBuilder.CreateType();

            return proxyType;
        }

        private T CreateDecoratorInstance<T>(ICallableInterceptor interceptor, T parent, Type t, Type proxyType)
            where T : class
        {
            ConstructorInfo constructor = proxyType.GetTypeInfo().GetConstructor(new Type[] { typeof(ICallableInterceptor), t });
            ParameterExpression interceptorParam = Expression.Parameter(typeof(ICallableInterceptor), "interceptor");
            ParameterExpression parentParam = Expression.Parameter(typeof(T), "parent");
            return Expression.Lambda<Func<ICallableInterceptor, T, T>>(Expression.New(constructor, new Expression[] { interceptorParam, parentParam }), interceptorParam, parentParam)
                .Compile()
                .Invoke(interceptor, parent);
        }

        private void EnshureAssemblies()
        {
            if (this.assemblyBuilder != null)
            {
                return;
            }

            AssemblyName asmName = new AssemblyName();

            asmName.Name = this.assemblyName;
            asmName.Version = new Version(1, 0, 0, 0);

            // TODO: pre debug
            // AssemblyBuilder asmBuilder = Thread.GetDomain().DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave, @"C:\Users\harrison\Documents\Visual Studio 2015\Projects\MassiveDynamicProxyGenerator\MassiveDynamicProxyGenerator\bin\Debug");
            // ModuleBuilder modBuilder = asmBuilder.DefineDynamicModule("DynamicProxyModule", "Testing.dll");

#if COREFX
            AssemblyBuilder asmBuilder = AssemblyBuilder.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
#else
            AssemblyBuilder asmBuilder = Thread.GetDomain().DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
#endif

            ModuleBuilder modBuilder = asmBuilder.DefineDynamicModule("DynamicProxyModule");

            this.assemblyBuilder = asmBuilder;
            this.moduleBuilder = modBuilder;
        }

        private TypeBuilder CreateEmptyType(Type baseInterfaceType)
        {
            this.EnshureAssemblies();

            string typeName = this.typeNameCreator.CreateTypeName(baseInterfaceType.Name.TrimStart('I'), 32);
            TypeBuilder typeBuilder = this.moduleBuilder.DefineType(
                                    string.Concat(this.assemblyName, ".", typeName),
                                    TypeAttributes.Public,
                                    typeof(object),
                                    new Type[] { baseInterfaceType });

            return typeBuilder;
        }

        private TypeBuilder CreateEmptyType(Type[] interfaceTypes)
        {
            this.EnshureAssemblies();

            string typeName = this.typeNameCreator.CreateTypeName("Mi", 32);
            TypeBuilder typeBuilder = this.moduleBuilder.DefineType(
                                    string.Concat(this.assemblyName, ".", typeName),
                                    TypeAttributes.Public,
                                    typeof(object),
                                    interfaceTypes);

            return typeBuilder;
        }
    }
}
