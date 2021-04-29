//code taken from this tutorial https://spin.atomicobject.com/2020/06/09/firebase-unity-user-accounts/
using System;
using System.Collections;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignUpHandler : MonoBehaviour
{
    public SceneField signInScene;
    public TMP_InputField emailTextBox;
    public TMP_InputField passwordTextBox;
    public TMP_InputField confirmPasswordTextBox;
    public Button backButton;
    public Button signupButton;
    public TMP_Text passwordErrorText;
    public string displayName = "";

    public TMP_InputField usernameInputField;



    public void SignUpButtonClicked() {
        if (usernameInputField.text == "")
        {
            passwordErrorText.text = "Nothing in the username input field.";
            passwordErrorText.enabled = true;
        }

        passwordErrorText.enabled = false;
        if (passwordTextBox.text != confirmPasswordTextBox.text)
        {
            passwordErrorText.text = "Passwords do not match.";
            passwordErrorText.enabled = true;
        }
        else
        {
            CreateUserWithEmailAsync();
        }
    }

    public void BackButtonClicked() {
        SceneManager.LoadScene(signInScene);
    }
    bool loginAttemptComplete = false;


    private void Start()
    {
        //StartCoroutine(WaitLogin());
    }

    IEnumerator WaitLogin() {
          while(true) {
            if(loginAttemptComplete == true) {
                SceneManager.LoadScene(signInScene);

            }
            yield return null;
          }

    }

    // Create a user with the email and password.
    public void CreateUserWithEmailAsync()
    {
        string email = emailTextBox.text;
        string password = passwordTextBox.text;

        Debug.Log(String.Format("Attempting to create user {0}...", email));
        DisableUI();


        GlobalHelper.global.auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            EnableUI();
            LogTaskCompletion(task, "User Creation");
            
        }
        );
    }

    // Log the result of the specified task, returning true if the task
    // completed successfully, false otherwise.
    public void LogTaskCompletion(Task task, string operation)
    {

        if (task.IsCanceled)
        {
            Debug.Log(operation + " canceled.");
        }
        else if (task.IsFaulted)
        {
            Debug.Log(operation + " encounted an error.");
            foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
            {
                string authErrorCode = "";
                Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                if (firebaseEx != null)
                {
                    authErrorCode = String.Format("AuthError.{0}: ", ((AuthError)firebaseEx.ErrorCode).ToString() );
                    GetErrorMessage((AuthError)firebaseEx.ErrorCode);
                }
                Debug.Log(authErrorCode + exception.ToString());
            }
        }
        else if (task.IsCompleted)
        {
            Debug.Log(operation + " completed");
           
            SceneToSceneData.accountCreationSuccessful = true;
            SceneToSceneData.newSignupUsername = usernameInputField.text;

            passwordErrorText.text = "Account successfully created!.";
            loginAttemptComplete = true;
            SceneManager.LoadScene(signInScene);
        }

    }

    void DisableUI()
    {
        emailTextBox.DeactivateInputField();
        passwordTextBox.DeactivateInputField();
        confirmPasswordTextBox.DeactivateInputField();
        backButton.interactable = false;
        signupButton.interactable = false;
        passwordErrorText.enabled = false;
    }

    void EnableUI()
    {
        emailTextBox.ActivateInputField();
        passwordTextBox.ActivateInputField();
        confirmPasswordTextBox.ActivateInputField();
        backButton.interactable = true;
        signupButton.interactable = true;
    }

  
    void GetErrorMessage(AuthError errorCode)
    {
        switch (errorCode)
        {
            case AuthError.MissingPassword:
                passwordErrorText.text = "Missing password.";
                passwordErrorText.enabled = true;
                break;
            case AuthError.WeakPassword:
                passwordErrorText.text = "Too weak of a password.";
                passwordErrorText.enabled = true;
                break;
            case AuthError.InvalidEmail:
                passwordErrorText.text = "Invalid email.";
                passwordErrorText.enabled = true;
                break;
            case AuthError.MissingEmail:
                passwordErrorText.text = "Missing email.";
                passwordErrorText.enabled = true;
                break;
            case AuthError.UserNotFound:
                passwordErrorText.text = "Account not found.";
                passwordErrorText.enabled = true;
                break;
            case AuthError.EmailAlreadyInUse:
                passwordErrorText.text = "Email already in use.";
                passwordErrorText.enabled = true;
                break;
            default:
                passwordErrorText.text = "Unknown error occurred.";
                passwordErrorText.enabled = true;
                break;
        }
    }
}