using Autodesk.Maya.OpenMaya;

namespace CsCmds.Core
{
    public static class Mel
    {
        public static string Evaluate(string command, bool displayEnabled = true, bool undoEnabled = true)
        {
            // HACK: thw workaround for the problem that Maya2014 throws a System.ApplicationException when you use the executeCommand(string, bool, bool)
            if (MGlobal.mayaVersion.StartsWith("2014"))
            {
                return MGlobal.executeCommandStringResult(command, displayEnabled);
            }
            return MGlobal.executeCommandStringResult(command, displayEnabled, undoEnabled);
        }

        public static void Evaluate(string command, MDGModifier modifier)
        {
            modifier.commandToExecute(command);
        }

        public static void EvaluateOnIdle(string command, bool displayEnabled = true)
        {
            MGlobal.executeCommandOnIdle(command, displayEnabled);
        }
    }
}
