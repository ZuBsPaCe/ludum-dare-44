﻿using UnityEngine;
using zs.Logic;

namespace zs.Helpers
{
    public class CameraFollow : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField]
        private Transform _followTarget = null;

        #endregion Serializable Fields

        #region Private Vars

        private bool _improvedCharacterMovement;

        #endregion Private Vars

        #region Public Vars

        public Transform FollowTarget
        {
            get { return _followTarget; }
            set
            {
                _followTarget = value;
            }
        }

        #endregion Public Vars

        #region Public Methods
        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
        }

        void Start()
        {
            if (PlayerPrefs.HasKey("ImprovedCharacterMovement"))
            {
                _improvedCharacterMovement = PlayerPrefs.GetInt("ImprovedCharacterMovement") == 1;
            }
        }
	
        void Update()
        {
            if (!_improvedCharacterMovement)
            {
                if (_followTarget != null)
                {
                    transform.position = Vector3.Lerp(transform.position, _followTarget.transform.position.with_z(transform.position.z), 0.1f);

                    const float maxDistance = 4;
                    Vector3 targetDir = _followTarget.position.with_z(0) - transform.position.with_z(0);
                    if (targetDir.sqrMagnitude > maxDistance * maxDistance)
                    {
                        transform.position = (_followTarget.position - targetDir.with_z(0).normalized * maxDistance).with_z(transform.position.z);
                    }
                }
            }
        }

        void LateUpdate()
        {
            if (_improvedCharacterMovement)
            {
                if (_followTarget != null)
                {
                    transform.position = _followTarget.transform.position.with_z(transform.position.z);
                }
            }
        }

        #endregion MonoBehaviour

        #region Private Methods
        #endregion Private Methods
    }
}
