using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using ScoresDatabase;
using ConfigurationDatabase;

public class ControladorUI : ControladorUIVideojuegos
{
    public int nLadrillos;

    public Text textoNivel;
    public Text textoLadrillos;

    public bola bola;
    public RaquetaArUco raqueta;
    
    [SerializeField] int nivelActual = 1;
    List<GameObject> niveles = new List<GameObject>();
    List<string> fondos = new List<string>(){"nivel1.jpg", "nivel2.jpg", "nivel3.jpg", "nivel4.jpg", "nivel5.jpg"};

    void Start()
    {
        juego = "Arkanoid";
        base.Start();
        
        //Valores por defecto
        bola.Velocity = 100;

        //Si tenemos una configuracion concreta 
        ArkanoidDb configArkanoidDb = new ArkanoidDb();
        ArkanoidEntity config = configArkanoidDb.getConfigurationByName(jugador);
        configArkanoidDb.close();
        
        if(config._juego != "Error")
        {
            //Inicializamos los valores segun nuestra configuracion guardada
            raqueta.gameObject.transform.localScale = new Vector3(100+config._tamRaqueta*50, 20, 20);
            bola.Velocity = config._velBola*20;
            raqueta._factor = -300+config._sensibilidad*500;
            nivelActual = config._nivel;
        } 

        raqueta.RecalcularLimites();

        //Cargamos los niveles e instanciamos los ladrillos acorde con el nivel seleccionado
        CargarNiveles();
        Instantiate(niveles[nivelActual-1],new Vector3(0, 0, 130), Quaternion.identity);
        
        //Cargamos el fondo acorde con el nivel
        StartCoroutine(scriptPanel.GetTexture(fondos[nivelActual-1]));

        //Cargamos la musica de fondo
        StartCoroutine(scriptPanel.GetSound("arkanoid.mp3"));

        nLadrillos = GameObject.FindGameObjectsWithTag("ladrillo").Length;

        textoNivel.text = "Nivel " + (nivelActual).ToString();
        textoLadrillos.text = "Ladrillos: " + nLadrillos.ToString();
    }

    void Update()
    {
        base.Update();

        textoLadrillos.text = "Ladrillos: " + nLadrillos.ToString();

        if (!victoria)
        {
            //CONDICION DE VICTORIA
            if (nivelActual == niveles.Count && nLadrillos == 0 || Input.GetKeyDown(KeyCode.V))
            {
                canvaGanar.SetActive(true);
                Destroy(bola);
                Destroy(raqueta);

                //Si el usuario está registrado se guarda su puntuación
                if (loadedData != null)
                {
                    puntuacionesDb = new PuntuacionesDb();
                    puntuacionesDb.addData(new PuntuacionesEntity(juego, jugador, puntos));
                    puntuacionesDb.close();
                }
                victoria = true;
            }

            //CONDICION PARA PASAR DE NIVEL
            if (nLadrillos == 0 && !victoria)
            {
                //Aumentamos el contador de nivel y lo mostramos
                nivelActual++; 
                textoNivel.text = "Nivel " + nivelActual.ToString();
                

                //Eliminamos el emboltorio de los ladrillos
                DestroyImmediate(GameObject.FindWithTag("ladrillos"));
                
                //Generamos los nuevos ladrillos
                Instantiate(niveles[nivelActual-1],new Vector3(0, 0, 130), Quaternion.identity); 
                nLadrillos = GameObject.FindGameObjectsWithTag("ladrillo").Length;

                //Al cambiar de nivel tambien cambiamos el fondo
                StartCoroutine(scriptPanel.GetTexture(fondos[nivelActual-1]));

                bola.vuelve();  
            }

            if(Input.GetKeyDown(KeyCode.L) && !victoria)
            {
                DestroyImmediate(GameObject.FindWithTag("ladrillo"));
                nLadrillos--;
            }

            if(Input.GetKeyDown(KeyCode.P) && !victoria)
            {
                for (int i=0; i < nLadrillos; i++)
                    DestroyImmediate(GameObject.FindWithTag("ladrillo"));
                nLadrillos = 0;

                bola.vuelve();
            }
        }
    }

    public void IncrementoPuntuacion(int incremento)
    {
        puntos += incremento;
        textoPuntuacion.text = puntos.ToString();
        nLadrillos--;
    }

    public void DecrementoPuntuacion(int decremento)
    {
        puntos -= decremento;
        textoPuntuacion.text = puntos.ToString();
    }

    private void CargarNiveles()
    {
        GameObject ladrillosNvl1 = Resources.Load("Niveles/LadrillosNvl1", typeof(GameObject)) as GameObject;
        GameObject ladrillosNvl2 = Resources.Load("Niveles/LadrillosNvl2", typeof(GameObject)) as GameObject;
        GameObject ladrillosNvl3 = Resources.Load("Niveles/LadrillosNvl3", typeof(GameObject)) as GameObject;
        GameObject ladrillosNvl4 = Resources.Load("Niveles/LadrillosNvl4", typeof(GameObject)) as GameObject;
        GameObject ladrillosNvl5 = Resources.Load("Niveles/LadrillosNvl5", typeof(GameObject)) as GameObject;

        niveles.Add(ladrillosNvl1);
        niveles.Add(ladrillosNvl2);
        niveles.Add(ladrillosNvl3);
        niveles.Add(ladrillosNvl4);
        niveles.Add(ladrillosNvl5);
    }
}
