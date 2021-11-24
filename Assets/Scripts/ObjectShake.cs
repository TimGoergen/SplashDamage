using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShake : MonoBehaviour
{
    private Vector3 originPosition;
    private Quaternion originRotation;
    public float shake_decay = 0.003f;
    public float shake_intensity = .2f;
    public bool isShaking = false;
    public float rangeAdjust = 0.2f;

    private float temp_shake_intensity = 0;

    private void OnMouseEnter() {
        Shake();
    }

    void Update()
    {
        if (temp_shake_intensity > 0 && isShaking == true)
        {
            transform.position = originPosition + Random.insideUnitSphere * temp_shake_intensity;
            transform.rotation = new Quaternion(
                originRotation.x + Random.Range(-temp_shake_intensity, temp_shake_intensity) * rangeAdjust,
                originRotation.y + Random.Range(-temp_shake_intensity, temp_shake_intensity) * rangeAdjust,
                originRotation.z + Random.Range(-temp_shake_intensity, temp_shake_intensity) * rangeAdjust,
                originRotation.w + Random.Range(-temp_shake_intensity, temp_shake_intensity) * rangeAdjust);
            temp_shake_intensity -= shake_decay;
        }
        else
        {
            isShaking = false;
        }
    }

    public void Shake()
    {
        if (!isShaking)
        {
            isShaking = true;
            originPosition = transform.position;
            originRotation = transform.rotation;
            temp_shake_intensity = shake_intensity;
        }
    }}
