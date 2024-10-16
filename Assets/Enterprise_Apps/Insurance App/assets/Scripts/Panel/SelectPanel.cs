using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPanel : MonoBehaviour,IPanel

{
    public Text casedetails;

    public void OnEnable()
    {
        casedetails.text = UIManager.Intance.activecase.name;
    }
    public void ProcessInfo()
    {

    }
}
