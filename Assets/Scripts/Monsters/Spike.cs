using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    static public bool isSpikeDamaged;
    public GameObject player;
    
    private float spikeDamage = 5f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isSpikeDamaged == false)
        {
            isSpikeDamaged = true;
            player.GetComponent<Player>().PlayerDamaged(spikeDamage);
            isSpikeDamaged = true;
            Invoke(nameof(CdSpike), 2f);
            Debug.Log("true");
        }

    }

    private void CdSpike()
    {
        isSpikeDamaged = false;
    }
}
