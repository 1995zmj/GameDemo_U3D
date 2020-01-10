//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PoolItem: MonoBehaviour
//{
//    private GameObject _prefab;
//    private Queue<GameObject> _queue;
//    public void init(GameObject prefab, int size = 0)
//    {
//        _prefab = prefab;
//        _queue = new Queue<GameObject>();
//        for(int i = 0; i < size; ++i)
//        {
//            GameObject gameObject = Instantiate(prefab, transform);
//            gameObject.SetActive(false);
//            _queue.Enqueue(gameObject);
//        }
//    }

//    public GameObject get(Transform parent)
//    {
//        GameObject gameObject;
//        if(_queue.Count > 0)
//        {
//            gameObject = _queue.Dequeue();
//            gameObject.transform.SetParent(parent);
//        }
//        else
//        {
//            gameObject = Instantiate(_prefab, parent);
//        }
//        gameObject.SetActive(false);
//        return gameObject;
//    }

//    public void put(GameObject gameObject)
//    {
//        gameObject.SetActive(false);
//        _queue.Enqueue(gameObject);
//    }

//}


//public class PoolManager : MonoBehaviour
//{
//    private static PoolManager _instance;
//    private Dictionary<string, PoolItem> _dictionary;
//    // Start is called before the first frame update
//    public static PoolManager getInstance()
//    {
//        //if(_instance == null)
//        //{
//        //    _instance = new PoolManager();
//        //    Debug.Log("初始化");
//        //}
//        return _instance;
//    }

//    private void Awake()
//    {
//        _instance = this;
//        _dictionary = new Dictionary<string, PoolItem>();
//    }

//    public void createPool(GameObject prefab, int count)
//    {
//        GameObject node = new GameObject();
//        PoolItem poolItem = node.AddComponent<PoolItem>();
//        _dictionary[prefab.tag] = poolItem;
//        poolItem.init(prefab, count);
//        node.name = prefab.tag;
//        node.transform.SetParent(transform, false);
//    }

//    public GameObject spawn(string tag,Transform parent)
//    {
//        PoolItem poolItem = _dictionary[tag];
//        GameObject gameObject = poolItem.get(parent);
//        return gameObject;
//    }
//}
