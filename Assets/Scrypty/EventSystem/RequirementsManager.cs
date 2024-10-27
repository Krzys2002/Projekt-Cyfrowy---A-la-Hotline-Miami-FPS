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
        foreach (var requirement in requirements)
        {
            requirement.OnStart();
            requirement.OnCompleted += CheckRequirements; 
        }
    }
    
    public void CheckRequirements()
    {
        foreach (var requirement in requirements)
        {
            if (!requirement.IsCompleted())
            {
                return;
            }
        }
        onRequirementsCompleted.Invoke();
    }
}
