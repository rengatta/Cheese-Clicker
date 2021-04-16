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

    public GameObject upgradesRoot;
    public GameObject purchasedUpgradesRoot;
    bool upgradesActive = true;

    public SceneField loadingScene;
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
        SceneManager.LoadScene(loadingScene);
        
    }

    public void MuteToggleChanged() {
        if(muteToggle.isOn) {
            audioSource.Pause();
        } else {
            audioSource.UnPause();
        }

    }

    public void ShowYourUpgradesButtonPressed()
    {
        if(upgradesActive) {
            upgradesRoot.SetActive(false);
            purchasedUpgradesRoot.SetActive(true);
            upgradesActive = false;
        } else {
            upgradesRoot.SetActive(true);
            purchasedUpgradesRoot.SetActive(false);
            upgradesActive = true;

        }

    }



}
