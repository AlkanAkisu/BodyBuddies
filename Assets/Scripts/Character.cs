using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

  [SerializeField] private float jumpForce = 400f;
  [SerializeField] private float speedFactor = 10f;
  [Range(0, 0.3f)] [SerializeField] private float movementSmoothing = .05f;
  [SerializeField] private LayerMask whatIsGround;
  [SerializeField] private Transform groundCheck;



  const float kGroundedRadius = .4f;
  private bool wasGrounded;
  public bool grounded;
  const float kSlowingFactor = 4f;
  const float kSecondsAfterLeftGround = .5f;
  private Rigidbody2D rb;
  private Animator animator;
  public bool facingRight;
  private Vector2 velocity = Vector2.zero;


  private float timeAfterLeave;
  private bool isFalling;

  private void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    facingRight = true;
    grounded = true;
  }

  void Update()
  {
    int inputValue = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
    inputValue = GameManager.rightBlocked && inputValue == 1
    ? 0
    : GameManager.leftBlocked && inputValue == -1
    ? 0
    : inputValue;

    AnimationHandler(inputValue);

    if (!GameManager.canMove)
    {
      StopCharacter();
      return;
    }

    //Below is when character can move

    if (Input.GetKeyUp(KeyCode.Q) && GameManager.plugNearby)
    {
      AudioManager.i.Play("Electric");
      Die();
    }

    if (Input.GetKeyUp(KeyCode.E))
      CarryBody();
    if (Input.GetKeyUp(KeyCode.S))
      PutBody();
    if (Input.GetKeyUp(KeyCode.R))
      LevelManager.i.RestartScene();

    Move(inputValue, Input.GetKeyDown(KeyCode.Space));


  }



  private void FixedUpdate()
  {
    wasGrounded = grounded;
    grounded = false;
    Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, kGroundedRadius, whatIsGround);

    foreach (Collider2D collider in colliders)
    {
      if (collider.gameObject != gameObject)
      {
        grounded = true;
      }
    }

    if (isFalling && grounded)
    {
      isFalling = false;
      //AudioManager.i.Play("Land");

    }
    //GiveTimeLeaveGround();

  }

  /* #region  Move Code */
  private void GiveTimeLeaveGround()
  {
    if (wasGrounded && !grounded)
    {
      timeAfterLeave += Time.deltaTime;
      grounded = true;
      if (timeAfterLeave > kSecondsAfterLeftGround)
      {
        timeAfterLeave = 0;
        grounded = false;
      }

    }
    else
    {
      timeAfterLeave = 0;
    }

  }

  public void Move(float move, bool jump)
  {
    if (!GameManager.canMove)
      return;


    if (move == 0 && Mathf.Abs(rb.velocity.x) > 0.1f && grounded)
      rb.AddForce(new Vector2(-rb.velocity.x * kSlowingFactor, 0f));

    Vector2 targetVelocity = new Vector2(move * speedFactor, rb.velocity.y);

    rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

    if ((move > 0 && !facingRight) || (move < 0 && facingRight))
      Flip();

    if (grounded && jump)
    {
      AudioManager.i.Play("Jump");
      grounded = false;
      rb.AddForce(new Vector2(0f, jumpForce));
    }
  }

  private void Flip()
  {
    Vector3 theScale = transform.localScale;
    facingRight = !facingRight;
    theScale.x *= -1;
    transform.localScale = theScale;
  }

  public void StopCharacter()
  {
    rb.velocity = new Vector2(0, rb.velocity.y);
  }

  /* #endregion */

  /* #region  Character Death */

  public void Die()
  {
    if (GameManager.dying)
      return;


    GameManager.canMove = false;
    StopCharacter();
    StartCoroutine(WaitUntilReachGround());


  }

  IEnumerator WaitUntilReachGround()
  {
    yield return new WaitWhile(() => !grounded);
    animator.SetTrigger("IsDead");
    GameManager.ReduceLife();
    GameManager.dying = true;

  }
  public void DeathAnimFinished()
  {
    StartCoroutine(KillPlayer());
  }

  IEnumerator KillPlayer()
  {

    SpriteRenderer sp = GetComponent<SpriteRenderer>();
    const float secondsForDisappear = 0.3f;

    setAlpha(0f);
    Vector2 insPosition = new Vector2(transform.position.x, transform.position.y - 0.14f);
    animator.SetTrigger("IsSpawned");
    transform.position = (Vector2)GameManager.spawnPoint.position;

    Instantiate(GameManager.sBodyPrefab, insPosition, Quaternion.identity);

    if (GameManager.carryingBody)
    {
      GameManager.targetBody.GetComponent<DeadBody>().carrying = false;
      GameManager.targetBody.GetComponent<SpriteRenderer>().flipX = false;
      GameManager.targetBody.transform.position = insPosition + (new Vector2(0, 0.5f));
      GameManager.carryingBody = false;
    }

    while (sp.color.a < 0.96f)
    {
      setAlpha(sp.color.a + (Time.deltaTime / (secondsForDisappear / 2)));
      yield return null;
    }
    setAlpha(1f);

    GameManager.spawned = true;
    GameManager.canMove = true;
    GameManager.dying = false;
  }


  private void setAlpha(float alpha)
  {
    SpriteRenderer sp = GameManager.character.GetComponent<SpriteRenderer>();
    sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, alpha);
  }

  /* #endregion */

  /* #region  Carry Body */


  private void CarryBody()
  {


    if (!IsThereBodyNear())
      return;

    GameObject body = GameManager.targetBody;
    DeadBody bodyScript = GameManager.targetBody.GetComponent<DeadBody>();
    GameManager.carryingBody = true;
    bodyScript.carrying = true;

  }

  private void PutBody()
  {
    if (GameManager.carryingBody && grounded)
    {
      GameManager.puttingBody = true;
      DeadBody script = GameManager.targetBody.GetComponent<DeadBody>();
      script.carrying = false;
      GameManager.carryingBody = false;
      Vector2 pos = transform.position;
      transform.position = pos + new Vector2(0, 0.63f);
      pos.y -= 0.11f;
      script.transform.position = pos;
      StartCoroutine(PuttingBodyFalse());

    }
  }
  IEnumerator PuttingBodyFalse()
  {
    yield return new WaitForSeconds(0.2f);
    GameManager.puttingBody = false;
  }

  private bool IsThereBodyNear()
  {
    Vector2 direction = facingRight ? Vector2.right : Vector2.left;
    Vector2 start = (Vector2)transform.position;
    start.y -= 0.25f;
    start.x += 0.45f * (facingRight ? 1 : -1);


    RaycastHit2D[] hits = Physics2D.RaycastAll(start, direction, 1.5f);

    bool found = false;
    List<GameObject> bodies = new List<GameObject>();
    foreach (RaycastHit2D i in hits)
    {
      if (i.collider.CompareTag("DeadBody"))
      {
        found = true;
        bodies.Add(i.collider.gameObject);
      }
    }

    if (found)
    {
      Debug.Log("Ok we got the body");
      GameManager.targetBody = getNearest(bodies);

    }

    return found;



  }

  GameObject getNearest(List<GameObject> arr)
  {
    if (arr.Capacity == 0) return arr[0];
    arr.Sort((g1, g2) => SortByDistance(g1, g2, transform.position));
    return arr[0];
  }

  int SortByDistance(GameObject p1, GameObject p2, Vector2 position)
  {
    return (int)(Vector2.Distance(p1.transform.position, position) - Vector2.Distance(p1.transform.position, position));
  }


  /* #endregion */
  private void AnimationHandler(int inputValue)
  {
    bool inAir = Mathf.Abs(rb.velocity.y) > 0.2f;
    bool isDying = GameManager.dying;
    bool isSpawned = GameManager.spawned;



    if (grounded)
      animator.SetBool("IsGrounded", true);
    else
      animator.SetBool("IsGrounded", false);

    //JumpHandler
    if (rb.velocity.y > 0.2f)
    {
      animator.SetBool("IsJumping", true);
      animator.SetBool("IsFalling", false);
    }
    else if (rb.velocity.y < -0.2f)
    {
      isFalling = true;
      animator.SetBool("IsJumping", false);
      animator.SetBool("IsFalling", true);
    }
    else
    {
      animator.SetBool("IsJumping", false);
      animator.SetBool("IsFalling", false);
    }

    if (inAir)
      return;

    if (!GameManager.canMove || inputValue == 0)
    {
      animator.SetBool("IsRunning", false);
    }
    else
    {
      animator.SetBool("IsRunning", true);
    }

  }

}