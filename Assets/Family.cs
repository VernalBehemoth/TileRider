using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Family : MonoBehaviour
{
    [SerializeField]
    public List<FamilyMember> familyMembers = new List<FamilyMember>();

    public static bool IsRunning;

    private InfoMessage infoMessage;


    private void Start()
    {
        infoMessage = GameObject.FindGameObjectWithTag("InfoCanvas").GetComponent<InfoMessage>();
        foreach (FamilyMember fm in familyMembers)
        {
            fm.CurrentSatisfactionLevel = fm.StartingSatisfaction;
        }
        StartCoroutine(DeteriorateSatisfaction());
    }

    private IEnumerator DeteriorateSatisfaction()
    {
        for (; ; )
        {
            foreach (FamilyMember familyMember in familyMembers)
            {
                familyMember.CurrentSatisfactionLevel -= familyMember.Deterioration;
                float satisfactionPercentage = familyMember.CurrentSatisfactionLevel / familyMember.StartingSatisfaction * 100;
                if (satisfactionPercentage <= 75)
                {
                    Message("OH NO! " + familyMember + " is starting to get sassy, better prove your worth.");
                }
                else if (satisfactionPercentage <= 50)
                {
                    Message("OH NO! " + familyMember + " is having a breaking down! Better do something about it!");
                }
                else if (satisfactionPercentage <= 20)
                {
                    RunAway();
                }
                yield return new WaitForSeconds(3F);
            }
        }
    }

    internal void GiveToMember(int amount, int memberNum, InventoryItem inventoryItem)
    {
        InventoryGroup inventoryGroup = GameManagerScript.inventory.FirstOrDefault(x => x.InventoryItem == inventoryItem);
        float amountToAdd = ((20 * familyMembers[memberNum].Deterioration) * amount);

        if (familyMembers[memberNum].CurrentSatisfactionLevel + amountToAdd >= familyMembers[memberNum].StartingSatisfaction)
        {
            familyMembers[memberNum].CurrentSatisfactionLevel = familyMembers[memberNum].StartingSatisfaction;
        }
        else if (GameManagerScript.inventory.FirstOrDefault(x => x.InventoryItem == inventoryItem).Amount + amount >= 0)
        {
            familyMembers[memberNum].CurrentSatisfactionLevel += amountToAdd;
        }
    }

    private void Message(string v)
    {
        infoMessage.DisplayMessage(v, 2F, false);
    }

    private void RunAway()
    {
        IsRunning = true;
        GameObject[] lostFamilyMembers = GameObject.FindGameObjectsWithTag("Lost");

        Message("OH NO! " + lostFamilyMembers[Random.Range(0, lostFamilyMembers.Length - 1)].name + " has run away, you must find him!");
        lostFamilyMembers[Random.Range(0, lostFamilyMembers.Length - 1)].SetActive(true);
    }
}
[System.Serializable]
public class FamilyMember
{
    public string Name;
    public float StartingSatisfaction;
    public float CurrentSatisfactionLevel;
    public float Deterioration;
}
