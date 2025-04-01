using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// ndr: make the generated dll assembly being copied to the bin directory
// ndr: store the DotNetWrapper.runtimeconfig.json file as a master in a location inside this project and make it being copied to the bin directory as well 


public static class WrapperTesterHelper
{

}


public class NonStaticWrapper
{
    public uint Function5(int integerVal)
    {
        return NativeLibWrapper.nativeFunction5Delegate(integerVal);
    }
}

public partial class NativeLibWrapper
{
    private const string DllName = "MockedLib.dll";

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct STRUCT1
    {
        public uint BaudRate;
        public uint Databits;
        public uint Parity;
        public uint StopBits;
        public byte EnableTimestamps;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct STRUCT3
    {
        public long IntervalTimeout;
        public long ReadTimeout;
        public long WriteTimeout;
    }

    //[LibraryImport(DllName, EntryPoint = "nativeFunction1")]
    //[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    //private static partial uint nativeFunction1(out IntPtr handle, string charPointer);

    //[LibraryImport(DllName, EntryPoint = "nativeFunction2")]
    //[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    //private static partial uint nativeFunction2(IntPtr handle, STRUCT1 struct1);

    //[LibraryImport(DllName, EntryPoint = "nativeFunction3")]
    //[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    //private static partial uint nativeFunction3(IntPtr handle, out STRUCT1 struct1);

    //[LibraryImport(DllName, EntryPoint = "nativeFunction4")]
    //[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    //private static partial uint nativeFunction4(IntPtr handle, STRUCT3 struct3);


    //public delegate uint nativeFunction5Delegate(int integerVal);
    [LibraryImport(DllName, EntryPoint = "nativeFunction5")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial uint nativeFunction5(int integerVal);

    internal static uint nativeFunction5Delegate(int integerVal)
    {
        return nativeFunction5(integerVal);
    }
}