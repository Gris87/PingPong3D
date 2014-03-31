using UnityEngine;
using System.Collections.Generic;

public class GameMenuNetwork : MonoBehaviour
{
    public bool isServerMode;

    void OnButtonPress()
    {
        Dictionary<string, object> arguments=new Dictionary<string, object>();

        arguments.Add("serverMode", isServerMode);

        SceneManager.LoadScene("NetworkScene", arguments);
    }
}
