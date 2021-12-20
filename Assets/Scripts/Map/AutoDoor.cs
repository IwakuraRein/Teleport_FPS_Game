using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator anim;
    private AudioSource audio;
    void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Open(){
        audio.PlayOneShot(audio.clip, 0.4f);
        anim.SetTrigger("Open");
    }
    public void Close(){
        audio.PlayOneShot(audio.clip, 0.4f);
        anim.SetTrigger("Close");
    }
    // private void OnCollisionEnter(Collision other) {
    //     Debug.Log("Something Hit Door.");
    //     if (other.transform.tag == "Player"){
    //         Debug.Log("Player Hit Door.");
    //         Open();
    //     }
    // }
    // private void OnCollisionExist(Collision other) {
    //     if (other.transform.tag == "Player"){
    //         Debug.Log("Player Exit Door.");
    //         Close();
    //     }
    // }
}
