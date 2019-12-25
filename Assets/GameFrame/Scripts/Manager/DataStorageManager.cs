using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataStorageManager : MonoBehaviour
{
    const string gameName = "/gamesave.save";
    private static DataStorageManager _instance;
    // Start is called before the first frame update
    public static DataStorageManager getInstance()
    {
        //if(_instance == null)
        //{
        //    _instance = new PoolManager();
        //    Debug.Log("初始化");
        //}
        return _instance;
    }

    private void Awake()
    {
        _instance = this;
        BaseDataInfo info = new BaseDataInfo();
        SaveAsJSON(info);
    }

    public void SaveAsJSON<T> (T info) where T : BaseDataInfo
    {
        string json = JsonUtility.ToJson(info);
        Debug.Log("Saving as JSON: " + json);
    }

    public void SaveAsFile<T>(T info) where T : BaseDataInfo
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + gameName);
        bf.Serialize(file, info);
        file.Close();
    }

    //TODO
    public void LoadByFile<T>(T info)
    {
        if(File.Exists(Application.persistentDataPath + gameName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + gameName, FileMode.Open);
            T tempinfo = (T)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            Debug.Log("没有保存过这个信息");
        }
    }

    public void setPlayerPrefs<T>(string key,string value)
    {
        PlayerPrefs.SetString(key, value);
    }
    public string getPlayerPrefs(string key)
    {
        string tempStr = "";
        if (PlayerPrefs.HasKey(key))
        {
            tempStr = PlayerPrefs.GetString(key);
        }
        else
        {
            Debug.Log("没有保存过");
        }
        return tempStr;
    }

}
