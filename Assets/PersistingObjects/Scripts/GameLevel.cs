using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : PersistableObject
{
    [SerializeField]
    private SpawnZone spawnZone;
    [SerializeField]
    int populationLimit;
    public int PopulationLimit {
		get {
			return populationLimit;
		}
	}
    [UnityEngine.Serialization.FormerlySerializedAs("persistentObjects")]
    [SerializeField]
    GameLevelObject[] levelObjects;
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
    public void SpawnShapes()
    {
        // return 
        spawnZone.SpawnShapes();
    }
    private void OnEnable()
    {
        Current = this;
        if (levelObjects == null)
        {
            levelObjects = new GameLevelObject[0];
        }
    }
    
    public void GameUpdate () {
        for (int i = 0; i < levelObjects.Length; i++) {
            levelObjects[i].GameUpdate();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Save(GameDataWriter writer)
    {
        writer.Write(levelObjects.Length);
        for (int i = 0; i < levelObjects.Length; i++)
        {
            levelObjects[i].Save(writer);
        }
    }

    public override void Load(GameDataReader reader)
    {
        int savedCount = reader.ReadInt();
        for (int i = 0; i < savedCount; i++)
        {
            levelObjects[i].Load(reader);
        }
    }
    
    public bool HasMissingLevelObjects {
        get {
            if (levelObjects != null) {
                for (int i = 0; i < levelObjects.Length; i++) {
                    if (levelObjects[i] == null) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
