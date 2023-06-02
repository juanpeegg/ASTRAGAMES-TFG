using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnityExample;

public class RotacionEscalones : MonoBehaviour
{
    // Start is called before the first frame update
    ArUcoWebCamExampleTexture arucoDetector;
    public int sensibilidad;
    public bool arrancado = false;

    void Start()
    {
        arucoDetector = (ArUcoWebCamExampleTexture)FindObjectOfType(typeof(ArUcoWebCamExampleTexture));
    }

    // Update is called once per frame
    void Update()
    {
        if (arrancado)
        {
            float rotation = 50f * Time.deltaTime;

            if (arucoDetector.coordenadasPantalla.x > 320 + sensibilidad)
            {
                transform.Rotate(0f, rotation, 0f);
            }

            if (arucoDetector.coordenadasPantalla.x < 320 - sensibilidad)
            {
                transform.Rotate(0f, -rotation, 0f);
            }

            //----------CONTROL MANUAL-----------------------------------
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(0f, rotation, 0f);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(0f, -rotation, 0f);
            }
        }

    }
}
