using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Maya.OpenMaya;
using CsCmds.Core;
using CsCmds.Dag;

[assembly: ExtensionPlugin(typeof(test.TestPlugin), "hal1932", "1.0", "Any")]
[assembly: MPxCommandClass(typeof(test.Test), "test")]

namespace test
{
    public class Test : MPxCommand, IMPxCommand
    {
        public override void doIt(MArgList args)
        {
        }
    }

    public class TestPlugin : IExtensionPlugin
    {
        public string GetMayaDotNetSdkBuildVersion()
        {
            return "201353";
        }

        public bool InitializePlugin()
        {
            return true;
        }

        public bool UninitializePlugin()
        {
            return true;
        }
    }
}
