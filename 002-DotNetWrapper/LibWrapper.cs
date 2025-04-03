using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using static DotNetWrapper.QxComNativeWrapper;

// ndr: make the generated dll assembly being copied to the bin directory
// ndr: store the DotNetWrapper.runtimeconfig.json file as a master in a location inside this project and make it being copied to the bin directory as well 
namespace DotNetWrapper
{
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

    public class QxComNonStaticWrapper
    {
        private nint _handle;
        private string _portName = "RandomPortName";
        private QXCOM_INVENTORY _inventory;
        private QXCOM_TIMEOUTS _timeouts;
        protected byte[] _portNameBytes
        {
            get
            {
                if (string.IsNullOrEmpty(_portName))
                    return [0x0];

                byte[] notTerminatedString, result;

                notTerminatedString = ASCIIEncoding.ASCII.GetBytes(_portName);
                result = new byte[notTerminatedString.Length + 1];
                Array.Copy(notTerminatedString, result, notTerminatedString.Length);
                result[result.Length - 1] = 0x0;
                return result;
            }
        }
        public bool Open()
        {
            QxComNativeWrapper.QxComOpen(out _handle, _portNameBytes);
            QxComNativeWrapper.QxComGetInventory(_handle, out _inventory);
            QxComNativeWrapper.QxComSetCommTimeouts(_handle, out _timeouts);

            return true;
        }
    }
    internal partial class QxComNativeWrapper
    {
        private const string DllName = "MockedLib.dll";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        internal struct QXCOM_INVENTORY
        {
            // public PortNameBuffer PortName;
            // public SerialPortType PortType;
            public uint LibraryVersion;
            public uint DriverVersion;
            public uint FpgaVersion;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        internal struct QXCOM_TIMEOUTS
        {
            public long IntervalTimeout;
            public long ReadTimeout;
            public long WriteTimeout;
        }


        [LibraryImport(DllName, EntryPoint = "qxComOpen")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        private static partial uint qxComOpen(out nint handle, byte[] portName);

        public static uint QxComOpen(out nint handle, byte[] portName)
        {
            return qxComOpen(out handle, portName);
        }


        [LibraryImport(DllName, EntryPoint = "qxComGetInventory")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        private static partial uint qxComGetInventory(nint handle, out QXCOM_INVENTORY inventory);

        public static uint QxComGetInventory(nint handle, out QXCOM_INVENTORY inventory)
        {
            return qxComGetInventory(handle, out inventory);
        }

        [LibraryImport(DllName, EntryPoint = "qxComSetCommTimeouts")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        private static partial uint qxComSetCommTimeouts(nint handle, out QXCOM_TIMEOUTS inventory);

        public static uint QxComSetCommTimeouts(nint handle, out QXCOM_TIMEOUTS timeouts)
        {
            return qxComSetCommTimeouts(handle, out timeouts);
        }
    }
}