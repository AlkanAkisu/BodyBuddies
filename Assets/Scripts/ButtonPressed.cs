using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressed : MonoBehaviour
{

  bool bodyStay = false, characterStay = false;
  bool wasBodyStay = false, wasCharacterStay = false;
  //bool isPressed

  void Awake()
  {
    bodyStay = false;
    characterStay = false;
    wasBodyStay = false;
    wasCharacterStay = false;
  }
  void OnTriggerStay2D(Collider2D other)
  {

    if (other.name == "Ground Detector")
      return;
    if (Utils.IsCharacter(other))
      characterStay = true;
    if (Utils.IsBody(other))
      bodyStay = true;

    if ((bodyStay && !wasBodyStay) || (characterStay && !wasCharacterStay))
      GetComponentInParent<Button>().ButtonPressed();

    wasBodyStay = bodyStay;
    wasCharacterStay = characterStay;
  }


  void OnTriggerExit2D(Collider2D other)
  {
    if (GameManager.puttingBody) return;

    if (Utils.IsCharacter(other))
      wasCharacterStay = false;
    if (Utils.IsBody(other))
      wasBodyStay = false;

    if (Utils.IsCharacter(other) || Utils.IsBody(other))
    {
      GetComponentInParent<Button>().ButtonDropped();
    }
  }

  private bool isHoldType()
  {
    return GetComponentInParent<Button>().pHoldType;
  }

}
