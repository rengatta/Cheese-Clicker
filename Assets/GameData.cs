using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameData : MonoBehaviour
{


    public float cheese_multiplier = 1f;
    
    private int _total_cheese = 0;
    public int total_cheese   // property
    {
        get { return _total_cheese; }   // get method
        set {
        
        _total_cheese = value; 
        
        
        
        }  // set method
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
