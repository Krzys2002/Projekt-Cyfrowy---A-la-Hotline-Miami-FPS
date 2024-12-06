using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;

public class StoreData
{
    public static readonly PlayerData Player = new PlayerData();
    public static readonly EnemyData Enemy = new EnemyData();
    
    public class PlayerData
    {
        public List<NPCConversation> PreviousConversations = new List<NPCConversation>();
    }
    
    public class EnemyData
    {
        public bool useObtacie;
    }
}
