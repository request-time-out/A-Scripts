// Decompiled with JetBrains decompiler
// Type: HSceneSpriteCoordinatesCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using AIProject.ColorDefine;
using CharaCustom;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEx;

public class HSceneSpriteCoordinatesCard : MonoBehaviour
{
  [SerializeField]
  private HSceneSpriteChaChoice hSceneSpriteChaChoice;
  [SerializeField]
  private HSceneSpriteClothCondition hSceneSpriteCloth;
  [SerializeField]
  private RawImage CardImage;
  [SerializeField]
  private Text SelectedLabel;
  private string filename;
  [SerializeField]
  private Button Sort;
  [SerializeField]
  private Button[] SortUpDown;
  [SerializeField]
  private GameObject SortPanel;
  [SerializeField]
  private HSceneSpriteCoordinatesNode CoordinatesNode;
  [SerializeField]
  private Transform Content;
  [SerializeField]
  private Button cross;
  [SerializeField]
  private List<Toggle> lstSortCategory;
  private List<HSceneSpriteCoordinatesNode> lstCoordinates;
  private List<CustomClothesFileInfo> lstCoordinatesBase;
  [SerializeField]
  private Button BeforeCoode;
  [SerializeField]
  private Button DecideCoode;
  private int sortKind;
  private bool Ascending;
  private HScene hScene;
  private HSceneManager hSceneManager;
  private ChaControl[] femailes;
  private IntReactiveProperty SelectedID;

  public HSceneSpriteCoordinatesCard()
  {
    base.\u002Ector();
  }

  private int _SelectedID
  {
    set
    {
      ((ReactiveProperty<int>) this.SelectedID).set_Value(value);
    }
    get
    {
      return ((ReactiveProperty<int>) this.SelectedID).get_Value();
    }
  }

  private void Start()
  {
    ObservableExtensions.Subscribe<int>(Observable.Where<int>((IObservable<M0>) this.SelectedID, (Func<M0, bool>) (x => x >= 0 && x < this.lstCoordinates.Count)), (Action<M0>) (x =>
    {
      for (int index = 0; index < this.lstCoordinates.Count; ++index)
      {
        if (this.lstCoordinates[index].id == x)
        {
          this.SelectedLabel.set_text(this.lstCoordinates[index].coodeName.get_text());
          this.filename = this.lstCoordinates[index].fileName;
          this.CardImage.set_texture((Texture) PngAssist.ChangeTextureFromByte(this.lstCoordinatesBase[x].pngData == null ? PngFile.LoadPngBytes(this.lstCoordinatesBase[x].FullPath) : this.lstCoordinatesBase[x].pngData, 0, 0, (TextureFormat) 5, false));
        }
      }
      if (!((Component) this.CardImage).get_gameObject().get_activeSelf())
        ((Component) this.CardImage).get_gameObject().SetActive(true);
      for (int index = 0; index < this.lstCoordinates.Count; ++index)
      {
        if (this.lstCoordinates[index].id != x)
          ((Graphic) this.lstCoordinates[index].image).set_color(Color.get_white());
        else
          ((Graphic) this.lstCoordinates[index].image).set_color(Define.Get(Colors.Yellow));
      }
    }));
  }

  public void Init()
  {
    this.hScene = (HScene) ((Component) Singleton<HSceneFlagCtrl>.Instance).GetComponent<HScene>();
    this.femailes = this.hScene.GetFemales();
    this.hSceneManager = Singleton<HSceneManager>.Instance;
    // ISSUE: method pointer
    ((UnityEvent) this.Sort.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__2)));
    // ISSUE: method pointer
    ((UnityEvent) this.SortUpDown[0].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__3)));
    // ISSUE: method pointer
    ((UnityEvent) this.SortUpDown[1].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__4)));
    // ISSUE: method pointer
    ((UnityEvent) this.cross.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__5)));
    this.lstCoordinatesBase = CustomClothesFileInfoAssist.CreateClothesFileInfoList(false, true, true, true);
    this.lstCoordinates.Clear();
    for (int index = 0; index < this.lstCoordinatesBase.Count; ++index)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      HSceneSpriteCoordinatesCard.\u003CInit\u003Ec__AnonStorey0 initCAnonStorey0 = new HSceneSpriteCoordinatesCard.\u003CInit\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      initCAnonStorey0.\u0024this = this;
      // ISSUE: reference to a compiler-generated field
      initCAnonStorey0.no = index;
      HSceneSpriteCoordinatesNode spriteCoordinatesNode = (HSceneSpriteCoordinatesNode) Object.Instantiate<HSceneSpriteCoordinatesNode>((M0) this.CoordinatesNode, this.Content);
      ((Component) spriteCoordinatesNode).get_gameObject().SetActive(true);
      this.lstCoordinates.Add(spriteCoordinatesNode);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.lstCoordinates[initCAnonStorey0.no].id = initCAnonStorey0.no;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.lstCoordinates[initCAnonStorey0.no].coodeName.set_text(this.lstCoordinatesBase[initCAnonStorey0.no].name);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.lstCoordinates[initCAnonStorey0.no].CreateCoodeTime = this.lstCoordinatesBase[initCAnonStorey0.no].time;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      ((UnityEvent) ((Button) ((Component) this.lstCoordinates[initCAnonStorey0.no]).GetComponent<Button>()).get_onClick()).AddListener(new UnityAction((object) initCAnonStorey0, __methodptr(\u003C\u003Em__0)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.lstCoordinates[initCAnonStorey0.no].image = (Image) ((Component) this.lstCoordinates[initCAnonStorey0.no]).GetComponent<Image>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.lstCoordinates[initCAnonStorey0.no].fileName = this.lstCoordinatesBase[initCAnonStorey0.no].FileName;
    }
    PointerEnterTrigger pointerEnterTrigger1 = (PointerEnterTrigger) null;
    UITrigger.TriggerEvent triggerEvent1 = new UITrigger.TriggerEvent();
    PointerExitTrigger pointerExitTrigger1 = (PointerExitTrigger) null;
    UITrigger.TriggerEvent triggerEvent2 = new UITrigger.TriggerEvent();
    for (int index = 0; index < this.lstSortCategory.Count; ++index)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      HSceneSpriteCoordinatesCard.\u003CInit\u003Ec__AnonStorey1 initCAnonStorey1 = new HSceneSpriteCoordinatesCard.\u003CInit\u003Ec__AnonStorey1();
      // ISSUE: reference to a compiler-generated field
      initCAnonStorey1.\u0024this = this;
      // ISSUE: reference to a compiler-generated field
      initCAnonStorey1.no = index;
      // ISSUE: reference to a compiler-generated field
      ((UnityEventBase) this.lstSortCategory[initCAnonStorey1.no].onValueChanged).RemoveAllListeners();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.lstSortCategory[initCAnonStorey1.no].onValueChanged).AddListener(new UnityAction<bool>((object) initCAnonStorey1, __methodptr(\u003C\u003Em__0)));
      pointerEnterTrigger1 = (PointerEnterTrigger) null;
      UITrigger.TriggerEvent triggerEvent3 = new UITrigger.TriggerEvent();
      // ISSUE: reference to a compiler-generated field
      PointerEnterTrigger pointerEnterTrigger2 = (PointerEnterTrigger) ((Component) this.lstSortCategory[initCAnonStorey1.no]).get_gameObject().GetComponent<PointerEnterTrigger>();
      if (Object.op_Equality((Object) pointerEnterTrigger2, (Object) null))
      {
        // ISSUE: reference to a compiler-generated field
        pointerEnterTrigger2 = (PointerEnterTrigger) ((Component) this.lstSortCategory[initCAnonStorey1.no]).get_gameObject().AddComponent<PointerEnterTrigger>();
      }
      if (((UITrigger) pointerEnterTrigger2).get_Triggers().Count > 0)
        ((UITrigger) pointerEnterTrigger2).get_Triggers().Clear();
      ((UITrigger) pointerEnterTrigger2).get_Triggers().Add(triggerEvent3);
      // ISSUE: method pointer
      ((UnityEvent<BaseEventData>) triggerEvent3).AddListener(new UnityAction<BaseEventData>((object) initCAnonStorey1, __methodptr(\u003C\u003Em__1)));
      pointerExitTrigger1 = (PointerExitTrigger) null;
      UITrigger.TriggerEvent triggerEvent4 = new UITrigger.TriggerEvent();
      // ISSUE: reference to a compiler-generated field
      PointerExitTrigger pointerExitTrigger2 = (PointerExitTrigger) ((Component) this.lstSortCategory[initCAnonStorey1.no]).get_gameObject().GetComponent<PointerExitTrigger>();
      if (Object.op_Equality((Object) pointerExitTrigger2, (Object) null))
      {
        // ISSUE: reference to a compiler-generated field
        pointerExitTrigger2 = (PointerExitTrigger) ((Component) this.lstSortCategory[initCAnonStorey1.no]).get_gameObject().AddComponent<PointerExitTrigger>();
      }
      if (((UITrigger) pointerExitTrigger2).get_Triggers().Count > 0)
        ((UITrigger) pointerExitTrigger2).get_Triggers().Clear();
      ((UITrigger) pointerExitTrigger2).get_Triggers().Add(triggerEvent4);
      // ISSUE: method pointer
      ((UnityEvent<BaseEventData>) triggerEvent4).AddListener(new UnityAction<BaseEventData>((object) initCAnonStorey1, __methodptr(\u003C\u003Em__2)));
    }
    this.ListSort(0);
    // ISSUE: method pointer
    ((UnityEvent) this.BeforeCoode.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__6)));
    // ISSUE: method pointer
    ((UnityEvent) this.DecideCoode.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__7)));
    // ISSUE: method pointer
    this.hSceneSpriteChaChoice.SetAction(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__8)));
  }

  private void SetCoordinatesCharacter()
  {
    this._SelectedID = -1;
  }

  public void ListSort(int sortkind)
  {
    HSceneSpriteCoordinatesCard.ListComparer listComparer = new HSceneSpriteCoordinatesCard.ListComparer();
    this.sortKind = sortkind;
    listComparer.nCompare = sortkind;
    listComparer.bAscending = this.Ascending;
    if (this.lstCoordinates == null || this.lstCoordinates.Count == 0)
      return;
    this.lstCoordinates.Sort((IComparer<HSceneSpriteCoordinatesNode>) listComparer);
    for (int index = 0; index < this.lstCoordinates.Count; ++index)
    {
      int num = index;
      ((Component) this.lstCoordinates[index]).get_transform().SetSiblingIndex(num);
    }
  }

  public void ListSortUpDown(int Ascending)
  {
    HSceneSpriteCoordinatesCard.ListComparer listComparer = new HSceneSpriteCoordinatesCard.ListComparer();
    this.Ascending = Ascending == 0;
    listComparer.nCompare = this.sortKind;
    listComparer.bAscending = this.Ascending;
    if (this.lstCoordinates == null || this.lstCoordinates.Count == 0)
      return;
    this.lstCoordinates.Sort((IComparer<HSceneSpriteCoordinatesNode>) listComparer);
    for (int index = 0; index < this.lstCoordinates.Count; ++index)
    {
      int num = index;
      ((Component) this.lstCoordinates[index]).get_transform().SetSiblingIndex(num);
    }
  }

  public void EndProc()
  {
    for (int index = 0; index < this.lstCoordinates.Count; ++index)
      Object.Destroy((Object) ((Component) this.lstCoordinates[index]).get_gameObject());
    this.lstCoordinates.Clear();
    ((UnityEventBase) this.Sort.get_onClick()).RemoveAllListeners();
    ((UnityEventBase) this.SortUpDown[0].get_onClick()).RemoveAllListeners();
    ((UnityEventBase) this.SortUpDown[1].get_onClick()).RemoveAllListeners();
    ((UnityEventBase) this.cross.get_onClick()).RemoveAllListeners();
    ((UnityEventBase) this.BeforeCoode.get_onClick()).RemoveAllListeners();
    ((UnityEventBase) this.DecideCoode.get_onClick()).RemoveAllListeners();
    this.CloseSort();
  }

  private void OnDisable()
  {
    this.CloseSort();
  }

  public void CloseSort()
  {
    this.SortPanel.SetActive(false);
  }

  private class ListComparer : IComparer<HSceneSpriteCoordinatesNode>
  {
    public int nCompare;
    public bool bAscending;

    public int Compare(HSceneSpriteCoordinatesNode a, HSceneSpriteCoordinatesNode b)
    {
      switch (this.nCompare)
      {
        case 0:
          return this.bAscending ? this.SortCompare<string>(a.coodeName.get_text(), b.coodeName.get_text()) : this.SortCompare<string>(b.coodeName.get_text(), a.coodeName.get_text());
        case 1:
          return this.bAscending ? this.SortCompare<DateTime>(a.CreateCoodeTime, b.CreateCoodeTime) : this.SortCompare<DateTime>(b.CreateCoodeTime, a.CreateCoodeTime);
        default:
          return 0;
      }
    }

    private int SortCompare<T>(T a, T b) where T : IComparable
    {
      return a.CompareTo((object) b);
    }
  }
}
