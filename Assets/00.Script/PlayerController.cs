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
    private bool isRun;


    [Header("Attack")]
    [SerializeField] private float aaCoolDownTime = 0.6f;
    [SerializeField] private float curaaCoolDownTime;
    [SerializeField] private bool isAttack = false;

    [Header("Camera")]
    [SerializeField] private Camera mainCam;

    [Header("Animation")]
    private readonly int hashIsRun = Animator.StringToHash("IsRun");
    private readonly int hashaa = Animator.StringToHash("aa");
     int hashaaCount = Animator.StringToHash("aaCount");


    private KeyCode jumpKeyCode = KeyCode.Space;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!isAttack)
        {
            _MoveInput();
        }
    }

    private void Update()
    {
        if (!isAttack)
        {
            _JumpInput();
        }
        OnClickAttack();
    }

    private void OnClickAttack()
    {
        if (Input.GetMouseButtonDown(0) && aaCoolDownTime <= 0)
        {
            curaaCoolDownTime = aaCoolDownTime;
            isAttack = true;
            animator.SetTrigger(hashaa);
        }
        else
        {
            aaCoolDownTime -= Time.deltaTime;
        }
    }
    public void MoveOn()
    {
        isAttack = false;
    }

    #region Move
    private void _MoveInput()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isRun = moveInput.magnitude != 0;
        animator.SetBool(hashIsRun, isRun);

        Vector3 lookForward = new Vector3(mainCam.transform.forward.x, 0f, mainCam.transform.forward.z).normalized;
        Vector3 lookRight = new Vector3(mainCam.transform.right.x, 0f, mainCam.transform.right.z).normalized;
        Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

        //characterController.Move(moveDir * speed * Time.deltaTime);
        _MoveTo(moveDir);

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
    #endregion

    #region Jump
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
    #endregion

    #region Gravity
    private void _Gravity()
    {
        moveDirection.y += gravity * Time.deltaTime;
    }
    #endregion
}
