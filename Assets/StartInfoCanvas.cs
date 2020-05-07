using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartInfoCanvas : MonoBehaviour
{
    public GameObject menu; 
    private bool isShowing = true;
    public static bool allowGod = false;

    public static string currentTask = "Find my hammer";
    void Start()
    {
        Time.timeScale = 0;
    }
 
    void Update() 
    {
        if (Input.GetKeyDown("tab")) {
            if(isShowing)
            {
                menu.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                menu.SetActive(true);
                Time.timeScale = 0;
                try
                {
                    if (PlayerTasks.currentTask >= 7)
                    {
                        GameObject.FindGameObjectWithTag("GODTOGGLE").GetComponent<Toggle>().interactable = true;

                        if (Player.GodMode)
                            GameObject.FindGameObjectWithTag("GODTOGGLE").GetComponent<Toggle>().isOn = true;
                    }
                    GameObject.FindGameObjectWithTag("CurrentTask").GetComponent<Text>().text = currentTask;
                }
                catch { }
            }
            isShowing = !isShowing;
        }
    }
}
