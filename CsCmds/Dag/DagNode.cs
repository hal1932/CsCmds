using Autodesk.Maya.OpenMaya;
using CsCmds.Core;

namespace CsCmds.Dag
{
    public class DagNode : DependNode
    {
        private MDagPath _dagPath;
        public MDagPath DagPath
        {
            get { return _dagPath ?? (_dagPath = MDagPath.getAPathTo(MObject)); }
        }

        private MFnDagNode _fnDagNode;
        public MFnDagNode FnDagNode
        {
            get { return _fnDagNode ?? (_fnDagNode = new MFnDagNode(MObject)); }
        }

        internal DagNode(MObject obj)
            : base(obj)
        { }

        public static DagNode DownCastFrom(DependNode node)
        {
            return (node.MObject.hasFn(MFn.Type.kDagNode)) ?
                new DagNode(node.MObject) : null;
        }
    }
}
