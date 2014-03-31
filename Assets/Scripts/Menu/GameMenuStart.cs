using UnityEngine;
using System.Collections.Generic;

public class GameMenuStart : MonoBehaviour
{
    public int difficulty=0;

    void OnButtonPress()
    {
        Dictionary<string, object> arguments=new Dictionary<string, object>();

        arguments.Add("difficulty", difficulty);

        SceneManager.LoadScene("MainScene", arguments);
    }
}
