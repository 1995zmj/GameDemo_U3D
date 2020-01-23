using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject
{
    [SerializeField]
    List<Shape> perfabs = new List<Shape>();
    [SerializeField]
    List<Material> materials = new List<Material>();
    
    public Shape Get(int shapeId = 0, int materialId = 0)
    {
        Shape instance = Instantiate(perfabs[shapeId]);
        instance.ShapeId = shapeId;
        instance.SetMaterial(materials[materialId], materialId);
        return instance;
    }

    public Shape GetRandom()
    {
        return Get(Random.Range(0,perfabs.Count),Random.Range(0,materials.Count));
    }
}
