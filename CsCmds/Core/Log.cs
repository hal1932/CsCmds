using Autodesk.Maya.OpenMaya;
using System;
using System.Diagnostics;

namespace CsCmds.Core
{
    public static class Log
    {
        [Conditional("DEBUG")]
        public static void DebugLine(object arg)
        {
            DebugLine(arg.ToString());
        }
        [Conditional("DEBUG")]
        public static void DebugLine(string format, params object[] args)
        {
            WriteLine(format, args);
        }

        public static void WriteLine(object arg)
        {
            WriteLine(arg.ToString());
        }
        public static void WriteLine(string format, params object[] args)
        {
            MGlobal.displayInfo(string.Format(format, args));
        }

        public static void WarningLine(object arg)
        {
            Warning(arg.ToString());
        }
        public static void Warning(string format, params object[] args)
        {
            MGlobal.displayWarning(string.Format(format, args));
        }

        public static void ErrorLine(Exception e)
        {
            var tmp = e;
            while (tmp != null)
            {
                ErrorLine(tmp.ToString());
                tmp = tmp.InnerException;
            }
        }
        public static void ErrorLine(object arg)
        {
            ErrorLine(arg.ToString());
        }
        public static void ErrorLine(string format, params object[] args)
        {
            MGlobal.displayError(string.Format(format, args));
        }
    }
}
