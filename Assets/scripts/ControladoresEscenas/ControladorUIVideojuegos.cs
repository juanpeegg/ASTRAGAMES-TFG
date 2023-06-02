using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ScoresDatabase;
using UnityEngine;

public class ControladorUIVideojuegos : ControladorMenu
{
    public int puntos = 0;
    public Text textoPuntuacion;
    public Text textoNombre;

    public GameObject canvaGanar;
    protected bool victoria;

    protected PlayerInfo loadedData;
    protected string jugador;
    protected string juego;

    protected PuntuacionesDb puntuacionesDb;
    protected int puntuacionDB;
    public GameObject msjRecord;

    protected Url scriptPanel;
    
    // Start is called before the first frame update
    protected void Start()
    {
        scriptPanel = GameObject.FindWithTag("fondo").GetComponent<Url>();

        //Si estamos registrados se nos almacenaran las puntuaciones
        loadedData = DataSaver.loadData<PlayerInfo>("players"); 
        if (loadedData != null)
        {
            //Inicializamos valores
            jugador = loadedData.nombre;
        }
        else
        {
            jugador = "Jugador";
        }
        textoNombre.text = jugador;

        puntuacionesDb = new PuntuacionesDb();
        puntuacionDB = puntuacionesDb.getBestScorePlayer(juego, jugador);
        puntuacionesDb.close();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuMinijuegos");
        }

        if (!victoria)
        {
            //CONDICION RECORD
            if (loadedData != null)
            {
                if (puntos > puntuacionDB && puntos > 0)
                {
                    msjRecord.SetActive(true);
                }
                else
                {
                    msjRecord.SetActive(false);
                }
            }
        }
    }
}
