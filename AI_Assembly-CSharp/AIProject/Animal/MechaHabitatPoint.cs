// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.MechaHabitatPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using UniRx;
using UnityEngine;

namespace AIProject.Animal
{
  public class MechaHabitatPoint : AnimalPoint
  {
    [SerializeField]
    private Vector2 _coolTimeRange = Vector2.get_zero();
    private WildMecha _user;

    public WildMecha User
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

    public Vector2 CoolTimeRange
    {
      get
      {
        return this._coolTimeRange;
      }
    }

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
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) this), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.UpdateCoolTime()));
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

    public bool InUsed
    {
      get
      {
        return Object.op_Inequality((Object) this._user, (Object) null);
      }
    }

    public bool IsCountStop { get; set; } = true;

    public bool IsCountCoolTime { get; set; }

    public bool IsActive { get; set; }

    public float CoolTimeCounter { get; private set; }

    public bool ForcedAdd { get; set; }

    public Func<MechaHabitatPoint, bool> AddCheck { get; set; }

    public Func<MechaHabitatPoint, WildMecha> AddAnimalAction { get; set; }

    public void SetCoolTime(float coolTime)
    {
      this.IsCountCoolTime = true;
      this.CoolTimeCounter = coolTime;
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

    public bool SetUse(WildMecha _mecha)
    {
      if (Object.op_Equality((Object) _mecha, (Object) null) || Object.op_Inequality((Object) this._user, (Object) null) && Object.op_Inequality((Object) this._user, (Object) _mecha))
        return false;
      this._user = _mecha;
      return true;
    }

    public bool StopUse(WildMecha _mecha)
    {
      if (Object.op_Equality((Object) _mecha, (Object) null) || Object.op_Equality((Object) this._user, (Object) null) || Object.op_Inequality((Object) this._user, (Object) _mecha))
        return false;
      this._user = (WildMecha) null;
      this.SetCoolTime();
      return true;
    }
  }
}
