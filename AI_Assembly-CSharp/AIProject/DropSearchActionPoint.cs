// Decompiled with JetBrains decompiler
// Type: AIProject.DropSearchActionPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject
{
  public class DropSearchActionPoint : SearchActionPoint
  {
    [SerializeField]
    private int _mapItemID = -1;
    [SerializeField]
    private float _setCoolTime = 2400f;
    private float _coolTime;
    private List<GameObject> _mapItems;
    private bool _isCoolTime;
    private IDisposable _updateDisposable;

    public int MapItemID
    {
      get
      {
        return this._mapItemID;
      }
    }

    public float CoolTime
    {
      get
      {
        return this._setCoolTime;
      }
    }

    public float CurrentCoolTime
    {
      get
      {
        return this._coolTime;
      }
    }

    public bool HaveMapItems
    {
      get
      {
        return !this._mapItems.IsNullOrEmpty<GameObject>();
      }
    }

    public bool IsCoolTime
    {
      get
      {
        return this._isCoolTime;
      }
    }

    private void Awake()
    {
      this._mapItems = MapItemData.Get(this._mapItemID);
    }

    protected override void Start()
    {
      base.Start();
      if (Singleton<Game>.IsInstance())
      {
        Dictionary<int, float> pointCoolTimeTable = Singleton<Game>.Instance.Environment?.DropSearchActionPointCoolTimeTable;
        float num;
        if (!pointCoolTimeTable.IsNullOrEmpty<int, float>() && pointCoolTimeTable.TryGetValue(this.RegisterID, out num))
        {
          if ((double) num <= 0.0)
          {
            pointCoolTimeTable.Remove(this.RegisterID);
          }
          else
          {
            this._coolTime = num;
            this._isCoolTime = true;
            if (((Component) this).get_gameObject().get_activeSelf())
              ((Component) this).get_gameObject().SetActive(false);
          }
        }
      }
      if (this._updateDisposable != null)
        this._updateDisposable.Dispose();
      this._updateDisposable = ObservableExtensions.Subscribe<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Component) this), (Action<M0>) (_ => this.OnUpdate()));
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      if (this._mapItems.IsNullOrEmpty<GameObject>())
        return;
      using (List<GameObject>.Enumerator enumerator = this._mapItems.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          GameObject current = enumerator.Current;
          if (Object.op_Inequality((Object) current, (Object) null) && !current.get_activeSelf())
            current.SetActive(true);
        }
      }
    }

    protected override void OnDisable()
    {
      if (!this._mapItems.IsNullOrEmpty<GameObject>())
      {
        using (List<GameObject>.Enumerator enumerator = this._mapItems.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            GameObject current = enumerator.Current;
            if (Object.op_Inequality((Object) current, (Object) null) && current.get_activeSelf())
              current.SetActive(false);
          }
        }
      }
      base.OnDisable();
    }

    private void OnDestroy()
    {
      if (this._updateDisposable == null)
        return;
      this._updateDisposable.Dispose();
    }

    protected override void InitSub()
    {
      base.InitSub();
      if (this._labels.IsNullOrEmpty<CommandLabel.CommandInfo>())
        return;
      foreach (CommandLabel.CommandInfo label in this._labels)
      {
        if (label != null)
          label.CoolTimeFillRate = (Func<float>) null;
      }
    }

    private void OnUpdate()
    {
      if (!this._isCoolTime || Mathf.Approximately(Time.get_timeScale(), 0.0f) || !Singleton<Manager.Map>.IsInstance())
        return;
      EnvironmentSimulator simulator = Singleton<Manager.Map>.Instance.Simulator;
      if (Object.op_Equality((Object) simulator, (Object) null) || !simulator.EnabledTimeProgression || !Singleton<Game>.IsInstance())
        return;
      Dictionary<int, float> pointCoolTimeTable = Singleton<Game>.Instance.Environment?.DropSearchActionPointCoolTimeTable;
      if (pointCoolTimeTable == null)
        return;
      float num1;
      if (!pointCoolTimeTable.TryGetValue(this.RegisterID, out num1))
      {
        this.SetAvailable();
      }
      else
      {
        float unscaledDeltaTime = Time.get_unscaledDeltaTime();
        this._coolTime = Mathf.Max(num1 - unscaledDeltaTime, -1f);
        pointCoolTimeTable[this.RegisterID] = this._coolTime;
        if ((double) this._coolTime > 0.0)
          return;
        Camera cameraComponent = Manager.Map.GetCameraComponent();
        if (Object.op_Equality((Object) cameraComponent, (Object) null) || !Singleton<Resources>.IsInstance())
          return;
        LocomotionProfile.DropSearchActionPointSettings actionPointSetting = Singleton<Resources>.Instance.LocomotionProfile.DropSearchActionPointSetting;
        Transform transform = ((Component) cameraComponent).get_transform();
        float num2 = Vector3.Angle(transform.get_forward(), Vector3.op_Subtraction(((Component) this).get_transform().get_position(), transform.get_position()));
        float num3 = Vector3.Distance(transform.get_position(), ((Component) this).get_transform().get_position());
        if ((double) actionPointSetting.AvailableAngle >= (double) num2 && (double) actionPointSetting.AvailableDistance >= (double) num3)
          return;
        this._isCoolTime = false;
        this._coolTime = 0.0f;
        pointCoolTimeTable.Remove(this.RegisterID);
        ((Component) this).get_gameObject().SetActive(true);
      }
    }

    public void SetAvailable()
    {
      this._isCoolTime = false;
      this._coolTime = 0.0f;
      if (Singleton<Game>.IsInstance())
      {
        Dictionary<int, float> pointCoolTimeTable = Singleton<Game>.Instance.Environment?.DropSearchActionPointCoolTimeTable;
        if (!pointCoolTimeTable.IsNullOrEmpty<int, float>())
          pointCoolTimeTable.Remove(this.RegisterID);
      }
      if (((Component) this).get_gameObject().get_activeSelf())
        return;
      ((Component) this).get_gameObject().SetActive(true);
    }

    public void SetCoolTime()
    {
      this._isCoolTime = true;
      this._coolTime = this._setCoolTime;
      if (Singleton<Game>.IsInstance())
      {
        Dictionary<int, float> pointCoolTimeTable = Singleton<Game>.Instance.Environment?.DropSearchActionPointCoolTimeTable;
        if (pointCoolTimeTable != null)
          pointCoolTimeTable[this.RegisterID] = this._coolTime = this._setCoolTime;
      }
      if (!((Component) this).get_gameObject().get_activeSelf())
        return;
      ((Component) this).get_gameObject().SetActive(false);
    }

    public override bool Entered(
      Vector3 basePosition,
      float distance,
      float radiusA,
      float radiusB,
      float angle,
      Vector3 forward)
    {
      return ((Behaviour) this).get_isActiveAndEnabled() && !this._isCoolTime && base.Entered(basePosition, distance, radiusA, radiusB, angle, forward);
    }
  }
}
