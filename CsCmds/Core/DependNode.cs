using Autodesk.Maya.OpenMaya;
using CsCmds.Dag;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CsCmds.Core
{
    public class DependNode
    {
        public MObject MObject { get; private set; }

        private MFnDependencyNode _fnDependNode;
        public MFnDependencyNode FnDependNode
        {
            get { return _fnDependNode ?? (_fnDependNode = new MFnDependencyNode(MObject)); }
        }

        protected DependNode(MObject obj)
        {
            MObject = obj;
        }

        #region enumerate
        public static IEnumerable<DependNode> Enumerate(Func<string, bool> filter = null, params MFn.Type[] types)
        {
            var typeFilter = CreateTypeFilter(types);
            var iter = new MItDependencyNodes(typeFilter);
            var tmpFn = new MFnDependencyNode();
            while (!iter.isDone)
            {
                tmpFn.setObject(iter.item);
                if (!tmpFn.name.IsFilterd(filter))
                {
                    yield return new DependNode(iter.item);
                }
                iter.next();
            }
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

        public MSelectionList Select(bool replace = false)
        {
            var list = new MSelectionList();
            if (!replace)
            {
                MGlobal.getActiveSelectionList(list, true);
            }
            list.add(MObject);
            return list;
        }

        public void Delete(MDGModifier modifier)
        {
            modifier.deleteNode(MObject);
            MObject = null;
            _fnDependNode = null;
        }

        #region plug, attr
        public Plug FindPlug(string name)
        {
            var plug = FnDependNode.findPlug(name);
            return (plug != null) ? new Plug(plug, this) : null;
        }

        public IEnumerable<Plug> EnumeratePlugs(Func<string, bool> filter = null)
        {
            for (uint i = 0; i < FnDependNode.attributeCount; ++i)
            {
                var attrObj = FnDependNode.attribute(i);
                var plug = new MPlug(MObject, attrObj);

                if (plug.name.IsFilterd(filter))
                {
                    continue;
                }
                yield return new Plug(plug, this);
            }
        }

        public IEnumerable<Plug> EnumerateConnectedPlugs(Func<string, bool> filter = null)
        {
            var plugs = new MPlugArray();
            FnDependNode.getConnections(plugs);

            return (filter != null) ?
                plugs.Where(plug => !plug.name.IsFilterd(filter))
                    .Select(plug => new Plug(plug, this))
                : plugs.Select(plug => new Plug(plug, this));
        }

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
