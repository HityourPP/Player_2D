using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImagePool : MonoBehaviour
{
    public GameObject afterImagePrefab;

    //�洢δ����Ķ���
    private Queue<GameObject> availableObjects= new Queue<GameObject>();

    public static PlayerAfterImagePool Instance { get; private set; }//ʹ�õ���
    private void Awake()
    {
        Instance = this;
        GrowPool();
    }
    private void GrowPool()//���������
    {
        for(int i = 0; i < 10; i++)
        {
            var instanceToAdd = Instantiate(afterImagePrefab);//����һ��Ԥ����
            instanceToAdd.transform.SetParent(transform);//���²�����������ڽű����ڵ�������
            AddToPool(instanceToAdd);//������ӵ��������
        }
    }
    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        availableObjects.Enqueue(instance);//������ӵ���β
    }
    public GameObject GetFromPool()//�Ӷ������ȡ������
    {
        if (availableObjects.Count == 0)
        {
            GrowPool();
        }
        var instance = availableObjects.Dequeue();//ȡ���׵�һ�����󣬲�����Ӷ���ɾȥ
        instance.SetActive(true);
        return instance;
    }
}
