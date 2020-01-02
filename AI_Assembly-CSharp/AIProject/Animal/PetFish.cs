// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.PetFish
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.MiniGames.Fishing;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject.Animal
{
  public class PetFish : AnimalBase, IPetAnimal, INicknameObject
  {
    [SerializeField]
    private float _nicknameHeightOffset = 0.5f;
    protected Transform _nicknameRoot;
    private bool _nicknameEnabled;
    protected CommandLabel.CommandInfo[] giveFoodLabels;

    public AIProject.SaveData.AnimalData AnimalData { get; set; }

    public string Nickname
    {
      get
      {
        return this.AnimalData?.Nickname ?? this.Name;
      }
      set
      {
        if (this.AnimalData != null)
          this.AnimalData.Nickname = value;
        Action changeNickNameEvent = this.ChangeNickNameEvent;
        if (changeNickNameEvent == null)
          return;
        changeNickNameEvent();
      }
    }

    public PetHomePoint HomePoint { get; private set; }

    public override ItemIDKeyPair ItemID
    {
      get
      {
        if (this.AnimalData == null)
          return base.ItemID;
        return new ItemIDKeyPair()
        {
          categoryID = this.AnimalData.ItemCategoryID,
          itemID = this.AnimalData.ItemID
        };
      }
    }

    public override bool WaitPossible
    {
      get
      {
        return false;
      }
    }

    public Transform NicknameRoot
    {
      get
      {
        return this._nicknameRoot;
      }
    }

    public bool NicknameEnabled
    {
      get
      {
        return this._nicknameEnabled && this.BodyEnabled;
      }
      set
      {
        this._nicknameEnabled = value;
      }
    }

    public Action ChangeNickNameEvent { get; set; }

    protected override void Awake()
    {
      base.Awake();
    }

    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (Object.op_Inequality((Object) this.FollowTarget, (Object) null))
      {
        this.Position = this.FollowTarget.get_position();
        this.Rotation = this.FollowTarget.get_rotation();
      }
      if (this.AnimalData == null)
        return;
      this.AnimalData.Position = this.Position;
      this.AnimalData.Rotation = this.Rotation;
    }

    public void Initialize(PetHomePoint _homePoint)
    {
      this.Clear();
      PetHomePoint petHomePoint = _homePoint;
      this.HomePoint = petHomePoint;
      if (Object.op_Equality((Object) petHomePoint, (Object) null) || this.AnimalData == null)
        return;
      Resources.FishingTable fishingTable = !Singleton<Resources>.IsInstance() ? (Resources.FishingTable) null : Singleton<Resources>.Instance.Fishing;
      if (fishingTable == null)
        return;
      Dictionary<int, Dictionary<int, FishInfo>> fishInfoTable = fishingTable.FishInfoTable;
      Dictionary<int, Tuple<GameObject, RuntimeAnimatorController>> fishModelTable = fishingTable.FishModelTable;
      Dictionary<int, FishInfo> dictionary;
      FishInfo fishInfo;
      Tuple<GameObject, RuntimeAnimatorController> tuple;
      if (!((IReadOnlyDictionary<int, Dictionary<int, FishInfo>>) fishInfoTable).IsNullOrEmpty<int, Dictionary<int, FishInfo>>() && !((IReadOnlyDictionary<int, Tuple<GameObject, RuntimeAnimatorController>>) fishModelTable).IsNullOrEmpty<int, Tuple<GameObject, RuntimeAnimatorController>>() && (fishInfoTable.TryGetValue(this.AnimalData.ItemCategoryID, out dictionary) && !((IReadOnlyDictionary<int, FishInfo>) dictionary).IsNullOrEmpty<int, FishInfo>()) && (dictionary.TryGetValue(this.AnimalData.ItemID, out fishInfo) && fishModelTable.TryGetValue(fishInfo.ModelID, out tuple)))
      {
        this.LoadBody(tuple.Item1);
        if (Object.op_Inequality((Object) this.animator, (Object) null))
          this.animator.set_runtimeAnimatorController(tuple.Item2);
        _homePoint.SetRootPoint(fishInfo.TankPointID, (IPetAnimal) this);
        this.FollowTarget = _homePoint.GetRootPoint(fishInfo.TankPointID);
        if (Object.op_Inequality((Object) this.bodyObject, (Object) null))
        {
          this._nicknameRoot = new GameObject("Nickname Root").get_transform();
          this._nicknameRoot.SetParent(this.bodyObject.get_transform(), false);
          Renderer componentInChildren = (Renderer) this.bodyObject.GetComponentInChildren<Renderer>(true);
          if (Object.op_Inequality((Object) componentInChildren, (Object) null))
          {
            Vector3 position = this.Position;
            Quaternion rotation = this.Rotation;
            this.Position = Vector3.get_zero();
            this.Rotation = Quaternion.get_identity();
            Bounds bounds1 = componentInChildren.get_bounds();
            Vector3 center = ((Bounds) ref bounds1).get_center();
            Bounds bounds2 = componentInChildren.get_bounds();
            this._nicknameRoot.set_localPosition(new Vector3(0.0f, (float) (((Bounds) ref bounds2).get_extents().y / 2.0 + center.y) + this._nicknameHeightOffset, (float) center.z));
            this.Position = position;
            this.Rotation = rotation;
          }
          else
            this._nicknameRoot.set_localPosition(new Vector3(0.0f, this._nicknameHeightOffset, 0.0f));
        }
      }
      if (Object.op_Equality((Object) this._nicknameRoot, (Object) null))
      {
        this._nicknameRoot = new GameObject("Nickname Root").get_transform();
        this._nicknameRoot.SetParent(((Component) this).get_transform(), false);
        this._nicknameRoot.set_localPosition(new Vector3(0.0f, this._nicknameHeightOffset, 0.0f));
      }
      if (Object.op_Equality((Object) this.FollowTarget, (Object) null))
        this.FollowTarget = ((Component) this.HomePoint).get_transform();
      this.SetStateData();
      bool flag = false;
      this.MarkerEnabled = flag;
      this.BodyEnabled = flag;
      this.SetState(AnimalState.Start, (Action) null);
    }

    public void Initialize(AIProject.SaveData.AnimalData _animalData)
    {
      AIProject.SaveData.AnimalData animalData = _animalData;
      this.AnimalData = animalData;
      if (animalData != null)
        return;
      this.SetState(AnimalState.Destroyed, (Action) null);
    }

    public void Release()
    {
      this.SetState(AnimalState.Destroyed, (Action) null);
    }

    protected override void OnDestroy()
    {
      this.Active = false;
      base.OnDestroy();
    }

    public override bool IsNeutralCommand
    {
      get
      {
        return false;
      }
    }

    public override CommandLabel.CommandInfo[] Labels
    {
      get
      {
        return AnimalBase.emptyLabels;
      }
    }

    protected override void EnterStart()
    {
      this.ChangeState(AnimalState.Idle, (Action) null);
    }

    protected override void ExitStart()
    {
      bool flag = true;
      this.MarkerEnabled = flag;
      this.BodyEnabled = flag;
      this.Active = true;
    }

    protected override void EnterIdle()
    {
    }

    protected override void OnIdle()
    {
    }

    protected override void ExitIdle()
    {
    }

    protected override void AnimationIdle()
    {
    }

    protected override void EnterSwim()
    {
    }

    protected override void OnSwim()
    {
    }

    protected override void ExitSwim()
    {
    }

    protected override void AnimationSwim()
    {
    }

    protected override void EnterEat()
    {
    }

    protected override void OnEat()
    {
    }

    protected override void AnimationEat()
    {
    }

    protected override void ExitEat()
    {
    }
  }
}
