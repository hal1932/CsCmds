using Autodesk.Maya.OpenMaya;

namespace CsCmds.Core
{
    public static class Python
    {
        // TODO: 返り値をジェネリクスで綺麗に扱う方法を考える、outと参照が混ざっててめんどくさい
        public static string Evaluate(string command, bool displayEnabled = true, bool undoEnabled = true)
        {
            return MGlobal.executePythonCommandStringResult(command, displayEnabled, undoEnabled);
        }

        public static void EvaluateOnIdle(string command, bool displayEnabled = true)
        {
            MGlobal.executePythonCommandOnIdle(command, displayEnabled);
        }
    }
}
