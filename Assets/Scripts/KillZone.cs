using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public float offsetY = -2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameDirector.Instance.HandleGameOver();
        }
    }

    private void Start()
    {
        offsetY = GameDirector.Instance.player.position.y + offsetY;
    }

    private void Update()
    {
        if (GameDirector.Instance.player != null)
        {
            var player = GameDirector.Instance.player;
            transform.position = new Vector3(player.position.x, offsetY, player.position.z);
        }
    }
}
