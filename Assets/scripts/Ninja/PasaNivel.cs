using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasaNivel : MonoBehaviour
{
    public bool pasado = false;

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

    void OnTriggerEnter(Collider other)
    {
        if (!pasado && other.tag == "Player")
        {
            pasado = true;
            // acelerar caida del personaje 
            // ordenar destruccion de fila. el padre de pasar el nivel
            //Debug.Log(this.gameObject.transform.parent.gameObject);
            control.destruirFila(this.gameObject.transform.parent.gameObject);
            // sumar puntuacion
            control.IncrementoPuntuacion(1);
            // bajar la cámara a la siguiente fila se hace por estar emparentado
            // Un seguido más.
            control.seguidos++;

            //segun cuantos seguidos lleve, acelero mas o menos la caida
            if (control.seguidos == 1)
            {
                control.acelerarCaida(8);
            }
            else control.acelerarCaida(4);
            
        }

    }

}
