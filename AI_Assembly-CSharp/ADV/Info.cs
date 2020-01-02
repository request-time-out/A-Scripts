// Decompiled with JetBrains decompiler
// Type: ADV.Info
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace ADV
{
  [Serializable]
  public class Info
  {
    public Info.Audio audio = new Info.Audio();
    public Info.Anime anime = new Info.Anime();

    [Serializable]
    public class Audio
    {
      public Info.Audio.Eco eco = new Info.Audio.Eco();
      public bool is2D;
      public bool isNotMoveMouth;

      [Serializable]
      public class Eco
      {
        [SerializeField]
        [Range(10f, 5000f)]
        private float _delay = 50f;
        [SerializeField]
        [Range(0.0f, 1f)]
        private float _decayRatio = 0.5f;
        [SerializeField]
        [Range(0.0f, 1f)]
        private float _wetMix = 1f;
        [SerializeField]
        [Range(0.0f, 1f)]
        private float _dryMix = 1f;
        [SerializeField]
        private bool _use;

        public bool use
        {
          get
          {
            return this._use;
          }
          set
          {
            this._use = value;
          }
        }

        public float delay
        {
          get
          {
            return this._delay;
          }
          set
          {
            this._delay = value;
          }
        }

        public float decayRatio
        {
          get
          {
            return this._decayRatio;
          }
          set
          {
            this._decayRatio = value;
          }
        }

        public float wetMix
        {
          get
          {
            return this._wetMix;
          }
          set
          {
            this._wetMix = value;
          }
        }

        public float dryMix
        {
          get
          {
            return this._dryMix;
          }
          set
          {
            this._dryMix = value;
          }
        }
      }
    }

    [Serializable]
    public class Anime
    {
      public Info.Anime.Play play = new Info.Anime.Play();

      [Serializable]
      public class Play
      {
        [Header("Effect")]
        [SerializeField]
        [Range(0.0f, 10f)]
        private float _crossFadeTime = 0.8f;
        [SerializeField]
        [Range(0.001f, 3f)]
        private float _transitionDuration = 0.3f;
        [Header("Animation")]
        [SerializeField]
        private bool _isCrossFade;
        [SerializeField]
        private int _layerNo;
        [SerializeField]
        [Range(0.0f, 1f)]
        private float _normalizedTime;

        public float crossFadeTime
        {
          get
          {
            return this._crossFadeTime;
          }
          set
          {
            this._crossFadeTime = value;
          }
        }

        public bool isCrossFade
        {
          get
          {
            return this._isCrossFade;
          }
          set
          {
            this._isCrossFade = value;
          }
        }

        public int layerNo
        {
          get
          {
            return this._layerNo;
          }
          set
          {
            this._layerNo = value;
          }
        }

        public float transitionDuration
        {
          get
          {
            return this._transitionDuration;
          }
          set
          {
            this._transitionDuration = value;
          }
        }

        public float normalizedTime
        {
          get
          {
            return this._normalizedTime;
          }
          set
          {
            this._normalizedTime = value;
          }
        }
      }
    }
  }
}
