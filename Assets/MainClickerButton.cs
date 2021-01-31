using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainClickerButton : MonoBehaviour
{
    public GameData gameData;
    public int cheesePerClick = 1;

    public void OnButtonClick() {

        gameData.total_cheese += cheesePerClick;

    }
}
