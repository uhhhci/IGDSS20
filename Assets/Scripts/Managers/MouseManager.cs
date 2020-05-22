using Assets.Scripts;
using Assets.Scripts.Enums;
using Assets.Scripts.SerializableEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for mouse movement including panning,zooming and clicking
/// </summary>
public class MouseManager : MonoBehaviour
{
    [SerializeField] private MouseSettings Settings;
    public Vector3Event LeftClick;
    public Vector3Event RightClickDrag;
    public FloatEvent MouseScroll;

    private Vector3 lastMousePosition = Vector3.zero;
    private bool draggingRight;

    void Update()
    {
        if (Input.GetMouseButtonDown((int) MouseButton.LEFT))
        {
            LeftClick.Invoke(Input.mousePosition);
        }

        if (Input.GetMouseButtonDown((int) MouseButton.RIGHT))
        {
            draggingRight = true;
        }


        if (Input.GetMouseButtonUp((int) MouseButton.RIGHT))
        {
            draggingRight = false;
        }

        if (draggingRight)
        {
            var mouseMovement = GetMouseMovementSinceLastFrame();
            RightClickDrag.Invoke(mouseMovement);
        }

        //Use only the y component as the x component is not set by Unity.
        var mouseWheelDelta = GetMouseWheelRotationDifferenceSinceLastFrame();
        if (Mathf.Abs(mouseWheelDelta) > Settings.ScrollThreshold)
        {
            MouseScroll.Invoke(mouseWheelDelta);
        }

        lastMousePosition = Input.mousePosition;
    }

    private Vector3 GetMouseMovementSinceLastFrame()
    {
        return Input.mousePosition - lastMousePosition;
    }

    private float GetMouseWheelRotationDifferenceSinceLastFrame()
    {
        return Input.mouseScrollDelta.y * Time.deltaTime;
    }

    public GameObject FindClickedObject(Vector3 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
            return hit.transform.gameObject;

        return null;
    }

    public void PrintClickedObject(Vector3 mousePosition)
    {
        var clickedObject = FindClickedObject(mousePosition);
        if (clickedObject is null)
            return;

        print($"Clicked object [ Name: {clickedObject.name} , Type: {clickedObject.GetType().Name}]");
    }
}
