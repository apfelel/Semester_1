using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Flashbang : MonoBehaviour
{
    [SerializeField]
    private Light2D _light, _globalMid, _globalBack, _lightBack;

    [SerializeField]
    private GameObject _flock;
    [SerializeField]
    private ParticleSystem _fireFly;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        StartCoroutine(Flash());
        StartCoroutine(Animation());
        _flock.SetActive(true);
        _fireFly.Play();
    }

    IEnumerator Flash()
    {
        var globalManager = GlobalLightManager.Instance.gameObject;
        globalManager.SetActive(false);

        for (int i = 0; i < 20; i++)
        {
            _lightBack.intensity += 9f / 20;
            _globalMid.intensity -= 0.06f;
            _globalBack.intensity -= 0.03f;
            yield return new WaitForFixedUpdate();
            _light.intensity += 0.04f;
        }

        globalManager.SetActive(true);
        yield return new WaitForSeconds(2);
        for (int i = 200; i > 0; i--)
        {
            _lightBack.intensity -= 9f / 200;
            yield return new WaitForFixedUpdate();
            _light.intensity = Mathf.Max(0, _light.intensity - 0.02f);
        }
        _light.intensity = 0;
        Destroy(this);

    }

    IEnumerator Animation()
    {
        GameManager.Instance.PlayerCinematic.MoveXSec(1, 2.4f);
        yield return new WaitForSeconds(2.4f);
        GameManager.Instance.PlayerCinematic.Wait(3);
        yield return new WaitForSeconds(2);

    }
}
