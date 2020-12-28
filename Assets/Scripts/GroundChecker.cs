using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
  [SerializeField] LayerMask groundLayer;
  void OnTriggerStay2D(Collider2D other)
  {
    if (((1 << other.gameObject.layer) & groundLayer) == 0) return;
    Character script = GameManager.character.GetComponent<Character>();
    script.grounded = true;
  }

  void OnTriggerExit2D(Collider2D other)
  {
    if (((1 << other.gameObject.layer) & groundLayer) == 0) return;
    Character script = GameManager.character.GetComponent<Character>();
    script.grounded = true;
  }
}
