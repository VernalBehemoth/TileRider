using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTasks : MonoBehaviour
{
    //Collect the hammer

    List<PlayerTask> playerTasks = new List<PlayerTask>();
    Interactor interactor;
    InfoMessage infoMessage;
    public static int currentTask = -1;
    Text pausedTxt;
    private void Awake()
    {
    }
    void Start()
    {
        
        infoMessage = GameObject.FindGameObjectWithTag("InfoCanvas").GetComponent<InfoMessage>();
        interactor = GameObject.FindGameObjectWithTag("Player").GetComponent<Interactor>();
        playerTasks.Add(new PlayerTask { TaskOrder = 0, TaskName = "'Find Hammer'", Message = "Have a look for my hammer, i shall fix the bridge!" });
        playerTasks.Add(new PlayerTask { TaskOrder = 1, TaskName = "'Hammer Time'", Message = "Now lets put this hammer to use!" });
        playerTasks.Add(new PlayerTask { TaskOrder = 2, TaskName = "'Halle Berrys'", Message = "Now collect berries for your family, they require 4 types" });
        playerTasks.Add(new PlayerTask { TaskOrder = 3, TaskName = "'Drew Berry More'", Message = "Now go home and give your family many berries 50+" });
        playerTasks.Add(new PlayerTask { TaskOrder = 4, TaskName = "'She ain't nothing but a..'", Message = "Now find at least 3 chests (there may be more)" });
        playerTasks.Add(new PlayerTask { TaskOrder = 5, TaskName = "'Give gold'", Message = "Now go home and give your family some GOLD!" });
        playerTasks.Add(new PlayerTask { TaskOrder = 6, TaskName = "'O well'", Message = "Now one of your family has got lost, you will loose them foreaver if you dont find them" });
        playerTasks.Add(new PlayerTask { TaskOrder = 7, TaskName = "'WIN'", Message = "ALL TASKS COMPLETE!! Go beat up some livestock to celebrate!" });
        if (GameObject.FindGameObjectWithTag("PAUSED") != null)
        {
            pausedTxt = GameObject.FindGameObjectWithTag("PAUSED").GetComponent<Text>();
            pausedTxt.enabled = false;
        }
        StartCoroutine(NotifyAboutTasks());
    }

    IEnumerator NotifyAboutTasks()
    {
        for (; ; )
        {

            if (currentTask == -1)
                currentTask = 0;
            if (currentTask == 0 && interactor.CanFixBridge)
                currentTask = 1;
            if (currentTask == 1 && Interactor.BridgeFixed)
                currentTask = 2;
            if (currentTask == 2 && interactor.AreBerriesCollected)
                currentTask = 3;
            if (currentTask == 3 && Interactor.ProvidedForFamilyBerries)
                currentTask = 4;
            if (currentTask == 4 && interactor.ChestsLocated)
                currentTask = 5;
            if (currentTask == 5 && Interactor.ProvidedForFamilyGold)
            {
                try
                {
                    GameObject go = GameObject.FindGameObjectsWithTag("Lost").FirstOrDefault(x => x.name == "LostFamily");

                    foreach (Transform t in go.transform)
                        t.gameObject.SetActive(true);
                }
                catch
                {
                    Interactor.FindTheFamilyMember = true;
                }


                currentTask = 6;

            }
            if (currentTask == 6 && Interactor.FindTheFamilyMember)
            {
                currentTask = 7;
                infoMessage.DisplayMessage("Current Task: "+playerTasks[currentTask].TaskName + " : " + playerTasks[currentTask].Message, 200F, true);

                try
                {
                    GameObject animalContainer = GameObject.FindGameObjectWithTag("AnimalContainer");
                    Vector3 pos = animalContainer.transform.position;
                    pos.x += 0.06F;
                    Instantiate(animalContainer, pos, animalContainer.transform.rotation);

                    StartInfoCanvas.allowGod = true;
                    Player.GodMode = true;
                    infoMessage.DisplayMessage(@"YOU FOUND ME! LOVE YOU!!! HAVE GOD MODE AS A GIFT (hit 'TAB' to enable)", 45F, true);
                    StartCoroutine(SlowTime());
                }
                catch(Exception ex)
                {
                    infoMessage.DisplayMessage("FAILED TO CREATED ALL THE ENEMIES", 10F, true);
                    Debug.Log(ex.Message);
                }
                infoMessage.SetIsFinished(true);
                break;
            }
            else
            {
                infoMessage.DisplayMessage("Current Task: "+playerTasks[currentTask].TaskName + " : " + playerTasks[currentTask].Message, 5F, false);
                yield return new WaitForSeconds(2F);
            }
            StartInfoCanvas.currentTask = playerTasks[currentTask].TaskName + " : " + playerTasks[currentTask].Message;
        }
    }

    IEnumerator SlowTime()
    {

        pausedTxt.enabled = true;
        Time.timeScale = 0;

        yield return new WaitForSeconds(3F);

        pausedTxt.enabled = false;

        yield return new WaitForSeconds(0.1F);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

internal class PlayerTask
{
    public int TaskOrder { get; set; }
    public string TaskName { get; set; }
    public string Message { get; set; }
    public bool IsComplete { get; set; }
    public int Progress { get; set; }
}