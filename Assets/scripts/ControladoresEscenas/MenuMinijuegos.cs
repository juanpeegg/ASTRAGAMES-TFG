using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using ScoresDatabase;
using ConfigurationDatabase;

public class MenuMinijuegos : ControladorMenu
{
    [SerializeField] Text msjDescripcion;
    [SerializeField] GameObject seleccion;
    [SerializeField] GameObject seleccionNivel;
    [SerializeField] GameObject seleccionNivelNinja;
    [SerializeField] GameObject panelRanking;
    [SerializeField] GameObject panelMiProgreso;
    [SerializeField] GameObject panelConfiguracion;
    [SerializeField] GameObject panelJuegos1;
    [SerializeField] GameObject panelJuegos2;
    [SerializeField] Button botonIzqda;
    [SerializeField] Button botonDcha;
    [SerializeField] Text textRanking;

    GameObject panelJuegos;

    [SerializeField] Button btnRanking;
    [SerializeField] GameObject btnMiProgreso;
    [SerializeField] Button btnAceptar;
    [SerializeField] Button btnAjustes;
    [SerializeField] Dropdown camera;

    //Lleva el nombre del juego que se ha seleccionado y el jugador actual
    private string juego, jugador;

    //Arkanoid
    private int nivel;
    public SliderController barraRaqueta, barraBola, barraSensibilidadArkanoid;
    private int tamRaqueta, velBola, sensibilidadArkanoid;

    //Skyroads
    public SliderController barraRobot, barraSensibilidadSkyroads;
    private int velRobot, sensibilidadSkyroads;

    //Color
    public SliderController inputTiempo;
    private int tiempo;

    //Burbujas
    public SliderController inputTiempoBurbujas;
    private int tiempoBurbujas;
    public Dropdown formatoBurbujas;

    //Ninja
    private int nivelNinja;
    public SliderController barraSensibilidadNinja;
    private int sensibilidadNinja;
    public SliderController barraPosicionNinja;
    private int posicionNinja;

    //Barco
    public Toggle boss;
    public SliderController barraVelocidadBarco, barraEnemigosIzq, barraEnemigosDcha, barraSensibilidadBarco;
    public Dropdown frecDisparo, nivelBarco;


    [SerializeField] Text msjDescripcionNivelNinja;
    public GameObject prefabRanking;
    public GameObject prefabMiProgreso;
    public GameObject contenedorRanking;
    public GameObject contenedorMiProgreso;
    PuntuacionesDb puntuacionesDb;

    List<PuntuacionesEntity> puntuaciones;

    PlayerInfo loadedData;

    private void Start()
    {
        panelJuegos = GameObject.Find("PanelJuegos");
        loadedData = DataSaver.loadData<PlayerInfo>("players");

        if (loadedData == null)
        {
            btnMiProgreso.SetActive(false);
            jugador = "Jugador";
        }
        else
        {
            jugador = loadedData.nombre;
        }
    }

    private void Seleccionar(float offset, string nombreJuego)
    {
        Clicar();
        juego = nombreJuego;

        seleccion.SetActive(true);
        btnAjustes.interactable = true;
        btnRanking.interactable = true;
        btnMiProgreso.GetComponent<Button>().interactable = true;
        btnAceptar.interactable = true;
        textRanking.text = "Ranking " + nombreJuego;
        seleccion.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(offset, -15, 0);
    }

    private void SeleccionarNivel(int offset, int nivelActual)
    {
        Clicar();
        if (juego == "Arkanoid")
        {
            nivel = nivelActual;
            seleccionNivel.SetActive(true);
            seleccionNivel.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(offset, -50, 0);
        }
        
        else if (juego == "Ninja")
        {
            nivelNinja = nivelActual;
            seleccionNivelNinja.SetActive(true);
            seleccionNivelNinja.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(offset, 7, 0);
        }
    }

    public void IrAJugar()
    {
        Clicar();
        int camara = camera.value;
        PlayerPrefs.SetInt("camara", camara);
        switch (juego)
        {
            case "Arkanoid":
                SceneManager.LoadScene("ArkanoidArUco");
                break;

            case "SkyRoads":
                SceneManager.LoadScene("SkyRoads");
                break;

            case "Color":
                SceneManager.LoadScene("Color");
                break;

            case "Ninja":
                SceneManager.LoadScene("Ninja");
                break;

            case "Invasion":
                SceneManager.LoadScene("Barco2");
                break;

            case "Burbujas":
                SceneManager.LoadScene("Burbujas");
                break;

            default:
                break;
        }
    }
    public void IrARanking()
    {
        Clicar();

        panelJuegos.gameObject.SetActive(false);
        panelRanking.SetActive(true);

        puntuacionesDb = new PuntuacionesDb();
        puntuaciones = puntuacionesDb.getGlobalScore(juego);
        puntuacionesDb.close();

        for (int i = 0; i < contenedorRanking.transform.childCount; i++)
        {
            Destroy(contenedorRanking.transform.GetChild(i).gameObject);
        }

        foreach (var puntuacion in puntuaciones)
        {
            GameObject fila = Instantiate(prefabRanking);
            fila.transform.SetParent(contenedorRanking.transform);
            fila.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = puntuacion._jugador;
            fila.gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = puntuacion._puntuacion.ToString();
        }
    }

    public void IrAMiProgreso()
    {
        Clicar();

        panelJuegos.gameObject.SetActive(false);
        panelMiProgreso.SetActive(true);

        puntuacionesDb = new PuntuacionesDb();
        puntuaciones = puntuacionesDb.getScoresPlayer(juego, jugador);
        puntuacionesDb.close();

        for (int i = 0; i < contenedorMiProgreso.transform.childCount; i++)
        {
            Destroy(contenedorMiProgreso.transform.GetChild(i).gameObject);
        }

        foreach (var puntuacion in puntuaciones)
        {
            GameObject fila = Instantiate(prefabMiProgreso);
            fila.transform.SetParent(contenedorMiProgreso.transform);
            fila.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = puntuacion._fecha;
            fila.gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = puntuacion._puntuacion.ToString();
        }
    }

    public void irPanelDerecha()
    {
        Clicar();
        botonDcha.interactable = false;
        botonIzqda.interactable = true;
        panelJuegos2.SetActive(true);
        panelJuegos1.SetActive(false);
        msjDescripcion.text = "";
        seleccion.SetActive(false);
        btnRanking.interactable = false;
        btnAjustes.interactable = false;
        btnAceptar.interactable = false;
        btnMiProgreso.GetComponent<Button>().interactable = false;
    }

    public void irPanelIzquierda()
    {
        Clicar();
        botonDcha.interactable = true;
        botonIzqda.interactable = false;
        panelJuegos1.SetActive(true);
        panelJuegos2.SetActive(false);
        msjDescripcion.text = "";
        seleccion.SetActive(false);
        btnRanking.interactable = false;
        btnAjustes.interactable = false;
        btnAceptar.interactable = false;
        btnMiProgreso.GetComponent<Button>().interactable = false;

    }

    public void VolverPantallaMiniJuegos()
    {
        Clicar();

        panelRanking.SetActive(false);
        panelMiProgreso.SetActive(false);
        panelConfiguracion.SetActive(false);
        panelConfiguracion.transform.Find("PanelArkanoid").gameObject.SetActive(false);
        panelJuegos.gameObject.SetActive(true);
    }

    public void IrAConfiguracion()
    {
        Clicar();

        panelJuegos.gameObject.SetActive(false);
        panelConfiguracion.SetActive(true);

        GameObject panelArkanoid = panelConfiguracion.transform.Find("PanelArkanoid").gameObject;
        GameObject panelSkyroads = panelConfiguracion.transform.Find("PanelSkyroads").gameObject;
        GameObject panelColor = panelConfiguracion.transform.Find("PanelColor").gameObject;
        GameObject panelNinja = panelConfiguracion.transform.Find("PanelNinja").gameObject;
        GameObject panelInvasion = panelConfiguracion.transform.Find("PanelInvasion").gameObject;
        GameObject panelBurbujas = panelConfiguracion.transform.Find("PanelBurbujas").gameObject;
        switch (juego)
        {
            case "Arkanoid":
                panelArkanoid.SetActive(true);
                panelSkyroads.SetActive(false);
                panelColor.SetActive(false);
                panelInvasion.SetActive(false);
                panelNinja.SetActive(false);
                panelBurbujas.SetActive(false);

                //Si existe, cargamos la configuracion anterior 
                ArkanoidDb configArkanoidDb = new ArkanoidDb();
                ArkanoidEntity config1 = configArkanoidDb.getConfigurationByName(jugador);
                configArkanoidDb.close();

                if (config1._juego != "Error")
                {
                    //Inicializamos los valores segun nuestra configuracion guardada
                    barraRaqueta.OnSliderChanged(config1._tamRaqueta);
                    barraBola.OnSliderChanged(config1._velBola);
                    barraSensibilidadArkanoid.OnSliderChanged(config1._sensibilidad);
                    nivel = config1._nivel;
                }
                else
                {
                    barraRaqueta.OnSliderChanged(1);
                    barraBola.OnSliderChanged(5);
                    barraSensibilidadArkanoid.OnSliderChanged(3);
                    nivel = 1;
                }
                seleccionNivel.SetActive(true);
                seleccionNivel.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(-155 + 75 * nivel, -50, 0);
                break;

            case "SkyRoads":
                panelArkanoid.SetActive(false);
                panelSkyroads.SetActive(true);
                panelColor.SetActive(false);
                panelInvasion.SetActive(false);
                panelNinja.SetActive(false);
                panelBurbujas.SetActive(false);

                //Si existe, cargamos la configuracion anterior 
                SkyroadsDb configSkyroadsDb = new SkyroadsDb();
                SkyroadsEntity config2 = configSkyroadsDb.getConfigurationByName(jugador);
                configSkyroadsDb.close();

                if (config2._juego != "Error")
                {
                    //Inicializamos los valores segun nuestra configuracion guardada
                    barraRobot.OnSliderChanged(config2._velRobot);
                    barraSensibilidadSkyroads.OnSliderChanged(config2._sensibilidad);
                }
                else
                {
                    barraRobot.OnSliderChanged(2);
                    barraSensibilidadSkyroads.OnSliderChanged(4);
                }
                break;

            case "Color":
                panelArkanoid.SetActive(false);
                panelColor.SetActive(true);
                panelSkyroads.SetActive(false);
                panelInvasion.SetActive(false);
                panelNinja.SetActive(false);
                panelBurbujas.SetActive(false);

                //Si existe, cargamos la configuracion anterior 
                ColorDb configColorDb = new ColorDb();
                ColorEntity config3 = configColorDb.getConfigurationByName(jugador);
                configColorDb.close();

                if (config3._juego != "Error")
                {
                    //Inicializamos los valores segun nuestra configuracion guardada
                    inputTiempo.OnSliderChanged(config3._tiempo);
                }
                else
                {
                    inputTiempo.OnSliderChanged(60);
                }
                break;

            case "Ninja":
                panelArkanoid.SetActive(false);
                panelColor.SetActive(false);
                panelSkyroads.SetActive(false);
                panelInvasion.SetActive(false);
                panelNinja.SetActive(true);
                panelBurbujas.SetActive(false);

                //Si existe, cargamos la configuracion anterior 
                NinjaDb configNinjaDb = new NinjaDb();
                NinjaEntity config4 = configNinjaDb.getConfigurationByName(jugador);
                configNinjaDb.close();

                if (config4._juego != "Error")
                {
                    //Inicializamos los valores segun nuestra configuracion guardada
                    barraSensibilidadNinja.OnSliderChanged(config4._sensibilidad);
                    nivelNinja = config4._nivel;
                    barraPosicionNinja.OnSliderChanged(config4._posicion);
                }
                else
                {
                    barraSensibilidadNinja.OnSliderChanged(25);
                    barraPosicionNinja.OnSliderChanged(1);
                    nivelNinja = 1;
                }
                seleccionNivel.SetActive(true);
                seleccionNivel.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(-155 + 75 * nivel, -50, 0);
                break;

            case "Invasion":
                panelArkanoid.SetActive(false);
                panelColor.SetActive(false);
                panelSkyroads.SetActive(false);
                panelNinja.SetActive(false);
                panelInvasion.SetActive(true);
                panelBurbujas.SetActive(false);

                //Si existe, cargamos la configuracion anterior 
                BarcoDb configInvasionDb = new BarcoDb();
                BarcoEntity config5 = configInvasionDb.getConfigurationByName(jugador);
                configInvasionDb.close();

                if (config5._juego != "Error")
                {
                    //Inicializamos los valores segun nuestra configuracion guardada
                    barraVelocidadBarco.OnSliderChanged(config5._velBarco);
                    barraEnemigosIzq.OnSliderChanged(config5._enemigosIzquierda);
                    barraEnemigosDcha.OnSliderChanged(config5._enemigosDerecha);
                    barraSensibilidadBarco.OnSliderChanged(config5._sensibilidad);
                    if (config5._boss == 0)
                        boss.isOn = false;
                    else
                        boss.isOn = true;
                    frecDisparo.SetValueWithoutNotify(config5._frecuenciaDisparo);
                    nivelBarco.SetValueWithoutNotify(config5._nivel);
                }
                else
                {
                    barraVelocidadBarco.OnSliderChanged(6);
                    barraEnemigosIzq.OnSliderChanged(10);
                    barraEnemigosDcha.OnSliderChanged(10);
                    barraSensibilidadBarco.OnSliderChanged(40);
                    boss.isOn = true;
                    frecDisparo.SetValueWithoutNotify(2);
                    nivelBarco.SetValueWithoutNotify(1);
                }
                break;

            case "Burbujas":
                panelArkanoid.SetActive(false);
                panelColor.SetActive(false);
                panelSkyroads.SetActive(false);
                panelInvasion.SetActive(false);
                panelNinja.SetActive(false);
                panelBurbujas.SetActive(true);

                //Si existe, cargamos la configuracion anterior 
                BurbujasDb configBurbujasDb = new BurbujasDb();
                BurbujasEntity config6 = configBurbujasDb.getConfigurationByName(jugador);
                configBurbujasDb.close();

                if (config6._juego != "Error")
                {
                    //Inicializamos los valores segun nuestra configuracion guardada
                    inputTiempoBurbujas.OnSliderChanged(config6._tiempo);
                    formatoBurbujas.SetValueWithoutNotify(config6._formato);
                }
                else
                {
                    inputTiempoBurbujas.OnSliderChanged(60);
                    formatoBurbujas.SetValueWithoutNotify(0);
                }
                break;

            default:
                break;
        }

    }

    public void GuardarConfiguracion()
    {
        Clicar();

        switch (juego)
        {
            case "Arkanoid":
                tamRaqueta = barraRaqueta.valor;
                velBola = barraBola.valor;
                sensibilidadArkanoid = barraSensibilidadArkanoid.valor;

                ArkanoidDb configArkanoidDb = new ArkanoidDb();
                configArkanoidDb.addData(new ArkanoidEntity(juego, jugador, tamRaqueta, velBola, sensibilidadArkanoid, nivel));
                configArkanoidDb.close();
                break;

            case "SkyRoads":
                velRobot = barraRobot.valor;
                sensibilidadSkyroads = barraSensibilidadSkyroads.valor;

                SkyroadsDb configSkyroadsDb = new SkyroadsDb();
                configSkyroadsDb.addData(new SkyroadsEntity(juego, jugador, velRobot, sensibilidadSkyroads));
                configSkyroadsDb.close();
                break;

            case "Color":
                tiempo = inputTiempo.valor;

                ColorDb configColorDb = new ColorDb();
                configColorDb.addData(new ColorEntity(juego, jugador, tiempo));
                configColorDb.close();
                break;

            case "Ninja":
                sensibilidadNinja = barraSensibilidadNinja.valor;
                posicionNinja = barraPosicionNinja.valor;
                NinjaDb configNinjaDb = new NinjaDb();
                configNinjaDb.addData(new NinjaEntity(juego, jugador, sensibilidadNinja, nivelNinja, posicionNinja));
                configNinjaDb.close();
                break;
            case "Invasion":
               
                BarcoDb configBarcoDb = new BarcoDb();
                int aux = 0;
                if (boss.isOn)
                    aux = 1;
                configBarcoDb.addData(new BarcoEntity(juego, jugador, barraVelocidadBarco.valor, barraEnemigosIzq.valor, barraEnemigosDcha.valor, frecDisparo.value, aux, barraSensibilidadBarco.valor,
                    nivelBarco.value));
                configBarcoDb.close();
                break;

            case "Burbujas":
                tiempoBurbujas = inputTiempoBurbujas.valor;

                BurbujasDb configBurbujasDb = new BurbujasDb();
                configBurbujasDb.addData(new BurbujasEntity(juego, jugador, tiempoBurbujas, formatoBurbujas.value));
                configBurbujasDb.close();
                break;
            default:
                break;
        }

        panelConfiguracion.SetActive(false);
        panelJuegos.gameObject.SetActive(true);
    }

    public void AparecerDescripcionArkanoid()
    {
        Seleccionar(-209f, "Arkanoid");
        msjDescripcion.text = "Destruye todos los ladrillos con una bola y hazla rebotar con tu raqueta.\n ¡Cuidado que no se te caiga al vacío!";

    }

    public void AparecerDescripcionSkyRoads()
    {
        Seleccionar(0, "SkyRoads");
        msjDescripcion.text = "Recorre con Tuercas los diferentes escenarios hasta llegar a la meta, ¡Cuidado con chocarte no sea que pierdas un tornillo!";
    }

    public void AparecerDescripcionColor()
    {
        Seleccionar(209f, "Color");
        msjDescripcion.text = "Elige las manchas de pintura que se correspondan con el color del texto ¡Cuidado con equivocarte!";
    }

    public void AparecerDescripcionNinja()
    {
        Seleccionar(-209f, "Ninja");
        msjDescripcion.text = "Hanzo es el mejor de los ninjas de todo el Japón. Ha conseguido escalar una torre enorme pero, ¡Necesita de tu ayuda para bajar!";
    }

    public void AparecerDescripcionInvasion()
    {
        Seleccionar(0f, "Invasion");
        msjDescripcion.text = "Un bonito poblado ha sido invadido por un grupo de monstruos. ¡Acaba con ellos en tu barco pirata y alcanza el puerto!";
    }

    public void AparecerDescripcionBurbujas()
    {
        Seleccionar(209f, "Burbujas");
        msjDescripcion.text = "Muévete por la pantalla y explota las pompas azules. ¡Cuidado con las rojas!";
    }

    public void SeleccionNvl1()
    {
        SeleccionarNivel(-80, 1);
    }

    public void SeleccionNvl2()
    {
        SeleccionarNivel(-5, 2);
    }

    public void SeleccionNvl3()
    {
        SeleccionarNivel(70, 3);
    }

    public void SeleccionNvl4()
    {
        SeleccionarNivel(145, 4);
    }

    public void SeleccionNvl5()
    {
        SeleccionarNivel(220, 5);
    }

    public void SeleccionNvl1Ninja()
    {
        SeleccionarNivel(-80, 1);
        msjDescripcionNivelNinja.text = "Únicamente tendrás que enfrentarte con filas rotatorias";
    }

    public void SeleccionNvl2Ninja()
    {
        SeleccionarNivel(-5, 2);
        msjDescripcionNivelNinja.text = "Te enfrentarás a filas rotatorias y a pinchos. ¡Cuidado no acabes clavado!";
    }

    public void SeleccionNvl3Ninja()
    {
        SeleccionarNivel(70, 3);
        msjDescripcionNivelNinja.text = "La cosa se pone interesante... Rotaciones, pinchos, estrellas ninja y... ¡aceleradores!";
    }

    public void SeleccionNvl4Ninja()
    {
        SeleccionarNivel(145, 4);
        msjDescripcionNivelNinja.text = "Rotaciones, pinchos, estrellas ninja, aceleradores... pero, ojo, ¡pueden aparecer dobles!";
    }

    public void SeleccionNvl5Ninja()
    {
        SeleccionarNivel(220, 5);
        msjDescripcionNivelNinja.text = "Sólo para los más valientes. El nivel 4 completo pero con mayor probabilidad de obstáculos. ¿Estás preparado?";
    }
}
