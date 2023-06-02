using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farolillo : MonoBehaviour
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
        // si toco acelerador, aumento velocidad y salgo disparado
        if (collision.collider.tag == "Player")
        {    
            control.IncrementoPuntuacion(3);
            control.activarSonidoFarolillo();
            Destroy(this.transform.parent.gameObject);
        }
    }
}
