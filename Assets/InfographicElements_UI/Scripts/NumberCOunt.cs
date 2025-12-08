using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NumberCOunt : MonoBehaviour
{
    Text txt;
    string numbers;
    int num = 0;
    string[] count ={"1245,12", "2356,23" , "3467,34" , "4578,54" , "5689,65" , "679076" };
    // Start is called before the first frame update
    void Awake()
    {
        txt = GetComponent<Text>();
        numbers = txt.text;
        txt.text = "";
        num = Random.Range(0, count.Length);
        // TODO: add optional delay when to start
        StartCoroutine("PlayNumber");
    }

    IEnumerator PlayNumber()
    {
        txt.text = count[num].ToString();
        if (num < 5)
        {
            num++;
        }
        else
        {
            num = 0;
        }
     
        yield return new WaitForSeconds(0.125f);
        StartCoroutine("PlayNumber");
    }
}
