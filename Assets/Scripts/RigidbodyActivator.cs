using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class RigidbodyActivator : MonoBehaviour
    {
        private List<KeyValuePair<float, Transform>> _anims = new List<KeyValuePair<float, Transform>>();

        private bool _activated = false;
        private float _currentTime = 0;

        private void FixedUpdate()
        {
            if (_activated)
            {
                var animsToRemove = new List<KeyValuePair<float, Transform>>();
                foreach (var anim in _anims)
                {
                    if (_currentTime < anim.Key)
                    {
                        continue;
                    }

                    anim.Value.localScale -= Vector3.one / 2 * Time.deltaTime;

                    if (anim.Value.localScale.y < 0.2f)
                    {
                        animsToRemove.Add(anim);
                    }
                }

                foreach (var anim in animsToRemove)
                {
                    _anims.Remove(anim);
                }

                if (_anims.Count < 1)
                {
                    Destroy(gameObject);
                }

                _currentTime += Time.deltaTime;
            }
        }

        public void Activate()
        {
            if (_activated)
            {
                return;
            }

            foreach (var child in transform.GetComponentsInChildren<Collider>())
            {
                if (child.isTrigger)
                {
                    continue;
                }

                child.enabled = true;
                child.attachedRigidbody.isKinematic = false;
                _anims.Add(new KeyValuePair<float, Transform>(1 + (Random.value - 0.5f), child.transform));
                //child.attachedRigidbody.transform.DOScale(0, 2).SetDelay(1 + (Random.value - 0.5f)).OnComplete(OnScaleCompleted);
            }

            _activated = true;
        }

        private void OnScaleCompleted()
        {
            Destroy(gameObject);
        }
    }
}
