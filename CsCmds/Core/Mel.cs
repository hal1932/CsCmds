using Autodesk.Maya.OpenMaya;

namespace CsCmds.Core
{
    public static class Mel
    {
        public static MCommandResult Evaluate(string command, bool displayEnabled = true, bool undoEnabled = true)
        {
            var result = new MCommandResult();
            MGlobal.executeCommand(command, result, displayEnabled, undoEnabled);
            return result;
        }

        public static void Evaluate(string command, MDGModifier modifier)
        {
            modifier.commandToExecute(command);
        }

        public static void EvaluateFile(string filepath)
        {
            MGlobal.sourceFile(filepath.AsNormalizedPath());
        }

        public static void EvaluateOnIdle(string command, bool displayEnabled = true)
        {
            MGlobal.executeCommandOnIdle(command, displayEnabled);
        }
    }
}
