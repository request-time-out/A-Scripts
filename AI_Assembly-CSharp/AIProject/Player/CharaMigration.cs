// Decompiled with JetBrains decompiler
// Type: AIProject.Player.CharaMigration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace AIProject.Player
{
  public class CharaMigration : PlayerStateBase
  {
    private Subject<Unit> _onEndMenu = new Subject<Unit>();
    private Dictionary<int, string> _agentCharaFiles = new Dictionary<int, string>();
    private Dictionary<int, int> _agentCharaMapIDs = new Dictionary<int, int>();

    protected override void OnAwake(PlayerActor player)
    {
      MapUIContainer.SetActiveCharaMigrationUI(true);
      Singleton<Input>.Instance.ReserveState(Input.ValidType.UI);
      Singleton<Input>.Instance.SetupState();
      player.SetScheduledInteractionState(false);
      player.ReleaseInteraction();
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
      this._agentCharaFiles.Clear();
      this._agentCharaMapIDs.Clear();
      foreach (KeyValuePair<int, AgentData> keyValuePair in Singleton<Game>.Instance.WorldData.AgentTable)
      {
        this._agentCharaFiles[keyValuePair.Key] = keyValuePair.Value.CharaFileName;
        this._agentCharaMapIDs[keyValuePair.Key] = keyValuePair.Value.MapID;
      }
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndMenu, 1), (Action<M0>) (_ =>
      {
        if (this.CheckChange(player))
          return;
        player.Controller.ChangeState("DeviceMenu");
      }));
    }

    protected override void OnRelease(PlayerActor player)
    {
      player.PlayerController.CommandArea.RefreshCommands();
    }

    protected override void OnUpdate(PlayerActor player, ref Actor.InputInfo info)
    {
      player.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
      if (MapUIContainer.CharaMigrateUI.IsActiveControl)
        return;
      this._onEndMenu.OnNext(Unit.get_Default());
    }

    protected override void OnAfterUpdate(PlayerActor player, Actor.InputInfo info)
    {
      player.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }

    [DebuggerHidden]
    public override IEnumerator End(Actor actor)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CharaMigration.\u003CEnd\u003Ec__Iterator0 endCIterator0 = new CharaMigration.\u003CEnd\u003Ec__Iterator0();
      return (IEnumerator) endCIterator0;
    }

    private bool CheckChange(PlayerActor player)
    {
      WorldData worldData = Singleton<Game>.Instance.WorldData;
      Singleton<Manager.Map>.Instance.ChangedCharaFiles.Clear();
      foreach (KeyValuePair<int, AgentData> keyValuePair in worldData.AgentTable)
      {
        if (this._agentCharaFiles[keyValuePair.Key] != keyValuePair.Value.CharaFileName || this._agentCharaMapIDs[keyValuePair.Key] != keyValuePair.Value.MapID)
          Singleton<Manager.Map>.Instance.ChangedCharaFiles[keyValuePair.Key] = keyValuePair.Value.CharaFileName;
      }
      if (Singleton<Manager.Map>.Instance.ChangedCharaFiles.Count <= 0)
        return false;
      player.Controller.ChangeState("CharaChange");
      return true;
    }
  }
}
