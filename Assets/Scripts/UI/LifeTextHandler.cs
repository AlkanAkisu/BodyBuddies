using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LifeTextHandler : MonoBehaviour
{
  // Start is called before the first frame update
  TextMeshProUGUI textMesh;
  void Awake()
  {
    textMesh = GetComponent<TextMeshProUGUI>();
  }
  void Update()
  {
    textMesh.text = GameManager.currentLives + " Lives Left";
  }
}
