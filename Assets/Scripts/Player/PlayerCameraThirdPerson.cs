using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraThirdPerson : MonoBehaviour
{
    public bool LockCursor = true;

    public float MouseSensitivity = 10;
    public float DstFromTarget = 2;
    public Vector2 PitchMinMax = new Vector2(-89, 89);
    public Transform Target;

    private float yaw;
    private float pitch;

    private Vector3 currentRotation;

    void Start()
    {
        if (LockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void LateUpdate()
    {

        yaw += Input.GetAxis("Mouse X") * MouseSensitivity;
        pitch += Input.GetAxis("Mouse Y") * MouseSensitivity;
        pitch = Mathf.Clamp(pitch, PitchMinMax.x, PitchMinMax.y);

        currentRotation = new Vector3(pitch, yaw);
        //transform.eulerAngles = currentRotation;
        Target.eulerAngles = currentRotation;
        transform.LookAt(Target);

        transform.position = Target.position - Target.forward * DstFromTarget;
    }
}
