using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using UsersDatabase;
using ConfigurationDatabase;

public class MenuPrincipal : ControladorMenu
{
    void Start()
    {
        DataSaver.deleteData("players");
    }

    public void IrARegistro()
    {
        Clicar();
        SceneManager.LoadScene("Registro");
    }

    public void IrALogin()
    {
        Clicar();
        SceneManager.LoadScene("Login");
    }

    public void IrAJugarDefecto()
    {
        Clicar();
        SceneManager.LoadScene("MenuMinijuegos");
    }

    public void IrAEditar()
    {
        Clicar();
        SceneManager.LoadScene("EditarUsuarios");
    }

    public void IrABorrar()
    {
        Clicar();
        SceneManager.LoadScene("BorrarUsuarios");
    }

    public void Salir()
    {
        //ArkanoidDb configArkanoid = new ArkanoidDb();
        //configArkanoid.deleteUsuario("Jugador");
        //configArkanoid.close();
        
        sonidoClick.SetActive(true);
        Application.Quit();
    }
}
