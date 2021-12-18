using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(CameraMove))]
//[RequireComponent(typeof(PlayerController))]
public class PortalPlacement : MonoBehaviour
{
    [SerializeField]
    private PortalPair portals;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Crosshair crosshair;

    [SerializeField]
    private Transform testTransform;
    bool AbleToPlace;

    //private CameraMove cameraMove;
    //private PlayerController playerController;
    public GameObject MainCam;
    [SerializeField]
    private LayerMask placementMask;
    [SerializeField]
    private LayerMask checkIntersectMask;
    [SerializeField]
    private LayerMask checkIntersectMask2; //不包含portal层的mask。和portal相交时不启动自我修复（FixIntersects）。

    [SerializeField]
    private Texture ableTexture, disableTexture;


    [SerializeField]
    GameObject previewPortal;
    private Collider wallCollider;

    private void Awake()
    {
        //cameraMove = GetComponent<CameraMove>();
        //playerController = GetComponent<PlayerController>();
        MainCam = GameObject.FindGameObjectWithTag("MainCamera");
        
        AbleToPlace=false;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.CapsLock)) {
            PreviewPortal(MainCam.transform.position, MainCam.transform.forward, 250.0f);
            // if(Input.GetButtonDown("Fire1"))
            if(Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Try to place portal 1.");
                if (AbleToPlace) previewPortal.SetActive(false);
                FirePortal(0, MainCam.transform.position, MainCam.transform.forward, 250.0f);
                //FirePortal(0, playerController.transform.TransformPoint(MainCam.transform.position), playerController.transform.TransformDirection(MainCam.transform.forward), 250.0f);
            }
            // else if (Input.GetButtonDown("Fire2"))
            else if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Try to place portal 2.");
                if (AbleToPlace) previewPortal.SetActive(false);
                FirePortal(1, MainCam.transform.position, MainCam.transform.forward, 250.0f);
                //FirePortal(1, playerController.transform.TransformPoint(MainCam.transform.position), playerController.transform.TransformDirection(MainCam.transform.forward), 250.0f);
            }
        }
        else previewPortal.SetActive(false);
    }
    private void PreviewPortal(Vector3 pos, Vector3 dir, float distance){
        Debug.DrawLine(pos, pos+dir*20, Color.blue);
        RaycastHit hit;
        bool ifHit = Physics.Raycast(pos, dir, out hit, distance, layerMask);
        if(hit.collider != null)
        {
            // Orient the portal according to camera look direction and surface direction.
            //var cameraRotation = cameraMove.TargetRotation;
            //var cameraRotation = playerController.TargetRotation;
            //var portalRight = cameraRotation * Vector3.right;
            
            // if(Mathf.Abs(portalRight.x) >= Mathf.Abs(portalRight.z))
            // {
            //     portalRight = (portalRight.x >= 0) ? Vector3.right : -Vector3.right;
            // }
            // else
            // {
            //     portalRight = (portalRight.z >= 0) ? Vector3.forward : -Vector3.forward;
            // }

            var portalForward = -hit.normal;
            //var portalUp = -Vector3.Cross(portalRight, portalForward);
            var portalUp = Vector3.up;
            var portalRotation = Quaternion.LookRotation(portalForward, portalUp);
            
            // Attempt to place the portal.
            AbleToPlace = PlacePreview(hit.collider, hit.point, portalRotation);
        }
    }
    private void FirePortal(int portalID, Vector3 pos, Vector3 dir, float distance)
    {
        Debug.DrawLine(pos, pos+dir*20, Color.green, 1.0f);
        RaycastHit hit;
        bool ifHit = Physics.Raycast(pos, dir, out hit, distance, layerMask);
        if(!ifHit) Debug.Log("Not hit. "+dir);
        if(hit.collider != null)
        {
            // If we shoot a portal, recursively fire through the portal.
            // if (hit.collider.tag == "Portal")
            // {
            //     var inPortal = hit.collider.GetComponent<Portal>();

            //     if(inPortal == null)
            //     {
            //         return;
            //     }

            //     var outPortal = inPortal.OtherPortal;

            //     // Update position of raycast origin with small offset.
            //     Vector3 relativePos = inPortal.transform.InverseTransformPoint(hit.point + dir);
            //     relativePos = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativePos;
            //     pos = outPortal.transform.TransformPoint(relativePos);

            //     // Update direction of raycast.
            //     Vector3 relativeDir = inPortal.transform.InverseTransformDirection(dir);
            //     relativeDir = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativeDir;
            //     dir = outPortal.transform.TransformDirection(relativeDir);

            //     distance -= Vector3.Distance(pos, hit.point);

            //     FirePortal(portalID, pos, dir, distance);

            //     return;
            // }

            // Orient the portal according to camera look direction and surface direction.
            //var cameraRotation = cameraMove.TargetRotation;
            //var cameraRotation = playerController.TargetRotation;
            //var portalRight = cameraRotation * Vector3.right;
            
            // if(Mathf.Abs(portalRight.x) >= Mathf.Abs(portalRight.z))
            // {
            //     portalRight = (portalRight.x >= 0) ? Vector3.right : -Vector3.right;
            // }
            // else
            // {
            //     portalRight = (portalRight.z >= 0) ? Vector3.forward : -Vector3.forward;
            // }

            //-----------尝试直接用test portal的位置来放置portal
            // var portalForward = -hit.normal;
            // //var portalUp = -Vector3.Cross(portalRight, portalForward);
            // var portalUp = Vector3.up;
            // var portalRotation = Quaternion.LookRotation(portalForward, portalUp);
            
            // // Attempt to place the portal.
            // bool wasPlaced = portals.Portals[portalID].PlacePortal(hit.collider, hit.point, portalRotation);
            // if(wasPlaced)
            // {
            //     Debug.Log("Place success.");
            //     crosshair.SetPortalPlaced(portalID, true);
            // }
            // else Debug.Log("Place fail.");

            if (AbleToPlace) {
                portals.Portals[portalID].GetComponent<Portal>().wallCollider = hit.collider;
                portals.Portals[portalID].transform.position = testTransform.position;
                portals.Portals[portalID].transform.rotation = testTransform.rotation;
                portals.Portals[portalID].gameObject.SetActive(true);
                portals.Portals[portalID].GetComponent<Portal>().IsPlaced = true;
                crosshair.SetPortalPlaced(portalID, true);
                Debug.Log("Place success.");
            }
            else Debug.Log("Place fail.");
        }
    }
    public bool PlacePreview(Collider wallCollider, Vector3 pos, Quaternion rot)
    {
        testTransform.position = pos;
        testTransform.rotation = rot;
        testTransform.position -= testTransform.forward * 0.001f;

        FixOverhangs();
        FixIntersects();

        previewPortal.transform.position = testTransform.position;
        previewPortal.transform.rotation = testTransform.rotation;

        previewPortal.SetActive(true);

        if (CheckOverlap())
        {
            this.wallCollider = wallCollider;
            // previewPortal.transform.position = testTransform.position;
            // previewPortal.transform.rotation = testTransform.rotation;

            previewPortal.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.green; //访问outline的renderer
            return true;
        }
        previewPortal.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;  //访问outline的renderer

        return false;
    }
        private void FixOverhangs()
    {
        float scale_1 = transform.localScale.x;
        float scale_2 = transform.localScale.y;
        var testPoints = new List<Vector3>
        {
            new Vector3(-scale_1-0.1f,  0.0f, 0.1f),
            new Vector3( scale_1+0.1f,  0.0f, 0.1f),
            new Vector3( 0.0f, -scale_2*2 - 0.1f, 0.1f),
            new Vector3( 0.0f,  scale_2*2 + 0.1f, 0.1f)
        };

        var testDirs = new List<Vector3>
        {
             Vector3.right,
            -Vector3.right,
             Vector3.up,
            -Vector3.up
        };

        for(int i = 0; i < 4; ++i)
        {
            RaycastHit hit;
            Vector3 raycastPos = testTransform.TransformPoint(testPoints[i]);
            Vector3 raycastDir = testTransform.TransformDirection(testDirs[i]);

            if(Physics.CheckSphere(raycastPos, 0.05f, placementMask))
            {
                break;
            }
            else if(Physics.Raycast(raycastPos, raycastDir, out hit, 2.1f, placementMask))
            {
                var offset = hit.point - raycastPos;
                testTransform.Translate(offset, Space.World);
            }
        }
    }

    // Ensure the portal cannot intersect a section of wall.
    private void FixIntersects()
    {
        float scale_1 = transform.localScale.x;
        float scale_2 = transform.localScale.y;
        var testDirs = new List<Vector3>
        {
             Vector3.right,
            -Vector3.right,
             Vector3.up,
            -Vector3.up
        };

        // var testDists = new List<float> { scale_1+0.05f, scale_1+0.05f, scale_2*2+0.05f, scale_2*2+0.05f };
        var testDists = new List<float> { scale_1, scale_1, scale_2*2-0.15f, scale_2*2-0.15f };

        for (int i = 0; i < 4; ++i)
        {
            RaycastHit hit;
            Vector3 raycastPos = testTransform.TransformPoint(0.0f, 0.0f, -0.05f);
            Vector3 raycastDir = testTransform.TransformDirection(testDirs[i]);

            if (Physics.Raycast(raycastPos, raycastDir, out hit, testDists[i], checkIntersectMask2))
            {
                var offset = (hit.point - raycastPos);
                var newOffset = -raycastDir * (testDists[i] - offset.magnitude);
                testTransform.Translate(newOffset, Space.World);
            }
        }
    }

    // Once positioning has taken place, ensure the portal isn't intersecting anything.
    private bool CheckOverlap()
    {
        float offset = 0.15f;
        float scale_1 = transform.localScale.x;
        float scale_2 = transform.localScale.y;
        var checkExtents = new Vector3(scale_1-offset, scale_2*2-2*offset, 0.05f);

        var checkPositions = new Vector3[]
        {
            testTransform.position + testTransform.TransformVector(new Vector3( 0.0f,  0.0f, -0.1f)),

            testTransform.position + testTransform.TransformVector(new Vector3(-scale_1, -2*scale_2, -0.1f)),
            testTransform.position + testTransform.TransformVector(new Vector3(-scale_1,  2*scale_2, -0.1f)),
            testTransform.position + testTransform.TransformVector(new Vector3( scale_1, -2*scale_2, -0.1f)),
            testTransform.position + testTransform.TransformVector(new Vector3( scale_1,  2*scale_2, -0.1f)),

            testTransform.TransformVector(new Vector3(0.0f, 0.0f, 0.2f))
        };

        // Ensure the portal does not intersect walls.
        var intersections = Physics.OverlapBox(checkPositions[0], checkExtents, testTransform.rotation, checkIntersectMask);
        Debug.Log(intersections.Length);
        if(intersections.Length > 1)
        {
            return false;
        }
        else if(intersections.Length == 1) 
        {
            
            // We are allowed to intersect the old portal position.
            if (intersections[0] != previewPortal.GetComponent<BoxCollider>())
            {
                return false;
            }
        }

        // Ensure the portal corners overlap a surface.
        bool isOverlapping = true;

        for(int i = 1; i < checkPositions.Length - 1; ++i)
        {
            isOverlapping &= Physics.Linecast(checkPositions[i], 
                checkPositions[i] + checkPositions[checkPositions.Length - 1], placementMask);
        }

        return isOverlapping;
    }

}
