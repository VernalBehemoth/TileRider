using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerScript : MonoBehaviour
{
    public HungerBar mHungerBar;  
    public HealthScript mHealthScript;
    public HealthBar mHealthBar;

    public float HungerRate = 2;
    public float CurrentHungerPoints = 100;
    public float MaxHungerPoints = 100;

    void Start() {
        CurrentHungerPoints = MaxHungerPoints;
        mHungerBar.SetMaxBarValue(CurrentHungerPoints);
        InvokeRepeating("HungerIncrease", 0, 5f);
    }

    public void HungerIncrease() {
        CurrentHungerPoints -= HungerRate;

        mHungerBar.SetBarValue(CurrentHungerPoints);
        
        if(CurrentHungerPoints <= 0){
            CurrentHungerPoints = 0;
            DamageTakenFromHunger(5);
            Debug.Log("Character is suffering from hunger");
        }
        Debug.Log("CurrentHungerPoints: " + CurrentHungerPoints);
    }

    public void DamageTakenFromHunger(float damageTaken) {
        mHealthScript.TakeDamage(damageTaken, mHealthBar);
    }

    public void Eat(float hungerPoints) {
        CurrentHungerPoints += hungerPoints;
    }
}
