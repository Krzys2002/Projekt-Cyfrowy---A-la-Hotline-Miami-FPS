using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;

public static class StoreData
{
    public static class PlayerData
    {
        public static List<NPCConversation> PreviousConversations = new List<NPCConversation>();
    }
    
    public static class EnemyData
    {
        public static bool useObtacie;
    }
    
    public static class LevelData
    {
        public static List<string> ClearedLevels = new List<string>();
        public static string LastClearedLevel;
        public static Vector3 RespawnPoint = Vector3.zero;
    }
}
