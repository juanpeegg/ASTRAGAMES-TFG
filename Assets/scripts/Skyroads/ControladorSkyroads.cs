using System.Collections;
using System.Collections.Generic;
using ScoresDatabase;
using ConfigurationDatabase;
using UnityEngine;

public class ControladorSkyroads : ControladorUIVideojuegos
{
    public robot robot;

    void Start()
    {
        juego = "SkyRoads";
        base.Start();

        //Valores por defecto
        robot.vel = 10;
        robot.sensibilidad = 40;

        //Si tenemos una configuracion concreta 
        SkyroadsDb configSkyroadsDb = new SkyroadsDb();
        SkyroadsEntity config = configSkyroadsDb.getConfigurationByName(jugador);
        configSkyroadsDb.close();

        if(config._juego != "Error")
        {
            //Inicializamos los valores segun nuestra configuracion guardada
            robot.vel = config._velRobot*5;
            robot.sensibilidad = config._sensibilidad*10;
        } 

       //Cargamos el fondo a corde con el nivel
       //StartCoroutine(scriptPanel.GetTexture(fondos[nivelActual-1]));

        //Cargamos la musica de fondo
        StartCoroutine(scriptPanel.GetSound("skyroads.mp3")); 
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (!victoria)
        {
            //CONDICION DE VICTORIA
            if (robot.meta || Input.GetKeyDown(KeyCode.V))
            {
                canvaGanar.SetActive(true);
                Destroy (robot);

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

    public void IncrementoPuntuacion(int incremento)
    {
        puntos += incremento;
        textoPuntuacion.text = puntos.ToString();
    }
}
