//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class FPCharacterControllerMovement : MonoBehaviour
//{
//    private CharacterController characterController;
//    private Transform characterTransform;
//    private Vector3 movementDirection;
//    private Animator characterAnimator;
//    private bool isCrouching = false;
//    private float originalHeight;
//    private float velocity;

//    public float sprintSpeed;
//    public float walkSpeed;
//    public float sprintSpeedWhenCrouched;
//    public float walkSpeedWhenCrouched;
//    public float gravity = 9.8f;
//    public float jumpHeight;
//    public float crouchHeight = 1f;

//    // Start is called before the first frame update
//    private void Start()
//    {
//        characterController = GetComponent<CharacterController>();
//        characterAnimator = GetComponentInChildren<Animator>();
//        characterTransform = transform;
//        originalHeight = characterController.height;
//    }

//    // Update is called once per frame
//    private void Update()
//    {
//        //Debug.Log(GetComponent<CharacterController>().velocity);
//        float tmp_CurrentSpeed = walkSpeed;
//        var grd = characterController.isGrounded;
//        if (grd)
//        {
//            var tmp_Horizontal = Input.GetAxis("Horizontal");
//            var tmp_Vertical = Input.GetAxis("Vertical");
//            movementDirection = characterTransform.TransformDirection(new Vector3(tmp_Horizontal, 0, tmp_Vertical));

//            if (Input.GetButtonDown("Jump"))
//            {
//                movementDirection.y = jumpHeight;
//            }

//            if (Input.GetButtonDown("Crouch"))
//            {
//                var tmp_CurrentHeight = isCrouching ? originalHeight : crouchHeight;
//                StartCoroutine(DoCrouch(tmp_CurrentHeight));
//                isCrouching = !isCrouching;
//            }

//            if (isCrouching)
//            {
//                tmp_CurrentSpeed = Input.GetButton("Sprint") ? sprintSpeedWhenCrouched : walkSpeedWhenCrouched;
//            }
//            else
//            {
//                tmp_CurrentSpeed = Input.GetButton("Sprint") ? sprintSpeed : walkSpeed;
//            }

//            var tmp_Velocity = characterController.velocity;
//            tmp_Velocity.y = 0;
//            velocity = tmp_Velocity.magnitude;
//            characterAnimator.SetFloat("Velocity", velocity, 0.25f, Time.deltaTime);
//        }

//        movementDirection.y -= gravity * Time.deltaTime;
//        characterController.Move(Time.deltaTime * tmp_CurrentSpeed * movementDirection);
//    }

//    private IEnumerator DoCrouch(float _target)
//    {
//        float tmp_CurrentHeight = 0;
//        while (Mathf.Abs(characterController.height - _target) > 0.1f)
//        {
//            yield return null;

//            characterController.height = Mathf.SmoothDamp(characterController.height, _target, ref tmp_CurrentHeight, Time.deltaTime * 5);
//        }
//    }
//}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FPCharacterControllerMovement : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private Animator tp_CharacterAnimator;
    private Vector3 movementDirection;
    private Transform characterTransform;
    private float velocity;


    private bool isCrouched;
    private float originHeight;

    public float SprintingSpeed = 8;
    public float WalkSpeed = 4;

    public float SprintingSpeedWhenCrouched = 2;
    public float WalkSpeedWhenCrouched = 1.5f;

    public float Gravity = 9.8f;
    public float JumpHeight = 3;
    public float CrouchHeight = 1f;

    public float CurrentSpeed { get; private set; }
    private IEnumerator crouchCoroutine;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        //        characterAnimator = GetComponentInChildren<Animator>();
        characterTransform = transform;
        originHeight = characterController.height;
    }


    private void Update()
    {
        CurrentSpeed = WalkSpeed;
        if (characterController.isGrounded)
        {
            var tmp_Horizontal = Input.GetAxis("Horizontal");
            var tmp_Vertical = Input.GetAxis("Vertical");
            movementDirection =
                characterTransform.TransformDirection(new Vector3(tmp_Horizontal, 0, tmp_Vertical)).normalized;
            if (isCrouched)
            {
                CurrentSpeed = Input.GetKey(KeyCode.LeftShift) ? SprintingSpeedWhenCrouched : WalkSpeedWhenCrouched;
            }
            else
            {
                CurrentSpeed = Input.GetKey(KeyCode.LeftShift) ? SprintingSpeed : WalkSpeed;
            }

            if (Input.GetButtonDown("Jump"))
            {
                movementDirection.y = JumpHeight;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                var tmp_CurrentHeight = isCrouched ? originHeight : CrouchHeight;
                if (crouchCoroutine != null)
                {
                    StopCoroutine(crouchCoroutine);
                    crouchCoroutine = null;
                }

                crouchCoroutine = DoCrouch(tmp_CurrentHeight);
                StartCoroutine(crouchCoroutine);
                isCrouched = !isCrouched;
            }

            if (characterAnimator != null)
            {
                characterAnimator.SetFloat("Velocity",
                    CurrentSpeed * movementDirection.normalized.magnitude,
                    0.25f,
                    Time.deltaTime);

                tp_CharacterAnimator.SetFloat("Velocity",
                    CurrentSpeed * movementDirection.normalized.magnitude,
                    0.25f,
                    Time.deltaTime);
                tp_CharacterAnimator.SetFloat("Movement_X", tmp_Horizontal, 0.25f, Time.deltaTime);
                tp_CharacterAnimator.SetFloat("Movement_Y", tmp_Vertical, 0.25f, Time.deltaTime);
            }
        }


        movementDirection.y -= Gravity * Time.deltaTime;
        var tmp_Movement = CurrentSpeed * Time.deltaTime * movementDirection;
        characterController.Move(tmp_Movement);
    }


    private IEnumerator DoCrouch(float _target)
    {
        float tmp_CurrentHeight = 0;
        while (Mathf.Abs(characterController.height - _target) > 0.1f)
        {
            yield return null;
            characterController.height =
                Mathf.SmoothDamp(characterController.height, _target,
                    ref tmp_CurrentHeight, Time.deltaTime * 5);
        }
    }

    internal void SetupAnimator(Animator _animator)
    {
        Debug.Log($"Execute! the animator is empty??? {_animator == null}");
        characterAnimator = _animator;
    }
}