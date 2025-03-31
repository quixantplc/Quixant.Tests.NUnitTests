using System;
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
        return NativeLibWrapper.nativeFunction5(integerVal);
    }
}

public static class NativeLibWrapper
{
    private const string DllName = "libmocked.dll";

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

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint nativeFunction1(out IntPtr handle, string charPointer);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint nativeFunction2(IntPtr handle, STRUCT1 struct1);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint nativeFunction3(IntPtr handle, out STRUCT1 struct1);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint nativeFunction4(IntPtr handle, STRUCT3 struct3);



    public delegate uint nativeFunction5Delegate(int integerVal);
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint nativeFunction5(int integerVal);

}