//code taken from this tutorial https://spin.atomicobject.com/2020/06/09/firebase-unity-user-accounts/
using UnityEngine;
using Firebase.Extensions;

public class AuthSetup : MonoBehaviour
{
    
    private Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

    // Start is called before the first frame update
    // When the app starts, check to make sure that we have
    // the required dependencies to use Firebase, and if not,
    // add them if possible.
    void Start()
    {
        Debug.Log("Loading scene started.");
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            Debug.Log("Checking for dependencies.");
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Debug.Log("Initializing Firebase.");
                GlobalHelper.global.authManager.InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });



    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log("QUITTING");
            Application.Quit();

        }
    }





}