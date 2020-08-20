using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GameObjects;
using ObjectAccess;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Serialisation
{
    [Serializable]  
    public class ToSerial
    {
        public List<string> ls = new List<string>{"a","b","hello"};
        public SerialisableVector3 v = new SerialisableVector3(new Vector3(1,2,0));
    }
    
    public class testsave
    {
        public static void tryserialise()
        {
            string sceneName = Access.sceneInfo.SceneName;
            string path = Path.Combine("Saves", sceneName);
            Debug.Log(path);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fullpath = Path.Combine(path, "hello.test");
            FileStream fileStream = new FileStream(fullpath, FileMode.Create);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            ToSerial ts = new ToSerial();
            ts.ls[1] = "c";
            try
            
            {
                binaryFormatter.Serialize(fileStream,ts);
            }
            catch (Exception e)
            {
                Debug.Log("failed to serialise" + e.Message);
            }
            
            fileStream.Close();
            
            FileStream fs = new FileStream(fullpath,FileMode.Open);
            ToSerial read = (ToSerial) binaryFormatter.Deserialize(fs);
            fs.Close();
            Debug.Log("read block pos: " + read.ls[1]);
        }
    }

}