using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacionesRobot : MonoBehaviour
{
    Animator animator;
    public Animator animatorLuz;
    public bool running=false; 
    robot yo;
    // Start is called before the first frame update
    void Start()
    {
        animator=GetComponent<Animator>();
        yo=GetComponent<robot>();
    }

    // Update is called once per frame
    void Update()
    {
        running=yo.vel>2;;
        if(running){
            animator.SetBool("Running",true);
            animatorLuz.SetBool("parpadeo",true);
        }
        else{
            animator.SetBool("Running",false);
            animatorLuz.SetBool("parpadeo",false);
        }
    }
}
