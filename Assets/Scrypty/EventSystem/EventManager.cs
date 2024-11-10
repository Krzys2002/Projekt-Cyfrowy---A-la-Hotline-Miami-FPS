using System.Collections;
using System.Collections.Generic;
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
        public UnityAction OnPlayerEnterDialogue;
        public UnityAction OnPlayerExitDialogue;
    }
    
    public class ObjectEvents
    {
        public UnityAction<GameObject, int> OnObjectInteract;
    }
    
    public class LevelEvents
    {
        public class OnFinishLevel : UnityEvent<GameObject> { }
        
        private Dictionary<string, OnFinishLevel> mapOnFinishLevel = new Dictionary<string, OnFinishLevel>();
        
        public OnFinishLevel OnFinishLevelFilter(string levelName = "")
        {
            mapOnFinishLevel.TryAdd(levelName, new OnFinishLevel());
            return mapOnFinishLevel[levelName];
        }
    }
    
    public class EnemyEvents
    {
        public UnityAction<Component> OnAnyEnemyDeath;
        
        public class OnEnemyDeath : UnityEvent<Component> { }

        private Dictionary<string, OnEnemyDeath> mapOnEnemyDeath = new Dictionary<string, OnEnemyDeath>();
        
        public OnEnemyDeath OnEnemyDeathFilter(string subLevel)
        {
            mapOnEnemyDeath.TryAdd(subLevel, new OnEnemyDeath());
            return mapOnEnemyDeath[subLevel];
        }
    }
}
