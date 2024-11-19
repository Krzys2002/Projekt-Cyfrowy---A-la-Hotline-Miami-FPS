using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class RequirementsManager : MonoBehaviour
{
    [SerializeField] 
    private List<Requirement> requirements;
    [SerializeField]
    private UnityEvent onRequirementsCompleted;
    
    
    // Start is called before the first frame update
    void Start()
    {
        // Start all requirements
        foreach (var requirement in requirements)
        {
            // Start coroutine for each requirement
            StartCoroutine(requirement.OnStart());
            requirement.OnCompleted += CheckRequirements; 
        }
    }
    
    public void CheckRequirements()
    {
        // Check if all requirements are completed
        foreach (var requirement in requirements)
        {
            if (!requirement.IsCompleted())
            {
                return;
            }
        }
        
        // if so invoke onRequirementsCompleted event
        onRequirementsCompleted.Invoke();
    }
}
