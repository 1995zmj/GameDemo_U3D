using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : GameLevelObject
{
    [SerializeField]
    Vector3 angularVelocity;
    void Start()
    {

    }

    public override void GameUpdate()
    {
        transform.Rotate(angularVelocity * Time.deltaTime);
    }
}
