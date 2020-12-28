using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{


  void OnCollisionEnter2D(Collision2D other)
  {
    if (Utils.IsCharacter(other) && !GameManager.dying)
      GameManager.CharacterDied();
  }
}
