using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneradorEscalones : MonoBehaviour
{
    //El prefab con la fila de escalones
    public GameObject filaEscalones;

    //La primera fila de escalones ya viiene hecha
    public GameObject escalon1;

    //Distancia entre filas
    public float distanciaEscalones = 0.066f;

    //Numero de filas de escalones que tendrá la torre
    public int nFilas = 30;

    //Van a ser hijos de la torre
    public GameObject torre;

    public GameObject acelerador;

    public GameObject pincho;

    public GameObject estrella;

    public GameObject farolillo;

    //a menor dificultad, más posibilidades de que te salga una fila sin nada especial
    public int dificultad = 1;

    public Text canvasNivel;


    /* NIVEL 1: NO HAY NADA, SALTAR DE FILA + ROTACIONES.
     * NIVEL 2: LO DEL NIVEL 1 + PINCHOS
     * NIVEL 3: FILA EN FILA, ROTACIONES, PINCHOS, ESTRELLAS Y ACELERADORES (UNICOS)
     * NIVEL 4: LO DEL NIVEL 3 + BAJA POSIBILIDAD DE COMBINACIONES DOBLES
     * NIVEL 5: LO DEL NIVEL 4 + POSIBILIDAD MAS ALTA DE COMBINACIONES DOBLES Y MENOR
     *          DE QUE NO HAYA NADA
     */


    Dictionary<int, float> orientacionesObstaculos = new Dictionary<int, float>()
    {
    {0, 45f},
    {1, 90f},
    {2, 45f},
    {3, 90f},
    {4, 0f},
    {5, -45f},
    {6, 0f},
    {7, -45f}
    };

    Dictionary<int, float> orientacionesFarolillo = new Dictionary<int, float>()
    {
    {0, 0f},
    {1, 90f},
    {2, -90f},
    {3, 180f},
    };

    // Start is called before the first frame update
    public void StartGenerador(int dificultad, int posicion)
    {
        this.dificultad = dificultad;
        //Ponemos el nivel en pantalla
        canvasNivel.text = dificultad.ToString();
        //Al empezar, se crean las 30 filas
        for(int i=0; i< nFilas - 1; i++)
        {
            // Fila creada y como padre la torre
            GameObject actual = Instantiate(filaEscalones, torre.transform);
            // Se van poniendo equidistantes
            actual.transform.localPosition = escalon1.transform.localPosition -
                new Vector3(0, distanciaEscalones * (i + 1), 0);
            // Le cambio el nombre para facilitar
            actual.name = "FilaEscalones" + (i + 2);
            // Convertimos la fila al tipo que determinemos
            int agujeros = agujerosFila(actual);
            //Creamos un farolillo para la fila
            float r = Random.value;
            // el farolillo o no va o va a uno de los 4 centros
            if (r > 0.2 && agujeros < 4)
                crearFarolillo(actual, r);
            
            // Hay que completar la fila con lo que toque (o no)
            completarFila(actual, agujeros);
            //En la posicion en la que se empieza
            if (i+1 == posicion)
            {
                GameObject.Find("base4").transform.position = actual.transform.Find("Posicion").position;
            }
            else if (i+1 < posicion)
            {
                actual.SetActive(false);
            }
        }

         
    }

    public void crearFarolillo(GameObject actual, float r)
    {
        GameObject farolillos = Instantiate(farolillo);
        int hijo = 0;
        if (r >= 0.2 && r < 0.4)
        {
            hijo = 0;
            farolillos.transform.position = actual.transform.GetChild(4).GetChild(0).position + new Vector3(-2, 12.3f, 0);
        }
        else if (r >= 0.4 && r < 0.6)
        {
            hijo = 1;
            farolillos.transform.position = actual.transform.GetChild(1).GetChild(0).position + new Vector3(0, 12.3f, 2);
        }
        else if (r >= 0.6 && r < 0.8)
        {
            hijo = 2;
            farolillos.transform.position = actual.transform.GetChild(3).GetChild(0).position + new Vector3(0, 12.3f, -2.8f);
        }
        else //entre 0.8 y 1
        {
            hijo = 3;
            farolillos.transform.position = actual.transform.GetChild(6).GetChild(0).position + new Vector3(2.8f, 12.3f, 0);
        }

        // Obtener la rotación actual del objeto en Euler Angles
        Vector3 rotacionActual = farolillos.transform.rotation.eulerAngles;
        // Sumo en el eje z la rotacion que me diga mi diccionario
        rotacionActual.y += orientacionesFarolillo[hijo];

        // Crear una nueva rotación a partir de los Euler Angles modificados
        Quaternion nuevaRotacion = Quaternion.Euler(rotacionActual);

        // Asignar la nueva rotación al objeto
        farolillos.transform.rotation = nuevaRotacion;

        //Lo ponemos el penultimo de la fila
        farolillos.transform.SetParent(actual.transform);

        farolillos.transform.SetSiblingIndex(actual.transform.childCount - 3);
    }
    /*
    -acelerador + 1 pincho (random 0)
    -acelerador + estrella (random 1)
    -2 pinchos (random 2)
    -2 estrellas (random 3)
    -acelerador (random 4)
    -absolutamente nada(segun nivel de dificultad, mas o menos probable) (5,6,7,8)
    */
    public void completarFila(GameObject actual, int agujeros)
    {
        // 1, 2 o 3 agujeros (4, 5 y 6 escalones)
        if (agujeros <= 3 && agujeros >= 1)
        {
            determinarTipo(actual);
        }
        else // 4 o 5 agujeros (2 o 3 escalones)
        {
            filaRotatoria(actual);
        }
    }
    /* NIVEL 1: NO HAY NADA, SALTAR DE FILA + ROTACIONES.
     * NIVEL 2: LO DEL NIVEL 1 + PINCHOS
     * NIVEL 3: FILA EN FILA, ROTACIONES, PINCHOS, ESTRELLAS Y ACELERADORES (UNICOS)
     * NIVEL 4: LO DEL NIVEL 3 + BAJA POSIBILIDAD DE COMBINACIONES DOBLES
     * NIVEL 5: LO DEL NIVEL 4 + POSIBILIDAD MAS ALTA DE COMBINACIONES DOBLES Y MENOR
     *          DE QUE NO HAYA NADA
     */
    public void determinarTipo(GameObject actual)
    {
        if (dificultad == 2)
        {
            float random = Random.value;
            if (random <= 0.5)
                ponerObjeto(actual, pincho);
        }
        else if (dificultad == 3)
        {
            //1 de cada 3 pinchos, 1 de cada 4 estrellas, 1 de cada 10 aceleradores
            float random = Random.value;
            if (random <= 0.1)
                ponerObjeto(actual, acelerador);
            else if (random > 0.1 & random <= 0.35)
                ponerObjeto(actual, estrella);
            else if (random > 0.35 && random <= 0.69)
                ponerObjeto(actual, pincho);
            //en cualquier otro caso, no se pone nada, fila limpia
        }
        else if (dificultad == 4)
        {
            //3 de cada 10 pinchos, 1 de cada 5 estrellas, 1 de cada 10 aceleradores
            // 1 de cada 5 fila doble
            float random = Random.value;
            if (random <= 0.1)
                ponerObjeto(actual,acelerador);
            else if (random > 0.1 & random <= 0.30)
                ponerObjeto(actual, estrella);
            else if (random > 0.35 && random <= 0.65)
                ponerObjeto(actual, pincho);
            else if(random > 0.65 && random <= 0.75)
                ponerObjetoDoble(actual, pincho);
            else if (random > 0.75 && random <= 0.85)
                ponerObjetoDoble(actual, estrella);
            
            //en cualquier otro caso, no se pone nada, fila limpia
        }
        else if (dificultad == 5)
        {
            //3 de cada 10 pinchos, 1 de cada 5 estrellas, 1 de cada 10 aceleradores
            // 3 de cada 10 fila doble
            float random = Random.value;
            if (random <= 0.1)
                ponerObjeto(actual, acelerador);
            else if (random > 0.1 & random <= 0.30)
                ponerObjeto(actual, estrella);
            else if (random > 0.35 && random <= 0.65)
                ponerObjeto(actual, pincho);
            else if (random > 0.65 && random <= 0.8)
                ponerObjetoDoble(actual, pincho);
            else if (random > 0.8 && random <= 0.95)
                ponerObjetoDoble(actual, estrella);

            //en cualquier otro caso, no se pone nada, fila limpia
        }
    }

    //pongo el objeto que se indique en una posicion random
    public void ponerObjeto(GameObject actual, GameObject poner)
    {
        int childCount = actual.transform.childCount;
        int random = Random.Range(0, 8);

        while (!actual.transform.GetChild(random).gameObject.activeSelf)
        {
            random = Random.Range(0, 8);
        }

        GameObject p = Instantiate(poner);

        //Cogemos la posicion del cubito que tenemos para marcar posiciones
        p.transform.position = actual.transform.GetChild(random).GetChild(0).position;//+ new Vector3(0,1,0);
        p.transform.position -= new Vector3(0, 0.5f, 0);

        // Obtener la rotación actual del objeto en Euler Angles
        Vector3 rotacionActual = p.transform.rotation.eulerAngles;
        Debug.Log(p + " " + rotacionActual);
        // Sumo en el eje z la rotacion que me diga mi diccionario
        rotacionActual.y += orientacionesObstaculos[random];

        // Crear una nueva rotación a partir de los Euler Angles modificados
        Quaternion nuevaRotacion = Quaternion.Euler(rotacionActual);

        // Asignar la nueva rotación al objeto
        p.transform.rotation = nuevaRotacion;

        //Vector3 rotacion = p.transform.rotation.eulerAngles;
        //Vector3 rotacionNueva = new Vector3(0, 0, actual.transform.GetChild(random).rotation.eulerAngles.z);
        //p.transform.rotation += Quaternion.Euler(rotacionNueva);
        p.transform.SetParent(actual.transform);
        GameObject hijo = p.transform.GetChild(0).gameObject;
        hijo.transform.SetParent(actual.transform);
        DestroyImmediate(p);
        hijo.transform.SetSiblingIndex(childCount-2);
    }

    public void ponerObjetoDoble(GameObject actual, GameObject poner)
    {
        int childCount = actual.transform.childCount;
        int random1 = Random.Range(0, 8);
        int random2 = Random.Range(0, 8);

        while (!actual.transform.GetChild(random1).gameObject.activeSelf )
        {
            random1 = Random.Range(0, 8);
        }

        while (!actual.transform.GetChild(random2).gameObject.activeSelf ||
            random2 == random1)
        {
            random2 = Random.Range(0, 8);
        }

        //objeto 1
        GameObject p = Instantiate(poner);

        p.transform.position = actual.transform.GetChild(random1).GetChild(0).position;// + new Vector3(0, 1, 0);
        p.transform.position -= new Vector3(0, 0.5f, 0);
        // en el eje z hay que sumar la rotacion que diga el diccionario
        Vector3 rotacionActual = p.transform.rotation.eulerAngles;
        rotacionActual.y += orientacionesObstaculos[random1];
        Quaternion nuevaRotacion = Quaternion.Euler(rotacionActual);
        p.transform.rotation = nuevaRotacion;

        p.transform.SetParent(actual.transform);
        GameObject hijo = p.transform.GetChild(0).gameObject;
        hijo.transform.SetParent(actual.transform);
        DestroyImmediate(p);
        hijo.transform.SetSiblingIndex(childCount - 2);

        //objeto 2
        GameObject p2 = Instantiate(poner);

        p2.transform.position = actual.transform.GetChild(random2).GetChild(0).position;// + new Vector3(0, 1, 0);
        p2.transform.position -= new Vector3(0, 0.5f, 0);
        rotacionActual = p2.transform.rotation.eulerAngles;
        rotacionActual.y += orientacionesObstaculos[random2];
        nuevaRotacion = Quaternion.Euler(rotacionActual);
        p2.transform.rotation = nuevaRotacion;

        p2.transform.SetParent(actual.transform);
        hijo = p2.transform.GetChild(0).gameObject;
        hijo.transform.SetParent(actual.transform);
        DestroyImmediate(p2);
        hijo.transform.SetSiblingIndex(childCount - 1);
       
    }


    // Esta funcion sirve para modificar una fila y hacerla única.
    //devuelve el numero de agujeros
    public int agujerosFila(GameObject actual)
    {
        // En funcion de ese random, determinamos
        // cuantos agujeros tiene esta fila
        float random = Random.value;

        //[)[)[]
        if (random >= 0f && random < 0.2f)
        {
            // Se elimina uno
            eliminarEscalon(1, actual);
            return 1;
            
        }
        else if (random >= 0.2f && random < 0.5f)
        {
            // Se elimina dos
            eliminarEscalon(2, actual);
            return 2;
        }
        else if (random >= 0.5f && random < 0.8f)
        {
            // Se elimina tres
            eliminarEscalon(3, actual);
            return 3;
        }
        else if (random >= 0.8f && random < 0.9f)
        {
            // Se elimina cuatro
            eliminarEscalon(4, actual);
            return 4;
        }
        else // if (random >= 0.9f && random < 1f)
        {
            // Se elimina cinco
            eliminarEscalon(5, actual);
            return 5;
        }
    }

    public void eliminarEscalon(int num, GameObject actual)
    {
        GameObject hijo;
        for (int i=0; i < num; i++)
        {
            // Obtengo un hijo y lo quito si no estaba ya quitado
            hijo = actual.transform.GetChild(Random.Range(0, 8)).gameObject;
            if (!hijo.activeSelf)
            {
                i--;
            }
            else
            {
                hijo.SetActive(false);
            }
        }
        
    }

    public bool filaRotatoria(GameObject actual)
    {
        float random = Random.value;
        //con un 20% de opciones, gira
        if (random<=0.2)
        {
            actual.AddComponent<FilaRota>();
            return true;
        }
        return false;
    }
}
