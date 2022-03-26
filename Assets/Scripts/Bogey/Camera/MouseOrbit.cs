using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOrbit : MonoBehaviour
{

    public Transform PlayerTransform;
    [SerializeField] private Vector3 cameraOffset;
    [Range(0.01f, 1.0f)] public float SmoothFactor;

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position - PlayerTransform.position;
    }

    // After the Update Method
    void LateUpdate()
    {
        Vector3 newPosition = PlayerTransform.position + cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPosition, SmoothFactor);
    }
}
