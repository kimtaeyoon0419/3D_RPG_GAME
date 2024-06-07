// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{

    [Header("Component")]
    private CharacterController characterController;

    [Header("Move")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Vector3 moveDirection;

    private bool isRun;

    private KeyCode jumpKeyCode = KeyCode.Space;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        _MoveInput();
    }

    private void Update()
    {
        _JumpInput();
    }

    private void _MoveInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (x != 0 || z != 0)
        {
            isRun = true;
        }
        else
        {
            isRun = false;
        }

        _MoveTo(new Vector3(x, 0, z));
    }

    private void _JumpInput()
    {
        if (Input.GetKeyDown(jumpKeyCode))
        {
            _JumpTo();
        }
        if (characterController.isGrounded == false)
        {
            _Gravity();
        }
    }

    private void _MoveTo(Vector3 direction)
    {
        moveDirection = new Vector3(direction.x, moveDirection.y, direction.z);
        characterController.Move(moveDirection * speed * Time.deltaTime);

        if (isRun)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z)), Time.deltaTime * turnSpeed);
        }
    }

    private void _JumpTo()
    {
        if (characterController.isGrounded == true)
        {
            moveDirection.y = jumpForce;
        }
    }

    private void _Gravity()
    {
            moveDirection.y += gravity * Time.deltaTime;
    }
}
