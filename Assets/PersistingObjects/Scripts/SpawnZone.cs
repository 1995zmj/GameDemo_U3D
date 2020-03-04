using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnZone : PersistableObject
{
    [System.Serializable]
    public struct SpawnConfiguration
    {
        public enum MovementDirection
        {
            Forward,
            Upward,
            Outward,
            Random
        }
        public ShapeFactory[] factories;
        public MovementDirection movementDirection;
        public FloatRange speed;
        public FloatRange angularSpeed;
        public FloatRange scale;
        public ColorRangeHSV color;
        public bool uniformColor;
    }
    [SerializeField]
    SpawnConfiguration spawnConfig;
    public abstract Vector3 SpawnPoint { get; }
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

    // public virtual void ConfigureSpawn(Shape shape)
    public virtual Shape SpawnShape()
    {
        int factoryIndex = Random.Range(0, spawnConfig.factories.Length);
        Shape shape = spawnConfig.factories[factoryIndex].GetRandom();
        Transform t = shape.transform;
        t.localPosition = SpawnPoint;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * spawnConfig.scale.RandomValueInRange;

        // shape.SetColor(Random.ColorHSV(
        //     hueMin: 0f, hueMax: 1f,
        //     saturationMin: 0.5f, saturationMax: 1f,
        //     valueMin: 0.25f, valueMax: 1f,
        //     alphaMin: 1f, alphaMax: 1f
        // ));
        if (spawnConfig.uniformColor)
        {
            shape.SetColor(spawnConfig.color.RandomInRange);
        }
        else
        {
            for (int i = 0; i < shape.ColorCount; i++)
            {
                shape.SetColor(spawnConfig.color.RandomInRange, i);
            }
        }
        shape.AngularVelocity = Random.onUnitSphere * spawnConfig.angularSpeed.RandomValueInRange;
        Vector3 direction;
        switch (spawnConfig.movementDirection)
        {
            case SpawnConfiguration.MovementDirection.Upward:
                direction = transform.up;
                break;
            case SpawnConfiguration.MovementDirection.Outward:
                direction = (t.localPosition - transform.position).normalized;
                break;
            case SpawnConfiguration.MovementDirection.Random:
                direction = Random.onUnitSphere;
                break;
            default:
                direction = transform.forward;
                break;
        }

        shape.Velocity = transform.forward * spawnConfig.speed.RandomValueInRange;
        return shape;
    }
}
