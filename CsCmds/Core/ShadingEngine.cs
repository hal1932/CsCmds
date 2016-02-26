using Autodesk.Maya.OpenMaya;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CsCmds.Core
{
    public class ShadingEngine : ObjectSet
    {
        internal ShadingEngine(MObject obj)
            : base(obj)
        { }

        public static new ShadingEngine DownCastFrom(DependNode node)
        {
            return (node.HasFn(MFn.Type.kShadingEngine)) ?
                new ShadingEngine(node.MObject) : null;
        }

        public static ShadingEngine DownCastFrom(ObjectSet node)
        {
            return (node.HasFn(MFn.Type.kShadingEngine)) ?
                new ShadingEngine(node.MObject) : null;
        }

        #region enumerate
        public static new IEnumerable<ShadingEngine> Enumerate(Func<string, bool> filter = null)
        {
            return EnumerateRaw(filter, MFn.Type.kShadingEngine)
                .Select(obj => new ShadingEngine(obj));
        }
        #endregion
    }
}
