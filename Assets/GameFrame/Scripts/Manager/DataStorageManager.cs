using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataStorageManager : Singleton<DataStorageManager>
{
    const string gameName = "/gamesave.save";

    public void SaveAsJSON<T>(T info) where T : BaseDataInfo
    {
        string key = info.GetTag();
        string json = JsonUtility.ToJson(info);
        Debug.Log(key);
        Debug.Log("Saving as JSON: " + json);
        SetPlayerPrefs(key, json);
    }

    public void LoadByJSON<T>(ref T info) where T : BaseDataInfo
    {
        string json = GetPlayerPrefs(info.GetTag());
        info = JsonUtility.FromJson<T>(json);
        Debug.Log("Load by JSON: " + json);
    }

    public void SetPlayerPrefs(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public string GetPlayerPrefs(string key)
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
        if (File.Exists(Application.persistentDataPath + gameName))
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

}
