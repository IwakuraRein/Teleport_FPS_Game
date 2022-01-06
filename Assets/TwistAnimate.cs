using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwistAnimate : MonoBehaviour
{
    public float progress=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }
   
    public void OnEnable()
    {
        Image image =  this.GetComponent<Image>();

        image.material.SetFloat("Twist Progress", 0.5f);

    }
    // Update is called once per frame
    void Update()
    {
        Image image= this.GetComponent<Image>();
        
        image.material.SetFloat("_Twist_Progress", progress);
    }
}
