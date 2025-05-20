using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 curMovementInput;
    private bool isMove;

    [Header("Jump")]
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float jumpPower = 80f;
    private bool isJump;

    [Header("Look")]
    [SerializeField] private Transform cameraContainer;
    [SerializeField] private float minXLook = -85f;
    [SerializeField] private float maxXLook = 85f;
    [SerializeField] private float lookSensitivity = 0.1f;
    private float camCurXRot;
    private Vector2 mouseDelta;
    private bool canLook = true;

    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // 입력 y값은 forward(앞뒤, z축), 입력 x값은 right(좌우, x축)
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;

        // 점프할때만 y값을 변경해야 하기 때문에 값을 유지
        dir.y = rb.velocity.y;

        rb.velocity = dir;
    }

    public bool IsMoveInput()
    {
        return isMove;
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
            isMove = true;
            animator.SetBool("isMove", true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
            isMove = false;
            animator.SetBool("isMove", false);
        }
    }

    public void CameraLook()
    {
        // 마우스의 xy 위치와 회전축 xy는 서로 반대임.
        // 마우스 y움직임(실제로는 카메라 x회전)은 최대, 최소로 제한. 카메라를 회전시킨다.
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        // 마우스 x움직임(실제로는 카메라 y회전)은 플레이어 자체를 회전 시켜서 이동에 반영되도록 한다.
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            animator.SetTrigger("isJump");
            isJump = true;
        }

    }

    public bool IsGrounded()
    {
        // 충돌체를 감지하는 Ray 생성
        // 플레이어 위치를 기준으로 앞뒤좌우 0.2씩 떨어트리고 0.01정도 살짝 위로 올려서 확인
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        // 4개의 ray중 한개라도 groundLayerMask의 오브젝트에 충돌했는지 검사한다
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsJumpInput()
    {
        return isJump;
    }
}
