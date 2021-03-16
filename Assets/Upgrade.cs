using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool mouseEntered = false;
    public GameObject upgradeOverlay;
    public TextMeshProUGUI upgradeOverlayDescription;
    public TextMeshProUGUI upgradeOverlayName;
    public TextMeshProUGUI upgradeOverlayCost;
    public string upgradeText;
    public string upgradeName;
    public float upgradeCost;
    public GameData gameData;
    public UnityEvent upgradeBought;
    bool purchased = false;

    public GameObject purchasedUpgradesContent;


    public void GetUpgradeInfo() {
        upgradeOverlayDescription.text = upgradeText;
        upgradeOverlayName.text = upgradeName;
        if (!purchased)
        {
            upgradeOverlayCost.text = "Cost: " + upgradeCost.ToString();
            if (gameData.total_cheese - upgradeCost < 0)
                upgradeOverlayCost.color = Color.red;
            else
                upgradeOverlayCost.color = Color.green;
        }   
        else
        {
            upgradeOverlayCost.text = "Purchased";
            upgradeOverlayCost.color = Color.green;
        }

    }


    public void RemoveUpgrade() {
        upgradeOverlay.SetActive(false);
        purchased = true;
        mouseEntered = false;
        GetComponent<Button>().interactable = false;
        transform.SetParent(purchasedUpgradesContent.transform);
       
        //Destroy(this.gameObject);
    }

    public void AttemptPurchase() {
        if (gameData.total_cheese - upgradeCost < 0)
            return;
        else
        {
            gameData.total_cheese -= upgradeCost;
            upgradeBought.Invoke();
            RemoveUpgrade();

        }
    }
 

    private void Update() {
        if(mouseEntered) {
            upgradeOverlay.transform.position = Input.mousePosition;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (!mouseEntered) {
            upgradeOverlay.SetActive(true);
            GetUpgradeInfo();
            mouseEntered = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        mouseEntered = false;
        upgradeOverlay.SetActive(false);
    }



}
