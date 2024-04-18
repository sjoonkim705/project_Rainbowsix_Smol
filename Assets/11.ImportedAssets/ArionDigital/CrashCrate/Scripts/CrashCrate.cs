namespace ArionDigital
{
    using System.Collections;
    using UnityEngine;

    public class CrashCrate : MonoBehaviour, IHitable
    {
        [Header("Whole Create")]
        public int RequiredHit = 2;
        private int _hitCount = 0;
        public MeshRenderer wholeCrate;
        public BoxCollider boxCollider;
        [Header("Fractured Create")]
        public GameObject fracturedCrate;


        public void Hit(int amount, Vector3 position)
        {
            _hitCount++;
            if (_hitCount == RequiredHit)
            {
                wholeCrate.enabled = false;
                boxCollider.enabled = false;
                fracturedCrate.SetActive(true);
                StartCoroutine(DropItemAndDestroy_Coroutine());
            }
            //crashAudioClip.Play();
        }

        private IEnumerator DropItemAndDestroy_Coroutine()
        {
            yield return new WaitForSeconds(1.5f);
            if (Random.Range(0,3) == 0)
            {
                ItemObjectFactory.Instance.MakePercent(transform.position);
            }
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }

    }
}