using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ClearAllPlayerPrefs{

    [MenuItem("Tools/Clear PlayerPrefs")]
    private static void NewMenuOption()
    {
        PlayerPrefs.DeleteAll();
    }
}
