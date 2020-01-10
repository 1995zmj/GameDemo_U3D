using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class EnemyA : MonoBehaviour
{
    void Start()
    {
        EventManager.StartListening("CALL_ALL_PLAYERS", gameObject, WhoIAmCallBack);
    }

    void WhoIAmCallBack()
    {
        Debug.Log("I'm EnemyA, called by the new filter engine!");
    }
}
