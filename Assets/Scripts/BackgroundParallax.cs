using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    private Vector2 startPos;

    [SerializeField ]private GameObject cam;

    [Range(0, 1)]
    [SerializeField] private float parallaxEffect;

    void Start()
    {
        startPos = transform.position;
    }


    void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(startPos.x + distance, startPos.y, transform.position.z);
    }
}
