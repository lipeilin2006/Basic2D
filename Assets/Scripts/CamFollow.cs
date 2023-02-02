using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject yin;
    public GameObject yang;
    bool isdark;
    float camYoffset;
    void Start()
    {
        player = GameObject.Find("Player");
        yin = GameObject.Find("yin");
        yang = GameObject.Find("yang");
        isdark = true;
        yin.SetActive(false);
        yang.SetActive(false);
        camYoffset = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //阴阳切换
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isdark)
            {
                camYoffset = camYoffset + 30;
                GetComponent<Camera>().backgroundColor = Color.white;
                StartCoroutine(TurnToYang());
                isdark = false;
            }
            else
            {
                camYoffset = camYoffset - 30;
                GetComponent<Camera>().backgroundColor = Color.black;
                StartCoroutine(TurnToYin());
                isdark = true;
            }
        }
        gameObject.transform.position = new Vector3(player.transform.position.x, camYoffset, gameObject.transform.position.z);
    }

    //转换为阴的动画
    public IEnumerator TurnToYin()
    {
        yang.SetActive(false);
        yin.SetActive(true);
        yield return new WaitForSeconds(1);
        yin.SetActive(false);
    }

    //转换为阳的动画
    public IEnumerator TurnToYang()
    {
        yin.SetActive(false);
        yang.SetActive(true);
        yield return new WaitForSeconds(1);
        yang.SetActive(false);
    }
}
