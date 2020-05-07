using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameManagerScript gameManager = null;
    InventorySlot[] slots;

    #region Singleton

	public static InventoryUI instance;

	void Awake ()
	{
		instance = this;
	}

	#endregion

    void Start() {
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();     
        //InvokeRepeating("UpdateUI", 0, 1);   
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
		{
			if (i < GameManagerScript.inventory.Count())
			{
				slots[i].AddItem(GameManagerScript.inventory[i]);
			} else
			{
				slots[i].ClearSlot();
			}
		}
    }

    void Update() 
    {
        // UpdateUI();

		// for (int i = 0; i < slots.Length; i++)
		// {
		// 	if (i < gameManager.inventory.Count)
		// 	{
        //         Debug.Log(gameManager.inventory.FirstOrDefault(x => x.InventoryItem == InventoryItem.Thirst_Berry);
		// 		slots[i].AddItem(gameManager.inventory.FirstOrDefault(x => x.InventoryItem == InventoryItem.Thirst_Berry));
		// 	} else
		// 	{
		// 		slots[i].ClearSlot();
		// 	}
		// }
    }
}
