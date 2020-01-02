// Decompiled with JetBrains decompiler
// Type: AIProject.MiniGames.Fishing.FishLureSearcher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject.MiniGames.Fishing
{
  public class FishLureSearcher : MonoBehaviour
  {
    [SerializeField]
    private Fish fish;
    private CollisionState collisionState;
    private float reFindCounter;
    private const float MaxRange = 100f;

    public FishLureSearcher()
    {
      base.\u002Ector();
    }

    private FishingDefinePack.FishParamGroup FishParam
    {
      get
      {
        return Singleton<Resources>.Instance.FishingDefinePack.FishParam;
      }
    }

    private FishingDefinePack.FishHitParamGroup HitParam
    {
      get
      {
        return Singleton<Resources>.Instance.FishingDefinePack.FishParam.HitParam;
      }
    }

    public float FollowPercentage { get; set; }

    private void Awake()
    {
      if (!Object.op_Implicit((Object) this.fish))
        this.fish = (Fish) ((Component) this).GetComponent<Fish>();
      if (!Object.op_Implicit((Object) this.fish))
        this.fish = (Fish) ((Component) this).GetComponentInChildren<Fish>();
      if (Object.op_Implicit((Object) this.fish))
        return;
      Object.Destroy((Object) this);
    }

    public bool OnSearch
    {
      get
      {
        return !Object.op_Equality((Object) this.fish, (Object) null) && this.fish.OnSearch;
      }
    }

    private void OnEnable()
    {
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDisable<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ =>
      {
        if (this.OnSearch)
          this.OnUpdate();
        else
          this.collisionState = CollisionState.None;
      }));
    }

    public void ResetFollowPercentage()
    {
      this.FollowPercentage = 0.0f;
      FishFoodInfo selectedFishFood = MapUIContainer.FishingUI.SelectedFishFood;
      if (selectedFishFood == null)
        return;
      FishInfo fishInfo = this.fish.fishInfo;
      StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(fishInfo.CategoryID, fishInfo.ItemID);
      if (stuffItemInfo == null)
        return;
      Dictionary<int, Dictionary<int, float>> hitBaseRangeTable = Singleton<Resources>.Instance.Fishing.FishHitBaseRangeTable;
      int grade = (int) stuffItemInfo.Grade;
      Dictionary<int, float> dictionary;
      float num;
      if (!hitBaseRangeTable.TryGetValue(fishInfo.SizeID, out dictionary) || !dictionary.TryGetValue(grade, out num))
        return;
      float element = (float) selectedFishFood.RarelityHitRange.GetElement<int>(grade);
      this.FollowPercentage = Mathf.Clamp(num + element, 0.0f, 100f);
    }

    private float RandomPercentage
    {
      get
      {
        return Random.Range(0.0f, 100f);
      }
    }

    private bool FollowEnter
    {
      get
      {
        return (double) this.RandomPercentage <= (double) this.FollowPercentage;
      }
    }

    private void ChangeFollowState()
    {
      this.collisionState = CollisionState.None;
      this.reFindCounter = 0.0f;
      this.fish.ChangeState(Fish.State.FollowLure);
    }

    private void OnUpdate()
    {
      float num = this.collisionState == CollisionState.Enter || this.collisionState == CollisionState.Stay ? 1.05f : 1f;
      float distanceFishEyeToLure = this.fish.GetWorldDistanceFishEyeToLure();
      bool area = this.fish.CheckLureInAngleFindArea();
      bool flag = (double) distanceFishEyeToLure <= (double) this.FishParam.FindDistance * (double) num;
      if (area && flag)
      {
        switch (this.collisionState)
        {
          case CollisionState.None:
          case CollisionState.Exit:
            if (this.FollowEnter)
            {
              this.ChangeFollowState();
              break;
            }
            this.collisionState = CollisionState.Enter;
            this.reFindCounter = 0.0f;
            break;
          case CollisionState.Enter:
          case CollisionState.Stay:
            this.collisionState = CollisionState.Stay;
            if (this.fish.state == Fish.State.FollowLure)
              break;
            if ((double) this.FishParam.ReFindTime <= (double) this.reFindCounter)
            {
              if (this.FollowEnter)
              {
                this.ChangeFollowState();
                break;
              }
              this.reFindCounter = 0.0f;
              break;
            }
            this.reFindCounter += Time.get_deltaTime();
            break;
        }
      }
      else
      {
        switch (this.collisionState)
        {
          case CollisionState.None:
          case CollisionState.Exit:
            this.collisionState = CollisionState.None;
            break;
          case CollisionState.Enter:
          case CollisionState.Stay:
            this.collisionState = CollisionState.Exit;
            this.reFindCounter = 0.0f;
            if (this.fish.state != Fish.State.FollowLure)
              break;
            this.fish.SetWaitOrSwim();
            break;
        }
      }
    }
  }
}
