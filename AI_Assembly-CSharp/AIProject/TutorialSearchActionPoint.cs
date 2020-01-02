// Decompiled with JetBrains decompiler
// Type: AIProject.TutorialSearchActionPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class TutorialSearchActionPoint : ActionPoint
  {
    [SerializeField]
    private string _labelNullName = string.Empty;
    [SerializeField]
    private string _baseNullName = string.Empty;
    [SerializeField]
    private string _recoveryNullName = string.Empty;
    [SerializeField]
    private string[] _textList = new string[0];
    [SerializeField]
    private int _textID = 14;
    [SerializeField]
    private int _animPoseID;
    [SerializeField]
    private int _iconID;
    private Actor.SearchInfo _searchInfo;

    protected override void OnEnable()
    {
      base.OnEnable();
      CommandArea commandArea = (!Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player)?.PlayerController?.CommandArea;
      if (Object.op_Equality((Object) commandArea, (Object) null))
        return;
      if (this._labels == null)
        this.Init();
      if (commandArea.ContainsCommandableObject((ICommandable) this))
        return;
      commandArea.AddCommandableObject((ICommandable) this);
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      CommandArea commandArea = (!Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player)?.PlayerController?.CommandArea;
      if (Object.op_Equality((Object) commandArea, (Object) null))
        return;
      bool flag = commandArea.ContainsConsiderationObject((ICommandable) this);
      if (!commandArea.ContainsCommandableObject((ICommandable) this))
        return;
      commandArea.RemoveCommandableObject((ICommandable) this);
      if (!flag)
        return;
      commandArea.RefreshCommands();
    }

    private void Awake()
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance.MinimapUI.MinimapTutorialActionPointInit(this, this._iconID);
    }

    private void OnDestroy()
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance.MinimapUI.MinimapTutorialActionPointDestroy(this);
    }

    protected override void InitSub()
    {
      Tuple<int, string, Action<PlayerActor, ActionPoint>> pair;
      if (!ActionPoint.LabelTable.TryGetValue(EventType.Search, out pair))
        return;
      Resources instance = Singleton<Resources>.Instance;
      CommonDefine.CommonIconGroup icon = instance.CommonDefine.Icon;
      Dictionary<int, List<string>> commandLabelTextTable = instance.Map.EventPointCommandLabelTextTable;
      int index = !Singleton<GameSystem>.IsInstance() ? 0 : Singleton<GameSystem>.Instance.languageInt;
      List<string> source;
      commandLabelTextTable.TryGetValue(this._textID, out source);
      string str = source.GetElement<string>(index) ?? string.Empty;
      Sprite sprite = (Sprite) null;
      Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(this._iconID, out sprite);
      Transform transform = ((Component) this).get_transform().FindLoop(this._labelNullName)?.get_transform() ?? ((Component) this).get_transform();
      this._labels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          Text = str,
          Icon = sprite,
          IsHold = true,
          TargetSpriteInfo = icon.ActionSpriteInfo,
          Transform = transform,
          Condition = (Func<PlayerActor, bool>) null,
          Event = (Action) (() =>
          {
            Action<PlayerActor, ActionPoint> action = pair.Item3;
            if (action == null)
              return;
            action(Singleton<Manager.Map>.Instance.Player, (ActionPoint) this);
          })
        }
      };
    }

    public ActionPointInfo GetPlayerActionPointInfo()
    {
      return new ActionPointInfo()
      {
        poseID = this._animPoseID,
        baseNullName = this._baseNullName,
        recoveryNullName = this._recoveryNullName
      };
    }

    public Actor.SearchInfo GetSearchInfo()
    {
      if (this._searchInfo == null && Singleton<Resources>.IsInstance())
      {
        CommonDefine commonDefine = Singleton<Resources>.Instance.CommonDefine;
        Resources.GameInfoTables gameInfo = Singleton<Resources>.Instance.GameInfo;
        ItemIDKeyPair driftwoodId = commonDefine.ItemIDDefine.DriftwoodID;
        StuffItemInfo stuffItemInfo = gameInfo.GetItem(driftwoodId.categoryID, driftwoodId.itemID);
        if (stuffItemInfo == null)
          return (Actor.SearchInfo) null;
        this._searchInfo = new Actor.SearchInfo()
        {
          IsSuccess = true,
          ItemList = new List<Actor.ItemSearchInfo>()
          {
            new Actor.ItemSearchInfo()
            {
              categoryID = stuffItemInfo.CategoryID,
              id = stuffItemInfo.ID,
              name = stuffItemInfo.Name,
              count = 1
            }
          }
        };
      }
      return this._searchInfo;
    }

    public override bool TutorialHideMode()
    {
      return Manager.Map.GetTutorialProgress() != 3;
    }
  }
}
