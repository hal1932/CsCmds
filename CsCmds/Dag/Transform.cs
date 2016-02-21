using Autodesk.Maya.OpenMaya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsCmds.Dag
{
    public class Transform : DagNode
    {
        internal Transform(MObject obj, MFnDagNode fn)
            : base(obj, fn)
        { }

        #region shape
        public Shape GetShape()
        {
            return (DagPath.childCount > 0) ?
                new Shape(DagPath.child(0), null)
                : null;
        }

        public IEnumerable<Shape> EnumerateShapes()
        {
            for (uint i = 0; i < DagPath.childCount; ++i)
            {
                var childObj = DagPath.child(i);
                yield return new Shape(childObj, null);
            }
        }
        #endregion
    }
}
