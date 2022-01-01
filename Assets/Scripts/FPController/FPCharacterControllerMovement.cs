using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCharacterControllerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Transform characterTransform;
    private Vector3 movementDirection;
    private Animator characterAnimator;
    private bool isCrouching = false;
    private float originalHeight;
    private float velocity;

    public float sprintSpeed;
    public float walkSpeed;
    public float sprintSpeedWhenCrouched;
    public float walkSpeedWhenCrouched;
    public float gravity = 9.8f;
    public float jumpHeight;
    public float crouchHeight = 1f;

    // Start is called before the first frame update
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterTransform = transform;
        originalHeight = characterController.height;
    }

    // Update is called once per frame
    private void Update()
    {
        float tmp_CurrentSpeed = walkSpeed;
        var grd = characterController.isGrounded;
        if (grd)
        {
            var tmp_Horizontal = Input.GetAxis("Horizontal");
            var tmp_Vertical = Input.GetAxis("Vertical");
            movementDirection = characterTransform.TransformDirection(new Vector3(tmp_Horizontal, 0, tmp_Vertical));

            if (Input.GetButtonDown("Jump"))
            {
                movementDirection.y = jumpHeight;
            }

            if (Input.GetButtonDown("Crouch"))
            {
                var tmp_CurrentHeight = isCrouching ? originalHeight : crouchHeight;
                StartCoroutine(DoCrouch(tmp_CurrentHeight));
                isCrouching = !isCrouching;
            }

            if (isCrouching)
            {
                tmp_CurrentSpeed = Input.GetButton("Sprint") ? sprintSpeedWhenCrouched : walkSpeedWhenCrouched;
            }
            else
            {
                tmp_CurrentSpeed = Input.GetButton("Sprint") ? sprintSpeed : walkSpeed;
            }

            if (characterAnimator != null)
            {
                var tmp_Velocity = characterController.velocity;
                tmp_Velocity.y = 0;
                velocity = tmp_Velocity.magnitude;
                characterAnimator.SetFloat("Velocity", velocity, 0.25f, Time.deltaTime);
            }
        }

        movementDirection.y -= gravity * Time.deltaTime;
        characterController.Move(Time.deltaTime * tmp_CurrentSpeed * movementDirection);
    }

    private IEnumerator DoCrouch(float _target)
    {
        float tmp_CurrentHeight = 0;
        while (Mathf.Abs(characterController.height - _target) > 0.1f)
        {
            yield return null;

            characterController.height = Mathf.SmoothDamp(characterController.height, _target, ref tmp_CurrentHeight, Time.deltaTime * 5);
        }
    }

    internal void SetupAnimator(Animator _animator)
    {
        characterAnimator = _animator;
    }
}
