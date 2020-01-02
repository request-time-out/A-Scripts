// Decompiled with JetBrains decompiler
// Type: AIProject.UI.FishingUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.MiniGames.Fishing;
using AIProject.SaveData;
using Manager;
using ReMotion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEx;

namespace AIProject.UI
{
  public class FishingUI : MenuUIBehaviour
  {
    [SerializeField]
    private Color whiteColor = Color.get_white();
    [SerializeField]
    private Color redColor = Color.get_red();
    [SerializeField]
    private Color greenColor = Color.get_green();
    [SerializeField]
    private Color lightGreenColor = Color.get_green();
    [SerializeField]
    private Color blueColor = Color.get_blue();
    [SerializeField]
    private Color yellowColor = Color.get_yellow();
    private FishingHowToUI[] howToElements = new FishingHowToUI[0];
    [Space(5f)]
    [SerializeField]
    private FishingUI.SelectFoodGroup selectFoodGroup = new FishingUI.SelectFoodGroup();
    [Space(5f)]
    [SerializeField]
    private FishingUI.HitWaitGroup hitWaitGroup = new FishingUI.HitWaitGroup();
    [Space(5f)]
    [SerializeField]
    private FishingUI.FishingGroup fishingGroup = new FishingUI.FishingGroup();
    [Space(5f)]
    [SerializeField]
    private FishingUI.ResultGroup resultGroup = new FishingUI.ResultGroup();
    [SerializeField]
    private ItemIDKeyPair _fakeBaitItemID = new ItemIDKeyPair();
    [SerializeField]
    private int _foodCountNormalFontSize = 40;
    [SerializeField]
    private int _foodCountInfinityFontSize = 48;
    private bool displayGuide = true;
    private Dictionary<int, FishingUI.ArrowAnimParam> AnimTable = new Dictionary<int, FishingUI.ArrowAnimParam>();
    private FishInfo getFishInfo = new FishInfo();
    [Space(5f)]
    [SerializeField]
    private CanvasGroup howToGroup;
    [SerializeField]
    private GameObject howToElementPrefab;
    [SerializeField]
    private CanvasGroup fishFoodTypeGroup;
    [SerializeField]
    private Image fishFoodImage;
    [SerializeField]
    private Text fishFoodNumText;
    [SerializeField]
    private Text fishFoodNumOverText;
    [SerializeField]
    private int _baitItemCategoryID;
    private MenuUIBehaviour[] _menuUIElements;
    private GameObject getFishModel;
    private int selectFishFoodIndex;
    private IDisposable updateDisposable;
    private IDisposable lateUpdateDisposable;
    private IDisposable fadeSubscriber;
    private IDisposable foodChangeSubscriber;
    private float getExperience;
    private IDisposable drawHitTextSubscriber;
    private IDisposable drawGetTextDisposable;
    private IDisposable drawResultWindowDisposable;
    private IDisposable drawLevelUpTextDisposable;
    private IDisposable finishingFishingWaitKeyDisposable;

    private MenuUIBehaviour[] MenuUIElements
    {
      get
      {
        MenuUIBehaviour[] menuUiElements = this._menuUIElements;
        if (menuUiElements != null)
          return menuUiElements;
        return this._menuUIElements = new MenuUIBehaviour[1]
        {
          (MenuUIBehaviour) this
        };
      }
    }

    private List<FishFoodInfo> FoodInfoList { get; set; }

    public FishFoodInfo SelectedFishFood { get; private set; }

    public bool IsActive { get; private set; }

    private bool DisplayShortcutUI
    {
      get
      {
        return Singleton<Game>.IsInstance() && Object.op_Inequality((Object) Singleton<Game>.Instance.MapShortcutUI, (Object) null);
      }
    }

    public bool FocusInOn
    {
      get
      {
        return this.IsActive && Singleton<Input>.Instance.FocusLevel == this.FocusLevel && (this.IsActiveControl && this.EnabledInput) && !this.DisplayShortcutUI;
      }
    }

    protected override void Awake()
    {
      base.Awake();
      this.OpenAllCanvas();
      this.CloseAllCanvas();
      this.FoodInfoList = ListPool<FishFoodInfo>.Get();
    }

    protected override void Start()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x)));
      if (this.howToElements == null || this.howToElements.Length == 0)
      {
        this.howToElements = new FishingHowToUI[3];
        for (int index = 0; index < 3; ++index)
        {
          GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.howToElementPrefab, ((Component) this.howToGroup).get_transform());
          this.howToElements[index] = (FishingHowToUI) gameObject.GetComponent<FishingHowToUI>();
          ((Component) this.howToElements[index]).get_gameObject().SetActive(false);
        }
      }
      base.Start();
    }

    protected void OnDestroy()
    {
      ListPool<FishFoodInfo>.Release(this.FoodInfoList);
    }

    private void SetActiveControl(bool isActive)
    {
      IEnumerator _coroutine = !isActive ? this.DoStop() : this.DoSetup();
      if (this.fadeSubscriber != null)
        this.fadeSubscriber.Dispose();
      this.fadeSubscriber = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), (Action<M0>) (_ => {}), (Action<Exception>) (ex => Debug.LogException(ex)));
    }

    private void SetSelectedFood(int _selectedIndex)
    {
      if (_selectedIndex < 0)
        _selectedIndex = this.FoodInfoList.Count - 1;
      else if (this.FoodInfoList.Count <= _selectedIndex)
        _selectedIndex = 0;
      this.selectFishFoodIndex = _selectedIndex;
      FishFoodInfo element = this.FoodInfoList.GetElement<FishFoodInfo>(this.selectFishFoodIndex);
      if (element == null)
        this.SelectedFishFood = (FishFoodInfo) null;
      else
        this.SelectedFishFood = element;
    }

    [DebuggerHidden]
    private IEnumerator DoSetup()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FishingUI.\u003CDoSetup\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator DoStop()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FishingUI.\u003CDoStop\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void DestroyGetFishModel()
    {
      if (!Object.op_Inequality((Object) this.getFishModel, (Object) null))
        return;
      Object.Destroy((Object) this.getFishModel);
      this.getFishModel = (GameObject) null;
    }

    public void UseFishFood()
    {
      this.SelectedFishFood?.UseFood();
      this.ResetSelectFoodUI();
    }

    public int FishFoodNum
    {
      get
      {
        int? count = this.SelectedFishFood?.Count;
        return count.HasValue ? count.Value : 0;
      }
    }

    public bool HaveSomeFishFood()
    {
      foreach (FishFoodInfo foodInfo in this.FoodInfoList)
      {
        if (0 < foodInfo.Count)
          return true;
      }
      return false;
    }

    public bool FilledItemSlot()
    {
      return Singleton<Manager.Map>.Instance.Player.PlayerData.InventorySlotMax <= Singleton<Manager.Map>.Instance.Player.PlayerData.ItemList.Count;
    }

    [DebuggerHidden]
    private IEnumerator ChangeFood(int _moveIndex)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FishingUI.\u003CChangeFood\u003Ec__Iterator2()
      {
        _moveIndex = _moveIndex,
        \u0024this = this
      };
    }

    private float Lerp(float a, float b, float t)
    {
      return Mathf.Lerp(a, b, t);
    }

    private float InverseLerp(float a, float b, float value)
    {
      return Mathf.InverseLerp(a, b, value);
    }

    private Color Lerp(Color a, Color b, float t)
    {
      return new Color(Mathf.Lerp((float) a.r, (float) b.r, t), Mathf.Lerp((float) a.g, (float) b.g, t), Mathf.Lerp((float) a.b, (float) b.b, t), Mathf.Lerp((float) a.a, (float) b.a, t));
    }

    private void EasingImage(
      Image _image,
      float _count,
      float _limit,
      Color _color1,
      Color _color2,
      Vector3 _scale1,
      Vector3 _scale2)
    {
      float time = Mathf.InverseLerp(0.0f, _limit, _count);
      float t = EasingFunctions.EaseOutQuint(time, 1f);
      ((Graphic) _image).set_color(this.Lerp(_color1, _color2, t));
      ((Component) _image).get_transform().set_localScale(Vector3.Lerp(_scale1, _scale2, t));
    }

    private void OnUpdate()
    {
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      FishingManager fishingSystem = Singleton<Manager.Map>.Instance.FishingSystem;
      if (fishingSystem.scene != FishingManager.FishingScene.SelectFood)
        return;
      float axis = fishingSystem.GetAxis(ActionID.MouseWheel);
      if (2 <= this.FoodInfoList.Count && this.foodChangeSubscriber == null)
      {
        if ((double) axis != 0.0)
        {
          IEnumerator _coroutine = this.ChangeFood((int) Mathf.Sign(axis));
          this.foodChangeSubscriber = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false));
        }
        else if (fishingSystem.IsPressedVertical)
        {
          if (fishingSystem.IsPressedAxis(ActionID.SelectVertical))
          {
            IEnumerator _coroutine = this.ChangeFood((int) Mathf.Sign(fishingSystem.GetAxis(ActionID.SelectVertical)));
            this.foodChangeSubscriber = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false));
          }
          else if (fishingSystem.IsPressedAxis(ActionID.MoveVertical))
          {
            IEnumerator _coroutine = this.ChangeFood((int) Mathf.Sign(fishingSystem.GetAxis(ActionID.MoveVertical)));
            this.foodChangeSubscriber = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false));
          }
        }
      }
      if (0 < this.SelectedFishFood.Count && fishingSystem.NextButtonDown)
      {
        this.selectFoodGroup.Hide();
        fishingSystem.ChangeFishScene(FishingManager.FishingScene.StartMotion);
      }
      else
      {
        if (!fishingSystem.BackButtonDown)
          return;
        fishingSystem.EndFishing();
      }
    }

    private void OnLateUpdate()
    {
      FishingManager fishingSystem = Singleton<Manager.Map>.Instance.FishingSystem;
      switch (fishingSystem.scene)
      {
        case FishingManager.FishingScene.Fishing:
          float num = this.fishingGroup.heartPointImageWidth * fishingSystem.HeartPointScale;
          foreach (RectTransform heartPointRect in this.fishingGroup.heartPointRects)
          {
            Vector2 sizeDelta = heartPointRect.get_sizeDelta();
            sizeDelta.x = (__Null) (double) num;
            heartPointRect.set_sizeDelta(sizeDelta);
          }
          float timeScale = fishingSystem.TimeScale;
          Color timeColor = this.GetTimeColor(fishingSystem.TimeScale);
          foreach (Image imageOutline in this.fishingGroup.imageOutlines)
          {
            float a = (float) ((Graphic) imageOutline).get_color().a;
            timeColor.a = (__Null) (double) a;
            ((Graphic) imageOutline).set_color(timeColor);
          }
          foreach (CircleOutline circleOutline in this.fishingGroup.circleOutlines)
          {
            float a = (float) circleOutline.get_effectColor().a;
            timeColor.a = (__Null) (double) a;
            circleOutline.set_effectColor(timeColor);
          }
          float a1 = (float) ((Graphic) this.fishingGroup.timerImage).get_color().a;
          timeColor.a = (__Null) (double) a1;
          ((Graphic) this.fishingGroup.timerImage).set_color(timeColor);
          this.fishingGroup.timerCircleImage.set_fillAmount(timeScale);
          break;
      }
    }

    private Color GetTimeColor(float t)
    {
      t = Mathf.Clamp01(t);
      if (0.5 < (double) t)
      {
        t = Mathf.InverseLerp(0.5f, 1f, t);
        return Color.Lerp(this.yellowColor, this.lightGreenColor, t);
      }
      t = Mathf.InverseLerp(0.0f, 0.5f, t);
      return Color.Lerp(this.redColor, this.yellowColor, t);
    }

    private void InitializeUI()
    {
      this.InitSelectFoodUI();
      this.InitHitWaitUI();
      this.InitFishingUI();
      this.InitResultUI();
    }

    public void InitSelectFoodUI()
    {
      FishingUI.SelectFoodGroup selectFoodGroup = this.selectFoodGroup;
      bool flag = this.FoodInfoList.Count <= 1;
      this.SetActive((Component) selectFoodGroup.selectUpImage, !flag);
      this.SetActive((Component) selectFoodGroup.selectDownImage, !flag);
      this.ResetSelectFoodUI();
    }

    public void ResetSelectFoodUI()
    {
      FishingUI.SelectFoodGroup selectFoodGroup = this.selectFoodGroup;
      int num1 = !Singleton<Resources>.IsInstance() ? 999 : Singleton<Resources>.Instance.DefinePack.MapDefines.ItemSlotMax;
      if (this.SelectedFishFood != null)
      {
        selectFoodGroup.fishFoodNameText.set_text(this.SelectedFishFood.FoodName);
        this.fishFoodImage.set_sprite(this.SelectedFishFood.Icon);
        if (this.SelectedFishFood.IsInfinity)
        {
          ((Graphic) this.fishFoodNumText).set_color(this.redColor);
          this.fishFoodNumText.set_text("∞");
          this.fishFoodNumText.set_fontSize(this._foodCountInfinityFontSize);
          this.SetActive((Component) this.fishFoodNumOverText, false);
        }
        else
        {
          int count = this.SelectedFishFood.Count;
          ((Graphic) this.fishFoodNumText).set_color(count > 0 ? this.whiteColor : this.redColor);
          this.fishFoodNumText.set_text(string.Format("{0}", (object) Mathf.Clamp(count, 0, num1)));
          this.fishFoodNumText.set_fontSize(this._foodCountNormalFontSize);
          this.SetActive((Component) this.fishFoodNumOverText, num1 < count);
        }
      }
      else
      {
        selectFoodGroup.fishFoodNameText.set_text(string.Empty);
        this.fishFoodImage.set_sprite((Sprite) null);
        int num2 = 0;
        ((Graphic) this.fishFoodNumText).set_color(num2 > 0 ? this.whiteColor : this.redColor);
        this.fishFoodNumText.set_text(string.Format("{0}", (object) Mathf.Clamp(num2, 0, num1)));
        this.SetActive((Component) this.fishFoodNumOverText, num1 < num2);
      }
    }

    public void InitHitWaitUI()
    {
    }

    public void InitFishingUI()
    {
      FishingUI.FishingGroup fishingGroup = this.fishingGroup;
      foreach (RectTransform heartPointRect in fishingGroup.heartPointRects)
      {
        Vector2 sizeDelta = heartPointRect.get_sizeDelta();
        sizeDelta.x = (__Null) (double) fishingGroup.heartPointImageWidth;
        heartPointRect.set_sizeDelta(sizeDelta);
      }
      fishingGroup.timerCircleImage.set_fillAmount(1f);
      Color timeColor = this.GetTimeColor(1f);
      foreach (CircleOutline circleOutline in fishingGroup.circleOutlines)
      {
        float a = (float) circleOutline.get_effectColor().a;
        timeColor.a = (__Null) (double) a;
        circleOutline.set_effectColor(timeColor);
      }
      foreach (Image imageOutline in fishingGroup.imageOutlines)
      {
        float a = (float) ((Graphic) imageOutline).get_color().a;
        timeColor.a = (__Null) (double) a;
        ((Graphic) imageOutline).set_color(timeColor);
      }
      float a1 = (float) ((Graphic) fishingGroup.timerImage).get_color().a;
      timeColor.a = (__Null) (double) a1;
      ((Graphic) fishingGroup.timerImage).set_color(timeColor);
      this.SetAlpha((Graphic) fishingGroup.hitImage, 0.0f);
    }

    public void InitResultUI()
    {
      FishingUI.ResultGroup resultGroup = this.resultGroup;
      this.SetAlpha((Graphic) resultGroup.getImage, 0.0f);
      resultGroup.resultWindow.set_alpha(0.0f);
      resultGroup.fishNameText.set_text(string.Empty);
      resultGroup.rarelityText.set_text(string.Empty);
      resultGroup.fishImage.set_texture((Texture) null);
      resultGroup.flavorText.set_text(string.Empty);
      resultGroup.experiencePointText.set_text(string.Empty);
      resultGroup.levelText.set_text(string.Empty);
      resultGroup.experienceBarImage.set_fillAmount(0.0f);
      ((Graphic) resultGroup.experienceBarImage).set_color(this.blueColor);
      ((Graphic) resultGroup.levelUpText).set_color(this.yellowColor);
      this.SetAlpha((Graphic) resultGroup.levelUpText, 0.0f);
    }

    private void SetFishingHowToUI(
      FishingHowToUI _ui,
      string _text,
      FishingUI.MouseInputType _type)
    {
      Sprite sprite = (Sprite) null;
      Singleton<Resources>.Instance.itemIconTables.InputIconTable.TryGetValue((int) _type, out sprite);
      _ui.image.set_sprite(sprite);
      _ui.text.set_text(_text);
      this.SetEnable((Behaviour) _ui.image, Object.op_Inequality((Object) sprite, (Object) null));
    }

    private void ResetHowToUI(FishingManager.FishingScene _scene)
    {
      switch (_scene)
      {
        case FishingManager.FishingScene.SelectFood:
          this.SetActive((Component) this.howToGroup, this.displayGuide);
          this.SetActiveHowToElement(3);
          FishingHowToUI[] howToElements1 = this.howToElements;
          this.SetFishingHowToUI(howToElements1[0], "釣る", FishingUI.MouseInputType.Left);
          this.SetFishingHowToUI(howToElements1[1], "やめる", FishingUI.MouseInputType.Right);
          this.SetFishingHowToUI(howToElements1[2], "エサ変更", FishingUI.MouseInputType.Wheel);
          break;
        case FishingManager.FishingScene.WaitHit:
          this.SetActive((Component) this.howToGroup, this.displayGuide);
          this.SetActiveHowToElement(2);
          FishingHowToUI[] howToElements2 = this.howToElements;
          this.SetFishingHowToUI(howToElements2[0], "ウキの移動", FishingUI.MouseInputType.Move);
          this.SetFishingHowToUI(howToElements2[1], "もどる", FishingUI.MouseInputType.Right);
          break;
        case FishingManager.FishingScene.Fishing:
          this.SetActive((Component) this.howToGroup, this.displayGuide);
          this.SetActiveHowToElement(1);
          this.SetFishingHowToUI(this.howToElements[0], "力の方向", FishingUI.MouseInputType.Move);
          break;
        default:
          this.SetActive((Component) this.howToGroup, false);
          break;
      }
    }

    private void ResetFishFoodTypeUI(FishingManager.FishingScene _scene)
    {
      switch (_scene)
      {
        case FishingManager.FishingScene.SelectFood:
        case FishingManager.FishingScene.StartMotion:
        case FishingManager.FishingScene.WaitHit:
        case FishingManager.FishingScene.Fishing:
        case FishingManager.FishingScene.Success:
        case FishingManager.FishingScene.Failure:
          this.SetActive((Component) this.fishFoodTypeGroup, true);
          break;
        default:
          this.SetActive((Component) this.fishFoodTypeGroup, false);
          break;
      }
    }

    private void SetActiveHowToElement(int _index)
    {
      for (int index = 0; index < this.howToElements.Length; ++index)
        this.SetActive((Component) this.howToElements[index], index < _index);
    }

    public void ChangeFishScene(FishingManager.FishingScene _scene)
    {
      if (_scene == FishingManager.FishingScene.None)
        return;
      this.CloseAllCanvas();
      switch (_scene)
      {
        case FishingManager.FishingScene.SelectFood:
          this.StartSelectFood();
          break;
        case FishingManager.FishingScene.WaitHit:
          this.StartWaitHit();
          break;
        case FishingManager.FishingScene.Fishing:
          this.StartFishing();
          this.StartDrawHitText();
          break;
      }
      this.ApplyFishScene(_scene);
    }

    public void ApplyFishScene(FishingManager.FishingScene _scene)
    {
      this.ResetHowToUI(_scene);
      this.ResetFishFoodTypeUI(_scene);
    }

    public void StartSelectFood()
    {
      this.ResetSelectFoodUI();
      this.selectFoodGroup.Show();
    }

    public void StartWaitHit()
    {
      this.hitWaitGroup.Show();
    }

    public void StartFishing()
    {
      this.fishingGroup.Show();
    }

    private void StartDrawHitText()
    {
      if (this.drawHitTextSubscriber != null)
        this.drawHitTextSubscriber.Dispose();
      IEnumerator _coroutine = this.DrawHitText();
      this.drawHitTextSubscriber = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false));
    }

    [DebuggerHidden]
    private IEnumerator DrawHitText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FishingUI.\u003CDrawHitText\u003Ec__Iterator3()
      {
        \u0024this = this
      };
    }

    private void SetGetFishUI(FishInfo _info)
    {
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      FishingManager fishingSystem = Singleton<Manager.Map>.Instance.FishingSystem;
      this.getFishInfo = _info;
      this.DestroyGetFishModel();
      Resources.FishingTable fishing = Singleton<Resources>.Instance.Fishing;
      Dictionary<int, Tuple<GameObject, RuntimeAnimatorController>> fishModelTable = fishing.FishModelTable;
      Renderer renderer = (Renderer) null;
      float num1 = -50f;
      int modelId = _info.ModelID;
      Tuple<GameObject, RuntimeAnimatorController> tuple;
      if (fishModelTable.TryGetValue(modelId, out tuple) && Object.op_Inequality((Object) tuple.Item1, (Object) null))
      {
        this.getFishModel = (GameObject) Object.Instantiate<GameObject>((M0) tuple.Item1, new Vector3(0.0f, num1, 0.0f), Quaternion.get_identity());
        this.getFishModel.get_transform().SetParent(fishingSystem.RootObject.get_transform(), true);
        this.SetLayer(this.getFishModel.get_transform(), LayerMask.NameToLayer("Fishing"));
        Animator componentInChildren = (Animator) this.getFishModel.GetComponentInChildren<Animator>(true);
        if (Object.op_Inequality((Object) componentInChildren, (Object) null) && Object.op_Inequality((Object) tuple.Item2, (Object) null))
          componentInChildren.set_runtimeAnimatorController(tuple.Item2);
        renderer = (Renderer) this.getFishModel.GetComponentInChildren<Renderer>(true);
      }
      Camera fishModelCamera = fishingSystem.fishModelCamera;
      if (Object.op_Inequality((Object) renderer, (Object) null))
      {
        float num2 = fishModelCamera.get_nearClipPlane() + 5f;
        Bounds bounds = renderer.get_bounds();
        Vector3 center = ((Bounds) ref bounds).get_center();
        ref Vector3 local = ref center;
        local.x = (__Null) (local.x - ((double) num2 + ((Bounds) ref bounds).get_extents().x));
        ((Component) fishModelCamera).get_transform().set_position(center);
        ((Component) fishModelCamera).get_transform().set_rotation(Quaternion.Euler(0.0f, 90f, 0.0f));
        fishModelCamera.set_orthographic(true);
        float num3 = Mathf.Max((float) ((Bounds) ref bounds).get_extents().z, (float) ((Bounds) ref bounds).get_extents().y);
        fishModelCamera.set_orthographicSize(Mathf.Max(num3, fishing.ResultFishReferenceExtent) * 1.2f);
      }
      else
      {
        ((Component) fishModelCamera).get_transform().set_position(new Vector3(0.0f, num1, 0.0f));
        ((Component) fishModelCamera).get_transform().set_rotation(Quaternion.Euler(90f, 0.0f, 0.0f));
      }
      this.resultGroup.fishImage.set_texture((Texture) fishingSystem.fishModelCamera.get_targetTexture());
      StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(this.getFishInfo.CategoryID, this.getFishInfo.ItemID);
      if (stuffItemInfo != null)
      {
        this.resultGroup.rarelityText.set_text(Singleton<Resources>.Instance.FishingDefinePack.UIParam.RarelityLabelList.GetElement<string>((int) stuffItemInfo.Grade) ?? "☆☆☆");
        this.resultGroup.fishNameText.set_text(stuffItemInfo.Name);
        this.resultGroup.flavorText.set_text(stuffItemInfo.Explanation);
      }
      else
      {
        this.resultGroup.rarelityText.set_text("☆☆☆");
        this.resultGroup.fishNameText.set_text(string.Empty);
        this.resultGroup.flavorText.set_text(string.Empty);
      }
      this.getExperience = (float) Random.Range(this.getFishInfo.MinExPoint, this.getFishInfo.MaxExPoint);
      this.resultGroup.experiencePointText.set_text(string.Format("{0}", (object) this.getExperience));
      this.UpdateExperienceUI();
    }

    private void SetLayer(Transform _t, int _layer)
    {
      if (Object.op_Equality((Object) _t, (Object) null))
        return;
      for (int index = 0; index < _t.get_childCount(); ++index)
        this.SetLayer(_t.GetChild(index), _layer);
      ((Component) _t).get_gameObject().set_layer(_layer);
    }

    private void UpdateExperienceUI()
    {
      Skill fishingSkill = Singleton<Manager.Map>.Instance.Player.PlayerData.FishingSkill;
      if (Singleton<Resources>.Instance.FishingDefinePack.SystemParam.MaxLevel <= fishingSkill.Level)
      {
        ((Graphic) this.resultGroup.levelText).set_color(this.redColor);
        this.resultGroup.levelText.set_text("Lv Max");
        this.resultGroup.experienceBarImage.set_fillAmount(1f);
      }
      else
      {
        ((Graphic) this.resultGroup.levelText).set_color(this.whiteColor);
        this.resultGroup.levelText.set_text(string.Format("Lv {0,2}", (object) fishingSkill.Level));
        this.resultGroup.experienceBarImage.set_fillAmount(fishingSkill.Experience / (float) fishingSkill.NextExperience);
      }
    }

    public void StartDrawResult(FishInfo _info)
    {
      if (Singleton<Manager.Map>.IsInstance())
        Singleton<Manager.Map>.Instance.Player.PlayerData.ItemList.AddItem(new StuffItem(_info.CategoryID, _info.ItemID, 1));
      this.CloseAllCanvas();
      this.SetGetFishUI(_info);
      this.PlaySystemSE(SoundPack.SystemSE.Fishing_Result);
      this.StartDrawResultWindow();
    }

    private void StartDrawGetText()
    {
      if (this.drawGetTextDisposable != null)
        this.drawGetTextDisposable.Dispose();
      IEnumerator _coroutine = this.DrawGetText();
      this.drawGetTextDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false));
    }

    private void StartDrawResultWindow()
    {
      if (this.drawResultWindowDisposable != null)
        this.drawResultWindowDisposable.Dispose();
      IEnumerator _coroutine = this.DrawFishParameter();
      this.drawResultWindowDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false));
    }

    [DebuggerHidden]
    private IEnumerator DrawGetText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FishingUI.\u003CDrawGetText\u003Ec__Iterator4()
      {
        \u0024this = this
      };
    }

    private ValueTuple<int, float> GetNextFishingSkill(int _getExperience)
    {
      if (!Singleton<Manager.Map>.IsInstance())
        return new ValueTuple<int, float>(1, 0.0f);
      Skill fishingSkill = Singleton<Manager.Map>.Instance.Player.PlayerData.FishingSkill;
      int level = fishingSkill.Level;
      float num1 = (float) _getExperience + fishingSkill.Experience;
      int num2 = 0;
      while (true)
      {
        float num3 = (float) fishingSkill.CalculationNextExp(level + num2);
        if ((double) num3 <= (double) num1)
        {
          num1 -= num3;
          ++num2;
        }
        else
          break;
      }
      return new ValueTuple<int, float>(level + num2, num1);
    }

    [DebuggerHidden]
    private IEnumerator DrawFishParameter()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FishingUI.\u003CDrawFishParameter\u003Ec__Iterator5()
      {
        \u0024this = this
      };
    }

    private void DrawLevelUpText(int _prevLevel, int _newLevel)
    {
      if (this.drawLevelUpTextDisposable != null)
        this.drawLevelUpTextDisposable.Dispose();
      IEnumerator _coroutine = this.DrawLevelUpTextCoroutine();
      this.drawLevelUpTextDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false));
    }

    private float EaseInCirc(float _count, float _limit)
    {
      float time = Mathf.InverseLerp(0.0f, _limit, _count);
      return EasingFunctions.EaseInCirc(time, 1f);
    }

    private float EaseOutCirc(float _count, float _limit)
    {
      float time = Mathf.InverseLerp(0.0f, _limit, _count);
      return EasingFunctions.EaseOutCirc(time, 1f);
    }

    [DebuggerHidden]
    private IEnumerator DrawLevelUpTextCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FishingUI.\u003CDrawLevelUpTextCoroutine\u003Ec__Iterator6()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator FinishingFishingWaitKey()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FishingUI.\u003CFinishingFishingWaitKey\u003Ec__Iterator7()
      {
        \u0024this = this
      };
    }

    private void OpenAllCanvas()
    {
      this.SetActive((Component) this.howToGroup, true);
      this.SetActive((Component) this.fishFoodTypeGroup, true);
      this.selectFoodGroup.Show();
      this.hitWaitGroup.Show();
      this.fishingGroup.Show();
      this.resultGroup.Show();
    }

    private void CloseAllCanvas()
    {
      this.SetActive((Component) this.howToGroup, false);
      this.SetActive((Component) this.fishFoodTypeGroup, false);
      this.selectFoodGroup.Hide();
      this.hitWaitGroup.Hide();
      this.fishingGroup.Hide();
      this.resultGroup.Hide();
    }

    private void SetActive(Component _c, bool _a)
    {
      if (Object.op_Equality((Object) _c, (Object) null) || Object.op_Equality((Object) _c.get_gameObject(), (Object) null) || _c.get_gameObject().get_activeSelf() == _a)
        return;
      _c.get_gameObject().SetActive(_a);
    }

    private void SetActive(GameObject _g, bool _a)
    {
      if (Object.op_Equality((Object) _g, (Object) null) || _g.get_activeSelf() == _a)
        return;
      _g.SetActive(_a);
    }

    private void SetEnable(Behaviour _b, bool _a)
    {
      if (Object.op_Equality((Object) _b, (Object) null) || _b.get_enabled() == _a)
        return;
      _b.set_enabled(_a);
    }

    public void SetAlpha(Graphic _g, float _a)
    {
      if (Object.op_Equality((Object) _g, (Object) null))
        return;
      Color color = _g.get_color();
      color.a = (__Null) (double) _a;
      _g.set_color(color);
    }

    public float GetAlpha(Graphic _g)
    {
      return Object.op_Equality((Object) _g, (Object) null) ? 0.0f : (float) _g.get_color().a;
    }

    private void PlaySystemSE(SoundPack.SystemSE se)
    {
      (!Singleton<Resources>.IsInstance() ? (SoundPack) null : Singleton<Resources>.Instance.SoundPack)?.Play(se);
    }

    public enum FadeType
    {
      None,
      FadeIn,
      Wait,
      FadeOut,
    }

    public class ArrowAnimParam
    {
      public Color normalColor = Color.get_white();
      public Color changeColor = Color.get_green();
      public Vector3 normalScale = Vector3.get_one();
      public Vector3 changeScale = Vector3.op_Multiply(Vector3.get_one(), 1.3f);
      public int moveIndex;
      public float fadeTime;
      public float waitTime;
      public IDisposable disposable;
      public Image image;

      public ArrowAnimParam()
      {
      }

      public ArrowAnimParam(int _moveIndex, Image _image)
      {
        this.moveIndex = _moveIndex;
        this.image = _image;
      }

      public ArrowAnimParam(
        int _moveIndex,
        Image _image,
        Color _normalColor,
        Vector3 _normalScale,
        Color _changeColor,
        Vector3 _changeScale)
      {
        this.moveIndex = _moveIndex;
        this.image = _image;
        this.normalColor = _normalColor;
        this.normalScale = _normalScale;
        this.changeColor = _changeColor;
        this.changeScale = _changeScale;
      }

      public FishingUI.FadeType FadeType { get; private set; }

      private static FishingDefinePack.UIParamGroup UIParam
      {
        get
        {
          return Singleton<Resources>.Instance.FishingDefinePack.UIParam;
        }
      }

      public Color IColor
      {
        get
        {
          return Object.op_Inequality((Object) this.image, (Object) null) ? ((Graphic) this.image).get_color() : Color.get_white();
        }
        private set
        {
          if (!Object.op_Inequality((Object) this.image, (Object) null))
            return;
          ((Graphic) this.image).set_color(value);
        }
      }

      public Vector3 IScale
      {
        get
        {
          return Object.op_Inequality((Object) this.image, (Object) null) ? ((Component) this.image).get_transform().get_localScale() : Vector3.get_one();
        }
        set
        {
          if (!Object.op_Inequality((Object) this.image, (Object) null))
            return;
          ((Component) this.image).get_transform().set_localScale(value);
        }
      }

      public static float DeltaTime
      {
        get
        {
          return Time.get_deltaTime();
        }
      }

      public void Reset()
      {
        this.fadeTime = 0.0f;
        this.waitTime = 0.0f;
        this.FadeType = FishingUI.FadeType.None;
        if (this.disposable != null)
          this.disposable.Dispose();
        this.IColor = this.normalColor;
        this.IScale = this.normalScale;
      }

      public void PlayFadeIn()
      {
        this.waitTime = 0.0f;
        switch (this.FadeType)
        {
          case FishingUI.FadeType.None:
            this.fadeTime = 0.0f;
            if (this.disposable != null)
              this.disposable.Dispose();
            IEnumerator _coroutine1 = this.FadeIn();
            this.disposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine1), false));
            break;
          case FishingUI.FadeType.FadeOut:
            if (this.disposable != null)
              this.disposable.Dispose();
            IEnumerator _coroutine2 = this.FadeIn();
            this.disposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine2), false));
            break;
        }
      }

      [DebuggerHidden]
      private IEnumerator FadeIn()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new FishingUI.ArrowAnimParam.\u003CFadeIn\u003Ec__Iterator0()
        {
          \u0024this = this
        };
      }

      public void PlayFadeOut()
      {
        this.waitTime = FishingUI.ArrowAnimParam.UIParam.ArrowAnimWaitTimeLimit;
        if (this.disposable != null)
          this.disposable.Dispose();
        IEnumerator _coroutine = this.FadeOut();
        this.disposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false));
      }

      [DebuggerHidden]
      private IEnumerator FadeOut()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new FishingUI.ArrowAnimParam.\u003CFadeOut\u003Ec__Iterator1()
        {
          \u0024this = this
        };
      }

      public void PlayCancel()
      {
        this.waitTime = FishingUI.ArrowAnimParam.UIParam.ArrowAnimWaitTimeLimit;
        if (this.disposable != null)
          this.disposable.Dispose();
        IEnumerator _coroutine = this.Cancel();
        this.disposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false));
      }

      [DebuggerHidden]
      private IEnumerator Cancel()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new FishingUI.ArrowAnimParam.\u003CCancel\u003Ec__Iterator2()
        {
          \u0024this = this
        };
      }

      private void SetImageState(Color _color, Vector3 _scale)
      {
        this.IColor = _color;
        this.IScale = _scale;
      }

      private Color Lerp(Color a, Color b, float t)
      {
        return new Color(Mathf.Lerp((float) a.r, (float) b.r, t), Mathf.Lerp((float) a.g, (float) b.g, t), Mathf.Lerp((float) a.b, (float) b.b, t), Mathf.Lerp((float) a.a, (float) b.a, t));
      }

      private void EasingImage(
        Image _image,
        float _count,
        float _limit,
        Color _color1,
        Color _color2,
        Vector3 _scale1,
        Vector3 _scale2)
      {
        float time = Mathf.InverseLerp(0.0f, _limit, _count);
        float t = EasingFunctions.EaseOutQuint(time, 1f);
        this.IColor = this.Lerp(_color1, _color2, t);
        this.IScale = Vector3.Lerp(_scale1, _scale2, t);
      }
    }

    public enum MouseInputType
    {
      Left,
      Right,
      Wheel,
      Move,
    }

    [Serializable]
    public abstract class GroupBase
    {
      public CanvasGroup group;

      public void Show()
      {
        if (((Component) this.group).get_gameObject().get_activeSelf())
          return;
        ((Component) this.group).get_gameObject().SetActive(true);
      }

      public void Hide()
      {
        if (!((Component) this.group).get_gameObject().get_activeSelf())
          return;
        ((Component) this.group).get_gameObject().SetActive(false);
      }

      public void SetAlpha(float _f)
      {
        this.group.set_alpha(_f);
      }

      public float Alpha
      {
        get
        {
          float? nullable = this.group != null ? new float?(this.group.get_alpha()) : new float?();
          return nullable.HasValue ? nullable.Value : 0.0f;
        }
        set
        {
          if (!Object.op_Implicit((Object) this.group))
            return;
          this.group.set_alpha(value);
        }
      }
    }

    [Serializable]
    public class SelectFoodGroup : FishingUI.GroupBase
    {
      public Text fishFoodNameText;
      public Image selectUpImage;
      public Image selectDownImage;
    }

    [Serializable]
    public class HitWaitGroup : FishingUI.GroupBase
    {
    }

    [Serializable]
    public class FishingGroup : FishingUI.GroupBase
    {
      public float heartPointImageWidth = 800f;
      public RectTransform[] heartPointRects = new RectTransform[0];
      public Image timerImage;
      public Image timerCircleImage;
      public Image[] imageOutlines;
      public CircleOutline[] circleOutlines;
      public Image hitImage;
    }

    [Serializable]
    public class ResultGroup : FishingUI.GroupBase
    {
      public Image getImage;
      public CanvasGroup resultWindow;
      public Text fishNameText;
      public Text rarelityText;
      public RawImage fishImage;
      public Text flavorText;
      public Text experiencePointText;
      public Text levelText;
      public Image experienceBarImage;
      public Text levelUpText;
    }
  }
}
