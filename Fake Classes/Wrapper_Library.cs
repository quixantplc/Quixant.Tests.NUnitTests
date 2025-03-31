using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

namespace TestNUnitCallback.Fake_Classes
{
    public class Wrapper_Library
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CALLBACK(nint sourcePort, ushort eventMask);

        private CALLBACK callback = (nint sourcePort, ushort eventMask) =>
        {
            Console.WriteLine("Callback Called");
        };

        [InlineArray(0x20)]
        internal struct PortNameBuffer
        {
            private byte _element0;

            public void CopyArrayFrom(byte[] source)
            {
                int size = Math.Min(source.Length, 0x20);

                for (int i = 0; i < size; i++)
                {
                    this[i] = source[i];
                }
            }
        }

        internal struct Inventory
        {
            public PortNameBuffer port_name;
            public uint library_version;
        }

        public int Open()
        {
            return Native.Open();
        }

        public string[] ComplexOpen()
        {
            string[] results = new string[4];
            int result = Native.Open();
            string openStr = string.Format("Open: {0}\n", result);
            Native.GetInventory(0, out Inventory inventory);
            string portname = Encoding.ASCII.GetString(inventory.port_name);
            string getInvStr = string.Format("GetInventory - Library Version {0}, Port Name: {1} \n", inventory.library_version, portname);
            // string.Concat(results, getInvStr); it has problems with strings concatenations
            result = Native.SetParameters(0, inventory);
            string setParams = string.Format("SetParameters: {0}\n", result.ToString());
            result = Native.SetEventMask(0, callback);
            string setEventMask = string.Format("SetEventMask: {0}", result.ToString());

            results[0] = openStr;
            results[1] = getInvStr;
            results[2] = setParams;
            results[3] = setEventMask;
            return results;
        }

        public int Close()
        {
            return Native.Close();
        }
    }
}
