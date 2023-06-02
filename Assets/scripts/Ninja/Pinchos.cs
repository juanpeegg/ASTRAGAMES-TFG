using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinchos : MonoBehaviour
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
        // si toco pincho resto puntos e inicio cuenta atras.
        // durante 2 segundos, eres inmune
        //Debug.Log("detecto");
        misSensores2 sensor = collision.collider.gameObject.GetComponent<misSensores2>();
        if (collision.collider.tag == "Player" && sensor.golpeDisponible)
        {
            control.IncrementoPuntuacion(-2);
            sensor.golpeDisponible = false;
            control.activarParpadeo(true);
            control.activarSonidoDano();
            
        }
    }


}
