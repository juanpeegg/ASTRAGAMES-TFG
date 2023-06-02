using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miNavMesh : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;
    public Transform A, B;
    Vector3 P,Q,P1,P2,Ob;
    bool objetivo=false;
    float t,tMax;
    // Start is called before the first frame update

    
    void Start()
    {

        agent=GetComponent<UnityEngine.AI.NavMeshAgent>();
        P=A.position;
        Q=B.position;
        P1=new Vector3(Q.x-P.x,0,0);
        P2=new Vector3(0,0,Q.z-P.z);
        tMax=8+3*Random.value;
    }

    // Update is called once per frame
    void Update()
    {
        if(objetivo==false){
            Ob=P+Random.value*P1+Random.value*P2;
            objetivo=true;
            agent.destination=Ob;
            t=Time.time;
        }
        else{
            Vector3 miP=new Vector3(transform.position.x,Ob.y,transform.position.z);
            if(Vector3.Distance(miP,Ob)<3 | Time.time-t>tMax){
                agent.destination=transform.position;
                if(transform.childCount>0){
                    transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("Walk",false);
                }
                Invoke("ponObjetivo",4+4*Random.value);
            }
        }
    }

    void ponObjetivo(){
        
        if(transform.childCount>0){
            objetivo=false;
            tMax=8+3*Random.value;
            transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("Walk",true);
        }
        
    }
    
}
