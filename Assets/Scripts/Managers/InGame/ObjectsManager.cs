using Scripts.Controllers.InGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Managers.InGame
{
    public class ObjectsManager : MonoBehaviour
    {
        [SerializeField] private CollectableController _collectable;
        [SerializeField] private List<Scripts.Controllers.InGame.ObstacleController> _obstacles;

        private static ObjectsManager _instance;
        public static ObjectsManager Instance { get { if (_instance == null) _instance = FindObjectOfType<ObjectsManager>(); return _instance; } }

        private int _collectableVelocity = 0;
        private bool _itStarted = false;
        private bool _itStoped = false;
        private int _counter;
        private Coroutine _moveCollectable;

        public void UpdateVelocityMovement(int streetVelocity)
        {
            if (!_itStarted) _itStarted = true;
            _collectableVelocity = streetVelocity;
        }

        public void StopAll()
        {
            _itStoped = true;
        }

        private void Update()
        {
            if(!_itStoped && _itStarted)
            {
                ManageCollectable();
                //ManageObstacles();
                //_counter = UIManager.Instance.GetActualScore();
            }
        }

        private void ManageCollectable()
        {
            if (_collectable.IsReady())
                if(_moveCollectable == null)
                    _moveCollectable = StartCoroutine(MoveCollectable());
        }

        private IEnumerator MoveCollectable()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(30, 40));
            _collectable.MoveCollectable(_collectableVelocity);
            _moveCollectable = null;
        }

        private void ManageObstacles()
        {
            throw new NotImplementedException();
        }

        public void DisableObstacleCollider()
        {
            _obstacles.ForEach(obstacle => obstacle.GetCapsuleCollider().enabled = false);
        }
    } 
}
