using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using OpenCVForUnityExample;
using ScoresDatabase;
using ConfigurationDatabase;
using OpenCVForUnity.CoreModule;


public class ControladorBurbuja : ControladorUIVideojuegos
{
    private float tRestante = 60f;
    public Text tiempo;

    public GameObject sonidoAzulG, sonidoRojaG, sonidoCompletoG;

    public int nAzules = 0;

    public GenerarBurbujas generador;

    public int contadorPuntos = 0;

    PoseTextureWebCamExample openPose;

    public int formato = 0;

    List<int> usables = new List<int>();

    public bool empezado = false;

    public GameObject cilindros;

    public Text cuenta;

    public int nCuenta = 3;

    void Start()
    {
        juego = "Burbujas";
        base.Start();

        for (int i = 0; i < 18; i++)
        {
            usables.Add(i);
        }

        generador = GameObject.Find("ControladorUI").GetComponent<GenerarBurbujas>();

        openPose = (PoseTextureWebCamExample)FindObjectOfType(typeof(PoseTextureWebCamExample));

        cilindros = GameObject.Find("Cilindros");

        //Si tenemos una configuracion concreta 
        BurbujasDb configBurbujasDb = new BurbujasDb();
        BurbujasEntity config = configBurbujasDb.getConfigurationByName(jugador);
        configBurbujasDb.close();

        if (config._juego != "Error")
        {
            //Inicializamos los valores segun nuestra configuracion guardada
            tRestante = config._tiempo;
            formato = config._formato;
        }
        else //si no, valores por defecto
        {
            tRestante = 60;
            formato = 0;
        }

        actualizarUsables();

    }

    public bool heGanado()
    {
        return victoria;
    }

    void Update()
    {
        base.Update();

        if(empezado && !victoria)
            tRestante -= Time.deltaTime;

        if (tRestante <= 0 && !victoria)
        {
            tiempo.text = 0.ToString();
            canvaGanar.SetActive(true);

            //Si el usuario está registrado se guarda su puntuación
            if (loadedData != null)
            {
                puntuacionesDb = new PuntuacionesDb();
                puntuacionesDb.addData(new PuntuacionesEntity(juego, jugador, puntos));
                puntuacionesDb.close();
            }
            victoria = true;
        }
        else
            tiempo.text = ((int)tRestante).ToString();

        //vuelvo a pedir al generador que genere otra pantalla
        if (nAzules == 0)
        {
            generador.regeneraInvocando();
        }

        //Para regenerar la pantalla
        if (Input.GetKey(KeyCode.P))
        {
            Destroy(GameObject.Find("Burbujas"));
            nAzules = 0;
        }

        //Para pasarse la pantalla
        if (Input.GetKey(KeyCode.D))
        {
            GameObject b = GameObject.Find("Burbujas");

            for (int i = 0; i < b.transform.childCount; i++)
            {
                if (b.transform.GetChild(i).gameObject.GetComponent<Burbuja>().esAzul)
                    b.transform.GetChild(i).gameObject.GetComponent<Burbuja>().explotar();
            }
        }

        controlarColisiones();

        if (Input.GetKey(KeyCode.Space))
        {
            empezado = true;
        }

    }

    public void controlarColisiones()
    {
        if (openPose.points.Count > 0)
        {
            for (int i = 0; i < openPose.POSE_PAIRS.GetLength(0); i++)
            {
                //Cojo la primera pareja.
                string partFrom = openPose.POSE_PAIRS[i, 0];
                string partTo = openPose.POSE_PAIRS[i, 1];

                //Cojo las identidades correspondientes de la primera pareja.
                int idFrom = openPose.BODY_PARTS[partFrom];
                int idTo = openPose.BODY_PARTS[partTo];

                //Elimino todas las burbujas entre los 2 puntos de control
                if (openPose.points[idFrom] != null && openPose.points[idTo] != null && openPose.points[idFrom].x != -1 &&
                    openPose.points[idFrom].y != -1 && openPose.points[idTo].x != -1 && openPose.points[idTo].y != -1 && usable(idFrom) && usable(idTo))
                {
                    //Debug.Log(partFrom + "-" + partTo);
                    Vector3 coord1 = transformarCoordenada(openPose.points[idFrom]);
                    Vector3 coord2 = transformarCoordenada(openPose.points[idTo]);

                    //Coloco el cilindro en el punto medio
                    GameObject actual = cilindros.transform.Find(i.ToString()).gameObject;
                    actual.SetActive(true);
                    actual.transform.position = new Vector3((coord1.x+coord2.x)/2,(coord1.y+coord2.y)/2,-41);

                    //Doy largura = distancia y doy anchura proporcional a la distancia
                    float anchura = calcularAnchura(distancia(coord1,coord2));
                    actual.transform.localScale = new Vector3(anchura,distancia(coord1, coord2)/2, anchura);

                    //Coloco ahora la rotacion que será en dirección de la coordenada 1 a la 2
                    actual.transform.LookAt(coord2);

                    Quaternion newRotation = Quaternion.Euler(actual.transform.rotation.eulerAngles.x + 90, actual.transform.rotation.eulerAngles.y, actual.transform.rotation.eulerAngles.z);
                    actual.transform.rotation = newRotation;
                }
                else
                {
                    cilindros.transform.Find(i.ToString()).gameObject.SetActive(false);
                }
            }
            //Quedan las manos que no las detecta openpose
            //Mano derecha
            calcularManos(17, "3", "RWrist",3);
            //Mano izquierda
            calcularManos(18, "5", "LWrist",6);
            
            //solo dejo activadas las manos cuando están activados los brazos también.
            cilindros.transform.Find(17.ToString()).gameObject.SetActive(cilindros.transform.Find(3.ToString()).gameObject.activeSelf);
            cilindros.transform.Find(18.ToString()).gameObject.SetActive(cilindros.transform.Find(5.ToString()).gameObject.activeSelf);


            //voy a colocar una esfera en la cabeza para cubrir la cabeza entera. El centro de la esfera irá al punto 
            //0 que es la nariz
            colocarEsferaCabeza();

            //La esfera sólo se muestra si se detectan orejas y nariz
            cilindros.transform.Find(19.ToString()).gameObject.SetActive(cilindros.transform.Find(14.ToString()).gameObject.activeSelf && cilindros.transform.Find(16.ToString()).gameObject.activeSelf &&
                cilindros.transform.Find(15.ToString()).gameObject.activeSelf && cilindros.transform.Find(13.ToString()).gameObject.activeSelf);
        }

    }

    public void colocarEsferaCabeza()
    {
        //cogemos la esfera de la cabeza
        GameObject cabeza = cilindros.transform.Find("19").gameObject;
        //la colocamos en el punto meedio de los ojos
        Vector3 coord1 = transformarCoordenada(openPose.points[14]);
        Vector3 coord2 = transformarCoordenada(openPose.points[15]);
        cabeza.transform.position = new Vector3((coord1.x + coord2.x) / 2, (coord1.y + coord2.y) / 2, -41); 
        //cogemos la distancia entre los puntos 16 y 17
        float tama = distancia(transformarCoordenada(openPose.points[16]), transformarCoordenada(openPose.points[17]));
        //colocamos la escala correcta
        cabeza.transform.localScale = new Vector3(tama,tama,tama);


    }

    //Manos es si se quiere la mano derecha (17) o izquierda (18)
    //Numero es el segmento de referencia. 3 si es brazo derecho y 5 si es izquierdo
    //Nombre es la muñeca de referencia. RWrist o LWrist
    //n es el numero de referencia del codo 
    public void calcularManos(int manos, string numero, string nombre, int nn)
    {
        //Longitud del cilindro de la mano
        float tama = cilindros.transform.Find(numero).transform.localScale.y;

        //Coordenada con la que empezamos
        Vector3 coord = transformarCoordenada(openPose.points[openPose.BODY_PARTS[nombre]]);
        
        //Direccion que sigue el trozo del codo a la muñeca
        Vector3 direccion = (transformarCoordenada(openPose.points[nn+1]) - transformarCoordenada(openPose.points[nn])).normalized;

        //Buscamos la mano que toca
        GameObject mano1 = cilindros.transform.Find(manos.ToString()).gameObject;

        //Ponemos la anchura del cilindro que hace de brazo y la longitud que hemos calculado antes
        mano1.transform.localScale = new Vector3(cilindros.transform.Find(numero).transform.localScale.x, tama, cilindros.transform.Find(numero).transform.localScale.z);

        //Calculamos la segunda coordenada (objetivo)
        Vector3 coordObj = coord + direccion * tama * 2 ;

        //Coloco la posición determinada
        mano1.transform.position = new Vector3((coord.x + coordObj.x) / 2, (coord.y + coordObj.y) / 2, -41);

        //Coloco ahora la rotacion que será en dirección de la coordenada 1 a la 2
        mano1.transform.LookAt(coordObj);
        Quaternion newRotation = Quaternion.Euler(mano1.transform.rotation.eulerAngles.x + 90, mano1.transform.rotation.eulerAngles.y, mano1.transform.rotation.eulerAngles.z);
        mano1.transform.rotation = newRotation;

    }

    public float calcularAnchura(float distancia)
    {
        //aqui habrá que ver el valor que se devuelve de anchura del cilindro
        return distancia/2;
    }

    public bool usable(int punto)
    {
        return usables.Contains(punto);

    }

    public void actualizarUsables()
    {

        if (formato == 1) // solo cabeza
        {
            for (int i = 2; i < 14; i++)
            {
                usables.Remove(i);
            }
        }
        else if (formato == 2)
        {
            for (int i = 3; i < 14; i++)
            {
                if (i != 5)
                    usables.Remove(i);
            }
        }
        else if (formato == 3)
        {
            for (int i = 8; i < 14; i++)
            {
                usables.Remove(i);
            }
        }
        else if (formato == 4)
        {
            for (int i = 9; i < 14; i++)
            {
                if (i != 11)
                    usables.Remove(i);
            }
        }

    }

    public float distancia(Vector3 origen, Vector3 destino)
    {
        return Mathf.Sqrt(Mathf.Pow((destino.x - origen.x), 2) + Mathf.Pow((destino.y - origen.y), 2));
    }

    //Recibe un punto y devuelve la coordenada transformada al juego en unity
    public Vector3 transformarCoordenada(Point p)
    {
        return new Vector3(-320f + (float)p.x, 199f - (float)p.y, -41f);
    }

    public void IncrementoPuntuacion(int incremento)
    {
        puntos += incremento;
        textoPuntuacion.text = puntos.ToString();

    }

    public void DecrementoPuntuacion(int decremento)
    {
        puntos -= decremento;
        textoPuntuacion.text = puntos.ToString();
    }

    public void DecrementoAzules()
    {
        nAzules--;
    }

    public void modificarTiempo(float t)
    {
        tRestante -= t;
    }

    public void sonidoAzul()
    {
        sonidoAzulG.SetActive(false);
        sonidoAzulG.SetActive(true);
    }

    public void sonidoRoja()
    {
        sonidoRojaG.SetActive(false);
        sonidoRojaG.SetActive(true);
    }

    public void sonidoPantallaCompleta()
    {
        sonidoCompletoG.SetActive(false);
        sonidoCompletoG.SetActive(true);
    }

    public void quitarRenderers()
    {
        for(int i = 0; i < cilindros.transform.childCount; i++)
        {
            cilindros.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = !cilindros.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled;
        }
    }

    public void activarCuentaAtras()
    {
        poner();
        Invoke("poner",1f);
        Invoke("poner",2f);
        Invoke("activarEmpezado", 3f);
    }

    public void poner()
    {

        if (nCuenta == 0)
        {
            nCuenta = 3;
        }
        cuenta.text = nCuenta.ToString();
        cuenta.enabled = true;
        nCuenta--;
        
    }

    public void activarEmpezado()
    {
        empezado = true;
        cuenta.enabled = false;
    }

}
