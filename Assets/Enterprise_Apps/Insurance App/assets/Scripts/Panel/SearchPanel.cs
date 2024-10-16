using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchPanel : MonoBehaviour,IPanel

{
    public InputField casenumber;
    public SelectPanel selectpanel;

    public void ProcessInfo()
    {
        AwsManager.Instance.GetList(casenumber.text,()=> 
        {
            selectpanel.gameObject.SetActive(true);
        
        });
    }

   
}
