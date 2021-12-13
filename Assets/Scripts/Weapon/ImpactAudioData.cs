using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Weapon
{
    [CreateAssetMenu(menuName = "FPS/Impact Audio Data")]
    public class ImpactAudioData : ScriptableObject
    {
        public List<ImpactTagsWithAudio> impactTagsWithAudios;
    }

    [System.Serializable]
    public class ImpactTagsWithAudio
    {
        public string tag;
        public List<AudioClip> impactAudioClips;
    }
}