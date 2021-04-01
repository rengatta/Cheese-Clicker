using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Extensions;

public static class GlobalHelper
{

    static GlobalHelper()
    {

        GameObject globalInstance = GameObject.Find("Global");
        if(globalInstance != null)
        _global = globalInstance.GetComponent<Global>();

    }




    public static TextMeshProUGUI test;
    private static Global _global;
    public static Global global
    {
        get
        {

            if (_global == null)
            {
                GameObject globalInstance = GameObject.Find("Global");
                if (globalInstance != null)
                {
                    _global = GameObject.Find("Global").GetComponent<Global>();
                } 
            }

            return _global;
        }
    }

  
}

public static class SceneToSceneData {
    public static bool isLoading = false;
    public static string loadPath = "";

}


public class Global : MonoBehaviour
{
    public string email = "";
    public AuthManager authManager;
    public Firebase.Auth.FirebaseAuth auth;

}
