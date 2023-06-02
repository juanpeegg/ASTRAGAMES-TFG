using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float tiempoReal = 5f;
    private float tiempoActual = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tiempoActual += Time.deltaTime;
        if (tiempoActual > tiempoReal)
        {
            Destroy(this.gameObject);
        }
    }
}
