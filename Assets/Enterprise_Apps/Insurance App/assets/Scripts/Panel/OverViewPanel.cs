using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class OverViewPanel : MonoBehaviour,IPanel
{
    public Text casenumber;
    public Text nameTitle;
    public Text date;
    public Text location;
   
    public RawImage photo;
    public Text photonotes;

    public void OnEnable()
    {
        casenumber.text =  "Case No. " + UIManager.Intance.activecase.caseId;
        nameTitle.text = UIManager.Intance.activecase.name;
        date.text = DateTime.Today.ToString();


        Texture2D reimage = new Texture2D(1,1);
        reimage.LoadImage(UIManager.Intance.activecase.photo);


        photo.texture = (Texture)reimage;
        photonotes.text = "Photo : /n" + UIManager.Intance.activecase.photonotes;
    }

    public void ProcessInfo()
    {
        throw new System.NotImplementedException();
    }
}
