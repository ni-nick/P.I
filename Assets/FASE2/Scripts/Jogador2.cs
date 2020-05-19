﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Jogador2 : MonoBehaviour
{
    public float forcaPulo;
    public float velocidadeMax;

    public int Vida;
    public int Recompensas;

    //variaveis de objetos
    public Text TextVida;
    public Text TextRecompensas;

    public bool isGround;

    //tiro do player
    public Animator anim;
    public float fireRate = 0.5f;
    public float nextfire; // quando pode dar o proximo tiro
    public GameObject tiroPefab;
    public Transform shootspawner;

    //chekpoint
    [SerializeField] public Transform player;
    [SerializeField] public Transform PontoRespwn;
    [SerializeField] public Transform player2;
    [SerializeField] public Transform PontoRespwn2;
    [SerializeField] public Transform player3;
    [SerializeField] public Transform PontoRespwn3;

    //levar dano do oscuno
    public float danoTempo = 1f;
    private bool levouDano = false;
    
    void Start()
    {
        TextVida.text = Vida.ToString();
        TextRecompensas.text = Recompensas.ToString();
    }

    void FixedUpdate(){

        float movimento = Input.GetAxis("Horizontal");

        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

        rigidbody.velocity = new Vector2(movimento * velocidadeMax, rigidbody.velocity.y);


        if (movimento < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;

        }
        else if (movimento > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (movimento > 0 || movimento < 0)
        {
            GetComponent<Animator>().SetBool("andando", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("andando", false);
        }


        if (Input.GetKeyDown(KeyCode.UpArrow) && isGround)
        {
            rigidbody.AddForce(new Vector2(0, forcaPulo));
        }

        // pular

        if (isGround)
        {
            GetComponent<Animator>().SetBool("pulando", false);
        }
        else
        {
            GetComponent<Animator>().SetBool("pulando", true);
            GetComponent<Animator>().SetBool("andando", false);
        }

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time > nextfire)
        { 
            nextfire = Time.time + fireRate;
            GetComponent<Animator>().SetBool("atirando", true);
            GameObject tempotiro = Instantiate(tiroPefab, shootspawner.position, shootspawner.rotation);


            //if(virado pra esquerda)
            //{
            //tempotiro.transform.eulerAngles = new Vector3(0, 0, 180);
            //}

        }

        else
        {
            GetComponent<Animator>().SetBool("atirando", false);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("moeda"))
       {
            gameObject.GetComponent<AudioSource>().Play(); 
            Destroy(collision2D.gameObject);
            Recompensas++;
            TextRecompensas.text = Recompensas.ToString();
        }

        if (collision2D.gameObject.CompareTag("MoedaGanhou"))
        {
            SceneManager.LoadScene("YouWin");
        }

        if (collision2D.gameObject.CompareTag("ZonaMorte"))
        {
            player.transform.position = PontoRespwn.transform.position;
            Vida--;
            TextVida.text = Vida.ToString();
            // GetComponent<Animator>().SetBool("morrendo", true);

            if (Vida == 0)
            {
                SceneManager.LoadScene("GameOver");
            }
        }

        if (collision2D.gameObject.CompareTag("ZonaMorte2"))
        {
            player2.transform.position = PontoRespwn2.transform.position;
            Vida--;
            TextVida.text = Vida.ToString();
            // GetComponent<Animator>().SetBool("morrendo", true);

            if (Vida == 0)
            {
                SceneManager.LoadScene("GameOver");
            }
        }

        if (collision2D.gameObject.CompareTag("ZonaMorte3"))
        {
            player3.transform.position = PontoRespwn3.transform.position;
            Vida--;
            TextVida.text = Vida.ToString();
            // GetComponent<Animator>().SetBool("morrendo", true);

            if (Vida == 0)
            {
                SceneManager.LoadScene("GameOver");
            }
        }

        if (collision2D.gameObject.CompareTag("Oscuno") && !levouDano)
        {
            StartCoroutine(LevouDano());
        }
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Inimigos"))
        {

            player.transform.position = PontoRespwn.transform.position;
            Vida--;
            TextVida.text = Vida.ToString();
            if (Vida == 0)
            {
                SceneManager.LoadScene("GameOver");
            }

        }

        if (collision2D.gameObject.CompareTag("plataforma"))
        {
            isGround = true;
        }

        if (collision2D.gameObject.CompareTag("trampolim"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 15f); // aumentar os números, aumenta a força do pulo no trampolim
        }

        if (collision2D.gameObject.CompareTag("auxiliar"))
        {
            isGround = true;
        }

        if (collision2D.gameObject.CompareTag("Oscuno") && !levouDano)
        {
            StartCoroutine(LevouDano());
        }
    }


    void OnCollisionExit2D(Collision2D collision2D)
    {

        if (collision2D.gameObject.CompareTag("plataforma"))
        {
            isGround = false;
        }

        if (collision2D.gameObject.CompareTag("auxiliar"))
        {
            isGround = false;
        }

    }

    IEnumerator LevouDano()
    {
        levouDano = true;
        Vida--;
        TextVida.text = Vida.ToString();
        if(Vida <= 0)
        {
            //anim.SetTrigger("morrendo"); //criar a animação >:(
            Invoke("LunaMorte",2f);

        }

        else
        {
            Physics2D.IgnoreLayerCollision(9,11); // ignorar a camada do player e do inimigo pra poder levar dano
            for(float i = 0; i < danoTempo; i += 0.2f) // vai fazer o sprite piscar pra mostrar que levou dano
            {
                GetComponent<SpriteRenderer>().enabled = false;
                yield return new WaitForSeconds(0.1f);
                GetComponent<SpriteRenderer>().enabled = true;
                yield return new WaitForSeconds(0.1f);
            }
            Physics2D.IgnoreLayerCollision(9, 11, false);
            levouDano = false;
        }
    }

    void LunaMorte()
    {
        SceneManager.LoadScene("GameOver");
    }



}
