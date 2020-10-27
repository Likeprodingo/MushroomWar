using System.Collections;
using UnityEngine;

namespace Util
{
    public interface IHealth
    {
        void GetDamage(float damage);

        IEnumerator Death();
    }
}