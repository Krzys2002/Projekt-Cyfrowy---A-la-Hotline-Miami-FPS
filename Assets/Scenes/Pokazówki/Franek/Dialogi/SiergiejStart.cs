using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class SiergiejStart : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                ConversationManager.Instance.StartConversation(myConversation);
            }
        }
    }
}
