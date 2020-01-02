// Decompiled with JetBrains decompiler
// Type: AIProject.StoryPointEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UnityEngine;

namespace AIProject
{
  public class StoryPointEffect : MonoBehaviour
  {
    [SerializeField]
    private ParticleSystem _particle;
    [SerializeField]
    private ParticleSystemRenderer _particleRenderer;
    private bool _play;
    private int _groupID;
    private int _pointID;

    public StoryPointEffect()
    {
      base.\u002Ector();
    }

    public static bool Switch { get; set; }

    public bool IsActive
    {
      get
      {
        return Object.op_Inequality((Object) this._particle, (Object) null) && Object.op_Inequality((Object) this._particleRenderer, (Object) null);
      }
    }

    public bool IsPlaying
    {
      get
      {
        return this.IsActive && this._particle.get_isPlaying();
      }
    }

    public EventPoint Point { get; private set; }

    private void Awake()
    {
      if (Object.op_Equality((Object) this._particle, (Object) null))
        this._particle = (ParticleSystem) ((Component) this).GetComponentInChildren<ParticleSystem>(true);
      if (!Object.op_Equality((Object) this._particleRenderer, (Object) null))
        return;
      this._particleRenderer = this._particle != null ? (ParticleSystemRenderer) ((Component) this._particle).GetComponent<ParticleSystemRenderer>() : (ParticleSystemRenderer) null;
    }

    private void Start()
    {
      ObservableExtensions.Subscribe<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) this), (Action<M0>) (_ => this.OnUpdate()));
    }

    private void OnEnable()
    {
      this.Show();
    }

    private void OnDisable()
    {
      this.Hide();
    }

    private void OnUpdate()
    {
      this.Point = EventPoint.GetTargetPoint();
      if (Object.op_Inequality((Object) this.Point, (Object) null) && StoryPointEffect.Switch && ((Behaviour) this).get_isActiveAndEnabled())
      {
        if (this.DiffPoint(this.Point))
          this.SetPosition(this.Point);
        if (this._play)
          return;
        this.Play();
      }
      else
      {
        if (!this._play)
          return;
        this.Stop();
      }
    }

    public void Play()
    {
      this._play = true;
      if (Object.op_Equality((Object) this.Point, (Object) null) || Object.op_Equality((Object) this._particle, (Object) null))
        return;
      if (this.DiffPoint(this.Point))
        this.SetPosition(this.Point);
      this._particle.Play(true);
    }

    public bool DiffPoint(EventPoint point)
    {
      if (Object.op_Equality((Object) point, (Object) null))
        return false;
      return point.GroupID != this._groupID || point.PointID != this._pointID;
    }

    public void SetPosition(EventPoint point)
    {
      if (Object.op_Equality((Object) point, (Object) null))
      {
        this._groupID = this._pointID = -1;
      }
      else
      {
        this._groupID = point.GroupID;
        this._pointID = point.PointID;
        Transform labelPoint = point.LabelPoint;
        ((Component) this).get_transform().SetPositionAndRotation(labelPoint.get_position(), labelPoint.get_rotation());
      }
    }

    public void Stop()
    {
      this._play = false;
      if (this._particle == null)
        return;
      this._particle.Stop(true, (ParticleSystemStopBehavior) 1);
    }

    public void Show()
    {
      if (Object.op_Equality((Object) this._particleRenderer, (Object) null) || ((Renderer) this._particleRenderer).get_enabled())
        return;
      ((Renderer) this._particleRenderer).set_enabled(true);
    }

    public void Hide()
    {
      if (Object.op_Equality((Object) this._particleRenderer, (Object) null) || !((Renderer) this._particleRenderer).get_enabled())
        return;
      ((Renderer) this._particleRenderer).set_enabled(false);
    }

    public void FadeOutAndDestroy()
    {
      if (Object.op_Equality((Object) this._particle, (Object) null))
      {
        Object.Destroy((Object) ((Component) this).get_gameObject());
      }
      else
      {
        this.Stop();
        ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.SkipWhile<long>((IObservable<M0>) Observable.EveryUpdate(), (Func<M0, bool>) (_ => this._particle.IsAlive(true))), 1), (Action<M0>) (_ => Object.Destroy((Object) ((Component) this).get_gameObject())));
      }
    }
  }
}
