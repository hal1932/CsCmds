using Autodesk.Maya.OpenMaya;
using CsCmds.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsCmds.Dag
{
    public class DagNode : DependNode
    {
        private MDagPath _dagPath;
        public MDagPath DagPath
        {
            get { return _dagPath ?? (_dagPath = MDagPath.getAPathTo(MObject)); }
        }   

        public MFnDagNode FnDagNode { get; private set; }

        internal DagNode(MObject obj, MFnDagNode fn)
            : base(obj, fn ?? new MFnDagNode(obj))
        {
            FnDagNode = fn;
        }

        public static DagNode DownCastFrom(DependNode node)
        {
            return (node.MObject.hasFn(MFn.Type.kDagNode)) ?
                new DagNode(node.MObject, null) : null;
        }
    }
}
