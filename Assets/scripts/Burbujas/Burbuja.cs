using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burbuja : MonoBehaviour
{
    public bool explotada = false;

    public bool esAzul = true;

    public ControladorBurbuja control;

    // Start is called before the first frame update
    void Start()
    {
        control = GameObject.Find("ControladorUI").GetComponent<ControladorBurbuja>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void explotar()
    {
        if (!explotada && control != null &&  !control.heGanado() && control.empezado)
        {
            explotada = true;

            GetComponent<Animator>().SetBool("explotada", true);

            if (esAzul)
            {
                control.IncrementoPuntuacion(1);
                control.modificarTiempo(-0.08f);
                control.DecrementoAzules();
                control.sonidoAzul();
            }
            else
            {
                control.DecrementoPuntuacion(1);
                control.modificarTiempo(0.2f);
                control.sonidoRoja();
            }
        }
    }

    public void destruir()
    {
        Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Cilindro")
        {
            explotar();
        }
    }

    /*void OnMouseOver()
    {
       if(!explotada && !control.heGanado())
            explotar();
    }*/
}
