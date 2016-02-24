using Autodesk.Maya.OpenMaya;
using CsCmds.Core;
using System.Collections.Generic;
using System.Linq;

namespace CsCmds.Dag
{
    public class Shape : DagNode
    {
        internal Shape(MObject obj, Transform transform)
            : base(obj)
        {
            _transform = transform;
        }

        public static new Shape DownCastFrom(DependNode node)
        {
            return (node.HasFn(MFn.Type.kShape)) ?
                new Shape(node.MObject, null) : null;
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
                        _transform = new Transform(parentObj);
                        break;
                    }
                }
            }
            return _transform;
        }

        public IEnumerable<ShadingEngine> EnumerableShadingEngines()
        {
            ShadingEngine result = null;

            // シェイプ単位で探す
            var iogPlug = FnDagNode.findPlug("instObjGroups");
            for (uint i = 0; i < iogPlug.numChildren; ++i)
            {
                var elemPlug = iogPlug.elementByLogicalIndex(i);

                var dstPlugs = new MPlugArray();
                elemPlug.connectedTo(dstPlugs, false, true);

                var sgObj = dstPlugs.Select(dstPlug => dstPlug.node)
                    .FirstOrDefault(dstObj => dstObj.apiType == MFn.Type.kShadingEngine);
                if (sgObj != null)
                {
                    result = new ShadingEngine(sgObj);
                    break;
                }
            }

            yield return result;

            // シェイプ単位で見つからなかったら、フェース単位でも探す
            if (result == null)
            {
                var meshFn = new MFnMesh(MObject);

                var shaderObjs = new MObjectArray();
                var indices = new MIntArray();
                meshFn.getConnectedShaders(
                    meshFn.dagPath.instanceNumber, shaderObjs, indices);

                foreach (var shaderObj in shaderObjs)
                {
                    yield return new ShadingEngine(shaderObj);
                }
            }
        }

        private Transform _transform;
    }
}
