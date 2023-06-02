using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class camaraSkyRoad : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    float smooth=0.2f, dt=0.01f;

    [SerializeField]
    Text counter;


    float d,t;
    Vector3 Q;
    robot miRobot;
    Animator animator;

    public bool empezar, corriendo;

    public GameObject sonidoA;
    public GameObject sonidoB;

    // Start is called before the first frame update
    void Start()
    {
        
        miRobot=target.gameObject.GetComponent<robot>();
        animator=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(empezar){
            animator.SetBool("iniciar",true);
            empezar=false;
        }
        if(empezar==false & Input.GetKeyDown(KeyCode.Space)){
            empezar=true;
        }

        if(corriendo){
            Q=new Vector3(Q.x,Q.y,target.position.z+d);
            transform.position=Vector3.Lerp(transform.position,Q,smooth);
        }
        
    }

    public void fin(){
        animator.enabled=false;
        
        
        Q=transform.position;
        d=transform.position.z-target.position.z;
        
        Invoke("paso3",0.2f);
        Invoke("paso2",1.2f);
        Invoke("paso1",2.2f);
        Invoke("inicio",3.2f);
        
    }

    void paso3(){
        ActivarSonidoA();
        counter.text="3";
    }

    void paso2(){
        ActivarSonidoA();
        counter.text="2";
    }

    void paso1(){
        ActivarSonidoA();
        counter.text="1";
    }

    void inicio(){
        ActivarSonidoB();
        counter.text="";
        corriendo=true;
        miRobot.iniciar();
    }

    public void ActivarSonidoA()
    {
        sonidoA.SetActive(true);
        Invoke("DesactivarSonidoA", 1);
    }

    public void ActivarSonidoB()
    {
        sonidoB.SetActive(true);
        Invoke("DesactivarSonidoB", 1);
    }

    public void DesactivarSonidoA()
    {
        sonidoA.SetActive(false);
    }

    public void DesactivarSonidoB()
    {
        sonidoB.SetActive(false);
    }
}
