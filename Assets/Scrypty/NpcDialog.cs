using System;
using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class NpcDialog
{
    [SerializeField] 
    private NPCConversation dialog;

    [SerializeField] 
    private List<Requirement> requirements;

    private bool readyToInteract;

    public void Start()
    {
        // Start all requirements
        foreach (var requirement in requirements)
        {
            // Start coroutine for each requirement
            requirement.OnStart();
            requirement.OnCompleted += CheckRequirements; 
        }

        if (requirements.Count < 1)
        {
            readyToInteract = true;
        }
    }

    public void startDialog(Transform npcTransform)
    {
        ConversationManager.Instance.StartConversation(dialog);
        EventManager.Player.OnPlayerEnterDialogue.Invoke(dialog, npcTransform);
    }

    public bool isReady()
    {
        return readyToInteract;
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

        readyToInteract = true;
    }

    public NPCConversation GetConversation()
    {
        return dialog;
    }
}
