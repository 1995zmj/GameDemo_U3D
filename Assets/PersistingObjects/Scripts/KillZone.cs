using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] private float dyingDuration;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var shape = other.GetComponent<Shape>();
        if (shape)
        {
            if (dyingDuration <= 0f) {
                shape.Die();
            }
            else if (!shape.IsMarkedAsDying) {
                shape.AddBehavior<DyingShapeBehavior>().Initialize(
                    shape, dyingDuration
                );
            }
        }
    }
    
    void OnDrawGizmos () {
        Gizmos.color = Color.magenta;
        Gizmos.matrix = transform.localToWorldMatrix;
        var c = GetComponent<Collider>();
        var b = c as BoxCollider;
        if (b != null) {
            Gizmos.DrawWireCube(b.center, b.size);
            return;
        }
        var s = c as SphereCollider;
        if (s != null) {
            Gizmos.DrawWireSphere(s.center, s.radius);
            return;
        }
    }
}
