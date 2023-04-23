using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guide : MonoBehaviour
{

    public TMPro.TextMeshProUGUI speaker;
    public TMPro.TextMeshProUGUI text;

    public List<string> guideSpeaker = new List<string>();
    public List<string> guideText = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Active), 2f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) Invoke(nameof(Guiding), 0f);
    }

    void Active()
    {
        speaker.gameObject.transform.parent.parent.gameObject.SetActive(true);
        text.gameObject.transform.parent.parent.gameObject.SetActive(true);
        Invoke(nameof(Guiding), 0f);
    }

    void Guiding()
    {
        if (guideText.Count != 0 && guideSpeaker.Count != 0)
        {
            speaker.text = guideSpeaker[0];
            text.text = guideText[0];
            guideSpeaker.RemoveAt(0);
            guideText.RemoveAt(0);
        }
        else
        {
            speaker.gameObject.transform.parent.parent.gameObject.SetActive(false);
            text.gameObject.transform.parent.parent.gameObject.SetActive(false);
        }
    } 

    /*
    void Guiding()
    {
        int words = guideText[0].Split().Length;
        text.text = guideText[0];
        guideText.RemoveAt(0);

        if (guideText.Count != 0) Invoke(nameof(Guiding), words / 2.5f);
        else Invoke(nameof(Waiting), 1f);
    }

    void Waiting()
    {
        if (guideText.Count != 0)
        {
            int words = guideText[0].Split().Length;
            text.text = guideText[0];
            guideText.RemoveAt(0);
            Invoke(nameof(Guiding), words / 2.5f);
        }
        else Invoke(nameof(Waiting), 1f);
    }
    */
}
