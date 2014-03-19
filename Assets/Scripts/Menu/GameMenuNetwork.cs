using UnityEngine;
using System.Collections;

public class GameMenuNetwork : MonoBehaviour
{
    public bool isServerMode;

    void OnMouseDown()
    {
        Hashtable arguments=new Hashtable();

        arguments.Add("serverMode", isServerMode);

        SceneManager.LoadScene("NetworkScene", arguments);
    }
}
