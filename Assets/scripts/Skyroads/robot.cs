using System.Collections;
using System.Collections.Generic;
using OpenCVForUnityExample;
using UnityEngine;

public class robot : MonoBehaviour
{
    [SerializeField]
    float

            _xMin,
            _xMax,
            smooth = 0.2f,
            dSuelo,
            hMin,
            h,
            difSalto = 15,
            vSalto = 20,
            g = 20;

    float

            izq = -12,
            der = 12,
            cen = 0,
            c,
            p,
            vy;

    public int vel;
    public int sensibilidad;

    Vector3 Q;

    [SerializeField]
    LayerMask suelo;

    public bool corriendo;

    public GameObject controlador;
    public GameObject sonidoPila;

    public bool
            tocaSuelo,
            sobreObstaculo,
            salta,
            saltando,
            colision,
            meta;


    ArUcoWebCamExampleTexture arucoDetector;

    // Start is called before the first frame update
    void Start()
    {
        _xMin = -15;
        _xMax = 15;
        arucoDetector = (ArUcoWebCamExampleTexture)FindObjectOfType(typeof(ArUcoWebCamExampleTexture));
        transform.position = new Vector3(cen, -1.8f, 9.2f);
        Q = transform.position;
        saltando = false;
        corriendo = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Cuando pasa el tiempo la cam pone a true corriendo
        if (corriendo)
        {
            tocaSuelo = Physics.Raycast(transform.position, -Vector3.up, dSuelo, suelo);
            if (tocaSuelo & saltando & vy <= 0)
            {
                saltando = false;
                vy = 0;
            }
                   
            if (!saltando)
            {
                p = arucoDetector.coordenadasPantalla.x;
                if (p < c - sensibilidad)
                {
                    Q =
                        new Vector3(izq,
                            transform.position.y,
                            transform.position.z);
                }
                if (p > c + sensibilidad)
                {
                    Q =
                        new Vector3(der,
                            transform.position.y,
                            transform.position.z);
                }
                if (Mathf.Abs(p - c) <= sensibilidad)
                {
                    Q =
                        new Vector3(cen,
                            transform.position.y,
                            transform.position.z);
                }
                //Si me desplazo y detecto colisión en los laterales no se me permite avanzar hacia ese lado
                colision = Physics.CheckBox(Vector3.Lerp(transform.position, Q, smooth) + 3*transform.up, new Vector3(3,3,1.5f), Quaternion.identity, LayerMask.GetMask("obstaculos"));
                if(!colision)
                    transform.position = Vector3.Lerp(transform.position, Q, smooth);              
            }

            //Condicion salto
            h = arucoDetector.coordenadasPantalla.y - 240;
            salta = (h < hMin);
            if (salta & tocaSuelo & !saltando)
            {
                saltando = true;
                vy = vSalto;
            }

            if (saltando)
            {
                vy -= g * Time.deltaTime;
                //Comprobar si estoy sobre un obstáculo, de serlo asi dejo de caer
                sobreObstaculo = Physics.Raycast(transform.position, -Vector3.up, dSuelo, LayerMask.GetMask("obstaculos"));
                if(sobreObstaculo)
                {
                    vy = 0;
                }

                transform.Translate(vy * Vector3.up * Time.deltaTime, Space.World);
            }

            if (transform.position.y < -1.8f || !sobreObstaculo && !tocaSuelo && !saltando)
            {
                transform.position = new Vector3(transform.position.x, -1.8f, transform.position.z);
            }

            //Si me desplazo y detecto colisión delante no se me permite avanzar hacia delante
            colision = Physics.CheckBox(transform.position + 3*transform.forward + 3*transform.up, new Vector3(3,3,1.5f), Quaternion.identity, LayerMask.GetMask("obstaculos"));
            if(!colision)
                transform.Translate(-vel * Vector3.forward * Time.deltaTime,Space.World);

            //Comprobar si he capturado un item
            RaycastHit hit;
            if (Physics.BoxCast(transform.position, new Vector3(3,3,1.5f), transform.position + 3*transform.forward + 3*transform.up, out hit, transform.rotation, 3, LayerMask.GetMask("item")))
            {
                ActivarSonidoItem();
                controlador.SendMessage("IncrementoPuntuacion", 10);
                hit.transform.gameObject.SetActive(false);
            }
            
            //Comprobar si se ha llegado a la meta
            if(Physics.CheckBox(transform.position + 3*transform.forward + 3*transform.up, new Vector3(3,3,1.5f), Quaternion.identity, LayerMask.GetMask("meta")))
            {
                corriendo = false;
                meta = true;
            }
                
        }
    }

    void centra()
    {
        hMin = arucoDetector.coordenadasPantalla.y - 240 - difSalto;
        c = arucoDetector.coordenadasPantalla.x;
    }

    public void iniciar()
    {
        centra();
        corriendo = true;
    }

    public void ActivarSonidoItem()
    {
        sonidoPila.SetActive(true);
        Invoke("DesactivarSonidoItem", 1);
    }

    public void DesactivarSonidoItem()
    {
        sonidoPila.SetActive(false);
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawLine(transform.position, transform.position - Vector3.up*dSuelo);

        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireCube(transform.position + 3*transform.forward + 3*transform.up, new Vector3(6,6,3));  

        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireCube(transform.position + 3*transform.up, new Vector3(6,6,3));  
    }
}
