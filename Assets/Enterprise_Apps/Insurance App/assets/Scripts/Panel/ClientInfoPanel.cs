using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ClientInfoPanel : MonoBehaviour,IPanel
{
    public Text CaseNumber;
    public InputField firstInput, lastInput;
    public LocationPanel locationpanel;

    public void OnEnable()
    {
        CaseNumber.text = "Case Number "+ UIManager.Intance.activecase.caseId;
    }
    public void ProcessInfo()
    {
        if(string.IsNullOrEmpty(firstInput.text)|| string.IsNullOrEmpty(lastInput.text))
        {
            Debug.Log("input is empty");

        }
        else
        {
            UIManager.Intance.activecase.name = firstInput.text+ " "+ lastInput.text;
            locationpanel.gameObject.SetActive(true);
        }
    }
}
