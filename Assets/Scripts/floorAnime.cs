using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorAnime : MonoBehaviour
{
    [SerializeField]
    private float speedX;
    [SerializeField]
    private float speedY;
    private Vector2 offset;
    private Material thisMaterial;

    // Start is called before the first frame update
    void Start()
    {
        thisMaterial=GetComponent<MeshRenderer>().material;
        Debug.Log(thisMaterial.GetTextureOffset("_MainTex"));
    }

    // Update is called once per frame
    void Update()
    {
        offset.x = (offset.x+Time.deltaTime * speedX) % 1;
        offset.y = (offset.x+Time.deltaTime * speedY) % 1;
        thisMaterial.SetTextureOffset("_MainTex", offset);
        //Debug.Log(thisMaterial.GetTextureOffset("_MainTex"));
    }
}
