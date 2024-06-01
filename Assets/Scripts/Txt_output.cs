using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Txt_output : MonoBehaviour
{
    [SerializeField] 
    string txt;
    [SerializeField]
    string asterisk;
    private string currentTxt;
    [SerializeField]
    int fontSize = 24;
    [SerializeField]
    float currentSymDelay, skipSymDelay, normalSymDelay;

    public TextMeshProUGUI outputText;
   
    void StartOutput(string txt)
    {
        outputText.fontSize = fontSize;
        currentTxt = "";
        outputText.text = "";
        currentSymDelay = normalSymDelay;
        StartCoroutine(txt_print(txt));
    }

    IEnumerator txt_print(string txt)
    {
        foreach(var sym in txt) {
            currentTxt += sym;
            outputText.text = currentTxt + asterisk;
            yield return new WaitForSeconds(currentSymDelay);
        }

        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            currentSymDelay = skipSymDelay;
        }
    }
}
