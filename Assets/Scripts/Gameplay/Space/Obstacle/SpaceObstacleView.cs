using Abstracts;
using System;
using Gameplay.Abstracts;
using UnityEngine;

namespace Gameplay.Space.Obstacle
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class SpaceObstacleView : MonoBehaviour
    {
        public event Action<EntityView> OnTriggerEnter = (EntityView _) => {};
        public event Action<EntityView> OnTriggerStay = (EntityView _) => { };
        public event Action<EntityView> OnTriggerExit = (EntityView _) => { };

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out EntityView unitView))
            {
                OnTriggerEnter(unitView);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if(collision.gameObject.TryGetComponent(out EntityView unitView))
            {
                OnTriggerStay(unitView);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out EntityView unitView))
            {
                OnTriggerExit(unitView);
            }
        }
    }
}