using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UsersDatabase;

public class EditarConfiguracion : ControladorMenu
{
    /*
    // Start is called before the first frame update
    public GameObject myPrefab;
    public GameObject contenedor;

    public Button btnEditar;

    public GameObject msjEdicion;
    List<UsuarioEntity> usuarios;


    void Start()
    {
        UsuarioDb usuarioDb = new UsuarioDb();
        usuarios = usuarioDb.getAllUsers();
        usuarioDb.close();

        foreach (var usuario in usuarios)
        {
            GameObject fila = Instantiate(myPrefab);
            fila.transform.SetParent(contenedor.transform);
            fila.gameObject.transform.GetChild(0).gameObject.GetComponent<Dropdown>().value = usuario._dificultad;
            fila.gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = usuario._nombre;
        }
    }

    public void Update()
    {
        int i = 0;
        int dificultadUI = -1;

        while (!btnEditar.interactable && i < usuarios.Count)
        {
            dificultadUI = contenedor.gameObject.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Dropdown>().value;
            if (usuarios[i]._dificultad != dificultadUI)
            {
                btnEditar.interactable = true;
            }
            i++;
        }


        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (btnEditar.interactable) Editar();
        }
    }

    public void Editar()
    {
        Clicar();

        int dificultadUI;
        int i = 0;

        //Recorre toda la BD 
        foreach (var usuario in usuarios)
        {
            dificultadUI = contenedor.gameObject.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Dropdown>().value;

            //Si se ha efectuado algun cambio se debe de modificar la BD
            if (usuario._dificultad != dificultadUI)
            {
                UsuarioDb usuarioDb = new UsuarioDb();
                usuarioDb.updateDificultad(usuario._nombre, dificultadUI.ToString());
                usuarioDb.close();
            }
            i++;
        }
        msjEdicion.SetActive(true);
        Invoke("DesactivarMensajeEdicion", 1);
        Invoke("Volver", 1);
    }

    public void DesactivarMensajeEdicion()
    {
        msjEdicion.SetActive(false);
    }*/
}
