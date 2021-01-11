using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChase : MonoBehaviour
{
    [SerializeField]
    private GameObject pursued;

    [Header("Camera coordinats"), Space(10)]
    public float cameraX = 5f;
    public float cameraY = 5f;
    public float cameraZ = 5f;

    void Update()
    {
        transform.position = new Vector3(pursued.transform.position.x, transform.position.y, pursued.transform.position.z - cameraZ);
    }
}
