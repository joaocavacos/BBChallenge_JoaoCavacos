using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private int playerCount = 0;
    
    public void SetWidth(float width)
    {
        transform.localScale = new Vector3(width, transform.localScale.y, transform.localScale.z);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && playerCount == 0)
        {
            playerCount = 1;
            GameDirector.Instance.IncreasePlatformScore(1);
        }
    }
}
