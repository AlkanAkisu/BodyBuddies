using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
class Sound
{
  public AudioClip clip;
  public string name;
  [Range(0f, 1f)]
  public float volume = 0.3f;

  public bool loop = false;

  [HideInInspector] public AudioSource source;
}