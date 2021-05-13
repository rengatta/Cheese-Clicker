//code taken from this tutorial https://spin.atomicobject.com/2020/06/09/firebase-unity-user-accounts/
using System;
using System.Collections.Generic;
using System.Collections;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    public SceneField signInScene;
    public SceneField loadingScene;
    public SceneField mainScene;

    // Firebase User keyed by Firebase Auth.
    protected Dictionary<string, Firebase.Auth.FirebaseUser> userByAuth =
    new Dictionary<string, Firebase.Auth.FirebaseUser>();


    public bool signingOut = false;

    public void DeleteUserAccount() {
        Debug.Log("Deleting user account");
        Firebase.Auth.FirebaseUser user = GlobalHelper.global.auth.CurrentUser;
  
        if (user != null)
        {
       
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("users/" + GlobalHelper.global.auth.CurrentUser.UserId + "/saveData");
            reference.SetRawJsonValueAsync(null).ContinueWithOnMainThread(task => {
                user.DeleteAsync().ContinueWithOnMainThread(task2 => {
                    if (task2.IsCanceled)
                    {
                        Debug.LogError("DeleteAsync was canceled.");
                        return;
                    }
                    if (task2.IsFaulted)
                    {
                        Debug.LogError("DeleteAsync encountered an error: " + task2.Exception);
                        return;
                    }

                    Debug.Log("User deleted successfully.");

                    SceneManager.LoadScene(signInScene);

                });

            });

         
        }
    }



    public void InitializeFirebase() {
        Debug.Log("InitializeFirebase");
        GlobalHelper.global.auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        GlobalHelper.global.auth.StateChanged -= AuthStateChanged;
        GlobalHelper.global.auth.StateChanged += AuthStateChanged;

        Debug.Log("CURRENT USER: " + GlobalHelper.global.auth.CurrentUser.Email);

        StartCoroutine(WaitToSignOut());
        AuthStateChanged(this, null);



    }

    public void SignOut() {
        StartCoroutine(WaitToSignOut());

    }

    System.Collections.IEnumerator WaitToSignOut() {

        if(!SceneToSceneData.canSignout)
            Debug.Log("Waiting to signout");
        while(!SceneToSceneData.canSignout) {
            Debug.Log("Can't sign out.");
            yield return null;
        }
 
        Debug.Log("Signing out.");
        GlobalHelper.global.auth.SignOut();
        

    }


    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        Debug.Log("AuthStateChanged");
        Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
        Firebase.Auth.FirebaseUser user = null;


        if (senderAuth != null) userByAuth.TryGetValue(senderAuth.App.Name, out user);
        if (senderAuth == GlobalHelper.global.auth && senderAuth.CurrentUser != user)
        {
            bool signedIn = user != senderAuth.CurrentUser && senderAuth.CurrentUser != null;
            user = senderAuth.CurrentUser;
            userByAuth[senderAuth.App.Name] = user;

            if (signedIn)
            {
                GlobalHelper.global.email = user.Email;
            }
        }
        else
        {
            Debug.Log("Loading sign in scene.");
            SceneManager.LoadScene(signInScene);
        }

    }

    
}