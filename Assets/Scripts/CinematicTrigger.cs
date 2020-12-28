using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicTrigger : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other) {
        GameManager.isCinematic = true;
    }
}
