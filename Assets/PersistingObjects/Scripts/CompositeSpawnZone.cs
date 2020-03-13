using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeSpawnZone : SpawnZone
{
    [SerializeField]
    bool surfaceOnly;
    [SerializeField]
    SpawnZone[] spawnZones;

    [SerializeField]
    bool sequential = true;
    int nextSequentialIndex = 0;

    [SerializeField]
    bool overrideConfig;
    public override Vector3 SpawnPoint
    {
        get
        {
            int index = 0;
            if (sequential)
            {
                index = nextSequentialIndex++;
                if (nextSequentialIndex >= spawnZones.Length)
                {
                    nextSequentialIndex = 0;
                }
            }
            else
            {
                index = Random.Range(0, spawnZones.Length);
            }
            return spawnZones[index].SpawnPoint;
        }
    }

    public override void SpawnShapes ()
    {
        if (overrideConfig)
        {
            // return 
            base.SpawnShapes();
        }
        else
        {
            int index = 0;
            if (sequential)
            {
                index = nextSequentialIndex++;
                if (nextSequentialIndex >= spawnZones.Length)
                {
                    nextSequentialIndex = 0;
                }
            }
            else
            {
                index = Random.Range(0, spawnZones.Length);
            }
            // return 
            spawnZones[index].SpawnShapes();
        }
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.Write(nextSequentialIndex);
    }

    public override void Load(GameDataReader reader)
    {
        if (reader.Version >= 7) {
            base.Load(reader);
        }
        nextSequentialIndex = reader.ReadInt();
    }


}
