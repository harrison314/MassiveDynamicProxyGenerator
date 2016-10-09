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

namespace MassiveDynamicProxyGenerator
{
    public class ProxygGenerator
    {
        private static int assemblyCount = 1;
        private readonly string assemblyName;
        private readonly ITypeNameCreator typeNameCreator;
        private readonly GeneratedTypeList generatedTypeList;
        private AssemblyBuilder assemblyBuilder;
        private ModuleBuilder moduleBuilder;

        public ProxygGenerator()
        {
            this.assemblyBuilder = null;
            this.moduleBuilder = null;
            this.typeNameCreator = DefaultInstances.TypeNameCreator;
            int number = Interlocked.Increment(ref assemblyCount);
            this.assemblyName = string.Format(CultureInfo.InvariantCulture, "MassiveDynamic.DynamicProxys{0}", number);
            this.generatedTypeList = DefaultInstances.TypedList;
        }

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
                this.assemblyName = settings.AssemblyName;
            }
            else
            {
                int number = Interlocked.Increment(ref assemblyCount);
                this.assemblyName = string.Format(CultureInfo.InvariantCulture, "MassiveDynamic.DynamicProxys{0}", number);
            }
        }

        public ProxygGenerator(Action<ProxygGeneratorSettings> configure)
            : this(SettingsUtils.Apply(configure, new ProxygGeneratorSettings()))
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }
        }

        public T GenerateProxy<T>(IInterceptor interceptor)
            where T : class
        {
            return this.GenerateProxy<T>(interceptor, false);
        }

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

        public object GenerateProxy(Type interfaceType, IInterceptor interceptor)
        {
            return this.GenerateProxy(interfaceType, interceptor);
        }

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

        public T GenerateProxy<T>(IInterceptor interceptor, params Type[] additionalTypes)
            where T : class
        {
            if (interceptor == null)
            {
                throw new ArgumentNullException(nameof(interceptor));
            }

            Type[] interfaceTypes = new Type[additionalTypes.Length + 1];
            for (int i = 0; i < additionalTypes.Length; i++)
            {
                interfaceTypes[i] = additionalTypes[i];
            }

            interfaceTypes[interfaceTypes.Length - 1] = typeof(T);

            Type proxyType = this.GenerateType(interfaceTypes);

            return (T)this.CreateTypedProxyInstance(proxyType, interceptor);
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

            Type proxyType = typeBuilder.CreateType();
            return proxyType;
        }

        public object GenerateProxy(IInterceptor interceptor, params Type[] additionalTypes)
        {
            if (interceptor == null)
            {
                throw new ArgumentNullException(nameof(interceptor));
            }

            if (additionalTypes.Length < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(additionalTypes)); // TODO: doplnit lepsiu spravu
            }

            Type proxyType = this.GenerateType(additionalTypes);

            return this.CreateTypedProxyInstance(proxyType, interceptor);
        }

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

            Type proxyType = this.GenerateType(interfaceTypes);

            return this.CreateTypedProxyInstance(proxyType, interceptor);
        }

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
            ConstructorInfo constructor = proxyType.GetConstructor(new Type[] { typeof(IInterceptor) });
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
            ConstructorInfo constructor = proxyType.GetConstructor(new Type[] { typeof(IInstanceProvicer) });
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
            ConstructorInfo constructor = proxyType.GetConstructor(new Type[] { typeof(ICallableInterceptor), t });
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
            AssemblyBuilder asmBuilder = Thread.GetDomain().DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
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
