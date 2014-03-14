using UnityEngine;
using System.Collections;

public class GameMenuNetwork : MonoBehaviour
{
	public bool isServerMode;

	void OnMouseUp()
	{
		Hashtable arguments=new Hashtable();
		
		arguments.Add("serverMode", isServerMode);
		
		SceneManager.LoadScene("NetworkScene", arguments);
	}
}
