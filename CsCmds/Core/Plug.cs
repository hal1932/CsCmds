﻿using Autodesk.Maya.OpenMaya;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CsCmds.Core
{
    public class Plug
    {
        public MPlug MPlug { get; private set; }
        public DependNode ParentNode { get; private set; }

        public string Name => MPlug.name;
        public bool IsSource => MPlug.isSource();
        public bool IsDestination => MPlug.isDestination;
        public bool IsArray => MPlug.isArray;
        public bool IsCompound => MPlug.isCompound;
        public bool IsKeyable => MPlug.isKeyable;
        public bool IsLocked => MPlug.isLocked;
        public bool IsNull => MPlug.isNull;
        public int ChildCount => (int)MPlug.numChildren;

        internal Plug(MPlug plug, DependNode parent)
        {
            Debug.Assert(plug != null);
            MPlug = plug;
            ParentNode = parent;
        }

        #region get/set value
        public MDataHandle GetDataHandle()
        {
            return MPlug.asMDataHandle();
        }

        public T GetValue<T>()
        {
            var type = typeof(T);
            T value = default(T);

            if (type == typeof(bool)) CopyTypedValue(out value, MPlug.asBool());
            else if (type == typeof(char)) CopyTypedValue(out value, MPlug.asChar());
            else if (type == typeof(double)) CopyTypedValue(out value, MPlug.asDouble());
            else if (type == typeof(float)) CopyTypedValue(out value, MPlug.asFloat());
            else if (type == typeof(int)) CopyTypedValue(out value, MPlug.asInt());
            else if (type == typeof(short)) CopyTypedValue(out value, MPlug.asShort());
            else if (type == typeof(string)) CopyTypedValue(out value, MPlug.asString());
            else if (type == typeof(MAngle)) CopyTypedValue(out value, MPlug.asMAngle());
            else if (type == typeof(MDistance)) CopyTypedValue(out value, MPlug.asMDistance());
            else if (type == typeof(MObject)) CopyTypedValue(out value, MPlug.asMObject());
            else if (type == typeof(MTime)) CopyTypedValue(out value, MPlug.asMTime());
            else throw new NotSupportedException("not supported type: " + type.Name);

            return value;
        }

        public void SetValue<T>(T value, MDGModifier modifier)
        {
            var type = typeof(T);

            if (type == typeof(bool)) modifier.newPlugValueBool(MPlug, ChangeType<bool, T>(value));
            else if (type == typeof(char)) modifier.newPlugValueChar(MPlug, ChangeType<char, T>(value));
            else if (type == typeof(double)) modifier.newPlugValueDouble(MPlug, ChangeType<double, T>(value));
            else if (type == typeof(float)) modifier.newPlugValueFloat(MPlug, ChangeType<float, T>(value));
            else if (type == typeof(int)) modifier.newPlugValueInt(MPlug, ChangeType<int, T>(value));
            else if (type == typeof(short)) modifier.newPlugValueShort(MPlug, ChangeType<short, T>(value));
            else if (type == typeof(string)) modifier.newPlugValueString(MPlug, ChangeType<string, T>(value));
            else if (type == typeof(MAngle)) modifier.newPlugValueMAngle(MPlug, ChangeType<MAngle, T>(value));
            else if (type == typeof(MDistance)) modifier.newPlugValueMDistance(MPlug, ChangeType<MDistance, T>(value));
            else if (type == typeof(MObject)) modifier.newPlugValue(MPlug, ChangeType<MObject, T>(value));
            else if (type == typeof(MTime)) modifier.newPlugValueMTime(MPlug, ChangeType<MTime, T>(value));
            else throw new NotSupportedException("not supported type: " + type.Name);
        }

        private void CopyTypedValue<T, U>(out T outValue, U value)
        {
            outValue = ChangeType<T, U>(value);
        }

        private T ChangeType<T, U>(U value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        #endregion

        #region connections
        public void ConnectTo(Plug destination, MDGModifier modifier)
        {
            modifier.connect(MPlug, destination.MPlug);
        }

        public void DisconnectFrom(Plug destination, MDGModifier modifier)
        {
            modifier.disconnect(MPlug, destination.MPlug);
        }

        public IEnumerable<Plug> EnumerateDestinations()
        {
            return EnumerateConnectionsImpl(true, false);
        }

        public IEnumerable<Plug> EnumerateSources()
        {
            return EnumerateConnectionsImpl(false, true);
        }

        public IEnumerable<Plug> EnumerateConnections()
        {
            return EnumerateConnectionsImpl(true, true);
        }

        private IEnumerable<Plug> EnumerateConnectionsImpl(bool asDestination, bool asSource)
        {
            var connections = new MPlugArray();
            MPlug.connectedTo(connections, asDestination, asSource);
            return connections.Select(conn => new Plug(conn, ParentNode));
        }
        #endregion

        #region children
        public Plug GetChild(int index)
        {
            return new Plug(MPlug.child((uint)index), ParentNode);
        }

        public IEnumerable<Plug> EnumerateChildren()
        {
            for (var i = 0; i < ChildCount; ++i)
            {
                yield return GetChild(i);
            }
        }
        #endregion

        public void Delete(MDGModifier modifier)
        {
            modifier.removeAttribute(MPlug.node, MPlug.attribute);
            MPlug = null;
        }
    }
}
