// Decompiled with JetBrains decompiler
// Type: AIProject.OptimizedDropdown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ReMotion;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEx.Misc;

namespace AIProject
{
  [RequireComponent(typeof (RectTransform))]
  [AddComponentMenu("YK/UI/Dropdown", 35)]
  public class OptimizedDropdown : Selectable, IPointerClickHandler, IEventSystemHandler, ISubmitHandler, ICancelHandler
  {
    private static OptimizedDropdown.OptionData _noOptionData = new OptimizedDropdown.OptionData();
    [SerializeField]
    private RectTransform _template;
    [SerializeField]
    private Text _captionText;
    [SerializeField]
    private Image _captionImage;
    [SerializeField]
    [Space]
    private Text _itemText;
    [SerializeField]
    private Image _itemImage;
    [SerializeField]
    private bool _focusedSelectedItem;
    [SerializeField]
    [Space]
    private int _value;
    [SerializeField]
    [Space]
    private OptimizedDropdown.OptionDataList _options;
    [SerializeField]
    [Space]
    private OptimizedDropdown.DropdownEvent _onValueChanged;
    private ScrollRect _scrollRect;
    private GameObject _dropdown;
    private GameObject _blocker;
    private List<OptimizedDropdown.DropdownItem> _items;
    private bool _validTemplate;
    private OptimizedDropdown.DropdownItemPool _pool;
    private Rect? _rect;
    private IDisposable _fadeSubscriber;

    protected OptimizedDropdown()
    {
      base.\u002Ector();
    }

    public bool FocusedSelectedItem
    {
      get
      {
        return this._focusedSelectedItem;
      }
      set
      {
        this._focusedSelectedItem = value;
      }
    }

    public ScrollRect ScrollRect
    {
      get
      {
        return this._scrollRect;
      }
    }

    public RectTransform Template
    {
      get
      {
        return this._template;
      }
      set
      {
        this._template = value;
        this.RefreshShownValue();
      }
    }

    public Text CaptionText
    {
      get
      {
        return this._captionText;
      }
      set
      {
        this._captionText = value;
        this.RefreshShownValue();
      }
    }

    public Image CaptionImage
    {
      get
      {
        return this._captionImage;
      }
      set
      {
        this._captionImage = value;
        this.RefreshShownValue();
      }
    }

    public Text ItemText
    {
      get
      {
        return this._itemText;
      }
      set
      {
        this._itemText = value;
        this.RefreshShownValue();
      }
    }

    public Image ItemImage
    {
      get
      {
        return this._itemImage;
      }
      set
      {
        this._itemImage = value;
        this.RefreshShownValue();
      }
    }

    public List<OptimizedDropdown.OptionData> Options
    {
      get
      {
        return this._options.Options;
      }
      set
      {
        this._options.Options = value;
        this.RefreshShownValue();
      }
    }

    public OptimizedDropdown.DropdownEvent OnValueChanged
    {
      get
      {
        return this._onValueChanged;
      }
      set
      {
        this._onValueChanged = value;
      }
    }

    public int Value
    {
      get
      {
        return this._value;
      }
      set
      {
        if (Application.get_isPlaying() && (value == this._value || this.Options.Count == 0))
          return;
        this._value = Mathf.Clamp(value, 0, this.Options.Count - 1);
        this.RefreshShownValue();
        this._onValueChanged.Invoke(this._value);
      }
    }

    public bool IsExpanded
    {
      get
      {
        return Object.op_Inequality((Object) this._dropdown, (Object) null);
      }
    }

    protected virtual void Awake()
    {
      if (Object.op_Implicit((Object) this._captionImage))
        ((Behaviour) this._captionImage).set_enabled(Object.op_Inequality((Object) this._captionImage.get_sprite(), (Object) null));
      if (!Object.op_Implicit((Object) this._template))
        return;
      ((Component) this._template).get_gameObject().SetActive(false);
    }

    public void RefreshShownValue()
    {
      OptimizedDropdown.OptionData optionData = OptimizedDropdown._noOptionData;
      if (this.Options.Count > 0)
        optionData = this.Options[Mathf.Clamp(this._value, 0, this.Options.Count - 1)];
      if (Object.op_Implicit((Object) this._captionText))
      {
        if (optionData != null && optionData.Text != null)
          this._captionText.set_text(optionData.Text);
        else
          this._captionText.set_text(string.Empty);
      }
      if (!Object.op_Implicit((Object) this._captionImage))
        return;
      if (optionData != null)
        this._captionImage.set_sprite(optionData.Sprite);
      else
        this._captionImage.set_sprite((Sprite) null);
      ((Behaviour) this._captionImage).set_enabled(Object.op_Inequality((Object) this._captionImage.get_sprite(), (Object) null));
    }

    public void AddOptions(List<OptimizedDropdown.OptionData> options)
    {
      this.Options.AddRange((IEnumerable<OptimizedDropdown.OptionData>) options);
      this.RefreshShownValue();
    }

    public void AddOptions(List<string> options)
    {
      for (int index = 0; index < options.Count; ++index)
        this.Options.Add(new OptimizedDropdown.OptionData(options[index]));
      this.RefreshShownValue();
    }

    public void ClearOptions()
    {
      this.Options.Clear();
      this.RefreshShownValue();
    }

    private void SetupTemplate()
    {
      this._validTemplate = false;
      if (!Object.op_Implicit((Object) this._template))
      {
        Debug.LogError((object) "The dropdown template is not assigned. The template needs to be assigned must have a child GameObject with a Toggle component serving as the item.", (Object) this);
      }
      else
      {
        GameObject gameObject = ((Component) this._template).get_gameObject();
        gameObject.SetActive(true);
        Toggle componentInChildren = (Toggle) ((Component) this._template).GetComponentInChildren<Toggle>();
        this._validTemplate = true;
        if (!Object.op_Implicit((Object) componentInChildren) || Object.op_Equality((Object) ((Component) componentInChildren).get_transform(), (Object) this.Template))
        {
          this._validTemplate = false;
          Debug.LogError((object) "The dropdown template is not valid. The template must have a vhild GameObject with a child GameObject with a Toggle component serving as the item.", (Object) this.Template);
        }
        else if (Object.op_Inequality((Object) this.ItemText, (Object) null) && !((Component) this.ItemText).get_transform().IsChildOf(((Component) componentInChildren).get_transform()))
        {
          this._validTemplate = false;
          Debug.LogError((object) "The dropdown template is not valid. The Item Text must be on the item GameObject or children of it.", (Object) this.Template);
        }
        else if (Object.op_Inequality((Object) this.ItemImage, (Object) null) && !((Component) this.ItemImage).get_transform().IsChildOf(((Component) componentInChildren).get_transform()))
        {
          this._validTemplate = false;
          Debug.LogError((object) "The dropdown template is not valid. The Item Image must be on the item GameOBject or children of it.", (Object) this.Template);
        }
        if (!this._validTemplate)
        {
          gameObject.SetActive(false);
        }
        else
        {
          OptimizedDropdown.DropdownItem dropdownItem = (OptimizedDropdown.DropdownItem) ((Component) componentInChildren).get_gameObject().AddComponent<OptimizedDropdown.DropdownItem>();
          dropdownItem.Text = this._itemText;
          dropdownItem.Image = this._itemImage;
          dropdownItem.Toggle = componentInChildren;
          dropdownItem.RectTransform = ((Component) componentInChildren).get_transform() as RectTransform;
          Canvas orAddComponent = OptimizedDropdown.GetOrAddComponent<Canvas>(gameObject);
          orAddComponent.set_overrideSorting(true);
          orAddComponent.set_sortingOrder(30000);
          OptimizedDropdown.GetOrAddComponent<GraphicRaycaster>(gameObject);
          OptimizedDropdown.GetOrAddComponent<CanvasGroup>(gameObject);
          gameObject.SetActive(false);
          this._validTemplate = true;
        }
        this._pool = new OptimizedDropdown.DropdownItemPool((Action<OptimizedDropdown.DropdownItem>) null, (Action<OptimizedDropdown.DropdownItem>) null, ((Component) componentInChildren).get_gameObject());
      }
    }

    private static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
      T component = go.GetComponent<T>();
      return Object.op_Inequality((Object) (object) component, (Object) null) ? component : go.AddComponent<T>();
    }

    public virtual void OnPointerClick(PointerEventData ped)
    {
      this.Show();
    }

    public virtual void OnSubmit(BaseEventData bed)
    {
      this.Show();
    }

    public virtual void OnCancel(BaseEventData bed)
    {
      this.Hide();
    }

    public void Show()
    {
      if (!((UIBehaviour) this).IsActive() || !this.IsInteractable() || Object.op_Inequality((Object) this._dropdown, (Object) null) && this._dropdown.get_activeSelf())
        return;
      if (!this._validTemplate)
      {
        this.SetupTemplate();
        if (!this._validTemplate)
          return;
      }
      List<Canvas> toRelease = ListPool<Canvas>.Get();
      ((Component) this).GetComponentsInParent<Canvas>(false, (List<M0>) toRelease);
      if (toRelease.Count <= 0)
        return;
      Canvas rootCanvas = toRelease[0];
      ListPool<Canvas>.Release(toRelease);
      ((Component) this._template).get_gameObject().SetActive(true);
      if (Object.op_Equality((Object) this._dropdown, (Object) null))
      {
        this._dropdown = (GameObject) Object.Instantiate<GameObject>((M0) ((Component) this._template).get_gameObject());
        ((Object) this._dropdown).set_name("Dropdown List");
        ((Component) this._dropdown.GetComponentInChildren<OptimizedDropdown.DropdownItem>()).get_gameObject().SetActive(false);
        this._scrollRect = (ScrollRect) this._dropdown.GetComponent<ScrollRect>();
      }
      this._dropdown.SetActive(true);
      RectTransform transform1 = this._dropdown.get_transform() as RectTransform;
      ((Transform) transform1).SetParent(((Component) this._template).get_transform().get_parent(), false);
      OptimizedDropdown.DropdownItem componentInChildren = (OptimizedDropdown.DropdownItem) ((Component) this._template).GetComponentInChildren<OptimizedDropdown.DropdownItem>();
      GameObject gameObject = ((Component) ((Component) this._dropdown.GetComponentInChildren<OptimizedDropdown.DropdownItem>(true)).get_transform().get_parent()).get_gameObject();
      RectTransform transform2 = gameObject.get_transform() as RectTransform;
      ((Component) componentInChildren.RectTransform).get_gameObject().SetActive(true);
      if (!this._rect.HasValue)
        this._rect = new Rect?(transform2.get_rect());
      Rect rect1 = componentInChildren.RectTransform.get_rect();
      Vector2 min1 = ((Rect) ref rect1).get_min();
      Rect rect2 = this._rect.Value;
      Vector2 min2 = ((Rect) ref rect2).get_min();
      Vector2 vector2_1 = Vector2.op_Addition(Vector2.op_Subtraction(min1, min2), Vector2.op_Implicit(((Transform) componentInChildren.RectTransform).get_localPosition()));
      Vector2 max1 = ((Rect) ref rect1).get_max();
      Rect rect3 = this._rect.Value;
      Vector2 max2 = ((Rect) ref rect3).get_max();
      Vector2 vector2_2 = Vector2.op_Addition(Vector2.op_Subtraction(max1, max2), Vector2.op_Implicit(((Transform) componentInChildren.RectTransform).get_localPosition()));
      Vector2 size = ((Rect) ref rect1).get_size();
      this._items.Clear();
      Toggle toggle = (Toggle) null;
      for (int index = 0; index < this.Options.Count; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        OptimizedDropdown.\u003CShow\u003Ec__AnonStorey0 showCAnonStorey0 = new OptimizedDropdown.\u003CShow\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        showCAnonStorey0.\u0024this = this;
        // ISSUE: reference to a compiler-generated field
        showCAnonStorey0.index = index;
        OptimizedDropdown.DropdownItem dropdownItem = this.AddItem(this.Options[index], gameObject.get_transform(), this._items);
        if (Object.op_Inequality((Object) dropdownItem, (Object) null))
        {
          dropdownItem.Toggle.set_isOn(this.Value == index);
          // ISSUE: method pointer
          ((UnityEvent<bool>) dropdownItem.Toggle.onValueChanged).AddListener(new UnityAction<bool>((object) showCAnonStorey0, __methodptr(\u003C\u003Em__0)));
          if (Object.op_Inequality((Object) toggle, (Object) null))
          {
            Navigation navigation1 = ((Selectable) toggle).get_navigation();
            Navigation navigation2 = ((Selectable) dropdownItem.Toggle).get_navigation();
            ((Navigation) ref navigation1).set_mode((Navigation.Mode) 4);
            ((Navigation) ref navigation2).set_mode((Navigation.Mode) 4);
            ((Navigation) ref navigation1).set_selectOnDown((Selectable) dropdownItem.Toggle);
            ((Navigation) ref navigation1).set_selectOnRight((Selectable) dropdownItem.Toggle);
            ((Navigation) ref navigation2).set_selectOnLeft((Selectable) toggle);
            ((Navigation) ref navigation2).set_selectOnUp((Selectable) toggle);
            ((Selectable) toggle).set_navigation(navigation1);
            ((Selectable) dropdownItem.Toggle).set_navigation(navigation2);
          }
          toggle = dropdownItem.Toggle;
        }
      }
      Vector2 sizeDelta = transform2.get_sizeDelta();
      sizeDelta.y = (__Null) (size.y * (double) this._items.Count + vector2_1.y - vector2_2.y);
      transform2.set_sizeDelta(sizeDelta);
      Rect rect4 = transform1.get_rect();
      double height1 = (double) ((Rect) ref rect4).get_height();
      Rect rect5 = transform2.get_rect();
      double height2 = (double) ((Rect) ref rect5).get_height();
      float num1 = (float) (height1 - height2);
      if ((double) num1 > 0.0)
        transform1.set_sizeDelta(new Vector2((float) transform1.get_sizeDelta().x, (float) transform1.get_sizeDelta().y - num1));
      Vector3[] vector3Array = new Vector3[4];
      transform1.GetWorldCorners(vector3Array);
      RectTransform transform3 = ((Component) rootCanvas).get_transform() as RectTransform;
      Rect rect6 = transform3.get_rect();
      for (int index1 = 0; index1 < 2; ++index1)
      {
        bool flag = false;
        for (int index2 = 0; index2 < 4; ++index2)
        {
          Vector3 vector3 = ((Transform) transform3).InverseTransformPoint(vector3Array[index2]);
          double num2 = (double) ((Vector3) ref vector3).get_Item(index1);
          Vector2 min3 = ((Rect) ref rect6).get_min();
          double num3 = (double) ((Vector2) ref min3).get_Item(index1);
          int num4;
          if (num2 >= num3)
          {
            double num5 = (double) ((Vector3) ref vector3).get_Item(index1);
            Vector2 max3 = ((Rect) ref rect6).get_max();
            double num6 = (double) ((Vector2) ref max3).get_Item(index1);
            num4 = num5 > num6 ? 1 : 0;
          }
          else
            num4 = 1;
          if (num4 != 0)
          {
            flag = true;
            break;
          }
        }
        if (flag)
          RectTransformUtility.FlipLayoutOnAxis(transform1, index1, false, false);
      }
      for (int index = 0; index < this._items.Count; ++index)
      {
        RectTransform rectTransform = this._items[index].RectTransform;
        rectTransform.set_anchorMin(new Vector2((float) rectTransform.get_anchorMin().x, 0.0f));
        rectTransform.set_anchorMax(new Vector2((float) rectTransform.get_anchorMax().x, 0.0f));
        rectTransform.set_anchoredPosition(new Vector2((float) rectTransform.get_anchoredPosition().x, (float) (vector2_1.y + size.y * (double) (this._items.Count - 1 - index) + size.y * rectTransform.get_pivot().y)));
        rectTransform.set_sizeDelta(new Vector2((float) rectTransform.get_sizeDelta().x, (float) size.y));
      }
      this.AlphaFadeList(0.15f, 0.0f, 1f, (Action) (() => {}));
      ((Component) this._template).get_gameObject().SetActive(false);
      this._blocker = this.CreateBlocker(rootCanvas);
      if (!this._focusedSelectedItem)
        return;
      RectTransform transform4 = ((Component) ((IEnumerable<Toggle>) ((Component) this._scrollRect.get_content()).GetComponentsInChildren<Toggle>()).FirstOrDefault<Toggle>((Func<Toggle, bool>) (x => x.get_isOn()))).get_transform() as RectTransform;
      Rect rect7 = this._scrollRect.get_content().get_rect();
      double height3 = (double) ((Rect) ref rect7).get_height();
      Rect rect8 = this._scrollRect.get_viewport().get_rect();
      double height4 = (double) ((Rect) ref rect8).get_height();
      float num7 = (float) (height3 - height4);
      // ISSUE: variable of the null type
      __Null y = transform4.get_anchoredPosition().y;
      Rect rect9 = transform4.get_rect();
      double num8 = (double) ((Rect) ref rect9).get_height() / 2.0;
      double num9 = y - num8;
      Rect rect10 = this._scrollRect.get_viewport().get_rect();
      double num10 = (double) ((Rect) ref rect10).get_height() / 2.0;
      float num11 = (float) (num9 - num10);
      this._scrollRect.get_verticalScrollbar().set_value(Mathf.InverseLerp(0.0f, num7, Mathf.Clamp(num11, 0.0f, num7)));
    }

    protected virtual GameObject CreateBlocker(Canvas rootCanvas)
    {
      GameObject gameObject = new GameObject("Blocker");
      RectTransform rectTransform = (RectTransform) gameObject.AddComponent<RectTransform>();
      ((Transform) rectTransform).SetParent(((Component) rootCanvas).get_transform(), false);
      rectTransform.set_anchorMin(Vector2.op_Implicit(Vector3.get_zero()));
      rectTransform.set_anchorMax(Vector2.op_Implicit(Vector3.get_one()));
      rectTransform.set_sizeDelta(Vector2.get_zero());
      Canvas canvas = (Canvas) gameObject.AddComponent<Canvas>();
      canvas.set_overrideSorting(true);
      Canvas component = (Canvas) this._dropdown.GetComponent<Canvas>();
      canvas.set_sortingLayerID(component.get_sortingLayerID());
      canvas.set_sortingOrder(component.get_sortingOrder() - 1);
      gameObject.AddComponent<GraphicRaycaster>();
      ((Graphic) gameObject.AddComponent<Image>()).set_color(Color.get_clear());
      // ISSUE: method pointer
      ((UnityEvent) ((Button) gameObject.AddComponent<Button>()).get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CCreateBlocker\u003Em__2)));
      return gameObject;
    }

    protected virtual void DestroyBlocker(GameObject blocker)
    {
      Object.Destroy((Object) blocker);
    }

    protected virtual OptimizedDropdown.DropdownItem CreateItem(
      OptimizedDropdown.DropdownItem itemTemplate)
    {
      return (OptimizedDropdown.DropdownItem) Object.Instantiate<OptimizedDropdown.DropdownItem>((M0) itemTemplate);
    }

    private OptimizedDropdown.DropdownItem AddItem(
      OptimizedDropdown.OptionData data,
      Transform parent,
      List<OptimizedDropdown.DropdownItem> items)
    {
      OptimizedDropdown.DropdownItem dropdownItem = this._pool.Get();
      ((Transform) dropdownItem.RectTransform).SetParent(parent, false);
      ((Component) dropdownItem).get_gameObject().SetActive(true);
      ((Object) ((Component) dropdownItem).get_gameObject()).set_name("Item " + (object) items.Count + (data.Text == null ? (object) string.Empty : (object) (": " + data.Text)));
      ((UnityEventBase) dropdownItem.Toggle.onValueChanged).RemoveAllListeners();
      if (Object.op_Inequality((Object) dropdownItem, (Object) null))
        dropdownItem.Toggle.set_isOn(false);
      if (Object.op_Implicit((Object) dropdownItem.Text))
        dropdownItem.Text.set_text(data.Text);
      if (Object.op_Implicit((Object) dropdownItem.Image))
      {
        dropdownItem.Image.set_sprite(data.Sprite);
        ((Behaviour) dropdownItem.Image).set_enabled(Object.op_Inequality((Object) dropdownItem.Image.get_sprite(), (Object) null));
      }
      items.Add(dropdownItem);
      return dropdownItem;
    }

    private void AlphaFadeList(float duration, float alpha, Action onCompleted)
    {
      CanvasGroup component = (CanvasGroup) this._dropdown.GetComponent<CanvasGroup>();
      this.AlphaFadeList(duration, component.get_alpha(), alpha, onCompleted);
    }

    private void AlphaFadeList(float duration, float start, float end, Action onCompleted)
    {
      if (end.Equals(start))
        return;
      if (this._fadeSubscriber != null)
        this._fadeSubscriber.Dispose();
      this._fadeSubscriber = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(duration, true), true), (Action<M0>) (x => this.Alpha = Mathf.Lerp(start, end, ((TimeInterval<float>) ref x).get_Value())), (Action) (() =>
      {
        Action action = onCompleted;
        if (action == null)
          return;
        action();
      }));
    }

    private float Alpha
    {
      set
      {
        if (Object.op_Equality((Object) this._dropdown, (Object) null))
          return;
        ((CanvasGroup) this._dropdown.GetComponent<CanvasGroup>()).set_alpha(value);
      }
    }

    public void Hide()
    {
      if (Object.op_Inequality((Object) this._dropdown, (Object) null))
        this.AlphaFadeList(0.15f, 0.0f, (Action) (() =>
        {
          if (!((UIBehaviour) this).IsActive())
            return;
          if ((double) Time.get_timeScale() != 0.0)
          {
            ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds(0.150000005960464)), (Action<M0>) (_ =>
            {
              Queue<OptimizedDropdown.DropdownItem> queue = (Queue<OptimizedDropdown.DropdownItem>) EnumerableExtension.ToQueue<OptimizedDropdown.DropdownItem>((IEnumerable<M0>) this._items);
              while (queue.Count > 0)
                this._pool.Release(queue.Dequeue());
              this._items.Clear();
              this._dropdown.SetActive(false);
            }));
          }
          else
          {
            Queue<OptimizedDropdown.DropdownItem> queue = (Queue<OptimizedDropdown.DropdownItem>) EnumerableExtension.ToQueue<OptimizedDropdown.DropdownItem>((IEnumerable<M0>) this._items);
            while (queue.Count > 0)
              this._pool.Release(queue.Dequeue());
            this._items.Clear();
            this._dropdown.SetActive(false);
          }
        }));
      if (Object.op_Inequality((Object) this._blocker, (Object) null))
        this.DestroyBlocker(this._blocker);
      this._blocker = (GameObject) null;
      this.Select();
    }

    protected internal class DropdownItem : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, ICancelHandler
    {
      [SerializeField]
      private Text _text;
      [SerializeField]
      private Image _image;
      [SerializeField]
      private RectTransform _rectTransform;
      [SerializeField]
      private Toggle _toggle;

      public DropdownItem()
      {
        base.\u002Ector();
      }

      public Text Text
      {
        get
        {
          return this._text;
        }
        set
        {
          this._text = value;
        }
      }

      public Image Image
      {
        get
        {
          return this._image;
        }
        set
        {
          this._image = value;
        }
      }

      public RectTransform RectTransform
      {
        get
        {
          return this._rectTransform;
        }
        set
        {
          this._rectTransform = value;
        }
      }

      public Toggle Toggle
      {
        get
        {
          return this._toggle;
        }
        set
        {
          this._toggle = value;
        }
      }

      public virtual void OnPointerEnter(PointerEventData ped)
      {
        EventSystem.get_current().SetSelectedGameObject(((Component) this).get_gameObject());
      }

      public virtual void OnCancel(BaseEventData eventData)
      {
        Dropdown componentInParent = (Dropdown) ((Component) this).GetComponentInParent<Dropdown>();
        if (!Object.op_Implicit((Object) componentInParent))
          return;
        componentInParent.Hide();
      }
    }

    protected internal class DropdownItemPool
    {
      private readonly Stack<OptimizedDropdown.DropdownItem> _stack = new Stack<OptimizedDropdown.DropdownItem>();
      private readonly Action<OptimizedDropdown.DropdownItem> _onGet;
      private readonly Action<OptimizedDropdown.DropdownItem> _onReleased;
      private GameObject _template;

      public DropdownItemPool(
        Action<OptimizedDropdown.DropdownItem> onGet,
        Action<OptimizedDropdown.DropdownItem> onReleased,
        GameObject template)
      {
        this._onGet = onGet;
        this._onReleased = onReleased;
        this._template = template;
      }

      public int CountAll { get; private set; }

      public int CountActive
      {
        get
        {
          return this.CountAll - this.CountInactive;
        }
      }

      public int CountInactive
      {
        get
        {
          return this._stack.Count;
        }
      }

      public OptimizedDropdown.DropdownItem Get()
      {
        if (Object.op_Equality((Object) this._template, (Object) null))
        {
          Debug.LogError((object) "_templateを設定していません");
          return (OptimizedDropdown.DropdownItem) null;
        }
        OptimizedDropdown.DropdownItem dropdownItem;
        if (this._stack.Count == 0)
        {
          dropdownItem = (OptimizedDropdown.DropdownItem) ((GameObject) Object.Instantiate<GameObject>((M0) this._template)).GetComponent<OptimizedDropdown.DropdownItem>();
          ++this.CountAll;
        }
        else
          dropdownItem = this._stack.Pop();
        if (this._onGet != null)
          this._onGet(dropdownItem);
        return dropdownItem;
      }

      public void Release(OptimizedDropdown.DropdownItem element)
      {
        if (this._stack.Count > 0 && object.ReferenceEquals((object) this._stack.Peek(), (object) element))
          Debug.LogError((object) "Internal error. Trying to destroy object that is already released to pool.");
        if (this._onReleased != null)
          this._onReleased(element);
        this._stack.Push(element);
      }
    }

    [Serializable]
    public class OptionData
    {
      [SerializeField]
      private string _text = string.Empty;
      [SerializeField]
      private Sprite _sprite;

      public OptionData()
      {
      }

      public OptionData(string text)
      {
        this._text = text;
      }

      public OptionData(Sprite sprite)
      {
        this._sprite = sprite;
      }

      public OptionData(string text, Sprite sprite)
      {
        this._text = text;
        this._sprite = sprite;
      }

      public string Text
      {
        get
        {
          return this._text;
        }
        set
        {
          this._text = value;
        }
      }

      public Sprite Sprite
      {
        get
        {
          return this._sprite;
        }
        set
        {
          this._sprite = value;
        }
      }
    }

    [Serializable]
    public class OptionDataList
    {
      [SerializeField]
      private List<OptimizedDropdown.OptionData> _options = new List<OptimizedDropdown.OptionData>();

      public List<OptimizedDropdown.OptionData> Options
      {
        get
        {
          return this._options;
        }
        set
        {
          this._options = value;
        }
      }
    }

    [Serializable]
    public class DropdownEvent : UnityEvent<int>
    {
      public DropdownEvent()
      {
        base.\u002Ector();
      }
    }
  }
}
