using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Building
{
    public abstract class Building: MonoBehaviour
    {
        [SerializeField] protected float _activeTime = default;
        [SerializeField] protected MeshRenderer _renderer = default; 
        [SerializeField] protected float _darCoefficient = default;
        protected float CurrentActivation;
        protected Color StartColor;
        protected  abstract IEnumerator Attack();

        protected void OnEnable()
        {
            StartColor = _renderer.material.color;
            AddEssence();
        }

        public void AddEssence()
        {
            _renderer.material.color = StartColor;
            CurrentActivation += _activeTime;
            StartCoroutine(ActivationUpdate());
            StartCoroutine(Attack());
        }

        private IEnumerator ActivationUpdate()
        {
            while (CurrentActivation > 0)
            {
                yield return new WaitForSeconds(1);
                CurrentActivation -= 1;
            }

            var material = _renderer.material;
            Color color = material.color;
            color.b /= 1.7f;
            color.r /= 1.7f;
            color.g /= 1.7f;
            material.color = color;
        }
    }
}