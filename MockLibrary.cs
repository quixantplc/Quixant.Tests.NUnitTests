using TestNUnitCallback.Fake_Classes;

namespace TestNUnitCallback.Mock
{
    public class MockLibrary : IMockLibrary
    {
        public int Open() => Native.Open();
        public int Close() => Native.Close();
    }

    public interface IMockLibrary
    {
        int Open();
        int Close();
    }
}
