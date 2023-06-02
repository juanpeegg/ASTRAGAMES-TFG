using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parpadeo : MonoBehaviour
{
    public Material matOpaco;
    public Material matTransp; 
    public bool activado = false;

    private int signo = -1;
    public float aumento = 5f;

    // Start is called before the first frame update
    void Start()
    {
        matOpaco = GetComponent<SkinnedMeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (activado)
        {
            Material mat = GetComponent<SkinnedMeshRenderer>().material;
            Color color = mat.color;
            color.a += aumento * Time.deltaTime * signo;
            mat.color = color;
            if (mat.color.a < 0.06f)
            {
                signo = 1;
            }
            else if(mat.color.a > 0.94f)
            {
                signo = -1;
            }
        }
    }

    public void restaurar()
    {
        Material mat = GetComponent<SkinnedMeshRenderer>().material;
        Color color = mat.color;
        color.a = 1f;
        mat.color = color;
        GetComponent<SkinnedMeshRenderer>().material = matOpaco;
    }

    public void cambiar()
    {
        GetComponent<SkinnedMeshRenderer>().material = matTransp;
    }
}
