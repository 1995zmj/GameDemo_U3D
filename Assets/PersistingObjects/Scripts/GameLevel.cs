using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : PersistableObject
{
    [SerializeField]
    SpawnZone spawnZone;

    [SerializeField]
    PersistableObject[] persistableObjects;
    // Start is called before the first frame update
    // void Start()
    // {
    //     Game.Instance.spawnZoneOfLevel = spawnZone;
    // }

    public static GameLevel Current { get; private set; }

    // public Vector3 SpawnPoint
    // {
    //     get
    //     {
    //         return spawnZone.SpawnPoint;
    //     }
    // }
    public Shape SpawnShape()
    {
        return spawnZone.SpawnShape();
    }
    private void OnEnable()
    {
        Current = this;
        if (persistableObjects == null)
        {
            persistableObjects = new PersistableObject[0];
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Save(GameDataWriter writer)
    {
        writer.Write(persistableObjects.Length);
        for (int i = 0; i < persistableObjects.Length; i++)
        {
            persistableObjects[i].Save(writer);
        }
    }

    public override void Load(GameDataReader reader)
    {
        int savedCount = reader.ReadInt();
        for (int i = 0; i < savedCount; i++)
        {
            persistableObjects[i].Load(reader);
        }
    }
}
