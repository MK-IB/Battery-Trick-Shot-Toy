using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;

public class ToyController : MonoBehaviour
{

    private void Start()
    {
       
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Battery"))
        {
            other.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
            {
                other.gameObject.SetActive(false); 
            });
            AudioManager.instance.PlayClip(AudioManager.instance.pop);
        }

        if (other.gameObject.CompareTag("charger"))
        {
            //transform.parent = null;
            transform.parent.DOLocalRotate(new Vector3(0, 90, 0), 0.5f);
            transform.parent.localPosition = new Vector3(0.7f, 0, 0.6f);
            Rotation rotation = transform.root.GetComponent<Rotation>();
            if (rotation) rotation.enabled = false;
            StartCoroutine(ToyDance());
        }
        if (other.gameObject.CompareTag("obstacle"))
        {
            other.GetComponent<Collider>().enabled = false;
            GameManager.instance.gameOver = true;
            AudioManager.instance.bgAudioSource.enabled = false;
            Vibration.Vibrate(27);
            Time.timeScale = 1;
            transform.parent.parent.GetComponent<SplineFollower>().enabled = false;
            GetComponent<Collider>().enabled = false;
            VirtualCameraManager.instance.phoneFollower.Follow = null;
            AudioManager.instance.PlayClip(AudioManager.instance.obstacleHit);
            GameManager.instance.StartCoroutine(GameManager.instance.LevelFailed(1.5f));
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
        lastFocusVcam.LookAt(transform);
        AudioManager.instance.bgAudioSource.enabled = false;
        
        yield return new WaitForSeconds(1);
        GetComponent<Animator>().enabled = true;
        
        GameManager.instance.StartCoroutine(GameManager.instance.LevelComplete(2.5f));
    }
}
