using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Granada : MonoBehaviour
{
    public ControladorBarco control;
    public bool destruccion = false;
    public float tActual = 0f;
    public float tTotal = 1.2f;
    public float radio = 30f;

    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        control = GameObject.Find("ControladorUI").GetComponent<ControladorBarco>();
        //GetComponent<Rigidbody>().material.bounciness = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (destruccion)
        {
            tActual += Time.deltaTime;
            if (tTotal < tActual)
            {
                //inicio la explosion
                // crea una esfera que abarca el objeto actual y busca otros objetos en la capa especificada
                Collider[] hits = Physics.OverlapSphere(transform.position, radio);

                // recorre todos los objetos que intersectan con la esfera y los destruyo
                // (les alcanzó la granada)
                foreach (Collider hit in hits)
                {
                    Debug.Log(hit.gameObject);
                    if (hit.tag == "Enemigo")
                    {
                        Destroy(hit.gameObject);
                        control.IncrementoPuntuacion(2);
                        control.activarDano();
                    }
                }
                //explosiono y destruyo la granada
                GameObject exp = Instantiate(explosion);
                exp.transform.position = transform.position;
                control.activarGranada();
                Destroy(this.gameObject);
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Respawn")
        {
            destruccion = true;
        }
        else if(collision.gameObject.tag == "Enemigo")
        {
            //Si doy con un enemigo, que caiga a plomo al suelo
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.constraints = rb.constraints | RigidbodyConstraints.FreezePositionX;
            rb.constraints = rb.constraints | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radio);
    }
}
