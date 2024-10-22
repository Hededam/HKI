using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if INVECTOR_BASIC || INVECTOR_AI_TEMPLATE
using Invector;
#endif

namespace BNG
{
    public class Damageable : MonoBehaviour
    {
        public float Health = 100;
        public int XPCount = 10;
        private float _startingHealth;
        public bool SelfDestruct = false;
        public float SelfDestructDelay = 0.1f;
        public GameObject SpawnOnDeath;
        public List<GameObject> ActivateGameObjectsOnDeath;
        public List<GameObject> DeactivateGameObjectsOnDeath;
        public List<Collider> DeactivateCollidersOnDeath;
        public bool DestroyOnDeath = true;
        public bool DropOnDeath = true;
        public float DestroyDelay = 0f;
        public bool Respawn = false;
        public float RespawnTime = 10f;
        public bool RemoveBulletHolesOnDeath = true;
        public FloatEvent onDamaged;
        public UnityEvent onDestroyed;
        public UnityEvent onRespawn;
        private PlayerXp playerXp; // Reference til PlayerXp-scriptet på spilleren
#if INVECTOR_BASIC || INVECTOR_AI_TEMPLATE
        public bool SendDamageToInvector = true;
#endif

        bool destroyed = false;
        Rigidbody rigid;
        bool initialWasKinematic;
        private RagdollController ragdollController;

        private void Start()
        {
            _startingHealth = Health;
            rigid = GetComponent<Rigidbody>();
            if (rigid)
            {
                initialWasKinematic = rigid.isKinematic;
            }
            ragdollController = GetComponent<RagdollController>();
           
            playerXp = GameObject.Find("Gamestuff").GetComponent<PlayerXp>(); // Find PlayerXp-komponenten på "Player"-objektet
        }

        void OnEnable()
        {
            if (SelfDestruct)
            {
                Invoke("DestroyThis", SelfDestructDelay);
            }
        }

        public virtual void DealDamage(float damageAmount)
        {
            Debug.Log("Dealing damage: " + damageAmount);
            DealDamage(damageAmount, transform.position);
        }

        public virtual void DealDamage(float damageAmount, Vector3? hitPosition = null, Vector3? hitNormal = null, bool reactToHit = true, GameObject sender = null, GameObject receiver = null)
        {
            if (destroyed)
            {
                return;
            }
            Debug.Log("Damage received: " + damageAmount + ", Health remaining: " + Health);
            Health -= damageAmount;
            onDamaged?.Invoke(damageAmount);

            if (Health <= 0)
            {
                DestroyThis();
            }
        }


        public virtual void DestroyThis()
        {
            Health = 0;
            destroyed = true;
            playerXp.GainXP(XPCount); // Tilføj 10 XP

            if (ragdollController != null)
            {
                ragdollController.ActivateRagdoll();
            }

            Animator animator = GetComponent<Animator>();
            if (animator)
            {
                animator.SetTrigger("Die");
            }

            foreach (var go in ActivateGameObjectsOnDeath)
            {
                go.SetActive(true);
            }

            foreach (var go in DeactivateGameObjectsOnDeath)
            {
                go.SetActive(false);
            }

            foreach (var col in DeactivateCollidersOnDeath)
            {
                col.enabled = false;
            }

            if (SpawnOnDeath != null)
            {
                var go = GameObject.Instantiate(SpawnOnDeath);
                go.transform.position = transform.position;
                go.transform.rotation = transform.rotation;
            }

            if (rigid)
            {
                rigid.isKinematic = true;
            }

            if (onDestroyed != null)
            {
                onDestroyed.Invoke();
            }

            if (DestroyOnDeath)
            {
                Destroy(this.gameObject, DestroyDelay);
            }
            else if (Respawn)
            {
                StartCoroutine(RespawnRoutine(RespawnTime));
            }

            Grabbable grab = GetComponent<Grabbable>();
            if (DropOnDeath && grab != null && grab.BeingHeld)
            {
                grab.DropItem(false, true);
            }

            if (RemoveBulletHolesOnDeath)
            {
                BulletHole[] holes = GetComponentsInChildren<BulletHole>();
                foreach (var hole in holes)
                {
                    GameObject.Destroy(hole.gameObject);
                }
                Transform decal = transform.Find("Decal");
                if (decal)
                {
                    GameObject.Destroy(decal.gameObject);
                }
            }
        }

        IEnumerator RespawnRoutine(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Health = _startingHealth;
            destroyed = false;

            foreach (var go in ActivateGameObjectsOnDeath)
            {
                go.SetActive(false);
            }

            foreach (var go in DeactivateGameObjectsOnDeath)
            {
                go.SetActive(true);
            }
            foreach (var col in DeactivateCollidersOnDeath)
            {
                col.enabled = true;
            }

            if (rigid)
            {
                rigid.isKinematic = initialWasKinematic;
            }

            if (onRespawn != null)
            {
                onRespawn.Invoke();
            }
        }
    }
}
