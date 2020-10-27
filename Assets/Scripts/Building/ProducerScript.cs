using System.Collections;
using Drop;
using ObjectPool;
using UnityEngine;

namespace Building
{
    public class ProducerScript : MonoBehaviour
    {
        
        [SerializeField] private Sprite _essenceSprite = default;
        [SerializeField] private float _timeDelay = default;
        public IEnumerator ProduceEssence(DropScript dropScript)
        {
            yield return new WaitForSeconds(_timeDelay);
            dropScript.Type = PoolObjectType.Essence;
            dropScript.Sprite = _essenceSprite;
            Inventory.Inventory.Instance.AddItem(dropScript);
            dropScript.Despawn();
        }
    }
}