using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI welcomeText;
    public GameObject settingsMenu;
    public AudioSource audioSource;
    public Toggle muteToggle;

    void Start()
    {

    }

    public void ToggleSettingsMenu() {
        if(settingsMenu.activeSelf) {
            settingsMenu.SetActive(false);
        } else {
            settingsMenu.SetActive(true);
        }

    }

    public void LogoutButtonPressed() {
        if(GlobalHelper.global.auth != null) {
            GlobalHelper.global.auth.SignOut();
        }
        
    }

    public void MuteToggleChanged() {
        if(muteToggle.isOn) {
            audioSource.Pause();
        } else {
            audioSource.UnPause();
        }

    }


}
