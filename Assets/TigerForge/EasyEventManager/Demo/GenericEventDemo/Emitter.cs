using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class Emitter : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Welcome in TigerForge - Easy Event Manager, version 2.2!");
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("I'm the Emitter. I called PlayerA and I set his age to 24 yo.");
            EventManager.SetData("I_AM_PLAYER_A", 24);
            EventManager.EmitEvent("I_AM_PLAYER_A");
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            Debug.Log("I'm the Emitter. I'll call the Players after 2 seconds, but only the Players with 'Player' tag will reply.");
            EventManager.EmitEvent("I_AM_PLAYER", "tag:Player", 2);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            Debug.Log("I'm the Emitter. I'll call the Players placed in the 'Water' Layer. Moreover, I'll tell them who I am.");
            EventManager.EmitEvent("I_AM_PLAYER", "layer:4", 0, gameObject);
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            if (!EventManager.EventExists("PLAYER_A_ONCE_EVENT")) {
                Debug.Log("Player A has removed this event!");
                return;
            }
            Debug.Log("I'm the Emitter. I'm going to call Player A with EmitEvenData method (passing 'STEVEN' string data). Player A will stop the listener without specifying the CallBack (new feature).");
            EventManager.EmitEventData("PLAYER_A_ONCE_EVENT", "STEVEN");
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            Debug.Log("I'm the Emitter. I'll call all the Players with name containing 'emy' string and with tag starting with 'Enemy', in the 'Default' layer.");
            EventManager.EmitEvent("CALL_ALL_PLAYERS", "name:*emy*;tag:Enemy*;layer:0");
        }
    }
}
