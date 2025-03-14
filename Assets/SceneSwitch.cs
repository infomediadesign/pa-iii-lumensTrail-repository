using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    [SerializeField] private int sceneIndex;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            MovementBaseState.movementSpeedModifier = 1;
            if (sceneIndex == 3) 
            {
                SoundManager.PlayLevel3Music();
                SoundManager.currentLevel = 2;
            }
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
