using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[System.Serializable]
public class RatBuildings : MonoBehaviour
{

    
    public TextMeshProUGUI infoText;
    public GameData gameData;
    public string buildingName = "";
    public float baseCheeseCost = 20;
    public float cheeseCost = 20;
    public float cheesePerSecond = 10;
    [HideInInspector]
    public float totalCheesePerSecond = 0;
    [HideInInspector]
    public int amount = 0;

     
 

    private float _cheesePerSecondUpgradeModifier = 1.0f;
    public float cheesePerSecondUpgradeModifier
    {
        get { return _cheesePerSecondUpgradeModifier; }

        set { 
            _cheesePerSecondUpgradeModifier = value;
            CalculateCPS();
        }

    }

    float GetUnitCPS() {
        return cheesePerSecond * cheesePerSecondUpgradeModifier;

    }

  

    public float CalculateCost(int buyAmount) {

        float totalCost = 0f;
        totalCost += baseCheeseCost * Mathf.Pow(1.1f, amount);
        for (int i = 1; i < buyAmount; i++)
        {
            totalCost +=  baseCheeseCost * Mathf.Pow(1.1f, amount + i);
        }
        return totalCost;
    }



    private void Start() {
        ChangeText();
        StartCoroutine(UpdateCheese());
    }

    public void ChangeText() {
        cheeseCost = CalculateCost(0);
        infoText.text = buildingName + ": " + "\n" + "Amount: " + (int)amount + "\n" + "Cost: " + (int)cheeseCost + "\n" + "Unit CPS: " + (int)GetUnitCPS() + "\n" + "Total CPS: " + (int)totalCheesePerSecond + "\n";
    }

    private void OnValidate() {
        cheeseCost = CalculateCost(0);
        infoText.text = buildingName 
        + ": " + "\n" + "Amount: " + amount 
        + "\n" + "Cost: " + cheeseCost + "\n"
        + "Unit CPS: " + GetUnitCPS() + "\n" 
        + "Total CPS: " + totalCheesePerSecond + "\n";
    }

    public void LoadSaveData(BuildingData buildingData) {
        this.amount = buildingData.amount;
        CalculateCPS(); 
    }

    public BuildingData GetBuildingData() {
        return new BuildingData(buildingName, amount);
    }


    public void OnBuyButtonClick(int buy_amount) {

        float buyCost = CalculateCost(buy_amount);
        if (gameData.total_cheese - buyCost < 0)
            return;
        else {
            amount += buy_amount;
            gameData.total_cheese -= buyCost;
            CalculateCPS();

        }
       
    }

    void CalculateCPS() {
        totalCheesePerSecond = amount * cheesePerSecond * cheesePerSecondUpgradeModifier;
        ChangeText();
    }



    IEnumerator UpdateCheese() {
        WaitForSeconds wfs_cheese = new WaitForSeconds(1);
        while(true) {
            yield return wfs_cheese;
            gameData.total_cheese += this.totalCheesePerSecond;

        }

    }
}
