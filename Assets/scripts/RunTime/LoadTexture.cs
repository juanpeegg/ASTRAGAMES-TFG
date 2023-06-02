using UnityEngine;
using UnityEngine.UI;

public class LoadTexture : MonoBehaviour {
	Texture2D myTexture;

	// Use this for initialization
	void Start () 
    {
		// load texture from resource folder
		myTexture = Resources.Load("SampleImage") as Texture2D;

		GameObject plano = GameObject.Find("Quad");
        plano.GetComponent<Renderer>().material.SetTexture("_BaseMap", myTexture);
	}
}