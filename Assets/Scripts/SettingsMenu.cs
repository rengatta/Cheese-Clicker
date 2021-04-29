using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public GameObject scoresMenu;
    public GameObject mainSettingsMenu;
    public TextMeshProUGUI scoreText;
    public SaveManager saveManager;

    public GameObject warningPromptRoot;
    public TextMeshProUGUI warningPromptText;

    public GameObject deleteUserPrompt;

    public void OnScoresButtonPressed() {
        scoresMenu.SetActive(true);
        mainSettingsMenu.SetActive(false);

        scoreText.text = "Highest CPS: " + (int)saveManager.currentScoreData.highestCPS +
        "\nHighest Cheese: " + (int)saveManager.currentScoreData.highestCheese +
        "\nTotal Cheese Made: " + (int)saveManager.currentScoreData.totalCheeseCollected +
        "\nTotal Cheese Spent: " + (int)saveManager.currentScoreData.totalCheeseSpent;
    }


    public void ScoresMenuBackButtonPressed() {
        scoresMenu.SetActive(false);
        mainSettingsMenu.SetActive(true);
    }



    public void CheckDeleteUserAccount() {
        if (GlobalHelper.global.userID == "Guest")
        {
            warningPromptRoot.SetActive(true);
            warningPromptText.text = "This functionality is disabled for local guests.";

        }
        else
        {
            deleteUserPrompt.SetActive(true);

        }

    }

    public void DeleteUserAccount() {
        GlobalHelper.global.authManager.DeleteUserAccount();

    }



}
