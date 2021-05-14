using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeUsername : MonoBehaviour
{
    public TMP_InputField inputText;
    public SaveManager saveManager;
    public GameObject changeUsernameRoot;

    public GameObject warningPromptRoot;
    public TextMeshProUGUI warningPromptText;


    public void ChangeUsernameConfirmButtonPressed() {

        if (inputText.text != "" && saveManager.gameData.username != "Guest") {
            saveManager.gameData.username = inputText.text;
            SceneToSceneData.username = inputText.text;
            saveManager.CreateSaveData();
            changeUsernameRoot.SetActive(false);
        } else {
   
            if(saveManager.gameData.username == "Guest") {
                warningPromptRoot.SetActive(true);
                warningPromptText.text = "Cannot change username on a local Guest account.";
                changeUsernameRoot.SetActive(false);
            }

        }
    }
}
