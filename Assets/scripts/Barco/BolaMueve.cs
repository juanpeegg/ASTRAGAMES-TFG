using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaMueve : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject explosion;

    Vector3 direccionActual;

    bool enMov = false;

    float v = 75f;

    public ControladorBarco control;
    void Start()
    {
        control = GameObject.Find("ControladorUI").GetComponent<ControladorBarco>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enMov)
        {
            transform.position += v * direccionActual * Time.deltaTime;
        }
    }

    public void lanzarDireccion(Vector3 direction)
    {
        direccionActual = direction;
        enMov = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemigo")
        {
            //suma puntuacion
            control.IncrementoPuntuacion(1);
            control.activarDano();
            //en vez del destroy se puede poner una animacion de caida al suelo herido
            GameObject miEfecto=Instantiate(explosion);
            miEfecto.transform.position=collision.gameObject.transform.position;
            Destroy(collision.gameObject);       
        }
        if (collision.gameObject.tag == "Diana")
        {
            //suma puntuacion
            control.IncrementoPuntuacion(3);
            control.activarDiana();
            control.generadorDiana.golpeada(collision.gameObject.GetComponent<Diana>().posicion);
            //en vez del destroy se puede poner una animacion de caida al suelo herido
            GameObject miEfecto = Instantiate(explosion);
            miEfecto.transform.position = collision.gameObject.transform.position;
            Destroy(collision.gameObject);
             
        }
        if (collision.gameObject.tag != "Bola" && collision.gameObject.tag  != "Barco")
            Destroy(this.gameObject);
    }
}
