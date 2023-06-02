using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UsersDatabase;
using ScoresDatabase;
using ConfigurationDatabase;

public class BorrarUsuarios : ControladorMenu
{
    // Start is called before the first frame update
    public GameObject myPrefab;
    public GameObject contenedor;
    public GameObject msjBorrado;
    public Button btnBorrar;
    List<UsuarioEntity> usuarios;
    bool seleccionado;
    void Start()
    {
        UsuarioDb usuarioDb = new UsuarioDb();
        usuarios = usuarioDb.getAllUsers();
        usuarioDb.close();

        foreach (var usuario in usuarios)
        {
            GameObject fila = Instantiate(myPrefab);
            fila.transform.SetParent(contenedor.transform);
            fila.gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = usuario._nombre;
        }
    }

    public void Update()
    {
        int i = 0;
        while (!btnBorrar.interactable && i < usuarios.Count)
        {
            seleccionado = contenedor.gameObject.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Toggle>().isOn;
            if (seleccionado)
            {
                btnBorrar.interactable = true;
            }
            i++;
        }


        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (btnBorrar.interactable) Borrar();
        }
    }
    public void Borrar()
    {
        Clicar();
        int i = 0;
        //Recorre toda la BD 
        foreach (var usuario in usuarios)
        {
            seleccionado = contenedor.gameObject.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Toggle>().isOn;

            //Al borrar un usuario se deben borrar todas las tablas de la BD implicadas
            if (seleccionado)
            {
                //Tabla usuario
                UsuarioDb usuarioDb = new UsuarioDb();
                usuarioDb.deleteUsuario(usuario._nombre);
                usuarioDb.close();

                //Tabla puntuaciones
                PuntuacionesDb puntuacionDB = new PuntuacionesDb();
                puntuacionDB.deleteUsuario(usuario._nombre);
                puntuacionDB.close();

                //Tablas de configuración
                ArkanoidDb arkanoidDb = new ArkanoidDb();
                arkanoidDb.deleteUsuario(usuario._nombre);
                arkanoidDb.close();

                SkyroadsDb skyroadsDb = new SkyroadsDb();
                skyroadsDb.deleteUsuario(usuario._nombre);
                skyroadsDb.close();

                ArkanoidDb colorDb = new ArkanoidDb();
                colorDb.deleteUsuario(usuario._nombre);
                colorDb.close();
            }
            i++;
        }
        msjBorrado.SetActive(true);
        Invoke("DesactivarMensajeBorrado", 1);
        Invoke("Volver", 1);
    }

    public void DesactivarMensajeBorrado()
    {
        msjBorrado.SetActive(false);
    }
}
