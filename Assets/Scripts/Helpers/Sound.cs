using System.Collections.Generic;
using UnityEngine;
using zs.Assets.Scripts.Helpers;

namespace zs.Helpers
{
    public class Sound : MonoBehaviour
    {
        #region Serializable Fields
        #endregion Serializable Fields

        #region Private Vars

        private SoundList _sounds = null;

        private float _walkTargetVolume;
        private AudioSource _walkAudioSource = null;

        #endregion Private Vars

        #region Public Vars
        #endregion Public Vars

        #region Public Methods

        public static Sound Instance { get; private set; }

        public void PlayWalk(AudioSource source, float volume)
        {
            if (!source.isPlaying)
            {
                _walkAudioSource = source;

                source.volume = 0;
                source.loop = true;
                source.Play();
            }

            _walkTargetVolume = volume;
        }

        public void StopWalk(AudioSource source)
        {
            source.Stop();
        }

        public void PlayJump(AudioSource source)
        {
            source.PlayOneShot(_sounds.Jump);
        }

        public void PlayBump(AudioSource source)
        {
            source.PlayOneShot(_sounds.Bump);
        }

        public void PlayDie(AudioSource source)
        {
            source.PlayOneShot(_sounds.Die);
        }

        public void PlayButton(AudioSource source)
        {
            source.PlayOneShot(_sounds.Button);
        }

        public void PlayTileDestroy(AudioSource source)
        {
            source.PlayOneShot(_sounds.TileDestroy);
        }
        
        public void PlayFallIntoVoid(AudioSource source)
        {
            source.PlayOneShot(_sounds.FallIntoVoid);
        }

        public void PlayJumpPad(AudioSource source)
        {
            source.PlayOneShot(_sounds.JumpPad);
        }

        #endregion Public Methods

        #region MonoBehaviour
	
        void Awake()
        {
            Instance = this;

            _sounds = Resources.Load<SoundList>("SoundList");
            Debug.Assert(_sounds);
        }

        void Start()
        {
        }
	
        void Update()
        {
            if (_walkAudioSource != null)
            {
                if (_walkAudioSource.volume < _walkTargetVolume)
                {
                    float volume = _walkAudioSource.volume;
                    volume += 16 * Time.deltaTime;

                    if (volume >= _walkTargetVolume)
                    {
                        _walkAudioSource.volume = _walkTargetVolume;
                    }
                    else
                    {
                        _walkAudioSource.volume = volume;
                    }
                }
                else if (_walkAudioSource.volume > _walkTargetVolume)
                {
                    float volume = _walkAudioSource.volume;
                    volume -= 8 * Time.deltaTime;

                    if (volume <= _walkTargetVolume)
                    {
                        _walkAudioSource.volume = _walkTargetVolume;
                    }
                    else
                    {
                        _walkAudioSource.volume = volume;
                    }
                }
            }
        }

        #endregion MonoBehaviour

        #region Private Methods
        #endregion Private Methods
    }
}
