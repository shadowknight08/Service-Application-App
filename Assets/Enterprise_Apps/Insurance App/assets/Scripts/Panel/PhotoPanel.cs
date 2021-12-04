using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PhotoPanel : MonoBehaviour,IPanel
{
    public Text caseId;
    public RawImage photoImage;
    public InputField photoNotes;
	public GameObject overViewpanel;

	public string imgpath;

     void OnEnable()
    {
		caseId.text = "Case No. " + UIManager.Intance.activecase.caseId;
	}

	public void ProcessInfo()	
	{

		byte[] imgdata = null;
        if (string.IsNullOrEmpty(imgpath) == false)
        {
			Texture2D image = NativeCamera.LoadImageAtPath(imgpath, 512, false);
		    imgdata = image.EncodeToPNG();

		}



		UIManager.Intance.activecase.photo = imgdata;
		UIManager.Intance.activecase.photonotes = photoNotes.text;
		overViewpanel.SetActive(true);



	}
	public void TakePictureButton()
    {
		TakePicture(512);
    }

	private void TakePicture(int maxSize)
	{
		NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
		{
			Debug.Log("Image path: " + path);
			if (path != null)
			{
				// Create a Texture2D from the captured image
				Texture2D texture = NativeCamera.LoadImageAtPath(path, maxSize);
				if (texture == null)
				{
					Debug.Log("Couldn't load texture from " + path);
					return;
				}

				photoImage.texture = texture;
				photoImage.gameObject.SetActive(true);
				imgpath = path;
			}
		}, maxSize);

		Debug.Log("Permission result: " + permission);
	}

	
    
}
