/*
 *Author: Daniel Zhao
 *Last Modified: 2/4/2023
 *Description: Any object with this script can hold at least one material.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MaterialAffector))]

public class MaterialHolder : MonoBehaviour
{

    public List<MaterialProperty> PropertyList
    {
        get { return propertyList; }
        set { Debug.LogError("Attempted to write to readonly PropertyList. Use Add/RemoveProperty() instead"); }
    }

    [SerializeField] private List<MaterialProperty> propertyList = new List<MaterialProperty>();
    //Properties are ordered by priority. Lower indexed properties are applied first,
    //higher index properties are applied last. This makes them trump lower indexed properties.
    [SerializeField] private MaterialProperty defaultMaterial;
    [SerializeField] private GameObject swapIcon;
    private bool markedForSwap;
    private bool onDefaultMaterial;
    private MaterialAffector ma;


    private int prevPropCount = -1;

    // Start is called before the first frame update
    void Start()
    {
        ma = GetComponent<MaterialAffector>();
        prevPropCount = propertyList.Count;
        if (propertyList.Count <= 0 && defaultMaterial)
        {
            onDefaultMaterial = true;
            propertyList.Add(defaultMaterial);
        }
        else if (!defaultMaterial)
        {
            Debug.LogWarning($"Object \"{transform.name}\" is missing a default material!");
        }
        ma.ApplyMaterials(propertyList);
    }

    public bool ValidPropertyAction(bool isTaking) 
    {
        //Checks to see if the attempted action is valid. If not, return False. Otherwise, return true
        if (isTaking)
        {
            //Is attempting take. If on default, return false. Otherwise return true.
            return !onDefaultMaterial;
        }
        else
        {
            //Is attempting give. Always return true. (Might want to change later)
            return true;
        }
    }

    public MaterialProperty RemoveProperty(int index = -1) 
        //Defaults to last property (Highest priority)
    {
        MaterialProperty removedProperty;
        if (index == -1)
        {
            removedProperty = propertyList[propertyList.Count - 1];
            propertyList.RemoveAt(propertyList.Count - 1);
        }
        else
        {
            removedProperty = propertyList[index];
            propertyList.RemoveAt(index);
        }


        if (propertyList.Count <= 0 && defaultMaterial)
        {
            onDefaultMaterial = true;
            propertyList.Add(defaultMaterial);
        }
        else if (!defaultMaterial)
        {
            Debug.LogWarning($"Object \"{transform.name}\" is missing a default material!");
        }

        ma.ApplyMaterials(propertyList);
        DemarkForSwap();
        return removedProperty;
    }

    public void AddProperty(MaterialProperty mp, int index = -1) 
        //Defaults to last property (Highest priority)
    {
        if (index != -1)
        {
            propertyList.Insert(index, mp);
        }
        else
        {
            propertyList.Add(mp);
        }

        if(onDefaultMaterial)
        {
            onDefaultMaterial = false;
            propertyList.RemoveAt(0);
        }
        DemarkForSwap();
        ma.ApplyMaterials(propertyList);
    }

    GameObject swapIconGO;
    public void MarkForSwap()
    {
        swapIconGO = Instantiate
            (swapIcon, transform.position, 
            Quaternion.identity, transform);
    }
    public void DemarkForSwap()
    {
        if (swapIconGO)
        {
            Destroy(swapIconGO);
        }
    }

}
