using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flicker : MonoBehaviour
{
    [SerializeField]
    Color col1,col2;
    [SerializeField]
    float i1,i2;
    [SerializeField]
    MeshRenderer renderer;
    [SerializeField]
    Light luz;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cambia();
    }

    void cambia(){
        float t=Random.value;
        renderer.materials[1].SetColor("_BaseColor",Color.Lerp(col1,col2,t));
        luz.intensity=(1-t)*i1+t*i2;
        Invoke("cambia",4f*Random.value);
    }
}
