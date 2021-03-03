using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class BuildingData  {
    public string buildingName = "";
    public float baseCheeseCost = 20;
    public float cheeseCost = 20;
    public float cheesePerSecond = 10;
    public float totalCheesePerSecond = 0;
    public int amount = 0;
    public BuildingData(string buildingName, float baseCheeseCost, float cheeseCost, float cheesePerSecond, float totalCheesePerSecond, int amount) {
        this.buildingName = buildingName;
        this.baseCheeseCost = baseCheeseCost;
        this.cheeseCost = cheeseCost;
        this.cheesePerSecond = cheesePerSecond;
        this.totalCheesePerSecond = totalCheesePerSecond;
        this.amount = amount;
    }
}

[System.Serializable]
public class SaveData {

    public List<BuildingData> buildingData = new List<BuildingData>();
    public float currentCheese = 0f;
    public string username = "";
    public SaveData() {

    }

    public void AddBuildingData(BuildingData buildingData) {
        this.buildingData.Add(buildingData);
    }
}


public class SaveManager : MonoBehaviour
{
    public Transform buildings;
    public GameData gameData;
    
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
        }
    }

    void Start() {

        CreateSaveData("StartData");
        gameData.username = "Guest";
    }




    public void CreateSaveData(string username)
    {

        SaveData newSave = new SaveData();
        newSave.currentCheese = gameData.total_cheese;
        foreach (Transform child in buildings)
        {
            RatBuildings ratBuilding = child.GetComponent<RatBuildings>();
            if (ratBuilding != null)
            {
                newSave.AddBuildingData(ratBuilding.GetBuildingData());
            }
        }
        Save(newSave, username);
    }


    public void CreateSaveData() {

        SaveData newSave = new SaveData();
        newSave.currentCheese = gameData.total_cheese;
        foreach (Transform child in buildings) {
            RatBuildings ratBuilding = child.GetComponent<RatBuildings>();
            if(ratBuilding != null) {
                newSave.AddBuildingData(ratBuilding.GetBuildingData());
            }
        }
        Save(newSave);
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


    public void LoadSaveData(string username)
    {
        //string path = Application.dataPath + "\\LevelSaves\\" + levelName;
        string path = Application.dataPath + "\\Saves\\" + username;

        if (!File.Exists(path))
        {
            Debug.Log("FILE DOES NOT EXIST.");
        }
        else
        {
            string readText = File.ReadAllText(path);

            SaveData currentSaveData = JsonUtility.FromJson<SaveData>(readText);

            for (int i = 0; i < buildings.childCount; i++)
            {
                RatBuildings ratBuilding = buildings.GetChild(i).GetComponent<RatBuildings>();
                if (ratBuilding != null)
                {
                    int index = currentSaveData.buildingData.FindIndex(x => x.buildingName == ratBuilding.buildingName);
                    ratBuilding.LoadSaveData(currentSaveData.buildingData[index]);
                }

            }
            gameData.total_cheese = currentSaveData.currentCheese;
            gameData.username = currentSaveData.username;


        }

    }
    public void LoadSaveData()
    {
        //string path = Application.dataPath + "\\LevelSaves\\" + levelName;
        string path = EditorUtility.OpenFilePanel("Select a level file", Application.dataPath + "\\Saves\\", "");

        if (path.Length == 0)
        {
            Debug.Log("File failed to load.");
            return;
        }

        if (!File.Exists(path))
        {
            Debug.Log("FILE DOES NOT EXIST.");
        }
        else
        {
            string readText = File.ReadAllText(path);

            SaveData currentSaveData = JsonUtility.FromJson<SaveData>(readText);

            for(int i=0; i < buildings.childCount; i++) {
                RatBuildings ratBuilding = buildings.GetChild(i).GetComponent<RatBuildings>();
                if (ratBuilding != null)
                {
                    int index = currentSaveData.buildingData.FindIndex(x => x.buildingName == ratBuilding.buildingName);
                    ratBuilding.LoadSaveData(currentSaveData.buildingData[index]);
                }

            }
            gameData.total_cheese = currentSaveData.currentCheese;
            gameData.username = currentSaveData.username;
 

        }

    }



  
}
