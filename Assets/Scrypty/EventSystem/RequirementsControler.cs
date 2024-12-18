using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class RequirementsControler
{
    [SerializeField] 
    private List<Requirement> requirements;
    [SerializeField]
    public UnityEvent onRequirementsCompleted;
    
    
    // Start is called before the first frame update
    public void Start()
    {
        // Start all requirements
        foreach (var requirement in requirements)
        {
            // Start coroutine for each requirement
            requirement.OnStart();
            requirement.OnCompleted += CheckRequirements; 
        }
        
        if(requirements.Count == 0)
        {
            onRequirementsCompleted.Invoke();
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
        if (onRequirementsCompleted != null)
        {
            Debug.Log("All requirements completed");
            onRequirementsCompleted.Invoke();
        }
    }
}
