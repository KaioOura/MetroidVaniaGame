using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    private static Action[] buffer;
    private static float[] timestamps;
    private static int head;
    private const int capacity = 10;

    public float BufferTime;

    void Awake()
    {
        head = 0;
        buffer = new Action[capacity];
        timestamps = new float[capacity];
    }

    void Update()
    {
        ManageInputs();
    }

    public static void Add(Action action)
    {
        if (head >= capacity - 1)
            head = 0;

        buffer[head] = action;
        timestamps[head] = Time.time;
    }

    public void ManageInputs()
    {
        float currentTime = Time.time;
        for (int i = 0; i < capacity; i++)
        {
            if (currentTime - timestamps[i] > BufferTime)
            {
                buffer[i] = null;  // Invalidate old input
            }

            buffer[i]?.Invoke();
        }
    }
}

public enum InputEnum
{
    None,
    Attack,
    Jump
}
