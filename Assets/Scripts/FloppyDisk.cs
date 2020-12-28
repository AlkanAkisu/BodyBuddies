using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloppyDisk : MonoBehaviour
{

  [TextArea(5, 10)] public string[] sentences;
  private bool read;

  void Awake()
  {
    read = false;
  }
  void OnTriggerEnter2D(Collider2D other)
  {
    // GameManager.SceneFinished();
    // return;
    if (GameManager.reading || read)
      return;

    if (!GameManager.isTutorial)
    {
      if (Utils.IsCharacter(other))
      {
        GameManager.reading = true;
        read = true;
        Dialogue.i.ReadFloppyDisk(this);
      }
    }
    else
    {
      GameManager.SceneFinished();
    }
  }
}
