// Decompiled with JetBrains decompiler
// Type: CraftItemBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.UI;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftItemBox : MenuUIBehaviour
{
  public Dictionary<int, int> playerstrage = new Dictionary<int, int>();
  private BuildPartsRecipe[] buildPartsRecipes = new BuildPartsRecipe[3];
  [SerializeField]
  private BoolReactiveProperty _EndLoad = new BoolReactiveProperty(false);
  private Vector3 _velocity = Vector3.get_zero();
  private bool bAlphaAdd = true;
  public GameObject[] ItemKind;
  private GameObject[] LockImages;
  [SerializeField]
  private GameObject unLockPanel;
  [SerializeField]
  private Text[] resipeMatName;
  [SerializeField]
  private Image[] resipeMatImage;
  [SerializeField]
  private Text[] resipeHavingNum;
  [SerializeField]
  private Text[] resipeNeedNum;
  [SerializeField]
  private GameObject[] resipe;
  [SerializeField]
  private Button unLockButton;
  [SerializeField]
  private Button cancelButton;
  public BuildPartsMgr buildPartsMgr;
  public ScrollRect scroll;
  private RectTransform SelectedRectTransform;
  public GameObject itemButton;
  public Transform scrollView;
  public GameObject carsol;
  [SerializeField]
  private CraftControler craftControler;
  private int selectedID;
  private Color color;
  private MenuUIBehaviour[] _menuUIList;
  private bool _prevEndLoad;
  private Input inputManager;
  private float _vel;
  private float _alphaVelocity;

  protected MenuUIBehaviour[] MenuUIList
  {
    get
    {
      MenuUIBehaviour[] menuUiList = this._menuUIList;
      if (menuUiList != null)
        return menuUiList;
      return this._menuUIList = new MenuUIBehaviour[1]
      {
        (MenuUIBehaviour) this
      };
    }
  }

  public Action<MoveDirection> OnMove { get; set; }

  public bool isActive
  {
    get
    {
      return ((ReactiveProperty<bool>) this._isActive).get_Value();
    }
    set
    {
      ((ReactiveProperty<bool>) this._isActive).set_Value(value);
    }
  }

  public bool EndLoad
  {
    get
    {
      return ((ReactiveProperty<bool>) this._EndLoad).get_Value();
    }
    set
    {
      ((ReactiveProperty<bool>) this._EndLoad).set_Value(value);
    }
  }

  protected override void Start()
  {
    this.inputManager = Singleton<Input>.Instance;
    this.selectedID = 0;
    ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._isActive, (Action<M0>) (x => this.SetActiveControl(x)));
    ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this._EndLoad, (Func<M0, bool>) (x => x)), (Action<M0>) (x => this.ChangeList(1)));
    this.OnMove = (Action<MoveDirection>) (x => this.SelectMove(x));
    if (Object.op_Inequality((Object) this.carsol, (Object) null))
    {
      this.color.r = ((Graphic) this.carsol.GetComponent<Image>()).get_color().r;
      this.color.g = ((Graphic) this.carsol.GetComponent<Image>()).get_color().g;
      this.color.b = ((Graphic) this.carsol.GetComponent<Image>()).get_color().b;
      this.color.a = ((Graphic) this.carsol.GetComponent<Image>()).get_color().a;
    }
    ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnUpdate()));
    this.playerstrage.Add(0, 1);
    for (int index = 0; index < this.buildPartsRecipes.Length; ++index)
    {
      this.buildPartsRecipes[index].MatName = this.resipeMatName[index];
      this.buildPartsRecipes[index].Image = this.resipeMatImage[index];
      this.buildPartsRecipes[index].HavingNum = this.resipeHavingNum[index];
      this.buildPartsRecipes[index].NeedNum = this.resipeNeedNum[index];
      this.buildPartsRecipes[index].root = this.resipe[index];
    }
  }

  private void SetActiveControl(bool isActive)
  {
    if (isActive)
    {
      this.carsol.SetActive(true);
      this.selectedID = 0;
      ((Selectable) this.ItemKind[this.selectedID].GetComponent<Button>()).Select();
    }
    else
    {
      this.carsol.SetActive(false);
      this.inputManager.ClearMenuElements();
      this.inputManager.FocusLevel = -1;
      this.inputManager.ReserveState(Input.ValidType.Action);
      this.inputManager.SetupState();
    }
  }

  private void OnUpdate()
  {
    if (this.ItemKind == null || this.ItemKind.Length == 0 || (Object.op_Equality((Object) this.ItemKind[0], (Object) null) || !this.isActive))
      return;
    if (this.bAlphaAdd)
    {
      this.color.a = (__Null) (double) Mathf.SmoothDamp((float) this.color.a, 0.3921569f, ref this._alphaVelocity, 0.00095f, float.PositiveInfinity, Time.get_unscaledDeltaTime());
      if (this.color.a == 0.39215686917305)
      {
        CraftItemBox craftItemBox = this;
        craftItemBox.bAlphaAdd = ((craftItemBox.bAlphaAdd ? 1 : 0) ^ 1) != 0;
      }
    }
    else
    {
      this.color.a = (__Null) (double) Mathf.SmoothDamp((float) this.color.a, 0.0f, ref this._alphaVelocity, 0.00095f, float.PositiveInfinity, Time.get_unscaledDeltaTime());
      if (this.color.a == 0.0)
      {
        CraftItemBox craftItemBox = this;
        craftItemBox.bAlphaAdd = ((craftItemBox.bAlphaAdd ? 1 : 0) ^ 1) != 0;
      }
    }
    ((Graphic) this.carsol.GetComponent<Image>()).set_color(this.color);
    GameObject selectedGameObject = EventSystem.get_current().get_currentSelectedGameObject();
    if (Object.op_Equality((Object) selectedGameObject, (Object) null) || Object.op_Inequality((Object) selectedGameObject.get_transform().get_parent(), (Object) ((Component) this.scrollView).get_transform()))
      return;
    this.SelectedRectTransform = (RectTransform) selectedGameObject.GetComponent<RectTransform>();
    RectTransform component = (RectTransform) ((Component) this.scrollView).GetComponent<RectTransform>();
    Rect rect1 = this.SelectedRectTransform.get_rect();
    double width1 = (double) ((Rect) ref rect1).get_width();
    Rect rect2 = component.get_rect();
    double width2 = (double) ((Rect) ref rect2).get_width();
    float num1 = (float) (width1 / width2);
    if (this.carsol.get_transform().get_localPosition().x >= 1000.0)
    {
      float num2 = this.scroll.get_horizontalNormalizedPosition() + num1;
      if ((double) num2 >= 1.0)
        num2 = 1f;
      this.scroll.set_horizontalNormalizedPosition(Mathf.SmoothDamp(this.scroll.get_horizontalNormalizedPosition(), num2, ref this._vel, this._followAccelerationTime, float.PositiveInfinity, Time.get_unscaledDeltaTime()));
    }
    else if (this.carsol.get_transform().get_localPosition().x < 0.0)
    {
      float num2 = this.scroll.get_horizontalNormalizedPosition() - num1;
      if ((double) num2 <= 0.0)
        num2 = 0.0f;
      this.scroll.set_horizontalNormalizedPosition(Mathf.SmoothDamp(this.scroll.get_horizontalNormalizedPosition(), num2, ref this._vel, this._followAccelerationTime, float.PositiveInfinity, Time.get_unscaledDeltaTime()));
    }
    this.carsol.get_transform().set_position(Vector3.SmoothDamp(this.carsol.get_transform().get_position(), this.ItemKind[this.selectedID].get_transform().get_position(), ref this._velocity, this._followAccelerationTime, float.PositiveInfinity, Time.get_unscaledDeltaTime()));
  }

  private void ChangeItemKind(int formKind, int nID)
  {
    this.craftControler._nPartsForm = formKind;
    this.craftControler._nID = nID;
    this.craftControler.ChangeParts();
  }

  public void ChangeList(int id)
  {
    if (!this._prevEndLoad)
      this._prevEndLoad = true;
    for (int index = 0; index < this.ItemKind.Length; ++index)
    {
      if (!Object.op_Equality((Object) this.ItemKind[index], (Object) null))
      {
        Object.Destroy((Object) this.ItemKind[index]);
        this.ItemKind[index] = (GameObject) null;
        Object.Destroy((Object) this.LockImages[index]);
        this.LockImages[index] = (GameObject) null;
      }
    }
    int key = 0;
    Dictionary<int, Tuple<int, int>> dictionary = new Dictionary<int, Tuple<int, int>>();
    List<BuildPartsPool>[] baseParts = Singleton<CraftCommandListBaseObject>.Instance.BaseParts;
    for (int index1 = 0; index1 < baseParts.Length; ++index1)
    {
      for (int index2 = 0; index2 < baseParts[index1].Count; ++index2)
      {
        if (baseParts[index1][index2].GetCategoryKind() == id)
        {
          dictionary.Add(key, new Tuple<int, int>(index1, index2));
          ++key;
        }
      }
    }
    this.ItemKind = new GameObject[key];
    this.LockImages = new GameObject[key];
    for (int index = 0; index < this.ItemKind.Length; ++index)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CraftItemBox.\u003CChangeList\u003Ec__AnonStorey0 listCAnonStorey0 = new CraftItemBox.\u003CChangeList\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      listCAnonStorey0.\u0024this = this;
      this.ItemKind[index] = (GameObject) Object.Instantiate<GameObject>((M0) this.itemButton);
      this.ItemKind[index].get_transform().SetParent(this.scrollView, false);
      // ISSUE: reference to a compiler-generated field
      listCAnonStorey0.nForm = dictionary[index].Item1;
      // ISSUE: reference to a compiler-generated field
      listCAnonStorey0.nPool = dictionary[index].Item2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      bool flag = Singleton<CraftCommandListBaseObject>.Instance.BaseParts[listCAnonStorey0.nForm][listCAnonStorey0.nPool].CheckLock();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ((Text) this.ItemKind[index].GetComponentInChildren<Text>()).set_text(this.buildPartsMgr.BuildPartPoolDic[listCAnonStorey0.nForm][listCAnonStorey0.nPool].Item2.Name);
      // ISSUE: reference to a compiler-generated field
      listCAnonStorey0.button = (Button) this.ItemKind[index].GetComponent<Button>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      ((UnityEvent) listCAnonStorey0.button.get_onClick()).AddListener(new UnityAction((object) listCAnonStorey0, __methodptr(\u003C\u003Em__0)));
      if (index == 0)
      {
        // ISSUE: reference to a compiler-generated field
        ((Selectable) listCAnonStorey0.button).Select();
      }
      this.ItemKind[index].SetActive(true);
      this.LockImages[index] = ((Component) this.ItemKind[index].get_transform().GetChild(1)).get_gameObject();
      if (flag)
        this.LockImages[index].SetActive(true);
    }
    this.inputManager.FocusLevel = 0;
    this.inputManager.MenuElements = this.MenuUIList;
    this.inputManager.ReserveState(Input.ValidType.UI);
    this.inputManager.SetupState();
  }

  private void SelectMove(MoveDirection moveDir)
  {
    if (moveDir != 2)
    {
      if (moveDir == null)
      {
        int num = this.selectedID - 1;
        if (num < 0)
          num = this.ItemKind.Length - 1;
        this.selectedID = num;
      }
    }
    else
    {
      int num = this.selectedID + 1;
      if (num >= this.ItemKind.Length)
        num = 0;
      this.selectedID = num;
    }
    ((Selectable) this.ItemKind[this.selectedID].GetComponent<Button>()).Select();
  }

  private void Clicked(int nForm, int nPool, Button clickedButton)
  {
    if (!Singleton<CraftCommandListBaseObject>.Instance.BaseParts[nForm][nPool].CheckLock())
    {
      this.ChangeItemKind(nForm, nPool);
      this.isActive = false;
    }
    else
      this.SetUnLockPanel(nForm, nPool, clickedButton);
  }

  private void SetUnLockPanel(int nForm, int nPool, Button clickedButton)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    CraftItemBox.\u003CSetUnLockPanel\u003Ec__AnonStorey1 panelCAnonStorey1 = new CraftItemBox.\u003CSetUnLockPanel\u003Ec__AnonStorey1();
    // ISSUE: reference to a compiler-generated field
    panelCAnonStorey1.nForm = nForm;
    // ISSUE: reference to a compiler-generated field
    panelCAnonStorey1.nPool = nPool;
    // ISSUE: reference to a compiler-generated field
    panelCAnonStorey1.clickedButton = clickedButton;
    // ISSUE: reference to a compiler-generated field
    panelCAnonStorey1.\u0024this = this;
    this.unLockPanel.SetActive(true);
    bool flag = true;
    for (int index = 0; index < this.buildPartsRecipes.Length; ++index)
      this.buildPartsRecipes[index].root.SetActive(false);
    int num1 = 0;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    for (int index = 0; index < this.buildPartsMgr.BuildPartPoolDic[panelCAnonStorey1.nForm][panelCAnonStorey1.nPool].Item2.recipe.Length && this.buildPartsMgr.BuildPartPoolDic[panelCAnonStorey1.nForm][panelCAnonStorey1.nPool].Item2.recipe[index].Item2 > 0; ++index)
      ++num1;
    for (int index = 0; index < num1; ++index)
    {
      int num2 = 0;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int num3 = this.buildPartsMgr.BuildPartPoolDic[panelCAnonStorey1.nForm][panelCAnonStorey1.nPool].Item2.recipe[index].Item2;
      this.buildPartsRecipes[index].HavingNum.set_text(string.Format("{0}", (object) num2));
      this.buildPartsRecipes[index].NeedNum.set_text(string.Format("／ {0}", (object) num3));
      ((Graphic) this.buildPartsRecipes[index].HavingNum).set_color(Color.get_white());
      this.buildPartsRecipes[index].root.SetActive(true);
      if (num2 < num3)
      {
        ((Graphic) this.buildPartsRecipes[index].HavingNum).set_color(Color.get_red());
        flag = false;
      }
    }
    if (!flag)
      ((Selectable) this.unLockButton).set_interactable(false);
    if (this.unLockButton != null)
    {
      // ISSUE: method pointer
      ((UnityEvent) this.unLockButton.get_onClick()).AddListener(new UnityAction((object) panelCAnonStorey1, __methodptr(\u003C\u003Em__0)));
    }
    if (this.cancelButton == null)
      return;
    // ISSUE: method pointer
    ((UnityEvent) this.cancelButton.get_onClick()).AddListener(new UnityAction((object) panelCAnonStorey1, __methodptr(\u003C\u003Em__1)));
  }

  private void UnLock(int nForm, int nPool, Button clickedButton)
  {
    Singleton<CraftCommandListBaseObject>.Instance.BaseParts[nForm][nPool].UnLock();
    this.unLockPanel.SetActive(false);
    ((Component) ((Component) clickedButton).get_transform().GetChild(1)).get_gameObject().SetActive(false);
  }

  private void close()
  {
    this.isActive = false;
  }

  public override void OnInputMoveDirection(MoveDirection moveDir)
  {
    this.OnMove(moveDir);
  }
}
