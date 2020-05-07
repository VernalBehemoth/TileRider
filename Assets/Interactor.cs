using System.Linq;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public bool CanFixBridge;
    public bool AreBerriesCollected;
    public static bool ProvidedForFamilyBerries { get; set; }
    public static bool ProvidedForFamilyGold { get; internal set; }
    public static bool BridgeFixed { get; internal set; }
    public static bool FindTheFamilyMember { get; internal set; }

    public GameManagerScript gameManager = null;
    private InfoMessage infoMessage;

    public bool ChestsLocated { get; internal set; }
    public static int chestsLocated = 0;
    private void Start()
    {
    }
    private void OnEnable()
    {
        infoMessage = GameObject.FindGameObjectWithTag("InfoCanvas").GetComponent<InfoMessage>();
    }

    public void AddToInventory(InventoryItem item, int num)
    {
        gameManager.AddItemToInventory(item, num);

        int berryCount = 0; ;
        foreach (InventoryGroup inventoryGroup in GameManagerScript.inventory)
        {
            if (inventoryGroup.InventoryItem == InventoryItem.Health_Berry)
            { berryCount++; }
            if (inventoryGroup.InventoryItem == InventoryItem.Double_Health_Berry)
            { berryCount++; }
            if (inventoryGroup.InventoryItem == InventoryItem.Stamina_Berry)
            { berryCount++; }
            if (inventoryGroup.InventoryItem == InventoryItem.Thirst_Berry)
            { berryCount++; }
            if (inventoryGroup.InventoryItem == InventoryItem.Hunger_Berry)
            { berryCount++; }
            if (inventoryGroup.InventoryItem == InventoryItem.KissOfDeath_Berry)
            { berryCount++; }
            if (inventoryGroup.InventoryItem == InventoryItem.Poison_Berry)
            { berryCount++; }
        }

        if (berryCount >= 4)
        {
            AreBerriesCollected = true;
        }


    }

    private void FixedUpdate()
    {
        if (!CanFixBridge && GameManagerScript.inventory.Any(x => x.InventoryItem == InventoryItem.Hammer))
        {
            CanFixBridge = true;
        }
        if (!ChestsLocated && chestsLocated >= 3)
        {
            ChestsLocated = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Lost")
        {
            FindTheFamilyMember = true;
           
            GameObject.Destroy(other.gameObject,1F);
        }
    }
}
