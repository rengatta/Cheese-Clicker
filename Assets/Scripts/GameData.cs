using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameData : MonoBehaviour
{

    public SaveManager saveManager;

    public float cheese_multiplier = 1f;
    public TextMeshProUGUI welcomeText;






    private float _total_cheese = 0;

    public float total_cheese   
    {
        get { 
            return _total_cheese; 
        }  
        set {
            if (value > _total_cheese) {

                saveManager.currentScoreData.totalCheeseCollected += value - _total_cheese;


            } else {
                saveManager.currentScoreData.totalCheeseSpent += value - _total_cheese;
            }

            _total_cheese = value; 

            if(value > saveManager.currentScoreData.highestCheese) {
                saveManager.currentScoreData.highestCheese = value;
            }
        }  
    }

  


    private string _username = "";
    public string username 
    {
        get { return _username; }  
        set {
            _username = value;
            welcomeText.text = "Welcome, " + username;
        } 
    }





}
