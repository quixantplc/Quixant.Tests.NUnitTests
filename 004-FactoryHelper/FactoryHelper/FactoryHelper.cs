using System.Reflection;
using System.Reflection.Metadata;

namespace FactoryHelper
{
    public static class FactoryHelper
    {
        public static /*nint*/object? Factory(string assemblyName, string typeName, string methodName, int ctorParams, params object[] args)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            object? instance = assembly.CreateInstance(typeName,false, BindingFlags.Default, null, args[..(ctorParams-1)], null, null);
            object? res = assembly.GetType(typeName).GetMethod(methodName).Invoke(instance, args[ctorParams..]);
            //return (nint)res;
            return res;
        }
    }
}
