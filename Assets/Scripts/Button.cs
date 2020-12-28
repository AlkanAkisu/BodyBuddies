using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
  // Start is called before the first frame update
  [SerializeField] private Door[] doors;
  [SerializeField] private bool holdType;
  private bool isPressed, isHolding;
  private bool oneTime = true;

  [SerializeField] private Collider2D openCollider, closeCollider;
  [SerializeField] private Sprite openSprite, closeSprite;
  private SpriteRenderer sp;

  [HideInInspector] public bool pHoldType;
  void Awake()
  {
    isPressed = false;
    pHoldType = holdType;
    sp = GetComponent<SpriteRenderer>();

  }



  public void ButtonPressed()
  {
    if (pHoldType && !openCollider.enabled)
      return;

    if (openCollider.enabled)
    {
      openCollider.enabled = false;
      closeCollider.enabled = true;
      sp.sprite = closeSprite;
    }

    if (pHoldType)
    {
      ActivateAllDoors();
    }
    else if (!oneTime || !isPressed)
    {

      ActivateAllDoors();
      isPressed = true;
    }

  }
  public void ButtonHolding()
  {
    if (!pHoldType) return;

  }

  public void ButtonDropped()
  {
    if (!holdType) return;


    openCollider.enabled = true;
    closeCollider.enabled = false;
    sp.sprite = openSprite;

    InvertAllDoors();
  }

  private void ActivateAllDoors()
  {
    foreach (Door door in doors)
    {
      door.activateDoor();
    }
  }
  private void InvertAllDoors()
  {
    foreach (Door door in doors)
    {
      door.InvertDoor();
    }
  }

}
