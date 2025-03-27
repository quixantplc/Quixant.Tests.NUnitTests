using TestNUnitCallback.Fake_Classes;
using TestNUnitCallback.Helpers;

namespace TestNUnitCallback
{
    public class FodyTests
    {
        [TearDown]
        public void CleanUp()
        {
           
        }

        [Test]
        public void Test_Open()
        {
            MockStaticInterceptorAttribute.MockMethod("Open", args => 999); // here we can return a call against the c++ bridge which return back the params we sent to our mocked function

            int result = 0;

            try
            {
                var wrapper = new Wrapper_Library();
                result = wrapper.Open();
            }
            catch (ReturnValueException e)
            {
                result = (int)e.ReturnValue;
            }

            // or using the helper to clean the code a bit var result = TestHelper.ExecuteWithMockHandling(() => wrapper.Open());

            Assert.That(result, Is.EqualTo(999));
        }


        [Test]
        public void Test_Open_fail()
        {
            MockStaticInterceptorAttribute.MockMethod("Open", (args) =>
            {
                // Simulates an exception
                throw new InvalidOperationException("Mock Error");
            });

            var wrapper = new Wrapper_Library();
            var ex = Assert.Throws<InvalidOperationException>(() => wrapper.Open()); // doesn't work at the moment
            Assert.That(ex.Message, Is.EqualTo("Mock Error"));
        }
    }
}