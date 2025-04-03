using System.Runtime.InteropServices;
using DotNetWrapper;

namespace TestRunner
{
    public class Tests
    {
        public required NativeBridge.native_callback _platformCallback;

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Test1()
        {
            // set callback
            NativeBridge.SetCallback((methodName, handle) =>
            {
                Console.WriteLine("Callback called with method name: " + methodName + " and handle: " + handle);
                Assert.Multiple(() =>
                {
                    Assert.That(Marshal.PtrToStringUTF8(methodName), Is.EqualTo("nativeFunction5"));
                    Assert.That(handle, Is.EqualTo(5));
                });
                return 0;
            });
            // Calling function 5
            var nonStaticWrapper = new NonStaticWrapper();
            uint res = nonStaticWrapper.Function5(5);
            Assert.That(res, Is.EqualTo(10));
        }
    }

}