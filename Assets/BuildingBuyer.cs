using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface BuildingInterface {
    int cheese_cost {
        get; set;
    }

    int cheese_per_second {
        get; set;
    }

    int total_cheese_per_second {
        get; set;
    }

    int amount {
        get; set;
    }

}





public class BuildingBuyer : MonoBehaviour
{
    public GameData gameData;

    public void BuyBuilding() {



    }






}
