using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour
{
    
    Camera mainCamera;

    // Update is called once per frame
    void Update()
    {
        Camera cam = Camera.main;
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
            cam.transform.rotation * Vector3.up);
    }
}
