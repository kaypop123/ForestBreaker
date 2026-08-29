using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    private Transform CameraTransform;
    [SerializeField]
    private float ParallaxEffect = 0.5f;

    private Vector3 lastCameraPosition;
    // Start is called before the first frame update
    void Start()
    {
        lastCameraPosition = CameraTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
       
        Vector3 deltaMovement = CameraTransform.position - lastCameraPosition;

        transform.position -= new Vector3(deltaMovement.x * ParallaxEffect, deltaMovement.y * ParallaxEffect);

        lastCameraPosition = CameraTransform.position;
    }
}