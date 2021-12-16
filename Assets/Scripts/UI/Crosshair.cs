using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private PortalPair portalPair;

    [SerializeField]
    private Image inPortalImg;

    [SerializeField]
    private Image outPortalImg;
    [SerializeField]
    private float alpha = 0.6f;

    private void Start()
    {
        var portals = portalPair.Portals;

        inPortalImg.color = new Vector4(portals[0].PortalColour.r, portals[0].PortalColour.g, portals[0].PortalColour.b, alpha);
        outPortalImg.color = new Vector4(portals[1].PortalColour.r, portals[1].PortalColour.g, portals[1].PortalColour.b, alpha);
        Debug.Log(outPortalImg.color);
        inPortalImg.gameObject.SetActive(false);
        outPortalImg.gameObject.SetActive(false);
    }

    public void SetPortalPlaced(int portalID, bool isPlaced)
    {
        if(portalID == 0)
        {
            inPortalImg.gameObject.SetActive(isPlaced);
        }
        else
        {
            outPortalImg.gameObject.SetActive(isPlaced);
        }
    }
}
