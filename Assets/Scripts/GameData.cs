using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameData : MonoBehaviour
{

    public float cheese_multiplier = 1f;
    public TextMeshProUGUI welcomeText;

    private float _total_cheese = 0;
    public float total_cheese   
    {
        get { return _total_cheese; }  
        set {
        _total_cheese = value; 
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
