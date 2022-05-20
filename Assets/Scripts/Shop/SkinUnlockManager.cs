using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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
    [Space(20)] public GameObject coinEffect;
    public GameObject noThanksTextButton;
    public RectTransform coinRect;
    public GameObject nextButtonOnUnlock;
    public TextMeshProUGUI skinInformationText;
    public TextMeshProUGUI coinRewardInfoText;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        StartCoroutine(UnlockSkin());
        coinsText.SetText(ShopDataHolder.instance.GetCoins().ToString());
        skinInformationText.SetText("NEW SKIN ON ITS WAY !");
        nextButtonOnUnlock.SetActive(false);
        multiplierIndicator.GetComponent<DOTweenAnimation>().tween.Restart();
        coinRewardInfoText.gameObject.SetActive(false);
        if (ISManager.instance.isRewardedVideoAvaliable)
        {
            getCoinsWithAdButton.SetActive(true);
            nextLevelButton.SetActive(false);
            noThanksTextButton.SetActive(true);
            multiplierScale.SetActive(true);
        }
        else
        {
            getCoinsWithAdButton.SetActive(false);
            nextLevelButton.SetActive(true);
            noThanksTextButton.SetActive(false);
            multiplierScale.SetActive(false);
        }
    }

    private void Start()
    {
    }
    public List<int> GetUnlockedIndicesList()
    {
        var unlockedIndicesNum = ShopDataHolder.instance.GetDisplayedToys();
        char[] chars = unlockedIndicesNum.ToCharArray();
        int[] indicesArr = Array.ConvertAll(chars, c => (int)Char.GetNumericValue(c));
        return new List<int>(indicesArr);
    }

    private List<int> unlockedIndicesList =  new List<int>();
    IEnumerator UnlockSkin()
    {
        unlockedIndicesList = GetUnlockedIndicesList();

        _skinUnlockedIndex = PlayerPrefs.GetInt("skinIndex", 0);
        if(_skinUnlockedIndex < ShopDataHolder.instance.toySkins.Count)
            currentSkin.sprite = ShopDataHolder.instance.toySkins[_skinUnlockedIndex];
        Debug.Log("Skin Index = " + _skinUnlockedIndex);
        /*for (int i = 0; i < unlockedIndicesList.Count; i++)
        {
            print(unlockedIndicesList[i]);
        }*/
        /*if (unlockedIndicesList.Count == 1)
        {
            currentSkin.sprite =
                ShopDataHolder.instance.toySkins[0];
        }
        else
        {
            currentSkin.sprite =
                ShopDataHolder.instance.toySkins[unlockedIndicesList[unlockedIndicesList.Count - 1]];
        }*/
        /*if(ShopDataHolder.instance.GetUnlockedToy() < 0 && ShopDataHolder.instance.GetUnlockedToy() + 1 <= ShopManager.instance.currentUnlockIndex)
            currentSkin.sprite = ShopDataHolder.instance.toySkins[ShopManager.instance.currentUnlockIndex];
        else currentSkin.sprite = ShopDataHolder.instance.toySkins[ShopDataHolder.instance.GetUnlockedToy()];*/

        _fillAmount = PlayerPrefs.GetFloat("skinFillAmount", 0);
        _fillAmount += 0.25f;
        DOTween.To(() => fillingImage.fillAmount, x => fillingImage.fillAmount = x, _fillAmount, 1f).OnComplete(() =>
        {
         CheckSkinUnlock();   
        });
        percSkinLoadedText.SetText((_fillAmount * 100).ToString() + "% UNLOCKED");
        yield return null;
        //print(" unlockde skin saved= " + ShopDataHolder.instance.GetUnlockedToy());
    }

    void CheckSkinUnlock()
    {
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
            //yield return new WaitForSeconds(0.25f);
            getSkinWithAdButton.SetActive(true);
            ShopDataHolder.instance.UnlockShopToy(PlayerPrefs.GetInt("skinIndex", 0));
            PlayerPrefs.SetInt("skinIndex", PlayerPrefs.GetInt("skinIndex", 0) + 1);
            
            /*ShopDataHolder.instance.SetDisplayedToys(ShopDataHolder.instance.GetDisplayedToys() + 1);
            print("Skin index added = " + GetUnlockedIndicesList()[GetUnlockedIndicesList().Count - 1]);
            //yield return new WaitForSeconds(1);
            unlockedIndicesList = GetUnlockedIndicesList();

            for (int i = 0; i < unlockedIndicesList.Count; i++)
            {
                print(unlockedIndicesList[i]);
            }
            */

        }
    }
    
    private bool _isUnlockedRewardPlayed, _getCoinRewardedPlayed;
    public void GetSkinWithAd()
    {
        ISManager.instance.ShowRewadedVideo();
        _getCoinRewardedPlayed = false;
        _isUnlockedRewardPlayed = true;
        ShopDataHolder.instance.SetToyLockState(1);
        ShopDataHolder.instance.SetShowToy(1);
        //ShopDataHolder.instance.SetUnlockedToy(ShopDataHolder.instance.GetUnlockedToy() + unlockedIndicesList[unlockedIndicesList.Count - 1]);

        int valToAdd = PlayerPrefs.GetInt("skinIndex", 0) - 1;
        if(valToAdd >= 0)
            ShopDataHolder.instance.SetUnlockedToy(ShopDataHolder.instance.GetUnlockedToy() + valToAdd);
        Debug.Log("Skin Index after getting toy= " + _skinUnlockedIndex);
        Debug.Log("Skin Index final = " + PlayerPrefs.GetInt("skinIndex", 0));

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
        _getCoinRewardedPlayed = true;
        _isUnlockedRewardPlayed = false;
        ISManager.instance.ShowRewadedVideo();
        multiplierIndicator.GetComponent<DOTweenAnimation>().tween.Pause();
    }
    
    //REWARD CALLBACK CALLS IT
    public void DecideRewardCallback()
    {
        if (_isUnlockedRewardPlayed)
        {
            skinInformationText.SetText("CONGRATULATIONS !!! YOU GOT NEW SKIN !");
            if(!nextLevelButton.activeSelf)
                nextButtonOnUnlock.SetActive(true);
            getSkinWithAdButton.SetActive(false);
        }else if (_getCoinRewardedPlayed)
        {
            StartCoroutine(ChangeButtonToNextLevel());
        }
    }

    IEnumerator ChangeButtonToNextLevel()
    {
        yield return new WaitForSeconds(1);
        int totalCoins = _claimedCoins + ShopDataHolder.instance.GetCoins();
        coinsText.SetText(totalCoins.ToString());
        ShopDataHolder.instance.SetCoins(totalCoins);

        getCoinsWithAdButton.SetActive(false);
        nextLevelButton.SetActive(true);
        noThaksTextButton.SetActive(false);
        multiplierScale.SetActive(false);
        coinRewardInfoText.gameObject.SetActive(true);
        coinRewardInfoText.SetText("WOAH ! YOU GOT " + _claimedCoins + " COINS !!");
        coinEffect.GetComponent<ParticleControlScript>().PlayControlledParticles(transform.position, coinRect);
    }
}