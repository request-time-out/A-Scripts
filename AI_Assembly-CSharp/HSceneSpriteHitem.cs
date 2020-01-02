// Decompiled with JetBrains decompiler
// Type: HSceneSpriteHitem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.SaveData;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEx;

public class HSceneSpriteHitem : MonoBehaviour
{
  public ScrollCylinder hSceneScroll;
  public HItemNode Node;
  public GameObject lstHItemTop;
  public Text ExplanatoryText;
  public GameObject ConfirmPanel;
  public Button Yes;
  public Button No;
  private Dictionary<int, ValueTuple<int, HSceneSpriteHitem.HitemInfo>> lstHItem;
  private Dictionary<int, bool> bHItemEffect;
  private List<HSceneSpriteHitem.HitemInfo> hitemInfos;
  private HSceneNodePool hSceneNodePool;
  private List<GameObject> pool;
  private List<ScrollCylinderNode> nodes;
  private HItemNode tmpNode;
  private Toggle toggle;
  private bool canUse;
  private int ItemId;
  private Image[] tmpImgs;
  private PointerClickTrigger PointerClickTrigger;
  private UITrigger.TriggerEvent onClick;
  private Image[] images;
  [SerializeField]
  [Tooltip("薬のCategoryIDを指定")]
  private int ItemCategory;
  private const string IconObjName = "ItemIcon";
  private const string OnCursorName = "OnCursor";

  public HSceneSpriteHitem()
  {
    base.\u002Ector();
  }

  [DebuggerHidden]
  public IEnumerator Init()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new HSceneSpriteHitem.\u003CInit\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  private void Update()
  {
    if (this.lstHItem.Count == 0 || !this.canUse)
      return;
    int ScrollId = (int) this.hSceneScroll.GetTarget().Item1;
    int itemId = this.ConvertScrollIDToItemID(ScrollId);
    if (!this.lstHItem.ContainsKey(itemId))
      return;
    // ISSUE: cast to a reference type
    // ISSUE: explicit reference operation
    this.ExplanatoryText.set_text((^(HSceneSpriteHitem.HitemInfo&) ref this.lstHItem[itemId].Item2).ExplanatoryText);
    for (int index1 = 0; index1 < this.nodes.Count; ++index1)
    {
      this.tmpImgs = (Image[]) ((Component) this.nodes[index1]).GetComponentsInChildren<Image>();
      for (int index2 = 0; index2 < this.tmpImgs.Length; ++index2)
      {
        if (!(((Object) this.tmpImgs[index2]).get_name() != "OnCursor"))
        {
          if (index1 == ScrollId && !((Toggle) ((Component) this.nodes[index1]).GetComponent<Toggle>()).get_isOn())
            ((Behaviour) this.tmpImgs[index2]).set_enabled(true);
          else
            ((Behaviour) this.tmpImgs[index2]).set_enabled(false);
        }
      }
    }
  }

  public void SetVisible(bool visible)
  {
    for (int index = 0; index < this.pool.Count; ++index)
    {
      if (index < this.lstHItem.Count)
        this.pool[index].SetActive(visible);
    }
    if (visible)
      this.hSceneScroll.ListNodeSet(this.nodes);
    if (this.canUse)
    {
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      this.ExplanatoryText.set_text((^(HSceneSpriteHitem.HitemInfo&) ref this.lstHItem[this.ConvertScrollIDToItemID((int) this.hSceneScroll.GetTarget().Item1)].Item2).ExplanatoryText);
    }
    else
      this.ExplanatoryText.set_text("Hアイテムを所持していません");
  }

  public void SetUse(int no, bool use)
  {
    if (this.pool.Count == 0)
      return;
    this.bHItemEffect[no] = use;
  }

  public bool Effect(int id)
  {
    return this.bHItemEffect.ContainsKey(id) && this.bHItemEffect[id];
  }

  public void ItemRemove()
  {
    this.canUse = true;
    this.lstHItem.Clear();
    this.hitemInfos.Clear();
    this.nodes.Clear();
    List<GameObject> list = this.hSceneNodePool.GetList();
    for (int index = 0; index < list.Count; ++index)
    {
      if (list[index].get_gameObject().get_activeSelf())
      {
        ((HItemNode) list[index].GetComponent<HItemNode>()).ScaleSet.SetActive(false);
        list[index].get_gameObject().SetActive(false);
      }
    }
    this.CheckHadItem(3, ref this.hitemInfos);
    this.CheckHadItem(5, ref this.hitemInfos);
    this.CheckHadItem(6, ref this.hitemInfos);
    this.CheckHadItem(7, ref this.hitemInfos);
    for (int index1 = 0; index1 < this.hitemInfos.Count; ++index1)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      HSceneSpriteHitem.\u003CItemRemove\u003Ec__AnonStorey2 removeCAnonStorey2 = new HSceneSpriteHitem.\u003CItemRemove\u003Ec__AnonStorey2();
      // ISSUE: reference to a compiler-generated field
      removeCAnonStorey2.\u0024this = this;
      // ISSUE: reference to a compiler-generated field
      removeCAnonStorey2.no = index1;
      this.hSceneNodePool.Get(0);
      // ISSUE: reference to a compiler-generated field
      this.tmpNode = (HItemNode) this.pool[removeCAnonStorey2.no].GetComponent<HItemNode>();
      // ISSUE: reference to a compiler-generated field
      this.tmpNode.text.set_text(this.hitemInfos[removeCAnonStorey2.no].ItemName);
      // ISSUE: reference to a compiler-generated field
      int num = this.CheckHadItemNum(this.hitemInfos[removeCAnonStorey2.no].id);
      if (num > 999)
        this.tmpNode.NumTxt.set_text("999+");
      else
        this.tmpNode.NumTxt.set_text(string.Format("{0}", (object) num));
      this.tmpNode.ScaleSet.SetActive(true);
      this.toggle = this.tmpNode.Toggle;
      this.onClick = new UITrigger.TriggerEvent();
      if (Object.op_Inequality((Object) this.toggle, (Object) null))
      {
        this.images = (Image[]) ((Component) this.toggle).GetComponentsInChildren<Image>();
        this.PointerClickTrigger = (PointerClickTrigger) ((Component) this.toggle).GetComponent<PointerClickTrigger>();
        if (Object.op_Equality((Object) this.PointerClickTrigger, (Object) null))
          this.PointerClickTrigger = (PointerClickTrigger) ((Component) this.toggle).get_gameObject().AddComponent<PointerClickTrigger>();
        ((UITrigger) this.PointerClickTrigger).get_Triggers().Clear();
        ((UITrigger) this.PointerClickTrigger).get_Triggers().Add(this.onClick);
        // ISSUE: method pointer
        ((UnityEvent<BaseEventData>) this.onClick).AddListener(new UnityAction<BaseEventData>((object) removeCAnonStorey2, __methodptr(\u003C\u003Em__0)));
        if (((Selectable) this.toggle).get_interactable())
          ((Selectable) this.toggle).set_interactable(false);
        for (int index2 = 0; index2 < this.images.Length; ++index2)
        {
          if (((Object) ((Component) this.images[index2]).get_transform()).get_name() == "ItemIcon")
          {
            // ISSUE: reference to a compiler-generated field
            Resources.ItemIconTables.SetIcon(Resources.ItemIconTables.IconCategory.Item, this.hitemInfos[removeCAnonStorey2.no].iconId, this.images[index2], false);
            if (Object.op_Inequality((Object) this.images[index2].get_sprite(), (Object) null))
              ((Behaviour) this.images[index2]).set_enabled(true);
          }
          if (!((Graphic) this.images[index2]).get_raycastTarget())
            ((Graphic) this.images[index2]).set_raycastTarget(true);
        }
      }
      this.nodes.Add((ScrollCylinderNode) this.tmpNode);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.lstHItem.Add(this.hitemInfos[removeCAnonStorey2.no].id, new ValueTuple<int, HSceneSpriteHitem.HitemInfo>(removeCAnonStorey2.no, this.hitemInfos[removeCAnonStorey2.no]));
      // ISSUE: reference to a compiler-generated field
      this.pool[removeCAnonStorey2.no].SetActive(true);
    }
    if (this.lstHItem.Count == 0)
    {
      this.canUse = false;
      this.hSceneNodePool.Get(0);
      this.lstHItem.Add(-1, new ValueTuple<int, HSceneSpriteHitem.HitemInfo>(-1, new HSceneSpriteHitem.HitemInfo()));
      this.toggle = (Toggle) this.pool[0].GetComponent<Toggle>();
      this.PointerClickTrigger = (PointerClickTrigger) this.pool[0].GetComponent<PointerClickTrigger>();
      ((UITrigger) this.PointerClickTrigger).get_Triggers().Clear();
      if (Object.op_Inequality((Object) this.toggle, (Object) null))
      {
        if (((Selectable) this.toggle).get_interactable())
          ((Selectable) this.toggle).set_interactable(false);
        this.images = (Image[]) ((Component) this.toggle).GetComponentsInChildren<Image>();
        for (int index = 0; index < this.images.Length; ++index)
        {
          if (((Object) ((Component) this.images[index]).get_transform()).get_name() == "ItemIcon")
            ((Behaviour) this.images[index]).set_enabled(false);
          if (((Graphic) this.images[index]).get_raycastTarget())
            ((Graphic) this.images[index]).set_raycastTarget(false);
        }
      }
      this.tmpNode = (HItemNode) this.pool[0].GetComponent<HItemNode>();
      this.tmpNode.text.set_text(string.Empty);
      this.nodes.Add((ScrollCylinderNode) this.tmpNode);
    }
    ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.NextFrame((FrameCountType) 0), (Action<M0>) (_ =>
    {
      this.hSceneScroll.ListNodeSet(this.nodes);
      if (this.canUse)
      {
        int itemId = this.ConvertScrollIDToItemID((int) this.hSceneScroll.GetTarget().Item1);
        if (!this.lstHItem.ContainsKey(itemId))
          return;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        this.ExplanatoryText.set_text((^(HSceneSpriteHitem.HitemInfo&) ref this.lstHItem[itemId].Item2).ExplanatoryText);
      }
      else
        this.ExplanatoryText.set_text("Hアイテムを所持していません");
    }));
  }

  protected void RemoveItem(int _ID)
  {
    int index1 = -1;
    List<StuffItem> itemList = Singleton<Manager.Map>.Instance.Player.PlayerData.ItemList;
    for (int index2 = 0; index2 < itemList.Count; ++index2)
    {
      if (itemList[index2].CategoryID == this.ItemCategory && itemList[index2].ID == _ID)
      {
        index1 = index2;
        break;
      }
    }
    if (index1 != -1)
    {
      StuffItem stuffItem = itemList[index1];
      Singleton<Manager.Map>.Instance.Player.PlayerData.ItemList.RemoveItem(new StuffItem(stuffItem.CategoryID, stuffItem.ID, 1));
    }
    else
    {
      List<StuffItem> itemListInStorage = Singleton<Game>.Instance.Environment.ItemListInStorage;
      for (int index2 = 0; index2 < itemListInStorage.Count; ++index2)
      {
        if (itemListInStorage[index2].CategoryID == this.ItemCategory && itemListInStorage[index2].ID == _ID)
        {
          index1 = index2;
          break;
        }
      }
      StuffItem stuffItem1 = itemListInStorage[index1];
      StuffItem stuffItem2 = new StuffItem(stuffItem1.CategoryID, stuffItem1.ID, 1);
      itemListInStorage.RemoveItem(stuffItem2);
    }
    ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.NextFrame((FrameCountType) 0), (Action<M0>) (_ => this.ItemRemove()));
  }

  private bool CheckHadItem(int id, ref List<HSceneSpriteHitem.HitemInfo> hitemInfos)
  {
    bool flag = this.CheckHadItem(id, 0, ref hitemInfos);
    if (!flag)
      flag = this.CheckHadItem(id, 1, ref hitemInfos);
    return flag;
  }

  private bool CheckHadItem(int id, int mode, ref List<HSceneSpriteHitem.HitemInfo> hitemInfos)
  {
    foreach (StuffItem stuffItem in mode != 0 ? Singleton<Game>.Instance.Environment.ItemListInStorage : Singleton<Manager.Map>.Instance.Player.PlayerData.ItemList)
    {
      if (stuffItem.CategoryID == this.ItemCategory && stuffItem.ID == id)
      {
        StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(stuffItem.CategoryID, id);
        if (stuffItemInfo != null)
        {
          HSceneSpriteHitem.HitemInfo hitemInfo;
          hitemInfo.id = stuffItemInfo.ID;
          hitemInfo.iconId = stuffItemInfo.IconID;
          hitemInfo.ItemName = stuffItemInfo.Name;
          hitemInfo.ExplanatoryText = stuffItemInfo.Explanation;
          hitemInfos.Add(hitemInfo);
          return true;
        }
      }
    }
    return false;
  }

  private int CheckHadItemNum(int id)
  {
    int num = 0;
    foreach (StuffItem stuffItem in Singleton<Manager.Map>.Instance.Player.PlayerData.ItemList)
    {
      if (stuffItem.CategoryID == this.ItemCategory && stuffItem.ID == id)
      {
        num = stuffItem.Count;
        break;
      }
    }
    foreach (StuffItem stuffItem in Singleton<Game>.Instance.Environment.ItemListInStorage)
    {
      if (stuffItem.CategoryID == this.ItemCategory && stuffItem.ID == id)
      {
        num += stuffItem.Count;
        break;
      }
    }
    return num;
  }

  private int ConvertScrollIDToItemID(int ScrollId)
  {
    using (Dictionary<int, ValueTuple<int, HSceneSpriteHitem.HitemInfo>>.Enumerator enumerator = this.lstHItem.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        KeyValuePair<int, ValueTuple<int, HSceneSpriteHitem.HitemInfo>> current = enumerator.Current;
        if (current.Value.Item1 == ScrollId)
          return current.Key;
      }
    }
    return -1;
  }

  public void EndProc()
  {
    this.hSceneScroll.ClearBlank();
    List<GameObject> list = this.hSceneNodePool.GetList();
    for (int index1 = 0; index1 < list.Count; ++index1)
    {
      int index2 = index1;
      Object.Destroy((Object) list[index2].get_gameObject());
    }
    list.Clear();
    ((UnityEventBase) this.Yes.get_onClick()).RemoveAllListeners();
    ((UnityEventBase) this.No.get_onClick()).RemoveAllListeners();
    if (!this.ConfirmPanel.get_activeSelf())
      return;
    this.ConfirmPanel.SetActive(false);
  }

  public struct HitemInfo
  {
    public int id;
    public int iconId;
    public string ItemName;
    public string ExplanatoryText;
  }
}
