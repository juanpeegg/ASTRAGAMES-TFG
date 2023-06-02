using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnityExample;

public class Raqueta : MonoBehaviour
{
    
    float _xMin, _xMax;

    public float _smooth;
    public int _factor;
    [SerializeField][Range(1, 320)] int intervalo = 160;
    FaceDetectionWebCamTextureExample faceDetector;


    [SerializeField]
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        faceDetector = (FaceDetectionWebCamTextureExample)FindObjectOfType(typeof(FaceDetectionWebCamTextureExample));
        gameObject.transform.localScale = new Vector3(150, 20, 20);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = new Vector3((faceDetector.centroRectangulo.x - 320) / intervalo, 0, 0); //Parecido a Input.GetAxis [-2, 2]
        //Debug.Log(input);
        //transform.Translate(input * _velocity * Time.deltaTime, Space.World);

        float x = transform.position.x;
        //transform.position = new Vector3(Mathf.Clamp(x, _xMin, _xMax), transform.position.y, transform.position.z);
        pos = new Vector3(input.x * _factor, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, pos, _smooth);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, _xMin, _xMax), transform.position.y, transform.position.z);
    }

    public void RecalcularLimites()
    {
        _xMin = -(-0.5f * gameObject.transform.localScale.x + 305);
        _xMax = (-0.5f * gameObject.transform.localScale.x + 305);
    }
}
