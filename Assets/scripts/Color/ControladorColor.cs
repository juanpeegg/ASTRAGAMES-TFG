using System.Collections;
using System.Collections.Generic;
using OpenCVForUnityExample;
using ScoresDatabase;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ConfigurationDatabase;

public class ControladorColor : ControladorUIVideojuegos
{
    List<string> colores = new List<string> { "Rojo", "Amarillo", "Verde", "Azul" };

    public List<GameObject> manchas = new List<GameObject>();

    public bool activo;

    public GameObject esfera;

    public Text texto;

    public Text timer;
    public float tiempo;

    ArUcoWebCamExampleTexture arucoDetector;

    ControladorEsfera controlEsfera;

    public GameObject sonidos;

    void Start()
    {
        juego = "Color";
        base.Start();
        arucoDetector = (ArUcoWebCamExampleTexture)FindObjectOfType(typeof(ArUcoWebCamExampleTexture));
        controlEsfera = esfera.GetComponent<ControladorEsfera>();

        //Valores por defecto    
        tiempo = 60;

        //Si tenemos una configuracion concreta 
        ColorDb configColorDb = new ColorDb();
        ColorEntity config = configColorDb.getConfigurationByName(jugador);
        configColorDb.close();

        if(config._juego != "Error")
        {
            //Inicializamos los valores segun nuestra configuracion guardada
            tiempo = config._tiempo;
        }

        //Cargamos la musica de fondo
        StartCoroutine(scriptPanel.GetComponent<Url>().GetSound("color.mp3"));

        timer.text = tiempo.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        //Debug.Log(faceDetector);
        //La esfera se mueve siguiendo los movimientos de la cara
        esfera.transform.position = new Vector3( 320 - arucoDetector.coordenadasPantalla.x, 0, -(arucoDetector.coordenadasPantalla.y - 240));
        
        //Si se pulsa el espacio empieza el juego
        if (Input.GetKeyDown(KeyCode.Space) && !activo)
        {
            activo = true;
            texto.text = colores[Random.Range(0, colores.Count)];
            actualizarColor();
        }
        
        if(activo)
        {
            //Si el nombre del panel es igual al color seleccionado
            if (texto.text == controlEsfera.colorSelec)
            {
                //Debug.Log("CORRECTO EL COLOR ES " + texto.text);
                puntos += 10;
                textoPuntuacion.text = puntos.ToString();

                //Randomizamos
                texto.text = colores[Random.Range(0, colores.Count)];
                actualizarColor();

                foreach (var m in manchas)
                {
                    string colorTag = ElegirColor();
                    m.tag = colorTag;
                    switch (colorTag)
                    {
                        case "Rojo":
                            m.GetComponent<MeshRenderer>().material.color =
                                Color.red;
                            break;
                        case "Amarillo":
                            m.GetComponent<MeshRenderer>().material.color =
                                Color.yellow;
                            break;
                        case "Verde":
                            m.GetComponent<MeshRenderer>().material.color =
                                Color.green;
                            break;
                        case "Azul":
                            m.GetComponent<MeshRenderer>().material.color =
                                Color.blue;
                            break;
                        default:
                            break;
                    }
                }
                colores = new List<string> { "Rojo", "Amarillo", "Verde", "Azul" };
            }

            if (!victoria)
            {
                DecrementarContador();

                //CONDICION DE VICTORIA
                if (tiempo == 0 || Input.GetKeyDown(KeyCode.V))
                {
                    timer.text = "0";
                    canvaGanar.SetActive(true);
                    Destroy (controlEsfera);

                    //Si el usuario está registrado se guarda su puntuación
                    if (loadedData != null)
                    {
                        puntuacionesDb = new PuntuacionesDb();
                        puntuacionesDb.addData(new PuntuacionesEntity(juego, jugador, puntos));
                        puntuacionesDb.close();
                    }
                    victoria = true;
                }
            }
        }
    }

    string ElegirColor()
    {
        int randomIndex = Random.Range(0, colores.Count);
        string randomColor = colores[randomIndex];
        colores.Remove(colores[randomIndex]);
        return randomColor;
    }

    void DecrementarContador()
    {
        tiempo -= Time.deltaTime;
        if(tiempo <= 0)
            tiempo = 0;
        timer.text = tiempo.ToString("f0");
    }

    public void actualizarColor()
    {
        Debug.Log(texto.text);
        switch (texto.text)
        {
            case "Amarillo":
                texto.color = Color.yellow;
                desactivarTodos();
                sonidos.transform.Find("Amarillo").gameObject.SetActive(true);
                break;
            case "Rojo":
                texto.color = Color.red;
                desactivarTodos();
                sonidos.transform.Find("Rojo").gameObject.SetActive(true);
                break;
            case "Verde":
                texto.color = Color.green;
                desactivarTodos();
                sonidos.transform.Find("Verde").gameObject.SetActive(true);
                break;
            case "Azul":
                texto.color = Color.blue;
                desactivarTodos();
                sonidos.transform.Find("Azul").gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void desactivarTodos()
    {
        sonidos.transform.Find("Amarillo").gameObject.SetActive(false);
        sonidos.transform.Find("Rojo").gameObject.SetActive(false);
        sonidos.transform.Find("Verde").gameObject.SetActive(false);
        sonidos.transform.Find("Azul").gameObject.SetActive(false);
    }
}
