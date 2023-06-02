using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnityExample;

public class RaquetaArUco : MonoBehaviour
{
    
    float _xMin, _xMax;

    public float _smooth;
    public int _factor;
    [SerializeField][Range(1, 320)] int intervalo = 160;
    ArUcoWebCamExampleTexture arucoDetector;

    [SerializeField]
    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        arucoDetector = (ArUcoWebCamExampleTexture)FindObjectOfType(typeof(ArUcoWebCamExampleTexture));
        gameObject.transform.localScale = new Vector3(150, 20, 20);
    }

    // Update is called once per frame
    void Update()
    {
        //Las coordenadas ya llegan pasadas a unity
        Vector3 input = new Vector3(-arucoDetector.coordenadas.x / intervalo, 0, 0); //Parecido a Input.GetAxis [-2, 2]

        pos = new Vector3(input.x * _factor, transform.position.y, transform.position.z);
        //El lerp sirve para ir de una posicion a otra poco a poco
        transform.position = Vector3.Lerp(transform.position, pos, _smooth);
        //El clamp sirve para limitar entre valores
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, _xMin, _xMax), transform.position.y, transform.position.z);
    }

    public void RecalcularLimites()
    {
        _xMin = -(-0.5f * gameObject.transform.localScale.x + 305);
        _xMax = (-0.5f * gameObject.transform.localScale.x + 305);
    }
}
