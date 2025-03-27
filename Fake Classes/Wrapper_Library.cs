namespace TestNUnitCallback.Fake_Classes
{
    public class Wrapper_Library
    {
        public int Open()
        {
            return Native.Open();
        }

        public int Close()
        {
            return Native.Close();
        }
    }
}
