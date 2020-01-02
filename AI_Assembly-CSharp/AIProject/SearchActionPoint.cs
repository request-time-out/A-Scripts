// Decompiled with JetBrains decompiler
// Type: AIProject.SearchActionPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEx;

namespace AIProject
{
  public class SearchActionPoint : ActionPoint
  {
    [SerializeField]
    private int _tableID = -1;
    [SerializeField]
    private int _grade;

    public int TableID
    {
      get
      {
        return this._tableID;
      }
    }

    public int Grade
    {
      get
      {
        return this._grade;
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
        SearchActionPoint searchActionPoint = this;
        if (this._playerEventType.Contains(pair.Key) && playerEventMask.Contains(pair.Key) && AIProject.Definitions.Action.NameTable.TryGetValue(pair.Key, out ValueTuple<int, string> _))
        {
          ActionPointInfo actionPointInfo = this._playerInfos.Find((Predicate<ActionPointInfo>) (x => x.eventTypeMask == pair.Key));
          string actionName = actionPointInfo.actionName;
          Sprite sprite;
          if (pair.Key == EventType.Search)
          {
            if (actionPointInfo.searchAreaID > -1)
              this._tableID = actionPointInfo.searchAreaID;
            if (actionPointInfo.gradeValue > -1 && actionPointInfo.gradeValue > this._grade)
              this._grade = actionPointInfo.gradeValue;
            Dictionary<int, int> dictionary;
            if (Singleton<Resources>.Instance.itemIconTables.EquipmentIconTable.TryGetValue(this._tableID, out dictionary))
            {
              int key;
              if (dictionary.TryGetValue(this._grade, out key))
                Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(key, out sprite);
              else
                sprite = (Sprite) null;
            }
            else
              sprite = (Sprite) null;
          }
          else
            Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(actionPointInfo.iconID, out sprite);
          Transform transform = ((Component) this).get_transform().FindLoop(actionPointInfo.labelNullName)?.get_transform() ?? ((Component) this).get_transform();
          toRelease1.Add(new CommandLabel.CommandInfo()
          {
            Text = actionName,
            Icon = sprite,
            IsHold = pair.Key == EventType.Search,
            TargetSpriteInfo = icon.ActionSpriteInfo,
            Transform = transform,
            Condition = (Func<PlayerActor, bool>) (x => closure_0.CanAction(x, pair.Key, actionPointInfo.searchAreaID)),
            ErrorText = (Func<PlayerActor, string>) (x => closure_0.ErrorText(x, pair.Key, actionPointInfo.searchAreaID)),
            CoolTimeFillRate = (Func<float>) (() =>
            {
              AIProject.SaveData.Environment.SearchActionInfo searchActionInfo;
              if (!Singleton<Game>.Instance.Environment.SearchActionLockTable.TryGetValue(closure_0.RegisterID, out searchActionInfo))
                return 0.0f;
              EnvironmentProfile environmentProfile = Singleton<Manager.Map>.Instance.EnvironmentProfile;
              return searchActionInfo.Count < environmentProfile.SearchCount ? 0.0f : 1f - searchActionInfo.ElapsedTime / environmentProfile.SearchCoolTimeDuration;
            }),
            Event = (System.Action) (() => pair.Value.Item3(Singleton<Manager.Map>.Instance.Player, (ActionPoint) closure_0))
          });
        }
      }
      this._labels = toRelease1.ToArray();
      ListPool<CommandLabel.CommandInfo>.Release(toRelease1);
      for (int key1 = 0; key1 < 2; ++key1)
      {
        List<CommandLabel.CommandInfo> toRelease2 = ListPool<CommandLabel.CommandInfo>.Get();
        foreach (KeyValuePair<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> keyValuePair in ActionPoint.DateLabelTable)
        {
          KeyValuePair<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> pair = keyValuePair;
          SearchActionPoint searchActionPoint = this;
          List<DateActionPointInfo> dateActionPointInfoList;
          if (this._playerDateEventType[key1].Contains(pair.Key) && AIProject.Definitions.Action.NameTable.TryGetValue(pair.Key, out ValueTuple<int, string> _) && this._playerDateInfos.TryGetValue(key1, out dateActionPointInfoList))
          {
            DateActionPointInfo actionPointInfo = dateActionPointInfoList.Find((Predicate<DateActionPointInfo>) (x => x.eventTypeMask == pair.Key));
            string actionName = actionPointInfo.actionName;
            Sprite sprite;
            if (pair.Key == EventType.Search)
            {
              if (actionPointInfo.searchAreaID > -1)
                this._tableID = actionPointInfo.searchAreaID;
              if (actionPointInfo.gradeValue > -1 && actionPointInfo.gradeValue > this._grade)
                this._grade = actionPointInfo.gradeValue;
              Dictionary<int, int> dictionary;
              if (Singleton<Resources>.Instance.itemIconTables.EquipmentIconTable.TryGetValue(this._tableID, out dictionary))
              {
                int key2;
                if (dictionary.TryGetValue(this._grade, out key2))
                  Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(key2, out sprite);
                else
                  sprite = (Sprite) null;
              }
              else
                sprite = (Sprite) null;
            }
            else
              Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(actionPointInfo.iconID, out sprite);
            Transform transform = ((Component) this).get_transform().FindLoop(actionPointInfo.labelNullName)?.get_transform() ?? ((Component) this).get_transform();
            toRelease2.Add(new CommandLabel.CommandInfo()
            {
              Text = actionName,
              Icon = sprite,
              IsHold = pair.Key == EventType.Search,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = transform,
              Condition = (Func<PlayerActor, bool>) (x => closure_1.CanAction(x, pair.Key, actionPointInfo.searchAreaID)),
              ErrorText = (Func<PlayerActor, string>) (x => closure_1.ErrorText(x, pair.Key, actionPointInfo.searchAreaID)),
              CoolTimeFillRate = (Func<float>) (() =>
              {
                AIProject.SaveData.Environment.SearchActionInfo searchActionInfo;
                if (!Singleton<Game>.Instance.Environment.SearchActionLockTable.TryGetValue(closure_1.RegisterID, out searchActionInfo))
                  return 0.0f;
                EnvironmentProfile environmentProfile = Singleton<Manager.Map>.Instance.EnvironmentProfile;
                return searchActionInfo.Count < environmentProfile.SearchCount ? 0.0f : 1f - searchActionInfo.ElapsedTime / environmentProfile.SearchCoolTimeDuration;
              }),
              Event = (System.Action) (() => pair.Value.Item3(Singleton<Manager.Map>.Instance.Player, (ActionPoint) closure_1))
            });
          }
        }
        this._dateLabels[key1] = toRelease2.ToArray();
        ListPool<CommandLabel.CommandInfo>.Release(toRelease2);
      }
      List<CommandLabel.CommandInfo> toRelease3 = ListPool<CommandLabel.CommandInfo>.Get();
      foreach (KeyValuePair<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> keyValuePair in ActionPoint.SickLabelTable)
      {
        KeyValuePair<EventType, Tuple<int, string, System.Action<PlayerActor, ActionPoint>>> pair = keyValuePair;
        SearchActionPoint searchActionPoint = this;
        if (this._playerEventType.Contains(pair.Key) && playerEventMask.Contains(pair.Key) && AIProject.Definitions.Action.NameTable.TryGetValue(pair.Key, out ValueTuple<int, string> _))
          toRelease3.Add(new CommandLabel.CommandInfo()
          {
            Text = pair.Value.Item2,
            Icon = (Sprite) null,
            IsHold = false,
            TargetSpriteInfo = icon.ActionSpriteInfo,
            Transform = ((Component) this).get_transform(),
            Event = (System.Action) (() => pair.Value.Item3(Singleton<Manager.Map>.Instance.Player, (ActionPoint) searchActionPoint))
          });
      }
      this._sickLabels = toRelease3.ToArray();
      ListPool<CommandLabel.CommandInfo>.Release(toRelease3);
      if (!this._playerInfos.IsNullOrEmpty<ActionPointInfo>() && this._playerInfos.Exists((Predicate<ActionPointInfo>) (x => x.eventTypeMask == EventType.Search)) || !this._agentEventType.Contains(EventType.Search))
        return;
      this._tableID = this._agentInfos.Find((Predicate<ActionPointInfo>) (x => x.eventTypeMask == EventType.Search)).searchAreaID;
    }

    public bool CanSearch(EventType eventType, StuffItem itemInfo)
    {
      if (eventType != EventType.Search)
        return true;
      int key = itemInfo.ID;
      if (itemInfo.CategoryID == -1)
        key = -1;
      Dictionary<int, int> dictionary;
      if (!Singleton<Resources>.Instance.CommonDefine.SearchItemGradeTable.TryGetValue(0, out dictionary))
        return false;
      switch (this.TableID)
      {
        case 0:
        case 1:
        case 2:
          if (key == -1)
          {
            if (0 >= this.Grade)
              return true;
            break;
          }
          int num1;
          if (dictionary.TryGetValue(key, out num1) && num1 >= this.Grade)
            return true;
          break;
        default:
          int num2;
          if (dictionary.TryGetValue(key, out num2) && num2 >= this.Grade)
            return true;
          break;
      }
      return false;
    }

    private bool CanAction(PlayerActor player, EventType eventType, int searchAreaID)
    {
      int registerId = this.RegisterID;
      Dictionary<int, AIProject.SaveData.Environment.SearchActionInfo> searchActionLockTable = Singleton<Game>.Instance.Environment.SearchActionLockTable;
      AIProject.SaveData.Environment.SearchActionInfo searchActionInfo1;
      if (!searchActionLockTable.TryGetValue(registerId, out searchActionInfo1))
      {
        AIProject.SaveData.Environment.SearchActionInfo searchActionInfo2 = new AIProject.SaveData.Environment.SearchActionInfo();
        searchActionLockTable[registerId] = searchActionInfo2;
        searchActionInfo1 = searchActionInfo2;
      }
      if (searchActionInfo1.Count >= Singleton<Manager.Map>.Instance.EnvironmentProfile.SearchCount || player.PlayerData.ItemList.Count >= player.PlayerData.InventorySlotMax)
        return false;
      StuffItem itemInfo = player.PlayerData.EquipedSearchItem(searchAreaID);
      return this.CanSearch(eventType, itemInfo);
    }

    private string ErrorText(PlayerActor player, EventType eventType, int searchAreaID)
    {
      AIProject.SaveData.Environment.SearchActionInfo searchActionInfo;
      if (Singleton<Game>.Instance.Environment.SearchActionLockTable.TryGetValue(this.RegisterID, out searchActionInfo))
      {
        EnvironmentProfile environmentProfile = Singleton<Manager.Map>.Instance.EnvironmentProfile;
        if (searchActionInfo.Count >= environmentProfile.SearchCount)
          return "しばらく採取できません";
      }
      if (player.PlayerData.ItemList.Count >= player.PlayerData.InventorySlotMax)
        return "ポーチがいっぱいです";
      StuffItem itemInfo = player.PlayerData.EquipedSearchItem(searchAreaID);
      return !this.CanSearch(eventType, itemInfo) ? "装備のランクが足りません" : string.Empty;
    }
  }
}
