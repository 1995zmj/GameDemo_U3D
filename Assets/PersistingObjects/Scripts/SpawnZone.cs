using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnZone : PersistableObject
{
    public abstract Vector3 SpawnPoint {get;}
    // [SerializeField]
    // bool surfaceOnly;
    // public Vector3 SpawnPoint
    // {
    //     get
    //     {
    //         // return Random.insideUnitSphere * 5f + transform.position;
    //         return transform.TransformPoint(
    //             surfaceOnly ? Random.onUnitSphere : Random.insideUnitSphere
    //             );
    //     }
    // }
    
    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.cyan;
    //     Gizmos.matrix = transform.localToWorldMatrix;
    //     Gizmos.DrawWireSphere(Vector3.zero, 1f);
    // }
    // // Start is called before the first frame update
    // void Start()
    // {

    // }

    // // Update is called once per frame
    // void Update()
    // {

    // }
}
