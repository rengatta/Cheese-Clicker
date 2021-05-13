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
    public static SaveData tempSaveData;
    public static bool isLoading = false;
    public static string loadPath = "";
    public static string username = "";
    public static bool resetSave = false;
    public static bool canCloudSave = false;
    public static string genericString = "";
    public static bool accountCreationSuccessful = false;
    public static bool authLoaded = false;

    public static string newSignupUsername = "";

    public static bool canSignout = true;

}


public class Global : MonoBehaviour
{
    public string email = "";
    public string userID = "";
    public AuthManager authManager;
    public TextMeshProUGUI tempText;
    public Firebase.Auth.FirebaseAuth auth;
    public Firebase.Auth.FirebaseUser currentUser;
}
