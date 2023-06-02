using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class misSensores2 : MonoBehaviour
{

    [SerializeField]
    float r = 1;
    public float h = 2;
    [SerializeField]
    float sepV1 = 0.02f, sepV2 = 0.05f;

    [SerializeField]
    LayerMask suelos, obstaculos, items, victorias;

    public bool suelo, victoria;

    private ControladorNinja control;

    Vector3 CSuelo;

    public bool golpeDisponible = true;

    // Start is called before the first frame update
    void Start()
    {
        control = GameObject.Find("ControladorUI").GetComponent<ControladorNinja>();
    }

    // Update is called once per frame
    void Update()
    {

        CSuelo = transform.position - 1.5f * h * Vector3.up - 1.5f * (sepV1 + sepV2) * Vector3.up;
        suelo = Physics.CheckBox(CSuelo, new Vector3(0.5f * r, sepV2 - sepV1, 0.5f * r), transform.rotation, suelos);

        if (suelo)
        {
            control.ponerSonidoSalto();
            control.pararSonidoCaigo();
        }

        RaycastHit hit;

        // Lanzo un boxcast para detectar que fila estoy tocando en cada momento 
        if (Physics.BoxCast(CSuelo, new Vector3(0.5f * r, sepV2 - sepV1, 0.5f * r), Vector3.down, out hit, transform.rotation, 10, suelos))
        {
            //el 8 es el suelo
            if (hit.collider.gameObject.layer == 8)
            {
                control.romperFila = hit.collider.gameObject.transform.parent.gameObject;
            }
        }


        victoria = Physics.CheckBox(CSuelo, new Vector3(0.5f * r, sepV2 - sepV1, 0.5f * r), transform.rotation, victorias);

        //Si gano, cambio de animación
        if (victoria)
        {
            //transform.GetChild(3).GetComponent<Animator>().SetBool("gano", true);
        }


        // Hay que poner la mirilla.
        // Si toco el suelo, busco la posicion equivalente en la pantalla
        Debug.DrawRay(transform.position, Vector3.up * -40);
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 40, suelos))
        {
            Vector3 puntoPantalla = Camera.main.WorldToScreenPoint(hit.point);
            control.activarDestino(puntoPantalla);
        }
        else
        {
            control.desactivarDestino();
        }

    }


    void OnDrawGizmosSelected()
    {

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(-0.5f * h * Vector3.up - 0.5f * (sepV1 + sepV2) * Vector3.up, new Vector3(r, sepV2 - sepV1, r));

    }

}
