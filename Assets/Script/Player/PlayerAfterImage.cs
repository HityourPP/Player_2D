using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImage : MonoBehaviour
{
    private Transform player;

    private SpriteRenderer SR;
    private SpriteRenderer playerSR;
    private Color color;

    private float timeActivated;
    private float activeTime = 0.1f;//��Ӱ����ʱ��
    private float alpha;
    public float alphaSet = 25f;//���Ʋ�Ӱ͸����
    public float alphaDecay = 10f;

    private void OnEnable()
    {//��ȡ����Լ���ʼ��
        SR = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSR = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        SR.sprite = playerSR.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        timeActivated= Time.time;
    }
    private void Update()
    {
        alpha -= alphaDecay * Time.deltaTime;//������͸����
        color = new Color(1f,1f,1f,alpha);
        SR.color = color;
        if (Time.time >= (timeActivated + activeTime))
        {//����Żض���أ��Ӷ������ٴ�ʹ��
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }

}
