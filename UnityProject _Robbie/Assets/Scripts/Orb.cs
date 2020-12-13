using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    int player;

    public GameObject OrbVFXPrefab;

    void Start()
    {
        player = LayerMask.NameToLayer("Player");

        GameManager.RegisterOrb(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == player)
        {
            gameObject.SetActive(false);

            Instantiate(OrbVFXPrefab, transform.position, transform.rotation);

            AudioManager.PlayOrbAudio();

            GameManager.PlayerGrabbedOrb(this);
        }
    }
}
