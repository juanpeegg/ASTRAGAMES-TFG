using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mover4 : MonoBehaviour
{
    private ControladorNinja control;
    [SerializeField]
    misSensores2 sensores;

    [SerializeField]
    float vYMaxima, g, vSalto;

    Vector3 P;

    RaycastHit hit;
    float vy = 0;

    bool iniciado = false;


    // Start is called before the first frame update
    void Start()
    {
        sensores = GetComponent<misSensores2>();
        control = GameObject.Find("ControladorUI").GetComponent<ControladorNinja>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            iniciado = true;
            control.torre.GetComponent<RotacionEscalones>().arrancado = true;
        }
        if (iniciado)
        {
            P = transform.position;
            //Debug.Log(control.seguidos);
            // Cae y salta
            if (!sensores.suelo)
            {
                vy -= g * Time.deltaTime;
            }
            else
            {
                vy = vSalto;
                if (control.seguidos >= 3)
                {
                    //Debug.Log("rebotar en fila y rojo");
                    Invoke("romperEscalon", 0.5f);
                }
                control.seguidos = 0;
            }

            if (sensores.victoria)
            {
                vy = 0;
                control.heGanado();
            }


            P += vy * Vector3.up * Time.deltaTime;

            transform.position = P;

        }


    }

    public void acelerarCaida(int caida)
    {
        vy -= caida;
    }

    private void romperEscalon()
    {
        control.romperEscalon();
    }
}
