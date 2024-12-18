using System;
using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;

public class NpcDialogsMenager : MonoBehaviour
{
    [SerializeField] 
    private List<NpcDialog> Dialogs;

    private NpcDialog inDialog;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (NpcDialog Object in Dialogs)
        {
            Object.Start();
        }

        //ConversationManager.OnConversationEnded += ConversationEnd;
    }

    private void OnEnable()
    {
        ConversationManager.OnConversationEnded += ConversationEnd;
    }

    private void OnDisable()
    {
        ConversationManager.OnConversationEnded -= ConversationEnd;
    }

    void ConversationEnd()
    {
        if(inDialog == null)
        {
            return;
        }

        if (EventManager.Player.OnPlayerExitDialogue != null)
        {
            EventManager.Player.OnPlayerExitDialogue.Invoke(inDialog.GetConversation());
        }

        StoreData.PlayerData.PreviousConversations.Add(inDialog.GetConversation());
        inDialog.Complete();
        inDialog = null;
    }

    public void TryEnterDialog()
    {
        foreach (NpcDialog dialog in Dialogs)
        {
            if (!dialog.isReady())
            {
               continue; 
            }
            
            dialog.startDialog(transform);
            inDialog = dialog;
            if (EventManager.Player.OnPlayerEnterDialogue != null)
            {
                EventManager.Player.OnPlayerEnterDialogue.Invoke(dialog.GetConversation(), transform);
            }
            
            break;
        }
    }
}
