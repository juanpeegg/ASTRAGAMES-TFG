using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DbTestProUsuario : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //UsuarioDb usuarioDb = new UsuarioDb();

        //Add Data
        //usuarioDb.addData(new UsuarioEntity("Kike", 3));
        //usuarioDb.close();


        //Fetch All Data
        /*UsuarioDb usuarioDb2 = new UsuarioDb();
        System.Data.IDataReader reader = usuarioDb2.getAllData();

        int fieldCount = reader.FieldCount;
        List<UsuarioEntity> myList = new List<UsuarioEntity>();
        while (reader.Read())
        {
            UsuarioEntity entity = new UsuarioEntity(reader[0].ToString(),
                                                       reader[1].ToString());

            Debug.Log("id: " + entity._idUsuario);
			Debug.Log("nombre: " + entity._nombre);
			Debug.Log("dificultad: " + entity._dificultad);
            myList.Add(entity);
        }*/

    }

    // Update is called once per frame
    void Update()
    {

    }
}