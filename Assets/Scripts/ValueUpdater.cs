using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ValueUpdater : MonoBehaviour
{
    public GameData gameData;


    public TextMeshProUGUI cheese_text;
    public TextMeshProUGUI cps_text;


    IEnumerator CalculateCPS() {
        WaitForSeconds wfs_cps = new WaitForSeconds(1);
        float prev_cheese = 0;
        float new_cheese = 0;
        while (true) {
            prev_cheese = gameData.total_cheese;
            yield return wfs_cps;
            new_cheese = gameData.total_cheese;
            cps_text.text = "Total CPS: " + (int)(new_cheese - prev_cheese);
            if(gameData.saveManager.currentScoreData.highestCPS < new_cheese - prev_cheese) {
                gameData.saveManager.currentScoreData.highestCPS = new_cheese - prev_cheese;
            }

        }

    }
    public void Start()
    {
        StartCoroutine(CalculateCPS());
    }


    void Update()
    {
        cheese_text.text = "Cheese: " + (int)gameData.total_cheese;
    }
}
