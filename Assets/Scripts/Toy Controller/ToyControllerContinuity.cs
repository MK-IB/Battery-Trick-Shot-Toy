using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;

public class ToyControllerContinuity : MonoBehaviour
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
            Rotation rotation = transform.root.GetComponent<Rotation>();
            if (rotation) rotation.enabled = false;
            GetComponent<Animator>().enabled = true;
            
            Vibration.Vibrate(27);
            
            transform.parent.parent.GetComponent<SplineFollower>().enabled = false;
            ContinuityManager.instance.currentToyController = this;
            ContinuityManager.instance.CheckLevelComplete();
            GetComponent<Collider>().enabled = false;
            GetComponent<ToyControllerContinuity>().enabled = false;
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

    public IEnumerator ToyDance()
    {
        Transform lastFocusVcam = VirtualCameraManager.instance.phoneLastFocus.transform;
        lastFocusVcam.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z - 4);
        AudioManager.instance.bgAudioSource.enabled = false;
        
        yield return new WaitForSeconds(1);
        GetComponent<Animator>().enabled = true;
        
        GameManager.instance.StartCoroutine(GameManager.instance.LevelComplete(2.5f));
    }
}
