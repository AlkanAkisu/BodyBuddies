using UnityEngine;
using UnityEngine.SceneManagement;

class LevelManager : MonoBehaviour
{
  private static LevelManager _instance;
  public static LevelManager i { get { return _instance; } }

  private bool flag;

  void Awake()
  {
    _instance = this;
    flag = false;
  }


  public void NextLevel()
  {
    int next = SceneManager.GetActiveScene().buildIndex + 1;

    if (!flag && GameManager.lastScene != (next - 1))
    {
      Debug.Log("Load Scene");
      SceneManager.LoadScene(next, LoadSceneMode.Single);
      flag = true;
    }
  }

  public void RestartScene()
  {
    if (GameManager.isTutorial || GameManager.reading)
      return;
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
  }


}