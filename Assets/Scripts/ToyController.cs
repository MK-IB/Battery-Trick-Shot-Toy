using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;

public class ToyController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Battery"))
        {
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("charger"))
        {
            //transform.parent = null;
            transform.parent.DOLocalRotate(new Vector3(0, 90, 0), 0.5f);
            transform.parent.localPosition = new Vector3(0, 0, 0.76f);
            StartCoroutine(ToyDance());
        }
        if (other.gameObject.CompareTag("halfWayActionTrigger") && !GameManager.instance.gameOver)
        {
            VirtualCameraManager.instance.StartCoroutine(VirtualCameraManager.instance.HalfWayCameraAction());
        }
    }

    IEnumerator ToyDance()
    {
        Transform lastFocusVcam = VirtualCameraManager.instance.phoneLastFocus.transform;
        lastFocusVcam.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z - 4);
        AudioManager.instance.bgAudioSource.enabled = false;
        
        yield return new WaitForSeconds(1);
        GetComponent<Animator>().enabled = true;
        
        GameManager.instance.StartCoroutine(GameManager.instance.LevelComplete(2.5f));
    }
}
