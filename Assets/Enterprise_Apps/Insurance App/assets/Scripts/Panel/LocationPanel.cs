using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LocationPanel : MonoBehaviour,IPanel
{
    public RawImage mapImage;
    public InputField mapNotes;
    public Text caseno;

    public string apiKey;
    public float xCord, Ycord;
    public int zoom;
    public int size;
    public string url = "https://maps.googleapis.com/maps/api/staticmap?";


    public IEnumerator Start()
    {
        if(Input.location.isEnabledByUser == true)
        {
            Input.location.Start();
            int maxWait = 20;
            while(Input.location.status==LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1.0f);
                maxWait--;
            }

            if (maxWait < 0)
            {
                Debug.Log("TimeOut");
                yield break;
            }

            if(Input.location.status== LocationServiceStatus.Failed)
            {
                Debug.Log("unable to detect location");
                    
            }
            else
            {
                xCord = Input.location.lastData.latitude;
                Ycord = Input.location.lastData.longitude;
            }


            Input.location.Stop();
        }
        StartCoroutine(GetMapImage());
    }

    public void OnEnable()
    {
        caseno.text = "Case No." + UIManager.Intance.activecase.caseId;
    }

    IEnumerator GetMapImage()
    {
        url += "center=" + xCord + "," + Ycord + "&zoom=" + zoom + "&size=" + size + "x" + size + "&key=" + apiKey;
        using (UnityWebRequest  map = UnityWebRequestTexture.GetTexture(url))
        {
            yield return map.SendWebRequest();
            Debug.Log("map downloaded");

            if (map.error != null)
            {
                Debug.Log(map.error);
            }
            mapImage.texture = DownloadHandlerTexture.GetContent(map);
        }
    }
    public void ProcessInfo()
    {

    }
}
