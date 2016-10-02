using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    internal abstract class AbstractTypeBuilder<T>
    {
        private readonly TypeBuilder typeBuilder;
        private readonly HashSet<Type> implementInterfaces;
        private bool isMembersImplements;

        public IEnumerable<Type> ImplementInterfaces
        {
            get
            {
                return this.implementInterfaces;
            }
        }

        protected TypeBuilder TypeBuilder
        {
            get
            {
                return this.typeBuilder;
            }
        }

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

        public virtual void CheckType(Type interfaceType)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }

            if (!interfaceType.IsInterface)
            {
                string message = string.Format("Type {0} is not interface.", interfaceType.FullName);
                throw new InvalidOperationException(message);
            }
        }

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

            foreach (Type secundaryInterfaceType in interfaceType.GetInterfaces())
            {
                if (this.implementInterfaces.Add(secundaryInterfaceType))
                {
                    this.ImplementSimpleInterface(secundaryInterfaceType);
                }
            }
        }

        protected virtual void ImplementFields(TypeBuilder typeBuilder, Type interfaceType)
        {
        }

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
            ConstructorInfo conObj = typeof(object).GetConstructor(new Type[0]);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, conObj);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ret);
        }

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

        protected virtual void GenerateMethod(MethodInfo interfaceMethod, Type[] parameters, Type interfaceType, ILGenerator il, T context)
        {
            ConstructorInfo ci = typeof(NotImplementedException).GetConstructor(new Type[0]);

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Newobj, ci);
            il.Emit(OpCodes.Throw);
        }

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

        protected virtual void GenerateSetProperty(PropertyInfo interfaceProperity, Type interfaceType, ILGenerator il, T context)
        {
            ConstructorInfo ci = typeof(NotImplementedException).GetConstructor(new Type[0]);

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Newobj, ci);
            il.Emit(OpCodes.Throw);
        }

        protected virtual void GenerateGetProperty(PropertyInfo interfaceProperity, Type interfaceType, ILGenerator il, T context)
        {
            ConstructorInfo ci = typeof(NotImplementedException).GetConstructor(new Type[0]);

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Newobj, ci);
            il.Emit(OpCodes.Throw);
        }

        private void ImplementSimpleInterface(Type interfaceType)
        {
            foreach (MethodInfo methodInfo in interfaceType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!methodInfo.IsSpecialName)
                {
                    this.ImplementMethod(this.TypeBuilder, interfaceType, methodInfo, default(T));
                }
            }

            foreach (PropertyInfo properityInfo in interfaceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                this.ImplementedProperity(this.TypeBuilder, interfaceType, properityInfo, default(T));
            }
        }
    }
}
