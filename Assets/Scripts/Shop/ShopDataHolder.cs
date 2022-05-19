using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShopDataHolder : MonoBehaviour
{
    public static ShopDataHolder instance;

    public List<Sprite> toySkins;
    public int totalCoins;

    [HideInInspector] public GameObject _skinUnlockCanvas;
    public List<GameObject> toysList;
    private int _toyCounter = 1;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void OnEnable()
    {
        
    }

    private void Start()
    {
        _skinUnlockCanvas = transform.GetChild(0).gameObject;
        CommonUIEventsManager.instance.LevelStartEvent += DeactivateSkinUnlockCanvas;
        CommonUIEventsManager.instance.LevelCompleteEvent += ActivateSkinUnlockCanvas;
    }
    
    public void UnlockShopToy(int index)
    {
        DeacativateAllToys();
        toysList[index].SetActive(true);
    }

    public void ShowToyByIndex(int index)
    {
        DeacativateAllToys();
        for (int i = 0; i < toysList.Count; i++)
        {
            if(i == index)
                toysList[i].SetActive(true);
        }
    }

    void DeacativateAllToys()
    {
        //DEACTIVATE ALL
        toysList[0].transform.parent.GetChild(0).gameObject.SetActive(false);
        for (int i = 0; i < toysList.Count; i++)
        {
            toysList[i].SetActive(false);
        }
    }
    void ActivateSkinUnlockCanvas()
    {
        _skinUnlockCanvas.SetActive(true);
    }

    void DeactivateSkinUnlockCanvas()
    {
        _skinUnlockCanvas.SetActive(false);
    }
    
    public void SetDisplayedToys(string index)
    {
        PlayerPrefs.SetString("rewardedSKin", index);
    }
    public string GetDisplayedToys()
    {
        return PlayerPrefs.GetString("rewardedSKin", "0");
    }
    public void SetUnlockedToy(string indices)
    {
        PlayerPrefs.SetString("skinIndices", indices);
    }
    
    public string GetUnlockedToy()
    {
        return PlayerPrefs.GetString("skinIndices", "0");
    }
    public void SetShowToy(int i)
    {
        PlayerPrefs.SetInt("canShowToy", i);
    }
    public int GetShowToy()
    {
        return PlayerPrefs.GetInt("canShowToy", 0);
    }
    
    //GETTING AND SETTING THE TOY LOCK STATE
    public void SetToyLockState(int i)
    {
        PlayerPrefs.SetInt("skinLockState", i);
    }
    public int GetToyLockState()
    {
        return PlayerPrefs.GetInt("skinLockState", 0);
    }
    public void SetCoins(int coin)
    {
        PlayerPrefs.SetInt("coins", coin);
    }

    public int GetCoins()
    {
        return PlayerPrefs.GetInt("coins", 0);
    }

    public void SetDemoToyIndex(int index)
    {
        PlayerPrefs.SetInt("demoSkinIndex", index);
    }
    public int GetDemoToyIndex()
    {
        return PlayerPrefs.GetInt("demoSkinIndex", 0);
    }
}
