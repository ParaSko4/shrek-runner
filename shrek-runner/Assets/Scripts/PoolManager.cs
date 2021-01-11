using UnityEngine;

namespace Assets.Scripts
{
    public class PoolManager : MonoBehaviour
    {
        [HideInInspector]
        public DestructionWallPSPool destrWallPool;

        private void Awake()
        {
            destrWallPool = GetComponent<DestructionWallPSPool>();
        }
    }
}