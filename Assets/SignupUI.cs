using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SignupUI : MonoBehaviour
{
    public SceneField logInMenu;
    public void BackToLoginScreen()
    {
        SceneManager.LoadScene(logInMenu);

    }
}
