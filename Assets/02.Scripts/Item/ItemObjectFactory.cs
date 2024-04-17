using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ItemType
{
    Health,
    Credit
}

public class ItemObjectFactory : MonoBehaviour
{
    public static ItemObjectFactory Instance {  get; private set; }
    public List <GameObject> ItemPrefabs;

    private List <ItemObject> _itemPool;
    private int _poolSize = 10;


    private void Awake()
    {
        Instance = this;
        _itemPool = new List <ItemObject>();
    }

    private void PreparePool(int poolsize)
    {
        for (int i = 0; i < poolsize; i++)
        {
            GameObject item = null;
            foreach (GameObject prefab in ItemPrefabs)
            {
                item = Instantiate(prefab);
                item.transform.SetParent(this.transform);
                _itemPool.Add(item.GetComponent<ItemObject>());
                item.SetActive(false);
            }
        }
    }
    private void Start()
    {
        PreparePool(_poolSize); // poolsize 10인 풀 생성
    }
    private ItemObject Get(ItemType itemType)
    {
        foreach (ItemObject itemObject in _itemPool)
        {
            if(itemObject.gameObject.activeSelf == false && itemObject.ItemType == itemType)
            {
                return itemObject;
            }
        }
        return null;
    }
    public void MakePercent(Vector3 position)
    {
        int itemRandomFactor = Random.Range(0, 100);
        if (itemRandomFactor <= 20)
        {
            Make(ItemType.Health, position);
        }
        else
        {
            Make(ItemType.Credit, position);
        }

    }
    public void Make(ItemType itemType, Vector3 position)
    {
        ItemObject itemObject = Get(itemType);   
        if (itemObject != null)
        {
            itemObject.gameObject.SetActive(true);
            itemObject.Init();
            itemObject.transform.position = position;
        }
        
    }
}
