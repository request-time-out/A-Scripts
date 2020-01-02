// Decompiled with JetBrains decompiler
// Type: AIProject.CheckEventPointArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ADV;
using AIProject.Definitions;
using AIProject.Player;
using AIProject.SaveData;
using Cinemachine;
using IllusionUtility.GetUtility;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  public class CheckEventPointArea : MonoBehaviour
  {
    private EventPoint _eventPoint;
    private CollisionState _collisionState;

    public CheckEventPointArea()
    {
      base.\u002Ector();
    }

    public EventPoint EventPoint
    {
      get
      {
        return this._eventPoint;
      }
      set
      {
        this._eventPoint = value;
      }
    }

    public CollisionState CollisionState
    {
      get
      {
        return this._collisionState;
      }
    }

    private OpenData openData { get; }

    private CheckEventPointArea.PackData packData { get; set; }

    private void Start()
    {
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) this), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (System.Action<M0>) (_ => this.OnUpdate()));
    }

    private void OnUpdate()
    {
      if (Object.op_Equality((Object) this._eventPoint, (Object) null) || !Singleton<Manager.Map>.IsInstance())
        return;
      PlayerActor player = Manager.Map.GetPlayer();
      CommandArea commandArea = Manager.Map.GetCommandArea(player);
      if (Object.op_Equality((Object) commandArea, (Object) null))
        return;
      bool flag = this._eventPoint.IsNeutralCommand;
      if (flag)
      {
        IState state = player.PlayerController.State;
        int num1 = flag ? 1 : 0;
        int num2;
        switch (state)
        {
          case Normal _:
          case Houchi _:
            num2 = 1;
            break;
          default:
            num2 = state is Onbu ? 1 : 0;
            break;
        }
        flag = (num1 & num2) != 0;
      }
      if (flag)
      {
        float num1 = !this._eventPoint.EnableRangeCheck ? commandArea.AllAroundRadius : this._eventPoint.CheckRadius;
        Vector3 commandCenter = this._eventPoint.CommandCenter;
        Vector3 p1 = Vector3.op_Addition(player.Position, Quaternion.op_Multiply(player.Rotation, commandArea.Offset));
        flag &= (double) this.DistanceNonHeight(commandCenter, p1) <= (double) num1;
        if (flag)
        {
          float num2 = Mathf.Abs((float) (commandCenter.y - p1.y));
          flag &= (double) num2 < (double) commandArea.Height;
        }
        if (flag)
        {
          NavMeshHit navMeshHit;
          flag = ((flag ? 1 : 0) & (!Object.op_Inequality((Object) player.NavMeshAgent, (Object) null) || !((Behaviour) player.NavMeshAgent).get_isActiveAndEnabled() ? 0 : (!player.NavMeshAgent.Raycast(commandCenter, ref navMeshHit) ? 1 : 0))) != 0;
        }
      }
      if (flag)
      {
        switch (this._collisionState)
        {
          case CollisionState.None:
          case CollisionState.Exit:
            this._collisionState = CollisionState.Enter;
            this.OnHitEnter();
            this.OnHitStay();
            break;
          case CollisionState.Enter:
          case CollisionState.Stay:
            this._collisionState = CollisionState.Stay;
            this.OnHitStay();
            break;
        }
      }
      else
      {
        switch (this._collisionState)
        {
          case CollisionState.None:
          case CollisionState.Exit:
            this._collisionState = CollisionState.None;
            break;
          case CollisionState.Enter:
          case CollisionState.Stay:
            this._collisionState = CollisionState.Exit;
            this.OnHitExit();
            break;
        }
      }
    }

    private float DistanceNonHeight(Vector3 p0, Vector3 p1)
    {
      p0.y = (__Null) (double) (p1.y = (__Null) 0.0f);
      return Vector3.Distance(p0, p1);
    }

    private void OnHitEnter()
    {
      if (this._eventPoint.GroupID != 2)
        return;
      switch (this._eventPoint.PointID)
      {
        case 0:
          if (Manager.Map.TutorialMode)
          {
            this.StartMerchantStory0();
            break;
          }
          if (this._eventPoint.GetDedicatedNumber() != 0)
            break;
          this._eventPoint.SetDedicatedNumber(1);
          break;
        case 1:
          this._eventPoint.StartMerchantADV(4);
          break;
      }
    }

    private void OnHitStay()
    {
    }

    private void OnHitExit()
    {
    }

    private void EndTutorial()
    {
    }

    private void StartMerchantStory0()
    {
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      Manager.Map instance = Singleton<Manager.Map>.Instance;
      this.openData.FindLoad("0", -90, 1);
      this.packData = new CheckEventPointArea.PackData();
      this.packData.Init();
      this.packData.SetParam((IParams) instance.Merchant.MerchantData, (IParams) instance.Player.PlayerData);
      this.packData.onComplete = (System.Action) (() =>
      {
        this.EndMerchantStory0();
        this.packData.Release();
        this.packData = (CheckEventPointArea.PackData) null;
      });
      instance.Player.PlayerController.ChangeState("Idle");
      instance.Merchant.ChangeBehavior(Merchant.ActionType.TalkWithPlayer);
      MapUIContainer.SetVisibleHUD(false);
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.FadeCanvas.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 0, 2f, true), (System.Action<M0>) (_ => {}), (System.Action) (() =>
      {
        if (!Singleton<Manager.Map>.IsInstance())
          return;
        MapUIContainer.SetVisibleHUD(false);
        PlayerActor player = Singleton<Manager.Map>.Instance.Player;
        MerchantActor merchant = Singleton<Manager.Map>.Instance.Merchant;
        StoryPoint storyPoint = Manager.Map.GetStoryPoint(3);
        Transform transform1 = !Object.op_Inequality((Object) storyPoint, (Object) null) ? (Transform) null : ((Component) storyPoint).get_transform();
        if (Object.op_Inequality((Object) transform1, (Object) null))
        {
          GameObject loop = transform1.FindLoop("player_point");
          if (Object.op_Inequality((Object) loop, (Object) null))
            transform1 = loop.get_transform();
        }
        if (Object.op_Equality((Object) transform1, (Object) null))
          transform1 = Singleton<Manager.Map>.Instance.PlayerStartPoint;
        Transform transform2 = transform1;
        if (Object.op_Inequality((Object) transform2, (Object) null))
        {
          if (((Behaviour) player.NavMeshAgent).get_enabled())
            player.NavMeshAgent.Warp(transform2.get_position());
          else
            player.Position = transform2.get_position();
          player.Rotation = transform2.get_rotation();
        }
        AgentActor tutorialAgent = Singleton<Manager.Map>.Instance.TutorialAgent;
        if (Object.op_Inequality((Object) tutorialAgent, (Object) null))
        {
          if (((Behaviour) tutorialAgent.NavMeshAgent).get_enabled())
            tutorialAgent.NavMeshAgent.Warp(storyPoint.Position);
          else
            tutorialAgent.Position = storyPoint.Position;
          tutorialAgent.Rotation = storyPoint.Rotation;
          if (tutorialAgent.TutorialType != AIProject.Definitions.Tutorial.ActionType.WaitAtAgit)
          {
            tutorialAgent.TargetStoryPoint = storyPoint;
            tutorialAgent.ChangeTutorialBehavior(AIProject.Definitions.Tutorial.ActionType.WaitAtAgit);
          }
        }
        Transform transform3 = ((Component) merchant.Locomotor).get_transform();
        transform3.LookAt(player.Position);
        Vector3 eulerAngles = transform3.get_eulerAngles();
        eulerAngles.x = (__Null) (double) (eulerAngles.z = (__Null) 0.0f);
        transform3.set_eulerAngles(eulerAngles);
        player.CommCompanion = (Actor) merchant;
        player.PlayerController.ChangeState("Communication");
        if (player.CameraControl.ShotType == ShotType.PointOfView)
        {
          ActorCameraControl cameraControl = player.CameraControl;
          Quaternion rotation = player.Rotation;
          // ISSUE: variable of the null type
          __Null y = ((Quaternion) ref rotation).get_eulerAngles().y;
          cameraControl.XAxisValue = (float) y;
          player.CameraControl.YAxisValue = 0.5f;
        }
        else
        {
          ActorCameraControl cameraControl = player.CameraControl;
          Quaternion rotation = player.Rotation;
          double num = ((Quaternion) ref rotation).get_eulerAngles().y - 30.0;
          cameraControl.XAxisValue = (float) num;
          player.CameraControl.YAxisValue = 0.6f;
        }
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        CinemachineBlendDefinition.Style prevStyle = (CinemachineBlendDefinition.Style) (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) 0;
        Manager.ADV.ChangeADVCamera((Actor) merchant);
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), 1), (System.Action<M0>) (_ => (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) prevStyle));
        Transform transform4 = ((Component) player.CameraControl.CameraComponent).get_transform();
        merchant.SetLookPtn(1, 3);
        merchant.SetLookTarget(1, 0, transform4);
        ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.Skip<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) player), 1), (Func<M0, bool>) (_ => player.CameraControl.CinemachineBrain.get_IsBlending())), 1), (System.Action<M0>) (_ => Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData)));
      }));
    }

    private void EndMerchantStory0()
    {
      this._eventPoint.SetDedicatedNumber(1);
      MapUIContainer.SetVisibleHUD(true);
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      Manager.Map instance = Singleton<Manager.Map>.Instance;
      instance.SetActiveMapEffect(true);
      MerchantActor merchant = instance.Merchant;
      MerchantData merchantData = merchant.MerchantData;
      if (Object.op_Inequality((Object) merchant, (Object) null))
      {
        if (merchantData != null)
          merchantData.Unlock = true;
        merchant.SetLookPtn(0, 3);
        merchant.SetLookTarget(0, 0, (Transform) null);
        merchant.ChangeBehavior(merchant.LastNormalMode);
      }
      AgentActor tutorialAgent = instance.TutorialAgent;
      if (Object.op_Inequality((Object) tutorialAgent, (Object) null))
      {
        tutorialAgent.ChangeFirstNormalBehavior();
        instance.TutorialAgent = (AgentActor) null;
        Manager.Map.SetTutorialProgressAndUIUpdate(16);
      }
      PlayerActor player = instance.Player;
      if (Object.op_Inequality((Object) player, (Object) null))
      {
        if (Manager.Config.GraphicData.CharasEntry[0])
        {
          player.CameraControl.Mode = CameraMode.Normal;
          player.PlayerController.ChangeState("Idle");
          ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.SkipWhile<long>((IObservable<M0>) Observable.EveryUpdate(), (Func<M0, bool>) (_ => player.CameraControl.CinemachineBrain.get_IsBlending())), 1), (System.Action<M0>) (_ =>
          {
            player.AddTutorialUI(Popup.Tutorial.Type.Girl, false);
            player.PlayerController.ChangeState("Normal");
          }));
        }
        else
        {
          player.PlayerController.ChangeState("Idle");
          Singleton<Manager.Map>.Instance.ApplyConfig((System.Action) (() =>
          {
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            CinemachineBlendDefinition.Style prevStyle = (CinemachineBlendDefinition.Style) (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style;
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) 0;
            player.CameraControl.Mode = CameraMode.Normal;
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), 1), (System.Action<M0>) (_ => (^(CinemachineBlendDefinition&) ref player.CameraControl.CinemachineBrain.m_DefaultBlend).m_Style = (__Null) prevStyle));
          }), (System.Action) (() =>
          {
            player.AddTutorialUI(Popup.Tutorial.Type.Girl, false);
            player.PlayerController.ChangeState("Normal");
          }));
        }
      }
      Singleton<Manager.Map>.Instance.SetBaseOpenState(-1, true);
      instance.Simulator.EnabledTimeProgression = true;
    }

    private class PackData : CharaPackData
    {
    }
  }
}
