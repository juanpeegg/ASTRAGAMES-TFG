using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public int valor;
    public Text valorTexto;

    public void OnSliderChanged(float value)
    {
        valor = (int) value;
        valorTexto.text = value.ToString();
        gameObject.GetComponent<Slider>().value = valor;
    }
}
