// Decompiled with JetBrains decompiler
// Type: AIProject.Player.ChickenCoopMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject.Player
{
  public class ChickenCoopMenu : PlayerStateBase
  {
    private Input _input;
    private FarmPoint _currentFarmPoint;

    protected override void OnAwake(PlayerActor player)
    {
      this._currentFarmPoint = player.CurrentFarmPoint;
      if (Object.op_Equality((Object) this._currentFarmPoint, (Object) null))
      {
        player.PlayerController.ChangeState("Normal");
      }
      else
      {
        this._input = Singleton<Input>.Instance;
        Input.ValidType state = this._input.State;
        this._input.ReserveState(Input.ValidType.UI);
        this._input.SetupState();
        this._input.ReserveState(state);
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
        MapUIContainer.SetVisibleHUD(false);
        int registerId = this._currentFarmPoint.RegisterID;
        List<AIProject.SaveData.Environment.ChickenInfo> chickenInfoList1 = (List<AIProject.SaveData.Environment.ChickenInfo>) null;
        Dictionary<int, List<AIProject.SaveData.Environment.ChickenInfo>> dictionary = !Singleton<Game>.IsInstance() ? (Dictionary<int, List<AIProject.SaveData.Environment.ChickenInfo>>) null : Singleton<Game>.Instance.Environment?.ChickenTable;
        if (dictionary != null && (!dictionary.TryGetValue(registerId, out chickenInfoList1) || chickenInfoList1 == null))
        {
          List<AIProject.SaveData.Environment.ChickenInfo> chickenInfoList2 = new List<AIProject.SaveData.Environment.ChickenInfo>();
          dictionary[registerId] = chickenInfoList2;
          chickenInfoList1 = chickenInfoList2;
        }
        if (chickenInfoList1 == null)
          chickenInfoList1 = new List<AIProject.SaveData.Environment.ChickenInfo>();
        MapUIContainer.ChickenCoopUI.currentChickens = chickenInfoList1;
        MapUIContainer.ChickenCoopUI.ClosedEvent = (Action) (() => MapUIContainer.CommandList.Visibled = true);
        MapUIContainer.RefreshCommands(0, player.ChickenCoopCommandInfos);
        MapUIContainer.CommandList.CancelEvent = (Action) (() =>
        {
          MapUIContainer.SetActiveCommandList(false);
          player.PlayerController.ChangeState("Normal");
        });
        MapUIContainer.SetActiveCommandList(true, "鶏小屋");
      }
    }

    protected override void OnUpdate(PlayerActor player, ref Actor.InputInfo info)
    {
      player.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
    }

    protected override void OnAfterUpdate(PlayerActor player, Actor.InputInfo info)
    {
      player.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }

    protected override void OnRelease(PlayerActor player)
    {
      if (Object.op_Equality((Object) this._currentFarmPoint, (Object) null))
        return;
      player.CurrentFarmPoint = (FarmPoint) null;
      MapUIContainer.ChickenCoopUI.ClosedEvent = (Action) null;
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
      MapUIContainer.SetVisibleHUD(true);
      if (this._input == null)
        return;
      this._input.SetupState();
    }
  }
}
