// Decompiled with JetBrains decompiler
// Type: AIProject.Player.Onbu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.SaveData;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject.Player
{
  public class Onbu : PlayerStateBase
  {
    private static readonly RaycastHit[] _raycastHits = new RaycastHit[100];
    private Subject<Unit> _onEndInAnimation = new Subject<Unit>();
    private Subject<bool> _activeSubjectCommand = new Subject<bool>();
    private LayerMask _layer = LayerMask.op_Implicit(0);
    private string _tag = string.Empty;
    private IDisposable _onEndInAnimationDisposable;
    private IDisposable _activeSubjectCommandDisposable;
    private CommandLabel.CommandInfo _cancelCommandInfo;
    private CommandLabel.CommandInfo _uncancelableCommandInfo;

    protected override void OnAwake(PlayerActor player)
    {
      Sprite sprite;
      Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(Singleton<Resources>.Instance.PlayerProfile.CommonActionIconID, out sprite);
      this._cancelCommandInfo = new CommandLabel.CommandInfo()
      {
        Text = "下ろす",
        Icon = sprite,
        TargetSpriteInfo = (CommandTargetSpriteInfo) null,
        Transform = (Transform) null,
        Event = (System.Action) (() => this.ExitState())
      };
      this._uncancelableCommandInfo = new CommandLabel.CommandInfo()
      {
        Text = (string) null,
        Icon = (Sprite) null,
        TargetSpriteInfo = (CommandTargetSpriteInfo) null,
        Transform = (Transform) null,
        Condition = (Func<PlayerActor, bool>) null,
        Event = (System.Action) (() => this.OutputWarningMessage())
      };
      player.Mode = Desire.ActionType.Onbu;
      player.CameraControl.CrossFade.FadeStart(-1f);
      AgentActor agentPartner = player.AgentPartner;
      agentPartner.Partner = (Actor) player;
      agentPartner.IsSlave = true;
      agentPartner.Animation.StopAllAnimCoroutine();
      agentPartner.BehaviorResources.ChangeMode(Desire.ActionType.Onbu);
      ((Behaviour) agentPartner.NavMeshAgent).set_enabled(false);
      player.PlayerController.CommandArea.RefreshCommands();
      if (player.PlayerController.PrevStateName == "Normal" || player.PlayerController.PrevStateName == "Houchi")
        this.ActivateTransfer(player, (Actor) agentPartner);
      else
        this.ActivateTransferImmediate(player, (Actor) agentPartner);
      this._onEndInAnimationDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndInAnimation, 1), (System.Action<M0>) (_ =>
      {
        this._activeSubjectCommandDisposable = ObservableExtensions.Subscribe<bool>(Observable.DistinctUntilChanged<bool>((IObservable<M0>) this._activeSubjectCommand), (System.Action<M0>) (isOn =>
        {
          if (isOn)
          {
            if (MapUIContainer.CommandLabel.SubjectCommand == this._cancelCommandInfo)
              return;
            MapUIContainer.CommandLabel.SubjectCommand = this._cancelCommandInfo;
            player.PlayerController.CommandArea.RefreshCommands();
          }
          else
          {
            if (MapUIContainer.CommandLabel.SubjectCommand == this._uncancelableCommandInfo)
              return;
            MapUIContainer.CommandLabel.SubjectCommand = this._uncancelableCommandInfo;
            player.PlayerController.CommandArea.RefreshCommands();
          }
        }));
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
      }));
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
      this._layer = Singleton<Resources>.Instance.DefinePack.MapDefines.HLayer;
      this._tag = Singleton<Resources>.Instance.DefinePack.MapDefines.OnbuMeshTag;
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      Singleton<Manager.Map>.Instance.CheckTutorialState(player);
    }

    private void ActivateTransfer(PlayerActor player, Actor partner)
    {
      EquipEventItemInfo itemInfo = (EquipEventItemInfo) null;
      PlayState info1;
      this.LoadLocomotionAnimation(player, out info1, ref itemInfo);
      player.ResetEquipEventItem(itemInfo);
      partner.Position = player.Position;
      partner.Rotation = player.Rotation;
      int onbuStateId = Singleton<Resources>.Instance.DefinePack.AnimatorState.OnbuStateID;
      PlayState info2 = Singleton<Resources>.Instance.Animation.PlayerLocomotionStateTable[(int) player.ChaControl.sex][onbuStateId];
      ActorAnimation animation1 = player.Animation;
      ActorAnimation animation2 = partner.Animation;
      AnimatorStateInfo animatorStateInfo = animation1.Animator.GetCurrentAnimatorStateInfo(0);
      if (info1 != null)
      {
        animation1.InitializeStates(info1);
        animation2.InitializeStates(info2);
        bool flag = false;
        foreach (PlayState.Info stateInfo in info1.MainStateInfo.InStateInfo.StateInfos)
        {
          if (((AnimatorStateInfo) ref animatorStateInfo).get_shortNameHash() == stateInfo.ShortNameStateHash)
          {
            flag = true;
            break;
          }
        }
        if (flag)
        {
          if (info1.MaskStateInfo.layer > 0)
          {
            if ((double) animation1.Animator.GetLayerWeight(info1.MaskStateInfo.layer) == 0.0)
              flag = false;
          }
          else
          {
            for (int index = 1; index < animation1.Animator.get_layerCount(); ++index)
            {
              if ((double) animation1.Animator.GetLayerWeight(index) > 0.0)
              {
                flag = false;
                break;
              }
            }
          }
        }
        if (flag)
        {
          animation1.InStates.Clear();
          animation1.OutStates.Clear();
          animation1.ActionStates.Clear();
          animation2.InStates.Clear();
          animation2.OutStates.Clear();
          animation2.ActionStates.Clear();
        }
        else
        {
          int layer = info1.Layer;
          if (animation1.RefsActAnimInfo)
          {
            animation1.StopAllAnimCoroutine();
            animation1.PlayInLocoAnimation(animation1.AnimInfo.outEnableBlend, animation1.AnimInfo.outBlendSec, layer);
            animation2.StopAllAnimCoroutine();
            animation2.PlayInLocoAnimation(animation1.AnimInfo.outEnableBlend, animation1.AnimInfo.outBlendSec, layer);
            animation1.RefsActAnimInfo = false;
          }
          else
          {
            bool enableFade = info1.MainStateInfo.InStateInfo.EnableFade;
            float fadeSecond = info1.MainStateInfo.InStateInfo.FadeSecond;
            animation1.StopAllAnimCoroutine();
            animation1.PlayInLocoAnimation(enableFade, fadeSecond, layer);
            animation2.StopAllAnimCoroutine();
            animation2.PlayInLocoAnimation(enableFade, fadeSecond, layer);
          }
        }
      }
      else
      {
        for (int index = 1; index < animation1.Animator.get_layerCount(); ++index)
          animation1.Animator.SetLayerWeight(index, 0.0f);
        animation1.InitializeStates(info2);
        animation2.InitializeStates(info2);
        int layer = info2.Layer;
        if (animation1.RefsActAnimInfo)
        {
          animation1.StopAllAnimCoroutine();
          animation1.PlayInLocoAnimation(animation1.AnimInfo.endEnableBlend, animation1.AnimInfo.endBlendSec, layer);
          animation2.StopAllAnimCoroutine();
          animation2.PlayInLocoAnimation(animation1.AnimInfo.inEnableBlend, animation1.AnimInfo.inBlendSec, layer);
          animation1.RefsActAnimInfo = false;
        }
        else
        {
          ActorAnimInfo actorAnimInfo1 = new ActorAnimInfo()
          {
            layer = info2.Layer,
            inEnableBlend = info2.MainStateInfo.InStateInfo.EnableFade,
            inBlendSec = info2.MainStateInfo.InStateInfo.FadeSecond
          };
          player.Animation.AnimInfo = actorAnimInfo1;
          ActorAnimInfo actorAnimInfo2 = actorAnimInfo1;
          bool inEnableBlend = actorAnimInfo2.inEnableBlend;
          float inBlendSec = actorAnimInfo2.inBlendSec;
          animation1.StopAllAnimCoroutine();
          animation1.PlayInLocoAnimation(inEnableBlend, inBlendSec, layer);
          animation2.StopAllAnimCoroutine();
          animation2.PlayInLocoAnimation(inEnableBlend, inBlendSec, layer);
        }
      }
      if (player.NavMeshAgent.get_isStopped())
        player.NavMeshAgent.set_isStopped(false);
      if (!player.IsKinematic)
        return;
      player.IsKinematic = false;
    }

    private void ActivateTransferImmediate(PlayerActor player, Actor partner)
    {
      EquipEventItemInfo itemInfo = (EquipEventItemInfo) null;
      PlayState info;
      this.LoadLocomotionAnimation(player, out info, ref itemInfo);
      player.ResetEquipEventItem(itemInfo);
      partner.Position = player.Position;
      partner.Rotation = player.Rotation;
      int onbuStateId = Singleton<Resources>.Instance.DefinePack.AnimatorState.OnbuStateID;
      PlayState playState = Singleton<Resources>.Instance.Animation.PlayerLocomotionStateTable[(int) player.ChaControl.sex][onbuStateId];
      ActorAnimation animation1 = player.Animation;
      ActorAnimation animation2 = partner.Animation;
      animation1.InStates.Clear();
      animation1.OutStates.Clear();
      animation1.ActionStates.Clear();
      partner.Animation.InStates.Clear();
      partner.Animation.OutStates.Clear();
      partner.Animation.ActionStates.Clear();
      PlayState.Info element1 = playState.MainStateInfo.InStateInfo.StateInfos.GetElement<PlayState.Info>(playState.MainStateInfo.InStateInfo.StateInfos.Length - 1);
      if (info != null)
      {
        PlayState.Info element2 = info.MainStateInfo.InStateInfo.StateInfos.GetElement<PlayState.Info>(info.MainStateInfo.InStateInfo.StateInfos.Length - 1);
        player.Animation.InStates.Enqueue(element2);
        if (!info.MainStateInfo.OutStateInfo.StateInfos.IsNullOrEmpty<PlayState.Info>())
        {
          foreach (PlayState.Info stateInfo in info.MainStateInfo.OutStateInfo.StateInfos)
            player.Animation.OutStates.Enqueue(stateInfo);
        }
        partner.Animation.InStates.Enqueue(element1);
        if (!playState.MainStateInfo.OutStateInfo.StateInfos.IsNullOrEmpty<PlayState.Info>())
        {
          foreach (PlayState.Info stateInfo in playState.MainStateInfo.OutStateInfo.StateInfos)
            partner.Animation.OutStates.Enqueue(stateInfo);
        }
        ActorAnimInfo animInfo = player.Animation.AnimInfo;
        int layer = info.Layer;
        player.Animation.StopAllAnimCoroutine();
        player.Animation.PlayInLocoAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, layer);
        partner.Animation.StopAllAnimCoroutine();
        partner.Animation.PlayInLocoAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, layer);
      }
      else
      {
        for (int index = 1; index < animation1.Animator.get_layerCount(); ++index)
          animation1.Animator.SetLayerWeight(index, 0.0f);
        player.Animation.InStates.Enqueue(element1);
        if (!playState.MainStateInfo.OutStateInfo.StateInfos.IsNullOrEmpty<PlayState.Info>())
        {
          foreach (PlayState.Info stateInfo in playState.MainStateInfo.OutStateInfo.StateInfos)
            player.Animation.OutStates.Enqueue(stateInfo);
        }
        partner.Animation.InStates.Enqueue(element1);
        if (!playState.MainStateInfo.OutStateInfo.StateInfos.IsNullOrEmpty<PlayState.Info>())
        {
          foreach (PlayState.Info stateInfo in playState.MainStateInfo.OutStateInfo.StateInfos)
            partner.Animation.OutStates.Enqueue(stateInfo);
        }
        ActorAnimInfo animInfo = player.Animation.AnimInfo;
        int layer = playState.Layer;
        player.Animation.StopAllAnimCoroutine();
        player.Animation.PlayInLocoAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, layer);
        partner.Animation.StopAllAnimCoroutine();
        partner.Animation.PlayInLocoAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, layer);
      }
      if (player.NavMeshAgent.get_isStopped())
        player.NavMeshAgent.set_isStopped(false);
      if (!player.IsKinematic)
        return;
      player.IsKinematic = false;
    }

    private void LoadLocomotionAnimation(
      PlayerActor player,
      out PlayState info,
      ref EquipEventItemInfo itemInfo)
    {
      Resources instance = Singleton<Resources>.Instance;
      LocomotionProfile locomotionProfile = instance.LocomotionProfile;
      PlayerProfile playerProfile = instance.PlayerProfile;
      StuffItem equipedLampItem = player.PlayerData.EquipedLampItem;
      CommonDefine.ItemIDDefines itemIdDefine = instance.CommonDefine.ItemIDDefine;
      if (equipedLampItem != null)
      {
        ItemIDKeyPair torchId = itemIdDefine.TorchID;
        ItemIDKeyPair flashlightId = itemIdDefine.FlashlightID;
        ItemIDKeyPair maleLampId = itemIdDefine.MaleLampID;
        if (equipedLampItem.CategoryID == torchId.categoryID && equipedLampItem.ID == torchId.itemID)
        {
          info = instance.Animation.PlayerLocomotionStateTable[(int) player.ChaControl.sex][playerProfile.PoseIDData.TorchOnbuLocoID];
          itemInfo = instance.GameInfo.CommonEquipEventItemTable[torchId.categoryID][torchId.itemID];
          itemInfo.ParentName = instance.LocomotionProfile.PlayerLocoItemParentName;
          return;
        }
        if (equipedLampItem.CategoryID == maleLampId.categoryID && equipedLampItem.ID == maleLampId.itemID)
        {
          info = instance.Animation.PlayerLocomotionStateTable[(int) player.ChaControl.sex][playerProfile.PoseIDData.LampOnbuLocoID];
          itemInfo = instance.GameInfo.CommonEquipEventItemTable[maleLampId.categoryID][maleLampId.itemID];
          itemInfo.ParentName = instance.LocomotionProfile.PlayerLocoItemParentName;
          return;
        }
        if (equipedLampItem.CategoryID == flashlightId.categoryID && equipedLampItem.ID == flashlightId.itemID)
        {
          info = instance.Animation.PlayerLocomotionStateTable[(int) player.ChaControl.sex][playerProfile.PoseIDData.TorchOnbuLocoID];
          itemInfo = instance.GameInfo.CommonEquipEventItemTable[flashlightId.categoryID][flashlightId.itemID];
          itemInfo.ParentName = instance.LocomotionProfile.PlayerLocoItemParentName;
          return;
        }
      }
      info = (PlayState) null;
    }

    private void ExitState()
    {
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() =>
      {
        PlayerActor player = Singleton<Manager.Map>.Instance.Player;
        return player.Controller.State.End((Actor) player);
      }), false), 1), (System.Action<M0>) (__ =>
      {
        PlayerActor player = Singleton<Manager.Map>.Instance.Player;
        player.CameraControl.CrossFade.FadeStart(-1f);
        player.PlayerController.ChangeState("Normal");
        player.CameraControl.Mode = CameraMode.Normal;
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
        AgentActor agentPartner = player.AgentPartner;
        agentPartner.ChangeBehavior(agentPartner.PrevMode);
        agentPartner.Partner = (Actor) null;
        agentPartner.IsSlave = false;
        ((Behaviour) agentPartner.NavMeshAgent).set_enabled(true);
        Vector3 point = Vector3.op_Addition(player.Position, Vector3.op_Multiply(player.Back, 10f));
        LayerMask hlayer = Singleton<Resources>.Instance.DefinePack.MapDefines.HLayer;
        GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag(Singleton<Resources>.Instance.DefinePack.MapDefines.OnbuMeshTag);
        List<MeshCollider> toRelease = ListPool<MeshCollider>.Get();
        foreach (GameObject gameObject in gameObjectsWithTag)
        {
          MeshCollider[] componentsInChildren = (MeshCollider[]) gameObject.GetComponentsInChildren<MeshCollider>();
          toRelease.AddRange((IEnumerable<MeshCollider>) componentsInChildren);
        }
        float num1 = float.PositiveInfinity;
        Vector3 vector3_1 = Vector3.get_zero();
        using (List<MeshCollider>.Enumerator enumerator = toRelease.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Vector3 vector3_2 = enumerator.Current.NearestVertexTo(point);
            float num2 = Vector3.Distance(player.Position, vector3_2);
            if ((double) num2 < (double) num1)
            {
              num1 = num2;
              vector3_1 = vector3_2;
            }
          }
        }
        ListPool<MeshCollider>.Release(toRelease);
        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(vector3_1, ref navMeshHit, 100f, -1))
          vector3_1 = ((NavMeshHit) ref navMeshHit).get_position();
        player.Partner.NavMeshAgent.Warp(vector3_1);
        Vector3 vector3_3 = Vector3.op_Addition(vector3_1, Vector3.op_Multiply(player.Partner.Forward, 10f));
        if (NavMesh.SamplePosition(vector3_3, ref navMeshHit, 100f, -1))
          vector3_3 = ((NavMeshHit) ref navMeshHit).get_position();
        player.NavMeshAgent.Warp(vector3_3);
        player.Partner = (Actor) null;
        agentPartner.IsVisible = true;
      }));
    }

    private void OutputWarningMessage()
    {
      MapUIContainer.PushMessageUI("ここに女の子を下ろすことはできません", 2, 0, (System.Action) null);
    }

    protected override void OnAfterUpdate(PlayerActor actor, Actor.InputInfo info)
    {
      actor.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }

    protected override void OnUpdate(PlayerActor actor, ref Actor.InputInfo info)
    {
      Singleton<Manager.Map>.Instance.CheckStoryProgress();
      if (actor.Animation.PlayingInLocoAnimation)
        return;
      this._onEndInAnimation.OnNext(Unit.get_Default());
      Manager.Input instance = Singleton<Manager.Input>.Instance;
      PlayerActor playerActor = actor;
      if (instance.State == Manager.Input.ValidType.Action)
      {
        Transform transform = ((Component) playerActor.CameraControl).get_transform();
        Vector2 moveAxis = instance.MoveAxis;
        Quaternion rotation = transform.get_rotation();
        Vector3 vector3 = Quaternion.op_Multiply(Quaternion.Euler(0.0f, (float) ((Quaternion) ref rotation).get_eulerAngles().y, 0.0f), Vector3.ClampMagnitude(new Vector3((float) moveAxis.x, 0.0f, (float) moveAxis.y), 1f));
        info.move = vector3;
        if (UnityEngine.Input.GetKeyDown((KeyCode) 325) || UnityEngine.Input.GetKeyDown((KeyCode) 282))
        {
          playerActor.PlayerController.ChangeState("Menu");
        }
        else
        {
          if (!UnityEngine.Input.GetKeyDown((KeyCode) 109))
            return;
          if (Singleton<MapUIContainer>.Instance.MinimapUI.VisibleMode == 0)
          {
            playerActor.Controller.ChangeState("WMap");
          }
          else
          {
            if (Singleton<MapUIContainer>.Instance.MinimapUI.VisibleMode != 2)
              return;
            if (Manager.Config.GameData.MiniMap)
              Singleton<MapUIContainer>.Instance.MinimapUI.OpenMiniMap();
            else
              playerActor.Controller.ChangeState("WMap");
          }
        }
      }
      else
      {
        info.move = Vector3.get_zero();
        Transform transform = ((Component) playerActor.CameraControl).get_transform();
        info.lookPos = Vector3.op_Addition(((Component) actor).get_transform().get_position(), Vector3.op_Multiply(transform.get_forward(), 100f));
      }
    }

    protected override void OnFixedUpdate(PlayerActor player, Actor.InputInfo info)
    {
      if (player.Animation.PlayingInLocoAnimation)
        return;
      this._layer = Singleton<Resources>.Instance.DefinePack.MapDefines.HLayer;
      this._tag = Singleton<Resources>.Instance.DefinePack.MapDefines.OnbuMeshTag;
      Vector3 position = player.Position;
      ref Vector3 local = ref position;
      local.y = (__Null) (local.y + 15.0);
      int num = Physics.SphereCastNonAlloc(position, 7.5f, Vector3.get_down(), Onbu._raycastHits, 25f, LayerMask.op_Implicit(this._layer));
      bool flag = false;
      for (int index = 0; index < num; ++index)
      {
        if (((Component) ((RaycastHit) ref Onbu._raycastHits[index]).get_collider()).get_tag() == this._tag)
        {
          flag = true;
          break;
        }
      }
      this._activeSubjectCommand.OnNext(flag);
      for (int index = 0; index < num; ++index)
        Onbu._raycastHits[index] = (RaycastHit) null;
    }

    protected override void OnRelease(PlayerActor player)
    {
      MapUIContainer.CommandLabel.SubjectCommand = (CommandLabel.CommandInfo) null;
      MapUIContainer.CommandLabel.RefreshCommands();
      if (this._onEndInAnimationDisposable != null)
        this._onEndInAnimationDisposable.Dispose();
      if (this._activeSubjectCommandDisposable != null)
        this._activeSubjectCommandDisposable.Dispose();
      player.Mode = Desire.ActionType.Normal;
    }
  }
}
