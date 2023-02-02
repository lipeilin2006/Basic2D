using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    GameObject player;
    GameObject Finish;
    GameObject NotFinish;
    Animator playeranim;
    AudioSource audio;

    Text showCoinCount;

    int coinCount;
    public int coinTotal;

    GameObject fail;

    bool isjumping;
    bool isdark;
    //移动速度
    public float MovementSpeed;
    //跳跃高度
    public float JumpHeight;
    //捡硬币音频1
    public AudioClip C1;
    //捡硬币音频2
    public AudioClip E3;
    //捡硬币音频3
    public AudioClip G5;
    //跳跃音效
    public AudioClip Jump;
    // Start is called before the first frame update
    void Start()
    {
        Finish = GameObject.Find("Success");
        NotFinish = GameObject.Find("NotFinish");
        showCoinCount = GameObject.Find("CoinsCount").GetComponent<Text>();
        fail = GameObject.Find("Fail");
        audio = gameObject.GetComponent<AudioSource>();
        player = gameObject;
        playeranim = GetComponent<Animator>();
        isjumping = false;
        isdark = true;
        fail.SetActive(false);
        Finish.SetActive(false);
        NotFinish.SetActive(false);
        coinCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        showCoinCount.text = "已收集 " + coinCount + " 个硬币";
        //跳跃
        if (Input.GetKeyDown(KeyCode.Space) && !isjumping)
        {
            playeranim.SetBool("isJump", true);
            playeranim.CrossFade("JumpUp", 0);
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpHeight, ForceMode2D.Impulse);
            isjumping = true;
            audio.clip = Jump;
            audio.Play();
            player.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
        //走动
        if (Input.GetKey(KeyCode.A))
        {
            //防止滞空动作被切换
            if (!isjumping)
            {
                playeranim.SetBool("isRun", true);
                playeranim.CrossFade("Run", 0);
            }
            player.transform.Translate(new Vector2(1, 0) * Time.deltaTime * MovementSpeed);
            player.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (!isjumping)
            {
                playeranim.SetBool("isRun", true);
                playeranim.CrossFade("Run", 0);
            }
            player.transform.Translate(new Vector2(1, 0) * Time.deltaTime * MovementSpeed);
            player.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            if (!isjumping)
            {
                playeranim.SetBool("isRun", false);
                playeranim.CrossFade("Idle", 0);
            }
        }
        //阴阳切换
        if (Input.GetKeyDown(KeyCode.C))
        {
            player.transform.position = new Vector3(player.transform.position.x, isdark ? player.transform.position.y + 30 : player.transform.position.y - 30, 0);
            isdark = !isdark;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //落地检测，动作切换
        if (collision.gameObject.tag == "Ground")
        {
            player.GetComponent<Rigidbody2D>().gravityScale = 0;
            if (isjumping)
            {
                playeranim.SetBool("isJump", false);
                playeranim.CrossFade("Land", 0);
                isjumping = false;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            player.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }
    //捡硬币
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //碰到铜币
        if (collision.gameObject.tag == "Coin1")
        {
            coinCount++;
            audio.clip = C1;
            audio.Play();
            Destroy(collision.gameObject);
        }//碰到银币
        else if (collision.gameObject.tag == "Coin2")
        {
            coinCount++;
            audio.clip = E3;
            audio.Play();
            Destroy(collision.gameObject);
        }//碰到金币
        else if (collision.gameObject.tag == "Coin3")
        {
            coinCount++;
            audio.clip = G5;
            audio.Play();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Spike")
        {
            fail.SetActive(true);
            Time.timeScale = 0;
        }
        else if (collision.gameObject.name == "Finish")
        {
            if (coinCount >= coinTotal)
            {
                Finish.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                StartCoroutine(NotFin());
            }
        }
    }

    IEnumerator NotFin()
    {
        NotFinish.SetActive(true);
        yield return new WaitForSeconds(1);
        NotFinish.SetActive(false);
    }
}
