using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_Health : NetworkBehaviour {

    [SyncVar (hook = "OnHealthChanged")]
    private int health = 100;
    private Text healthText;
    private GameObject respawnCountdown;
    private Text respawnText;
    private bool shouldDie = false;
    public bool isDead = false;

    public float timeToRespawn = 10f;

    public delegate void DieDelegate();
    public event DieDelegate EventDie;

    public delegate void RespawnDelegate();
    public event RespawnDelegate EventRespawn;

    // Use this for initialization
    void Start () {
        healthText = GameObject.Find("HealthText").GetComponent<Text>();
        respawnCountdown = GameObject.Find("GameManager").GetComponent<GameManager_References>().respawnCountdownText;
        respawnText = respawnCountdown.GetComponent<Text>();
        SetHealthText();
	}
	
	// Update is called once per frame
	void Update () {
        CheckCondition();
	}

    void CheckCondition()
    {
        if(health <= 0 && !shouldDie && !isDead)
        {
            shouldDie = true;
        }

        if(health <= 0 && shouldDie)
        {
            if(EventDie != null)
            {
                EventDie();
            }

            shouldDie = false;
        }

        if(health > 0 && isDead)
        {
            if(EventRespawn != null)
            {
                Debug.Log("Start Respawn Process");
                

                //StartCoroutine("RespawnCountdown");
                EventRespawn();

            }

            isDead = false;
        }
    }

    void SetHealthText()
    {
        if(isLocalPlayer)
        {
            healthText.text = "Health " + health.ToString();
        }
    }

    public void TakeDamage (int dmg)
    {
        health -= dmg;
    }

    void OnHealthChanged(int newHealth)
    {
        health = newHealth;
        SetHealthText();
    }

    public void ResetHealth()
    {
        health = 100;
    }

    public IEnumerator RespawnCountdown()
    {
        float respawnTime = timeToRespawn;
        respawnCountdown.SetActive(true);
        while (respawnTime > 0)
        {
            respawnTime -= Time.deltaTime;
            respawnText.text = "Respawn in: " + ((int)respawnTime).ToString();
            yield return null;
        }
        respawnCountdown.SetActive(false);
        GameObject.Find("GameManager").GetComponent<GameManager_References>().respawnButton.SetActive(true);
    }
}
