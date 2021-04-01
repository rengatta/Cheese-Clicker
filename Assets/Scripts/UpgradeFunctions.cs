using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeFunctions : MonoBehaviour
{
    public GameData gameData;

    public RatBuildings ratBuildingScript;
    public RatBuildings cowBuildingScript;
    public RatBuildings factoryBuildingScript;

    public void RatUpgrade1()
    {
        ratBuildingScript.cheesePerSecondUpgradeModifier *= 2.0f;

    }

    public void CowUpgrade1()
    {
        cowBuildingScript.cheesePerSecondUpgradeModifier *= 2.0f;

    }

    public void FactoryUpgrade1()
    {
        factoryBuildingScript.cheesePerSecondUpgradeModifier *= 2.0f;

    }

}
