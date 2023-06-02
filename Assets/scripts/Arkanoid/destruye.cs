using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destruye : MonoBehaviour
{
    [SerializeField]
    float t=0.5f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("fin",t);
    }



    void fin(){
        Destroy(gameObject);
    }
}
