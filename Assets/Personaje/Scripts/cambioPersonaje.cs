using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cambioPersonaje : MonoBehaviour
{
    int numPersonaje=0;
    [SerializeField]
    GameObject[] personajes;
    int n;
    // Start is called before the first frame update
    void Start()
    {
        n=personajes.Length;
        ver();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadPlus)){
            numPersonaje=(numPersonaje+1)%n;
            ver();
        }
        if(Input.GetKeyDown(KeyCode.KeypadMinus)){
            numPersonaje=numPersonaje-1;
            if(numPersonaje<0){numPersonaje+=n;}
            ver();
        }
    }

    void ver(){
        for(int j=0;j<n;j++){
            personajes[j].SetActive(numPersonaje==j);
        }
    }
}
