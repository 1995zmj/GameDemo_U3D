using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{

    public Transform pointPrefab;
    public Transform[] points;

    [Range(10, 100)] public int resolution = 10;
    public GraphFunctionName function;
    private List<GraphFunction> functions;
    private void Awake()
    {
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        Vector3 position = Vector3.zero;
        points = new Transform[resolution * resolution];
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
            }
            Transform point = Instantiate(pointPrefab);
            position.x = (x + 0.5f) * step - 1f;
            position.z = (z + 0.5f) * step - 1f;
            // position.y = position.x * position.x * position.x;
            point.localPosition = position;
            point.localScale = scale;
            point.SetParent(transform, false);

            points[i] = point;
        }


        #region 函数代理
        functions = new List<GraphFunction>();
        functions.Add(SineFunction);
        functions.Add(MultiSineFunction);
        #endregion
    }
    void Start()
    {

    }
    void Update()
    {
        float t = Time.time;
        GraphFunction f = functions[(int)function];
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = points[i];
            Vector3 position = point.localPosition;

            position.y = f(position.x, position.z, t);
            point.localPosition = position;
        }
    }

    public float SineFunction(float x, float z, float t)
    {
        return Mathf.Sin(Mathf.PI * (x + t));
    }

    public float MultiSineFunction(float x, float z, float t)
    {
        float y = Mathf.Sin(Mathf.PI * (x + t));
        y += Mathf.Sin(2f * Mathf.PI * (x + 2f * t)) / 2f;
        y *= 2f / 3f;
        return y;
    }
}
