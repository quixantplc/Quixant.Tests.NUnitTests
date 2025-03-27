using TestNUnitCallback.Helpers;

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
