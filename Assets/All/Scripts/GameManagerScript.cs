using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;

    public float CurrentHealth = 100f;
    public float CurrentStamina = 100f;
    public float CurrentHunger = 100f;
    public float CurrentThirst = 100f;

    private float HungerRate = 1f;
    private float ThirstRate = 1.25f;

    private float DamageTakenFromHunger = 2f;
    private float DamageTakenFromThirst = 1f;

    public static List<InventoryGroup> inventory;

    public HealthBar mHealthBar;
    public HungerBar mHungerBar;
    public ThirstBar mThirstBar;
    public StaminaBar mStaminaBar;

    public InventoryUI mInventoryUI;
    public GameObject infoUI;
    public bool bridgeIsFixed;

    private static GameObject player;

    [SerializeField]
    public Sprite[] icons;

    private void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Awake()
    {
        //MakeSingleton();

        inventory = new List<InventoryGroup>();

        infoUI.SetActive(true);
    }

    void Start()
    {
        SetupPlayer();
        Debug.Log("GameManager Is Starting");
        mHealthBar.SetMaxBarValue(CurrentHealth);
        mThirstBar.SetMaxBarValue(CurrentThirst);
        mHungerBar.SetMaxBarValue(CurrentHunger);

        InvokeRepeating("HungerIncrease", 10, 10f);
        InvokeRepeating("ThirstIncrease", 10, 10f);
    }

    void Update()
    {

    }

    private void SetupPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Player>().Setup(CurrentHealth);
    }



    // Hunger

    private void UpdateCurrentHunger(float newCurrentHunger)
    {
        newCurrentHunger = Mathf.Clamp(newCurrentHunger, 0, 100);
        // Debug.Log("Clamped newCurrentHunger: " + newCurrentHunger);
        CurrentHunger = newCurrentHunger;
        mHungerBar.SetBarValue(CurrentHunger);

        if (CurrentHunger <= 0)
        {
            CurrentHunger = 0;
            TakeDamage(DamageTakenFromHunger);
            // Debug.Log("Character is suffering from hunger");
        }
    }

    public void HungerIncrease()
    {
        float newCurrentHunger = CurrentHunger - HungerRate;

        UpdateCurrentHunger(newCurrentHunger);

        if (CurrentHunger <= 0)
        {
            TakeDamage(DamageTakenFromHunger);
            // Debug.Log("Character is suffering from hunger");
        }
        // Debug.Log("CurrentHungerPoints: " + CurrentHunger);
    }

    public void HungerAdjust(float hungerPoints)
    {
        float newCurrentHunger = CurrentHunger + hungerPoints;
        UpdateCurrentHunger(newCurrentHunger);
    }
    // END hunger

    //Health

    public void UpdateCurrentHealth(float newCurrentHealth)
    {
        newCurrentHealth = Mathf.Clamp(newCurrentHealth, 0, 100);
        // Debug.Log("Clamped newCurrentHealth: " + newCurrentHealth);
        CurrentHealth = newCurrentHealth;

        //Update player script with health
        player.GetComponent<Player>().SetHealth(CurrentHealth);

        mHealthBar.SetBarValue(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Debug.Log("Game Over");
        }
    }

    public void TakeDamage(float damageTaken)
    {
        var newCurrentHealth = CurrentHealth - damageTaken;
        UpdateCurrentHealth(newCurrentHealth);
    }

    public void GainHealth(float healthGain)
    {
        var newCurrentHealth = CurrentHealth + healthGain;
        UpdateCurrentHealth(newCurrentHealth);
    }

    //END health

    //Thirst 

    public void UpdateCurrentThirst(float newCurrentThirst)
    {
        newCurrentThirst = Mathf.Clamp(newCurrentThirst, 0, 100);
        // Debug.Log("Clamped newCurrentThirst: " + newCurrentThirst);
        CurrentThirst = newCurrentThirst;
        mThirstBar.SetBarValue(newCurrentThirst);
    }

    public void ThirstIncrease()
    {
        var newCurrentThirst = CurrentThirst - ThirstRate;

        UpdateCurrentThirst(newCurrentThirst);

        if (CurrentThirst <= 0)
        {
            TakeDamage(DamageTakenFromThirst);
            // Debug.Log("Character is suffering from thirst");
        }
    }

    public void ThirstDecrease(float thirstPoints)
    {
        var newCurrentThirst = CurrentThirst + thirstPoints;
        UpdateCurrentThirst(newCurrentThirst);
    }
    // END thirst

    //Stamina

    public void UpdateCurrentStamina(float newCurrentStamina)
    {
        newCurrentStamina = Mathf.Clamp(newCurrentStamina, 0, 100);
        // Debug.Log("Clamped newCurrentStamina: " + newCurrentStamina);
        CurrentStamina = newCurrentStamina;
        mStaminaBar.SetBarValue(newCurrentStamina);
    }

    public void StaminaAdjust(float staminaPoints)
    {
        float newStamina = CurrentStamina + staminaPoints;
        UpdateCurrentStamina(CurrentStamina);
    }
    //END stamina

    //Inventory Handling

    public void AddItemToInventory(InventoryItem item, int amountOfItems)
    {
        var addList = new List<InventoryGroup>();
        if (inventory.Any(x => x.InventoryItem == item))
        {
            var aItem = inventory.FirstOrDefault(x => x.InventoryItem == item);
            if(amountOfItems < 0 && aItem.Amount > 0 && amountOfItems+aItem.Amount >= 0)
            {
                aItem.Amount += amountOfItems;
            }
            else if (amountOfItems > 0)
            {
                aItem.Amount += amountOfItems;
            }

            if (aItem.Amount == 0)
            {
                inventory.Remove(aItem);
            }
        }
        else
        {
            var newItem = new InventoryGroup
            {
                InventoryItem = item,
                Amount = amountOfItems,
                icon = icons[(int)item]
            };
            inventory.Add(newItem);
        }
        if (inventory.Count == 0)
        {
            var newItem = new InventoryGroup
            {
                InventoryItem = item,
                Amount = amountOfItems,
                icon = icons[(int)item]
            };
            inventory.Add(newItem);
        }
        UpdateInventoryUI();


    }
    //END inventory handling

    //UI Control
    public void UpdateInventoryUI()
    {
        mInventoryUI.UpdateUI();
    }
}

public class InventoryGroup
{
    public InventoryItem InventoryItem { get; set; }
    public int Amount { get; set; }
    public Sprite icon { get; set; }
}

public enum InventoryItem
{
    Health_Berry,
    Hunger_Berry,
    Thirst_Berry,
    Double_Health_Berry,
    Stamina_Berry,
    Poison_Berry,
    KissOfDeath_Berry,
    Hammer,
    Gold
}
