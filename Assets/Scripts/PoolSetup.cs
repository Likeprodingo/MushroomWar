using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

[AddComponentMenu("Pool/PoolSetup")]
public class PoolSetup : MonoBehaviour {
	
    #region Unity scene settings
    [SerializeField] 
    private Transform _player =default;
    [SerializeField] 
    private PoolManager.PoolPart[] _pools = default;
    #endregion

    #region Methods
    void OnValidate() {
        for (int i = 0; i < _pools.Length; i++) {
            _pools[i]._name = _pools[i]._prefab.name;
        }
    }

    void Awake() {
        Initialize ();
    }

    void Initialize () {
        PoolManager.Initialize(_pools, _player);
    }
    #endregion

}