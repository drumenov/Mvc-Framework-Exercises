using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework.Services.Contracts
{
    public interface IDependencyContainer
    {
        void RegisterDependency<TSource, TDestinarion>();

        T CreateInstance<T>();

        object CreateInstance(Type type);
    }
}
