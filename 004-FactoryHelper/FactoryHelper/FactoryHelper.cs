using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

namespace FactoryHelper
{
    public static class FactoryHelper
    {
        public static nint Factory(string assemblyName, string typeName, string methodName, int ctorParams, params object[] args)
        {
            Console.WriteLine("FactoryHelper.Factory called");
            Assembly assembly = Assembly.Load(assemblyName);
            object? instance = assembly.CreateInstance(typeName,false, BindingFlags.Default, null,
                ctorParams == 0 ? null : args[..(ctorParams-1)], null, null);
            object? res = assembly.GetType(typeName).GetMethod(methodName).Invoke(instance, args[ctorParams..]);
            GCHandle handle = GCHandle.Alloc(res, GCHandleType.Pinned);
            return handle.AddrOfPinnedObject();
        }

        public delegate nint FactoryDelegate(string assemblyName, string typeName, string methodName, int ctorParams, params object[] args);
    }
}
