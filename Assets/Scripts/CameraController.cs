using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Move Around")]
    public Transform target;

    [Header("Zoom")]
    public float cameraOffset;
    public float maxZoomOffset;
    public float zoomSpeed;

    private float currentCameraOffset;

    [Header("Rotation")]
    public float minUpAngle;
    public float maxUpAngle;
    public float rotateSpeed;

    private float xRotation;
    private float yRotation;


    private CameraControls cameraControls;

    private void Awake(){
        cameraControls = new CameraControls();
        cameraControls.Enable();

        currentCameraOffset = cameraOffset;
        xRotation = transform.eulerAngles.x;
        yRotation = transform.eulerAngles.y;
        Move(Vector2.zero);
    }

    private void Update(){
        if(cameraControls.Camera.Move.IsPressed()){
            Move(cameraControls.Camera.Move.ReadValue<Vector2>());
        }

        Vector2 zoomDirection = Mouse.current.scroll.ReadValue();
        if(zoomDirection != Vector2.zero){
            Zoom(zoomDirection);
        }
    }

    private void Move(Vector2 direction){
        transform.position = target.position;

        Vector2 newDirection = new Vector2(direction.y, -direction.x);
        Vector3 rotation = newDirection * newDirection.magnitude * rotateSpeed * Time.deltaTime;
        yRotation += rotation.y;
        xRotation = Mathf.Clamp(xRotation + rotation.x, minUpAngle, maxUpAngle);

        Quaternion targetQuaternion = Quaternion.Euler(xRotation, yRotation, transform.eulerAngles.z);
        transform.rotation = targetQuaternion;
        
        transform.Translate(new Vector3(0, 0, -currentCameraOffset), Space.Self);
    }

    private void Zoom(Vector2 direction){
        direction.y = Mathf.Clamp(direction.y, -1, 1);

        Vector3 zoom = new Vector3(0, 0, direction.y) * zoomSpeed * Time.deltaTime;
        transform.Translate(zoom, Space.Self);

        float distance = Vector3.Distance(transform.position, target.position);
        if(distance < maxZoomOffset || distance > cameraOffset){
            Move(Vector2.zero);
        }else{
            currentCameraOffset = distance;
        }
    }
}
