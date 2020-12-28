using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class CinematicCharacter : MonoBehaviour
{

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
  private int inputValue;
  [SerializeField] private Camera cam;
  [SerializeField] Transform leftEnd, rightEnd, portalEnterance, portal;
  SpriteRenderer sp;

  void Awake()
  {
    animator = GetComponent<Animator>();
    rb = GetComponent<Rigidbody2D>();
    sp = GetComponent<SpriteRenderer>();
  }

  void Update()
  {




    if (!GameManager.canMove)
    {
      rb.velocity = new Vector2(0, rb.velocity.y);
      return;
    }
    CameraHandler();

    if (!GameManager.isCinematic)
    {
      inputValue = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
      inputValue = GameManager.rightBlocked && inputValue == 1
      ? 0
      : GameManager.leftBlocked && inputValue == -1
      ? 0
      : inputValue;

    }
    else
    {


      StartCoroutine(Cinematic());
    }

    AnimationHandler(inputValue);
    Move(inputValue);


  }

  private IEnumerator Cinematic()
  {
    inputValue = 1;
    yield return new WaitUntil(() => transform.position.x > portalEnterance.position.x);

    inputValue = 0;
    rb.velocity = Vector2.zero;
    animator.SetBool("IsRunning", false);
    yield return new WaitForSeconds(1.5f);
    rb.isKinematic = true;
    float passedTime = 0;
    Color color = sp.color;
    float seconds = 4f;
    float xDiff = portal.position.x - transform.position.x;
    float yDiff = portal.position.y - transform.position.y;
    Vector2 pos = transform.position;
    while (passedTime < seconds)
    {
      color.a -= Time.deltaTime / (seconds * 1.5f);
      pos.x += xDiff * Time.deltaTime / seconds;
      pos.y += yDiff * Time.deltaTime / seconds;
      passedTime += Time.deltaTime;
      sp.color = color;
      transform.position = pos;
      yield return null;
    }
    GameFinished();
  }

  private void GameFinished()
  {
    GameManager.isCinematic = false;
    GameManager.canMove = false;
    int next = SceneManager.GetActiveScene().buildIndex + 1;

    if (GameManager.lastScene != (next - 1))
    {
      Debug.Log("Load Scene");
      SceneManager.LoadScene(next, LoadSceneMode.Single);
    }
  }

  private void CameraHandler()
  {
    Vector3 camPos = cam.transform.position;
    camPos.x = transform.position.x;
    if (camPos.x <= leftEnd.position.x)
      camPos.x = leftEnd.position.x;
    else if (camPos.x >= rightEnd.position.x)
      camPos.x = rightEnd.position.x;
    cam.transform.position = camPos;
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

    //GiveTimeLeaveGround();

  }



  public void Move(float move)
  {
    if (!GameManager.canMove)
      return;
    if (move == 0 && Mathf.Abs(rb.velocity.x) > 0.1f && grounded)
      rb.AddForce(new Vector2(-rb.velocity.x * kSlowingFactor, 0f));

    Vector2 targetVelocity = new Vector2(move * speedFactor, rb.velocity.y);

    rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

    if ((move > 0 && !facingRight) || (move < 0 && facingRight))
      Flip();


  }

  private void Flip()
  {
    Vector3 theScale = transform.localScale;
    facingRight = !facingRight;
    theScale.x *= -1;
    transform.localScale = theScale;
  }

  private void AnimationHandler(int inputValue)
  {

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