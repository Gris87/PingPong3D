using UnityEngine;
using System.Collections;

public class GameMenuStart : MonoBehaviour
{
    public int difficulty=0;

    void OnMouseUp()
    {
        Hashtable arguments=new Hashtable();

        arguments.Add("difficulty", difficulty);

        SceneManager.LoadScene("MainScene", arguments);
    }
}
