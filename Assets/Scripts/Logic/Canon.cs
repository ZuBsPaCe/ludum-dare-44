using System;
using UnityEngine;

namespace zs.Logic
{
    public class Canon : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField]
        private SpriteRenderer _barrelSprite = null;

        [SerializeField]
        private float _viewDistance = 5f;

        [SerializeField]
        private float _waitAfterAim = 3f;

        [SerializeField]
        private float _shootDelay = 1f;

        [SerializeField]
        private float _rotateSpeed = 90f;

        [SerializeField]
        private Bullet _bulletPrefab = null;

        #endregion Serializable Fields

        #region Private Vars

        private float _currentLocalRotation = 0;
        private bool _scanDirectionUp;
        private float _lastAimTime = float.NegativeInfinity;
        private float _lastShootTime = float.NegativeInfinity;

        private RaycastHit2D[] _raycastHits = new RaycastHit2D[3];

        #endregion Private Vars

        #region Public Vars
        #endregion Public Vars

        #region Public Methods
        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
            Debug.Assert(_barrelSprite);
            Debug.Assert(_bulletPrefab);
        }

        void Start()
        {
        }
	
        void Update()
        {
            bool aimOnPlayer = false;

            if (CanAimOnPlayer(out Vector3 globalDir))
            {
                _lastAimTime = Time.time;

                Vector3 barrelGlobalDir = _barrelSprite.transform.rotation * Vector3.up;

                float angleDiff = Vector3.SignedAngle(barrelGlobalDir, globalDir, Vector3.forward);

                //Debug.Log("AngleDiff: " + angleDiff);

                if (angleDiff > 0.1f)
                {
                    // CCW
                    _currentLocalRotation += _rotateSpeed * Time.deltaTime;
                }
                else if (angleDiff < -0.1f)
                {
                    // CW
                    _currentLocalRotation -= _rotateSpeed * Time.deltaTime;
                }

                if (angleDiff <= 2f && angleDiff >= -2f)
                {
                    if (Time.time - _lastShootTime >= _shootDelay)
                    {
                        _lastShootTime = Time.time;

                        Vector3 spawnPos = _barrelSprite.transform.position + barrelGlobalDir * 0.25f;
                        Bullet bullet = Instantiate(_bulletPrefab, spawnPos, Quaternion.identity, Game.Instance.BulletContainer);
                        bullet.Direction = barrelGlobalDir;
                    }
                }


                aimOnPlayer = true;
            }
            else
            {
                if (Time.time - _lastAimTime >= _waitAfterAim)
                {
                    if (_scanDirectionUp)
                    {
                        // CCW

                        _currentLocalRotation += _rotateSpeed * Time.deltaTime;

                        if (_currentLocalRotation > 90)
                        {
                            _currentLocalRotation = 90;
                            _scanDirectionUp = false;
                        }
                    }
                    else
                    {
                        // CW
                        _currentLocalRotation -= _rotateSpeed * Time.deltaTime;

                        if (_currentLocalRotation < -90)
                        {
                            _currentLocalRotation = -90;
                            _scanDirectionUp = true;
                        }
                    }
                }
            }

            _barrelSprite.transform.localRotation = Quaternion.AngleAxis(_currentLocalRotation, Vector3.forward);

#if UNITY_EDITOR
            {
                Vector3 barrelGlobalDir = _barrelSprite.transform.rotation * Vector3.up;
                Vector3 startPos = _barrelSprite.transform.position + barrelGlobalDir * 0.25f;
                Vector3 endPos = startPos + barrelGlobalDir * _viewDistance;

                if (aimOnPlayer)
                {
                    Debug.DrawLine(startPos, endPos, Color.red);
                }
                else
                {
                    Debug.DrawLine(startPos, endPos, Color.red);
                }
            }
#endif
        }

        #endregion MonoBehaviour

        #region Private Methods

        private bool CanAimOnPlayer(out Vector3 globalDir)
        {
            Player player = Game.Instance.CurrentPlayer;
            if (player == null)
            {
                globalDir = Vector3.zero;
                return false;
            }

            globalDir = player.transform.position - transform.position;
            if (globalDir.magnitude > _viewDistance)
            {
                return false;
            }

            Vector3 up = transform.rotation * Vector3.up;
            float angle = Vector3.Angle(up, globalDir);

            if (angle > 90)
            {
                return false;
            }


            int hits = Physics2D.RaycastNonAlloc(_barrelSprite.transform.position, globalDir, _raycastHits, _viewDistance + 1f, ~LayerMask.GetMask("Bullet"));
            if (hits == 0)
            {
                return false;
            }

            float playerDistance = float.PositiveInfinity;
            float otherDistance = float.PositiveInfinity;

            for (int i = 0; i < hits; ++i)
            {
                RaycastHit2D hit = _raycastHits[i];

                if (hit.collider.tag == "Player")
                {
                    if (hit.distance < playerDistance)
                    {
                        playerDistance = hit.distance;
                    }
                }
                else
                {
                    if (hit.distance < otherDistance)
                    {
                        otherDistance = hit.distance;
                    }
                }
            }

            if (playerDistance > _viewDistance ||
                otherDistance < playerDistance)
            {
                return false;
            }

            return true;
        }

        #endregion Private Methods
    }
}
