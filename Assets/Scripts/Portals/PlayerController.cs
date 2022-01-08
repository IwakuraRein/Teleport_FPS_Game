using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering;

public class PlayerController : PortalableObject
{
    // private CameraMove cameraMove;
    // public float MoveSpd=4;
    // public float MsSpd=300;
    public GameObject MainCam;
    // public Transform playerModel;
    float angle;
    // private new Rigidbody rigidbody;
    public Quaternion TargetRotation { private set; get; }

    public Vector3 camLocalPos;
    public Quaternion camLocalRot;
    public FPMouseLook mouseController;
    //public GameObject third_person_character;

    protected override void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Physics.autoSyncTransforms = true;
        base.Awake();
        //third_person_character.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        //third_person_character.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        //third_person_character.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        //third_person_character.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        //third_person_character.transform.GetChild(3).GetComponent<SkinnedMeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;



        // cameraMove = GetComponent<CameraMove>();
        TargetRotation = MainCam.transform.rotation;
        MainCam = GameObject.FindGameObjectWithTag("MainCamera");
        Physics.IgnoreCollision(GetComponent<CharacterController>(), MainCam.GetComponent<Collider>());
        camLocalPos = MainCam.transform.localScale;
        camLocalRot = MainCam.transform.localRotation;
    }

    public override void Warp()
    {
        Debug.Log("player.warp ");
        mouseController.enabled=false;
        base.Warp();
        mouseController.syncThisFrame = true;
        mouseController.enabled=true;
        
        // MainCam.GetComponent<MainCam>().LockCollider=true;
        // MainCam.GetComponent<MainCam>().ExitPortal();
        // cameraMove.ResetTargetRotation();
        TargetRotation = Quaternion.LookRotation(MainCam.transform.forward, Vector3.up);
    }

    public override void ExitPortal(Collider wallCollider)
    {
        // MainCam.GetComponent<MainCam>().LockCollider=false;
        Physics.IgnoreCollision(GetComponent<CharacterController>(), wallCollider, false);

        --inPortalCount;

        if (inPortalCount == 0)
        {
            cloneObject.SetActive(false);
        }
    }
    public override void SetIsInPortal(Portal inPortal, Portal outPortal, Collider wallCollider)
    {
        this.inPortal = inPortal;
        this.outPortal = outPortal;

        Physics.IgnoreCollision(GetComponent<CharacterController>(), wallCollider);
        // Physics.IgnoreCollision(MainCam.GetComponent<Collider>(), inPortal.GetComponent<Collider>(), false);
        //Physics.IgnoreCollision(collider, wallCollider);

        cloneObject.SetActive(false);

        ++inPortalCount;
    }
    
    // private void manageExit(){
    //     MainCam.GetComponent<MainCam>().ExitPortal();
    // }
    

    // private void Start() {
    //     rigidbody = GetComponent<Rigidbody>();
    //     MainCam = GameObject.FindGameObjectWithTag("MainCamera");
    //     Cursor.lockState = CursorLockMode.Locked;
    //     // playerModel = transform.GetChild(0);
    // }
    // private void Update() {
    //     float msX = Input.GetAxis("Mouse X") * MsSpd * Time.deltaTime;
    //     transform.Rotate(Vector3.up * msX);
    //     float msY = Input.GetAxis("Mouse Y") * MsSpd * Time.deltaTime;
    //     angle -= msY;
    //     angle = Mathf.Clamp(angle, -90, 90);
    //     MainCam.transform.localRotation = Quaternion.Euler(angle, 0, 0);
    //     TargetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, MainCam.transform.rotation.eulerAngles.y, 0);
    //     camLocalPos = MainCam.transform.localScale;
    //     camLocalRot = MainCam.transform.localRotation;
        
    // }
    // private void FixedUpdate() {
    //     float x = Input.GetAxis("Horizontal");
    //     float z = Input.GetAxis("Vertical");
    //     Vector3 moveVector = new Vector3(x, 0, z);
    //     Vector3 newVelocity = transform.TransformDirection(moveVector) * MoveSpd;
    //     rigidbody.velocity= new Vector3(newVelocity.x, rigidbody.velocity.y, newVelocity.z);
    // }

    // cameramove.cs里也实现了移动

    // public override void SetIsInPortal(Portal inPortal, Portal outPortal, Collider wallCollider)
    // {
    //     this.inPortal = inPortal;
    //     this.outPortal = outPortal;

    //     Physics.IgnoreCollision(collider, wallCollider);

    //     cloneObject.SetActive(false);

    //     ++inPortalCount;

    //     //TeleportCam();
    //     Debug.Log("player enter portal");
    // }

    // public override void ExitPortal(Collider wallCollider)
    // {
    //     Physics.IgnoreCollision(collider, wallCollider, false);
    //     --inPortalCount;

    //     if (inPortalCount == 0)
    //     {
    //         cloneObject.SetActive(false);
    //     }

        
    //     MainCam.transform.localPosition = camLocalPos;
    //     MainCam.GetComponent<MainCam>().insidePortal = false;
    //     //MainCam.transform.localRotation = camLocalRot;
    //     Debug.Log("player exit portal");
    // }


}
