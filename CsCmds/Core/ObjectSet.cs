using Autodesk.Maya.OpenMaya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsCmds.Core
{
    public class ObjectSet : DependNode
    {
        internal ObjectSet(MObject obj)
            : base(obj)
        { }

        public static ObjectSet DownCastFrom(DependNode node)
        {
            return (node.HasFn(MFn.Type.kSet)) ?
                new ObjectSet(node.MObject) : null;
        }

        #region enumerate
        public static new ObjectSet FirstOrDefault(Func<MFnDependencyNode, bool> filter = null)
        {
            return Enumerate(filter).FirstOrDefault();
        }

        public static IEnumerable<ObjectSet> Enumerate(Func<MFnDependencyNode, bool> filter = null)
        {
            return EnumerateRaw(filter, MFn.Type.kSet)
                .Select(obj => new ObjectSet(obj));
        }
        #endregion
    }
}
