using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GenerarBurbujas : MonoBehaviour
{
    // Start is called before the first frame update
    int nPatrones;

    int patronActual;

    public int nFilas, nColumnas;

    public GameObject burbujaRoja, burbujaAzul;

    public GameObject referenciaInicial;

    //true para los azules y false para los rojos
    public bool[,,] arrayAlmacenado;

    public ControladorBurbuja control;

    bool regenerando = false;

    private string directoryPath = "StreamingAssets/Patrones";

    void Start()
    { 
        control = GameObject.Find("ControladorUI").GetComponent<ControladorBurbuja>();

        string folderPath = Path.Combine(Application.dataPath, directoryPath);

        ReadTextFilesInDirectory(folderPath);

        regenera();
        
    }

    void ReadTextFilesInDirectory(string path)
    {
        // Verificar si el directorio existe
        if (!Directory.Exists(path))
        {
            Debug.LogError("El directorio no existe: " + path);
            return;
        }

        // Obtener todos los archivos de texto en el directorio
        string[] textFiles = Directory.GetFiles(path, "*.txt");

        //Obtengo el número de patrones (número de archivos)
        nPatrones = textFiles.Length;
        arrayAlmacenado = new bool[nPatrones, nFilas, nColumnas];

        int patron = 0;
        
        // Leer cada archivo de texto
        foreach (string filePath in textFiles)
        {
            int i = nFilas-1;
            
            // Leer el contenido del archivo
            string fileContent = File.ReadAllText(filePath);

            string[] lines = fileContent.Split('\n');

            foreach (string s in lines)
            {
                int j = 0;
                string[] casilla = s.Split(' ');
                foreach (string ss in casilla)
                {
                    if(ss == "t")
                    {
                        arrayAlmacenado[patron, i, j] = true;
                    }
                    else
                        arrayAlmacenado[patron, i, j] = false;
                    j++;
                }
                i--;
            }
            patron++;

        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void regenera()
    {
        GameObject burbujasa = GameObject.Find("Burbujas");
        if (burbujasa != null)
            Destroy(burbujasa);

        control.nAzules = 0;
        GameObject burbujas = new GameObject();
        burbujas.name = "Burbujas";

        patronActual = Random.Range(0, nPatrones);

        for (int i = 0; i < nFilas; i++)
        {
            for (int j = 0; j < nColumnas; j++)
            {
                GameObject b;
                if (!arrayAlmacenado[patronActual,i,j])
                {
                    b = Instantiate(burbujaRoja);
                    //b.GetComponent<Burbuja>().esAzul = false;
                    
                }
                else
                {
                    b = Instantiate(burbujaAzul);
                    //b.GetComponent<Burbuja>().esAzul = true;
                    
                    control.nAzules++;
                }

                b.transform.position = referenciaInicial.transform.position + new Vector3(j * 29, i * 29, 0 );
                b.transform.SetParent(burbujas.transform);
            }
        }
        regenerando = false;

        control.empezado = false;
        if(control.puntos > 0)
            control.activarCuentaAtras();
    }

    public void regeneraInvocando()
    {
        if (!regenerando)
        {
            regenerando = true;
            Invoke("regenera", 0.75f);
            control.sonidoPantallaCompleta();
        }
    }
}
