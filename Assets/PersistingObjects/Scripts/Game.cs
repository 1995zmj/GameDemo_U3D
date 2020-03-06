using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Game : PersistableObject
{
    [SerializeField]
    ShapeFactory[] shapeFactories;
    public float CreationSpeed { get; set; }
    public float DestructionSpeed { get; set; }
    public KeyCode createKey = KeyCode.C;
    public KeyCode newGameKey = KeyCode.N;
    public KeyCode saveKey = KeyCode.S;
    public KeyCode loadKey = KeyCode.L;
    public KeyCode destroyKey = KeyCode.X;
    private List<Shape> shapes;
    private float creationProgress;
    private float destroyProgress;

    [SerializeField] Slider creationSpeedSlider;
    [SerializeField] Slider destructionSpeedSlider;

    public PersistentStorage storage;
    // public SpawnZone spawnZone;
    // public SpawnZone spawnZoneOfLevel { get; set; }
    public int levelCount = 2;
    int loadingLevelBuildIndex;
    const int saveVersion = 6;

    Random.State mainRandomState;

    [SerializeField] bool reseedOnLoad;
    public static Game Instance { get; private set; }
    void Start()
    {
        mainRandomState = Random.state;
        shapes = new List<Shape>();
        if (Application.isEditor)
        {
            // Scene loadedLevel = SceneManager.GetSceneByName("Level1");
            // if (loadedLevel.isLoaded)
            // {
            //     SceneManager.SetActiveScene(loadedLevel);
            //     return;
            // }

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene loadedLevel = SceneManager.GetSceneAt(i);
                if (loadedLevel.name.Contains("Level"))
                {
                    SceneManager.SetActiveScene(loadedLevel);
                    loadingLevelBuildIndex = loadedLevel.buildIndex;
                }
            }
        }
        BeginNewGame();
        StartCoroutine(LoadLevel(1));
    }

    private void OnEnable()
    {
        Instance = this;

        if (shapeFactories[0].FactoryId != 0)
        {
            for (int i = 0; i < shapeFactories.Length; i++)
            {
                shapeFactories[i].FactoryId = i;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(createKey))
        {
            // CreateShape();
            GameLevel.Current.SpawnShapes();
        }
        else if (Input.GetKeyDown(newGameKey))
        {
            BeginNewGame();
            StartCoroutine(LoadLevel(loadingLevelBuildIndex));
        }
        else if (Input.GetKeyDown(saveKey))
        {
            storage.Save(this, saveVersion);
        }
        else if (Input.GetKeyDown(loadKey))
        {
            BeginNewGame();
            storage.Load(this);
        }
        else if (Input.GetKeyDown(destroyKey))
        {
            DestroyShape();
        }
        else
        {
            for (int i = 0; i <= levelCount; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    BeginNewGame();
                    StartCoroutine(LoadLevel(i));
                    return;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        creationProgress += Time.deltaTime * CreationSpeed;
        while (creationProgress >= 1f)
        {
            creationProgress -= 1f;
            // CreateShape();
            GameLevel.Current.SpawnShapes();
        }


        destroyProgress += Time.deltaTime * DestructionSpeed;
        while (destroyProgress >= 1f)
        {
            destroyProgress -= 1f;
            DestroyShape();
        }

        for (int i = 0; i < shapes.Count; i++)
        {
            shapes[i].GameUpdate();
        }
    }

    // private void CreateShape()
    // {
    //     shapes.Add(GameLevel.Current.SpawnShape());
    // }

    public void AddShape(Shape shape)
    {
        shapes.Add(shape);
    }

    IEnumerator LoadLevel(int levelBuildIndex)
    {
        enabled = false;
        if (loadingLevelBuildIndex > 0)
        {
            yield return SceneManager.UnloadSceneAsync(loadingLevelBuildIndex);
        }
        yield return SceneManager.LoadSceneAsync(levelBuildIndex, LoadSceneMode.Additive); ;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelBuildIndex));
        loadingLevelBuildIndex = levelBuildIndex;
        enabled = true;
    }
    private void BeginNewGame()
    {
        Random.state = mainRandomState;
        int seed = Random.Range(0, int.MaxValue) ^ (int)Time.unscaledTime;
        mainRandomState = Random.state;
        Random.InitState(seed);
        creationSpeedSlider.value = CreationSpeed = 0;
        destructionSpeedSlider.value = DestructionSpeed = 0;
        for (int i = 0; i < shapes.Count; i++)
        {
            // Destroy(shapes[i].gameObject);
            // shapeFactory.Reclaim(shapes[i]);
            shapes[i].Recycle();
        }
        shapes.Clear();
    }

    public override void Save(GameDataWriter writer)
    {
        writer.Write(shapes.Count);
        writer.Write(Random.state);
        writer.Write(CreationSpeed);
        writer.Write(creationProgress);
        writer.Write(DestructionSpeed);
        writer.Write(destroyProgress);
        writer.Write(loadingLevelBuildIndex);
        GameLevel.Current.Save(writer);
        for (int i = 0; i < shapes.Count; i++)
        {
            writer.Write(shapes[i].OriginFactory.FactoryId);
            writer.Write(shapes[i].ShapeId);
            writer.Write(shapes[i].MaterialId);
            shapes[i].Save(writer);
        }
    }

    public override void Load(GameDataReader reader)
    {
        int version = reader.Version;
        Debug.Log(version);
        if (version > saveVersion)
        {
            Debug.LogError("Unsupported future save version " + version);
            return;
        }
        StartCoroutine(LoadGame(reader));
    }

    IEnumerator LoadGame(GameDataReader reader)
    {
        int version = reader.Version;
        int count = version <= 0 ? -version : reader.ReadInt();
        if (version >= 3)
        {
            Random.State state = reader.ReadRandomState();
            if (!reseedOnLoad)
            {
                Random.state = state;
            }
            creationSpeedSlider.value = CreationSpeed = reader.ReadFloat();
            creationProgress = reader.ReadFloat();
            destructionSpeedSlider.value = DestructionSpeed = reader.ReadFloat();
            destroyProgress = reader.ReadFloat();
        }
        // StartCoroutine(LoadLevel(version < 2 ? 1 : reader.ReadInt()));
        yield return LoadLevel(version < 2 ? 1 : reader.ReadInt());
        if (version >= 3)
        {
            GameLevel.Current.Load(reader);
        }
        for (int i = 0; i < count; i++)
        {
            int factoryId = version >= 5 ? reader.ReadInt() : 0;
            int ShapeId = version > 0 ? reader.ReadInt() : 0;
            int MaterialId = version > 0 ? reader.ReadInt() : 0;
            Shape instance = shapeFactories[factoryId].Get(ShapeId, MaterialId);
            instance.Load(reader);
            // shapes.Add(instance);
        }
    }

    public void DestroyShape()
    {
        if (shapes.Count > 0)
        {
            int index = Random.Range(0, shapes.Count);
            // Destroy(shapes[index].gameObject);
            // shapeFactory.Reclaim(shapes[index]);
            shapes[index].Recycle();
            int lastIndex = shapes.Count - 1;
            shapes[index] = shapes[lastIndex];
            shapes.RemoveAt(lastIndex);
        }

    }

}
