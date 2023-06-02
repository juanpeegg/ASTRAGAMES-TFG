using OpenCVForUnity.CoreModule;
using OpenCVForUnity.DnnModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.UnityUtils.Helper;
using OpenCVForUnityExample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SigueControlAruco : MonoBehaviour
{

    public GameObject quad;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 punto = quad.GetComponent<ArUcoWebCamExampleTexture>().coordenadas;
        transform.position = new Vector3((float)punto.x,(float)punto.y, transform.position.z);

    }
}
