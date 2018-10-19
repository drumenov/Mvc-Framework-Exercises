using SIS.Framework.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SIS.Framework.Services
{
    public class DependencyContainer : IDependencyContainer
    {
        private readonly IDictionary<Type, Type> dependencyDictionary;

        private Type this[Type key] => this.dependencyDictionary.ContainsKey(key) ? this.dependencyDictionary[key] : null;

        public DependencyContainer() {
            this.dependencyDictionary = new Dictionary<Type, Type>();
        }

        public T CreateInstance<T>() => (T)this.CreateInstance(typeof(T));

        public object CreateInstance(Type type) {
            Type instanceType = this[type] ?? type;

            if (instanceType.IsInterface || instanceType.IsAbstract) {
                throw new InvalidOperationException($"Type {instanceType.FullName} cannot be instantiated.");
            }

            ConstructorInfo constructor = instanceType.GetConstructors().OrderByDescending(x => x.GetParameters().Length).First();
            ParameterInfo[] constructorParameters = constructor.GetParameters();
            object[] parametersToBePassedToConstructor = new object[constructorParameters.Length];

            for (int i = 0; i < constructorParameters.Length; i++) {
                parametersToBePassedToConstructor[i] = this.CreateInstance(constructorParameters[i].ParameterType);
            }

            return constructor.Invoke(parametersToBePassedToConstructor);
        }

        public void RegisterDependency<TSource, TDestinarion>() {
            this.dependencyDictionary[typeof(TSource)] = typeof(TDestinarion);
        }
    }
}
