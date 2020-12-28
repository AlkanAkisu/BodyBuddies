using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
class Dialogue : MonoBehaviour
{
  public TextMeshProUGUI textDisplay;
  public float speed = 0.03f, waitTime = 0.6f, secondsForPopup = 1.2f;
  private int index = 0;

  [SerializeField] private GameObject panel;
  private bool isTerminalFinished;

  private static Dialogue _instance;
  public static Dialogue i { get { return _instance; } }

  void Awake()
  {
    panel.SetActive(false);
    _instance = this;
  }

  public void ReadFloppyDisk(FloppyDisk disk)
  {
    isTerminalFinished = false;
    StartCoroutine(ShowTerminal());
    GameManager.canMove = false;
    StartCoroutine(ReadFloppyDiskEnum(disk.sentences));
  }

  IEnumerator ShowTerminal()
  {
    panel.SetActive(true);
    RectTransform rect = panel.GetComponent<RectTransform>();
    rect.anchoredPosition = new Vector2(0, -400);
    while (rect.anchoredPosition.y < 0)
    {
      rect.anchoredPosition = new Vector2(0, rect.anchoredPosition.y + 400 * Time.deltaTime / secondsForPopup);
      yield return null;
    }
    isTerminalFinished = true;
  }

  IEnumerator ReadFloppyDiskEnum(string[] sentences)
  {
    bool typed = false;
    textDisplay.text = "";
    yield return new WaitWhile(() => !isTerminalFinished);
    foreach (char c in "Reading floppy disk")
    {
      textDisplay.text += c;
      if (!typed)
      {
        AudioManager.i.Play("Type");
        typed = true;
      }
      else
      {
        typed = false;
      }
      yield return new WaitForSeconds(speed * 1.5f);
    }
    foreach (char c in "....")
    {
      textDisplay.text += c;
      if (!typed)
      {
        AudioManager.i.Play("Type");
        typed = true;
      }
      else
      {
        typed = false;
      }
      yield return new WaitForSeconds(0.5f);
    }
    textDisplay.text = "";
    yield return new WaitForSeconds(speed);
    string randomFloppyNum = "Floppy disk #" + (int)Random.Range(100, 300);
    foreach (char c in randomFloppyNum)
    {
      textDisplay.text += c;
      if (!typed)
      {
        AudioManager.i.Play("Type");
        typed = true;
      }
      else
      {
        typed = false;
      }
      yield return new WaitForSeconds(speed);
    }
    yield return new WaitForSeconds(waitTime);
    textDisplay.text += '\n';
    foreach (string str in sentences)
    {
      foreach (char c in str.ToCharArray())
      {
        textDisplay.text += c;

        if (!typed)
        {
          AudioManager.i.Play("Type");
          typed = true;
        }
        else
        {
          typed = false;
        }
        yield return new WaitForSeconds(speed);
      }
      yield return new WaitForSeconds(waitTime);
      textDisplay.text += '\n';
    }
    yield return new WaitForSeconds(waitTime);
    RectTransform rect = panel.GetComponent<RectTransform>();
    while (rect.anchoredPosition.y > -500)
    {
      rect.anchoredPosition = new Vector2(0, rect.anchoredPosition.y - 500 * Time.deltaTime / secondsForPopup);
      yield return null;
    }
    panel.SetActive(false);
    GameManager.reading = false;
    yield return new WaitForSeconds(1f);
    GameManager.SceneFinished();
  }

}