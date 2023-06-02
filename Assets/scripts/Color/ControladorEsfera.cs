using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorEsfera : ControladorMenu
{
    public string colorSelec;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Pintura")
        {
            //col.gameObject.transform.localScale = new Vector3(1.6f*col.gameObject.transform.localScale.x, col.gameObject.transform.localScale.y, 1.6f*col.gameObject.transform.localScale.z);
            colorSelec = col.gameObject.tag;
            Clicar();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Pintura")
        {
            //col.gameObject.transform.localScale = new Vector3(30, 30, 30);
            colorSelec = "Nada";
        }
    }
}
