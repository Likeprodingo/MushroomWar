using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Building : MonoBehaviour
{
    [SerializeField]
    private Vector2Int _size = Vector2Int.one;
    [SerializeField]
    private Renderer _renderer = null;
    private Color _normalColor = default;

    public Vector2Int Size => _size;

    private void Awake()
    {
        _normalColor = _renderer.material.color;
    }

    public void SetTransparent(bool availible)
    {
        
        if (availible)
        {
           _renderer.material.color = Color.green;
        }
        else
        {
            _renderer.material.color = Color.red;
        }
    }
    
    public void SetNormal()
    {
        _renderer.material.color = _normalColor;
    }
    
    
    private void OnDrawGizmos()
    {
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                Gizmos.color = new Color(1f, 0.09f, 0.81f, 0.3f);
                Gizmos.DrawCube(transform.position +  new Vector3(x,0, y),new Vector3(1,0.1f,1));
            }
        }
    }
}
