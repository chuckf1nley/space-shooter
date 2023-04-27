using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float _camShakeTime = 1f;
    [SerializeField] private AnimationCurve _cameraShakeCurve;

    private bool isShaking;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ShakeCameraRoutine()
    {
        isShaking = true;
        Vector3 originalPos = transform.position;
        float timeRunning = 0f;

        while (timeRunning < _camShakeTime)
        {
            timeRunning += Time.deltaTime;
            float strength = _cameraShakeCurve.Evaluate(timeRunning / _camShakeTime);
            transform.position = originalPos + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = originalPos;
        isShaking = false;

    }

    public void ShakeCamera()
    {
        StartCoroutine(ShakeCameraRoutine());

        if (isShaking)
        {
            isShaking = false;
            StopCoroutine(ShakeCameraRoutine());
        }

    }

}
