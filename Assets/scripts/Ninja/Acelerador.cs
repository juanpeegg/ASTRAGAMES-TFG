using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acelerador : MonoBehaviour
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
            collision.collider.gameObject.GetComponent<Mover4>().acelerarCaida(20);
            control.destruirFila(transform.parent.gameObject);
            control.IncrementoPuntuacion(2);
            control.activarSonidoCaigo();
            Destroy(this.gameObject);
        }
    }
}
