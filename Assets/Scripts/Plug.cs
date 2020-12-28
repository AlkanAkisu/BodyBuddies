using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : MonoBehaviour
{
  void OnTriggerEnter2D(Collider2D other)
  {
    if (Utils.IsCharacter(other))
      GameManager.plugNearby = true;
  }
  void OnTriggerExit2D(Collider2D other)
  {
    if (Utils.IsCharacter(other))
      GameManager.plugNearby = false;
  }
}
