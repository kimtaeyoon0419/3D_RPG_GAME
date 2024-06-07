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
    private Animator animator;

    [Header("Move")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Vector3 moveDirection;

    [Header("Camera")]
    [SerializeField] private Camera mainCam;

    [Header("Animation")]
    private readonly int hashIsRun = Animator.StringToHash("IsRun");

    private bool isRun;

    private KeyCode jumpKeyCode = KeyCode.Space;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //_MoveInput();
        _Move();
    }

    private void Update()
    {
        _JumpInput();
    }


    private void _Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMove = moveInput.magnitude != 0;
        animator.SetBool(hashIsRun, isMove);

        if (isMove)
        {
            Vector3 lookForward = new Vector3(mainCam.transform.forward.x, 0f, mainCam.transform.forward.z).normalized;
            Vector3 lookRight = new Vector3(mainCam.transform.right.x, 0f, mainCam.transform.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            //characterController.Move(moveDir * speed * Time.deltaTime);
            _MoveTo(moveDir);
        }
    }

    private void _MoveInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (x != 0 || z != 0)
        {
            isRun = true;
            animator.SetBool(hashIsRun, true);
        }
        else
        {
            isRun = false;
            animator.SetBool(hashIsRun, false);
        }

        _MoveTo(new Vector3(x, 0, z));
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
