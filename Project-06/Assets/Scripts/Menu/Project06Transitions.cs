using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Project06Transitions : MonoBehaviour {

	public NetworkManager manager;

	//used to set up the host and move the player to the next scene
	public void _SetupHost()
	{
		manager.StartHost ();
	}

	public void _SetupServer()
	{
		manager.StartServer ();
	}

	public void _StartClientPlayer()
	{
		manager.StartClient ();
	}

	/// <summary>
	/// Updates the IP on the manager
	/// </summary>
	/// <param name="message">The IP to update to</param>
	public void UpdateIP(Object message)
	{
		Text textObj = ((GameObject)message).GetComponent<Text> ();
		if (textObj.text == "") 
		{
			manager.networkAddress = "localhost";
		} 
		else 
		{
			manager.networkAddress = textObj.text;
		}
		Debug.Log ("Updating address to... " + textObj.text);
	}

	/// <summary>
	/// Updates the port on the manager
	/// </summary>
	/// <param name="message">The port number to update to</param>
	public void UpdatePort(Object message)
	{
		Text textObj = ((GameObject)message).GetComponent<Text> ();
		int newPort;
		
		if (int.TryParse (textObj.text, out newPort)) 
		{
			manager.networkPort = newPort;
		} 
		else 
		{
			manager.networkPort = 7777;
		}
		Debug.Log ("Updating port to... " + newPort.ToString ());
	}
}
