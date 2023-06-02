using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bola : MonoBehaviour
{
    float r, ang;
    [SerializeField]
    float _velocity, factor; 
    
    float zMin = -220;

    Vector3 pos, dir;
    bool activo, hit;
    RaycastHit hitInfo;

    public GameObject controlador;

    [SerializeField]
    GameObject sonido;

    // Start is called before the first frame update
    void Start()
    {
        activo = false;
        pos = transform.position;
        hit = false;
        r = transform.localScale.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !activo)
        {
            activo = true;
            if (Random.value < 0.5f)
            {
                ang = Mathf.PI / 2 - (Mathf.PI / 4 - Random.value * Mathf.PI / 8);
            }
            else
            {
                ang = Mathf.PI / 2 + (Mathf.PI / 4 - Random.value * Mathf.PI / 16);
            }
            //Dirección que lleva la bola
            dir = Mathf.Cos(ang) * Vector3.right + Mathf.Sin(ang) * Vector3.forward;
            //Angulo que forma la direccion que tiene la bola y el eje forward (z)
            float ang2 = Vector3.Angle(dir, Vector3.forward);
            // Si el ángulo no está entre 30 y 50. Hay que modificar
            if (ang2 < 30 || ang2 > 150)
            {
                float l = dir.magnitude;
                if (dir.x > 0)
                {
                    dir = l * (dir + Vector3.right).normalized;
                }
                else
                {
                    dir = l * (dir - Vector3.right).normalized;
                }
            }
            //Angulo que forma la direccion que tiene la bola y el eje right (x)
            ang2 = Vector3.Angle(dir, Vector3.right);
            // Si el ángulo no está entre 40 y 140. Hay que modificar
            if (ang2 < 40 || ang2 > 140)
            {
                float l = dir.magnitude;
                if (dir.z > 0)
                {
                    dir = l * (dir + Vector3.forward).normalized;
                }
                else
                {
                    dir = l * (dir - Vector3.forward).normalized;
                }
            }

        }

        if (activo)
        {
            hit = false;

            //Lanzo 3 rayos, uno al centro y otros dos a los costados para detectar colisiones

            //Rayo al centro
            if (Physics.Raycast(transform.position, dir, out hitInfo, r))
            {
                hit = true;
                Vector3 N = hitInfo.normal;
                GameObject choque = hitInfo.collider.transform.gameObject;
                //Si he chocado con un ladrillo borro el ladrillo
                if (choque.tag == "ladrillo")
                {
                    Destroy(choque);
                    controlador.SendMessage("IncrementoPuntuacion", 10);
                }
                //Haya chocado con lo que haya chocado reboto
                Vector3 dir2 = Vector3.Reflect(dir, N);
                float l = dir.magnitude;
                Vector3 T = Vector3.Cross(N, Vector3.up);
                dir = dir2 + factor * (-0.2f + 0.4f * Random.value) * T;
                dir = l * (dir.normalized);
            }

            if (!hit)
            {
                //Rayo a la derechaa
                if (Physics.Raycast(transform.position + 10 * Vector3.right, dir, out hitInfo, r))
                {
                    hit = true;
                    Vector3 N = hitInfo.normal;
                    GameObject choque = hitInfo.collider.transform.gameObject;
                    if (choque.tag == "ladrillo")
                    {
                        Destroy(choque);
                        controlador.SendMessage("IncrementoPuntuacion", 10);
                    }
                    Vector3 dir2 = Vector3.Reflect(dir, N);
                    float l = dir.magnitude;
                    Vector3 T = Vector3.Cross(N, Vector3.up);
                    dir = dir2 + factor * (-0.2f + 0.4f * Random.value) * T;
                    dir = l * (dir.normalized);
                }
            }

            if (!hit)
            {
                //Rayo a la izquierda
                if (Physics.Raycast(transform.position - 10 * Vector3.right, dir, out hitInfo, r))
                {
                    hit = true;
                    Vector3 N = hitInfo.normal;
                    GameObject choque = hitInfo.collider.transform.gameObject;
                    if (choque.tag == "ladrillo")
                    {
                        Destroy(choque);
                        controlador.SendMessage("IncrementoPuntuacion", 10);
                    }
                    Vector3 dir2 = Vector3.Reflect(dir, N);
                    float l = dir.magnitude;
                    Vector3 T = Vector3.Cross(N, Vector3.up);
                    dir = dir2 + factor * (-0.2f + 0.4f * Random.value) * T;
                    dir = l * (dir.normalized);
                }
            }

            //Si no he chocado, pongo mi nueva posicion
            if (!hit)
            {
                transform.Translate(_velocity * dir * Time.deltaTime, Space.World);
            }
            else //Si si he chocado, pongo el sonidito del choque
            {
                GameObject.Instantiate(sonido);
            }
        }

        transform.position -= transform.position.y * Vector3.up;

        //Si no le he conseguido dar con la raqueta, se vuelve al minimo
        if (transform.position.z < zMin && activo)
        {
            activo = false;
            controlador.SendMessage("DecrementoPuntuacion", 2);
            Invoke("vuelve", 0.5f);
        }
    }

    public void vuelve()
    {
        activo = false;
        transform.position = pos;
    }

    public float Velocity
    {
        get { return _velocity; }
        set { _velocity = value; }
    }
}
