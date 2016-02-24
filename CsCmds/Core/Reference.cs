using Autodesk.Maya.OpenMaya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsCmds.Core
{
    public class Reference : DependNode
    {
        public string NameSpace { get { return FnDependNode.name; } }

        private Reference(MObject obj, MFnDependencyNode fn)
            : base(obj, fn)
        { }

        public static IEnumerable<Reference> Enumerate(Func<string, bool> nameSpaceFilter = null)
        {
            var iter = new MItDependencyNodes(MFn.Type.kReference);
            while (!iter.isDone)
            {
                if (nameSpaceFilter != null)
                {
                    var fn = new MFnDependencyNode(iter.item);
                    if (fn.name.IsFilterd(nameSpaceFilter))
                    {
                        continue;
                    }
                    yield return new Reference(iter.item, fn);
                }
                else
                {
                    yield return new Reference(iter.item, null);
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
