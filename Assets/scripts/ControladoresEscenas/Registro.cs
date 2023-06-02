using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UsersDatabase;
using System.Linq;
using System.Globalization;

public class Registro : ControladorMenu
{
    public InputField nombre;
    public Button aceptar;
    public GameObject msjError;
    public GameObject msjRegistro;

    public GameObject msjEncoding;

    private bool TieneCaracteresEspeciales(string value)
    {
        if (value == null) return false;

        var normalize = value.Normalize(NormalizationForm.FormD);

        var sb = new StringBuilder();

        foreach (var t in normalize.Where(t => CharUnicodeInfo.GetUnicodeCategory(t) != UnicodeCategory.NonSpacingMark))
            sb.Append(t);

        return (sb.ToString() != value);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (aceptar.interactable) Registrarse();
        }
    }

    public void Registrarse()
    {
        Clicar();

        UsuarioDb usuarioDb = new UsuarioDb();

        //Anadir usuario
        if (TieneCaracteresEspeciales(nombre.text))
        {
            msjError.SetActive(false);
            msjEncoding.SetActive(true);
        }
        else
        {
            int codigo = usuarioDb.addData(new UsuarioEntity(nombre.text));
            usuarioDb.close();

            if (codigo == 0)
            {
                //Se ha podido anadir al usuario correctamente
                msjEncoding.SetActive(false);
                msjError.SetActive(false);
                msjRegistro.SetActive(true);
                Invoke("DesactivarMensajeRegistro", 1);
                Invoke("Volver", 1);
            }
            //El usuario ya estaba registrado
            else
            {
                msjEncoding.SetActive(false);
                msjError.SetActive(true);
            }
        }
    }

    public void VerificarInputs()
    {
        aceptar.interactable = (nombre.text.Length > 0);
    }

    public void DesactivarMensajeRegistro()
    {
        msjRegistro.SetActive(false);
    }
}
