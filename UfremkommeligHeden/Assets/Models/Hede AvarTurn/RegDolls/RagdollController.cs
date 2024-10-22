using System.Collections.Generic;
using UnityEngine;

namespace BNG
{
    public class RagdollController : MonoBehaviour
    {
        public Animation animationComponent;
        public Animator animatorComponent;
        public List<Rigidbody> ragdollRigidbodies;
        public List<Collider> ragdollColliders;
        public bool startWithRagdollRigidbodiesActive = false;
        public bool startWithRagdollCollidersActive = false;

        private void Start()
        {
            SetRagdollActive(startWithRagdollRigidbodiesActive, startWithRagdollCollidersActive);
        }

        public void ActivateRagdoll()
        {
            if (animationComponent != null)
            {
                animationComponent.enabled = false;
            }
            if (animatorComponent != null)
            {
                animatorComponent.enabled = false;
            }
            SetRagdollActive(true, true);
        }

        private void SetRagdollActive(bool rigidbodiesActive, bool collidersActive)
        {
            foreach (Rigidbody rb in ragdollRigidbodies)
            {
                rb.isKinematic = !rigidbodiesActive;
            }
            foreach (Collider col in ragdollColliders)
            {
                col.enabled = collidersActive;
            }
        }
    }
}
