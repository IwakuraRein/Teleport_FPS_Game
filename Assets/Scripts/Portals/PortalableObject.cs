using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
// [RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PortalableObject : MonoBehaviour
{
    protected GameObject cloneObject;

    protected int inPortalCount = 0;
    
    public Portal inPortal;
    public Portal outPortal;

    protected new Rigidbody rigidbody;
    protected CharacterController playerCharaCtrl;
    protected new Collider collider;

    public static readonly Quaternion halfTurn = Quaternion.Euler(0.0f, 180.0f, 0.0f);

    protected virtual void Awake()
    {
        cloneObject = new GameObject();
        cloneObject.SetActive(false);
        var meshFilter = cloneObject.AddComponent<MeshFilter>();
        var meshRenderer = cloneObject.AddComponent<MeshRenderer>();

        meshFilter.mesh = GetComponent<MeshFilter>().mesh;
        meshRenderer.materials = GetComponent<MeshRenderer>().materials;
        cloneObject.transform.localScale = transform.localScale;

        rigidbody = GetComponent<Rigidbody>();
        playerCharaCtrl = GetComponent<CharacterController>();
        collider = GetComponent<Collider>();
    }

    private void LateUpdate()
    {
        if(inPortal == null || outPortal == null)
        {
            return;
        }

        if(cloneObject.activeSelf && inPortal.IsPlaced && outPortal.IsPlaced)
        {
            var inTransform = inPortal.transform;
            var outTransform = outPortal.transform;

            // Update position of clone.
            Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
            relativePos = halfTurn * relativePos;
            cloneObject.transform.position = outTransform.TransformPoint(relativePos);

            // Update rotation of clone.
            Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
            relativeRot = halfTurn * relativeRot;
            cloneObject.transform.rotation = outTransform.rotation * relativeRot;
        }
        else
        {
            cloneObject.transform.position = new Vector3(-1000.0f, 1000.0f, -1000.0f);
        }
    }

    public virtual void SetIsInPortal(Portal inPortal, Portal outPortal, Collider wallCollider)
    {
        this.inPortal = inPortal;
        this.outPortal = outPortal;

        Physics.IgnoreCollision(collider, wallCollider);

        cloneObject.SetActive(false);

        ++inPortalCount;
    }

    public virtual void ExitPortal(Collider wallCollider)
    {
        Physics.IgnoreCollision(collider, wallCollider, false);
        --inPortalCount;

        if (inPortalCount == 0)
        {
            cloneObject.SetActive(false);
        }
    }

    public virtual void Warp()
    {
        var inTransform = inPortal.transform;
        var outTransform = outPortal.transform;

        // Update position of object.
        Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
        relativePos = halfTurn * relativePos;
        var origPosition = transform.position;
        transform.position = outTransform.TransformPoint(relativePos);
        Debug.Log(origPosition+" "+outTransform.TransformPoint(relativePos)+" "+transform.position);

        // Update rotation of object.
        Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
        relativeRot = halfTurn * relativeRot;
        transform.rotation = outTransform.rotation * relativeRot;

        // Update velocity of rigidbody.
        Vector3 origVelocity = new Vector3();
        if (rigidbody!=null)
            origVelocity = rigidbody.velocity;
        else origVelocity = playerCharaCtrl.velocity;
        Vector3 relativeVel = inTransform.InverseTransformDirection(origVelocity);
        relativeVel = halfTurn * relativeVel;
        UpdateVelocity(outTransform.TransformDirection(relativeVel));

        // Swap portal references.
        var tmp = inPortal;
        inPortal = outPortal;
        outPortal = tmp;
        
    }
    public void UpdateVelocity(Vector3 newVelocity){
        if (rigidbody!=null)
            rigidbody.velocity = newVelocity;
    }
}
