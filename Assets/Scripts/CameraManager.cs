using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera activeCamera;
    [SerializeField] private CameraSettings cameraSettings;
    public Boundaries cameraBoundaries { get; set; }
    private Vector3? _calculatedMovement;

    public Vector3 CalculatedMovement
    {
        //This is valid C# 8 stuff, but unity does not support ??= yet
        //get => _calculatedMovement ??= Vector3.zero;
        get
        {
            _calculatedMovement = _calculatedMovement ?? Vector3.zero;
            return _calculatedMovement.Value;
        }
        set => _calculatedMovement = value;
    }


    private void Update()
    {
        ApplyCalculatedMovement();
    }

    private void ApplyCalculatedMovement()
    {

        activeCamera.transform.position = cameraBoundaries.ClampToBoundaries(activeCamera.transform.position + CalculatedMovement);
        CalculatedMovement = Vector3.zero;
    }

    public void PanActiveCamera(Vector3 direction)
    {
        float mainCameraMovementSpeed = cameraSettings.MoveSpeed;
        
        var panMovement = new Vector2(
                direction.x * mainCameraMovementSpeed,
                direction.y * mainCameraMovementSpeed);

        CalculatedMovement += new Vector3(-panMovement.x, 0, -panMovement.y);
    }

    public void ZoomActiveCamera(float amount)
    {
        var zoomedAmount = amount * cameraSettings.ZoomSpeed;
        CalculatedMovement += new Vector3(0, -zoomedAmount, zoomedAmount);
    }

    public Vector3 CalculateOffsetFromCameraAngle()
    {
        //TODO Allow for rotation
        var rotationVector = activeCamera.transform.rotation.eulerAngles;
        var angles = new List<float> { rotationVector.x, rotationVector.y, rotationVector.z };

        foreach (var angle in angles.Where(x => x != 0))
        {
            //Hardcoded for now, as camera rotation is not allowed
        }

        return new Vector3(0,0,75 * Mathf.Tan(Mathf.Deg2Rad * rotationVector.x)); 
    }
}
