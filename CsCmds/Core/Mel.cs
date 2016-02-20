using Autodesk.Maya.OpenMaya;

namespace CsCmds.Core
{
    public static class Mel
    {
        public static string Evaluate(string command, bool displayEnabled = true, bool undoEnabled = true)
        {
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
