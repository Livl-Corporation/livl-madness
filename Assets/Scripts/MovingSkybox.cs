using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSkybox : MonoBehaviour
{
    [SerializeField] private float skyboxRotationSpeed = 0.8f;

    private void MoveSkybox()
    {
        var skybox = RenderSettings.skybox;
        var rotation = skybox.GetFloat("_Rotation");
        skybox.SetFloat("_Rotation", rotation + skyboxRotationSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        MoveSkybox();
        Debug.Log("Skybox is moving");
    }
}
