using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAfterSeconds : MonoBehaviour
{
    public float AliveSeconds = 1.0f;

    private float StartTime;

    private void Awake()
    {
        StartTime = Time.time;
    }

    void Update()
    {
        if (Time.time > StartTime + AliveSeconds)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
