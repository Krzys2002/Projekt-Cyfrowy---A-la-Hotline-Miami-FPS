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
    private bool repeatable = false;
    [SerializeField] 
    private RequirementsControler requirementsControler;
    

    private bool RequirementsCompleted = false;
    private bool wasCompletedBefore = false;

    public void Start()
    {
        requirementsControler.onRequirementsCompleted.AddListener(RequirementsCompletedEvent);
        requirementsControler.Start();
        
        if(StoreData.Player.PreviousConversations.Contains(dialog))
        {
            wasCompletedBefore = true;
        }
    }

    public void startDialog(Transform npcTransform)
    {
        ConversationManager.Instance.StartConversation(dialog);
        EventManager.Player.OnPlayerEnterDialogue.Invoke(dialog, npcTransform);
    }
    
    public void RequirementsCompletedEvent()
    {
        RequirementsCompleted = true;
    }

    public bool isReady()
    {
        if(repeatable && RequirementsCompleted)
        {
            return true;
        }

        if (!wasCompletedBefore && RequirementsCompleted)
        {
            return true;
        }
        
        return false;
    }

    public NPCConversation GetConversation()
    {
        return dialog;
    }
    
    public void Complete()
    {
        wasCompletedBefore = true;
    }
}
