using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasselbandeSetPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.GetChild(1).transform.position = this.transform.GetChild(0).transform.position;
    }
}
