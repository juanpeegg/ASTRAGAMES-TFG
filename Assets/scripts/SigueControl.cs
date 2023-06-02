using OpenCVForUnity.CoreModule;
using OpenCVForUnity.DnnModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.UnityUtils.Helper;
using OpenCVForUnityExample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SigueControl : MonoBehaviour
{

    private string[] partes = { "Nose 0\n", "Neck 1\n", "RShoulder 2\n", "RElbow 3\n", "RWrist 4\n", "LShoulder 5\n", "LElbow 6\n", "LWrist 7\n", "RHip 8\n", "RKnee 9\n", "RAnkle 10\n", "LHip 11\n", "LKnee 12\n", "LAnkle 13\n", "REye 14\n", "LEye 15\n", "REar 16\n", "LEar 17\n" };

    public GameObject quad;

    [Header(" Nose 0\n Neck 1\n RShoulder 2\n RElbow 3\n RWrist 4\n LShoulder 5\n LElbow 6\n LWrist 7\n RHip 8\n RKnee 9\n RAnkle 10\n LHip 11\n LKnee 12\n LAnkle 13\n REye 14\n LEye 15\n REar 16\n LEar 17")]
    public int id = 0;

    public Text texto;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        //Controlamos que no se metan valores fuera de rango
        if (id > 17)
        {
            id = 0;
        }

        texto.text = partes[id];
    }

    // Update is called once per frame
    void LateUpdate()
    {

        //GameObject esfera = GameObject.Find("esfera");
        //Con los cálculos de abajo se pasa del sistema de coordenadas de opencv al de unity
        //Obtengo el script para el largo y ancho del panel
        WebCamTextureToMatHelper webCamTextureToMatHelper = quad.GetComponent<WebCamTextureToMatHelper>();
        int largo = webCamTextureToMatHelper.requestedWidth / 2; //320
        int alto = webCamTextureToMatHelper.requestedHeight / 2; //240
        Point punto = quad.GetComponent<PoseTextureWebCamExample>().getPunto(id);
        transform.position = new Vector3((float)punto.x - largo, alto - (float)punto.y, transform.position.z);

    }

    public void changePoint()
    {
        id++;
    }

    public void salir()
    {
        Application.Quit();
    }
}
