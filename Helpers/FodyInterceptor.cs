using System.Collections.Concurrent;
using System.Reflection;
using MethodDecorator.Fody.Interfaces;

namespace TestNUnitCallback.Helpers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MockStaticInterceptorAttribute : Attribute, IMethodDecorator
    {
        // we may also have the list of mocked methods we want and their default values. Later we can override it from within the tests
        private static readonly AsyncLocal<ConcurrentDictionary<string, Func<object[], object>>> _mockedMethods =
            new() { Value = new ConcurrentDictionary<string, Func<object[], object>>() };

        private MethodBase? _method;
        private object[]? _args;

        // first method will be called 
        public void Init(object instance, MethodBase method, object[] args)
        {
            _method = method;
            _args = args;
        }

        // here's the second. We have to save method and args somewhere, because we can't pass them to OnEntry and we don't which method we are calling
        public void OnEntry()
        {
            if (_method == null) return;

            string methodName = _method.Name;
            Console.WriteLine($"Intercepting call to: {methodName}");

            if (_mockedMethods.Value!.TryGetValue(methodName, out var fakeImplementation))
            {
                var result = fakeImplementation.Invoke(_args);
                throw new ReturnValueException(result);
            }
        }

        // it never goes here 'cause we return a value before
        public void OnExit() 
        {
            string methodName = _method!.Name;
            Console.WriteLine($"Exiting method: {methodName}");
        }

        // never goes here either
        public void OnException(Exception exception) 
        {
            Console.WriteLine($"Exception: {exception}");
        }

        // it actually mocks the method
        public static void MockMethod(string methodName, Func<object[], object> fakeImplementation)
        {
            _mockedMethods.Value!.TryAdd(methodName, fakeImplementation);
        }

        // can be invoked on setup or teardown
        public static void ClearMocks()
        {
            _mockedMethods.Value!.Clear();
        }
    }

    // We need an exception to force the method to return before it exits and get back to the actual method
    public class ReturnValueException : Exception
    {
        public object ReturnValue { get; }

        public ReturnValueException(object returnValue) => ReturnValue = returnValue;
    }
}