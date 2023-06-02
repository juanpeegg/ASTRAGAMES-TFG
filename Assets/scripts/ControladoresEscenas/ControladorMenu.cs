using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class ControladorMenu : MonoBehaviour
{
    public GameObject sonidoClick;

    public void Clicar()
    {
        sonidoClick.SetActive(true);
        Invoke("DesactivarSonidoClick", 1);
    }

    public void IrAPricipal()
    {
        Clicar();
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void IrAMinijuegos()
    {
        Clicar();
        SceneManager.LoadScene("MenuMinijuegos");
    }

    public void DesactivarSonidoClick()
    {
        sonidoClick.SetActive(false);
    }

    public void Volver()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}