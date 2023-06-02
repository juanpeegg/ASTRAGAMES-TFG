using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ajustar : MonoBehaviour
{
    [SerializeField]
    GameObject ob;
    [SerializeField]
    float h;
    [SerializeField]
    Transform P0,P1,P2,P3;
    Vector3 A,B,C,D;
    public Vector3[] pos;
    Mesh mesh, mesh2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)){
            A=P0.position;
            ob.transform.position=P0.transform.position;
            B=P1.position;
            C=P2.position;
            D=P3.position;
            B=B-A;
            C=C-A;
            D=D-A;
            A=A-A;
            mesh=ob.GetComponent<MeshFilter>().mesh;
            mesh2=new Mesh();
            
            pos=new Vector3[mesh.vertices.Length];
            ob.transform.position=A;
            for(int i=0;i<mesh.vertices.Length;i++){
                Vector3 P=mesh.vertices[i];
                Vector3 H1=A-P.y*(C-A);
                Vector3 H2=B-P.y*(D-B);
                Vector3 H=H1-P.x*(H2-H1);
                pos[i]=new Vector3(H.x,H.z,P.y)-A;
            }
            mesh2.vertices=pos;
            mesh2.triangles=mesh.triangles;
            mesh2.name="Mia";
            ob.GetComponent<MeshFilter>().mesh=mesh2;
        }
    }
}
