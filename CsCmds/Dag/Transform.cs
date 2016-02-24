using Autodesk.Maya.OpenMaya;
using CsCmds.Core;
using System.Collections.Generic;

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
