using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager2 : MonoBehaviour
{
    /*
     *  This implements an orbit camera.
     *  So in our Unity Project we take the "Rendering" component as Parent, 
     *  which is moved arround on the x-z plane and the camera follows.
     */
    public Transform cameraParentTransform;
    public Transform cameraTransform;

    public Vector2 panLimit1;
    public Vector2 panLimit2;
    public float zoomLimitMinY;
    public float zoomLimitMaxY;

    public float normalSpeed;
    public float fastSpeed;
    public float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;

    Vector3 newPosition;
    Quaternion newRotation;
    Vector3 newZoom;

    Vector3 dragStartPosition;
    Vector3 dragCurrentPosition;
    Vector3 rotateStartPosition;
    Vector3 rotateCurrentPosition;


    // Start is called before the first frame update
    void Start()
    {
        newPosition = cameraParentTransform.position;
        newRotation = cameraParentTransform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }
        if (Input.GetMouseButtonDown(1))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(1))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = cameraParentTransform.position + dragStartPosition - dragCurrentPosition;

            }
        }
        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;

        }
        if (Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }
    }
    void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (cameraParentTransform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (cameraParentTransform.forward * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (cameraParentTransform.right * movementSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (cameraParentTransform.right * -movementSpeed);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }
        if (Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;
        }
        if (Input.GetKey(KeyCode.T))
        {
            newZoom -= zoomAmount;
        }

        // camera parent movement:
        // lock camera parent to boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, panLimit1.x, panLimit2.x);
        newPosition.z = Mathf.Clamp(newPosition.z, panLimit1.y, panLimit2.y);
        // move and rotate camera parent
        cameraParentTransform.position = Vector3.Lerp(cameraParentTransform.position, newPosition, Time.deltaTime * movementTime);
        cameraParentTransform.rotation = Quaternion.Lerp(cameraParentTransform.rotation, newRotation, Time.deltaTime * movementTime);


        // lock zoom to boundaries
        if ((newZoom.y < zoomLimitMinY) || (newZoom.y > zoomLimitMaxY))
        {
            newZoom = cameraTransform.localPosition;
        }
        // move camera for zoom
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);

    }
}
