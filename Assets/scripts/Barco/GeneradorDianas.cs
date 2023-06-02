using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorDianas : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject diana;

    public int nDianas = 0;

    private ControladorBarco control;

    private int[] posiciones = { -40, -20, 0, 20, 40 };

    private bool[] ocupadas = { false, false, false, false, false };

    private float tiempoT = 0f;

    private float tiempoFinal = 5f;

    public bool quietas = true;

    void Start()
    {
        control = GameObject.Find("ControladorUI").GetComponent<ControladorBarco>();
    }

    // Update is called once per frame
    void Update()
    {
        if (nDianas < 5 && control.barco.arrancado)
        {
            //Cada 5 segundos mínimo desde que apareció la última y con una posibilidad entre 1000
            int aleatorio = Random.Range(0, 200);
            if (aleatorio == 0 && tiempoT > tiempoFinal)
            {
                tiempoT = 0f;
                int variacion = Random.Range(0, 5);
                while (ocupadas[variacion])
                {
                    variacion = Random.Range(0, 5);
                }
                ocupadas[variacion] = true;
                nDianas++;
                GameObject d = Instantiate(diana);
                d.transform.position = control.barco.transform.GetChild(0).transform.position + new Vector3(posiciones[variacion], 7, -75);
                if (!quietas)
                    d.transform.SetParent(control.barco.transform);
                d.GetComponent<Diana>().posicion = variacion;
            }
        }
        tiempoT += Time.deltaTime;

    }

    public void golpeada(int posicion)
    {
        nDianas--;
        ocupadas[posicion] = false;
    }
}
