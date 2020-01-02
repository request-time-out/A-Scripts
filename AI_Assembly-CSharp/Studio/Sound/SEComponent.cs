// Decompiled with JetBrains decompiler
// Type: Studio.Sound.SEComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using UnityEngine;

namespace Studio.Sound
{
  public class SEComponent : MonoBehaviour
  {
    [SerializeField]
    private AudioClip _clip;
    [SerializeField]
    private Manager.Sound.Type _soundType;
    [SerializeField]
    private bool _isLoop;
    [SerializeField]
    private SEComponent.RolloffType _type;
    [SerializeField]
    private Threshold _rolloffDistance;
    [SerializeField]
    [Range(0.0f, 1f)]
    private float _volume;
    private AudioSource _audioSource;

    public SEComponent()
    {
      base.\u002Ector();
    }

    public AudioClip Clip
    {
      get
      {
        return this._clip;
      }
      set
      {
        this._clip = value;
      }
    }

    public Manager.Sound.Type SoundType
    {
      get
      {
        return this._soundType;
      }
      set
      {
        this._soundType = value;
      }
    }

    public bool IsLoop
    {
      get
      {
        return this._isLoop;
      }
      set
      {
        this._isLoop = value;
        if (!Object.op_Inequality((Object) this._audioSource, (Object) null))
          return;
        this._audioSource.set_loop(value);
      }
    }

    public SEComponent.RolloffType DecayType
    {
      get
      {
        return this._type;
      }
      set
      {
        this._type = value;
        if (!Object.op_Inequality((Object) this._audioSource, (Object) null))
          return;
        if (this._type == SEComponent.RolloffType.線形)
          this._audioSource.set_rolloffMode((AudioRolloffMode) 1);
        else
          this._audioSource.set_rolloffMode((AudioRolloffMode) 0);
      }
    }

    public Threshold RolloffDistance
    {
      get
      {
        return this._rolloffDistance;
      }
      set
      {
        this._rolloffDistance = new Threshold(Mathf.Max(0.0f, value.min), value.max);
        if (!Object.op_Inequality((Object) this._audioSource, (Object) null))
          return;
        this._audioSource.set_minDistance(this._rolloffDistance.min);
        this._audioSource.set_maxDistance(this._rolloffDistance.max);
      }
    }

    public float Volume
    {
      get
      {
        return this._volume;
      }
      set
      {
        float num = Mathf.Max(0.0f, Mathf.Min(1f, value));
        this._volume = num;
        if (!Object.op_Inequality((Object) this._audioSource, (Object) null))
          return;
        this._audioSource.set_volume(num);
      }
    }

    private void OnEnable()
    {
      if (Object.op_Equality((Object) this._audioSource, (Object) null))
        this._audioSource = Singleton<Manager.Sound>.Instance.Play(this._soundType, this._clip, 0.0f);
      if (!this._audioSource.get_isPlaying())
        this._audioSource.Play();
      if (this._type == SEComponent.RolloffType.線形)
        this._audioSource.set_rolloffMode((AudioRolloffMode) 1);
      else
        this._audioSource.set_rolloffMode((AudioRolloffMode) 0);
      this._audioSource.set_loop(this._isLoop);
      this._audioSource.set_minDistance(this._rolloffDistance.min);
      this._audioSource.set_maxDistance(this._rolloffDistance.max);
      this._audioSource.set_volume(this._volume);
    }

    private void OnDisable()
    {
      if (!Object.op_Inequality((Object) this._audioSource, (Object) null) || !this._audioSource.get_isPlaying())
        return;
      this._audioSource.Stop();
    }

    private void Update()
    {
      if (Object.op_Equality((Object) this._audioSource, (Object) null))
        return;
      ((Component) this._audioSource).get_transform().set_position(((Component) this).get_transform().get_position());
    }

    public enum RolloffType
    {
      対数関数,
      線形,
    }
  }
}
