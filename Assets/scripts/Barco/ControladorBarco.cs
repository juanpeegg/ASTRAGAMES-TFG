using System.Collections;
using System.Collections.Generic;
using ScoresDatabase;
using ConfigurationDatabase;
using UnityEngine;

public class ControladorBarco : ControladorUIVideojuegos
{
    public Barco barco;

    public GameObject sonidoBarco;
    public GameObject sonidoBocina;
    public GameObject sonidoRisa;
    public GameObject sonidoCanon;
    public GameObject sonidoGranada;
    public GameObject sonidoRecibir;
    public GameObject sonidoVictoria;
    public GameObject sonidoDiana;

    public GeneradorEnemigos generador;
    public GeneradorDianas generadorDiana;
    private float tiempoBocina = 20f;
    private float tiempoActual = 0f;
    private bool risa = true;

    void Start()
    {
        juego = "Invasion";
        base.Start();

        generador = GameObject.Find("Generador").GetComponent<GeneradorEnemigos>();
        generadorDiana = GameObject.Find("Generador").GetComponent<GeneradorDianas>();
        //Si tenemos una configuracion concreta 
        BarcoDb configBarcoDb = new BarcoDb();
        BarcoEntity config = configBarcoDb.getConfigurationByName(jugador);
        configBarcoDb.close();

        if (config._juego != "Error")
        {
            generador.arrancarGenerador(config._enemigosIzquierda, config._enemigosDerecha, config._boss);
            barco.establecerValores(config._frecuenciaDisparo, config._velBarco, config._sensibilidad, 1);

        }
        else
        {
            generador.arrancarGenerador(10, 10, 1);
            barco.establecerValores(2, 6, 40, 1);
        }

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        tiempoActual += Time.deltaTime;
        if (tiempoBocina < tiempoActual)
        {
            activarBocina();
            tiempoActual = 0f;
            risa = true;
        }
        else if (tiempoActual > 10 && risa)
        {
            activarRisa();
            risa = false;
        }

    }

    public void activarBocina()
    {
        sonidoBocina.SetActive(false);
        sonidoBocina.SetActive(true);
    }

    public void activarRisa()
    {
        sonidoRisa.SetActive(false);
        sonidoRisa.SetActive(true);
    }

    public void activarDiana()
    {
        sonidoDiana.SetActive(false);
        sonidoDiana.SetActive(true);
    }

    public void activarCanon()
    {
        sonidoCanon.SetActive(false);
        sonidoCanon.SetActive(true);
    }

    public void activarDano()
    {
        sonidoRecibir.SetActive(false);
        sonidoRecibir.SetActive(true);
    }

    public void activarGranada()
    {
        sonidoGranada.SetActive(false);
        sonidoGranada.SetActive(true);
    }

    public void activarVictoria()
    {
        sonidoVictoria.SetActive(false);
        sonidoVictoria.SetActive(true);
    }

    public void IncrementoPuntuacion(int incremento)
    {
        puntos += incremento;
        textoPuntuacion.text = puntos.ToString();
    }

    public void heGanado()
    {
        if (!victoria)
        {
            canvaGanar.SetActive(true);
            barco.v = 0f;
            activarVictoria();
            sonidoBarco.SetActive(false);
            Time.timeScale = 0f;

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

    public bool isVictoria()
    {
        return victoria;
    }

}
