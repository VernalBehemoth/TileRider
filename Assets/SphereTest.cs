using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Sphere trigger, hit: " + other.transform.tag);
        if (other.transform.tag == "Spear")
        {

            Debug.Log("Spear trigger - hit beast");
            if (this.transform.GetComponent<Beast>() != null)
            {
                this.transform.GetComponent<Beast>().Hit(10);
            }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Sphere trigger, hit: " + other.transform.tag);
        if (other.transform.tag == "Spear")
        {

            Debug.Log("Spear trigger - hit beast");
            if (this.transform.GetComponent<Beast>() != null)
            {
                this.transform.GetComponent<Beast>().Hit(10);
            }
        }
    }
}
