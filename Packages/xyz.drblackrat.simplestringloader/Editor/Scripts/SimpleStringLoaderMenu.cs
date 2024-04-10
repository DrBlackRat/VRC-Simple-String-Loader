using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace DrBlackRat
{
    public class SimpleStringLoaderMenu : MonoBehaviour
    {
        [MenuItem("Tools/Simple String Loader/ Add Example Prefab to Scene")]
        public static void AddPrefab()
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Packages/xyz.drblackrat.simplestringloader/Runtime/Simple String Loader Example.prefab");
            PrefabUtility.InstantiatePrefab(prefab);
        }
    }
}

