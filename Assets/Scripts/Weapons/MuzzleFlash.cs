using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuzzleFlash : MonoBehaviour
{
    [SerializeField] Light muzzleLight;
    [SerializeField] List<Sprite> flashList;

    public void FlashMuzzle(float damage, int bulletsPerShot, float duration)
    {
        StartCoroutine(muzzleFlash(damage, bulletsPerShot, duration));
    }

    IEnumerator muzzleFlash(float damage, int bulletsPerShot, float duration)
    {
        var image = gameObject.GetComponent<Image>();

        muzzleLight.enabled = true;
        image.sprite = flashList[RandomSprite()];
        image.enabled = true;

        yield return new WaitForSeconds(duration);

        muzzleLight.enabled = false;
        gameObject.GetComponent<Image>().enabled = false;
    }

    int RandomSprite()
    {
        return Random.Range(0, flashList.Count);
    }
}
