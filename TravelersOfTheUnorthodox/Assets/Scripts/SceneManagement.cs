using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneManagement : MonoBehaviour
{
    private List<string> keyStrokeHistory;

    bool canSwitchScene = false;

    void Start()
    {
        keyStrokeHistory = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Menu");
        }

        KeyCode keyPressed = DetectKeyPressed();
        AddKeyStrokeToHistory(keyPressed.ToString());
        if (GetKeyStrokeHistory().Equals("S,C,E,N,E"))
        {
            canSwitchScene = true;
            ClearKeyStrokeHistory();
        }

        if (canSwitchScene)
        {
            if (!Input.GetKeyDown(KeyCode.None))
            {
                if (Input.GetKeyDown(KeyCode.Alpha0)) SceneManager.LoadScene(0);
                else if (Input.GetKeyDown(KeyCode.Alpha1)) SceneManager.LoadScene(1);
                else if (Input.GetKeyDown(KeyCode.Alpha2)) SceneManager.LoadScene(2);
                else if (Input.GetKeyDown(KeyCode.Alpha3)) SceneManager.LoadScene(3);
                else if (Input.GetKeyDown(KeyCode.Alpha4)) SceneManager.LoadScene(4);
                else if (Input.GetKeyDown(KeyCode.Alpha5)) SceneManager.LoadScene(5);
                else if (Input.GetKeyDown(KeyCode.Alpha6)) SceneManager.LoadScene(6);
            }
        }
    }

    private KeyCode DetectKeyPressed()
    {
        foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                return key;
            }
        }
        return KeyCode.None;
    }

    private void AddKeyStrokeToHistory(string keyStroke)
    {
        if (!keyStroke.Equals("None"))
        {
            keyStrokeHistory.Add(keyStroke);
            if (keyStrokeHistory.Count > 5)
            {
                keyStrokeHistory.RemoveAt(0);
            }
        }
    }

    private string GetKeyStrokeHistory()
    {
        return String.Join(",", keyStrokeHistory.ToArray());
    }

    private void ClearKeyStrokeHistory()
    {
        keyStrokeHistory.Clear();
    }
}
