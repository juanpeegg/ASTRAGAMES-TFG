using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparar : MonoBehaviour
{
    public GameObject bola, efecto;

    public GameObject bolaPrefab;

    private float tiempoTranscurrido = 0.0f;

    public float tiempoTotal = 2.0f;

    public Barco barco;
    // Start is called before the first frame update
    void Start()
    {
        barco = GetComponent<Barco>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!barco.control.isVictoria() && barco.arrancado)
        {
            tiempoTranscurrido += Time.deltaTime;
            if (tiempoTotal < tiempoTranscurrido && !barco.girando)
            {
                //se dispara cada cierto tiempo
                GameObject nuevaBola = Instantiate(bolaPrefab);
                nuevaBola.transform.position = bola.transform.position;
                nuevaBola.transform.rotation = bola.transform.rotation;
                nuevaBola.transform.parent=transform;
                Vector3 direction;
                //con la tangente calculamos la componente x
                if (barco.nivel == 0)
                {
                    direction = new Vector3(Mathf.Tan(-barco.orientacionesPosiciones[barco.posActual] * Mathf.Deg2Rad), 0f, -1).normalized;
                }
                else
                {
                    direction = new Vector3(Mathf.Tan(-barco.rot_anterior * Mathf.Deg2Rad), 0f, -1).normalized;
                }
                
                nuevaBola.GetComponent<BolaMueve>().lanzarDireccion(direction);

                efecto.SetActive(true);
                Invoke("fin",0.07f);
                barco.control.activarCanon();
                tiempoTranscurrido = 0f;
            }
        }     
    }

    void fin(){
        efecto.SetActive(false);
    }
}
