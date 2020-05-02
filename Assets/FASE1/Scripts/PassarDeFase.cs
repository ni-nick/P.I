﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassarDeFase : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) // quando o objeto para passar de fase tocar no "Player"
        {
            SceneManager.LoadScene("Fase2"); // ele pega a cena atual e vai para a fase 2
        }

    }
}