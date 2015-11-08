using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_Respawn : NetworkBehaviour {

    private Player_Health healthScript;
    private Image[] crosshairs;
    private GameObject respawnButton;

	// Use this for initialization
	void Start () {

        healthScript = GetComponent<Player_Health>();
        healthScript.EventRespawn += EnablePlayer;
        crosshairs = GameObject.Find("Crosshairs").GetComponentsInChildren<Image>();
        SetRespawnButton();
    }

    void SetRespawnButton()
    {
        if(isLocalPlayer)
        {
            respawnButton = GameObject.Find("GameManager").GetComponent<GameManager_References>().respawnButton;
            respawnButton.GetComponent<Button>().onClick.AddListener(CommenceRespawn);
            respawnButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update () {
	
	}

    void OnDisable()
    {
        healthScript.EventRespawn -= EnablePlayer;
    }

    void EnablePlayer()
    {
        //GetComponent<CharacterController> ().enabled = false;
        GetComponent<Player_Shoot>().enabled = true;
        //GetComponent<CapsuleCollider>().enabled = true;
        //GetComponent<Rigidbody>().detectCollisions = true;

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer visible in renderers)
        {
            visible.enabled = true;
        }

        if (isLocalPlayer)
        {
            GetComponent<UnityStandardAssets.Characters.
                FirstPerson.RigidbodyFirstPersonController>().enabled = true;
            foreach (Image img in crosshairs)
            {
                img.enabled = true;
            }

            respawnButton.SetActive(false);
        }

    }

    void CommenceRespawn()
    {
        CmdResetHealthOnServer();
    }

    void CmdResetHealthOnServer()
    {
        healthScript.ResetHealth();
    }
}