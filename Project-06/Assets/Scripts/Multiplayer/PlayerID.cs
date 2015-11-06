using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerID : NetworkBehaviour {

	[SyncVar]public string playerUniqueIdentity;
	private NetworkInstanceId playerNetID;
	private Transform myTransform;

	public override void OnStartLocalPlayer()
	{
		GetNetIdentity ();
		SetIdentity ();
	}
	// Use this for initialization
	void Awake () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (myTransform.name == "" || myTransform.name == "Player(Clone)")
			SetIdentity ();
	}

	[Client]
	void GetNetIdentity()
	{
		playerNetID = GetComponent<NetworkIdentity> ().netId;
		CmdGeneratePlayerIdentity (MakeUniqueIdentity ());
	}

	string MakeUniqueIdentity()
	{
		string uniquename = "Player " + playerNetID.ToString ();
		return uniquename;
	}

	void SetIdentity()
	{
		if (!isLocalPlayer)
			myTransform.name = playerUniqueIdentity;
		else
			myTransform.name = MakeUniqueIdentity();
	}

	[Command]
	void CmdGeneratePlayerIdentity(string name)
	{
		playerUniqueIdentity = name;
	}

}



















