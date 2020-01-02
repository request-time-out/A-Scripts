// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.PressStartToJoinExample_Assigner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
  [AddComponentMenu("")]
  public class PressStartToJoinExample_Assigner : MonoBehaviour
  {
    private static PressStartToJoinExample_Assigner instance;
    public int maxPlayers;
    private List<PressStartToJoinExample_Assigner.PlayerMap> playerMap;
    private int gamePlayerIdCounter;

    public PressStartToJoinExample_Assigner()
    {
      base.\u002Ector();
    }

    public static Player GetRewiredPlayer(int gamePlayerId)
    {
      if (!ReInput.get_isReady())
        return (Player) null;
      if (Object.op_Equality((Object) PressStartToJoinExample_Assigner.instance, (Object) null))
      {
        Debug.LogError((object) "Not initialized. Do you have a PressStartToJoinPlayerSelector in your scehe?");
        return (Player) null;
      }
      for (int index = 0; index < PressStartToJoinExample_Assigner.instance.playerMap.Count; ++index)
      {
        if (PressStartToJoinExample_Assigner.instance.playerMap[index].gamePlayerId == gamePlayerId)
          return ReInput.get_players().GetPlayer(PressStartToJoinExample_Assigner.instance.playerMap[index].rewiredPlayerId);
      }
      return (Player) null;
    }

    private void Awake()
    {
      this.playerMap = new List<PressStartToJoinExample_Assigner.PlayerMap>();
      PressStartToJoinExample_Assigner.instance = this;
    }

    private void Update()
    {
      for (int rewiredPlayerId = 0; rewiredPlayerId < ReInput.get_players().get_playerCount(); ++rewiredPlayerId)
      {
        if (ReInput.get_players().GetPlayer(rewiredPlayerId).GetButtonDown("JoinGame"))
          this.AssignNextPlayer(rewiredPlayerId);
      }
    }

    private void AssignNextPlayer(int rewiredPlayerId)
    {
      if (this.playerMap.Count >= this.maxPlayers)
      {
        Debug.LogError((object) "Max player limit already reached!");
      }
      else
      {
        int nextGamePlayerId = this.GetNextGamePlayerId();
        this.playerMap.Add(new PressStartToJoinExample_Assigner.PlayerMap(rewiredPlayerId, nextGamePlayerId));
        Player player = ReInput.get_players().GetPlayer(rewiredPlayerId);
        ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) player.controllers).maps).SetMapsEnabled(false, "Assignment");
        ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) player.controllers).maps).SetMapsEnabled(true, "Default");
        Debug.Log((object) ("Added Rewired Player id " + (object) rewiredPlayerId + " to game player " + (object) nextGamePlayerId));
      }
    }

    private int GetNextGamePlayerId()
    {
      return this.gamePlayerIdCounter++;
    }

    private class PlayerMap
    {
      public int rewiredPlayerId;
      public int gamePlayerId;

      public PlayerMap(int rewiredPlayerId, int gamePlayerId)
      {
        this.rewiredPlayerId = rewiredPlayerId;
        this.gamePlayerId = gamePlayerId;
      }
    }
  }
}
