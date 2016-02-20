using Autodesk.Maya.OpenMaya;
using System;
using System.Diagnostics;

namespace CsCmds.Core
{
    public static class Log
    {
        [Conditional("DEBUG")]
        public static void Debug(object arg)
        {
            Debug(arg.ToString());
        }
        [Conditional("DEBUG")]
        public static void Debug(string format, params object[] args)
        {
            Write(format, args);
        }

        public static void Write(object arg)
        {
            Write(arg.ToString());
        }
        public static void Write(string format, params object[] args)
        {
            MGlobal.displayInfo(string.Format(format, args));
        }

        public static void Warning(object arg)
        {
            Warning(arg.ToString());
        }
        public static void Warning(string format, params object[] args)
        {
            MGlobal.displayWarning(string.Format(format, args));
        }

        public static void Error(Exception e)
        {
            var tmp = e;
            while (tmp != null)
            {
                Error(tmp.ToString());
                tmp = tmp.InnerException;
            }
        }
        public static void Error(object arg)
        {
            Error(arg.ToString());
        }
        public static void Error(string format, params object[] args)
        {
            MGlobal.displayError(string.Format(format, args));
        }
    }
}
