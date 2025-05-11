using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;//player
    [SerializeField] private float smoothSpeed;

    [SerializeField] private float minX, minY, maxX, maxY;

    private Vector3 screenShakeActive;
    private float screenShakeAmount;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), smoothSpeed * Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY), -10);

        CameraShake();
    }

    private void CameraShake()
    {
        if (screenShakeAmount > 0)
        {
            screenShakeActive = new Vector3(Random.Range(-screenShakeAmount, screenShakeAmount), Random.Range(-screenShakeAmount, screenShakeAmount), 0f);
            screenShakeAmount -= Time.deltaTime;
        }
        else
        {
            screenShakeActive = Vector3.zero;
        }

        transform.position += screenShakeActive;//current position += shake
    }

    public void ScreenShake(float toShake)
    {
        //if(toShake > screenShakeAmount)
        screenShakeAmount = toShake;
    }

}
