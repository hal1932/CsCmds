using Autodesk.Maya.OpenMaya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsCmds.Core
{
    public class ShadingEngine : DependNode
    {
        internal ShadingEngine(MObject obj, MFnDependencyNode fn)
            : base(obj, fn)
        { }
    }
}
