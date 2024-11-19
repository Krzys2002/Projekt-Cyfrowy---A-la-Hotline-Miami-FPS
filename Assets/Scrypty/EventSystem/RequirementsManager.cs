using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class RequirementsManager : MonoBehaviour
{
    public RequirementsControler RequirementsControler;
    
    // Start is called before the first frame update
    void Start()
    {
        RequirementsControler.Start();
    }
}
