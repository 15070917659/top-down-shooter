using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;
    private CharacterController characterController;
    private Animator animator;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSpeed;
    private float speed;
    [SerializeField] private float gravity = 9.81f;

    private Vector3 moveDirection;

    public Vector2 moveInput {  get; private set; }
    private float verticalVelocity;

    private bool isRunning;
    

    private void Start()
    {
        player = GetComponent<Player>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        speed = walkSpeed;

        AssignInputEvents();
    }
    private void Update()
    {
        Move();
        ApplyRotation();
        AnimatorControllers();
    }
   
    /// <summary>
    /// 动画控制
    /// </summary>
    private void AnimatorControllers()
    {
        float xVelocity = Vector3.Dot(moveDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(moveDirection.normalized, transform.forward);
        animator.SetFloat("xVelocity", xVelocity, .1f, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, .1f, Time.deltaTime);

        bool playRunAnimation = isRunning && moveDirection.magnitude > 0;
        animator.SetBool("isRunning", playRunAnimation);
    }

    /// <summary>
    /// 随着鼠标瞄准
    /// </summary>
    private void ApplyRotation()
    {
        Vector3 lookingDirection = player.aim.GetMousePosition() - transform.position;
        lookingDirection.y = 0f;
        lookingDirection.Normalize();
        Quaternion desiredRotation = Quaternion.LookRotation(lookingDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, turnSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 移动
    /// </summary>
    private void Move()
    {
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        ApplyGravity();
        if (moveDirection.magnitude > 0)
        {
            characterController.Move(moveDirection * Time.deltaTime * speed);
        }
    }
    /// <summary>
    /// 重力
    /// </summary>
    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            verticalVelocity = verticalVelocity - gravity * Time.deltaTime;
            moveDirection.y = verticalVelocity;
        }
        else
            verticalVelocity = -.5f;
    }


    /// <summary>
    /// 输入事件
    /// </summary>
    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Player.MoveMent.performed += context => moveInput = context.ReadValue<Vector2>();
        controls.Player.MoveMent.canceled += context => moveInput = Vector2.zero;


        controls.Player.Run.performed += context =>
        {
            isRunning = true;
            speed = runSpeed;

        };
        controls.Player.Run.canceled += context =>
        {
            isRunning = false;
            speed = walkSpeed;
        };
    }
}
