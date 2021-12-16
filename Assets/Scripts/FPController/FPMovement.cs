using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPMovement : MonoBehaviour
{
    // �ƶ��ٶ�
    public float Speed;
    public float Gravity;
    public float JumpHeight;
    // ��ǰ��ɫ�� Rigidbody
    private Rigidbody characterRigidbody;
    private bool isOnPlane;
    private bool jumpAction = false;

    // Start is called before the first frame update
    void Start()
    {
        characterRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpAction = true;
        }
    }

    private void FixedUpdate()
    {
        if (isOnPlane)
        {
            var tmp_Horizontal = Input.GetAxis("Horizontal");
            var tmp_Vertical = Input.GetAxis("Vertical");

            // ģ������
            var tmp_CurrentDirection = new Vector3(tmp_Horizontal, 0, tmp_Vertical);
            // ��ģ������ת������������
            tmp_CurrentDirection = transform.TransformDirection(tmp_CurrentDirection);
            // ��������������ƶ��ٶ�
            tmp_CurrentDirection *= Speed;

            var tmp_CurrentVelocity = characterRigidbody.velocity;
            var tmp_VelocityChange = tmp_CurrentDirection - tmp_CurrentVelocity;
            tmp_VelocityChange.y = 0;

            // ʩ��һ����ǰ�ƽ�����
            characterRigidbody.AddForce(tmp_VelocityChange, ForceMode.VelocityChange);

            // ��Ծ����
            if (jumpAction)
            {
                characterRigidbody.velocity = new Vector3(tmp_CurrentVelocity.x, Mathf.Sqrt(2 * Gravity * JumpHeight), tmp_CurrentVelocity.z);
                jumpAction = false;
            }
        }

        // ʩ������
        characterRigidbody.AddForce(new Vector3(0, -Gravity * characterRigidbody.mass, 0));
    }

    // ����Ƿ����ڵ�����
    private void OnCollisionStay(Collision collision)
    {
        isOnPlane = true;
    }

    // ����Ƿ��뿪����
    private void OnCollisionExit(Collision collision)
    {
        isOnPlane = false;
    }
}

