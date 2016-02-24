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
        public static IEnumerable<ObjectSet> Enumerate(Func<string, bool> filter = null)
        {
            var iter = new MItDependencyNodes(MFn.Type.kSet);
            var tmpFn = new MFnDependencyNode();
            while (!iter.isDone)
            {
                tmpFn.setObject(iter.item);
                if (!tmpFn.name.IsFilterd(filter))
                {
                    yield return new ObjectSet(iter.item);
                }
                iter.next();
            }
        }
        #endregion
    }
}
