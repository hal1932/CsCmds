using Autodesk.Maya.OpenMaya;

namespace CsCmds.Core
{
    public class File
    {
        #region new, open, save, import
        public static void NewScene(bool force = false)
        {
            MFileIO.newFile(force);
        }

        public static void OpenScene(
            string filepath,
            string type = null,
            bool force = false,
            MFileIO.ReferenceMode refMode = MFileIO.ReferenceMode.kLoadDefault,
            bool ignoreVersion = false)
        {
            MFileIO.open(filepath.AsNormalizedPath(), type, force, refMode, ignoreVersion);
        }

        public static void SaveScene(
            bool force = false,
            string filepath = null,
            string type = null)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                MFileIO.save(force);
            }
            else
            {
                MFileIO.saveAs(filepath.AsNormalizedPath(), type, force);
            }
        }

        public static void ImportScene(
            string filepath,
            string type = null,
            bool preserveReference = false,
            string nameSpace = null,
            bool ignoreVersion = false)
        {
            MFileIO.importFile(filepath.AsNormalizedPath(), type, preserveReference, nameSpace, ignoreVersion);
        }
        #endregion
    }
}
