// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.FrogHabitatPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using UniRx;
using UnityEngine;

namespace AIProject.Animal
{
  public class FrogHabitatPoint : AnimalPoint
  {
    [SerializeField]
    private int _itemID = -1;
    [SerializeField]
    private float _moveRadius = 10f;
    private WildFrog _user;

    public int ItemID
    {
      get
      {
        return this._itemID;
      }
    }

    public WildFrog User
    {
      get
      {
        return this._user;
      }
      set
      {
        this._user = value;
      }
    }

    public float MoveRadius
    {
      get
      {
        return this._moveRadius;
      }
    }

    public Vector2 CoolTimeRange { get; set; } = new Vector2(240f, 300f);

    public bool InUsed
    {
      get
      {
        return Object.op_Inequality((Object) this.User, (Object) null);
      }
    }

    public bool IsCountStop { get; set; } = true;

    public bool IsCountCoolTime { get; set; }

    public bool IsActive { get; set; }

    public float CoolTimeCounter { get; private set; }

    public bool ForcedAdd { get; set; }

    public Func<FrogHabitatPoint, bool> AddCheck { get; set; }

    public Func<FrogHabitatPoint, WildFrog> AddAnimalAction { get; set; }

    protected override void Start()
    {
      base.Start();
      switch (this.LocateType)
      {
        case LocateTypes.Collider:
          this.LocateOnCollider();
          break;
        case LocateTypes.NavMesh:
          this.LocateOnNavMesh();
          break;
      }
      ObservableExtensions.Subscribe<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) this), (Action<M0>) (_ => this.UpdateCoolTime()));
    }

    [ContextMenu("Locate Ground On Collider")]
    private void LocateOnCollider()
    {
      AnimalPoint.RelocationOnCollider(((Component) this).get_transform(), 3f, !Singleton<Resources>.IsInstance() ? LayerMask.op_Implicit(LayerMask.NameToLayer("MapArea")) : Singleton<Resources>.Instance.DefinePack.MapDefines.MapLayer);
    }

    [ContextMenu("Locate Ground On NavMesh")]
    private void LocateOnNavMesh()
    {
      AnimalPoint.RelocationOnNavMesh(((Component) this).get_transform(), 10f);
    }

    public void SetCoolTime(float _coolTime)
    {
      this.IsCountCoolTime = true;
      this.CoolTimeCounter = _coolTime;
    }

    public void SetCoolTime()
    {
      this.IsCountCoolTime = true;
      this.CoolTimeCounter = this.CoolTimeRange.RandomRange();
    }

    private void UpdateCoolTime()
    {
      if (this.IsCountStop || Object.op_Inequality((Object) this._user, (Object) null) || (!this.IsActive || !this.IsCountCoolTime) || Mathf.Approximately(0.0f, Time.get_timeScale()))
        return;
      this.CoolTimeCounter -= Time.get_unscaledDeltaTime();
      if ((double) this.CoolTimeCounter > 0.0)
        return;
      this.CoolTimeCounter = 0.0f;
      if (this.AddCheck == null || this.AddAnimalAction == null || !this.AddCheck(this))
        return;
      this._user = this.AddAnimalAction(this);
      this.IsCountCoolTime = Object.op_Equality((Object) this._user, (Object) null);
    }

    public bool SetUse(WildFrog _frog)
    {
      if (Object.op_Equality((Object) _frog, (Object) null) || Object.op_Inequality((Object) this._user, (Object) null) && Object.op_Inequality((Object) this._user, (Object) _frog))
        return false;
      this._user = _frog;
      return true;
    }

    public bool StopUse(WildFrog _frog)
    {
      if (Object.op_Equality((Object) _frog, (Object) null) || Object.op_Equality((Object) this._user, (Object) null) || Object.op_Inequality((Object) this._user, (Object) _frog))
        return false;
      this._user = (WildFrog) null;
      this.SetCoolTime();
      return true;
    }
  }
}
