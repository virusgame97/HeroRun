﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rigid;
    Animator animator;
    private EnemiesGenerator enemies;
    private BonusGenerator bonusGenerator;
    [Range(-3.1f, 3.1f)] float value;
    public float increment;
    bool paper;
    bool mask;
    Vector2 startTouch, endTouch;
    GameObject gun;
    float punteggio;
    bool end;
    GameObject countImage;
    public GameObject imageMask;
    public GameObject imagePaper;
    GameObject stelle;
    bool endPolice;
    bool endVirus;
    public Sprite sprite1;
    AudioSource musicaGioco;


    // Start is called before the first frame update

    private void Awake()
    {
        bonusGenerator = GameObject.FindObjectOfType<BonusGenerator>();
        enemies = GameObject.FindObjectOfType<EnemiesGenerator>();
    }
    void Start()
    {
        //GameObject.Find("ImageMask").SetActive(false);
        endPolice = false;
        endVirus = false;
        end = false;
        punteggio = 0;
        mask = false;
        paper = false;
        increment = 0f;
        rigid=GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("Shot", false);
        animator.SetBool("Morto", false);
        animator.SetBool("Tosse", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Countdown", false);

        stelle = GameObject.Find("Stars");
        stelle.SetActive(false);
        gun = GameObject.Find("GunMedico");
        gun.SetActive(false);
        Score.animazioneFine = false;

        Score.countdown = true;
        Score.buttonPause = false;
        Score.pause = false;
        countImage = GameObject.Find("CountdownImage");
        countImage.SetActive(false);
        countImage.GetComponent<Animator>().SetBool("Count", false);

        musicaGioco = GameObject.Find("Music").GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!end && !Score.countdown)
        {
            if (Time.timeScale == 1)
            {
                if (!Score.pause)
                {
                    MovePlayer();
                    CalcolaPunteggio();
                    StartCoroutine(ShotPlayer());
                }
                
            }
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startTouch = Input.GetTouch(0).position;
            }

        }
        else if(end && !Score.countdown)
        {
            StartCoroutine(AnimationPerdente());
        }

        else
        {
            StartCoroutine(CountdownAnimation());
        }

    }

    public void MovePlayer()
    {
        increment += 0.01f;

        if (musicaGioco.pitch < 2.0f)
        {
            if (increment % 10 == 0)
            {
                musicaGioco.pitch += 0.1f;
            }
        }

        if (!end)
            rigid.AddForce((30 + increment) * new Vector3(0.0f, 0.0f, 25.0f));
        this.gameObject.transform.rotation = new Quaternion(0.0f, 0.0f,0.0f,0.0f);
        if (this.gameObject.transform.position.y < 0.1)
        {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x,0.1f, this.gameObject.transform.position.z);
        }

        transform.position = new Vector3(value, transform.position.y, transform.position.z);


        if(countImage.activeSelf == false)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startTouch = Input.GetTouch(0).position;
            }


            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                endTouch = Input.GetTouch(0).position;

                if ((endTouch.x < startTouch.x) && transform.position.x > -3.1f)
                {
                    if (value == -3.1f)
                        return;
                    value -= 3.1f;

                }
                if ((endTouch.x > startTouch.x) && transform.position.x < 3.1f)
                {
                    if (value == 3.1f)
                        return;
                    value += 3.1f;
                }

            }
        }
            
        
          

    }

    IEnumerator ShotPlayer()
    {
        if (Input.touchCount > 0 && countImage.activeSelf == false)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began )
            {

            }

            if (Input.touchCount > 0 &&  Input.GetTouch(0).phase == TouchPhase.Stationary )
            {
                animator.SetBool("Shot", true);
                yield return new WaitForSeconds(0.6f);
                gun.SetActive(true);


            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended )
            {
                yield return new WaitForSeconds(0.3f);
                animator.SetBool("Shot", false);
                yield return new WaitForSeconds(0.3f);
                gun.SetActive(false);
            }
        }
    }

    public void CalcolaPunteggio()
    {
         
            punteggio = punteggio + 0.5f + increment;
            Score.punteggio = (int)punteggio;
            Text t = GameObject.Find("Punti").GetComponent<Text>();
            t.text = "" + Score.punteggio;
        
    }
    
    IEnumerator AnimationPerdente()
    {
        if (endVirus || endPolice)
        {
            animator.SetBool("Shot", false);
            gun.SetActive(false);


            Score.buttonPause = false;
            animator.SetBool("Walk", true);
            yield return new WaitForSeconds(0.2f);
            GameObject cam = GameObject.Find("Main Camera");
            cam.transform.rotation = Quaternion.Euler(17.5f, 180, 0);
            cam.transform.position = new Vector3(this.transform.position.x, 4f, this.transform.position.z + 5f);
            Score.animazioneFine = true;
            this.gameObject.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;

            animator.SetBool("Tosse", true);

            yield return new WaitForSeconds(0.8f);
            animator.SetBool("Morto", true);

            yield return new WaitForSeconds(4.5f);
            Score.fine = true;
        }


    }

    IEnumerator CountdownAnimation()
    {

        countImage.GetComponent<Image>().sprite = sprite1;
        countImage.SetActive(true);
        countImage.GetComponent<Animator>().SetBool("Count", true);
        Score.countdown = false;

        yield return new WaitForSeconds(4f);

        countImage.SetActive(false);
        Score.buttonPause=true;
        Score.pause = false;
        countImage.GetComponent<Animator>().SetBool("Count", false);


    }



    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetType() == typeof(SphereCollider))
        {
            if (collision.gameObject.tag.Equals("CoronaVirus") || (collision.gameObject.tag.Equals("Person")))
            {
                if (!mask)
                {
                    Destroy(collision.gameObject);
                    enemies.enemies.Remove(collision.gameObject);
                    end = true;
                    endVirus = true;

                }
                else
                {
                    mask = false;
                    imageMask.GetComponent<Image>().color = new Color32(255, 235, 235, 80);
                    Destroy(collision.gameObject);
                    enemies.enemies.Remove(collision.gameObject);
                }
            }

            if (collision.gameObject.tag.Equals("Police"))
            {
                if (!paper && stelle.activeSelf == true)
                {
                    endPolice = true;
                    end = true;
                    this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, 0.1f, this.gameObject.transform.position.z - 10);
                    this.gameObject.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

                }
                else if (!paper && stelle.activeSelf == false)
                {
                    stelle.SetActive(true);
                    Destroy(collision.gameObject);
                    enemies.enemies.Remove(collision.gameObject);
                }
                else if (paper && stelle.activeSelf == false)
                {
                    paper = false;
                    imagePaper.GetComponent<Image>().color = new Color32(255, 235, 235, 80);
                    Destroy(collision.gameObject);
                    enemies.enemies.Remove(collision.gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Paper"))
        {
            Destroy(other.gameObject);
            bonusGenerator.bonus.Remove(other.gameObject);
            bonusGenerator.paper = true;
            if (stelle.activeSelf == true)
            {
                stelle.SetActive(false);
            }
            else
            {
                paper = true;
                imagePaper.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
        }
        if (other.gameObject.tag.Equals("Mask"))
        {
            Destroy(other.gameObject);
            bonusGenerator.bonus.Remove(other.gameObject);
            bonusGenerator.mask = true;
            mask = true;
            imageMask.GetComponent<Image>().color = new Color32(255, 235, 235, 255);
        }
    }

}
