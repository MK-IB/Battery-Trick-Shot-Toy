using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShopDataHolder : MonoBehaviour
{
    public static ShopDataHolder instance;

    public List<Sprite> mobileSkins;
    public int totalCoins;

    [HideInInspector] public GameObject _skinUnlockCanvas;
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

    void ActivateSkinUnlockCanvas()
    {
        _skinUnlockCanvas.SetActive(true);
    }

    void DeactivateSkinUnlockCanvas()
    {
        _skinUnlockCanvas.SetActive(false);
    }
    public void SetUnlockedToy(string indices)
    {
        PlayerPrefs.SetString("skinIndices", indices);
    }
    
    public string GetUnlockedToy()
    {
        return PlayerPrefs.GetString("skinIndices", "0");
    }
    public void SetToyLockState(int i)
    {
        PlayerPrefs.SetInt("skinLockState", i);
    }

    public void SetShowToy(int i)
    {
        PlayerPrefs.SetInt("canShowToy", i);
    }
    public int GetShowToy()
    {
        return PlayerPrefs.GetInt("canShowToy", 0);
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
