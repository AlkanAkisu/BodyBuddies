using UnityEngine;

class Utils : MonoBehaviour
{



  public static bool IsCharacter(Collider2D other)
  {
    return other.gameObject == GameManager.character;
  }
  public static bool IsCharacter(Collision2D other)
  {
    return other.gameObject == GameManager.character;
  }

  public static bool IsBody(Collider2D other)
  {
    return other.gameObject.CompareTag("DeadBody");
  }
  public static bool IsBody(Collision2D other)
  {
    return other.gameObject.CompareTag("DeadBody");
  }
}