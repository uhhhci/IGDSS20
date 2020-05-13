using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private MouseManager mouseManager;


    // Start is called before the first frame update
    void Start()
    {
        //Set boundaries for camera, by map generation
        var lowerBounds = new Vector3(-40, 1, -40);
        var upperBounds = new Vector3(40, 20, 40);
        cameraManager.cameraBoundaries = new Boundaries(lowerBounds, upperBounds);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
