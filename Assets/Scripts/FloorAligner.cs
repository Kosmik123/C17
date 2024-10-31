using UnityEngine;

namespace Bipolar
{
    public class FloorAligner : MonoBehaviour
    {
        [SerializeField]
        private Vector3 placementOffset;
        [SerializeField]
        private float detectionLength = 10;
        [SerializeField]
        private LayerMask detectedLayers = -1;
        [SerializeField]
        private bool alignRotation;

        private Collider[] selfColliders;

        private static readonly RaycastHit[] hits = new RaycastHit[8];

        private void Awake()
        {
            selfColliders = GetComponentsInChildren<Collider>();
        }

        private void Start()
        {
            Vector3 origin = transform.position + 0.5f * detectionLength * Vector3.up;
            int detectedCollidersCount = Physics.RaycastNonAlloc(origin, Vector3.down, hits, detectionLength, detectedLayers);
            if (detectedCollidersCount <= 0)
                return;

            for (int i = 0; i < detectedCollidersCount; i++)
            {
                var raycastHit = hits[i];
                if (System.Array.IndexOf(selfColliders, raycastHit.collider) > -1)
                    continue;

                transform.position = raycastHit.point + placementOffset;
                if (alignRotation)
                    transform.up = raycastHit.normal;
                return;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = (Color.red + Color.yellow) / 2;
            Gizmos.DrawLine(transform.position + 0.5f * detectionLength * Vector3.up, transform.position + 0.5f * detectionLength * Vector3.down);
        }

    }



}
