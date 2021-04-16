using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Extensions;
using UnityEngine.UI;

public class SignInHandler : MonoBehaviour
{
    public TMP_InputField email_input;
    public TMP_InputField password_input;
    public TextMeshProUGUI bad_signin_text;
    public SceneField loadingScene;
    public SceneField gameScene;
    public Toggle saveSigninInfoToggle;

    
    public void Start()
    {

        SceneToSceneData.username = "";
        if (GlobalHelper.global == null || GlobalHelper.global.auth == null)
        {
            SceneManager.LoadScene(loadingScene);
            
        }else {
            if (PlayerPrefs.GetInt("SaveInfoToggled") == 1) {
                saveSigninInfoToggle.isOn = true;
                if (PlayerPrefs.GetInt("SaveInfoSaved") == 1)
                {
                    email_input.text = PlayerPrefs.GetString("username");
                    password_input.text = PlayerPrefs.GetString("password");
                }

            }



        }

       

      
    }
    public void ToggleSaveSigninInfo() {
        if (saveSigninInfoToggle.isOn)
        {
            PlayerPrefs.SetInt("SaveInfoToggled", 1);
        } else {
            PlayerPrefs.SetInt("SaveInfoToggled", 0);
        }

    }


    public void SaveSigninInfo() {
        

    }

    public void LoginAsGuest() {
        GlobalHelper.global.userID = "Guest";
        SceneManager.LoadScene(gameScene);
      
    }

    public void OnSignInButtonPressed() {
        if(email_input.text == "" || password_input.text == "") {
            bad_signin_text.text = "Please enter an email and password.";
            return;
        }


        GlobalHelper.global.auth.SignInWithEmailAndPasswordAsync(email_input.text, password_input.text).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                
                Debug.Log("SignInWithEmailAndPasswordAsync was canceled.");
                bad_signin_text.text = "Login failed. Please try again";
            }
            else if (task.IsFaulted)
            {
                SceneToSceneData.genericString = "TESTING";
                bad_signin_text.text = "BAD EMAIL OR PASSWORD";

                // Debug.Log("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);

            } else {
                bad_signin_text.text = "Login succesful!";
                SceneToSceneData.canCloudSave = true;
                GlobalHelper.global.userID = GlobalHelper.global.auth.CurrentUser.UserId;
                PlayerPrefs.SetInt("SaveInfoSaved", 1);
                PlayerPrefs.SetString("username", email_input.text);
                PlayerPrefs.SetString("password", password_input.text);
                SceneManager.LoadScene(gameScene);
                //Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            }


        });


    }
}
