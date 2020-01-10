using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class MyEvent : UnityEvent<int> { }
public class Test : MonoBehaviour
{
    public UnityEvent myEvent;
    private Event k;

    void Start()
    {
    }

    void Update()
    {
    }

    public void MyFunction(int i)
    {
        print(i);
    }
    public void MyFunction2(int i)
    {
        print(i * 2);
    }

}
