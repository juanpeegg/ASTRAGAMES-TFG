using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilaRota : MonoBehaviour
{
    public float velocidadRotacion = 20f;
    public float maximoAngulo = 90f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float t = Mathf.PingPong(Time.time * velocidadRotacion, maximoAngulo) / maximoAngulo;
        transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 180, 0), t);


    }
}
