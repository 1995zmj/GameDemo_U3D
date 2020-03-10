using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShapeInstance
{
    public Shape Shape { get; private set; }
    int instanceIdOrSaveIndex;
    public ShapeInstance(Shape shape)
    {
        Shape = shape;
        instanceIdOrSaveIndex = shape.InstanceId;
    }
    public ShapeInstance(int saveIndex)
    {
        Shape = null;
        instanceIdOrSaveIndex = saveIndex;
    }

    public void Resolve () {
		if (instanceIdOrSaveIndex >= 0) {
			Shape = Game.Instance.GetShape(instanceIdOrSaveIndex);
			instanceIdOrSaveIndex = Shape.InstanceId;
		}
	}

    public static implicit operator ShapeInstance(Shape shape)
    {
        return new ShapeInstance(shape);
    }
    public bool IsValid
    {
        get
        {
            return Shape && instanceIdOrSaveIndex == Shape.InstanceId;
        }
    }

}

public class Shape : PersistableObject
{
    static int colorPropertyId = Shader.PropertyToID("_Color");
    static MaterialPropertyBlock sharedPropertyBlock;
    // private MeshRenderer meshRenderer;
    Color[] colors;
    private int shapeId = int.MinValue;

    public float Age { get; private set; }
    public int InstanceId { get; private set; }
    public int SaveIndex { get; set; }

    [SerializeField]
    MeshRenderer[] meshRenderers;

    List<ShapeBehavior> behaviorList = new List<ShapeBehavior>();

    public T AddBehavior<T>() where T : ShapeBehavior, new()
    {
        T behavior = ShapeBehaviorPool<T>.Get();
        behaviorList.Add(behavior);
        return behavior;
    }

    public ShapeFactory OriginFactory
    {
        get
        {
            return originFactory;
        }
        set
        {
            if (originFactory == null)
            {
                originFactory = value;
            }
            else
            {
                Debug.LogError("Not allowed to change origin factory.");
            }
        }
    }

    ShapeFactory originFactory;
    public int ShapeId
    {
        get
        {
            return shapeId;
        }
        set
        {
            if (shapeId == int.MinValue && value != int.MinValue)
            {
                shapeId = value;
            }
            else
            {
                Debug.LogError("Not allowed to change shapeId.");
            }
        }

    }

    public int MaterialId
    {
        get;
        private set;
    }


    public void SetMaterial(Material material, int materialId)
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material = material;
        }
        MaterialId = materialId;
    }
    public int ColorCount
    {
        get
        {
            return colors.Length;
        }
    }
    public void SetColor(Color color)
    {
        // this.color = color;
        if (sharedPropertyBlock == null)
        {
            sharedPropertyBlock = new MaterialPropertyBlock();
        }
        sharedPropertyBlock.SetColor(colorPropertyId, color);
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            colors[i] = color;
            meshRenderers[i].SetPropertyBlock(sharedPropertyBlock);
        }
    }

    public void SetColor(Color color, int index)
    {
        if (sharedPropertyBlock == null)
        {
            sharedPropertyBlock = new MaterialPropertyBlock();
        }
        sharedPropertyBlock.SetColor(colorPropertyId, color);
        colors[index] = color;
        meshRenderers[index].SetPropertyBlock(sharedPropertyBlock);
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.Write(colors.Length);
        for (int i = 0; i < colors.Length; i++)
        {
            writer.Write(colors[i]);
        }
        // writer.Write(AngularVelocity);
        // writer.Write(Velocity);
        writer.Write(Age);
        writer.Write(behaviorList.Count);
        for (int i = 0; i < behaviorList.Count; i++)
        {
            writer.Write((int)behaviorList[i].BehaviorType);
            behaviorList[i].Save(writer);
        }
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        if (reader.Version >= 5)
        {
            LoadColors(reader);
        }
        else
        {
            SetColor(reader.Version > 0 ? reader.ReadColor() : Color.white);
        }
        // AngularVelocity = reader.Version >= 4 ? reader.ReadVector3() : Vector3.zero;
        // Velocity = reader.Version >= 4 ? reader.ReadVector3() : Vector3.zero;
        if (reader.Version >= 6)
        {
            Age = reader.ReadFloat();
            int behaviorCount = reader.ReadInt();
            for (int i = 0; i < behaviorCount; i++)
            {
                // AddBehavior((ShapeBehaviorType)reader.ReadInt()).Load(reader);
                ShapeBehavior behavior = ((ShapeBehaviorType)reader.ReadInt()).GetInstance();
                behaviorList.Add(behavior);
                behavior.Load(reader);
            }
        }
        else
        {
            AddBehavior<RotationShapeBehavior>().AngularVelocity =
                reader.ReadVector3();
            AddBehavior<MovementShapeBehavior>().Velocity = reader.ReadVector3();
        }
    }

    void LoadColors(GameDataReader reader)
    {
        int count = reader.ReadInt();
        int max = count <= colors.Length ? count : colors.Length;
        int i = 0;
        for (; i < max; i++)
        {
            SetColor(reader.ReadColor(), i);
        }

        if (count > colors.Length)
        {
            for (; i < count; i++)
            {
                reader.ReadColor();
            }
        }
        else if (count < colors.Length)
        {
            for (; i < colors.Length; i++)
            {
                SetColor(Color.white, i);
            }
        }
    }

    // ShapeBehavior AddBehavior(ShapeBehaviorType type)
    // {
    //     switch (type)
    //     {
    //         case ShapeBehaviorType.Movement:
    //             return AddBehavior<MovementShapeBehavior>();
    //         case ShapeBehaviorType.Rotation:
    //             return AddBehavior<RotationShapeBehavior>();
    //     }
    //     Debug.LogError("Forgot to support " + type);
    //     return null;
    // }

    void Awake()
    {
        colors = new Color[meshRenderers.Length];
    }

    public void GameUpdate()
    {
        // transform.Rotate(AngularVelocity * Time.deltaTime);
        // transform.localPosition += Velocity * Time.deltaTime;
        Age += Time.deltaTime;
        for (int i = 0; i < behaviorList.Count; i++)
        {
            if (!behaviorList[i].GameUpdate(this))
            {
                behaviorList[i].Recycle();
                behaviorList.RemoveAt(i--);
            }
        }
    }
    public void Recycle()
    {
        Age = 0;
        InstanceId += 1;
        for (int i = 0; i < behaviorList.Count; i++)
        {
            // Destroy(behaviorList[i]);
            behaviorList[i].Recycle();
        }
        behaviorList.Clear();
        OriginFactory.Reclaim(this);
    }

    public void ResolveShapeInstances () {
		for (int i = 0; i < behaviorList.Count; i++) {
			behaviorList[i].ResolveShapeInstances();
		}
	}

    public void Die()
    {
        Game.Instance.Kill(this);
    }
}
