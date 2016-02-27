using Autodesk.Maya.OpenMaya;
using CsCmds.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CsCmds.Dag
{
    public class Transform : DagNode
    {
        internal Transform(MObject obj)
            : base(obj)
        { }

        public static new Transform DownCastFrom(DependNode node)
        {
            return (node.HasFn(MFn.Type.kTransform)) ?
                new Transform(node.MObject) : null;
        }

        #region enumerate
        public static new Transform FirstOrDefault(Func<MFnDependencyNode, bool> filter = null)
        {
            return Enumerate(filter).FirstOrDefault();
        }

        public static new IEnumerable<Transform> Enumerate(Func<MFnDependencyNode, bool> filter = null, params MFn.Type[] types)
        {
            return EnumerateRaw(filter, types)
                .Select(obj => new Transform(obj));
        }
        #endregion

        #region shape
        public Shape FirstShapeOrDefault()
        {
            for (uint i = 0; i < DagPath.childCount; ++i)
            {
                var child = DagPath.child(i);
                if (child.hasFn(MFn.Type.kShape))
                {
                    return new Shape(child, this);
                }
            }
            return default(Shape);
        }

        public IEnumerable<Shape> EnumerateShapes()
        {
            for (uint i = 0; i < DagPath.childCount; ++i)
            {
                var childObj = DagPath.child(i);
                yield return new Shape(childObj, this);
            }
        }
        #endregion
    }
}
