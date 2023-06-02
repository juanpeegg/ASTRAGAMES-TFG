using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
 
public class Url : MonoBehaviour 
{	
	public IEnumerator GetTexture(string imagen)
	{
		string path = Application.dataPath + "/StreamingAssets/" + imagen;
		Debug.Log(path);
		UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);
		yield return www.SendWebRequest();

		if(www.result != UnityWebRequest.Result.Success)
		{
			Debug.Log(www.error);
		}
		else
		{
			Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        	gameObject.GetComponent<RawImage>().texture = myTexture;

		}	
	}

	public IEnumerator GetSound(string sonido)
	{
		string path = Application.dataPath + "/StreamingAssets/" + sonido;
		Debug.Log(path);
		UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file:///" + path, AudioType.MPEG);
		yield return www.SendWebRequest();

		if(www.result != UnityWebRequest.Result.Success)
		{
			Debug.Log(www.error);
		}
		else
		{
			AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
        	gameObject.GetComponent<AudioSource>().clip = myClip;
			gameObject.GetComponent<AudioSource>().Play();
		}	
	}

	void OnGUI()
    {
        gameObject.GetComponent<AudioSource>().volume = GameObject.Find("SliderVolumen").GetComponent<Slider>().value;;
    }
}