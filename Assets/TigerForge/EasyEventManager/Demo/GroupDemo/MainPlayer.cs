using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class MainPlayer : MonoBehaviour
{
    EventsGroup Listeners = new EventsGroup();

    int coins = 0;

    void Start()
    {

        Listeners.Add("ON_ENEMY_SPAWNED", OnEnemySpawned);
        Listeners.Add("ON_ENEMY_KILLED", OnEnemyKilled);
        Listeners.Add("ON_COIN_TAKEN", OnCoinTaken);

        Listeners.StartListening();

    }

    void OnEnemySpawned()
    {
        var eventData = EventManager.GetDataGroup("ON_ENEMY_SPAWNED");

        if (eventData != null)
        Debug.Log("A new Enemy has been spawned: " + 
            "id " + eventData[0].ToInt() + 
            ", type: " + eventData[1].ToString() + 
            ", health: " + eventData[2].ToInt() + 
            ", strength: " + eventData[3].ToInt() + 
            ", can fly: " + eventData[4].ToBool());
    }

    void OnEnemyKilled()
    {
        var eventData = EventManager.GetIndexedDataGroup("ON_ENEMY_KILLED");

        Debug.Log("You killed an Enemy and earned: " + 
            eventData.ToInt("points") + " points, " + 
            eventData.ToInt("coins") + " gold coins and an " + 
            eventData.ToString("bonus") + "!");
    }

    void OnCoinTaken()
    {
        coins += EventManager.GetInt("ON_COIN_TAKEN");
        Debug.Log("You have taken a new Gold Coin! Now you've got: " + coins + " coins.");

    }

    void OnDestroy()
    {
        Listeners.StopListening();
    }


}
