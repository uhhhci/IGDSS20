using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * camera rotation = (45, 0, 0) --> CoSy fits
 * 
 * 
 * tasks:  
 * While holding the right mouse button, moving the mouse pans the camera (on the XZ-plane)  x
 * The camera cannot be moved outside the boundaries of the terrain (x)
 * scrolling the mouse wheel zooms the camera in and out 
 * There are limits for the minimum and maximum zoom 
 * Left clicking a tile outputs the type of tile as text on the console (the name of the GameObject is sufficient)
 * 
 * 
 */
public class MouseManager : MonoBehaviour
{
    // the main camera of the scene
    [Tooltip("Just put main camera in 0_O")]
    public GameObject mainCamera;

    // needs to fit so screensize
    [Tooltip("~ sensitivity of mouse. 0.1f should be fine.")]
    public float mainCameraMovementSpeed = 0.1f;

    public float zoomingSpeed = 1;

    // have to come from outside --> GameManager? 
    [Tooltip("Boundaries of terrain: negative x (left), positive x (right), negative z (near), positive z(far)")]
    public Vector4 boundariesOfTerrain = new Vector4 (-40, +40, -50, 30);
   
    // position of mouse at first registation of right click
    private Vector3 lastMousePosition; 



    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        zooming();

        // Camera.main.ScreenToWorldPoint(Input.mousePosition) 
        if (Input.GetMouseButtonDown(1))
        {   
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            panMainCamera();
        }

        // x, y, z = campostion.x, y,z ... 
        // camPosition = (x from panning, y from zooming, z from panning

    }


    /*
     * Method for moving the camera
     * needs to be cleaned up
     * T
     */ 
    private void panMainCamera(){
        Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
        //       mainCamera.transform.position += new Vector3(-mouseDelta.x * mainCameraMovementSpeed, 0, -mouseDelta.y * mainCameraMovementSpeed);
        
        //TODO: clean up, simplify
        mainCamera.transform.position = 
            new Vector3(
                Mathf.Clamp((mainCamera.transform.position.x - (mouseDelta.x * mainCameraMovementSpeed)), boundariesOfTerrain.x, boundariesOfTerrain.y),
                mainCamera.transform.position.y,
                Mathf.Clamp((mainCamera.transform.position.z - (mouseDelta.y * mainCameraMovementSpeed)), boundariesOfTerrain.z, boundariesOfTerrain.w));
        lastMousePosition = Input.mousePosition;
    }


    private float zooming()
    {
        float zoomFactor = 0;
        // y: positive --> up
        // Vector2 wheeled = Input.mouseScrollDelta;

        zoomFactor += Input.mouseScrollDelta.y * zoomingSpeed;


        return zoomFactor;
    }



    
}
