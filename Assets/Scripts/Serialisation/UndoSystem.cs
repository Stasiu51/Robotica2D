using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using GameObjects;
using ObjectAccess;
using Debug = UnityEngine.Debug;

namespace Serialisation
{
    public static class UndoSystem
    {
        private static string tempPath;
        private const string UNDOFOLDER = "Undo";
        private static string undopath;
        private static int undoIndex = -1;
        static BinaryFormatter bf = new BinaryFormatter();
        private static int greatestChangedIndex = undoIndex;

        static UndoSystem()
        {
            tempPath = Path.GetTempPath();
            undopath = Path.Combine(tempPath, UNDOFOLDER);
        }

        private static void saveState(Blocks.SavedBlocks save)
        {
            if (!Directory.Exists(undopath))
            {
                Directory.CreateDirectory(undopath);
            }
            
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms,save);
                byte[] thisSave = ms.ToArray();
                if (undoIndex >= 0)
                {
                    var oldsave = getSave(undoIndex);
                    var ms2 = new MemoryStream();
                    bf.Serialize(ms2,oldsave);
                    if (ms2.ToArray().SequenceEqual(thisSave))
                    {
                        Debug.Log("nothing new");
                        return;
                    }
                }

                undoIndex++;
                greatestChangedIndex = undoIndex;
                string fileName = Path.Combine(undopath, undoIndex.ToString());
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    ms.WriteTo(fs);
                    Debug.Log("saved");
                }
            }
        }

        private static Blocks.SavedBlocks getSave(int index)
        {
            if (index > greatestChangedIndex) throw new Exception("cannot redo further");
            if (index < 0) throw new Exception("nothing to undo");
            string fileName = Path.Combine(undopath, undoIndex.ToString());
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                return (Blocks.SavedBlocks) bf.Deserialize(fs);
            }
        }

        public static void saveUndo()
        {
            saveState(new Blocks.SavedBlocks(Access.managers.Blocks));
        }

        public static void undo()
        {
            if (undoIndex <= 0)
            {
                Debug.Log("nothing to undo");
                return;
            }
            undoIndex--;
            Access.managers.Blocks.loadFromSave(getSave(undoIndex));
        }

        public static void redo()
        {
            if (undoIndex >= greatestChangedIndex)
            {
                Debug.Log("cannot redo");
                return;
            }
            undoIndex++;
            Access.managers.Blocks.loadFromSave(getSave(undoIndex));
        }

        
        
        
    }
}