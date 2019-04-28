using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace zs.Assets.Scripts.Helpers
{
    [CreateAssetMenu(menuName = "Create SoundList", fileName = "SoundList", order = 1)]
    public class SoundList : ScriptableObject
    {
        public AudioClip Jump = null;
        public AudioClip Walk = null;
        public AudioClip Bump = null;
        public AudioClip Die = null;
        public AudioClip Button = null;
        public AudioClip TileDestroy = null;
        public AudioClip FallIntoVoid = null;
        public AudioClip JumpPad = null;
    }
}
