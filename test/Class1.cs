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
            Mel.Evaluate("polySphere");
            var sphere = DagNode.FirstOrDefault(node => node.name.StartsWith("pSphere"));
            Log.WriteLine(sphere.Name);
            // -> // pSphere1 //

            foreach (var childNode in sphere.EnumerateChildren())
            {
                Log.WriteLine(childNode.Name);
            }
            // -> // pSphereShape1 //

            var sphereShape = sphere.FirstChildOrDefault(node => node.hasFn(MFn.Type.kShape));
            Log.WriteLine(sphereShape.Name);
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
