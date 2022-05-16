using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinUnlockManager : MonoBehaviour
{
    public static SkinUnlockManager instance;

    private float _fillAmount;

    public Image currentSkin;
    public Image fillingImage;
    public GameObject getSkinWithAdButton;
    public GameObject getCoinsWithAdButton;
    public GameObject nextLevelButton;
    public GameObject noThaksTextButton;
    public GameObject multiplierScale;

    [Space(20)] public TextMeshProUGUI percSkinLoadedText;
    public RectTransform multiplierIndicator;
    public TextMeshProUGUI coinMultipliedText;
    public TextMeshProUGUI coinsText;
    public GameObject toyDisplayArea;
    public GameObject toySkinDisplayUI;
    
    private int _skinUnlockedIndex;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        StartCoroutine(UnlockSkin());
        coinsText.SetText(ShopDataHolder.instance.GetCoins().ToString());
        multiplierIndicator.GetComponent<DOTweenAnimation>().tween.Restart();
        getCoinsWithAdButton.SetActive(true);
        nextLevelButton.SetActive(false);
        noThaksTextButton.SetActive(true);
        multiplierScale.SetActive(true);
    }

    private void Start()
    {
    }
    public List<int> GetUnlockedIndicesList()
    {
        var unlockedIndicesNum = ShopDataHolder.instance.GetUnlockedToy();
        char[] chars = unlockedIndicesNum.ToCharArray();
        int[] indicesArr = Array.ConvertAll(chars, c => (int)Char.GetNumericValue(c));
        return new List<int>(indicesArr);
    }
    IEnumerator UnlockSkin()
    {
        List<int> unlockedIndicesList = GetUnlockedIndicesList();

        for (int i = 0; i < unlockedIndicesList.Count; i++)
        {
            print(unlockedIndicesList[i]);
        }
        if (unlockedIndicesList.Count == 1)
        {
            currentSkin.sprite =
                ShopDataHolder.instance.toySkins[0];
        }
        else
        {
            currentSkin.sprite =
                ShopDataHolder.instance.toySkins[unlockedIndicesList[unlockedIndicesList.Count - 1] + 1];
        }
        /*if(ShopDataHolder.instance.GetUnlockedToy() < 0 && ShopDataHolder.instance.GetUnlockedToy() + 1 <= ShopManager.instance.currentUnlockIndex)
            currentSkin.sprite = ShopDataHolder.instance.toySkins[ShopManager.instance.currentUnlockIndex];
        else currentSkin.sprite = ShopDataHolder.instance.toySkins[ShopDataHolder.instance.GetUnlockedToy()];*/

        _fillAmount = PlayerPrefs.GetFloat("skinFillAmount", 0);
        _fillAmount += 0.25f;
        DOTween.To(() => fillingImage.fillAmount, x => fillingImage.fillAmount = x, _fillAmount, 1f);
        percSkinLoadedText.SetText((_fillAmount * 100).ToString() + "% UNLOCKED");
        if (_fillAmount < 1)
        {
            getSkinWithAdButton.SetActive(false);
            percSkinLoadedText.gameObject.SetActive(true);
            toySkinDisplayUI.SetActive(true);
            toyDisplayArea.SetActive(false);
        }
        else
        {
            fillingImage.fillAmount = 0;
            _fillAmount = 0;
            PlayerPrefs.SetFloat("skinFillAmount", 0);
            
            toySkinDisplayUI.SetActive(false);
            toyDisplayArea.SetActive(true);
            percSkinLoadedText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.25f);
            getSkinWithAdButton.SetActive(true);
            ShopDataHolder.instance.SetUnlockedToy(ShopDataHolder.instance.GetUnlockedToy() + _skinUnlockedIndex++);
            print("Skin index added = " + ShopDataHolder.instance.GetUnlockedToy());
            yield return new WaitForSeconds(1);
            ShopDataHolder.instance.UnlockShopToy();
        }

        //print(" unlockde skin saved= " + ShopDataHolder.instance.GetUnlockedToy());
    }
    
    
    public void GetSkinWithAd()
    {
        if (ISManager.instance)
            ISManager.instance.ShowInterstitialAds();
        ShopDataHolder.instance.SetToyLockState(1);
        ShopDataHolder.instance.SetShowToy(1);
        //ShopDataHolder.instance.SetUnlockedToy(ShopDataHolder.instance.GetUnlockedToy() + _skinUnlockedIndex++);

    }

    public void GetUnlockedSkin()
    {
        ISManager.instance.ShowInterstitialAds();
        getSkinWithAdButton.SetActive(false);
        percSkinLoadedText.gameObject.SetActive(true);
        percSkinLoadedText.SetText("Congratulations ! Skin Added");
    }

    public void SaveSkinFillAmount()
    {
        PlayerPrefs.SetFloat("skinFillAmount", _fillAmount);
    }

    public void GetMultiplierCoins()
    {
        var value = Mathf.InverseLerp(-312, 312, multiplierIndicator.anchoredPosition.x);

        if (value > 0.6f)
            value = 1f - value;

        if (value < 0.2f)
            UpdateCoinClaimValue(2);
        else if (value < 0.4f && value > 0.2f)
            UpdateCoinClaimValue(3);
        else
            UpdateCoinClaimValue(5);
    }

    private int _claimedCoins;

    void UpdateCoinClaimValue(float val)
    {
        _claimedCoins = (int) (val * 100);
        coinMultipliedText.SetText(_claimedCoins.ToString());
    }

    public void PressMultipliedCoins()
    {
        if (ISManager.instance)
            ISManager.instance.ShowInterstitialAds();
        multiplierIndicator.GetComponent<DOTweenAnimation>().tween.Pause();
        StartCoroutine(ChangeButtonToNextLevel());
    }

    IEnumerator ChangeButtonToNextLevel()
    {
        yield return new WaitForSeconds(1);
        int totalCoins = _claimedCoins + ShopDataHolder.instance.GetCoins();
        Debug.Log("claimed coins = " + _claimedCoins);
        coinsText.SetText(totalCoins.ToString());
        ShopDataHolder.instance.SetCoins(totalCoins);

        getCoinsWithAdButton.SetActive(false);
        nextLevelButton.SetActive(true);
        noThaksTextButton.SetActive(false);
        multiplierScale.SetActive(false);
        Debug.Log("Coins added = " + totalCoins);
    }
}