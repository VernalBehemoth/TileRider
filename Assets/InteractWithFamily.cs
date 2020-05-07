using UnityEngine;

public class InteractWithFamily : MonoBehaviour
{
    private bool playerIsNearHouse;
    public GameObject familyScreen;
    private GameManagerScript gameManagerScript;
    private Family family;
    private InventorySlot itemToGive;
    private InfoMessage infoMessage;
    private void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("GameManager");
        gameManagerScript = go.GetComponent<GameManagerScript>();
        family = go.GetComponent<Family>();
        infoMessage = GameObject.FindGameObjectWithTag("InfoCanvas").GetComponent<InfoMessage>();
    }

    private void Update()
    {
        if (totalGiven >= 50)
        {
            Interactor.ProvidedForFamilyBerries = true;
        }

        if (goldGiven >= 3)
        {
            Interactor.ProvidedForFamilyGold = true;
        }
    }
    public void OpenMenu(InventorySlot inventorySlot)
    {
        if (inventorySlot != null)
        {
            if (inventorySlot.inventoryGroup != null && inventorySlot.inventoryGroup.InventoryItem != InventoryItem.Hammer)
            {
                if (inventorySlot.inventoryGroup.Amount > 0)
                {
                    itemToGive = inventorySlot;
                    familyScreen.GetComponent<Canvas>().enabled = true;
                }
                else { infoMessage.DisplayMessage("You do not have this resource", 2F); }
            }
            if (inventorySlot.inventoryGroup != null && inventorySlot.inventoryGroup.InventoryItem == InventoryItem.Hammer)
            {
                infoMessage.DisplayMessage("Hammers for the bridge...", 2F);
            }
        }
        else
        {
            infoMessage.DisplayMessage("Pick something up first!", 2F);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && Interactor.BridgeFixed)
        {
            playerIsNearHouse = true;
        }
    }

    private int totalGiven = 0;
    private int goldGiven = 0;
    public void GiveToFamilyMember(int memberNumber)
    {
        if (family == null || gameManagerScript == null || itemToGive == null || itemToGive.inventoryGroup == null)
        {
            return;
        }

        family.GiveToMember(1, memberNumber, itemToGive.inventoryGroup.InventoryItem);

        if (itemToGive.inventoryGroup.InventoryItem == InventoryItem.Gold)
        {
            goldGiven++;
        }
        else
        {
            totalGiven++;
        }

        gameManagerScript.AddItemToInventory(itemToGive.inventoryGroup.InventoryItem, -1);
    }
    public void CancelClick()
    {
        familyScreen.GetComponent<Canvas>().enabled = false;
        itemToGive = null;
    }
    public void Consume()
    {
        if (itemToGive.inventoryGroup.InventoryItem == InventoryItem.Hunger_Berry)
        {
            gameManagerScript.GetComponent<GameManagerScript>().HungerAdjust(20);
            infoMessage.DisplayMessage("Hunger is satisfied +20", 2F);
        }
        else if (itemToGive.inventoryGroup.InventoryItem == InventoryItem.Thirst_Berry)
        {
            gameManagerScript.GetComponent<GameManagerScript>().ThirstDecrease(20);
            infoMessage.DisplayMessage("Thirst is quenched +20", 2F);
        }
        else if (itemToGive.inventoryGroup.InventoryItem == InventoryItem.Poison_Berry)
        {
            gameManagerScript.GetComponent<GameManagerScript>().GainHealth(-10);
            infoMessage.DisplayMessage("Ahh poison! -10 health", 2F);
        }
        else if (itemToGive.inventoryGroup.InventoryItem == InventoryItem.Double_Health_Berry)
        {
            gameManagerScript.GetComponent<GameManagerScript>().GainHealth(20);
            infoMessage.DisplayMessage("Ooo I feel great! +20 health", 2F);
        }
        else if (itemToGive.inventoryGroup.InventoryItem == InventoryItem.Stamina_Berry)
        {
            gameManagerScript.GetComponent<GameManagerScript>().UpdateCurrentStamina(20);
            infoMessage.DisplayMessage("I could go on for hours! +20 Stamina", 2F, true);
        }
    }
}
