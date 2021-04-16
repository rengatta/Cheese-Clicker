//code taken from this tutorial https://spin.atomicobject.com/2020/06/09/firebase-unity-user-accounts/
using System;
using System.Collections.Generic;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class AuthManager : ScriptableObject
{
    public SceneField signInScene;
    public SceneField loadingScene;
    public SceneField mainScene;

    // Firebase Authentication instance.


    // Firebase User keyed by Firebase Auth.
    protected Dictionary<string, Firebase.Auth.FirebaseUser> userByAuth =
      new Dictionary<string, Firebase.Auth.FirebaseUser>();

    // Flag to check if fetch token is in flight.
    private bool fetchingToken = false;

 
    public bool signingOut = false;
    // Handle initialization of the necessary firebase modules:




    public void InitializeFirebase()
    {



        GlobalHelper.global.auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        GlobalHelper.global.auth.StateChanged -= AuthStateChanged;
        GlobalHelper.global.auth.IdTokenChanged -= IdTokenChanged;
        GlobalHelper.global.auth.StateChanged += AuthStateChanged;
        GlobalHelper.global.auth.IdTokenChanged += IdTokenChanged;

        Debug.Log("CURRENT USER: " + GlobalHelper.global.auth.CurrentUser.Email);
        if (GlobalHelper.global.auth.CurrentUser != null)
        {
            Debug.Log("SIGNING OUT");
            GlobalHelper.global.auth.SignOut();

        }


        AuthStateChanged(this, null);
        
    }

    

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
        Firebase.Auth.FirebaseUser user = null;


        if (senderAuth != null) userByAuth.TryGetValue(senderAuth.App.Name, out user);
        if (senderAuth == GlobalHelper.global.auth && senderAuth.CurrentUser != user)
        {
            bool signedIn = user != senderAuth.CurrentUser && senderAuth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
                //SceneManager.LoadScene(signInScene);
            }
            user = senderAuth.CurrentUser;
            userByAuth[senderAuth.App.Name] = user;

            if (signedIn)
            {
                GlobalHelper.global.email = user.Email;
                //Debug.Log("Signed in " + user.DisplayName);
               // DisplayDetailedUserInfo(user, 1);
               // SceneManager.LoadScene(mainScene);
            }
        }
        else
        {
            SceneManager.LoadScene(signInScene);
        }




    }

    // Track ID token changes.
    void IdTokenChanged(object sender, System.EventArgs eventArgs)
    {
        Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
        if (senderAuth == GlobalHelper.global.auth && senderAuth.CurrentUser != null && !fetchingToken)
        {
            senderAuth.CurrentUser.TokenAsync(false).ContinueWithOnMainThread(
              task => Debug.Log(String.Format("Token[0:8] = {0}", task.Result.Substring(0, 8))));
        }
    }

    // Display a more detailed view of a FirebaseUser.
    protected void DisplayDetailedUserInfo(Firebase.Auth.FirebaseUser user, int indentLevel)
    {
        string indent = new String(' ', indentLevel * 2);
        DisplayUserInfo(user, indentLevel);
        //Debug.Log(String.Format("{0}Anonymous: {1}", indent, user.IsAnonymous));
        //Debug.Log(String.Format("{0}Email Verified: {1}", indent, user.IsEmailVerified));
        //Debug.Log(String.Format("{0}Phone Number: {1}", indent, user.PhoneNumber));
        var providerDataList = new List<Firebase.Auth.IUserInfo>(user.ProviderData);
        var numberOfProviders = providerDataList.Count;
        if (numberOfProviders > 0)
        {
            for (int i = 0; i < numberOfProviders; ++i)
            {
                //Debug.Log(String.Format("{0}Provider Data: {1}", indent, i));
                DisplayUserInfo(providerDataList[i], indentLevel + 2);
            }
        }
    }

    // Display user information.
    protected void DisplayUserInfo(Firebase.Auth.IUserInfo userInfo, int indentLevel)
    {
        string indent = new String(' ', indentLevel * 2);
        var userProperties = new Dictionary<string, string> {
        {"Display Name", userInfo.DisplayName},
        {"Email", userInfo.Email},
        {"Photo URL", userInfo.PhotoUrl != null ? userInfo.PhotoUrl.ToString() : null},
        {"Provider ID", userInfo.ProviderId},
        {"User ID", userInfo.UserId}
      };
        foreach (var property in userProperties)
        {
            if (!String.IsNullOrEmpty(property.Value))
            {
               // Debug.Log(String.Format("{0}{1}: {2}", indent, property.Key, property.Value));
            }
        }
    }

    
}