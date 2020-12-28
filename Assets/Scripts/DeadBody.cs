using UnityEngine;

class DeadBody : MonoBehaviour
{

  public bool carrying = false;
  private SpriteRenderer sp;
  private Rigidbody2D rb;


  void Awake()
  {
    sp = GetComponent<SpriteRenderer>();
    rb = GetComponent<Rigidbody2D>();
    carrying = false;
  }
  void FixedUpdate()
  {
    if (carrying)
    {
      //GetComponent<BoxCollider2D>().enabled = false;
      Vector2 target = GameManager.character.transform.position;
      target.y += 0.8f;
      transform.position = target;
      transform.rotation = Quaternion.identity;
      sp.flipX = !GameManager.character.GetComponent<Character>().facingRight;
      rb.isKinematic = true;
    }
    else
    {
      rb.isKinematic = false;
    }
  }

}