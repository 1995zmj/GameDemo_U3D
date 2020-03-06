using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject
{
    [SerializeField]
    List<Shape> perfabs = new List<Shape>();
    [SerializeField]
    List<Material> materials = new List<Material>();

    [SerializeField]
    bool recycle = false;
    Scene poolScene;

    List<Shape>[] pools;

    public int FactoryId
    {
        get
        {
            return factoryId;
        }
        set
        {
            if (factoryId == int.MinValue && value != int.MinValue)
            {
                factoryId = value;
            }
            else
            {
                Debug.Log("Not allowed to change factoryId.");
            }
        }
    }

    [System.NonSerialized]
    int factoryId = int.MinValue;

    public Shape Get(int shapeId = 0, int materialId = 0)
    {
        Shape instance;
        if (recycle)
        {
            if (pools == null)
            {
                CreatePools();
            }
            List<Shape> pool = pools[shapeId];
            int lastIndex = pool.Count - 1;
            if (lastIndex >= 0)
            {
                instance = pool[lastIndex];
                pool.RemoveAt(lastIndex);
                instance.gameObject.SetActive(true);
            }
            else
            {
                instance = Instantiate(perfabs[shapeId]);
                instance.OriginFactory = this;
                instance.ShapeId = shapeId;
                SceneManager.MoveGameObjectToScene(instance.gameObject, poolScene);
            }
        }
        else
        {
            instance = Instantiate(perfabs[shapeId]);
            instance.ShapeId = shapeId;
        }
        instance.SetMaterial(materials[materialId], materialId);
        Game.Instance.AddShape(instance);
        return instance;
    }

    public Shape GetRandom()
    {
        return Get(Random.Range(0, perfabs.Count), Random.Range(0, materials.Count));
    }

    void CreatePools()
    {
        pools = new List<Shape>[perfabs.Count];
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<Shape>();
        }

        if (Application.isEditor)
        {
            poolScene = SceneManager.GetSceneByName(name);
            if (poolScene.isLoaded)
            {
                GameObject[] rootObjects = poolScene.GetRootGameObjects();
                for (int i = 0; i < rootObjects.Length; i++)
                {
                    Shape pooledShape = rootObjects[i].GetComponent<Shape>();
                    if (!pooledShape.gameObject.activeSelf)
                    {
                        pools[pooledShape.ShapeId].Add(pooledShape);
                    }
                }
                return;
            }
        }


        poolScene = SceneManager.CreateScene(name);
    }

    public void Reclaim(Shape shapeToRecycle)
    {
        if (shapeToRecycle.OriginFactory != this)
        {
            Debug.LogError("Tried to reclaim shape with wrong factory.");
            return;
        }
        if (recycle)
        {
            if (pools == null)
            {
                CreatePools();
            }
            pools[shapeToRecycle.ShapeId].Add(shapeToRecycle);
            shapeToRecycle.gameObject.SetActive(false);
        }
        else
        {
            Destroy(shapeToRecycle.gameObject);
        }
    }
}
