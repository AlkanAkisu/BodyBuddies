using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
  // Start is called before the first frame update
  [SerializeField] private Transform otherEnd;
  [SerializeField] float secondsForTeleport = 0.4f;
  [SerializeField] bool isWorking = true;
  [SerializeField] bool isOrange = true;
  private bool isTeleported;
  Animator anim;
  void Awake()
  {
    anim = GetComponent<Animator>();
    if (isOrange)
    {
      anim.SetBool("IsOrange", true);
    }
    else
    {
      anim.SetBool("IsOrange", false);
    }
  }

  // Update is called once per frame
  void Update()
  {

  }

  void OnTriggerStay2D(Collider2D other)
  {
    GameObject character = GameManager.character;
    if (!Utils.IsCharacter(other))
      return;
    if (Mathf.Abs(character.transform.position.x - transform.position.x) > 0.15f)
      return;

    if (!isTeleported && !GameManager.justTeleported && isWorking)
    {
      StartCoroutine(TeleportCharacter());
      isTeleported = true;
    }


  }


  void OnTriggerExit2D(Collider2D other)
  {
    if (isTeleported) return;
    GameManager.justTeleported = false;


  }
  IEnumerator TeleportCharacter()
  {
    GameManager.justTeleported = true;
    GameManager.canMove = false;
    Transform ch = GameManager.character.transform;
    SpriteRenderer sp = ch.GetComponent<SpriteRenderer>();

    while (sp.color.a > 0.02f)
    {
      setAlphaCharacter(sp.color.a - (Time.deltaTime / (secondsForTeleport / 2)));
      yield return null;
    }
    setAlphaCharacter(0f);

    ch.position = otherEnd.position;

    while (sp.color.a < 0.96f)
    {
      setAlphaCharacter(sp.color.a + (Time.deltaTime / (secondsForTeleport / 2)));
      yield return null;
    }
    setAlphaCharacter(1f);
    GameManager.canMove = true;
    isTeleported = false;

  }

  private void setAlphaCharacter(float alpha)
  {
    SpriteRenderer sp = GameManager.character.GetComponent<SpriteRenderer>();
    sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, alpha);
  }
}
