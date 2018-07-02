using System.Collections;
using System.IO;

using UnityEngine;

public class Screenshot : MonoBehaviour
{		
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.F12))
			StartCoroutine("TakeScreenshot");
	}

	IEnumerator TakeScreenshot()
	{
		yield return new WaitForEndOfFrame();

		Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

		texture.ReadPixels(new Rect(0,0,Screen.width, Screen.height),0,0);
		texture.Apply();

		yield return 0;

		byte[] bytes = texture.EncodeToPNG();

		File.WriteAllBytes(Application.dataPath + "/../Screenshots/screenshot" + PlayerPrefs.GetInt("screenshotCount") + ".png", bytes);
		int count =  PlayerPrefs.GetInt("screenshotCount");
		count++;
		PlayerPrefs.SetInt("screenshotCount",count);

		DestroyObject(texture);

		Debug.Log("SCREENSHOT TAKEN");
	}
}
