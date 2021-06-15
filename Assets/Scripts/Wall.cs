using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class Wall : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.contactCount > 0)
            {
                if (collision.gameObject.TryGetComponent(out PlayerOLD player))
                {
                    player.IsStanned = true;
                }

                var contact = collision.contacts[0];
                //collision.collider.attachedRigidbody.velocity = -contact.normal * 6 + Vector3.up * 2;
                collision.collider.attachedRigidbody.velocity = -(collision.collider.attachedRigidbody.rotation * (Vector3.forward * 6)) + Vector3.up * 2;
            }
        }
    }
}
