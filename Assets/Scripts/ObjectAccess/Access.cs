using UnityEngine;

namespace ObjectAccess
{
    public class Access : MonoBehaviour
    {
        private static GameObject accessObject;
        private static GameObject access()
        {
            if (accessObject == null) accessObject = GameObject.Find("ObjectAccess");
            return (accessObject);
        }

        public static UIElements uiElements => access().GetComponent<UIElements>();

        public static Managers managers => access().GetComponent<Managers>();

        public static Prefabs prefabs => access().GetComponent <Prefabs>();

        public static SceneInfo sceneInfo => access().GetComponent<SceneInfo>();

    }
}