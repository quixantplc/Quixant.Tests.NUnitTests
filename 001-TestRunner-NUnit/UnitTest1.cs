using System.Runtime.InteropServices;

namespace TestRunner
{
    public class Tests
    {
        private readonly NativeBridge.NativeCallback _platformCallback;

        [SetUp]
        public void Setup()
        {
            _platformCallback = new NativeBridge.NativeCallback((methodName, handle) =>
            {
                handle = 0;
                return 0;
            });
        }

        [Test]
        public void Test1()
        {
            // Calling function 5
            var nonStaticWrapper = new NonStaticWrapper();
        }
    }

    public class NativeBridge
    {
        [DllImport("libmocked.dll", CallingConvention = CallingConvention.Cdecl)]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint NativeCallback(byte[] methodName, int handle);
    }
}