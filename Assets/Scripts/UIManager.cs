using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;




public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI welcomeText;
    public GameObject settingsMenu;
    public SceneField logInMenu;

    void Start()
    {
        welcomeText.text = "Welcome, " + GlobalHelper.global.email;
    }


    public void ToggleSettingsMenu() {
        if(settingsMenu.activeSelf) {
            settingsMenu.SetActive(false);
        } else {
            settingsMenu.SetActive(true);
        }

    }

    public void LogoutButtonPressed() {
        GlobalHelper.global.auth.SignOut();

    }




}
