namespace TestNUnitCallback.Helpers
{
    public static class FodyHelper
    {
        public static int ExecuteWithMockHandling(Func<int> methodToTest)
        {
            try
            {
                return methodToTest();
            }
            catch (ReturnValueException e)
            {
                return (int)e.ReturnValue;
            }
        }
    }
}
