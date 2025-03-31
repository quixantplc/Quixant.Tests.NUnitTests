using NUnit.Framework.Constraints;
using TestNUnitCallback.Helpers;
using static TestNUnitCallback.Fake_Classes.Wrapper_Library;

namespace TestNUnitCallback.Fake_Classes
{
    internal class Native
    {
        [MockStaticInterceptor]
        public static int Open()
        {
            Console.WriteLine("Open Called");
            return 0;
        }

        [MockStaticInterceptor]
        public static int Close()
        {
            Console.WriteLine("Close Called");
            return 0;
        }

        public static int GetInventory(nint handle, out Inventory inventory)
        {
            inventory = new Inventory
            {
                library_version = 0,
                port_name = new PortNameBuffer()
            };
            return 0;
        }

        public static int SetParameters(nint handle, Inventory inventory)
        {
            return 0;
        }

        public static int SetEventMask(nint handle, CALLBACK inventory)
        {
            return 0;
        }

        internal static void GetInventory(ExactTypeConstraint exactTypeConstraint, out Inventory inventory_mock)
        {
            throw new NotImplementedException();
        }
    }

    #region NotStaticNative
    internal class NativeNotStatic
    {
        public int Open()
        {
            Console.WriteLine("Open Called");
            return 0;
        }

        public int Close()
        {
            Console.WriteLine("Close Called");
            return 0;
        }
    }

    public class NativeNotStatic_Wrapper
    {
        private static NativeNotStatic? _instance = null;
        public NativeNotStatic_Wrapper()
        {
            _instance ??= new NativeNotStatic();
        }
        public int Open()
        {
            return _instance.Open();
        }
    } 
    #endregion

}
