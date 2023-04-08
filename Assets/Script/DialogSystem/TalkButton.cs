using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkButton : MonoBehaviour
{
    private GameObject talk;
    public GameObject dialogUI;
    public PlayerController playerController;
    private void Start()
    {
        talk = gameObject.transform.GetChild(0).transform.gameObject;//获取子物体
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        talk.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        talk.SetActive(false);
    }
    private void Update()
    {
        if (talk.activeSelf && Input.GetKey(KeyCode.R))
        {
            dialogUI.SetActive(true);
            playerController.canNormalJump = false;
            playerController.CanJump = false;
            playerController.canMove = false;
            playerController.canFlip = false;
        }
    }
}
