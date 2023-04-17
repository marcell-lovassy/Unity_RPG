using UnityEditor;
using UnityEngine;

public class MenuHelpers : MonoBehaviour
{
    // Add a menu item named "Do Something" to MyMenu in the menu bar.
    [MenuItem("Helpers/Open save folder")]
    static void OpenSavesFolder()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }
}