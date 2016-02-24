using Autodesk.Maya.OpenMaya;
using System;
using System.Collections.Generic;

namespace CsCmds.Core
{
    public class Reference : DependNode
    {
        public string NameSpace { get { return FnDependNode.name; } }

        private Reference(MObject obj)
            : base(obj)
        { }

        public static IEnumerable<Reference> Enumerate(Func<string, bool> nameSpaceFilter = null)
        {
            var iter = new MItDependencyNodes(MFn.Type.kReference);
            var tmpFn = new MFnDependencyNode();
            while (!iter.isDone)
            {
                if (nameSpaceFilter != null)
                {
                    tmpFn.setObject(iter.item);
                    if (tmpFn.name.IsFilterd(nameSpaceFilter))
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
