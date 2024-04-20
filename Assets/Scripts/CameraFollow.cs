using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerPos;

    Vector3 camOffset;

    void Start()
    {
        camOffset = transform.position - playerPos.position;
    }

    private void FixedUpdate()
    {
        transform.position = playerPos.position + camOffset;
    }
}
