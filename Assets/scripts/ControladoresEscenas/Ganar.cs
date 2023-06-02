using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ganar : MonoBehaviour
{
    private ControladorNinja control;
    // Start is called before the first frame update
    void Start()
    {
        control = GameObject.Find("ControladorUI").GetComponent<ControladorNinja>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            control.heGanado();
        }
    }
}
