using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RatBuildings : MonoBehaviour
{

    public TextMeshProUGUI amount_text;
    public TextMeshProUGUI cost_text;
    public TextMeshProUGUI unit_cps_text;
    public TextMeshProUGUI total_cps_text;


    public GameData gameData;

    public int cheese_cost = 20;


    public int cheese_per_second = 10;

    [HideInInspector]
    public int total_cheese_per_second = 0;

    [HideInInspector]
    public int amount = 0;

    private void Start()
    {
        ChangeText();
        StartCoroutine(UpdateCheese());
    }

    public void ChangeText() {
        amount_text.text = "Amount: " + amount;
        cost_text.text = "Cost: " + cheese_cost;
        unit_cps_text.text = "Unit CPS: " + cheese_per_second;
        total_cps_text.text = "Total CPS: " + total_cheese_per_second;

    }


    public void OnBuyButtonClick(int buy_amount) {

        int buy_cost = buy_amount * cheese_cost;
        if (gameData.total_cheese - buy_cost < 0)
            return;
        else {
            amount += buy_amount;
            gameData.total_cheese -= buy_cost;
            CalculateCPS();
            ChangeText();
        }
       
    }

    void CalculateCPS() {
        total_cheese_per_second = amount * cheese_per_second;
    }



    IEnumerator UpdateCheese() {
        WaitForSeconds wfs_cheese = new WaitForSeconds(1);
        while(true) {
            yield return wfs_cheese;
            gameData.total_cheese += this.total_cheese_per_second;

        }

    }
}
