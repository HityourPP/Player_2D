using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    public TextAsset file;
    private List<string> textList = new List<string>();
    public PlayerController playerController;
    //UI���
    [Header("UI")]
    public TextMeshProUGUI text;
    public Image character;
    public Sprite face1,face2;
    private bool lineReadFinish;
    private float readSpeed;

    private int index;
    private void Awake()
    {
        ReadText(file);
    }
    private void OnEnable()
    {
        lineReadFinish = true;//�տ�ʼʱ���ֵ�һ������
        StartCoroutine(SetDialogUI());
    }
    private void Update()
    {
        if (index == textList.Count && Input.GetKey(KeyCode.Space))
        {
            //�رնԻ���
            gameObject.SetActive(false);
            index = 0;    
            playerController.CanJump = true;
            playerController.canFlip = true;
            playerController.canMove = true;
            return;
        }
        else if(Input.GetKey(KeyCode.Space) && lineReadFinish == true)
        {
            StartCoroutine(SetDialogUI());
        }
    }
    private void ReadText(TextAsset file)
    {
        textList.Clear();//��ȡǰ�������
        index = 0;

        var data = file.text.Split('\n');
        foreach (var line in data)
        {
            textList.Add(line);
        }
    }
    IEnumerator SetDialogUI()
    {
        lineReadFinish = false;
        text.text = "";
        if (textList[index] == "A")
        {
            character.sprite = face1;
            index++;
        }
        if (textList[index] == "B")
        {
            character.sprite = face2;
            index++;
        }
        for (int i = 0; i < textList[index].Length; i++)
        {
            text.text += textList[index][i];//����˳����ʾ����
            if (Input.GetKey(KeyCode.Space) && lineReadFinish == false)
            {
                readSpeed = 0.05f;
            }
            else
            {
                readSpeed = 0.15f;
            }
            yield return new WaitForSeconds(readSpeed);
        }
        index++;
        lineReadFinish = true;
    }


}
