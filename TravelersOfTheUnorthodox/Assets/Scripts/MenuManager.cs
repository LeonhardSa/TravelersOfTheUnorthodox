using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log(other.tag);
        
        if (other.tag.Equals("Player"))
        {
            Debug.Log(gameObject.name);

            switch (gameObject.name)
            {
                case string s when s.Contains("Play"):
                    SceneManager.LoadScene("Game");
                    break;
                case string s when s.Contains("HQ"):
                    SceneManager.LoadScene("End");
                    break;
                case string s when s.Contains("Tutorial"):
                    SceneManager.LoadScene("Demo");
                    break;
                case string s when s.Contains("Exit"):
                    Application.Quit();
                    break;
                default:
                    break;
            }
        }
    }
}
