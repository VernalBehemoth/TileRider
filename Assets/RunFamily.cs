using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunFamily : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Family.IsRunning == true)
        {
            Run();
            Family.IsRunning = false;
        }
    }

    private void Run()
    {

    }
}
