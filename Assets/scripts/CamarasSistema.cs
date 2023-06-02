using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CamarasSistema : MonoBehaviour
{
    Dropdown dropdown;
    public int maxOptionLength = 20;

    void Start()
    {
        // obtener una referencia al componente Dropdown
        dropdown = GetComponent<Dropdown>();

        // crear una lista de opciones
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        var devices = WebCamTexture.devices;
        for (int cameraIndex = 0; cameraIndex < devices.Length; cameraIndex++)
        {
            options.Add(new Dropdown.OptionData(devices[cameraIndex].name));
        }

        // Recortar opciones largas con puntos suspensivos
        foreach (Dropdown.OptionData option in options)
        {
            if (option.text.Length > maxOptionLength)
            {
                option.text = option.text.Substring(0, maxOptionLength - 3) + "...";
            }
        }

        // asignar la lista de opciones al Dropdown
        dropdown.options = options;
        // Obtener el valor guardado en PlayerPrefs
        int defaultValue = PlayerPrefs.GetInt("camara", 0);

        // Establecer el valor predeterminado del Dropdown
        dropdown.value = defaultValue;

    }
}
