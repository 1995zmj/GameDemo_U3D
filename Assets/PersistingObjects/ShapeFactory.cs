using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject
{
    [SerializeField]
    List<Shape> perfabs = new List<Shape>();
    
    public Shape Get(int shapeId)
    {
        return Instantiate(perfabs[shapeId]);
    }

    public Shape GetRandom()
    {
        return Get(Random.Range(0,perfabs.Count));
    }
}
