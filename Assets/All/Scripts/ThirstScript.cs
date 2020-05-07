using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirstScript : MonoBehaviour
{
    public ThirstBar mtThirstBar;  
    public HealthScript mHealthScript;
    public HealthBar mHealthBar;  

    public float ThirstRate = 2;
    public float DamageTakenFromThirstValue = 2;
    public float CurrentThirstPoints = 100;
    public float MaxThirstPoints = 100;

    void Start() {
        CurrentThirstPoints = MaxThirstPoints;
        mtThirstBar.SetMaxBarValue(CurrentThirstPoints);
        InvokeRepeating("ThirstIncrease", 0, 5f);
    }

    public void ThirstIncrease() {
        CurrentThirstPoints -= ThirstRate;

        mtThirstBar.SetBarValue(CurrentThirstPoints);        

        if(CurrentThirstPoints <= 0){
            CurrentThirstPoints = 0;
            DamageTakenFromThirst(DamageTakenFromThirstValue);
            Debug.Log("Character is suffering from thirst");
        }
        Debug.Log("CurrentThirstPoints: " + CurrentThirstPoints);
    }

    public void DamageTakenFromThirst(float damageTaken) {
        mHealthScript.TakeDamage(damageTaken, mHealthBar);
    }

    public void Drink(float hungerPoints) {
        CurrentThirstPoints += hungerPoints;
    }
}
