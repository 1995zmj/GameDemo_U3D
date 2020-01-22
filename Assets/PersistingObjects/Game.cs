using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Transform prefab;
    public KeyCode createKey = KeyCode.C;
    public KeyCode newGameKey = KeyCode.N;
    public KeyCode saveKey = KeyCode.S;

    private List<Transform> objects;
    // Start is called before the first frame update
    private string savePath;
    private void Awake() {
        objects = new List<Transform>();
        savePath = Path.Combine(Application.persistentDataPath, "saveFile");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(createKey))
        {
            CreateObject();
        }
        else if(Input.GetKeyDown(newGameKey))
        {
            BeginNewGame();
        }else if(Input.GetKeyDown(saveKey))
        {
            Save();
        }
    }

    private void CreateObject()
    {
        Transform t = Instantiate(prefab);
        t.localPosition = Random.insideUnitSphere * 5;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.1f,1f);
        objects.Add(t);
    }

    private void BeginNewGame()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            Destroy(objects[i].gameObject);
        }
        objects.Clear();
    }

    private void Save()
    {
        using (var writer = new BinaryWriter(File.Open(savePath, FileMode.Create)))
        {
            writer.Write(objects.Count);
            for (int i = 0; i < objects.Count; i++)
            {
                Transform t = objects[i];
                writer.Write(t.localPosition.x);
                writer.Write(t.localPosition.y);
                writer.Write(t.localPosition.z);
            }
        }
    }
    
    private void Load()
    {
        using (var reader = new BinaryReader(File.Open(savePath, FileMode.Open)))
        {
            int cout = reader.ReadInt32();
        }
    }
}
