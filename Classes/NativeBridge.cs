using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TestRunner;

public partial class NativeBridge
{
    private const string DllName = "MockedLib.dll";

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate uint native_callback(nint methodName, object handle);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate uint native_callback_qxcom(out nint handle, byte[] portNameBytes);

    [LibraryImport(DllName, EntryPoint = "set_native_callback")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void set_native_callback(nint callbackPtr);
    [LibraryImport(DllName, EntryPoint = "fill_structures")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void fill_structures(nint dictionary, int count);

    internal static void SetCallback(native_callback callback)
    {
        set_native_callback(Marshal.GetFunctionPointerForDelegate(callback));
    }

    internal static void SetCallbackQxCom(native_callback_qxcom callback)
    {
        set_native_callback(Marshal.GetFunctionPointerForDelegate(callback));
    }

    internal static void FillStructures(nint dictionary, int count)
    {
        fill_structures(dictionary, count);
    }
}