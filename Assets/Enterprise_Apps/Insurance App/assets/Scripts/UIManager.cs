using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager _instance;
    public static UIManager Intance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("uimanager is null");
            }

            return _instance;
        }
    }

    public Case activecase;
    public ClientInfoPanel clientinfopanel;
    public GameObject BorderPanel;
   

    public void Awake()
    {
        _instance = this;
    }

    public void CreateCase()
    {
        activecase = new Case();
        int randomcaseId = Random.Range(0, 1000);

        activecase.caseId =""+ randomcaseId;

        clientinfopanel.gameObject.SetActive(true);
        BorderPanel.SetActive(true);
    }


    public void Submit()
    {
        Case awscase = new Case();
        awscase = activecase;

        BinaryFormatter bf = new BinaryFormatter();

        string filepath = Application.persistentDataPath + "/case#" + awscase.caseId + ".dat";
        Debug.Log(Application.persistentDataPath);
        FileStream file = File.Create(filepath);
        bf.Serialize(file, awscase);
        file.Close();

        AwsManager.Instance.UploadToS3(filepath, awscase.caseId);
    }
}
