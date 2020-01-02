// Decompiled with JetBrains decompiler
// Type: AIProject.UI.StatusUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using AIProject.SaveData;
using AIProject.UI.Viewer;
using Illusion.Extensions;
using Manager;
using ReMotion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class StatusUI : MenuUIBehaviour
  {
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private CanvasGroup _contentCanvasGroup;
    private RectTransform _activeContent;
    [SerializeField]
    private RectTransform _playerContent;
    [SerializeField]
    private RectTransform _agentContent;
    [SerializeField]
    private Button _leftButton;
    [SerializeField]
    private Button _rightButton;
    [SerializeField]
    private Button[] _charaButtons;
    [SerializeField]
    private Image _tabFocusImage;
    [SerializeField]
    private Image _tabSelectionImage;
    [SerializeField]
    private Dictionary<int, CanvasGroup> _equipmentBackgrounds;
    [SerializeField]
    private Dictionary<int, CanvasGroup> _equipmentFlavorBackgrounds;
    [SerializeField]
    private Dictionary<int, CanvasGroup> _skillBackgrounds;
    [SerializeField]
    private Dictionary<int, CanvasGroup> _skillFlavorBackgrounds;
    [SerializeField]
    private Image _charaIconImage;
    [SerializeField]
    private Text _nameLabel;
    [SerializeField]
    private RawImage _cardRawImage;
    [SerializeField]
    private Image[] _phaseImages;
    [SerializeField]
    private Sprite _phaseActiveSprite;
    [SerializeField]
    private Sprite _phaseInactiveSprite;
    [SerializeField]
    private Toggle _lockParameterToggle;
    [SerializeField]
    private Text _sexLabel;
    [SerializeField]
    private Text _inventoryMaxLabel;
    [SerializeField]
    private Text _fishingLevelLabel;
    [SerializeField]
    private Text _totalPlayingTimeLabel;
    [SerializeField]
    private Button _slotExtendButton;
    [SerializeField]
    private Image _fishingExperienceImage;
    [SerializeField]
    [Header("ステータス")]
    private GameObject _moodContent;
    [SerializeField]
    private GameObject _motivationContent;
    [SerializeField]
    private Image _tempImage;
    [SerializeField]
    private Image[] _tempBorderImages;
    [SerializeField]
    private Slider _tempSlider;
    [SerializeField]
    private Image _hungerIcon;
    [SerializeField]
    private Text _hungerLabel;
    [SerializeField]
    private Image _physicalIcon;
    [SerializeField]
    private Text _physicalLabel;
    [SerializeField]
    private Image _moodIcon;
    [SerializeField]
    private Image[] _moodGraphs;
    [SerializeField]
    private Image[] _moodBorderGraphs;
    [SerializeField]
    private Image[] _motivationGraphs;
    [SerializeField]
    private Text _motivationLabel;
    [SerializeField]
    private Image _hIcon;
    [SerializeField]
    private Text _hLabel;
    [SerializeField]
    private float _hIconMin;
    [SerializeField]
    private float _hIconMax;
    [SerializeField]
    private AnimationCurve _hCurve;
    [SerializeField]
    private Text _sickNameLabel;
    [SerializeField]
    private Image _sickIcon;
    [SerializeField]
    [Header("フレーバースキル")]
    private Text _lifeStyleLabel;
    [SerializeField]
    private Text _pheromoneLabel;
    [SerializeField]
    private Text _reliabilityLabel;
    [SerializeField]
    private Text _reasonLabel;
    [SerializeField]
    private Text _instinctLabel;
    [SerializeField]
    private Text _dirtyLabel;
    [SerializeField]
    private Text _riskLabel;
    [SerializeField]
    private Text _darknessLabel;
    [SerializeField]
    private Text _sociabilityLabel;
    [SerializeField]
    private Button _handEQButton;
    [SerializeField]
    private Button _netEQButton;
    [SerializeField]
    private Button _shovelEQButton;
    [SerializeField]
    private Button _pickelEQButton;
    [SerializeField]
    private Button _rodEQButton;
    [SerializeField]
    private Button _hatEQButton;
    [SerializeField]
    private Button _ruckEQButton;
    [SerializeField]
    private Button _necklaceEQButton;
    [SerializeField]
    private Button _lampEQButton;
    [SerializeField]
    private Image _handEQImage;
    [SerializeField]
    private Image _netEQImage;
    [SerializeField]
    private Image _shovelEQImage;
    [SerializeField]
    private Image _pickelEQImage;
    [SerializeField]
    private Image _rodEQImage;
    [SerializeField]
    private Image _hatEQImage;
    [SerializeField]
    private Image _ruckEQImage;
    [SerializeField]
    private Image _necklaceEQImage;
    [SerializeField]
    private Image _lampEQImage;
    [SerializeField]
    private Sprite _noneSelectSprite;
    [SerializeField]
    private Text _equipItemNameLabel;
    [SerializeField]
    private Text _equipItemText;
    private IntReactiveProperty _eqFocusID;
    [SerializeField]
    private RectTransform _skillPanel;
    private IntReactiveProperty _skillFocusID;
    private int _skillSelectedID;
    [SerializeField]
    private Image _skillFocusImage;
    [SerializeField]
    private Button[] _normalSkillButtons;
    [SerializeField]
    private Button[] _hSkillButtons;
    [SerializeField]
    private Text[] _normalSkillTexts;
    [SerializeField]
    private Text[] _hSkillTexts;
    [SerializeField]
    private Text _skillNameLabel;
    [SerializeField]
    private Text _skillFlavorText;
    private IntReactiveProperty _selectedID;
    private IConnectableObservable<int> _selectIDChange;
    private int _focusID;
    private IDisposable _fadeSubscriber;
    private MenuUIBehaviour[] _menuUIList;
    private IDisposable _contentChangeDisposable;
    private IDisposable _pulseDisposable;
    private IDisposable[] _backgroundDisposables;

    public StatusUI()
    {
      AnimationCurve animationCurve = new AnimationCurve();
      animationCurve.set_keys(new Keyframe[4]
      {
        new Keyframe(0.0f, 1f),
        new Keyframe(0.5f, 1.2f),
        new Keyframe(0.9f, 0.9f),
        new Keyframe(1f, 1f)
      });
      this._hCurve = animationCurve;
      this._eqFocusID = new IntReactiveProperty(-1);
      this._skillFocusID = new IntReactiveProperty(-1);
      this._selectedID = new IntReactiveProperty(0);
      this._lifestyleID = new IntReactiveProperty(-1);
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public SystemMenuUI Observer { get; set; }

    public System.Action OnClose { get; set; }

    public int OpenID { get; set; }

    public Toggle LockParameterToggle
    {
      get
      {
        return this._lockParameterToggle;
      }
    }

    public IObservable<int> OnSelectIDChangedAsObservable()
    {
      if (this._selectIDChange == null)
      {
        this._selectIDChange = (IConnectableObservable<int>) Observable.Publish<int>(Observable.TakeUntilDestroy<int>((IObservable<M0>) this._selectedID, ((Component) this).get_gameObject()));
        this._selectIDChange.Connect();
      }
      return (IObservable<int>) this._selectIDChange;
    }

    private IntReactiveProperty _lifestyleID { get; }

    protected override void OnBeforeStart()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      StatusUI.\u003COnBeforeStart\u003Ec__AnonStorey5 startCAnonStorey5 = new StatusUI.\u003COnBeforeStart\u003Ec__AnonStorey5();
      // ISSUE: reference to a compiler-generated field
      startCAnonStorey5.\u0024this = this;
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (System.Action<M0>) new System.Action<bool>(startCAnonStorey5.\u003C\u003Em__0));
      // ISSUE: method pointer
      ((UnityEvent) this._leftButton.get_onClick()).AddListener(new UnityAction((object) startCAnonStorey5, __methodptr(\u003C\u003Em__1)));
      // ISSUE: method pointer
      ((UnityEvent) this._rightButton.get_onClick()).AddListener(new UnityAction((object) startCAnonStorey5, __methodptr(\u003C\u003Em__2)));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this.OnSelectIDChangedAsObservable(), (System.Action<M0>) new System.Action<int>(startCAnonStorey5.\u003C\u003Em__3));
      for (int index = 0; index < this._charaButtons.Length; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        StatusUI.\u003COnBeforeStart\u003Ec__AnonStorey3 startCAnonStorey3 = new StatusUI.\u003COnBeforeStart\u003Ec__AnonStorey3();
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey3.\u003C\u003Ef__ref\u00245 = startCAnonStorey5;
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey3.id = index;
        Button charaButton = this._charaButtons[index];
        if (Object.op_Inequality((Object) charaButton, (Object) null))
        {
          // ISSUE: method pointer
          ((UnityEvent) charaButton.get_onClick()).AddListener(new UnityAction((object) startCAnonStorey3, __methodptr(\u003C\u003Em__0)));
          // ISSUE: reference to a compiler-generated method
          ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) ((Selectable) charaButton).get_targetGraphic()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey3.\u003C\u003Em__1));
        }
      }
      // ISSUE: method pointer
      ((UnityEvent) this._slotExtendButton.get_onClick()).AddListener(new UnityAction((object) startCAnonStorey5, __methodptr(\u003C\u003Em__4)));
      CommonDefine commonDefine = Singleton<Resources>.Instance.CommonDefine;
      // ISSUE: reference to a compiler-generated field
      startCAnonStorey5.itemIDDefine = commonDefine.ItemIDDefine;
      // ISSUE: method pointer
      ((UnityEvent) this._handEQButton.get_onClick()).AddListener(new UnityAction((object) startCAnonStorey5, __methodptr(\u003C\u003Em__5)));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this._handEQImage), ((Component) this).get_gameObject()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey5.\u003C\u003Em__6));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerExitAsObservable((UIBehaviour) this._handEQImage), ((Component) this).get_gameObject()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey5.\u003C\u003Em__7));
      // ISSUE: method pointer
      ((UnityEvent) this._netEQButton.get_onClick()).AddListener(new UnityAction((object) startCAnonStorey5, __methodptr(\u003C\u003Em__8)));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this._netEQImage), ((Component) this).get_gameObject()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey5.\u003C\u003Em__9));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerExitAsObservable((UIBehaviour) this._netEQImage), ((Component) this).get_gameObject()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey5.\u003C\u003Em__A));
      // ISSUE: method pointer
      ((UnityEvent) this._shovelEQButton.get_onClick()).AddListener(new UnityAction((object) startCAnonStorey5, __methodptr(\u003C\u003Em__B)));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this._shovelEQImage), ((Component) this).get_gameObject()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey5.\u003C\u003Em__C));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerExitAsObservable((UIBehaviour) this._shovelEQImage), ((Component) this).get_gameObject()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey5.\u003C\u003Em__D));
      // ISSUE: method pointer
      ((UnityEvent) this._pickelEQButton.get_onClick()).AddListener(new UnityAction((object) startCAnonStorey5, __methodptr(\u003C\u003Em__E)));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this._pickelEQImage), ((Component) this).get_gameObject()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey5.\u003C\u003Em__F));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerExitAsObservable((UIBehaviour) this._pickelEQImage), ((Component) this).get_gameObject()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey5.\u003C\u003Em__10));
      // ISSUE: method pointer
      ((UnityEvent) this._rodEQButton.get_onClick()).AddListener(new UnityAction((object) startCAnonStorey5, __methodptr(\u003C\u003Em__11)));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this._rodEQImage), ((Component) this).get_gameObject()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey5.\u003C\u003Em__12));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerExitAsObservable((UIBehaviour) this._rodEQImage), ((Component) this).get_gameObject()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey5.\u003C\u003Em__13));
      // ISSUE: method pointer
      ((UnityEvent) this._hatEQButton.get_onClick()).AddListener(new UnityAction((object) startCAnonStorey5, __methodptr(\u003C\u003Em__14)));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this._hatEQImage), ((Component) this).get_gameObject()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey5.\u003C\u003Em__15));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerExitAsObservable((UIBehaviour) this._hatEQImage), ((Component) this).get_gameObject()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey5.\u003C\u003Em__16));
      // ISSUE: method pointer
      ((UnityEvent) this._ruckEQButton.get_onClick()).AddListener(new UnityAction((object) startCAnonStorey5, __methodptr(\u003C\u003Em__17)));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this._ruckEQImage), ((Component) this).get_gameObject()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey5.\u003C\u003Em__18));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerExitAsObservable((UIBehaviour) this._ruckEQImage), ((Component) this).get_gameObject()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey5.\u003C\u003Em__19));
      // ISSUE: method pointer
      ((UnityEvent) this._necklaceEQButton.get_onClick()).AddListener(new UnityAction((object) startCAnonStorey5, __methodptr(\u003C\u003Em__1A)));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this._necklaceEQImage), ((Component) this).get_gameObject()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey5.\u003C\u003Em__1B));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerExitAsObservable((UIBehaviour) this._necklaceEQImage), ((Component) this).get_gameObject()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey5.\u003C\u003Em__1C));
      // ISSUE: method pointer
      ((UnityEvent) this._lampEQButton.get_onClick()).AddListener(new UnityAction((object) startCAnonStorey5, __methodptr(\u003C\u003Em__1D)));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this._lampEQImage), ((Component) this).get_gameObject()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey5.\u003C\u003Em__1E));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerExitAsObservable((UIBehaviour) this._lampEQImage), ((Component) this).get_gameObject()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey5.\u003C\u003Em__1F));
      for (int index = 0; index < this._normalSkillButtons.Length; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        StatusUI.\u003COnBeforeStart\u003Ec__AnonStoreyF startCAnonStoreyF = new StatusUI.\u003COnBeforeStart\u003Ec__AnonStoreyF();
        // ISSUE: reference to a compiler-generated field
        startCAnonStoreyF.\u003C\u003Ef__ref\u00245 = startCAnonStorey5;
        // ISSUE: reference to a compiler-generated field
        startCAnonStoreyF.id = index;
        // ISSUE: reference to a compiler-generated field
        startCAnonStoreyF.button = this._normalSkillButtons[index];
        // ISSUE: reference to a compiler-generated field
        if (Object.op_Inequality((Object) startCAnonStoreyF.button, (Object) null))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          ((UnityEvent) startCAnonStoreyF.button.get_onClick()).AddListener(new UnityAction((object) startCAnonStoreyF, __methodptr(\u003C\u003Em__0)));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) ((Selectable) startCAnonStoreyF.button).get_targetGraphic()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStoreyF.\u003C\u003Em__1));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerExitAsObservable((UIBehaviour) ((Selectable) startCAnonStoreyF.button).get_targetGraphic()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStoreyF.\u003C\u003Em__2));
        }
      }
      for (int index = 0; index < this._hSkillButtons.Length; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        StatusUI.\u003COnBeforeStart\u003Ec__AnonStorey10 startCAnonStorey10 = new StatusUI.\u003COnBeforeStart\u003Ec__AnonStorey10();
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey10.\u003C\u003Ef__ref\u00245 = startCAnonStorey5;
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey10.id = this._normalSkillButtons.Length + index;
        // ISSUE: reference to a compiler-generated field
        startCAnonStorey10.button = this._hSkillButtons[index];
        // ISSUE: reference to a compiler-generated field
        if (Object.op_Inequality((Object) startCAnonStorey10.button, (Object) null))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          ((UnityEvent) startCAnonStorey10.button.get_onClick()).AddListener(new UnityAction((object) startCAnonStorey10, __methodptr(\u003C\u003Em__0)));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) ((Selectable) startCAnonStorey10.button).get_targetGraphic()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey10.\u003C\u003Em__1));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerExitAsObservable((UIBehaviour) ((Selectable) startCAnonStorey10.button).get_targetGraphic()), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey10.\u003C\u003Em__2));
        }
      }
      // ISSUE: method pointer
      ((UnityEvent<bool>) this._lockParameterToggle.onValueChanged).AddListener(new UnityAction<bool>((object) startCAnonStorey5, __methodptr(\u003C\u003Em__20)));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this._eqFocusID, (System.Action<M0>) new System.Action<int>(startCAnonStorey5.\u003C\u003Em__21));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this._skillFocusID, (System.Action<M0>) new System.Action<int>(startCAnonStorey5.\u003C\u003Em__22));
      KeyCodeDownCommand keyCodeDownCommand = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 324
      };
      // ISSUE: method pointer
      keyCodeDownCommand.TriggerEvent.AddListener(new UnityAction((object) startCAnonStorey5, __methodptr(\u003C\u003Em__23)));
      this._keyCommands.Add(keyCodeDownCommand);
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) new Func<long, bool>(startCAnonStorey5.\u003C\u003Em__24)), (System.Action<M0>) new System.Action<long>(startCAnonStorey5.\u003C\u003Em__25));
      this._contentCanvasGroup.set_alpha(1f);
      this._contentCanvasGroup.set_blocksRaycasts(true);
      if (Object.op_Equality((Object) this._activeContent, (Object) null))
        this._activeContent = this._playerContent;
      ((Selectable) this._leftButton).set_interactable(false);
      ((Selectable) this._rightButton).set_interactable(true);
      ((Component) this._skillPanel).get_gameObject().SetActive(false);
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      StatusUI.\u003COnBeforeStart\u003Ec__AnonStorey11 startCAnonStorey11 = new StatusUI.\u003COnBeforeStart\u003Ec__AnonStorey11();
      // ISSUE: reference to a compiler-generated field
      startCAnonStorey11.\u003C\u003Ef__ref\u00245 = startCAnonStorey5;
      // ISSUE: reference to a compiler-generated field
      startCAnonStorey11.param = (LifeStyleData.Param) null;
      // ISSUE: reference to a compiler-generated method
      UnityUIComponentExtensions.SubscribeToText((IObservable<string>) Observable.Select<int, string>((IObservable<M0>) this._lifestyleID, (Func<M0, M1>) new Func<int, string>(startCAnonStorey11.\u003C\u003Em__0)), this._lifeStyleLabel);
      Text lifeStyleLabel = this._lifeStyleLabel;
      ((Graphic) lifeStyleLabel).set_raycastTarget(true);
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<PointerEventData>(Observable.Where<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) lifeStyleLabel), (Func<M0, bool>) new Func<PointerEventData, bool>(startCAnonStorey11.\u003C\u003Em__1)), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey11.\u003C\u003Em__2));
      // ISSUE: reference to a compiler-generated method
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerExitAsObservable((UIBehaviour) lifeStyleLabel), (System.Action<M0>) new System.Action<PointerEventData>(startCAnonStorey11.\u003C\u003Em__3));
    }

    private void SetActiveControl(bool active)
    {
      Manager.Input instance = Singleton<Manager.Input>.Instance;
      IEnumerator coroutine;
      if (active)
      {
        if (((ReactiveProperty<int>) this._selectedID).get_Value() == this.OpenID)
          this._contentCanvasGroup.set_alpha(1f);
        else
          this._contentCanvasGroup.set_alpha(0.0f);
        ((Component) this._activeContent).get_gameObject().SetActiveIfDifferent(false);
        this._activeContent = this.OpenID != 0 ? this._agentContent : this._playerContent;
        ((ReactiveProperty<int>) this._selectedID).set_Value(this.OpenID);
        this.RefreshEquipments(this.OpenID);
        if (this.OpenID == 0)
          this.RefreshPlayerContent();
        else
          this.RefreshAgentContent(this.OpenID);
        this.RefreshName();
        ((Component) this._activeContent).get_gameObject().SetActiveIfDifferent(true);
        this.RefreshName();
        this.UsageRestriction();
        instance.FocusLevel = 0;
        instance.MenuElements = this.MenuUIList;
        coroutine = this.OpenCoroutine();
      }
      else
      {
        instance.ClearMenuElements();
        instance.FocusLevel = -1;
        coroutine = this.CloseCoroutine();
      }
      if (this._fadeSubscriber != null)
        this._fadeSubscriber.Dispose();
      this._fadeSubscriber = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (System.Action<M0>) (_ => {}), (System.Action<Exception>) (ex => Debug.LogException(ex)));
    }

    public MenuUIBehaviour[] MenuUIList
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

    [DebuggerHidden]
    private IEnumerator OpenCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new StatusUI.\u003COpenCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new StatusUI.\u003CCloseCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void ChangeContent(int id)
    {
      if (this._contentChangeDisposable != null)
        this._contentChangeDisposable.Dispose();
      this._contentChangeDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.ChangeContentCoroutine(id)), false), (System.Action<M0>) (_ => {}), (System.Action<Exception>) (ex => Debug.LogException(ex)));
    }

    [DebuggerHidden]
    private IEnumerator ChangeContentCoroutine(int id)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new StatusUI.\u003CChangeContentCoroutine\u003Ec__Iterator2()
      {
        id = id,
        \u0024this = this
      };
    }

    private void Close()
    {
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
      this.Observer.ChangeBackground(-1);
      this.OpenID = 0;
      System.Action onClose = this.OnClose;
      if (onClose == null)
        return;
      onClose();
    }

    private void RefreshName()
    {
      Dictionary<int, Sprite> actorIconTable = Singleton<Resources>.Instance.itemIconTables.ActorIconTable;
      if (((ReactiveProperty<int>) this._selectedID).get_Value() == 0)
      {
        this._nameLabel.set_text(Singleton<Manager.Map>.Instance.Player.CharaName);
        Sprite sprite;
        if (!actorIconTable.TryGetValue(-99, out sprite))
          return;
        this._charaIconImage.set_sprite(sprite);
        ((Graphic) this._charaIconImage).set_color(Color32.op_Implicit(new Color32((byte) 133, (byte) 214, (byte) 83, byte.MaxValue)));
      }
      else
      {
        AgentActor agentActor;
        if (!Singleton<Manager.Map>.Instance.AgentTable.TryGetValue(((ReactiveProperty<int>) this._selectedID).get_Value() - 1, ref agentActor))
          return;
        this._nameLabel.set_text(string.Format("{0}", (object) agentActor.CharaName));
        Sprite sprite;
        if (!actorIconTable.TryGetValue(((ReactiveProperty<int>) this._selectedID).get_Value() - 1, out sprite))
          return;
        this._charaIconImage.set_sprite(sprite);
        ((Graphic) this._charaIconImage).set_color(Color.get_white());
      }
    }

    private void RefreshPlayerContent()
    {
      ((Component) this._skillPanel).get_gameObject().SetActive(false);
      if (!Singleton<Manager.Map>.IsInstance() || !Singleton<Game>.IsInstance())
        return;
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      if (Object.op_Equality((Object) player, (Object) null) || player.PlayerData == null)
        return;
      PlayerData playerData = player.PlayerData;
      if (!playerData.CharaFileName.IsNullOrEmpty())
      {
        string path = player.ChaControl.chaFile.ConvertCharaFilePath(playerData.CharaFileName, playerData.Sex, false);
        if (File.Exists(path))
          this._cardRawImage.set_texture((Texture) PngAssist.ChangeTextureFromByte(PngFile.LoadPngBytes(path), 0, 0, (TextureFormat) 5, false));
        else
          this._cardRawImage.set_texture((Texture) null);
      }
      else
        this._cardRawImage.set_texture((Texture) null);
      ((Component) this._cardRawImage).get_gameObject().SetActiveIfDifferent(Object.op_Inequality((Object) this._cardRawImage.get_texture(), (Object) null));
      this._sexLabel.set_text(playerData.Sex != (byte) 0 ? "女性" : "男性");
      if (playerData.Sex == (byte) 1 && player.ChaControl.fileParam.futanari)
      {
        Text sexLabel = this._sexLabel;
        sexLabel.set_text(sexLabel.get_text() + "　ふたなり");
      }
      this._inventoryMaxLabel.set_text(string.Format("{0}", (object) playerData.InventorySlotMax));
      this._fishingLevelLabel.set_text(string.Format("{0}", (object) playerData.FishingSkill.Level));
      this._fishingExperienceImage.set_fillAmount(playerData.FishingSkill.Experience / (float) playerData.FishingSkill.NextExperience);
    }

    private void RefreshAgentContent(int id)
    {
      AgentActor agentActor;
      if (!Singleton<Manager.Map>.Instance.AgentTable.TryGetValue(id - 1, ref agentActor))
        return;
      ChaFileGameInfo fileGameInfo = agentActor.ChaControl.fileGameInfo;
      AgentData agentData = agentActor.AgentData;
      if (!agentData.CharaFileName.IsNullOrEmpty())
        this._cardRawImage.set_texture((Texture) PngAssist.ChangeTextureFromByte(PngFile.LoadPngBytes(agentActor.ChaControl.chaFile.ConvertCharaFilePath(agentData.CharaFileName, (byte) 1, false)), 0, 0, (TextureFormat) 5, false));
      else
        this._cardRawImage.set_texture((Texture) null);
      ((Component) this._cardRawImage).get_gameObject().SetActiveIfDifferent(Object.op_Inequality((Object) this._cardRawImage.get_texture(), (Object) null));
      for (int index = 0; index < this._phaseImages.Length; ++index)
      {
        Sprite sprite = this._phaseImages[index].get_sprite();
        if (index <= agentActor.ChaControl.fileGameInfo.phase)
        {
          if (Object.op_Inequality((Object) sprite, (Object) this._phaseActiveSprite))
            this._phaseImages[index].set_sprite(this._phaseActiveSprite);
        }
        else if (Object.op_Inequality((Object) sprite, (Object) this._phaseInactiveSprite))
          this._phaseImages[index].set_sprite(this._phaseInactiveSprite);
      }
      float num1 = agentData.StatsTable[0];
      int index1 = 0;
      if ((double) num1 <= (double) agentActor.ChaControl.fileGameInfo.tempBound.lower)
        index1 = 1;
      else if ((double) num1 >= (double) agentActor.ChaControl.fileGameInfo.tempBound.upper)
        index1 = 2;
      this._tempImage.set_sprite(Singleton<Resources>.Instance.itemIconTables.StatusIconTable[0][index1]);
      this._tempSlider.set_value(num1 / 100f);
      float num2 = agentActor.ChaControl.fileGameInfo.tempBound.lower / 100f;
      float num3 = agentActor.ChaControl.fileGameInfo.tempBound.upper / 100f;
      RectTransform rectTransform1 = ((Graphic) this._tempBorderImages[0]).get_rectTransform();
      rectTransform1.set_anchorMin(new Vector2(0.0f, 0.0f));
      rectTransform1.set_anchorMax(new Vector2(num2, 1f));
      rectTransform1.set_offsetMin(new Vector2(0.0f, (float) rectTransform1.get_offsetMin().y));
      rectTransform1.set_offsetMax(new Vector2(0.0f, (float) rectTransform1.get_offsetMax().y));
      RectTransform rectTransform2 = ((Graphic) this._tempBorderImages[1]).get_rectTransform();
      rectTransform2.set_anchorMin(new Vector2(num2, 0.0f));
      rectTransform2.set_anchorMax(new Vector2(num3, 1f));
      rectTransform2.set_offsetMin(new Vector2(0.0f, (float) rectTransform2.get_offsetMin().y));
      rectTransform2.set_offsetMax(new Vector2(0.0f, (float) rectTransform2.get_offsetMax().y));
      RectTransform rectTransform3 = ((Graphic) this._tempBorderImages[2]).get_rectTransform();
      rectTransform3.set_anchorMin(new Vector2(num3, 0.0f));
      rectTransform3.set_anchorMax(new Vector2(1f, 1f));
      rectTransform3.set_offsetMin(new Vector2(0.0f, (float) rectTransform3.get_offsetMin().y));
      rectTransform3.set_offsetMax(new Vector2(0.0f, (float) rectTransform3.get_offsetMax().y));
      float num4 = agentData.StatsTable[2];
      int index2 = 0;
      if ((double) num4 < 30.0)
        index2 = 1;
      this._hungerIcon.set_sprite(Singleton<Resources>.Instance.itemIconTables.StatusIconTable[2][index2]);
      this._hungerLabel.set_text(string.Format("{0}", (object) Mathf.Clamp(Mathf.FloorToInt((float) (int) num4), 0, 100)));
      float num5 = 100f - agentData.StatsTable[3];
      Sprite sprite1;
      if (Singleton<Resources>.Instance.itemIconTables.StatusIconTable[3].TryGetValue(Mathf.Min((int) ((double) num5 / 25.0), 3), out sprite1))
        this._physicalIcon.set_sprite(sprite1);
      this._physicalLabel.set_text(string.Format("{0}", (object) Mathf.Clamp(Mathf.FloorToInt((float) (int) num5), 0, 100)));
      bool flag1 = agentActor.ChaControl.fileGameInfo.phase >= 2;
      if (this._moodContent.get_activeSelf() != flag1)
        this._moodContent.SetActive(flag1);
      if (flag1)
      {
        float num6 = agentData.StatsTable[1];
        int index3 = 0;
        Color32 color32;
        ((Color32) ref color32).\u002Ector((byte) 100, (byte) 185, (byte) 22, byte.MaxValue);
        if ((double) num6 >= (double) agentActor.ChaControl.fileGameInfo.moodBound.upper)
        {
          index3 = 1;
          ((Color32) ref color32).\u002Ector((byte) 245, (byte) 178, (byte) 24, byte.MaxValue);
        }
        else if ((double) num6 <= (double) agentActor.ChaControl.fileGameInfo.moodBound.lower)
        {
          index3 = 2;
          ((Color32) ref color32).\u002Ector((byte) 35, (byte) 112, (byte) 216, byte.MaxValue);
        }
        this._moodIcon.set_sprite(Singleton<Resources>.Instance.itemIconTables.StatusIconTable[1][index3]);
        int num7 = (int) ((double) num6 / 10.0);
        for (int index4 = 0; index4 < this._moodGraphs.Length; ++index4)
        {
          Image moodGraph = this._moodGraphs[index4];
          ((Graphic) moodGraph).set_color(Color32.op_Implicit(color32));
          bool flag2 = index4 < num7;
          if (((Component) moodGraph).get_gameObject().get_activeSelf() != flag2)
            ((Component) this._moodGraphs[index4]).get_gameObject().SetActive(flag2);
        }
        float num8 = agentActor.ChaControl.fileGameInfo.moodBound.lower / 100f;
        float num9 = agentActor.ChaControl.fileGameInfo.moodBound.upper / 100f;
        RectTransform rectTransform4 = ((Graphic) this._moodBorderGraphs[0]).get_rectTransform();
        rectTransform4.set_anchorMin(new Vector2(0.0f, 0.0f));
        rectTransform4.set_anchorMax(new Vector2(num8, 1f));
        rectTransform4.set_offsetMin(new Vector2(0.0f, (float) rectTransform4.get_offsetMin().y));
        rectTransform4.set_offsetMax(new Vector2(0.0f, (float) rectTransform4.get_offsetMax().y));
        RectTransform rectTransform5 = ((Graphic) this._moodBorderGraphs[1]).get_rectTransform();
        rectTransform5.set_anchorMin(new Vector2(num8, 0.0f));
        rectTransform5.set_anchorMax(new Vector2(num9, 1f));
        rectTransform5.set_offsetMin(new Vector2(0.0f, (float) rectTransform5.get_offsetMin().y));
        rectTransform5.set_offsetMax(new Vector2(0.0f, (float) rectTransform5.get_offsetMax().y));
        RectTransform rectTransform6 = ((Graphic) this._moodBorderGraphs[2]).get_rectTransform();
        rectTransform6.set_anchorMin(new Vector2(num9, 0.0f));
        rectTransform6.set_anchorMax(new Vector2(1f, 1f));
        rectTransform6.set_offsetMin(new Vector2(0.0f, (float) rectTransform6.get_offsetMin().y));
        rectTransform6.set_offsetMax(new Vector2(0.0f, (float) rectTransform6.get_offsetMax().y));
      }
      bool flag3 = agentActor.ChaControl.fileGameInfo.phase >= 2;
      if (this._motivationContent.get_activeSelf() != flag3)
        this._motivationContent.SetActive(flag3);
      if (flag3)
      {
        float num6 = agentData.StatsTable[5];
        int num7 = (int) ((double) num6 / 10.0);
        for (int index3 = 0; index3 < this._motivationGraphs.Length; ++index3)
        {
          Image motivationGraph = this._motivationGraphs[index3];
          bool flag2 = index3 < num7;
          if (((Component) motivationGraph).get_gameObject().get_activeSelf() != flag2)
            ((Component) this._motivationGraphs[index3]).get_gameObject().SetActive(flag2);
        }
        this._motivationLabel.set_text(string.Format("{0}", (object) Mathf.Clamp(Mathf.FloorToInt(num6), 0, 100)));
      }
      float num10 = agentData.StatsTable[6];
      if ((double) num10 <= 20.0)
        ((Graphic) this._hIcon).set_color(Color32.op_Implicit(new Color32((byte) 134, (byte) 17, (byte) 11, byte.MaxValue)));
      else
        ((Graphic) this._hIcon).set_color(Color32.op_Implicit(new Color32((byte) 215, (byte) 102, (byte) 184, byte.MaxValue)));
      float hRate = num10 / 100f;
      if (this._pulseDisposable != null)
        this._pulseDisposable.Dispose();
      this._pulseDisposable = ObservableExtensions.Subscribe<TimeInterval<float>>(Observable.TakeUntilDestroy<TimeInterval<float>>(Observable.Repeat<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(1f, true), true)), (Component) this._hIcon), (System.Action<M0>) (x => ((Component) this._hIcon).get_transform().set_localScale(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_one(), Mathf.Lerp(this._hIconMin, this._hIconMax, hRate)), this._hCurve.Evaluate(((TimeInterval<float>) ref x).get_Value())))));
      this._hLabel.set_text(string.Format("{0}", (object) Mathf.Clamp(Mathf.FloorToInt((float) (int) num10), 0, 100)));
      int id1 = agentData.SickState.ID;
      ((Component) this._sickIcon).get_gameObject().SetActive(id1 > -1);
      if (id1 > -1)
      {
        this._sickNameLabel.set_text(string.Format("{0}", (object) AIProject.Definitions.Sickness.NameTable.get_Item(id1)));
        Sprite sprite2;
        if (Singleton<Resources>.Instance.itemIconTables.SickIconTable.TryGetValue(id1, out sprite2))
        {
          this._sickIcon.set_sprite(sprite2);
        }
        else
        {
          this._sickIcon.set_sprite((Sprite) null);
          ((Component) this._sickIcon).get_gameObject().SetActive(false);
        }
      }
      else
        this._sickNameLabel.set_text(string.Empty);
      ((ReactiveProperty<int>) this._lifestyleID).set_Value(fileGameInfo.lifestyle);
      this._pheromoneLabel.set_text(string.Format("{0}", (object) fileGameInfo.flavorState[0]));
      this._reliabilityLabel.set_text(string.Format("{0}", (object) fileGameInfo.flavorState[1]));
      this._reasonLabel.set_text(string.Format("{0}", (object) fileGameInfo.flavorState[2]));
      this._instinctLabel.set_text(string.Format("{0}", (object) fileGameInfo.flavorState[3]));
      this._dirtyLabel.set_text(string.Format("{0}", (object) fileGameInfo.flavorState[4]));
      this._riskLabel.set_text(string.Format("{0}", (object) fileGameInfo.flavorState[5]));
      this._sociabilityLabel.set_text(string.Format("{0}", (object) fileGameInfo.flavorState[7]));
      this._darknessLabel.set_text(string.Format("{0}", (object) fileGameInfo.flavorState[6]));
      ((Component) this._skillPanel).get_gameObject().SetActive(true);
      foreach (Selectable normalSkillButton in this._normalSkillButtons)
        normalSkillButton.set_interactable(fileGameInfo.phase >= 2);
      foreach (Selectable hSkillButton in this._hSkillButtons)
        hSkillButton.set_interactable(fileGameInfo.phase >= 2);
      this._lockParameterToggle.SetIsOnWithoutCallback(agentData.ParameterLock);
    }

    private void RefreshEquipments(int id)
    {
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      if (id == 0)
      {
        if (Object.op_Equality((Object) Singleton<Manager.Map>.Instance.Player, (Object) null) || Singleton<Manager.Map>.Instance.Player.PlayerData == null)
          return;
        PlayerData playerData = Singleton<Manager.Map>.Instance.Player.PlayerData;
        this.RefreshGloveEquipment(playerData.EquipedGloveItem);
        this.RefreshNetEquipment(playerData.EquipedNetItem);
        this.RefreshShovelEquipment(playerData.EquipedShovelItem);
        this.RefreshPickelEquipment(playerData.EquipedPickelItem);
        this.RefreshRodEquipment(playerData.EquipedFishingItem);
        this.RefreshHatEquipment(playerData.EquipedHeadItem);
        this.RefreshRuckEquipment(playerData.EquipedBackItem);
        this.RefreshNeckEquipment(playerData.EquipedNeckItem);
        this.RefreshLampEquipment(playerData.EquipedLampItem);
      }
      else
      {
        AgentActor agentActor;
        if (!Singleton<Manager.Map>.Instance.AgentTable.TryGetValue(id - 1, ref agentActor))
          return;
        AgentData agentData = agentActor.AgentData;
        this.RefreshGloveEquipment(agentData.EquipedGloveItem);
        this.RefreshNetEquipment(agentData.EquipedNetItem);
        this.RefreshShovelEquipment(agentData.EquipedShovelItem);
        this.RefreshPickelEquipment(agentData.EquipedPickelItem);
        this.RefreshRodEquipment(agentData.EquipedFishingItem);
        this.RefreshHatEquipment(agentData.EquipedHeadItem);
        this.RefreshRuckEquipment(agentData.EquipedBackItem);
        this.RefreshNeckEquipment(agentData.EquipedNeckItem);
        this.RefreshLampEquipment(agentData.EquipedLampItem);
        foreach (KeyValuePair<int, int> keyValuePair in agentActor.ChaControl.fileGameInfo.normalSkill)
          this.RefreshNormalSkill(keyValuePair.Key);
        foreach (KeyValuePair<int, int> keyValuePair in agentActor.ChaControl.fileGameInfo.hSkill)
          this.RefreshHSkill(keyValuePair.Key);
      }
    }

    private void RefreshGloveEquipment(StuffItem info)
    {
      Dictionary<int, int> dictionary;
      if (!Singleton<Resources>.Instance.itemIconTables.EquipmentIconTable.TryGetValue(0, out dictionary))
        return;
      CommonDefine.ItemIDDefines itemIdDefine = Singleton<Resources>.Instance.CommonDefine.ItemIDDefine;
      this._handEQImage.set_sprite(Singleton<Resources>.Instance.itemIconTables.ActionIconTable[info.ID != itemIdDefine.RareGloveID.itemID ? (info.ID != itemIdDefine.SRareGloveID.itemID ? dictionary[0] : dictionary[2]) : dictionary[1]]);
      this.RefreshEquipmentExplanation();
    }

    private void RefreshNetEquipment(StuffItem info)
    {
      Dictionary<int, int> dictionary;
      if (!Singleton<Resources>.Instance.itemIconTables.EquipmentIconTable.TryGetValue(5, out dictionary))
        return;
      CommonDefine.ItemIDDefines itemIdDefine = Singleton<Resources>.Instance.CommonDefine.ItemIDDefine;
      Sprite sprite;
      if (Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(info.ID != itemIdDefine.NetID.itemID ? (info.ID != itemIdDefine.RareNetID.itemID ? (info.ID != itemIdDefine.SRareNetID.itemID ? -1 : dictionary[2]) : dictionary[1]) : dictionary[0], out sprite))
        this._netEQImage.set_sprite(sprite);
      else
        this._netEQImage.set_sprite(this._noneSelectSprite);
      this.RefreshEquipmentExplanation();
    }

    private void RefreshShovelEquipment(StuffItem info)
    {
      Dictionary<int, int> dictionary;
      if (!Singleton<Resources>.Instance.itemIconTables.EquipmentIconTable.TryGetValue(3, out dictionary))
        return;
      CommonDefine.ItemIDDefines itemIdDefine = Singleton<Resources>.Instance.CommonDefine.ItemIDDefine;
      Sprite sprite;
      if (Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(info.ID != itemIdDefine.ShovelID.itemID ? (info.ID != itemIdDefine.RareShovelID.itemID ? (info.ID != itemIdDefine.SRareShovelID.itemID ? -1 : dictionary[2]) : dictionary[1]) : dictionary[0], out sprite))
        this._shovelEQImage.set_sprite(sprite);
      else
        this._shovelEQImage.set_sprite(this._noneSelectSprite);
      this.RefreshEquipmentExplanation();
    }

    private void RefreshPickelEquipment(StuffItem info)
    {
      Dictionary<int, int> dictionary;
      if (!Singleton<Resources>.Instance.itemIconTables.EquipmentIconTable.TryGetValue(4, out dictionary))
        return;
      CommonDefine.ItemIDDefines itemIdDefine = Singleton<Resources>.Instance.CommonDefine.ItemIDDefine;
      Sprite sprite;
      if (Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(info.ID != itemIdDefine.PickelID.itemID ? (info.ID != itemIdDefine.RarePickelID.itemID ? (info.ID != itemIdDefine.SRarePickelID.itemID ? -1 : dictionary[2]) : dictionary[1]) : dictionary[0], out sprite))
        this._pickelEQImage.set_sprite(sprite);
      else
        this._pickelEQImage.set_sprite(this._noneSelectSprite);
      this.RefreshEquipmentExplanation();
    }

    private void RefreshRodEquipment(StuffItem info)
    {
      Dictionary<int, int> dictionary;
      if (!Singleton<Resources>.Instance.itemIconTables.EquipmentIconTable.TryGetValue(6, out dictionary))
        return;
      CommonDefine.ItemIDDefines itemIdDefine = Singleton<Resources>.Instance.CommonDefine.ItemIDDefine;
      Sprite sprite;
      if (Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(info.ID != itemIdDefine.RodID.itemID ? -1 : dictionary[0], out sprite))
        this._rodEQImage.set_sprite(sprite);
      else
        this._rodEQImage.set_sprite(this._noneSelectSprite);
      this.RefreshEquipmentExplanation();
    }

    private void RefreshHatEquipment(StuffItem info)
    {
      StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(info.CategoryID, info.ID);
      if (Object.op_Inequality((Object) this._hatEQImage, (Object) null))
      {
        if (stuffItemInfo != null)
          Resources.ItemIconTables.SetIcon(Resources.ItemIconTables.IconCategory.Item, stuffItemInfo.IconID, this._hatEQImage, true);
        else
          this._hatEQImage.set_sprite(this._noneSelectSprite);
      }
      this.RefreshEquipmentExplanation();
    }

    private void RefreshRuckEquipment(StuffItem info)
    {
      StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(info.CategoryID, info.ID);
      if (Object.op_Inequality((Object) this._hatEQImage, (Object) null))
      {
        if (stuffItemInfo != null)
          Resources.ItemIconTables.SetIcon(Resources.ItemIconTables.IconCategory.Item, stuffItemInfo.IconID, this._ruckEQImage, true);
        else
          this._ruckEQImage.set_sprite(this._noneSelectSprite);
      }
      this.RefreshEquipmentExplanation();
    }

    private void RefreshNeckEquipment(StuffItem info)
    {
      StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(info.CategoryID, info.ID);
      if (Object.op_Inequality((Object) this._necklaceEQImage, (Object) null))
      {
        if (stuffItemInfo != null)
          Resources.ItemIconTables.SetIcon(Resources.ItemIconTables.IconCategory.Item, stuffItemInfo.IconID, this._necklaceEQImage, true);
        else
          this._necklaceEQImage.set_sprite(this._noneSelectSprite);
      }
      this.RefreshEquipmentExplanation();
    }

    private void RefreshLampEquipment(StuffItem info)
    {
      StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(info.CategoryID, info.ID);
      if (Object.op_Inequality((Object) this._lampEQImage, (Object) null))
      {
        if (stuffItemInfo != null)
          Resources.ItemIconTables.SetIcon(Resources.ItemIconTables.IconCategory.Item, stuffItemInfo.IconID, this._lampEQImage, true);
        else
          this._lampEQImage.set_sprite(this._noneSelectSprite);
      }
      this.RefreshEquipmentExplanation();
    }

    private void RefreshAccessory(
      int id,
      ChaControlDefine.ExtraAccessoryParts parts,
      StuffItem info)
    {
      int id1 = info.CategoryID == -1 ? -1 : info.ID;
      if (id == 0)
      {
        Singleton<Manager.Map>.Instance.Player.ChaControl.ChangeExtraAccessory(parts, id1);
        Singleton<Manager.Map>.Instance.Player.ChaControl.ShowExtraAccessory(parts, true);
      }
      else
      {
        AgentActor agentActor = Singleton<Manager.Map>.Instance.AgentTable.get_Item(id - 1);
        agentActor.ChaControl.ChangeExtraAccessory(parts, id1);
        agentActor.ChaControl.ShowExtraAccessory(parts, true);
      }
    }

    private void RefreshEquipmentExplanation()
    {
      this.RefreshEquipmentExplanation(((ReactiveProperty<int>) this._eqFocusID).get_Value());
    }

    private void RefreshEquipmentExplanation(int id)
    {
      string str1 = string.Empty;
      string str2 = string.Empty;
      StuffItem stuffItem = (StuffItem) null;
      if (Singleton<Manager.Map>.IsInstance())
      {
        if (((ReactiveProperty<int>) this._selectedID).get_Value() == 0)
        {
          if (Object.op_Inequality((Object) Singleton<Manager.Map>.Instance.Player, (Object) null))
          {
            PlayerData playerData = Singleton<Manager.Map>.Instance.Player.PlayerData;
            switch (id)
            {
              case 0:
                stuffItem = playerData.EquipedGloveItem;
                break;
              case 1:
                stuffItem = playerData.EquipedNetItem;
                break;
              case 2:
                stuffItem = playerData.EquipedShovelItem;
                break;
              case 3:
                stuffItem = playerData.EquipedPickelItem;
                break;
              case 4:
                stuffItem = playerData.EquipedFishingItem;
                break;
              case 5:
                stuffItem = playerData.EquipedHeadItem;
                break;
              case 6:
                stuffItem = playerData.EquipedBackItem;
                break;
              case 7:
                stuffItem = playerData.EquipedNeckItem;
                break;
              case 8:
                stuffItem = playerData.EquipedLampItem;
                break;
            }
          }
        }
        else
        {
          AgentActor agentActor;
          if (!Singleton<Manager.Map>.Instance.AgentTable.TryGetValue(((ReactiveProperty<int>) this._selectedID).get_Value() - 1, ref agentActor))
            return;
          AgentData agentData = agentActor.AgentData;
          switch (id)
          {
            case 0:
              stuffItem = agentData.EquipedGloveItem;
              break;
            case 1:
              stuffItem = agentData.EquipedNetItem;
              break;
            case 2:
              stuffItem = agentData.EquipedShovelItem;
              break;
            case 3:
              stuffItem = agentData.EquipedPickelItem;
              break;
            case 4:
              stuffItem = agentData.EquipedFishingItem;
              break;
            case 5:
              stuffItem = agentData.EquipedHeadItem;
              break;
            case 6:
              stuffItem = agentData.EquipedBackItem;
              break;
            case 7:
              stuffItem = agentData.EquipedNeckItem;
              break;
            case 8:
              stuffItem = agentData.EquipedLampItem;
              break;
          }
        }
      }
      if (stuffItem != null)
      {
        StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(stuffItem.CategoryID, stuffItem.ID);
        if (stuffItemInfo != null)
        {
          str1 = stuffItemInfo.Name;
          str2 = stuffItemInfo.Explanation;
        }
      }
      this._equipItemNameLabel.set_text(str1);
      this._equipItemText.set_text(str2);
    }

    private void RefreshNormalSkill(int id)
    {
      if (id >= this._normalSkillTexts.Length)
      {
        Debug.Log((object) "通常スキル");
      }
      else
      {
        AgentActor agentActor;
        if (!Singleton<Manager.Map>.Instance.AgentTable.TryGetValue(((ReactiveProperty<int>) this._selectedID).get_Value() - 1, ref agentActor))
          return;
        StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(16, agentActor.ChaControl.fileGameInfo.normalSkill[id]);
        this._normalSkillTexts[id].set_text(stuffItemInfo?.Name ?? "ーーーーー");
        this.RefreshSkillExplanation();
      }
    }

    private void RefreshHSkill(int id)
    {
      if (id >= this._hSkillTexts.Length)
      {
        Debug.Log((object) "Hスキル");
      }
      else
      {
        AgentActor agentActor;
        if (!Singleton<Manager.Map>.Instance.AgentTable.TryGetValue(((ReactiveProperty<int>) this._selectedID).get_Value() - 1, ref agentActor))
          return;
        StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(17, agentActor.ChaControl.fileGameInfo.hSkill[id]);
        this._hSkillTexts[id].set_text(stuffItemInfo?.Name ?? "ーーーーー");
        this.RefreshSkillExplanation();
      }
    }

    private void RefreshSkillExplanation()
    {
      this.RefreshSkillExplanation(((ReactiveProperty<int>) this._skillFocusID).get_Value());
    }

    private void RefreshSkillExplanation(int id)
    {
      AgentActor agentActor;
      if (id < 0 || !Singleton<Manager.Map>.Instance.AgentTable.TryGetValue(((ReactiveProperty<int>) this._selectedID).get_Value() - 1, ref agentActor))
        return;
      if (id < this._normalSkillButtons.Length)
      {
        StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(16, agentActor.ChaControl.fileGameInfo.normalSkill[id]);
        if (stuffItemInfo != null)
        {
          this._skillNameLabel.set_text(stuffItemInfo.Name);
          this._skillFlavorText.set_text(stuffItemInfo.Explanation);
        }
        else
        {
          this._skillNameLabel.set_text(string.Empty);
          this._skillFlavorText.set_text(string.Empty);
        }
      }
      else
      {
        int index = id - this._normalSkillButtons.Length;
        StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(17, agentActor.ChaControl.fileGameInfo.hSkill[index]);
        if (stuffItemInfo != null)
        {
          this._skillNameLabel.set_text(stuffItemInfo.Name);
          this._skillFlavorText.set_text(stuffItemInfo.Explanation);
        }
        else
        {
          this._skillNameLabel.set_text(string.Empty);
          this._skillFlavorText.set_text(string.Empty);
        }
      }
    }

    private void OnUpdate()
    {
      ((Component) this._tabFocusImage).get_transform().set_position(((Component) this._charaButtons[this._focusID]).get_transform().get_position());
      ((Transform) ((Graphic) this._tabSelectionImage).get_rectTransform()).set_position(((Component) this._charaButtons[((ReactiveProperty<int>) this._selectedID).get_Value()]).get_transform().get_position());
      if (((Component) this._playerContent).get_gameObject().get_activeSelf() && Singleton<Game>.IsInstance() && Singleton<Game>.Instance.Environment != null)
      {
        TimeSpan timeSpan = Singleton<Game>.Instance.Environment.TotalPlayTime.TimeSpan;
        this._totalPlayingTimeLabel.set_text(string.Format("{0} : {1:00} : {2:00}", (object) (int) timeSpan.TotalHours, (object) timeSpan.Minutes, (object) timeSpan.Seconds));
      }
      if (((ReactiveProperty<int>) this._skillFocusID).get_Value() <= -1)
        return;
      if (((ReactiveProperty<int>) this._skillFocusID).get_Value() < this._normalSkillButtons.Length)
      {
        Button normalSkillButton = this._normalSkillButtons[((ReactiveProperty<int>) this._skillFocusID).get_Value()];
        Vector3 vector3;
        ((Vector3) ref vector3).\u002Ector((float) ((Component) this._skillFocusImage).get_transform().get_position().x, (float) ((Component) normalSkillButton).get_transform().get_position().y, 0.0f);
        ((Component) this._skillFocusImage).get_transform().set_position(vector3);
      }
      else
      {
        Button hSkillButton = this._hSkillButtons[((ReactiveProperty<int>) this._skillFocusID).get_Value() - this._normalSkillButtons.Length];
        Vector3 vector3;
        ((Vector3) ref vector3).\u002Ector((float) ((Component) this._skillFocusImage).get_transform().get_position().x, (float) ((Component) hSkillButton).get_transform().get_position().y, 0.0f);
        ((Component) this._skillFocusImage).get_transform().set_position(vector3);
      }
    }

    private void ChangeBackground(int id)
    {
      if (this._backgroundDisposables == null)
        this._backgroundDisposables = new IDisposable[this._equipmentBackgrounds.Count + this._equipmentFlavorBackgrounds.Count + this._skillBackgrounds.Count + this._skillFlavorBackgrounds.Count];
      foreach (IDisposable backgroundDisposable in this._backgroundDisposables)
        backgroundDisposable?.Dispose();
      IObservable<TimeInterval<float>> observable = (IObservable<TimeInterval<float>>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(0.3f, true), true);
      int num = 0;
      using (Dictionary<int, CanvasGroup>.Enumerator enumerator = this._equipmentBackgrounds.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, CanvasGroup> kvp = enumerator.Current;
          float startAlpha = kvp.Value.get_alpha();
          int destAlpha = kvp.Key != id ? 0 : 1;
          this._backgroundDisposables[num++] = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) observable, (System.Action<M0>) (x => kvp.Value.set_alpha(Mathf.Lerp(startAlpha, (float) destAlpha, ((TimeInterval<float>) ref x).get_Value()))));
        }
      }
      using (Dictionary<int, CanvasGroup>.Enumerator enumerator = this._equipmentFlavorBackgrounds.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, CanvasGroup> kvp = enumerator.Current;
          float startAlpha = kvp.Value.get_alpha();
          int destAlpha = kvp.Key != id ? 0 : 1;
          this._backgroundDisposables[num++] = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) observable, (System.Action<M0>) (x => kvp.Value.set_alpha(Mathf.Lerp(startAlpha, (float) destAlpha, ((TimeInterval<float>) ref x).get_Value()))));
        }
      }
      using (Dictionary<int, CanvasGroup>.Enumerator enumerator = this._skillBackgrounds.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, CanvasGroup> kvp = enumerator.Current;
          float startAlpha = kvp.Value.get_alpha();
          int destAlpha = kvp.Key != id ? 0 : 1;
          this._backgroundDisposables[num++] = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) observable, (System.Action<M0>) (x => kvp.Value.set_alpha(Mathf.Lerp(startAlpha, (float) destAlpha, ((TimeInterval<float>) ref x).get_Value()))));
        }
      }
      using (Dictionary<int, CanvasGroup>.Enumerator enumerator = this._skillFlavorBackgrounds.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, CanvasGroup> kvp = enumerator.Current;
          float startAlpha = kvp.Value.get_alpha();
          int destAlpha = kvp.Key != id ? 0 : 1;
          this._backgroundDisposables[num++] = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) observable, (System.Action<M0>) (x => kvp.Value.set_alpha(Mathf.Lerp(startAlpha, (float) destAlpha, ((TimeInterval<float>) ref x).get_Value()))));
        }
      }
    }

    private void EquipmentSeq(
      StuffItem destItem,
      int categoryID,
      int[] itemIDs,
      System.Action<StuffItem> onSubmit)
    {
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
      InventoryUIController inventoryUI = MapUIContainer.SystemMenuUI.InventoryEnterUI;
      inventoryUI.isConfirm = false;
      inventoryUI.CountViewerVisible(false);
      inventoryUI.EmptyTextAutoVisible(true);
      inventoryUI.SetItemFilter(new InventoryFacadeViewer.ItemFilter[1]
      {
        new InventoryFacadeViewer.ItemFilter(categoryID, itemIDs)
      });
      PlayerData playerData = Singleton<Manager.Map>.Instance.Player.PlayerData;
      inventoryUI.itemList = (Func<List<StuffItem>>) (() => playerData.ItemList);
      if (Singleton<Resources>.Instance.GameInfo.GetItem(destItem.CategoryID, destItem.ID) != null)
        inventoryUI.itemList_System = (Func<List<StuffItem>>) (() => new List<StuffItem>()
        {
          StuffItem.CreateSystemItem(0, 0, 1)
        });
      inventoryUI.DoubleClickAction((System.Action<InventoryFacadeViewer.DoubleClickData>) null);
      inventoryUI.OnSubmit = (System.Action<StuffItem>) (item =>
      {
        InventoryUIController inventoryUiController = inventoryUI;
        if (inventoryUiController != null)
          inventoryUiController.OnClose();
        StuffItem stuffItem = new StuffItem(destItem);
        System.Action<StuffItem> action = onSubmit;
        if (action != null)
          action(item);
        if (Singleton<Resources>.Instance.GameInfo.GetItem(stuffItem.CategoryID, stuffItem.ID) == null)
          return;
        playerData.ItemList.AddItem(stuffItem);
      });
      inventoryUI.OnClose = (System.Action) (() =>
      {
        inventoryUI.itemList_System = (Func<List<StuffItem>>) null;
        inventoryUI.OnSubmit = (System.Action<StuffItem>) null;
        inventoryUI.IsActiveControl = false;
        Singleton<Manager.Input>.Instance.FocusLevel = 0;
        this.IsActiveControl = true;
        inventoryUI.OnClose = (System.Action) null;
      });
      this.OpenID = ((ReactiveProperty<int>) this._selectedID).get_Value();
      this.IsActiveControl = false;
      this.Observer.OpenModeMenu(SystemMenuUI.MenuMode.InventoryEnter);
    }

    private void SkillEquipmentSeq(bool isHSkill, int categoryID, System.Action<StuffItem> onSubmit)
    {
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
      InventoryUIController inventoryUI = MapUIContainer.SystemMenuUI.InventoryEnterUI;
      inventoryUI.isConfirm = true;
      inventoryUI.CountViewerVisible(false);
      inventoryUI.EmptyTextAutoVisible(true);
      AgentActor agentActor = Singleton<Manager.Map>.Instance.AgentTable.get_Item(((ReactiveProperty<int>) this._selectedID).get_Value() - 1);
      Dictionary<int, int> skill = !isHSkill ? agentActor.ChaControl.fileGameInfo.normalSkill : agentActor.ChaControl.fileGameInfo.hSkill;
      int[] array = Singleton<Resources>.Instance.GameInfo.GetItemTable(categoryID).Select<KeyValuePair<int, StuffItemInfo>, int>((Func<KeyValuePair<int, StuffItemInfo>, int>) (v => v.Key)).Where<int>((Func<int, bool>) (id => !skill.ContainsValue(id))).ToArray<int>();
      inventoryUI.SetItemFilter(new InventoryFacadeViewer.ItemFilter[1]
      {
        new InventoryFacadeViewer.ItemFilter(categoryID, array)
      });
      PlayerData playerData = Singleton<Manager.Map>.Instance.Player.PlayerData;
      inventoryUI.itemList = (Func<List<StuffItem>>) (() => playerData.ItemList);
      inventoryUI.DoubleClickAction((System.Action<InventoryFacadeViewer.DoubleClickData>) null);
      inventoryUI.OnSubmit = (System.Action<StuffItem>) (item =>
      {
        InventoryUIController inventoryUiController = inventoryUI;
        if (inventoryUiController != null)
          inventoryUiController.OnClose();
        System.Action<StuffItem> action = onSubmit;
        if (action == null)
          return;
        action(item);
      });
      inventoryUI.OnClose = (System.Action) (() =>
      {
        inventoryUI.OnSubmit = (System.Action<StuffItem>) null;
        inventoryUI.IsActiveControl = false;
        Singleton<Manager.Input>.Instance.FocusLevel = 0;
        this.IsActiveControl = true;
        inventoryUI.OnClose = (System.Action) null;
      });
      this.OpenID = ((ReactiveProperty<int>) this._selectedID).get_Value();
      this.IsActiveControl = false;
      this.Observer.OpenModeMenu(SystemMenuUI.MenuMode.InventoryEnter);
    }

    private void ChangeGloveEquipment(StuffItem dest, StuffItem source)
    {
      if (source.CategoryID == 0)
      {
        dest.CategoryID = source.CategoryID;
        dest.ID = -1;
        dest.Count = 0;
      }
      else
      {
        dest.CategoryID = source.CategoryID;
        dest.ID = source.ID;
        dest.Count = source.Count;
      }
      this.RefreshGloveEquipment(dest);
    }

    private void ChangeNetEquipment(StuffItem dest, StuffItem source)
    {
      if (source.CategoryID == 0)
      {
        dest.CategoryID = source.CategoryID;
        dest.ID = -1;
        dest.Count = 0;
      }
      else
      {
        dest.CategoryID = source.CategoryID;
        dest.ID = source.ID;
        dest.Count = source.Count;
      }
      this.RefreshNetEquipment(dest);
    }

    private void ChangeShovelEquipment(StuffItem dest, StuffItem source)
    {
      if (source.CategoryID == 0)
      {
        dest.CategoryID = source.CategoryID;
        dest.ID = -1;
        dest.Count = 0;
      }
      else
      {
        dest.CategoryID = source.CategoryID;
        dest.ID = source.ID;
        dest.Count = source.Count;
      }
      this.RefreshShovelEquipment(dest);
    }

    private void ChangePickelEquipment(StuffItem dest, StuffItem source)
    {
      if (source.CategoryID == 0)
      {
        dest.CategoryID = source.CategoryID;
        dest.ID = -1;
        dest.Count = 0;
      }
      else
      {
        dest.CategoryID = source.CategoryID;
        dest.ID = source.ID;
        dest.Count = source.Count;
      }
      this.RefreshPickelEquipment(dest);
    }

    private void ChangeRodEquipment(StuffItem dest, StuffItem source)
    {
      if (source.CategoryID == 0)
      {
        dest.CategoryID = source.CategoryID;
        dest.ID = -1;
        dest.Count = 0;
      }
      else
      {
        dest.CategoryID = source.CategoryID;
        dest.ID = source.ID;
        dest.Count = source.Count;
      }
      this.RefreshRodEquipment(dest);
    }

    private void ChangeHatEquipment(StuffItem dest, StuffItem source)
    {
      if (source.CategoryID == 0)
      {
        dest.CategoryID = source.CategoryID;
        dest.ID = -1;
        dest.Count = 0;
      }
      else
      {
        dest.CategoryID = source.CategoryID;
        dest.ID = source.ID;
        dest.Count = source.Count;
      }
      this.RefreshHatEquipment(dest);
      this.RefreshAccessory(((ReactiveProperty<int>) this._selectedID).get_Value(), ChaControlDefine.ExtraAccessoryParts.Head, dest);
    }

    private void ChangeRuckEquipment(StuffItem dest, StuffItem source)
    {
      if (source.CategoryID == 0)
      {
        dest.CategoryID = source.CategoryID;
        dest.ID = -1;
        dest.Count = 0;
      }
      else
      {
        dest.CategoryID = source.CategoryID;
        dest.ID = source.ID;
        dest.Count = source.Count;
      }
      this.RefreshRuckEquipment(dest);
      this.RefreshAccessory(((ReactiveProperty<int>) this._selectedID).get_Value(), ChaControlDefine.ExtraAccessoryParts.Back, dest);
    }

    private void ChangeNecklaceEquipment(StuffItem dest, StuffItem source)
    {
      if (source.CategoryID == 0)
      {
        dest.CategoryID = source.CategoryID;
        dest.ID = -1;
        dest.Count = 0;
      }
      else
      {
        dest.CategoryID = source.CategoryID;
        dest.ID = source.ID;
        dest.Count = source.Count;
      }
      this.RefreshNeckEquipment(dest);
      this.RefreshAccessory(((ReactiveProperty<int>) this._selectedID).get_Value(), ChaControlDefine.ExtraAccessoryParts.Neck, dest);
    }

    private void ChangeLampEquipment(StuffItem dest, StuffItem source)
    {
      if (source.CategoryID == 0)
      {
        dest.CategoryID = source.CategoryID;
        dest.ID = -1;
        dest.Count = 0;
      }
      else
      {
        dest.CategoryID = source.CategoryID;
        dest.ID = source.ID;
        dest.Count = source.Count;
      }
      this.RefreshLampEquipment(dest);
      if (((ReactiveProperty<int>) this._selectedID).get_Value() <= 0)
        return;
      AgentActor agentActor = Singleton<Manager.Map>.Instance.AgentTable.get_Item(((ReactiveProperty<int>) this._selectedID).get_Value() - 1);
      int id = dest.CategoryID == -1 ? -1 : dest.ID;
      ChaControlDefine.ExtraAccessoryParts parts = ChaControlDefine.ExtraAccessoryParts.Waist;
      agentActor.ChaControl.ChangeExtraAccessory(parts, id);
      agentActor.ChaControl.ShowExtraAccessory(parts, Singleton<Manager.Map>.Instance.Simulator.TimeZone == AIProject.TimeZone.Night);
    }

    public void UsageRestriction()
    {
      bool tutorialMode = Manager.Map.TutorialMode;
      this.SetInteractable((Selectable) this._rightButton, !tutorialMode);
      this.SetInteractable((Selectable) this._leftButton, !tutorialMode);
      if (this._charaButtons.IsNullOrEmpty<Button>())
        return;
      for (int index = 0; index < this._charaButtons.Length; ++index)
      {
        if (index != 0)
        {
          Button charaButton = this._charaButtons[index];
          if (!Object.op_Equality((Object) charaButton, (Object) null))
          {
            bool activeSelf = ((Component) charaButton).get_gameObject().get_activeSelf();
            bool flag = !tutorialMode;
            if (activeSelf != flag)
              ((Component) charaButton).get_gameObject().SetActive(flag);
          }
        }
      }
    }

    private bool SetInteractable(Selectable tar, bool active)
    {
      if (Object.op_Equality((Object) tar, (Object) null) || tar.get_interactable() == active)
        return false;
      tar.set_interactable(active);
      return true;
    }
  }
}
