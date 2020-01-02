// Decompiled with JetBrains decompiler
// Type: AIProject.WarpPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Player;
using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEx;

namespace AIProject
{
  public class WarpPoint : ActionPoint
  {
    [SerializeField]
    private int _tableID = -1;
    [SerializeField]
    private Renderer[] _renderers;

    public override CommandLabel.CommandInfo[] Labels
    {
      get
      {
        return Singleton<Manager.Map>.Instance.Player.PlayerController.State is Onbu ? this._sickLabels : this._labels;
      }
    }

    public override bool IsNeutralCommand
    {
      get
      {
        return base.IsNeutralCommand && ((Behaviour) this).get_isActiveAndEnabled();
      }
    }

    public int TableID
    {
      get
      {
        return this._tableID;
      }
    }

    public Renderer[] Renderers
    {
      get
      {
        return this._renderers;
      }
    }

    protected override void InitSub()
    {
      CommonDefine.CommonIconGroup icon = Singleton<Resources>.Instance.CommonDefine.Icon;
      EventType playerEventMask = Singleton<Resources>.Instance.DefinePack.MapDefines.PlayerEventMask;
      List<CommandLabel.CommandInfo> toRelease1 = ListPool<CommandLabel.CommandInfo>.Get();
      foreach (KeyValuePair<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> keyValuePair in ActionPoint.LabelTable)
      {
        KeyValuePair<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> pair = keyValuePair;
        WarpPoint warpPoint = this;
        if (this._playerEventType.Contains(pair.Key) && playerEventMask.Contains(pair.Key) && AIProject.Definitions.Action.NameTable.TryGetValue(pair.Key, out ValueTuple<int, string> _))
        {
          ActionPointInfo actionPointInfo = this._playerInfos.Find((Predicate<ActionPointInfo>) (x => x.eventTypeMask == pair.Key));
          string actionName = actionPointInfo.actionName;
          Sprite sprite;
          Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(actionPointInfo.iconID, out sprite);
          this._tableID = actionPointInfo.searchAreaID;
          Transform transform = ((Component) this).get_transform().FindLoop(actionPointInfo.labelNullName)?.get_transform() ?? ((Component) this).get_transform();
          toRelease1.Add(new CommandLabel.CommandInfo()
          {
            Text = actionName,
            Icon = sprite,
            IsHold = true,
            TargetSpriteInfo = icon.ActionSpriteInfo,
            Transform = transform,
            Condition = (Func<PlayerActor, bool>) (x => warpPoint.CanAccess()),
            ErrorText = (Func<PlayerActor, string>) (x => warpPoint.ErrorText()),
            Event = (System.Action) (() => pair.Value.Item3(Singleton<Manager.Map>.Instance.Player, (ActionPoint) warpPoint))
          });
        }
      }
      this._labels = toRelease1.ToArray();
      ListPool<CommandLabel.CommandInfo>.Release(toRelease1);
      for (int key = 0; key < 2; ++key)
      {
        List<CommandLabel.CommandInfo> toRelease2 = ListPool<CommandLabel.CommandInfo>.Get();
        foreach (KeyValuePair<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> keyValuePair in ActionPoint.DateLabelTable)
        {
          KeyValuePair<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> pair = keyValuePair;
          WarpPoint warpPoint = this;
          List<DateActionPointInfo> dateActionPointInfoList;
          if (this._playerDateEventType[key].Contains(pair.Key) && AIProject.Definitions.Action.NameTable.TryGetValue(pair.Key, out ValueTuple<int, string> _) && this._playerDateInfos.TryGetValue(key, out dateActionPointInfoList))
          {
            DateActionPointInfo dateActionPointInfo = dateActionPointInfoList.Find((Predicate<DateActionPointInfo>) (x => x.eventTypeMask == pair.Key));
            string actionName = dateActionPointInfo.actionName;
            Sprite sprite;
            Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(dateActionPointInfo.iconID, out sprite);
            Transform transform = ((Component) this).get_transform().FindLoop(dateActionPointInfo.labelNullName)?.get_transform() ?? ((Component) this).get_transform();
            toRelease2.Add(new CommandLabel.CommandInfo()
            {
              Text = actionName,
              Icon = sprite,
              IsHold = true,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = transform,
              Condition = (Func<PlayerActor, bool>) (x => warpPoint.CanAccess()),
              ErrorText = (Func<PlayerActor, string>) (x => warpPoint.ErrorText()),
              Event = (System.Action) (() => pair.Value.Item3(Singleton<Manager.Map>.Instance.Player, (ActionPoint) warpPoint))
            });
          }
        }
        this._dateLabels[key] = toRelease2.ToArray();
        ListPool<CommandLabel.CommandInfo>.Release(toRelease2);
      }
      List<CommandLabel.CommandInfo> toRelease3 = ListPool<CommandLabel.CommandInfo>.Get();
      foreach (KeyValuePair<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> keyValuePair in ActionPoint.SickLabelTable)
      {
        KeyValuePair<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> pair = keyValuePair;
        WarpPoint warpPoint = this;
        if (this._playerEventType.Contains(pair.Key) && playerEventMask.Contains(pair.Key) && AIProject.Definitions.Action.NameTable.TryGetValue(pair.Key, out ValueTuple<int, string> _))
        {
          ActionPointInfo actionPointInfo = this._playerInfos.Find((Predicate<ActionPointInfo>) (x => x.eventTypeMask == pair.Key));
          string actionName = actionPointInfo.actionName;
          Sprite sprite;
          Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(actionPointInfo.iconID, out sprite);
          Transform transform = ((Component) this).get_transform().FindLoop(actionPointInfo.labelNullName)?.get_transform() ?? ((Component) this).get_transform();
          toRelease3.Add(new CommandLabel.CommandInfo()
          {
            Text = pair.Value.Item2,
            Icon = sprite,
            IsHold = true,
            TargetSpriteInfo = icon.ActionSpriteInfo,
            Transform = transform,
            Condition = (Func<PlayerActor, bool>) (x => warpPoint.CanAccess()),
            ErrorText = (Func<PlayerActor, string>) (x => warpPoint.ErrorText()),
            Event = (System.Action) (() => pair.Value.Item3(Singleton<Manager.Map>.Instance.Player, (ActionPoint) warpPoint))
          });
        }
      }
      this._sickLabels = toRelease3.ToArray();
      ListPool<CommandLabel.CommandInfo>.Release(toRelease3);
      if (!this._playerInfos.IsNullOrEmpty<ActionPointInfo>() && this._playerInfos.Exists((Predicate<ActionPointInfo>) (x => x.eventTypeMask == EventType.Warp)) || !this._agentEventType.Contains(EventType.Warp))
        return;
      this._tableID = this._agentInfos.Find((Predicate<ActionPointInfo>) (x => x.eventTypeMask == EventType.Warp)).searchAreaID;
    }

    public bool CanAccess()
    {
      MapArea ownerArea = this.OwnerArea;
      Dictionary<int, List<WarpPoint>> dictionary;
      List<WarpPoint> warpPointList;
      return !Object.op_Equality((Object) ownerArea, (Object) null) && Singleton<Manager.Map>.Instance.WarpPointDic.TryGetValue(ownerArea.ChunkID, out dictionary) && (dictionary.TryGetValue(this._tableID, out warpPointList) && warpPointList.Count >= 2);
    }

    public string ErrorText()
    {
      MapArea ownerArea = this.OwnerArea;
      Dictionary<int, List<WarpPoint>> dictionary;
      List<WarpPoint> warpPointList;
      return Object.op_Equality((Object) ownerArea, (Object) null) || Singleton<Manager.Map>.Instance.WarpPointDic.TryGetValue(ownerArea.ChunkID, out dictionary) && dictionary.TryGetValue(this._tableID, out warpPointList) && warpPointList.Count >= 2 ? string.Empty : "同じ色のワープ装置が無いと、移動できません";
    }

    public WarpPoint PairPoint()
    {
      MapArea ownerArea = this.OwnerArea;
      if (Object.op_Equality((Object) ownerArea, (Object) null))
        return (WarpPoint) null;
      Dictionary<int, List<WarpPoint>> dictionary;
      List<WarpPoint> warpPointList;
      if (!Singleton<Manager.Map>.Instance.WarpPointDic.TryGetValue(ownerArea.ChunkID, out dictionary) || !dictionary.TryGetValue(this._tableID, out warpPointList))
        return (WarpPoint) null;
      if (warpPointList.Count < 2)
        return (WarpPoint) null;
      WarpPoint warpPoint1 = (WarpPoint) null;
      foreach (WarpPoint warpPoint2 in warpPointList)
      {
        if (!Object.op_Equality((Object) warpPoint2, (Object) this) && !Object.op_Equality((Object) warpPoint2, (Object) null))
          warpPoint1 = warpPoint2;
      }
      return warpPoint1;
    }
  }
}
