// Decompiled with JetBrains decompiler
// Type: CraftItemButtonUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.UI;
using Manager;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftItemButtonUI : MenuUIBehaviour
{
  private Vector3 _velocity = Vector3.get_zero();
  private bool bAlphaAdd = true;
  public Button[] ItemKind;
  public Button ItemKindButton;
  public ScrollRect scroll;
  [SerializeField]
  private RectTransform rectTransform;
  [SerializeField]
  private RectTransform ContentsRectTransform;
  private RectTransform SelectedRectTransform;
  private int selectedID;
  private MenuUIBehaviour[] _menuUIList;
  [SerializeField]
  private GameObject carsolPanel;
  private Color color;
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

  protected override void Start()
  {
    ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._isActive, (Action<M0>) (x => this.SetActiveControl(x)));
    this.OnMove = (Action<MoveDirection>) (x => this.SelectMove(x));
    if (Object.op_Inequality((Object) this.carsolPanel, (Object) null))
      this.color = ((Graphic) this.carsolPanel.GetComponent<Image>()).get_color();
    ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled() && this.carsolPanel.get_activeSelf())), (Action<M0>) (_ => this.OnUpdate()));
  }

  private void OnUpdate()
  {
    if (this.bAlphaAdd)
    {
      this.color.a = (__Null) (double) Mathf.SmoothDamp((float) this.color.a, 0.3921569f, ref this._alphaVelocity, 0.00095f, float.PositiveInfinity, Time.get_unscaledDeltaTime());
      if (this.color.a == 0.39215686917305)
      {
        CraftItemButtonUI craftItemButtonUi = this;
        craftItemButtonUi.bAlphaAdd = ((craftItemButtonUi.bAlphaAdd ? 1 : 0) ^ 1) != 0;
      }
    }
    else
    {
      this.color.a = (__Null) (double) Mathf.SmoothDamp((float) this.color.a, 0.0f, ref this._alphaVelocity, 0.00095f, float.PositiveInfinity, Time.get_unscaledDeltaTime());
      if (this.color.a == 0.0)
      {
        CraftItemButtonUI craftItemButtonUi = this;
        craftItemButtonUi.bAlphaAdd = ((craftItemButtonUi.bAlphaAdd ? 1 : 0) ^ 1) != 0;
      }
    }
    ((Graphic) this.carsolPanel.GetComponent<Image>()).set_color(this.color);
    GameObject selectedGameObject = EventSystem.get_current().get_currentSelectedGameObject();
    if (Object.op_Equality((Object) selectedGameObject, (Object) null) || Object.op_Inequality((Object) selectedGameObject.get_transform().get_parent(), (Object) ((Component) this.ContentsRectTransform).get_transform()))
      return;
    this.SelectedRectTransform = (RectTransform) selectedGameObject.GetComponent<RectTransform>();
    Vector3 vector3 = Vector3.op_Subtraction(((Transform) this.rectTransform).get_localPosition(), ((Transform) this.SelectedRectTransform).get_localPosition());
    Rect rect1 = this.ContentsRectTransform.get_rect();
    double height1 = (double) ((Rect) ref rect1).get_height();
    Rect rect2 = this.rectTransform.get_rect();
    double height2 = (double) ((Rect) ref rect2).get_height();
    float num1 = (float) (height1 - height2);
    Rect rect3 = this.ContentsRectTransform.get_rect();
    float num2 = ((Rect) ref rect3).get_height() - (float) vector3.y;
    float num3 = (float) this.scroll.get_normalizedPosition().y * num1;
    double num4 = (double) num3;
    Rect rect4 = this.SelectedRectTransform.get_rect();
    double num5 = (double) ((Rect) ref rect4).get_height() / 2.0;
    double num6 = num4 - num5;
    Rect rect5 = this.rectTransform.get_rect();
    double height3 = (double) ((Rect) ref rect5).get_height();
    float num7 = (float) (num6 + height3);
    double num8 = (double) num3;
    Rect rect6 = this.SelectedRectTransform.get_rect();
    double num9 = (double) ((Rect) ref rect6).get_height() / 2.0;
    float num10 = (float) (num8 + num9);
    this.carsolPanel.get_transform().set_position(Vector3.SmoothDamp(this.carsolPanel.get_transform().get_position(), ((Component) this.ItemKind[this.selectedID]).get_transform().get_position(), ref this._velocity, this._followAccelerationTime, float.PositiveInfinity, Time.get_unscaledDeltaTime()));
    if ((double) num2 > (double) num7)
    {
      float num11 = num2 - num7;
      this.scroll.set_verticalNormalizedPosition(Mathf.SmoothDamp(this.scroll.get_verticalNormalizedPosition(), (num3 + num11) / num1, ref this._vel, this._followAccelerationTime, float.PositiveInfinity, Time.get_unscaledDeltaTime()));
    }
    else
    {
      if ((double) num2 >= (double) num10)
        return;
      float num11 = num2 - num10;
      this.scroll.set_verticalNormalizedPosition(Mathf.SmoothDamp(this.scroll.get_verticalNormalizedPosition(), (num3 + num11) / num1, ref this._vel, this._followAccelerationTime, float.PositiveInfinity, Time.get_unscaledDeltaTime()));
    }
  }

  private void SetActiveControl(bool isActive)
  {
    Input instance = Singleton<Input>.Instance;
    if (isActive)
    {
      instance.FocusLevel = 0;
      instance.MenuElements = this.MenuUIList;
      instance.ReserveState(Input.ValidType.UI);
      instance.SetupState();
      this.selectedID = 0;
      ((Selectable) this.ItemKind[this.selectedID]).Select();
      this.carsolPanel.SetActive(true);
    }
    else
    {
      instance.ClearMenuElements();
      instance.FocusLevel = -1;
      instance.ReserveState(Input.ValidType.Action);
      instance.SetupState();
      this.carsolPanel.SetActive(false);
    }
  }

  private void SelectMove(MoveDirection moveDir)
  {
    if (moveDir != 3)
    {
      if (moveDir == 1)
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
    ((Selectable) this.ItemKind[this.selectedID]).Select();
  }

  public void CreateCategoryButton()
  {
    this.ItemKind = new Button[18];
    for (int index = 0; index < this.ItemKind.Length; ++index)
    {
      this.ItemKind[index] = (Button) Object.Instantiate<Button>((M0) this.ItemKindButton);
      ((Component) this.ItemKind[index]).get_transform().SetParent(((Component) this.ContentsRectTransform).get_transform(), false);
      ((Component) this.ItemKind[index]).get_gameObject().SetActive(true);
      if (Singleton<CraftCommandListBaseObject>.Instance.CategoryNames.ContainsKey(index + 1))
        ((Text) ((Component) this.ItemKind[index]).GetComponentInChildren<Text>()).set_text(Singleton<CraftCommandListBaseObject>.Instance.CategoryNames[index + 1]);
    }
  }

  public override void OnInputMoveDirection(MoveDirection moveDir)
  {
    this.OnMove(moveDir);
    ((Selectable) this.ItemKind[this.selectedID]).Select();
  }
}
