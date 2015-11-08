using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_Death : NetworkBehaviour {

    private Player_Health healthScript;
    private Image[] crosshairs;

	// Use this for initialization
	void Start () {
        crosshairs = GameObject.Find("Crosshairs").GetComponentsInChildren<Image>();
        healthScript = GetComponent<Player_Health>();
        healthScript.EventDie += DisablePlayer;
	}
	
    void OnDisable()
    {
        healthScript.EventDie -= DisablePlayer;
    }

    void DisablePlayer()
    {

		//GetComponent<CharacterController> ().enabled = false;
        GetComponent<Player_Shoot>().enabled = false;
        //GetComponent<CapsuleCollider>().enabled = false;
        //GetComponent<Rigidbody>().detectCollisions = false;

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach(Renderer visible in renderers)
        {
            visible.enabled = false;
        }

        healthScript.isDead = true;

        if(isLocalPlayer)
        {
            GetComponent<UnityStandardAssets.Characters.
				FirstPerson.RigidbodyFirstPersonController>().enabled = false;
            foreach (Image img in crosshairs)
            {
                img.enabled = false;
            }

            healthScript.StartCoroutine("RespawnCountdown");
        }
    }
}
