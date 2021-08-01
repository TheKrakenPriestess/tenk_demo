﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 3;
    private Vector3 bullectEulerAngles;
    private float timeVal;
    private float defendTimeVal = 3;
    private bool isDefended = true;
    private SpriteRenderer sr;
    public Sprite[] tankSprite; //上 右 下 左
    public GameObject bullectPrefab;
    public GameObject explosionPrefab;
    public GameObject defendEffectPrefab;
    public AudioSource moveAudio;
    public AudioClip[] tankAudio;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isDefended)
        {
            defendEffectPrefab.SetActive(true);
            defendTimeVal -= Time.deltaTime;
            if (defendTimeVal <= 0)
            {
                isDefended = false;
                defendEffectPrefab.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (PlayerManager.Instance.isDefeat)
        {
            return;
        }

        Move();
        if (timeVal >= 0.4f)
        {
            Attack();
        }
        else
        {
            timeVal += Time.fixedDeltaTime;
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bullectPrefab, transform.position,
                Quaternion.Euler(transform.eulerAngles + bullectEulerAngles));
            timeVal = 0;
        }
    }

    private void Move()
    {
        float v = Input.GetAxisRaw("Vertical");


        transform.Translate(Vector3.up * v * moveSpeed * Time.fixedDeltaTime, Space.World);

        if (v < 0)
        {
            sr.sprite = tankSprite[2];
            bullectEulerAngles = new Vector3(0, 0, -180);
        }

        else if (v > 0)
        {
            sr.sprite = tankSprite[0];
            bullectEulerAngles = new Vector3(0, 0, 0);
        }

        if (Mathf.Abs(v) > 0.05f)
        {
            moveAudio.clip = tankAudio[1];

            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }

        if (v != 0)
        {
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector3.right * h * moveSpeed * Time.fixedDeltaTime, Space.World);
        if (h < 0)
        {
            sr.sprite = tankSprite[3];
            bullectEulerAngles = new Vector3(0, 0, 90);
        }

        else if (h > 0)
        {
            sr.sprite = tankSprite[1];
            bullectEulerAngles = new Vector3(0, 0, -90);
        }

        if (Mathf.Abs(h) > 0.05f)
        {
            moveAudio.clip = tankAudio[1];

            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }
        else
        {
            moveAudio.clip = tankAudio[0];

            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }
    }

    private void Die()
    {
        if (isDefended)
        {
            return;
        }

        PlayerManager.Instance.isDead = true;

        Instantiate(explosionPrefab, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}