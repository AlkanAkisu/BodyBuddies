using UnityEngine;
using UnityEngine.SceneManagement;
class GameManager : MonoBehaviour
{

  public static GameObject character;
  public static bool canMove, doorMoving, justTeleported;
  public static bool leftBlocked, rightBlocked;
  public static bool dying, spawned;
  public static bool puttingBody, carryingBody;

  public static int lastScene;
  [SerializeField] private int _lastScene = 8;

  public static bool plugNearby;
  public static bool reading;
  public static int DeathLimit, currentLives;
  [SerializeField] private int _deathLimit = 5;

  public static bool isTutorial;
  public static bool isCinematic;
  [SerializeField] private bool _isTutorial = false;
  public static Transform spawnPoint;
  private static GameObject[] doors;
  private static Character chScript;
  public GameObject bodyPrefab;
  public static GameObject sBodyPrefab;
  public static GameObject targetBody;

  void Awake()
  {
    character = GameObject.FindGameObjectWithTag("Character");
    chScript = character.GetComponent<Character>();
    canMove = true;
    doorMoving = false;
    justTeleported = false;
    spawnPoint = GameObject.Find("Spawn Point").transform;
    sBodyPrefab = bodyPrefab;
    DeathLimit = _deathLimit;
    currentLives = _deathLimit;
    isTutorial = _isTutorial;
    lastScene = _lastScene;
    reading = false;
    plugNearby = false;
    dying = false;
    spawned = false;
    puttingBody = false;
    carryingBody = false;
    isCinematic = false;
    Debug.Log("awake");
  }
  void Start()
  {
    AudioManager.i.Play("Theme");
  }

  public void StartGame()
  {
    SceneManager.LoadScene(1, LoadSceneMode.Single);
  }

  void Update()
  {
    CheckDoors();
  }

  private void CheckDoors()
  {
    doors = GameObject.FindGameObjectsWithTag("Door");

    doorMoving = false;
    foreach (GameObject doorObj in doors)
    {
      if (doorObj.GetComponent<Door>().isActive())
      {
        doorMoving = true;
      }

    }

  }

  public static void CharacterDied()
  {
    if (!dying)
    {
      chScript.Die();
      dying = true;
    }
  }

  public static void SceneFinished()
  {
    LevelManager.i.NextLevel();
    Debug.Log("Scene Finished");
  }

  public static void ReduceLife()
  {
    if (isTutorial) return;
    currentLives--;
    if (currentLives <= 0)
    {
      //Refresh the scene
      LevelManager.i.RestartScene();
    }
  }





}