using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneTrigger : MonoBehaviour
{
    [SerializeField] private Sprite image1;
    [SerializeField] private Sprite image2;
    public UnityEngine.UI.Image canvasImage;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().CutsceneStopWalking();
            StartCoroutine(Cutscene());
        }
    }

    IEnumerator Cutscene()
    {
        canvasImage.sprite = image1;
        canvasImage.enabled = true;
        yield return new WaitForSeconds(3);
        canvasImage.sprite = image2;
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(4);
    }
}
