using Autodesk.Maya.OpenMaya;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CsCmds.Core
{
    public class DependNode
    {
        public MObject MObject { get; private set; }

        private MFnDependencyNode _fn;
        public MFnDependencyNode Fn
        {
            get { return _fn ?? (_fn = new MFnDependencyNode(MObject)); }
        }

        protected DependNode(MObject obj, MFnDependencyNode fn)
        {
            MObject = obj;
            _fn = fn;
        }

        #region enumerate
        public static IEnumerable<DependNode> Enumerate(string filter = null, params MFn.Type[] types)
        {
            var typeFilter = CreateTypeFilter(types);
            var iter = new MItDependencyNodes(typeFilter);
            while (!iter.isDone)
            {
                var fn = new MFnDependencyNode(iter.item);
                if (string.IsNullOrEmpty(filter) || fn.name.Contains(filter))
                {
                    yield return new DependNode(iter.item, fn);
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
                yield return new DependNode(obj, null);
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
            _fn = null;
        }

        #region plug, attr
        public Plug FindPlug(string name)
        {
            var plug = Fn.findPlug(name);
            return (plug != null) ? new Plug(plug, this) : null;
        }

        public IEnumerable<Plug> EnumeratePlugs(string filter = null)
        {
            var plugs = new MPlugArray();
            Fn.getConnections(plugs);

            return (!string.IsNullOrEmpty(filter)) ?
                plugs.Where(plug => plug.name.Contains(filter))
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

        #region conversion
        public Transform AsTransform()
        {
            return (Fn.type() == MFn.Type.kTransform) ?
                new Transform(MObject, Fn) : null;
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
