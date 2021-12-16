using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPMovement : MonoBehaviour
{
    // 移动速度
    public float Speed;
    public float Gravity;
    public float JumpHeight;
    // 当前角色的 Rigidbody
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

            // 模型坐标
            var tmp_CurrentDirection = new Vector3(tmp_Horizontal, 0, tmp_Vertical);
            // 从模型坐标转换成世界坐标
            tmp_CurrentDirection = transform.TransformDirection(tmp_CurrentDirection);
            // 对世界坐标乘上移动速度
            tmp_CurrentDirection *= Speed;

            var tmp_CurrentVelocity = characterRigidbody.velocity;
            var tmp_VelocityChange = tmp_CurrentDirection - tmp_CurrentVelocity;
            tmp_VelocityChange.y = 0;

            // 施加一个向前推进的力
            characterRigidbody.AddForce(tmp_VelocityChange, ForceMode.VelocityChange);

            // 跳跃功能
            if (jumpAction)
            {
                characterRigidbody.velocity = new Vector3(tmp_CurrentVelocity.x, Mathf.Sqrt(2 * Gravity * JumpHeight), tmp_CurrentVelocity.z);
                jumpAction = false;
            }
        }

        // 施加重力
        characterRigidbody.AddForce(new Vector3(0, -Gravity * characterRigidbody.mass, 0));
    }

    // 检测是否留在地面上
    private void OnCollisionStay(Collision collision)
    {
        isOnPlane = true;
    }

    // 检测是否离开地面
    private void OnCollisionExit(Collision collision)
    {
        isOnPlane = false;
    }
}

