using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnityExample;

public class Barco : MonoBehaviour
{

    public Dictionary<int, float> orientacionesPosiciones = new Dictionary<int, float>()
    {
    {0, -45f},
    {1, 0f},
    {2, 45f},
    };

    //Movimiento barco y cañón
    public Vector3 launchDirection = -Vector3.forward;
    public float v;
    public GameObject canon;
    public bool girando = false;
    private float tiempoTranscurrido = 0.0f;
    public float tiempoTotal = 1.0f;
    private Quaternion rotacionInicial;
    private Quaternion rotacionFinal;
    public int posActual = 1;

    public ControladorBarco control;
    public bool arrancado = false;

    //Input
    ArUcoWebCamExampleTexture arucoDetector;
    float p,h;
    float c = 640 / 2;
    float hMin = 480 / 2 - 100;
    public int sensibilidad;
    public float _smooth;
    [SerializeField] [Range(1, 320)] int intervalo = 160;
    public float rot_anterior = 0f;

    //Nivel de 3 cañones o nivel libre
    public int nivel;


    // Start is called before the first frame update
    void Start()
    {
        canon = transform.GetChild(0).gameObject;
        control = GameObject.Find("ControladorUI").GetComponent<ControladorBarco>();
        arucoDetector = (ArUcoWebCamExampleTexture)FindObjectOfType(typeof(ArUcoWebCamExampleTexture));

    }

    // Update is called once per frame
    void Update()
    {
        if (arrancado)
        {
            if (nivel == 0)
            {
                movimiento3canones();
            }
            else
            {         
                movimientoCanonLibre();
            }
                    
            /*h = arucoDetector.coordenadasPantalla.y ;
            bool hayGranada = (h < hMin);
            if(hayGranada && posActual != 1 && posibilidadGranada)
            {
                lanzarGranada();
            }*/
            
            transform.position -= Vector3.forward * v * Time.deltaTime;
        }

        if (!arrancado && Input.GetKeyDown(KeyCode.Space))
        {
            arrancado = true;
        }

        if (arrancado && Input.GetKeyDown(KeyCode.F))
        {
            v = 100;
        }

    }

    public void movimientoCanonLibre()
    {
        //cojo la posicion de la bola
        launchDirection = new Vector3(Mathf.Tan(-rot_anterior * Mathf.Deg2Rad), 0f, -1).normalized;

        //Debug.Log(launchDirection);
        Vector3 posicion = transform.GetChild(0).GetChild(0).position;
        Vector3 endPosition = posicion + launchDirection * 80;

        float input = (arucoDetector.coordenadasPantalla.x - 320 ) / intervalo;

        float input2 = input * sensibilidad * 0.6f;

        float clampedRotation = Mathf.Clamp(input2, -45f, 45f);

        if (-clampedRotation < 0)
        {
            posActual = 0;
        }
        else posActual = 2;

        Vector3 newRotation = new Vector3(canon.transform.rotation.eulerAngles.x, canon.transform.rotation.eulerAngles.y - rot_anterior, -clampedRotation);
        Quaternion newRot = Quaternion.Euler(newRotation);
        canon.transform.rotation = Quaternion.Lerp(canon.transform.rotation, newRot, _smooth);

        rot_anterior = -clampedRotation;

    }

    public void movimiento3canones()
    {
        //cojo la posicion de la bola
        Vector3 posicion = transform.GetChild(0).GetChild(0).position;
        Vector3 endPosition = posicion + launchDirection * 50;


        if (girando)
        {         
            tiempoTranscurrido += Time.deltaTime;
            float t = Mathf.Clamp01(tiempoTranscurrido / tiempoTotal);
            canon.transform.rotation = Quaternion.Lerp(rotacionInicial, rotacionFinal, t);

            if (t == 1.0f)
            {
                girando = false;          
            }
        }

        p = arucoDetector.coordenadasPantalla.x;
        if (p < c - sensibilidad && posActual != 2 && !girando)
        {
            //me muevo a la derecha
            moverDerecha();

        }
        else if (p > c + sensibilidad && posActual != 0 && !girando)
        {
            //me muevo a la izquierda
            moverIzquierda();
        }
        else if (Mathf.Abs(p - c) <= sensibilidad && posActual != 1 && !girando)
        {
            //me muevo al centro
            moverCentro();
        }
    }

    public void establecerValores(int frecuenciaDisparo, int velBarco, int sensibilidad, int nivel)
    {
        v = velBarco;
        this.sensibilidad = sensibilidad;
        this.nivel = nivel;
        switch (frecuenciaDisparo)
        {
            case 0:
                this.gameObject.GetComponent<Disparar>().tiempoTotal = 0.2f;
                break;
            case 1:
                this.gameObject.GetComponent<Disparar>().tiempoTotal = 0.5f;
                break;
            case 2:
                this.gameObject.GetComponent<Disparar>().tiempoTotal = 1f;
                break;
            case 3:
                this.gameObject.GetComponent<Disparar>().tiempoTotal = 1.5f;
                break;
            case 4:
                this.gameObject.GetComponent<Disparar>().tiempoTotal = 2f;
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("meta"))
        {
            control.heGanado();
        }
    }

    public void moverIzquierda()
    {
        //me muevo a la izquierda
        girando = true;
        tiempoTranscurrido = 0.0f;
        rotacionInicial = canon.transform.rotation;
        if (posActual == 1)
            rotacionFinal = Quaternion.Euler(rotacionInicial.eulerAngles.x, rotacionInicial.eulerAngles.y, -45f);
        else if (posActual == 2)
            rotacionFinal = Quaternion.Euler(rotacionInicial.eulerAngles.x, rotacionInicial.eulerAngles.y, -90f);
        posActual = 0;
        launchDirection = new Vector3(1, 0, -1);
    }

    public void moverDerecha()
    {
        //me muevo a la derecha
        girando = true;
        tiempoTranscurrido = 0.0f;
        rotacionInicial = canon.transform.rotation;
        if (posActual == 1)
            rotacionFinal = Quaternion.Euler(rotacionInicial.eulerAngles.x, rotacionInicial.eulerAngles.y, 45f);
        else if (posActual == 0)
            rotacionFinal = Quaternion.Euler(rotacionInicial.eulerAngles.x, rotacionInicial.eulerAngles.y, 90f);
        posActual = 2;
        launchDirection = new Vector3(-1, 0, -1);
    }

    public void moverCentro()
    {
        //me muevo al centro
        girando = true;
        tiempoTranscurrido = 0.0f;
        rotacionInicial = canon.transform.rotation;
        if (posActual == 0)
            rotacionFinal = Quaternion.Euler(rotacionInicial.eulerAngles.x, rotacionInicial.eulerAngles.y, 45f);
        else if (posActual == 2)
            rotacionFinal = Quaternion.Euler(rotacionInicial.eulerAngles.x, rotacionInicial.eulerAngles.y, -45f);
        posActual = 1;
        launchDirection = new Vector3(0, 0, -1);
    }
}
