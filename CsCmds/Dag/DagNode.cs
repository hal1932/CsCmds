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

        public override MFnDependencyNode FnDependNode
        {
            get { return FnDagNode; }
        }

        public MBoundingBox BoundingBox => FnDagNode.boundingBox;
        public int ChildCount => (int)FnDagNode.childCount;
        public int ParentCount => (int)FnDagNode.parentCount;
        public MDagPath DagPath => FnDagNode.dagPath;
        public string FullName => FnDagNode.fullPathName;

        internal DagNode(MObject obj)
            : base(obj)
        { }

        public static DagNode CreateFrom(DependNode node)
        {
            return (node.HasFn(MFn.Type.kDagNode)) ?
                new DagNode(node.MObject) : null;
        }

        #region enumerate
        public static new DagNode FirstOrDefault(Func<MFnDependencyNode, bool> filter = null)
        {
            return Enumerate(filter).FirstOrDefault();
        }

        public static IEnumerable<DagNode> Enumerate(Func<MFnDependencyNode, bool> filter = null)
        {
            return EnumerateRaw(filter, MFn.Type.kDagNode)
                .Select(obj => new DagNode(obj));
        }
        #endregion

        #region children
        public DagNode FirstChildOrDefault()
        {
            if (FnDagNode.childCount > 0)
            {
                return new DagNode(FnDagNode.child(0));
            }
            return null;
        }

        public DagNode FirstChildOrDefault(Func<MObject, bool> filter = null)
        {
            return EnumerateChildren(filter).FirstOrDefault();
        }

        public IEnumerable<DagNode> EnumerateChildren(Func<MObject, bool> filter = null)
        {
            for (uint i = 0; i < FnDagNode.childCount; ++i)
            {
                var child = FnDagNode.child(i);
                if (child.IsFilterd(filter))
                {
                    yield return new DagNode(child);
                }
            }
        }
        #endregion
    }
}
