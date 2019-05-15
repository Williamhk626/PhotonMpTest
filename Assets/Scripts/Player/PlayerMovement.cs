using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float walkingSpeed = 2;
    [SerializeField]
    private float runningSpeed = 10;
    public float Gravity = -12;
    public float JumpHeight = 1;
    public float TurnSmoothTime = 0.06f;
    public float MovementSpeedMultiplier = 1;
    [SerializeField]
    private float slopeForce = 5;
    [SerializeField]
    private float slopeForceRayLenght = 1.7f;


    private float turnSmoothVelocity;
    private float velocityY;
    private float currentSpeed;
    private Vector2 movementInput;
    private bool isRunning;
    private bool isJumping;

    public bool FaceForward { get; set; } = false;

    //public InputActions inputControls;

    public GameObject BulletPrefab;
    public Transform BulletSpawnPoint;
    public Camera Camera;
    public Transform targetTransform;
    private CharacterController controller;
    private PhotonView PV;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        PV = GetComponent<PhotonView>();
        if (!PV.IsMine)
        {
            Camera.gameObject.SetActive(false);
            //DisableInput();
        }
        else
        {
            //    inputControls.Player.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
            //    inputControls.Player.Run.performed += _ => isRunning = true;
            //    inputControls.Player.Jump.performed += _ => Jump();

            //    EnableInput();
        }
}

    #region Useless unity movement stuff
    //void OnEnable()
    //{
    //    if (PV != null)
    //        if (!PV.IsMine)
    //            return;

    //    // Enable input controls
    //    inputControls.Player.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
    //    inputControls.Player.Run.performed += _ => isRunning = true;
    //    inputControls.Player.Jump.performed += _ => Jump();

    //    EnableInput();
    //}
    //void OnDisable()
    //{
    //    // Disable input controls
    //    if (PV != null)
    //        if (!PV.IsMine)
    //            return;

    //    inputControls.Player.Movement.performed -= ctx => movementInput = ctx.ReadValue<Vector2>();
    //    inputControls.Player.Run.performed -= _ => isRunning = true;
    //    inputControls.Player.Jump.performed -= _ => Jump();
    //    DisableInput();
    //}

    //void EnableInput()
    //{
    //    inputControls.Player.Movement.Enable();
    //    inputControls.Player.Run.Enable();
    //    inputControls.Player.Jump.Enable();
    //}

    //void DisableInput()
    //{
    //    inputControls.Player.Movement.Disable();
    //    inputControls.Player.Run.Disable();
    //    inputControls.Player.Jump.Disable();
    //}
    #endregion

    void Update()
    {
        if (!PV.IsMine)
            return;

        if (Input.GetButton("Fire1"))
        {
            FaceForward = true;

            PV.RPC("Fire", RpcTarget.AllViaServer, transform.rotation);
        }
        else
        {
            FaceForward = false;
        }

        if (Input.GetButton("Run"))
            isRunning = true;

        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetButton("Jump"))
            Jump();

        Move();
    }

    void Move()
    {
        velocityY += Time.deltaTime * Gravity;

        Vector3 velocity;

        if (FaceForward)
        {

            transform.eulerAngles = new Vector3(0, targetTransform.eulerAngles.y, 0);

            velocity = (transform.forward * movementInput.y) + (transform.right * movementInput.x) + Vector3.up * velocityY;

            velocity = isRunning ? velocity * (runningSpeed * MovementSpeedMultiplier) : velocity * (walkingSpeed * MovementSpeedMultiplier);

            velocity.y = velocityY;
            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            if (movementInput != Vector2.zero)
            {
                float targetRotation = Mathf.Atan2(movementInput.x, movementInput.y) * Mathf.Rad2Deg + targetTransform.eulerAngles.y;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, TurnSmoothTime);
            }

            float targetspeed = (isRunning ? (runningSpeed * MovementSpeedMultiplier) : (walkingSpeed * MovementSpeedMultiplier)) * movementInput.magnitude;
            currentSpeed = targetspeed;

            velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
            controller.Move(velocity * Time.deltaTime);
        }

        if ((movementInput.x != 0 || movementInput.y != 0) && OnSlope())
            controller.Move(Vector3.down * controller.height / 2 * slopeForce * Time.deltaTime);


        if (controller.isGrounded)
        {
            isJumping = false;
            velocityY = 0;
        }

        // Reset input
        movementInput = Vector2.zero;
        isRunning = false;
    }

    bool OnSlope()
    {
        if (isJumping)
            return false;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, controller.height / 2 * slopeForceRayLenght))
            if (hit.normal != Vector3.up)
                return true;

        return false;
    }

    void Jump()
    {
        if (controller.isGrounded)
        {
            isJumping = true;
            velocityY = Mathf.Sqrt(-2 * Gravity * JumpHeight);
        }
    }

    [PunRPC]
    public void Fire(Quaternion rotation)
    {
        GameObject bullet;
        bullet = Instantiate(BulletPrefab, BulletSpawnPoint.position, BulletSpawnPoint.rotation) as GameObject;
        bullet.GetComponent<Bullet>().InitializeBullet(PV.Owner, rotation * Vector3.forward);
    }
}
