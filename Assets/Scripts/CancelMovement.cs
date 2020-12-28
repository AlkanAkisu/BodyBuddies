using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelMovement : MonoBehaviour
{
  [SerializeField] private LayerMask blockageLayer;
  // Start is called before the first frame update
  void OnTriggerStay2D(Collider2D other)
  {
    // if (((1 << other.gameObject.layer) & blockageLayer) == 0) return;

    // float characterX = GameManager.character.transform.position.x;

    // if (other.transform.position.x > characterX)
    // {
    //   GameManager.rightBlocked = true;
    // }
    // else
    // {
    //   GameManager.leftBlocked = true;
    // }

  }
  void OnTriggerExit2D(Collider2D other)
  {
    //   if (((1 << other.gameObject.layer) & blockageLayer) == 0)
    //     return;


    //   GameManager.rightBlocked = false;
    //   GameManager.leftBlocked = false;

  }
}
