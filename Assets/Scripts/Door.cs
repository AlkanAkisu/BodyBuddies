using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
  // Start is called before the first frame update

  [SerializeField] private Sprite closeSprite, openSprite;
  [SerializeField] private DoorState InitialState = DoorState.closed;
  private Collider2D _collider;
  private Vector2 direction;
  [HideInInspector] public DoorState state, lastState;
  private SpriteRenderer sp;
  private Animator animator;
  public enum DoorState
  {
    open,
    closed,
    opening,
    closing
  }
  void Awake()
  {
    sp = GetComponent<SpriteRenderer>();
    animator = GetComponent<Animator>();
    _collider = GetComponent<BoxCollider2D>();
    state = InitialState;
    if (state == DoorState.open)
    {
      animator.SetTrigger("Opened");
    }
    else if (state == DoorState.closed)
    {

    }
    else
    {
      animator.SetTrigger("Closed");
      sp.sprite = closeSprite;
    }
  }

  // Update is called once per frame
  void Update()
  {

    //For development
    // if (Input.GetKeyUp(KeyCode.F))
    //   activateDoor();
    HandleCollider();
  }

  public void activateDoor()
  {
    if (isActive())
      return;
    Debug.Log("activate doors");
    if (InitialState == DoorState.open)
    {
      state = DoorState.closing;
      animator.SetTrigger("Closing");
    }
    else if (InitialState == DoorState.closed)
    {
      state = DoorState.opening;
      animator.SetTrigger("Opening");
    }
  }

  public void InvertDoor()
  {
    Debug.Log("InvertDoor");


    if (state == DoorState.closing || state == DoorState.closed)
    {
      Debug.Log("InvertDoor:open");
      animator.SetTrigger("Opened");
      state = DoorState.open;
    }
    else if (state == DoorState.opening || state == DoorState.open)
    {
      Debug.Log("InvertDoor:close");
      animator.SetTrigger("Closed");
      state = DoorState.closed;
    }

  }

  public bool isActive()
  {
    return state == DoorState.closing || state == DoorState.opening;
  }

  public void DoorAnimationFinished()
  {
    //Debug.Log("Door anim");
    if (state == DoorState.closing)
    {
      state = DoorState.closed;
      sp.sprite = closeSprite;
    }
    else if (state == DoorState.opening)
    {
      state = DoorState.open;
      sp.sprite = openSprite;
    }


  }

  private void HandleCollider()
  {

    if (state == DoorState.open || state == DoorState.opening)
    {
      _collider.enabled = false;
    }
    else
    {
      _collider.enabled = true;
    }
  }
}
