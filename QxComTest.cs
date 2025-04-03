
using System.Runtime.InteropServices;
using System.Text;
using DotNetWrapper;
using Moq;
using static DotNetWrapper.QxComNativeWrapper;

namespace TestRunner
{
    public class QxComTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test_Open()
        {
            var mock = new Mock<IQxComMock>();
            nint handle = 0;
            string portName = "RandomPortNameCallback";
            byte[] portNameBytes = Encoding.UTF8.GetBytes(portName);
            QXCOM_TIMEOUTS timeouts = new QXCOM_TIMEOUTS();
            QXCOM_INVENTORY inventory = new QXCOM_INVENTORY();

            mock.Setup(x => x.QxComOpen(out handle, It.IsAny<byte[]>()))
                .Returns((out nint handle) =>
                {
                    Dictionary<IntPtr, (IntPtr, int)> dict = new();
                    handle = 0;
                    nint data = 0;
                    GCHandle h1 = GCHandle.Alloc(handle, GCHandleType.Pinned);
                    GCHandle d1 = GCHandle.Alloc(data, GCHandleType.Pinned);

                    int structSize = Marshal.SizeOf(typeof(nint));
                    dict[h1.AddrOfPinnedObject()] = (d1.AddrOfPinnedObject(), structSize);

                    KeyValuePairNative[] kvArray = new KeyValuePairNative[dict.Count];
                    int index = 0;
                    foreach (var kvp in dict)
                    {
                        kvArray[index].key = kvp.Key;
                        kvArray[index].value = kvp.Value.Item1;
                        kvArray[index].size = kvp.Value.Item2;
                        index++;
                    }

                    // Allochiamo memoria nativa per l'array
                    int structSizeNative = Marshal.SizeOf(typeof(KeyValuePairNative));
                    nint nativeDict = Marshal.AllocHGlobal(structSizeNative * kvArray.Length);

                    // Copiamo i dati in memoria nativa
                    for (int i = 0; i < kvArray.Length; i++)
                    {
                        nint ptr = nativeDict + (i * structSizeNative);
                        Marshal.StructureToPtr(kvArray[i], ptr, false);
                    }

                    MockResponse res = new()
                    {
                        response = 0
                    };
                    res.struct_dictionary = nativeDict;
                    return res;
                }
                );
            mock.Setup(x => x.QxComSetCommTimeouts(It.IsAny<nint>(), out timeouts))
                .Callback((nint value, out QXCOM_TIMEOUTS timeouts) =>
                {
                    timeouts = new QXCOM_TIMEOUTS() { IntervalTimeout = 100, ReadTimeout = 200, WriteTimeout = 300 };
                })
                .Returns(0);
            mock.Setup(x => x.QxComGetInventory(It.IsAny<nint>(), out inventory))
                .Callback((nint value, out QXCOM_INVENTORY inventory) =>
                {
                    inventory = new QXCOM_INVENTORY() { DriverVersion = 100, FpgaVersion = 300, LibraryVersion = 900 };
                })
                .Returns(0);

            // set callback
            //NativeBridge.SetCallbackQxCom());

            var nonStaticWrapper = new QxComNonStaticWrapper();
            bool res = nonStaticWrapper.Open();
            Assert.Multiple(() =>
            {
                Assert.That(res, Is.EqualTo(true));
                Assert.That(handle, Is.EqualTo(3));
            });
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    struct KeyValuePairNative
    {
        public nint key;   // Puntatore alla struttura da aggiornare
        public nint value; // Puntatore ai dati da scrivere
        public int size;  // Dimensione della struttura
    }

    public record MockResponse
    {
        public object? response;
        public nint? struct_dictionary = new();

    }

    internal interface IQxComMock
    {
        public MockResponse QxComOpen(out nint handle, byte[] portName);

        public uint QxComSetCommTimeouts(nint handle, out QXCOM_TIMEOUTS timeouts);

        public uint QxComGetInventory(nint handle, out QXCOM_INVENTORY inventory);
    }
}
