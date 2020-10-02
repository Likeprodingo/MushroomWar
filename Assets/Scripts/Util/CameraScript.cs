using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _objToFollow; 
    [SerializeField]
    private float _speed = 2.0f;
    [SerializeField]
    private float _xDistance = 10f;
    [SerializeField]
    private float _zDistance = 10f;
    
    public GameObject ObjToFollow
    {
        get => _objToFollow;
        set => _objToFollow = value;
    }

    void FixedUpdate()
    {
        float interpolation = _speed * Time.deltaTime;
        Vector3 position = transform.position;
        position.z = Mathf.Lerp(this.transform.position.z, _objToFollow.transform.position.z-_zDistance, interpolation);
        position.x = Mathf.Lerp(this.transform.position.x, _objToFollow.transform.position.x - _xDistance, interpolation);
        transform.position = position;
    }
}
