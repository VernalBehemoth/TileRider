using UnityEngine;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour
{
    public Image icon; // May be implemented later
    public Text amountText;
    public Text itemName;
    public InventoryGroup inventoryGroup;
    public Button clickButton;
    private GameManagerScript gameManagerScript;
    private void Start()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
    }
    public void AddItem(InventoryGroup group)
    {
        Debug.Log(GameManagerScript.inventory[0].InventoryItem);
        inventoryGroup = group;
        amountText.text = group.Amount.ToString();
        itemName.text = group.InventoryItem.ToString().Replace('_', ' ');
        bool isDepleted = false;
        if (group.Amount <= 0)
        {
            isDepleted = true;
        }

        foreach (Transform child in transform)
        {
            if (child.name == "ItemButton")
            {
                foreach (Transform grandChild in child)
                {
                    if (grandChild.name == "Icon")
                    {
                        if (isDepleted)
                        {
                            grandChild.GetComponentInChildren<Image>().sprite = group.icon;
                            grandChild.GetComponentInChildren<Image>().enabled = false;
                        }
                        else
                        {
                            grandChild.GetComponentInChildren<Image>().sprite = group.icon;
                            grandChild.GetComponentInChildren<Image>().enabled = true;
                        }
                    }
                }
            }
            else if (child.name == "ItemName")
            {
                if (isDepleted)
                {
                    child.GetComponent<Text>().text = "";
                }
            }
            else if (child.name == "AmountText")
            {
                if (isDepleted)
                {
                    child.GetComponent<Text>().text = "";
                }
            }
        }

        // switch (item)
        // {
        //     Thirst_Berry:
        //         icon = sprite.none;
        //         break;
        //     default:
        //         icon = sprite.none;
        //         break;
        // }
        // icon.sprite = item.icon;
        // icon.enabled = true;
    }
    public void OnclickButton()
    {
        Debug.Log("Pressed a button");
        //gameManagerScript.AddItemToInventory(inventoryGroup.InventoryItem, -1);
    }

    public void ClearSlot()
    {
        inventoryGroup = null;
        amountText.text = "";
        itemName.text = "";
        icon.enabled = false;

        // icon.sprite = null;
        // icon.enabled = false;
    }
}
