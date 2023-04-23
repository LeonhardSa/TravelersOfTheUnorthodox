using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateGuide : MonoBehaviour
{

    public bool clearAllText;
    public string speaker;
    public string text;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            if (clearAllText)
            {
                GameObject.FindObjectOfType<Guide>().guideSpeaker.Clear();
                GameObject.FindObjectOfType<Guide>().guideText.Clear();
            }

            GameObject.FindObjectOfType<Guide>().guideSpeaker.Add(speaker);
            GameObject.FindObjectOfType<Guide>().guideText.Add(text);

            this.gameObject.SetActive(false);
        }
    }
}
