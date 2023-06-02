using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destruye2 : MonoBehaviour
{
    [SerializeField]
    float t=2;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("fin2",t);
    }

    // Update is called once per frame
    void fin2()
    {
        Destroy(gameObject);
    }
}
