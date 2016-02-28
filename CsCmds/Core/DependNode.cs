using Autodesk.Maya.OpenMaya;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CsCmds.Core
{
    public class DependNode
    {
        public MObject MObject { get; private set; }

        private MFnDependencyNode _fnDependNode;
        public virtual MFnDependencyNode FnDependNode
        {
            get
            {
                return _fnDependNode ?? (_fnDependNode = new MFnDependencyNode(MObject));
            }
        }

        public MFn.Type ApiType => MObject.apiType;
        public bool IsNull => MObject.isNull;
        public string Name => FnDependNode.name;
        public bool IsLocked => FnDependNode.isLocked;
        public bool IsShared => FnDependNode.isShared;
        public bool IsFromReferenceFile => FnDependNode.isFromReferencedFile;
        public bool CanBeWritten => FnDependNode.canBeWritten;
        public int AttributeCount => (int)FnDependNode.attributeCount;

        internal DependNode(MObject obj)
        {
            Debug.Assert(obj != null);
            MObject = obj;
        }

        #region enumerate
        public static DependNode FirstOrDefault(Func<MFnDependencyNode, bool> filter = null)
        {
            return Enumerate(filter).FirstOrDefault();
        }

        public static IEnumerable<DependNode> Enumerate(Func<MFnDependencyNode, bool> filter = null, params MFn.Type[] types)
        {
            return EnumerateRaw(filter, types)
                .Select(obj => new DependNode(obj));
        }

        protected static IEnumerable<MObject> EnumerateRaw(Func<MFnDependencyNode, bool> filter = null, params MFn.Type[] types)
        {
            var typeFilter = CreateTypeFilter(types);
            var iter = new MItDependencyNodes(typeFilter);
            var tmpFn = new MFnDependencyNode();
            while (!iter.isDone)
            {
                if (filter != null)
                {
                    tmpFn.setObject(iter.item);
                    if (tmpFn.IsFilterd(filter))
                    {
                        yield return iter.item;
                    }
                }
                else
                {
                    yield return iter.item;
                }
                iter.next();
            }
        }

        public static DependNode FirstSelectedOrDefault()
        {
            return EnumerateSelected().FirstOrDefault();
        }

        public static IEnumerable<DependNode> EnumerateSelected()
        {
            var list = new MSelectionList();
            MGlobal.getActiveSelectionList(list, true);
            foreach (var item in list)
            {
                var obj = new MObject();
                item.getDependNode(obj);
                yield return new DependNode(obj);
            }
        }
        #endregion

        public bool HasFn(MFn.Type type)
        {
            return MObject.hasFn(type);
        }

        public void Delete(MDGModifier modifier)
        {
            modifier.deleteNode(MObject);
            MObject = null;
            _fnDependNode = null;
        }

        #region enumerate plugs
        public Plug FindPlug(string name)
        {
            var plug = FnDependNode.findPlug(name);
            return (plug != null) ? new Plug(plug, this) : null;
        }

        public Plug FirstPlugOrDefault(Func<MPlug, bool> filter = null)
        {
            return EnumeratePlugs(filter).FirstOrDefault();
        }

        public Plug FirstConnectedPlugOrDefault(Func<MPlug, bool> filter = null)
        {
            return EnumerateConnectedPlugs(filter).FirstOrDefault();
        }

        public IEnumerable<Plug> EnumeratePlugs(Func<MPlug, bool> filter = null)
        {
            for (uint i = 0; i < FnDependNode.attributeCount; ++i)
            {
                var attrObj = FnDependNode.attribute(i);
                var plug = new MPlug(MObject, attrObj);

                if (plug.IsFilterd(filter))
                {
                    yield return new Plug(plug, this);
                }
            }
        }

        public IEnumerable<Plug> EnumerateConnectedPlugs(Func<MPlug, bool> filter = null)
        {
            var plugs = new MPlugArray();
            FnDependNode.getConnections(plugs);

            return plugs.Where(plug => plug.IsFilterd(filter))
                .Select(plug => new Plug(plug, this));
        }
        #endregion

        #region enumerate connected nodes
        public DependNode FirstConnectedNodeOrDefault(Func<MObject, bool> filter = null)
        {
            return EnumerateConnectedNodes(filter).FirstOrDefault();
        }

        public DependNode FirstSourceNodeOrDefault(Func<MObject, bool> filter = null)
        {
            return EnumerateSourceNodes(filter).FirstOrDefault();
        }

        public DependNode FirstDestinationNodeOrDefault(Func<MObject, bool> filter = null)
        {
            return EnumerateDestinationNodes(filter).FirstOrDefault();
        }

        public IEnumerable<DependNode> EnumerateConnectedNodes(Func<MObject, bool> filter = null)
        {
            return EnumerateNodesImpl(true, true, filter, (plug) => true);
        }

        public IEnumerable<DependNode> EnumerateSourceNodes(Func<MObject, bool> filter = null)
        {
            return EnumerateNodesImpl(true, false, filter, (plug) => plug.isDestination);
        }

        public IEnumerable<DependNode> EnumerateDestinationNodes(Func<MObject, bool> filter = null)
        {
            return EnumerateNodesImpl(false, true, filter, (plug) => plug.isSource());
        }

        private IEnumerable<DependNode> EnumerateNodesImpl(bool asDestination, bool asSource, Func<MObject, bool> filter, Func<MPlug, bool> mplugFilter)
        {
            var plugs = new MPlugArray();
            FnDependNode.getConnections(plugs);

            var nodesiter = new List<IEnumerable<MObject>>();
            foreach (var plug in plugs.Where(mplugFilter))
            {
                var connectedPlugs = new MPlugArray();
                plug.connectedTo(connectedPlugs, asDestination, asSource);

                nodesiter.Add(
                    connectedPlugs.Select(p => p.node)
                        .Distinct()
                        .Where(n => n.IsFilterd(filter)));
            }

            return nodesiter.SelectMany(iter => iter)
                .Distinct()
                .Select(node => new DependNode(node));
        }
        #endregion

        #region attributes
        public MObject AddAttribute(string longName, string shortName, MFnData.Type type, MDGModifier modifier)
        {
            var attrFn = new MFnTypedAttribute();
            var attrObj = attrFn.create(longName, shortName, type);
            modifier.addAttribute(MObject, attrObj);
            return attrObj;
        }
        #endregion

        private static MIteratorType CreateTypeFilter(MFn.Type[] types)
        {
            var iterType = new MIteratorType();
            iterType.setObjectType(MIteratorType.objFilterType.kMObject);

            if (types.Length > 0)
            {
                var typeFilters = new MIntArray();
                Array.ForEach(types, t => typeFilters.Add((int)t));
                iterType.setFilterList(typeFilters);
            }
            return iterType;
        }
    }
}
