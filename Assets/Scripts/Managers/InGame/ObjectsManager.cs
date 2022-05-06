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

        private float _objectsVelocity = 0;
        private bool _itStarted = false;
        private bool _itStoped = false;
        private bool _readyToLaunch = true;
        private Coroutine _moveCollectable;
        private int _index;

        public void UpdateVelocityMovement(int streetVelocity)
        {
            if (!_itStarted) StartCoroutine(WaitUntilStarts(4));
            _objectsVelocity = streetVelocity * 1.05f;
        }

        private IEnumerator WaitUntilStarts(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _itStarted = true;
        }

        public void StopAll()
        {
            _itStoped = true;
        }

        private void Update()
        {
            if(!_itStoped && _itStarted)
            {
                ManageCollectable(false);
                ManageObstacles();
            }
            if (_itStoped) ManageCollectable(true);

        }

        private void ManageCollectable(bool stop)
        {
            if (!stop)
            {
                if (_collectable.IsReady())
                    if (_moveCollectable == null)
                        _moveCollectable = StartCoroutine(MoveCollectable());
            }
            else 
                if(_moveCollectable!=null)
                    StopCoroutine(_moveCollectable);
        }

        private IEnumerator MoveCollectable()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(15, 20));
            _collectable.MoveCollectable(_objectsVelocity);
            _moveCollectable = null;
        }

        private void ManageObstacles()
        {
            if (_readyToLaunch)
            {
                _index = UnityEngine.Random.Range(0, _obstacles.Count);
                if (_obstacles[_index].IsReady())
                    MoveObstacle();
            }
            else
            {
                if (_obstacles[_index].LaunchAnother())
                    _readyToLaunch = true;
            }
        }

        private void MoveObstacle()
        {
            _readyToLaunch = false;
            _obstacles[_index].MoveObstacle(_objectsVelocity);
        }

        public void EnableDamageCollider(bool value)
        {
            _obstacles.ForEach(obstacle => {
                obstacle.GetDamageCollider().enabled = value;
            });
        }
    } 
}
