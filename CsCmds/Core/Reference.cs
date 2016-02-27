using Autodesk.Maya.OpenMaya;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CsCmds.Core
{
    public class Reference : DependNode
    {
        public string NameSpace { get { return FnDependNode.name; } }

        private Reference(MObject obj)
            : base(obj)
        { }

        #region enumerate
        public static new Reference FirstOrDefault(Func<MFnDependencyNode, bool> filter = null)
        {
            return Enumerate(filter).FirstOrDefault();
        }

        public static IEnumerable<Reference> Enumerate(Func<MFnDependencyNode, bool> filter = null)
        {
            var iter = new MItDependencyNodes(MFn.Type.kReference);
            var tmpFn = new MFnDependencyNode();
            while (!iter.isDone)
            {
                if (filter != null)
                {
                    tmpFn.setObject(iter.item);
                    if (tmpFn.IsFilterd(filter))
                    {
                        continue;
                    }
                    yield return new Reference(iter.item);
                }
                else
                {
                    yield return new Reference(iter.item);
                }
                iter.next();
            }
        }
        #endregion

        public string GetFileName()
        {
            return MFileIO.getReferenceFileByNode(MObject);
        }

        public string GetFullName()
        {
            return MGlobal.executeCommandStringResult("referenceQuery -filename " + NameSpace);
        }
    }
}
