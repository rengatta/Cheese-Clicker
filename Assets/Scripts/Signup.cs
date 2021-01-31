using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signup : MonoBehaviour
{
    FirebaseAuth auth;
    // Start is called before the first frame update
    void Start()
    {
        FirebaseInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //https://firebase.google.com/docs/auth/unity/manage-users
    //https://spin.atomicobject.com/2020/06/09/firebase-unity-user-accounts/
    public void FirebaseInit() {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;


    }



    //https://firebase.google.com/docs/auth/unity/start
    public void SignupButtonClick() {

        string email = "";
        string password = "";

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });


    }


    public void DeleteUser() {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            user.DeleteAsync().ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("DeleteAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("DeleteAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User deleted successfully.");
            });
        }


    }


}
