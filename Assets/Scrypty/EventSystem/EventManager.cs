using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;
using UnityEngine.Events;

public class EventManager
{
    public static readonly PlayerEvents Player = new PlayerEvents();
    public static readonly ObjectEvents Objects = new ObjectEvents();
    public static readonly LevelEvents Levels = new LevelEvents();
    public static readonly EnemyEvents Enemies = new EnemyEvents();

    public class PlayerEvents
    {
        public UnityAction<NPCConversation, Transform> OnPlayerEnterDialogue;
        public UnityAction<NPCConversation> OnPlayerExitDialogue;
        public UnityAction<float> OnPlayerHealthChange;
    }
    
    public class ObjectEvents
    {
        public UnityAction<GameObject, int> OnObjectInteract;
    }
    
    public class LevelEvents
    {
        public class OnFinishLevel : UnityEvent<GameObject> { }
        
        // map of on finish level events
        private Dictionary<string, OnFinishLevel> mapOnFinishLevel = new Dictionary<string, OnFinishLevel>();
        
        // filter on finish level event
        public OnFinishLevel OnFinishLevelFilter(string levelName = "")
        {
            mapOnFinishLevel.TryAdd(levelName, new OnFinishLevel());
            return mapOnFinishLevel[levelName];
        }
    }
    
    public class EnemyEvents
    {
        public UnityAction<Component> OnAnyEnemyDeath;
        
        // define a UnityEvent that takes a Component as a parameter
        public class OnEnemyDeath : UnityEvent<Component> { }

        // map of on enemy death events
        private Dictionary<string, OnEnemyDeath> mapOnEnemyDeath = new Dictionary<string, OnEnemyDeath>();
        
        // filter on enemy death event
        public OnEnemyDeath OnEnemyDeathFilter(string subLevel)
        {
            mapOnEnemyDeath.TryAdd(subLevel, new OnEnemyDeath());
            return mapOnEnemyDeath[subLevel];
        }
    }
}
