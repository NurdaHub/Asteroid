using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour
{
    private T prefab;
    private Transform prefabParent;
    private List<T> poolList;

    public Pool(T newPrefab, int count, Transform newPrefabParent)
    {
        prefab = newPrefab;
        prefabParent = newPrefabParent;
        CreatePool(count);
    }

    private void CreatePool(int count)
    {
        poolList = new List<T>();

        for (int i = 0; i < count; i++)
            CreateObject();
    }

    private T CreateObject(bool isActive = false)
    {
        var currentObject = Object.Instantiate(prefab, prefabParent);
        currentObject.gameObject.SetActive(isActive);
        poolList.Add(currentObject);

        return currentObject;
    }

    private bool HasFreeElement(out T element)
    {
        foreach (var item in poolList)
        {
            if (!item.gameObject.activeInHierarchy)
            {
                element = item;
                item.gameObject.SetActive(true);

                return true;
            }
        }
        element = null;
        
        return false;
    }

    public T GetFreeElement()
    {
        if (HasFreeElement(out var element))
            return element;

        return CreateObject(true);
    }

    public bool HasActiveElement()
    {
        foreach (var item in poolList)
        {
            if (item.gameObject.activeInHierarchy)
                return true;
        }

        return false;
    }

    public void DeactivateAllElements()
    {
        foreach (var item in poolList)
            item.gameObject.SetActive(false);
    }
}
