using System.Collections;
using System.Collections.Generic;
using ScoresDatabase;
using ConfigurationDatabase;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControladorNinja : ControladorUIVideojuegos
{

    public GameObject mirilla;

    // Tengo el número de filas que he atravesado seguidas
    public int seguidos = 0;

    public Mover4 mover;

    public GameObject romperFila;

    public Parpadeo parpadeo;

    public GameObject torre;

    public GeneradorEscalones generador;

    public GameObject sonidoSalto;

    public GameObject sonidoGanar;

    public GameObject sonidoRomper;

    public GameObject sonidoDano;

    public GameObject sonidoCaigo;

    public GameObject sonidoFarolillo;

    public Text filasRestantes;

    void Start()
    {
        generador = GameObject.Find("Generador").GetComponent<GeneradorEscalones>();
        mover = GameObject.Find("base4").GetComponent<Mover4>();

        juego = "Ninja";
        base.Start();

        //Si tenemos una configuracion concreta 
        NinjaDb configNinjaDb = new NinjaDb();
        NinjaEntity config = configNinjaDb.getConfigurationByName(jugador);
        configNinjaDb.close();

        int dificultad = 1;
        int posicion = 1;
        if(config._juego != "Error")
        {
            //Inicializamos los valores segun nuestra configuracion guardada
            torre.GetComponent<RotacionEscalones>().sensibilidad = 105-config._sensibilidad;
            dificultad = config._nivel;
            posicion = config._posicion;
        }
        else //si no, valores por defecto
        {
            torre.GetComponent<RotacionEscalones>().sensibilidad = 25;
        }

        //Cargamos el fondo a corde con el nivel
        //StartCoroutine(scriptPanel.GetTexture(fondos[nivelActual-1]));

        filasRestantes.text = (30-posicion+1).ToString();
        // Iniciamos el nivel que toque
        generador.StartGenerador(dificultad, posicion-1);
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        //Con la tecla D borramos todas las filas
        if (Input.GetKeyDown(KeyCode.D))
        {
            GameObject cilindro = GameObject.Find("Cylinder");
            for (int i=1;i<cilindro.transform.childCount;i++)
            {
                Destroy(cilindro.transform.GetChild(i).gameObject);
            }
        }

        if (victoria)
        {
            // Si se ha ganado y se pulsa espacio se reinicia el nivel.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Ninja");
            }
        }

    }

    public void heGanado()
    {
        if (!victoria)
        {
            canvaGanar.SetActive(true);
            sonidoCaigo.SetActive(false);
            sonidoGanar.SetActive(true);

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

    public void IncrementoPuntuacion(int incremento)
    {
        puntos += incremento;
        textoPuntuacion.text = puntos.ToString();
    }

    public void destruirFila(GameObject fila)
    {
        // tengo que activar una fuerza que lance todo hacia los lados y cambiarle
        // previamente el material a color rojo.
        // Obtiene la cantidad de hijos del objeto padre

        int childCount = fila.transform.childCount;
        // Itera a través de los hijos del objeto padre excepto el pasar nivel y el de la posicion
        for (int i = 0; i < childCount-2; i++)
        {
            // Obtiene la referencia al hijo en la posición i
            GameObject childObject = fila.transform.GetChild(i).gameObject;
            if (childObject.GetComponent<MeshRenderer>() != null)
                childObject.GetComponent<MeshRenderer>().material.color = Color.red;           
            // Hay que desactivar el kinematic para que le afecten las fuerzas y          
            // Ahora si, aplicas la fuerza
            Rigidbody rb = childObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                //quito la protección para que les afecte la fuerza
                rb.constraints = RigidbodyConstraints.None;
                
                //explosition force, posicion de esfera, radio de esfera.
                rb.AddExplosionForce(0.0008f, fila.transform.position-new Vector3(0,5,0), 10f);
            }
        }
        //quito el collider de pasar el nivel por si se destruye la fila
        //por acumulacion de las anteriores
        fila.transform.Find("PasaNivel").gameObject.GetComponent<BoxCollider>().enabled = false;
        fila.GetComponent<Fila>().destruir();

        sonidoRomper.SetActive(false);
        sonidoRomper.SetActive(true);

        //Actualizo las restantes
        //Debug.Log("Le quito una");
        int numero;
        int.TryParse(filasRestantes.text, out numero);
        numero--;
        filasRestantes.text = numero.ToString();

    }

    public void activarDestino(Vector3 nueva)
    {
        mirilla.SetActive(true);
        mirilla.GetComponent<RectTransform>().position = nueva - new Vector3(0,25,0);
    }

    public void desactivarDestino()
    {
        mirilla.SetActive(false);
    }

    public void acelerarCaida(int caida)
    {
        mover.acelerarCaida(caida);
        activarSonidoCaigo();
    }

    public void romperEscalon()
    {
        destruirFila(romperFila);
        //Uno de pasarlo y otro extra de acumulación
        IncrementoPuntuacion(2);
        Debug.Log(romperFila);
    }

    public void activarParpadeo(bool a)
    {
        parpadeo.activado = a;
        if (!a)
        {
            parpadeo.restaurar();
        }
        else
        {
            parpadeo.cambiar();
            Invoke("activarDetector", 2f);
        }
 
    }

    public void activarDetector()
    {
        // Activamos de nuevo la posibilidad de golpes
        parpadeo.gameObject.transform.parent.parent.GetComponent<misSensores2>().golpeDisponible = true;
        activarParpadeo(false);
    }

    public void ponerSonidoSalto()
    {
        sonidoSalto.SetActive(true);
        Invoke("pararSonidoSalto", 1f);
    }

    public void pararSonidoSalto()
    {
        sonidoSalto.SetActive(false);
    }

    public void activarSonidoDano()
    {
        sonidoDano.SetActive(false);
        sonidoDano.SetActive(true);
    }

    public void activarSonidoCaigo()
    {
        if (!sonidoCaigo.activeSelf)
            sonidoCaigo.SetActive(true);
    }
    public void activarSonidoFarolillo()
    {
        sonidoFarolillo.SetActive(false);
        sonidoFarolillo.SetActive(true);
    }


    public void pararSonidoCaigo()
    {
        sonidoCaigo.SetActive(false);
    }

}
