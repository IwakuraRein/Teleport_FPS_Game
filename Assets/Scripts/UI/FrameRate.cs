using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameRate : MonoBehaviour
{
    // Start is called before the first frame update
    public Text txt;
    float delta = 0f;
    void Start()
    {
        txt = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;
        if (delta > 0.5) {
            delta = 0f;
            txt.text = "FPS: "+((int)(1.0f / Time.deltaTime)).ToString();
        }
    }
}
