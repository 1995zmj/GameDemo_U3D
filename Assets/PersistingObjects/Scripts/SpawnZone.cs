using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [System.Serializable]
    public struct SatelliteConfiguration
    {

        [FloatRangeSlider(0.1f, 1f)]
        public FloatRange relativeScale;

        public FloatRange orbitRadius;

        public FloatRange orbitFrequency;
    }

    public SatelliteConfiguration satellite;
    public ShapeFactory[] factories;
    public MovementDirection movementDirection;
    public FloatRange speed;
    public FloatRange angularSpeed;
    public FloatRange scale;
    public ColorRangeHSV color;
    public bool uniformColor;

    public MovementDirection oscillationDirection;

    public FloatRange oscillationAmplitude;

    public FloatRange oscillationFrequency;
}


public abstract class SpawnZone : PersistableObject
{

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
    public virtual void SpawnShapes()
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
        // if (spawnConfig.uniformColor)
        // {
        //     shape.SetColor(spawnConfig.color.RandomInRange);
        // }
        // else
        // {
        //     for (int i = 0; i < shape.ColorCount; i++)
        //     {
        //         shape.SetColor(spawnConfig.color.RandomInRange, i);
        //     }
        // }

        SetupColor(shape);
        float angularSpeed = spawnConfig.angularSpeed.RandomValueInRange;
        if (angularSpeed != 0f)
        {
            var rotation = shape.AddBehavior<RotationShapeBehavior>();
            rotation.AngularVelocity = Random.onUnitSphere * angularSpeed;
        }

        float speed = spawnConfig.speed.RandomValueInRange;
        if (speed != 0f)
        {

            var movement = shape.AddBehavior<MovementShapeBehavior>();
            movement.Velocity = GetDirectionVector(spawnConfig.movementDirection, t) * speed;
        }

        SetupOscillation(shape);
        CreateSatelliteFor(shape);
        // return shape;
    }
    void SetupOscillation(Shape shape)
    {
        float amplitude = spawnConfig.oscillationAmplitude.RandomValueInRange;
        float frequency = spawnConfig.oscillationFrequency.RandomValueInRange;
        if (amplitude == 0f || frequency == 0f)
        {
            return;
        }
        var oscillation = shape.AddBehavior<OscillationShapeBehavior>();
        oscillation.Offset = GetDirectionVector(
            spawnConfig.oscillationDirection, shape.transform
        ) * amplitude;
        oscillation.Frequency = frequency;
    }
    Vector3 GetDirectionVector(
        SpawnConfiguration.MovementDirection direction, Transform t
    )
    {
        switch (direction)
        {
            case SpawnConfiguration.MovementDirection.Upward:
                return transform.up;
            case SpawnConfiguration.MovementDirection.Outward:
                return (t.localPosition - transform.position).normalized;
            case SpawnConfiguration.MovementDirection.Random:
                return Random.onUnitSphere;
            default:
                return transform.forward;
        }
    }

    void CreateSatelliteFor(Shape focalShape)
    {
        int factoryIndex = Random.Range(0, spawnConfig.factories.Length);
        Shape shape = spawnConfig.factories[factoryIndex].GetRandom();
        Transform t = shape.transform;
        t.localRotation = Random.rotation;
        t.localScale = focalShape.transform.localScale *
        spawnConfig.satellite.relativeScale.RandomValueInRange;
        // t.localPosition = focalShape.transform.localPosition + Vector3.up;
        // shape.AddBehavior<MovementShapeBehavior>().Velocity = Vector3.up;
        SetupColor(shape);
        shape.AddBehavior<SatelliteShapeBehavior>().Initialize(
            shape, focalShape,
            spawnConfig.satellite.orbitRadius.RandomValueInRange,
            spawnConfig.satellite.orbitFrequency.RandomValueInRange
        );
    }

    void SetupColor(Shape shape)
    {
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
    }
}
