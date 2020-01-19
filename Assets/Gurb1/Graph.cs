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
    const float pi = Mathf.PI;
    private void Awake()
    {
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        points = new Transform[resolution * resolution];

        for (int i = 0; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab);
            point.localScale = scale;
            point.SetParent(transform, false);
            points[i] = point;
        }
        #region 函数代理
        functions = new List<GraphFunction>();
        functions.Add(SineFunction);
        functions.Add(Sine2DFunction);
        functions.Add(MultiSineFunction);
        functions.Add(MultiSine2DFunction);
        functions.Add(Ripple);
        functions.Add(Cylinder);
        functions.Add(Sphere);
        functions.Add(Torus);
        #endregion
    }
    void Start()
    {

    }
    void Update()
    {
        float t = Time.time;
        GraphFunction f = functions[(int)function];

        float step = 2f / resolution;
        for (int i = 0, z = 0; z < resolution; z++)
        {
            float v = (z + 0.5f) * step - 1f;
            for (int x = 0; x < resolution; x++, i++)
            {
                float u = (x + 0.5f) * step - 1f;
                points[i].localPosition = f(u, v, t);
            }
        }
    }

    public Vector3 SineFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(Mathf.PI * (x + t));
        p.z = z;
        return p;
    }

    public Vector3 Sine2DFunction(float x, float z, float t)
    {
        float y = Mathf.Sin(Mathf.PI * (x + t));
        y += Mathf.Sin(Mathf.PI * (z + t));
        y *= 0.5f;

        Vector3 p;
        p.x = x;
        p.y = y;
        p.z = z;
        return p;
    }

    public Vector3 MultiSineFunction(float x, float z, float t)
    {
        float y = Mathf.Sin(Mathf.PI * (x + t));
        y += Mathf.Sin(2f * Mathf.PI * (x + 2f * t)) / 2f;
        y *= 2f / 3f;

        Vector3 p;
        p.x = x;
        p.y = y;
        p.z = z;
        return p;
    }

    public Vector3 MultiSine2DFunction(float x, float z, float t)
    {
        float y = 4f * Mathf.Sin(Mathf.PI * (x + z + t * 0.5f));
        y += Mathf.Sin(Mathf.PI * (x + t));
        y += Mathf.Sin(2f * Mathf.PI * (z + 2f * t)) / 0.5f;
        y *= 1f / 5.5f;

        Vector3 p;
        p.x = x;
        p.y = y;
        p.z = z;
        return p;
    }

    public Vector3 Ripple(float x, float z, float t)
    {
        float d = Mathf.Sqrt(x * x + z * z);
        float y = Mathf.Sin(Mathf.PI * (4f * d - t));
        y /= 1f + 10f * d;

        Vector3 p;
        p.x = x;
        p.y = y;
        p.z = z;
        return p;
    }

    public Vector3 Cylinder(float u, float v, float t)
    {
        Vector3 p;
        float r = 0.8f + Mathf.Sin(pi * (6f * u + 2f * v + t) ) * 0.2f;
        p.x = r * Mathf.Sin(Mathf.PI * u);
        p.y = v;
        p.z = r * Mathf.Cos(Mathf.PI *u);
        return p;
    }

    public Vector3 Sphere(float u, float v, float t)
    {
        Vector3 p;
        float r = 0.8f + Mathf.Sin(Mathf.PI * (6f * u + t)) * 0.1f;
        r += Mathf.Sin(Mathf.PI * (4f * v + t)) * 0.1f;
        float s = r * Mathf.Cos(Mathf.PI * 0.5f * v);
        p.x = s * Mathf.Sin(Mathf.PI * u);
        p.y = r * Mathf.Sin(Mathf.PI * 0.5f * v);
        p.z = s * Mathf.Cos(Mathf.PI *u);
        return p;
    }

    public Vector3 Torus(float u, float v, float t)
    {
        Vector3 p;
        float r1 = 0.65f + Mathf.Sin(Mathf.PI * (6f * u + t)) * 0.1f;
        float r2 = 0.2f + Mathf.Sin(Mathf.PI * (4f * v + t)) * 0.05f;
        float s = r2 * Mathf.Cos(Mathf.PI * v) + r1;
        p.x = s * Mathf.Sin(Mathf.PI * u);
        p.y = r2 *Mathf.Sin(Mathf.PI * v);
        p.z = s * Mathf.Cos(Mathf.PI *u);
        return p;
    }
}
