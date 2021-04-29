using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase.Database;
using Firebase.Extensions;

[System.Serializable]
public class BuildingData  {
    public string buildingName = "";
    public int amount = 0;

    public BuildingData(string buildingName, int amount) {
        this.buildingName = buildingName;
        this.amount = amount;
    }
}

[System.Serializable]
public class ScoreData {
    public float totalCheeseCollected = 0.0f;
    public float totalCheeseSpent = 0.0f;
    public float highestCheese = 0.0f;
    public float highestCPS = 0.0f;
}



[System.Serializable]
public class SaveData {


    public List<BuildingData> buildingsPurchased = new List<BuildingData>();
    public List<string> upgradesPurchased = new List<string>();
    public ScoreData scoreData = new ScoreData();


    public float currentCheese = 0f;
    public string username = "";
    public string email = "";

   
    public SaveData() {

    }

    public void AddBuildingData(BuildingData buildingData) {
        this.buildingsPurchased.Add(buildingData);
    }
}


public class SaveManager : MonoBehaviour
{
    public ScoreData currentScoreData = new ScoreData();
    
    public Transform buildings;
    public GameData gameData;

    public Transform buyableUpgrades;
    public Transform purchasedUpgrades;
    public SceneField gameScene;
    public TextMeshProUGUI saveText;

    public GameObject cantCloudSavePrompt;


    IEnumerator ShowSaveText(string text) {
        saveText.text = text;

        yield return new WaitForSeconds(3);

        saveText.text = "";

    }

    public void ResetSaveData() {

        SceneToSceneData.username = gameData.username;
        SceneToSceneData.resetSave = true;
        ResetScene();
    }

    public void StartAutosave(float intervalMinutes) {
        StartCoroutine(Autosave(intervalMinutes));
    }

    IEnumerator Autosave(float interval) {
        while(true) {
            yield return new WaitForSeconds(interval * 60);
            CreateSaveData();
            StartCoroutine(ShowSaveText("Autosaved."));
        }
    }

    void Start() {

        if (GlobalHelper.global.userID == "") GlobalHelper.global.userID = "Guest";


      //  bool getCloudSaveUsername = false;

        if(SceneToSceneData.newSignupUsername != "") {
            gameData.username = SceneToSceneData.newSignupUsername;
        }
        else if (SceneToSceneData.resetSave)
        {
            gameData.username = SceneToSceneData.username;

        }
        else if (GlobalHelper.global.userID == "Guest")
        {
            gameData.username = "Guest";

        }
     
        if (SceneToSceneData.resetSave != true) {
            
            if (GlobalHelper.global.userID == "Guest") {
                Debug.Log("LOADING GUEST");
                LoadSaveData("Guest");

            }
            else {
                Debug.Log("CLOUDSAVE1");
                LoadCloudSaveData();

            }

        } else {

            //if the save is resetting, create a new save data on top of the initial starting scene to save over the previous data with a blank slate
            CreateSaveData();
            SceneToSceneData.resetSave = false;
        }

        StartAutosave(1);
    }


    SaveData GetSaveData() {
        SaveData newSave = new SaveData();
        newSave.scoreData = currentScoreData;
        newSave.currentCheese = gameData.total_cheese;

        foreach (Transform child in purchasedUpgrades)
        {
            newSave.upgradesPurchased.Add(child.GetComponent<Upgrade>().upgradeName);
        }


        foreach (Transform child in buildings)
        {
            RatBuildings ratBuilding = child.GetComponent<RatBuildings>();
            if (ratBuilding != null)
            {
                newSave.AddBuildingData(ratBuilding.GetBuildingData());
            }
        }
        return newSave;
    }

    //savebuttonpressed
    public void CreateSaveData() {

        Save(GetSaveData());
        StartCoroutine(ShowSaveText("Saved."));
    }



    public void LoadCloudSaveData() {
        if (GlobalHelper.global.userID != "Guest")
        {
            Debug.Log("LOADING CLOUD SAVE");

            // string json = JsonUtility.ToJson(saveData);

            FirebaseDatabase.DefaultInstance.GetReference("users/" + GlobalHelper.global.auth.CurrentUser.UserId + "/saveData").GetValueAsync().ContinueWithOnMainThread(t => {
                if (t.IsCanceled)
                {
                    Debug.Log("FirebaseDatabaseError: IsCanceled: " + t.Exception);
                    return;
                }

                if (t.IsFaulted)
                {
                    Debug.Log("FirebaseDatabaseError: IsFaulted:" + t.Exception);
                    return;
                }

                if (t.Result.Exists)  {
                    DataSnapshot snapshot = t.Result;

                    SaveData saveData1 = JsonUtility.FromJson<SaveData>(snapshot.GetRawJsonValue());
                    LoadSaveData2(saveData1);

                } else {
                    Debug.Log("No compatible data exists.");
                    
                }
             
            });

          
          
        }
    }

    public void LoadSaveData2(SaveData newLoadData)
    {
        Debug.Log("LOAD2");
        currentScoreData = newLoadData.scoreData;

        for (int i = 0; i < buildings.childCount; i++)
        {

            RatBuildings ratBuilding = buildings.GetChild(i).GetComponent<RatBuildings>();
            if (ratBuilding != null)
            {
                int index = newLoadData.buildingsPurchased.FindIndex(x => x.buildingName == ratBuilding.buildingName);
                ratBuilding.LoadSaveData(newLoadData.buildingsPurchased[index]);
            }

        }
        gameData.total_cheese = newLoadData.currentCheese;
        gameData.username = newLoadData.username;

        for (int i = buyableUpgrades.childCount - 1; i >= 0; i--)
        {

            Upgrade upgradeInstance = buyableUpgrades.GetChild(i).GetComponent<Upgrade>();

            if (upgradeInstance != null)
            {
                int index = newLoadData.upgradesPurchased.FindIndex(x => x == upgradeInstance.upgradeName);


                if (index != -1)
                {
                    upgradeInstance.PurchaseWithoutCost();
                }

            }

        }

    }




    public void Save(SaveData saveData) {

        if (GlobalHelper.global.userID != "Guest") {

            saveData.username = gameData.username;
            saveData.email = GlobalHelper.global.email.Replace(".", "_");
            
            string json = JsonUtility.ToJson(saveData);

            DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("users/" + GlobalHelper.global.auth.CurrentUser.UserId + "/saveData");
            
            reference.SetRawJsonValueAsync(json);

        } else {
            cantCloudSavePrompt.SetActive(true);
            string path = Application.streamingAssetsPath + "\\Saves\\" + gameData.username;

            if (path.Length == 0)
            {
                Debug.Log("Save failed.");
                return;
            }

            saveData.username = gameData.username;
            saveData.email = GlobalHelper.global.email;

            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText(path, json);
        }
      
    }

    public void ResetScene() {
        SceneManager.LoadScene(gameScene);

    }
  
    public bool LoadSaveDataGeneric(string path) {

        if (!File.Exists(path)) {
            Debug.Log("FILE DOES NOT EXIST.");
            return false;
        }
        else {
            string readText = File.ReadAllText(path);

            SaveData newLoadData = JsonUtility.FromJson<SaveData>(readText);
            currentScoreData = newLoadData.scoreData;

            for (int i = 0; i < buildings.childCount; i++)
            {

                RatBuildings ratBuilding = buildings.GetChild(i).GetComponent<RatBuildings>();
                if (ratBuilding != null)
                {
                    int index = newLoadData.buildingsPurchased.FindIndex(x => x.buildingName == ratBuilding.buildingName);
                    ratBuilding.LoadSaveData(newLoadData.buildingsPurchased[index]);
                }

            }
            gameData.total_cheese = newLoadData.currentCheese;
            gameData.username = newLoadData.username;
    
            for(int i=buyableUpgrades.childCount-1; i>=0; i--) {
       
                Upgrade upgradeInstance = buyableUpgrades.GetChild(i).GetComponent<Upgrade>();
       
                if (upgradeInstance != null)
                {
                    int index = newLoadData.upgradesPurchased.FindIndex(x => x == upgradeInstance.upgradeName);


                    if (index != -1)
                    {
                        upgradeInstance.PurchaseWithoutCost();
                    }

                }

            }

        }
        SceneToSceneData.isLoading = false;
        SceneToSceneData.loadPath = "";
        return true;
    }


    public void LoadSaveData(string username) {
        //string path = Application.streamingAssetsPath + "\\LevelSaves\\" + levelName;
        string path = Application.streamingAssetsPath + "\\Saves\\" + username;
        LoadSaveDataGeneric(path);
    }

}
