// Decompiled with JetBrains decompiler
// Type: AIProject.AllAreaMapUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEx;

namespace AIProject
{
  public class AllAreaMapUI : MonoBehaviour
  {
    [SerializeField]
    private Image[] IslandLabel;
    [SerializeField]
    private Text IslandName;
    [SerializeField]
    private GameObject GirlsList;
    [SerializeField]
    private GameObject GirlsListNode;
    private List<GameObject> GirlsNameList;
    [SerializeField]
    private Image WarpPanelLabel;
    [SerializeField]
    private Image WarpBG;
    [SerializeField]
    private GameObject WarpContent;
    [SerializeField]
    private Button WarpContentNode;
    private List<Button> WarpNodes;
    [SerializeField]
    private Button WorldMap;
    [SerializeField]
    private AllAreaMapActionFilter ActionFilter;
    [SerializeField]
    private GameObject PlayGuideList;
    [SerializeField]
    private GameObject PlayGuideNode;
    private MiniMapControler miniMapcontroler;
    private GameObject allMapcontroler;
    private Input Input;
    private AllAreaMapUI.ListComparer basePointComparer;
    private bool gameClear;
    [SerializeField]
    private MapActionCategoryUI mapAction;
    [SerializeField]
    private WarpListUI warpListUI;
    private Vector3 LocalScaleDef;
    private GameObject[] Guid;
    private Dictionary<int, AgentActor> sortedDic;
    private PointerEnterTrigger enterTrigger;
    private UITrigger.TriggerEvent onEnter;
    public IDisposable WarpSelectSubscriber;

    public AllAreaMapUI()
    {
      base.\u002Ector();
    }

    public List<Button> _WarpNodes
    {
      get
      {
        return this.WarpNodes;
      }
    }

    public Button _WorldMap
    {
      get
      {
        return this.WorldMap;
      }
    }

    public bool GameClear
    {
      set
      {
        this.gameClear = value;
      }
      get
      {
        return this.gameClear;
      }
    }

    public void Init(MiniMapControler miniMap, GameObject allCamera)
    {
      this.Input = Singleton<Input>.Instance;
      this.miniMapcontroler = miniMap;
      this.allMapcontroler = allCamera;
      AssetBundleInfo assetBundleInfo;
      if (Singleton<Resources>.Instance.Map.MapList.TryGetValue(Singleton<Manager.Map>.Instance.MapID, out assetBundleInfo))
        this.IslandName.set_text((string) assetBundleInfo.name);
      else
        this.IslandName.set_text("廃墟の島");
      this.SetSelectGirlList();
      this.RefreshWarpPointNode();
      ((UnityEventBase) this.WorldMap.get_onClick()).RemoveAllListeners();
      Button.ButtonClickedEvent onClick = this.WorldMap.get_onClick();
      // ISSUE: reference to a compiler-generated field
      if (AllAreaMapUI.\u003C\u003Ef__am\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        AllAreaMapUI.\u003C\u003Ef__am\u0024cache0 = new UnityAction((object) null, __methodptr(\u003CInit\u003Em__0));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction fAmCache0 = AllAreaMapUI.\u003C\u003Ef__am\u0024cache0;
      ((UnityEvent) onClick).AddListener(fAmCache0);
      DefinePack.AssetBundleManifestsDefine abManifests = Singleton<Resources>.Instance.DefinePack.ABManifests;
      if (Object.op_Equality((Object) this.Guid[0], (Object) null))
      {
        this.Guid[0] = (GameObject) Object.Instantiate<GameObject>((M0) this.PlayGuideNode);
        this.Guid[0].get_transform().SetParent(this.PlayGuideList.get_transform(), false);
        this.Guid[0].get_transform().set_localScale(this.LocalScaleDef);
        ((Image) this.Guid[0].GetComponentInChildren<Image>()).set_sprite(Singleton<Resources>.Instance.itemIconTables.InputIconTable[2]);
        ((Text) this.Guid[0].GetComponentInChildren<Text>()).set_text("拡大・縮小");
        this.Guid[0].get_gameObject().SetActive(true);
      }
      if (Object.op_Equality((Object) this.Guid[1], (Object) null))
      {
        this.Guid[1] = (GameObject) Object.Instantiate<GameObject>((M0) this.PlayGuideNode);
        this.Guid[1].get_transform().SetParent(this.PlayGuideList.get_transform(), false);
        this.Guid[1].get_transform().set_localScale(this.LocalScaleDef);
        ((Image) this.Guid[1].GetComponentInChildren<Image>()).set_sprite(Singleton<Resources>.Instance.itemIconTables.InputIconTable[0]);
        ((Text) this.Guid[1].GetComponentInChildren<Text>()).set_text("決定");
        this.Guid[1].get_gameObject().SetActive(true);
      }
      if (Object.op_Equality((Object) this.Guid[2], (Object) null))
      {
        this.Guid[2] = (GameObject) Object.Instantiate<GameObject>((M0) this.PlayGuideNode);
        this.Guid[2].get_transform().SetParent(this.PlayGuideList.get_transform(), false);
        this.Guid[2].get_transform().set_localScale(this.LocalScaleDef);
        ((Image) this.Guid[2].GetComponentInChildren<Image>()).set_sprite(Singleton<Resources>.Instance.itemIconTables.InputIconTable[1]);
        ((Text) this.Guid[2].GetComponentInChildren<Text>()).set_text("マップを閉じる");
        this.Guid[2].get_gameObject().SetActive(true);
      }
      ((Component) this.WorldMap).get_gameObject().SetActive(false);
      this.mapAction.Init();
      this.warpListUI.DisposeWarpListUI();
      this.warpListUI.Init();
      this.ActionFilter.Init(miniMap, this);
    }

    private void SetSelectGirlList()
    {
      if (this.GirlsNameList != null && this.GirlsNameList.Count > 0)
      {
        for (int index = 0; index < this.GirlsNameList.Count; ++index)
        {
          Object.Destroy((Object) this.GirlsNameList[index].get_gameObject());
          this.GirlsNameList[index] = (GameObject) null;
        }
        this.GirlsNameList.Clear();
      }
      this.sortedDic = this.miniMapcontroler.SortGirlDictionary();
      if (this.sortedDic == null)
        return;
      foreach (AgentActor agentActor in this.sortedDic.Values)
      {
        if (!Object.op_Equality((Object) agentActor, (Object) null))
        {
          GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.GirlsListNode);
          ((Image) gameObject.GetComponentInChildren<Image>()).set_sprite(Singleton<Resources>.Instance.itemIconTables.ActorIconTable[agentActor.ID]);
          ((Text) gameObject.GetComponentInChildren<Text>()).set_text(agentActor.CharaName);
          gameObject.get_gameObject().SetActive(true);
          gameObject.get_transform().SetParent(this.GirlsList.get_transform(), false);
          gameObject.get_transform().set_localScale(this.LocalScaleDef);
          this.GirlsNameList.Add(gameObject);
        }
      }
    }

    public void Refresh()
    {
      ((Component) this.WorldMap).get_gameObject().SetActive(false);
      this.RefreshWarpPointNode();
      Singleton<Input>.Instance.FocusLevel = this.mapAction.FocusLevel;
      Singleton<Input>.Instance.MenuElements = this.mapAction.MenuUIList;
      this.mapAction.DelCursor();
      this.warpListUI.DelCursor();
      this.SetSelectGirlList();
      this.ActionFilter.Refresh();
    }

    public void RefreshWarpPointNode()
    {
      this.DestroyWarpPointNode();
      List<MiniMapControler.IconInfo> baseIconInfos = this.miniMapcontroler.GetBaseIconInfos();
      if (baseIconInfos == null)
        return;
      List<MiniMapControler.IconInfo> iconInfoList = new List<MiniMapControler.IconInfo>();
      foreach (KeyValuePair<int, string> keyValuePair in Singleton<Resources>.Instance.itemIconTables.BaseName)
      {
        for (int index1 = 0; index1 < baseIconInfos.Count; ++index1)
        {
          int index2 = index1;
          BasePoint component = (BasePoint) ((Component) baseIconInfos[index2].Point).GetComponent<BasePoint>();
          if (keyValuePair.Key == component.ID)
          {
            iconInfoList.Add(new MiniMapControler.IconInfo(baseIconInfos[index2].Icon, baseIconInfos[index2].Name, baseIconInfos[index2].Point));
            break;
          }
        }
      }
      int mapId = Singleton<Manager.Map>.Instance.MapID;
      for (int index = 0; index < iconInfoList.Count; ++index)
      {
        int _index = index;
        if (((Component) iconInfoList[_index].Point).get_gameObject().get_activeSelf() && ((Component) iconInfoList[_index].Icon).get_gameObject().get_activeSelf())
        {
          bool flag = true;
          Dictionary<int, MinimapNavimesh.AreaGroupInfo> areaGroupInfo = this.miniMapcontroler.GetAreaGroupInfo(mapId);
          if (areaGroupInfo != null)
          {
            foreach (KeyValuePair<int, MinimapNavimesh.AreaGroupInfo> keyValuePair in areaGroupInfo)
            {
              int areaId = iconInfoList[_index].Point.OwnerArea.AreaID;
              if (Object.op_Inequality((Object) iconInfoList[_index].Point.OwnerArea, (Object) null) && keyValuePair.Value.areaID.Contains(areaId))
                flag = this.miniMapcontroler.RoadNaviMesh.areaGroupActive[keyValuePair.Key];
            }
          }
          Image componentInChildren = (Image) ((Component) iconInfoList[_index].Icon).GetComponentInChildren<Image>(true);
          if (!Object.op_Equality((Object) componentInChildren, (Object) null) && (((Behaviour) componentInChildren).get_enabled() || flag))
            this.AddWarpPointNode((BasePoint) ((Component) iconInfoList[_index].Point).GetComponent<BasePoint>(), _index);
        }
      }
    }

    private void AddWarpPointNode(BasePoint add, int _index)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AllAreaMapUI.\u003CAddWarpPointNode\u003Ec__AnonStorey0 nodeCAnonStorey0 = new AllAreaMapUI.\u003CAddWarpPointNode\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey0.add = add;
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey0.\u0024this = this;
      Button button = (Button) Object.Instantiate<Button>((M0) this.WarpContentNode);
      ((Component) button).get_transform().SetParent(this.WarpContent.get_transform(), false);
      ((Component) button).get_transform().set_localScale(this.LocalScaleDef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!Singleton<Resources>.Instance.itemIconTables.BaseName.TryGetValue(nodeCAnonStorey0.add.ID, out nodeCAnonStorey0.name))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        nodeCAnonStorey0.name = string.Format("拠点{0:00}", (object) nodeCAnonStorey0.add.ID);
      }
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey0.node = (WarpListNode) ((Component) button).GetComponent<WarpListNode>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey0.node.Set(nodeCAnonStorey0.add, nodeCAnonStorey0.name);
      int baseIconId = Singleton<Resources>.Instance.DefinePack.MinimapUIDefine.BaseIconID;
      bool flag = false;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (Singleton<Manager.Map>.Instance.GetBasePointOpenState(nodeCAnonStorey0.node.basePoint.ID, out flag) && flag && !nodeCAnonStorey0.node.canWarp)
      {
        // ISSUE: reference to a compiler-generated field
        nodeCAnonStorey0.node.IconSet(Singleton<Resources>.Instance.itemIconTables.ActionIconTable[baseIconId]);
      }
      // ISSUE: method pointer
      ((UnityEvent) button.get_onClick()).AddListener(new UnityAction((object) nodeCAnonStorey0, __methodptr(\u003C\u003Em__0)));
      this.enterTrigger = (PointerEnterTrigger) ((Component) button).get_gameObject().GetComponent<PointerEnterTrigger>();
      if (Object.op_Equality((Object) this.enterTrigger, (Object) null))
        this.enterTrigger = (PointerEnterTrigger) ((Component) button).get_gameObject().AddComponent<PointerEnterTrigger>();
      this.onEnter = new UITrigger.TriggerEvent();
      ((UITrigger) this.enterTrigger).get_Triggers().Add(this.onEnter);
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey0.id = this.WarpNodes.Count;
      // ISSUE: method pointer
      ((UnityEvent<BaseEventData>) this.onEnter).AddListener(new UnityAction<BaseEventData>((object) nodeCAnonStorey0, __methodptr(\u003C\u003Em__1)));
      ((Component) button).get_gameObject().SetActive(true);
      this.WarpNodes.Add(button);
    }

    private void DestroyWarpPointNode()
    {
      using (List<Button>.Enumerator enumerator = this.WarpNodes.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Button current = enumerator.Current;
          if (!Object.op_Equality((Object) ((Component) current).get_gameObject(), (Object) null))
            Object.Destroy((Object) ((Component) current).get_gameObject());
        }
      }
      this.WarpNodes.Clear();
    }

    private class ListComparer : IComparer<MiniMapControler.IconInfo>
    {
      private BasePoint pointA;
      private BasePoint pointB;

      public int Compare(MiniMapControler.IconInfo a, MiniMapControler.IconInfo b)
      {
        this.pointA = (BasePoint) ((Component) a.Point).GetComponent<BasePoint>();
        this.pointB = (BasePoint) ((Component) b.Point).GetComponent<BasePoint>();
        return this.SortCompare<int>(this.pointA.ID, this.pointB.ID);
      }

      private int SortCompare<T>(T a, T b) where T : IComparable
      {
        return a.CompareTo((object) b);
      }
    }
  }
}
