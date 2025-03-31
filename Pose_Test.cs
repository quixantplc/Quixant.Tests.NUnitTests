using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Pose;
using TestNUnitCallback.Fake_Classes;
using static TestNUnitCallback.Fake_Classes.Wrapper_Library;

namespace TestNUnitCallback
{
    public class Pose_Test
    {
        [Test]
        public void Open_OK()
        {
            Shim openShim = Shim.Replace(() => Native.Open()).With(
                () => { return 345; });

            int result = 0;
            // This block executes immediately
            PoseContext.Isolate(() =>
            {
                // All code that executes within this block
                // is isolated and shimmed methods are replaced

                var wrapper = new Wrapper_Library();
                result = wrapper.Open();

            }, openShim);

            Assert.That(result, NUnit.Framework.Is.EqualTo(345));
        }

        [Test]
        public void Open_Ko()
        {
            InvalidOperationException ex = null;
            Shim openShim = Shim.Replace(() => Native.Open())
                .With<int>(() => { throw new InvalidOperationException("An Error occurred."); }); // we need the <int> 'cause otherwise it doesn't know what to return since we throw an exception
            try
            {
                PoseContext.Isolate(() =>
                {
                    var wrapper = new Wrapper_Library();
                    wrapper.Open();
                    
                }, openShim);
            }
            catch (TargetInvocationException e)
            {
                ex = (InvalidOperationException)e.InnerException;
            }
            
            Assert.That(ex, NUnit.Framework.Is.TypeOf<InvalidOperationException>());
            Assert.That(ex.Message, NUnit.Framework.Is.EqualTo("An Error occurred."));
        }

        [Test]
        public void OpenComplex_OK()
        {
            Shim openShim = Shim.Replace(() => Native.Open()).With(
                () => { return 1; });
            Inventory inventory_mock = new()
            {
                library_version = 10
            };
            inventory_mock.port_name.CopyArrayFrom(Encoding.UTF8.GetBytes("Random Port Name")[..]);
            Shim getInventoryShim = Shim.Replace(() => Native.GetInventory(Pose.Is.A<nint>(), out inventory_mock)).With(
                (nint a, out Inventory inventory) =>
                {
                    inventory = inventory_mock;
                    return 5;
                });
            Shim setParamsShim = Shim.Replace(() => Native.SetParameters(Pose.Is.A<nint>(), Pose.Is.A<Inventory>())).With(
                (nint a, Inventory inventory) => { return 2; });

            var callback = (nint sourcePort, ushort eventMask) =>
            {
                Console.WriteLine("Callback Called");
            };
            Shim setEventMask = Shim.Replace(() => Native.SetEventMask(Pose.Is.A<nint>(), new CALLBACK(callback))).With(
                (nint a, CALLBACK callback) => { return 3; });

            // Any static function that is called within the block needs to be shimmed.
            Shim stringFormat = Shim.Replace(() => string.Format(Pose.Is.A<string>(), Pose.Is.A<object>())).With(
                (string format, object args) =>
                {
                    return string.Format(format, args);
                });
            Shim stringFormatArray = Shim.Replace(() => string.Format(Pose.Is.A<string>(), Pose.Is.A<object>(), Pose.Is.A<object>())).With(
                (string format, object args, object args1) =>
                {
                    return string.Format(format, args, args1);
                });
            Shim stringOperator = Shim.Replace(() => string.Concat(Pose.Is.A<string?>(), Pose.Is.A<string?>())).With(
                (string? a, string? b) =>
                {
                    return string.Concat(a,b);
                });

            string[] results = [];

            try
            {
                PoseContext.Isolate(() =>
                {
                    var wrapper = new Wrapper_Library();
                    results = wrapper.ComplexOpen();

                }, openShim, getInventoryShim, setParamsShim, setEventMask, stringFormat, stringFormatArray);
            }
            catch (Exception e)
            {
                
            }

            string res = string.Concat(results);
            Assert.That(res, NUnit.Framework.Is.EqualTo("Open: 1\nGetInventory - Lib Version: 10\n Port Name: Random Port Name\nSetParameters: 2\nSetEventMask: 3\n"));
        }
    }
}
