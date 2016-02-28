using Autodesk.Maya.OpenMaya;
using CsCmds.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CsCmds.Dag
{
    public class Shape : DagNode
    {
        internal Shape(MObject obj)
            : this(obj, null)
        { }

        internal Shape(MObject obj, Transform transform)
            : base(obj)
        {
            _transform = transform;
        }

        public static new Shape CreateFrom(DependNode node)
        {
            return (node.HasFn(MFn.Type.kShape)) ?
                new Shape(node.MObject, null) : null;
        }

        #region enumerate
        public static new Shape FirstOrDefault(Func<MFnDependencyNode, bool> filter = null)
        {
            return Enumerate(filter).FirstOrDefault();
        }

        public static new IEnumerable<Shape> Enumerate(Func<MFnDependencyNode, bool> filter = null, params MFn.Type[] types)
        {
            return EnumerateRaw(filter, MFn.Type.kShape)
                .Select(obj => new Shape(obj, null));
        }
        #endregion

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

        public ShadingEngine FirstShadingEngineOrDefault(Func<MObject, bool> filter = null)
        {
            return EnumerateShadingEngines(filter).FirstOrDefault();
        }

        public IEnumerable<ShadingEngine> EnumerateShadingEngines(Func<MObject, bool> filter = null)
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
                    .FirstOrDefault(dstObj => dstObj.apiType == MFn.Type.kShadingEngine && dstObj.IsFilterd(filter));
                if (sgObj != null)
                {
                    result = new ShadingEngine(sgObj);
                    break;
                }
            }

            if (result != null)
            {
                yield return result;
            }

            // シェイプがメッシュの場合、フェース単位でSGがアサインされている可能性がある
            // メッシュ以外のシェイプの例↓
            // https://knowledge.autodesk.com/ja/search-result/caas/CloudHelp/cloudhelp/2016/JPN/Maya-SDK/files/Shapes-Shapes-in-Maya-htm.html
            if (MObject.hasFn(MFn.Type.kMesh))
            {
                if (result == null)
                {
                    var meshFn = new MFnMesh(MObject);

                    var shaderObjs = new MObjectArray();
                    var indices = new MIntArray();
                    meshFn.getConnectedShaders(
                        meshFn.dagPath.instanceNumber, shaderObjs, indices);

                    Log.WriteLine("--------");
                    foreach (var shaderObj in shaderObjs)
                    {
                        Log.WriteLine(shaderObj.apiTypeStr);
                    }
                    Log.WriteLine("--------");

                    foreach (var shaderObj in shaderObjs.Where(obj => obj.IsFilterd(filter)))
                    {
                        yield return new ShadingEngine(shaderObj);
                    }
                }
            }
        }

        private Transform _transform;
    }
}
