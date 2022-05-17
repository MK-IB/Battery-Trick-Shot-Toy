using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MultipleShooterManager : MonoBehaviour
{
    public static MultipleShooterManager instance;
    public GameObject phone;
    [SerializeField] int maxShoot;
    [Space(30)]
    public int shootNumCounter = 1;

    [HideInInspector] public PhoneMovementDrift currentPhoneScript;
    [HideInInspector] public ToyControllerDrift currentToyController;
    [HideInInspector] public Vector3 phonePosition;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        maxShoot = GameObject.FindGameObjectsWithTag("charger").Length;
    }

    public void CheckLevelComplete()
    {
        if (shootNumCounter == maxShoot)
        {
            if (ShopDataHolder.instance.GetToyLockState() == 1)
            {
                StartCoroutine(currentToyController.ToyDance());
                Debug.Log("all round cleared !");
            }else
            {
                StartCoroutine(currentPhoneScript.ChangeToChargingScreen());
                StartCoroutine(currentPhoneScript.LevelCompleted());
            }
        }
        else
        {
            shootNumCounter++;
            GameObject newPhone = Instantiate(phone, phonePosition, quaternion.identity);
         ShooterWithDrift.instance.SetNewPhone(newPhone);
        }
    }
}
