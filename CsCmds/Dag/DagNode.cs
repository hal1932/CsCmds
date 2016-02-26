using Autodesk.Maya.OpenMaya;
using CsCmds.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CsCmds.Dag
{
    public class DagNode : DependNode
    {
        private MFnDagNode _fnDagNode;
        public MFnDagNode FnDagNode
        {
            get { return _fnDagNode ?? (_fnDagNode = new MFnDagNode(MObject)); }
        }

        public MBoundingBox BoundingBox => FnDagNode.boundingBox;
        public int ChildCount => (int)FnDagNode.childCount;
        public int ParentCount => (int)FnDagNode.parentCount;
        public MDagPath DagPath => FnDagNode.dagPath;
        public string FullName => FnDagNode.fullPathName;

        internal DagNode(MObject obj)
            : base(obj)
        { }

        public static DagNode DownCastFrom(DependNode node)
        {
            return (node.HasFn(MFn.Type.kDagNode)) ?
                new DagNode(node.MObject) : null;
        }

        public static IEnumerable<DagNode> Enumerate(Func<string, bool> filter = null)
        {
            return EnumerateRaw(filter, MFn.Type.kDagNode)
                .Select(obj => new DagNode(obj));
        }

        #region children
        public DagNode GetChild(int index)
        {
            return new DagNode(FnDagNode.child((uint)index));
        }

        public IEnumerable<DagNode> EnumerateChilds()
        {
            for (var i = 0; i < ChildCount; ++i)
            {
                yield return GetChild(i);
            }
        }
        #endregion
    }
}
