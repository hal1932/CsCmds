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

        public static new Transform CreateFrom(DependNode node)
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
            return EnumerateShapes().FirstOrDefault();
        }

        public IEnumerable<Shape> EnumerateShapes()
        {
            for (uint i = 0; i < FnDagNode.childCount; ++i)
            {
                var childObj = FnDagNode.child(i);
                if (childObj.hasFn(MFn.Type.kShape))
                {
                    yield return new Shape(childObj);
                }
            }
        }
        #endregion
    }
}
