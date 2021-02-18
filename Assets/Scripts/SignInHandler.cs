using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SignInHandler : MonoBehaviour
{
    public TMP_InputField email_input;
    public TMP_InputField password_input;
    public TextMeshProUGUI bad_signin_text;

    public void OnSignInButtonPressed() {

        GlobalHelper.global.auth.SignInWithEmailAndPasswordAsync(email_input.text, password_input.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                bad_signin_text.text = "BAD EMAIL OR PASSWORD";
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });

    }
}
