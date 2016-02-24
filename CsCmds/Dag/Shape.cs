using Autodesk.Maya.OpenMaya;
using CsCmds.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsCmds.Dag
{
    public class Shape : DagNode
    {
        internal Shape(MObject obj, MFnDagNode fn, Transform transform)
            : base(obj, fn)
        {
            _transform = transform;
        }

        public static new Shape DownCastFrom(DependNode node)
        {
            return (node.MObject.hasFn(MFn.Type.kShape)) ?
                new Shape(node.MObject, null, null) : null;
        }

        public Transform GetTransform()
        {
            if (_transform == null)
            {
                for (uint i = 0; i < FnDagNode.parentCount; ++i)
                {
                    var parentObj = FnDagNode.parent(i);
                    if (parentObj.apiType == MFn.Type.kTransform)
                    {
                        _transform = new Transform(parentObj, null);
                        break;
                    }
                }
            }
            return _transform;
        }

        private Transform _transform;
    }
}
