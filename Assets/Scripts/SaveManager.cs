using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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


    public float currentCheese = 0f;
    public string username = "";


    public ScoreData scoreData = new ScoreData();

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

    IEnumerator ShowSaveText(string text) {
        saveText.text = text;

        yield return new WaitForSeconds(3);

        saveText.text = "";

    }


    public void ResetSaveData()
    {
        string username = gameData.username;
        LoadSaveData("StartData");
        gameData.username = username;
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

        CreateSaveData("StartData");
        gameData.username = "Guest";


        if(SceneToSceneData.isLoading == true) {
            LoadSaveDataGeneric(SceneToSceneData.loadPath);
        }

        StartAutosave(1);
    }


    SaveData GetSaveData() {
        SaveData newSave = new SaveData();
        newSave.scoreData = currentScoreData;


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

    public void CreateSaveData(string username)
    {
        Save(GetSaveData(), username);
        StartCoroutine(ShowSaveText("Saved."));
    }


    public void CreateSaveData() {

        Save(GetSaveData());
        StartCoroutine(ShowSaveText("Saved."));
    }

    public void Save(SaveData saveData)
    {
        string username = GlobalHelper.global.email;
        string path;
        if (username == "")
            path = Application.dataPath + "\\Saves\\" + "guest";
        else
            path = Application.dataPath + "\\Saves\\" + username;

        if (path.Length == 0) {
            Debug.Log("Save failed.");
            return;
        }

        saveData.username = username;

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, json);
    }


    public void Save(SaveData saveData, string username)
    {

        string path = Application.dataPath + "\\Saves\\" + username;
        saveData.username = username;
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, json);
    }

  
    public void ResetScene() {
        SceneManager.LoadScene(gameScene);

    }

    public bool LoadSaveDataGeneric(string path) {

        if (!File.Exists(path))
        {
            Debug.Log("FILE DOES NOT EXIST.");
            return false;
        }
        else
        {
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


    public void LoadSaveData(string username)
    {
        //string path = Application.dataPath + "\\LevelSaves\\" + levelName;
        string path = Application.dataPath + "\\Saves\\" + username;
        SceneToSceneData.isLoading = true;
        SceneToSceneData.loadPath = path;
        ResetScene();
       // LoadSaveDataGeneric(path);

    }
    public void LoadSaveData()
    {
        //string path = Application.dataPath + "\\LevelSaves\\" + levelName;
        string path = EditorUtility.OpenFilePanel("Select a level file", Application.dataPath + "\\Saves\\", "");
        if(File.Exists(path)) {
            SceneToSceneData.isLoading = true;
            SceneToSceneData.loadPath = path;
            ResetScene();
        }
        //LoadSaveDataGeneric(path);

    }



  
}
