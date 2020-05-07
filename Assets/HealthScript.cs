using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float CurrentHealth = 100;
    //public HealthBar mHealthBar;

    public void TakeDamage(float damageTaken, HealthBar healthBar){
        CurrentHealth -= damageTaken;
        healthBar.SetMaxBarValue(100); 
        healthBar.SetBarValue(CurrentHealth); 
        Debug.Log("DamageTaken: " + damageTaken + " CurrentHealth: " + CurrentHealth);

        if(CurrentHealth <= 0){
            Debug.Log("Game Over");
        }
    }
}
