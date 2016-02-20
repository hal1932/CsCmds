using Autodesk.Maya.OpenMaya;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CsCmds.Core
{
    public class Plug
    {
        public MPlug MPlug { get; private set; }
        public DepNode ParentNode { get; private set; }

        public string Name { get { return MPlug.name; } }

        public bool IsSource { get { return MPlug.isConnected; } }
        public bool IsDestination { get { return MPlug.isDestination; } }

        public bool IsKeyable
        {
            get { return MPlug.isKeyable; }
            set { MPlug.isKeyable = value; }
        }

        public bool IsLocked
        {
            get { return MPlug.isLocked; }
            set { MPlug.isLocked = value; }
        }

        internal Plug(MPlug plug, DepNode parent)
        {
            MPlug = plug;
            ParentNode = parent;
        }

        #region getAttr, setAttr
        public T GetAttribute<T>()
        {
            var type = typeof(T);
            T value = default(T);

            if (type == typeof(bool)) SetValue(out value, MPlug.asBool());
            else if (type == typeof(char)) SetValue(out value, MPlug.asChar());
            else if (type == typeof(double)) SetValue(out value, MPlug.asDouble());
            else if (type == typeof(float)) SetValue(out value, MPlug.asFloat());
            else if (type == typeof(int)) SetValue(out value, MPlug.asInt());
            else if (type == typeof(short)) SetValue(out value, MPlug.asShort());
            else if (type == typeof(string)) SetValue(out value, MPlug.asString());
            else if (type == typeof(MAngle)) SetValue(out value, MPlug.asMAngle());
            else if (type == typeof(MDataHandle)) SetValue(out value, MPlug.asMDataHandle());
            else if (type == typeof(MDistance)) SetValue(out value, MPlug.asMDistance());
            else if (type == typeof(MObject)) SetValue(out value, MPlug.asMObject());
            else if (type == typeof(MTime)) SetValue(out value, MPlug.asMTime());
            else throw new NotSupportedException("not supported type: " + type.Name);

            return value;
        }

        public void SetAttribute<T>(T value, MDGModifier modifier)
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

        private void SetValue<T, U>(out T outValue, U value)
        {
            //outValue = (T)Convert.ChangeType(value, typeof(T));
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

        public void Delete(MDGModifier modifier)
        {
            modifier.removeAttribute(MPlug.node, MPlug.attribute);
            MPlug = null;
        }
    }
}
