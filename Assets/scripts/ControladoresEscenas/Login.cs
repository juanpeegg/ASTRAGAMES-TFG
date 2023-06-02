using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UsersDatabase;

public class Login : ControladorMenu
{
    public InputField nombre;
    public GameObject msjError;
    public Button login;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (login.interactable) Logearse();
        }
    }

    public void Logearse()
    {
        Clicar();

        UsuarioDb usuarioDb = new UsuarioDb();

        //Comprobar que existe ya ese usuario
        UsuarioEntity usuario = usuarioDb.getUserByName(nombre.text);
        usuarioDb.close();

        //El usuario esta logueado
        if (usuario._nombre != "Error")
        {
            msjError.SetActive(false);
            Debug.Log("Usuario: " + usuario._nombre);
            //DEBE ENVIAR LA INFO AL JUEGO
            PlayerInfo saveData = new PlayerInfo();
            saveData.nombre = usuario._nombre;
            DataSaver.saveData(saveData, "players");

            SceneManager.LoadScene("MenuMinijuegos");
        }
        else
        {
            msjError.SetActive(true);
        }
    }

    public void VerificarInputs()
    {
        login.interactable = (nombre.text.Length > 0);
    }
}
