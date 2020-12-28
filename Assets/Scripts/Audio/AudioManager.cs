using UnityEngine;
using System;
using UnityEngine.Audio;

class AudioManager : MonoBehaviour
{
  public Sound[] sounds;
  private static AudioManager _instance;
  public static AudioManager i { get { return _instance; } }



  void Awake()
  {
    _instance = this;
    foreach (Sound sound in sounds)
    {
      sound.source = gameObject.AddComponent<AudioSource>();
      sound.source.clip = sound.clip;
      sound.source.volume = sound.volume;
      sound.source.loop = sound.loop;
    }
  }

  public void Play(string name)
  {
    Sound s = Array.Find(sounds, sound => sound.name == name);
    s.source.Play();

  }
  public void Stop(string name)
  {
    Sound s = Array.Find(sounds, sound => sound.name == name);
    s.source.Stop();

  }

}