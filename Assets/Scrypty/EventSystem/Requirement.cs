using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Requirement
{
    
    public enum RequirementType
    {
        Interaction,
        LevelPass,
        EnemyDeathCount
    }

    public RequirementType type;
    public int countOrIndex;
    public GameObject target;

    private bool isCompleted;
    private int count;
    
    // Define a delegate and an event
    public delegate void RequirementCompletedHandler();
    public event RequirementCompletedHandler OnCompleted;

    public Requirement(RequirementType type, int countOrIndex, GameObject target)
    {
        this.type = type;
        this.countOrIndex = countOrIndex;
        this.target = target;
        isCompleted = false;

        OnStart();
    }
    
    public void OnStart()
    {
        switch (type)
        {
            case RequirementType.Interaction:
                EventManager.Objects.OnObjectInteract += Check;
                break;
            case RequirementType.LevelPass:
                EventManager.Levels.OnFinishLevelFilter(target.name).AddListener(Check);
                break;
            case RequirementType.EnemyDeathCount:
                if (target != null)
                {
                    EventManager.Enemies.OnEnemyDeathFilter(target).AddListener(Check);
                }
                else
                {
                    EventManager.Enemies.OnAnyEnemyDeath += Check;
                }
                break;
        }
    }

    public bool IsCompleted()
    {
        return isCompleted;
    }

    private void Check(Component com)
    {
        count++;
        Debug.Log("Count: " + count);

        if (countOrIndex <= count)
        {
            isCompleted = true;
            OnCompleted.Invoke();
        }
    }

    private void Check(GameObject target, int id)
    {
        if (target == this.target && id == countOrIndex)
        {
            isCompleted = true;
            OnCompleted.Invoke();
        }
    }

    private void Check(GameObject target)
    {
        if (target == this.target)
        {
            isCompleted = true;
            OnCompleted.Invoke();
        }
    }
}