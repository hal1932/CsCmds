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

        public static IEnumerable<Transform> Enumerate(Func<string, bool> filter = null, params MFn.Type[] types)
        {
            return EnumerateRaw(filter, types)
                .Select(obj => new Transform(obj));
        }

        #region shape
        public Shape GetShape()
        {
            return (DagPath.childCount > 0) ?
                new Shape(DagPath.child(0), this)
                : null;
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
