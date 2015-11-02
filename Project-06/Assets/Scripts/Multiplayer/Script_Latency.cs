using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Script_Latency : NetworkBehaviour {

    NetworkClient client;
    int latency;
    Text latencyText;

    // Use this for initialization
    void Start () {
		if (isLocalPlayer) 
		{
			client = GameObject.Find ("Network Manager").GetComponent<NetworkManager> ().client;
			latencyText = GameObject.Find ("Latency_Text").GetComponent<Text> ();
		} 
		else 
		{
			Destroy(this);
		}
	}
	
	// Update is called once per frame
	void Update ()
    {
		UpdateLatency ();
	}

    void UpdateLatency()
    {
		latency = client.GetRTT ();

		if (latency < 200)
			latencyText.color = Color.green;
		else if (latency < 300)
			latencyText.color = Color.yellow;
		else
			latencyText.color = Color.red;

		latencyText.text = latency.ToString () + " ms";

    }
}
