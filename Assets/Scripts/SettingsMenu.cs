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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
