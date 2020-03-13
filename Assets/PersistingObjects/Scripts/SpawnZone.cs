using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

        public IntRange amount;
        [FloatRangeSlider(0.1f, 1f)]
        public FloatRange relativeScale;

        public FloatRange orbitRadius;

        public FloatRange orbitFrequency;
        public bool uniformLifecycles;
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
    
    [System.Serializable]
    public struct LifecycleConfiguration
    {
        [FloatRangeSlider(0f, 2f)]
        public FloatRange growingDuration;

        [FloatRangeSlider(0f, 100f)]
        public FloatRange adultDuration;

        [FloatRangeSlider(0f, 2f)]
        public FloatRange dyingDuration;

        public Vector3 RandomDurations {
            get {
                return new Vector3(
                    growingDuration.RandomValueInRange,
                    adultDuration.RandomValueInRange,
                    dyingDuration.RandomValueInRange
                );
            }
        }
    }

    public LifecycleConfiguration lifecycle;
}


public abstract class SpawnZone : GameLevelObject
{

    [SerializeField]
    SpawnConfiguration spawnConfig;
    [SerializeField, Range(0f, 50f)]
    private float spawnProgress;

    private float spawnSpeed;
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
        shape.gameObject.layer = gameObject.layer;
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
        float growingDuration = spawnConfig.lifecycle.growingDuration.RandomValueInRange;
        Vector3 lifecycleDurations = spawnConfig.lifecycle.RandomDurations;
        int satelliteCount = spawnConfig.satellite.amount.RandomValueInRange;
		for (int i = 0; i < satelliteCount; i++)
        {
            CreateSatelliteFor(shape,
                spawnConfig.satellite.uniformLifecycles ? lifecycleDurations : spawnConfig.lifecycle.RandomDurations);
        }
        SetupLifecycle(shape, lifecycleDurations);
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

    public override void GameUpdate()
    {
        spawnProgress += Time.deltaTime * spawnSpeed;
        while (spawnProgress >= 1f)
        {
            spawnProgress -= 1f;
            SpawnShapes();
        }
    }

    public void SetupLifecycle(Shape shape, Vector3 durations)
    {
        if (durations.x > 0f) {
            if (durations.y > 0f || durations.z > 0f) {
                shape.AddBehavior<LifecycleShapeBehavior>().Initialize(
                    shape, durations.x, durations.y, durations.z
                );
            }
            else {
                shape.AddBehavior<GrowingShapeBehavior>().Initialize(
                    shape, durations.x
                );
            }
        }
        else if (durations.y > 0f) {
            shape.AddBehavior<LifecycleShapeBehavior>().Initialize(
                shape, durations.x, durations.y, durations.z
            );
        }
        else if (durations.z > 0f) {
            shape.AddBehavior<DyingShapeBehavior>().Initialize(
                shape, durations.z
            );
        }
    }

    void CreateSatelliteFor(Shape focalShape, Vector3 lifecycleDurations)
    {
        int factoryIndex = Random.Range(0, spawnConfig.factories.Length);
        Shape shape = spawnConfig.factories[factoryIndex].GetRandom();
        shape.gameObject.layer = gameObject.layer;
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
        SetupLifecycle(shape, lifecycleDurations);
    }

    void SetupColor(Shape shape)
    {
        // test index
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
    
    public override void Save (GameDataWriter writer) {
        writer.Write(spawnProgress);
    }

    public override void Load (GameDataReader reader) {
        spawnProgress = reader.ReadFloat();
    }
}
