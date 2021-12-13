using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FPS/Footstep Audio Data")]
public class FootstepAudioData : ScriptableObject
{
    public List<FootstepAudio> footstepAudios = new List<FootstepAudio>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class FootstepAudio
{
    public string tag;
    public List<AudioClip> audioClips = new List<AudioClip>();
    public float Delay;
    public float sprintDelay;
    public float chouchDelay;
}