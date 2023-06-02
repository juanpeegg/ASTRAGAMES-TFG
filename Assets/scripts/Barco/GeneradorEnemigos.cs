using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GeneradorEnemigos : MonoBehaviour
{
    [SerializeField]
    GameObject transporte;
    [SerializeField]
    Transform AD, BD, AI, BI;

    public GameObject[] enemigos;

    public int enemigosDcha;

    public int enemigosIzq;

    private ControladorBarco control;


    // Start is called before the first frame update
    void Start()
    {
        control = GameObject.Find("ControladorUI").GetComponent<ControladorBarco>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void arrancarGenerador(int eneIzq, int eneDcha, int diana )
    {
        //si no hay dianas hay que quitar el script
        if (diana == 1)
        {
            this.gameObject.GetComponent<GeneradorDianas>().quietas = false;
        }
        
        enemigosIzq = eneIzq;
        enemigosDcha = eneDcha;

        generarEnemigos();
    }
    

    public void generarEnemigos()
    {
        
        for (int i=0; i < enemigosDcha; i++)
        {
            GameObject transport=Instantiate(transporte);
            transport.transform.position=AD.position+Random.value*new Vector3(BD.position.x-AD.position.x,0,0)+Random.value*new Vector3(0,0,BD.position.z-AD.position.z);
            
            transport.GetComponent<miNavMesh>().A=AD;
            transport.GetComponent<miNavMesh>().B=BD;
           
            GameObject enemigoNuevo = Instantiate(enemigos[Random.Range(0,5)]);
            enemigoNuevo.GetComponent<Animator>().speed=0.7f*(0.7f+0.6f*Random.value);
            enemigoNuevo.transform.parent=transport.transform;
            enemigoNuevo.transform.localPosition=new Vector3(0,-1.8f,0);

        }
        //y ahora coloco los de la izquierda
        for (int i = 0; i < enemigosIzq; i++)
        {
            Vector3 P=AI.position+Random.value*new Vector3(BI.position.x-AI.position.x,0,0)+Random.value*new Vector3(0,0,BI.position.z-AI.position.z);
            GameObject transport=Instantiate(transporte,P,Quaternion.identity);
            
            transport.GetComponent<miNavMesh>().A=AI;
            transport.GetComponent<miNavMesh>().B=BI;

            GameObject enemigoNuevo = Instantiate(enemigos[Random.Range(0,5)]);
            enemigoNuevo.GetComponent<Animator>().speed=0.7f*(0.7f+0.6f*Random.value);
            enemigoNuevo.transform.parent=transport.transform;
            enemigoNuevo.transform.localPosition=new Vector3(0,-1.8f,0);         
        }
    }
}
