// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.ControlMapper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Rewired.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
  [AddComponentMenu("")]
  public class ControlMapper : MonoBehaviour
  {
    private const float blockInputOnFocusTimeout = 0.1f;
    private const string buttonIdentifier_playerSelection = "PlayerSelection";
    private const string buttonIdentifier_removeController = "RemoveController";
    private const string buttonIdentifier_assignController = "AssignController";
    private const string buttonIdentifier_calibrateController = "CalibrateController";
    private const string buttonIdentifier_editInputBehaviors = "EditInputBehaviors";
    private const string buttonIdentifier_mapCategorySelection = "MapCategorySelection";
    private const string buttonIdentifier_assignedControllerSelection = "AssignedControllerSelection";
    private const string buttonIdentifier_done = "Done";
    private const string buttonIdentifier_restoreDefaults = "RestoreDefaults";
    [SerializeField]
    [Tooltip("Must be assigned a Rewired Input Manager scene object or prefab.")]
    private InputManager _rewiredInputManager;
    [SerializeField]
    [Tooltip("Set to True to prevent the Game Object from being destroyed when a new scene is loaded.\n\nNOTE: Changing this value from True to False at runtime will have no effect because Object.DontDestroyOnLoad cannot be undone once set.")]
    private bool _dontDestroyOnLoad;
    [SerializeField]
    [Tooltip("Open the control mapping screen immediately on start. Mainly used for testing.")]
    private bool _openOnStart;
    [SerializeField]
    [Tooltip("The Layout of the Keyboard Maps to be displayed.")]
    private int _keyboardMapDefaultLayout;
    [SerializeField]
    [Tooltip("The Layout of the Mouse Maps to be displayed.")]
    private int _mouseMapDefaultLayout;
    [SerializeField]
    [Tooltip("The Layout of the Mouse Maps to be displayed.")]
    private int _joystickMapDefaultLayout;
    [SerializeField]
    private Rewired.UI.ControlMapper.ControlMapper.MappingSet[] _mappingSets;
    [SerializeField]
    [Tooltip("Display a selectable list of Players. If your game only supports 1 player, you can disable this.")]
    private bool _showPlayers;
    [SerializeField]
    [Tooltip("Display the Controller column for input mapping.")]
    private bool _showControllers;
    [SerializeField]
    [Tooltip("Display the Keyboard column for input mapping.")]
    private bool _showKeyboard;
    [SerializeField]
    [Tooltip("Display the Mouse column for input mapping.")]
    private bool _showMouse;
    [SerializeField]
    [Tooltip("The maximum number of controllers allowed to be assigned to a Player. If set to any value other than 1, a selectable list of currently-assigned controller will be displayed to the user. [0 = infinite]")]
    private int _maxControllersPerPlayer;
    [SerializeField]
    [Tooltip("Display section labels for each Action Category in the input field grid. Only applies if Action Categories are used to display the Action list.")]
    private bool _showActionCategoryLabels;
    [SerializeField]
    [Tooltip("The number of input fields to display for the keyboard. If you want to support alternate mappings on the same device, set this to 2 or more.")]
    private int _keyboardInputFieldCount;
    [SerializeField]
    [Tooltip("The number of input fields to display for the mouse. If you want to support alternate mappings on the same device, set this to 2 or more.")]
    private int _mouseInputFieldCount;
    [SerializeField]
    [Tooltip("The number of input fields to display for joysticks. If you want to support alternate mappings on the same device, set this to 2 or more.")]
    private int _controllerInputFieldCount;
    [SerializeField]
    [Tooltip("Display a full-axis input assignment field for every axis-type Action in the input field grid. Also displays an invert toggle for the user  to invert the full-axis assignment direction.\n\n*IMPORTANT*: This field is required if you have made any full-axis assignments in the Rewired Input Manager or in saved XML user data. Disabling this field when you have full-axis assignments will result in the inability for the user to view, remove, or modify these full-axis assignments. In addition, these assignments may cause conflicts when trying to remap the same axes to Actions.")]
    private bool _showFullAxisInputFields;
    [SerializeField]
    [Tooltip("Display a positive and negative input assignment field for every axis-type Action in the input field grid.\n\n*IMPORTANT*: These fields are required to assign buttons, keyboard keys, and hat or D-Pad directions to axis-type Actions. If you have made any split-axis assignments or button/key/D-pad assignments to axis-type Actions in the Rewired Input Manager or in saved XML user data, disabling these fields will result in the inability for the user to view, remove, or modify these assignments. In addition, these assignments may cause conflicts when trying to remap the same elements to Actions.")]
    private bool _showSplitAxisInputFields;
    [SerializeField]
    [Tooltip("If enabled, when an element assignment conflict is found, an option will be displayed that allows the user to make the conflicting assignment anyway.")]
    private bool _allowElementAssignmentConflicts;
    [SerializeField]
    [Tooltip("If enabled, when an element assignment conflict is found, an option will be displayed that allows the user to swap conflicting assignments. This only applies to the first conflicting assignment found. This option will not be displayed if allowElementAssignmentConflicts is true.")]
    private bool _allowElementAssignmentSwap;
    [SerializeField]
    [Tooltip("The width in relative pixels of the Action label column.")]
    private int _actionLabelWidth;
    [SerializeField]
    [Tooltip("The width in relative pixels of the Keyboard column.")]
    private int _keyboardColMaxWidth;
    [SerializeField]
    [Tooltip("The width in relative pixels of the Mouse column.")]
    private int _mouseColMaxWidth;
    [SerializeField]
    [Tooltip("The width in relative pixels of the Controller column.")]
    private int _controllerColMaxWidth;
    [SerializeField]
    [Tooltip("The height in relative pixels of the input grid button rows.")]
    private int _inputRowHeight;
    [SerializeField]
    [Tooltip("The width in relative pixels of spacing between columns.")]
    private int _inputColumnSpacing;
    [SerializeField]
    [Tooltip("The height in relative pixels of the space between Action Category sections. Only applicable if Show Action Category Labels is checked.")]
    private int _inputRowCategorySpacing;
    [SerializeField]
    [Tooltip("The width in relative pixels of the invert toggle buttons.")]
    private int _invertToggleWidth;
    [SerializeField]
    [Tooltip("The width in relative pixels of generated popup windows.")]
    private int _defaultWindowWidth;
    [SerializeField]
    [Tooltip("The height in relative pixels of generated popup windows.")]
    private int _defaultWindowHeight;
    [SerializeField]
    [Tooltip("The time in seconds the user has to press an element on a controller when assigning a controller to a Player. If this time elapses with no user input a controller, the assignment will be canceled.")]
    private float _controllerAssignmentTimeout;
    [SerializeField]
    [Tooltip("The time in seconds the user has to press an element on a controller while waiting for axes to be centered before assigning input.")]
    private float _preInputAssignmentTimeout;
    [SerializeField]
    [Tooltip("The time in seconds the user has to press an element on a controller when assigning input. If this time elapses with no user input on the target controller, the assignment will be canceled.")]
    private float _inputAssignmentTimeout;
    [SerializeField]
    [Tooltip("The time in seconds the user has to press an element on a controller during calibration.")]
    private float _axisCalibrationTimeout;
    [SerializeField]
    [Tooltip("If checked, mouse X-axis movement will always be ignored during input assignment. Check this if you don't want the horizontal mouse axis to be user-assignable to any Actions.")]
    private bool _ignoreMouseXAxisAssignment;
    [SerializeField]
    [Tooltip("If checked, mouse Y-axis movement will always be ignored during input assignment. Check this if you don't want the vertical mouse axis to be user-assignable to any Actions.")]
    private bool _ignoreMouseYAxisAssignment;
    [SerializeField]
    [Tooltip("An Action that when activated will alternately close or open the main screen as long as no popup windows are open.")]
    private int _screenToggleAction;
    [SerializeField]
    [Tooltip("An Action that when activated will open the main screen if it is closed.")]
    private int _screenOpenAction;
    [SerializeField]
    [Tooltip("An Action that when activated will close the main screen as long as no popup windows are open.")]
    private int _screenCloseAction;
    [SerializeField]
    [Tooltip("An Action that when activated will cancel and close any open popup window. Use with care because the element assigned to this Action can never be mapped by the user (because it would just cancel his assignment).")]
    private int _universalCancelAction;
    [SerializeField]
    [Tooltip("If enabled, Universal Cancel will also close the main screen if pressed when no windows are open.")]
    private bool _universalCancelClosesScreen;
    [SerializeField]
    [Tooltip("If checked, controls will be displayed which will allow the user to customize certain Input Behavior settings.")]
    private bool _showInputBehaviorSettings;
    [SerializeField]
    [Tooltip("Customizable settings for user-modifiable Input Behaviors. This can be used for settings like Mouse Look Sensitivity.")]
    private Rewired.UI.ControlMapper.ControlMapper.InputBehaviorSettings[] _inputBehaviorSettings;
    [SerializeField]
    [Tooltip("If enabled, UI elements will be themed based on the settings in Theme Settings.")]
    private bool _useThemeSettings;
    [SerializeField]
    [Tooltip("Must be assigned a ThemeSettings object. Used to theme UI elements.")]
    private ThemeSettings _themeSettings;
    [SerializeField]
    [Tooltip("Must be assigned a LanguageData object. Used to retrieve language entries for UI elements.")]
    private LanguageData _language;
    [SerializeField]
    [Tooltip("A list of prefabs. You should not have to modify this.")]
    private Rewired.UI.ControlMapper.ControlMapper.Prefabs prefabs;
    [SerializeField]
    [Tooltip("A list of references to elements in the hierarchy. You should not have to modify this.")]
    private Rewired.UI.ControlMapper.ControlMapper.References references;
    [SerializeField]
    [Tooltip("Show the label for the Players button group?")]
    private bool _showPlayersGroupLabel;
    [SerializeField]
    [Tooltip("Show the label for the Controller button group?")]
    private bool _showControllerGroupLabel;
    [SerializeField]
    [Tooltip("Show the label for the Assigned Controllers button group?")]
    private bool _showAssignedControllersGroupLabel;
    [SerializeField]
    [Tooltip("Show the label for the Settings button group?")]
    private bool _showSettingsGroupLabel;
    [SerializeField]
    [Tooltip("Show the label for the Map Categories button group?")]
    private bool _showMapCategoriesGroupLabel;
    [SerializeField]
    [Tooltip("Show the label for the current controller name?")]
    private bool _showControllerNameLabel;
    [SerializeField]
    [Tooltip("Show the Assigned Controllers group? If joystick auto-assignment is enabled in the Rewired Input Manager and the max joysticks per player is set to any value other than 1, the Assigned Controllers group will always be displayed.")]
    private bool _showAssignedControllers;
    private Action _ScreenClosedEvent;
    private Action _ScreenOpenedEvent;
    private Action _PopupWindowOpenedEvent;
    private Action _PopupWindowClosedEvent;
    private Action _InputPollingStartedEvent;
    private Action _InputPollingEndedEvent;
    [SerializeField]
    [Tooltip("Event sent when the UI is closed.")]
    private UnityEvent _onScreenClosed;
    [SerializeField]
    [Tooltip("Event sent when the UI is opened.")]
    private UnityEvent _onScreenOpened;
    [SerializeField]
    [Tooltip("Event sent when a popup window is closed.")]
    private UnityEvent _onPopupWindowClosed;
    [SerializeField]
    [Tooltip("Event sent when a popup window is opened.")]
    private UnityEvent _onPopupWindowOpened;
    [SerializeField]
    [Tooltip("Event sent when polling for input has started.")]
    private UnityEvent _onInputPollingStarted;
    [SerializeField]
    [Tooltip("Event sent when polling for input has ended.")]
    private UnityEvent _onInputPollingEnded;
    private static Rewired.UI.ControlMapper.ControlMapper Instance;
    private bool initialized;
    private int playerCount;
    private Rewired.UI.ControlMapper.ControlMapper.InputGrid inputGrid;
    private Rewired.UI.ControlMapper.ControlMapper.WindowManager windowManager;
    private int currentPlayerId;
    private int currentMapCategoryId;
    private List<Rewired.UI.ControlMapper.ControlMapper.GUIButton> playerButtons;
    private List<Rewired.UI.ControlMapper.ControlMapper.GUIButton> mapCategoryButtons;
    private List<Rewired.UI.ControlMapper.ControlMapper.GUIButton> assignedControllerButtons;
    private Rewired.UI.ControlMapper.ControlMapper.GUIButton assignedControllerButtonsPlaceholder;
    private List<GameObject> miscInstantiatedObjects;
    private GameObject canvas;
    private GameObject lastUISelection;
    private int currentJoystickId;
    private float blockInputOnFocusEndTime;
    private bool isPollingForInput;
    private Rewired.UI.ControlMapper.ControlMapper.InputMapping pendingInputMapping;
    private Rewired.UI.ControlMapper.ControlMapper.AxisCalibrator pendingAxisCalibration;
    private Action<InputFieldInfo> inputFieldActivatedDelegate;
    private Action<ToggleInfo, bool> inputFieldInvertToggleStateChangedDelegate;
    private Action _restoreDefaultsDelegate;

    public ControlMapper()
    {
      base.\u002Ector();
    }

    public event Action ScreenClosedEvent
    {
      add
      {
        this._ScreenClosedEvent += value;
      }
      remove
      {
        this._ScreenClosedEvent -= value;
      }
    }

    public event Action ScreenOpenedEvent
    {
      add
      {
        this._ScreenOpenedEvent += value;
      }
      remove
      {
        this._ScreenOpenedEvent -= value;
      }
    }

    public event Action PopupWindowClosedEvent
    {
      add
      {
        this._PopupWindowClosedEvent += value;
      }
      remove
      {
        this._PopupWindowClosedEvent -= value;
      }
    }

    public event Action PopupWindowOpenedEvent
    {
      add
      {
        this._PopupWindowOpenedEvent += value;
      }
      remove
      {
        this._PopupWindowOpenedEvent -= value;
      }
    }

    public event Action InputPollingStartedEvent
    {
      add
      {
        this._InputPollingStartedEvent += value;
      }
      remove
      {
        this._InputPollingStartedEvent -= value;
      }
    }

    public event Action InputPollingEndedEvent
    {
      add
      {
        this._InputPollingEndedEvent += value;
      }
      remove
      {
        this._InputPollingEndedEvent -= value;
      }
    }

    public event UnityAction onScreenClosed
    {
      add
      {
        this._onScreenClosed.AddListener(value);
      }
      remove
      {
        this._onScreenClosed.RemoveListener(value);
      }
    }

    public event UnityAction onScreenOpened
    {
      add
      {
        this._onScreenOpened.AddListener(value);
      }
      remove
      {
        this._onScreenOpened.RemoveListener(value);
      }
    }

    public event UnityAction onPopupWindowClosed
    {
      add
      {
        this._onPopupWindowClosed.AddListener(value);
      }
      remove
      {
        this._onPopupWindowClosed.RemoveListener(value);
      }
    }

    public event UnityAction onPopupWindowOpened
    {
      add
      {
        this._onPopupWindowOpened.AddListener(value);
      }
      remove
      {
        this._onPopupWindowOpened.RemoveListener(value);
      }
    }

    public event UnityAction onInputPollingStarted
    {
      add
      {
        this._onInputPollingStarted.AddListener(value);
      }
      remove
      {
        this._onInputPollingStarted.RemoveListener(value);
      }
    }

    public event UnityAction onInputPollingEnded
    {
      add
      {
        this._onInputPollingEnded.AddListener(value);
      }
      remove
      {
        this._onInputPollingEnded.RemoveListener(value);
      }
    }

    public InputManager rewiredInputManager
    {
      get
      {
        return this._rewiredInputManager;
      }
      set
      {
        this._rewiredInputManager = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public bool dontDestroyOnLoad
    {
      get
      {
        return this._dontDestroyOnLoad;
      }
      set
      {
        if (value != this._dontDestroyOnLoad && value)
          Object.DontDestroyOnLoad((Object) ((Component) ((Component) this).get_transform()).get_gameObject());
        this._dontDestroyOnLoad = value;
      }
    }

    public int keyboardMapDefaultLayout
    {
      get
      {
        return this._keyboardMapDefaultLayout;
      }
      set
      {
        this._keyboardMapDefaultLayout = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public int mouseMapDefaultLayout
    {
      get
      {
        return this._mouseMapDefaultLayout;
      }
      set
      {
        this._mouseMapDefaultLayout = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public int joystickMapDefaultLayout
    {
      get
      {
        return this._joystickMapDefaultLayout;
      }
      set
      {
        this._joystickMapDefaultLayout = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public bool showPlayers
    {
      get
      {
        return this._showPlayers && ReInput.get_players().get_playerCount() > 1;
      }
      set
      {
        this._showPlayers = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public bool showControllers
    {
      get
      {
        return this._showControllers;
      }
      set
      {
        this._showControllers = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public bool showKeyboard
    {
      get
      {
        return this._showKeyboard;
      }
      set
      {
        this._showKeyboard = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public bool showMouse
    {
      get
      {
        return this._showMouse;
      }
      set
      {
        this._showMouse = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public int maxControllersPerPlayer
    {
      get
      {
        return this._maxControllersPerPlayer;
      }
      set
      {
        this._maxControllersPerPlayer = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public bool showActionCategoryLabels
    {
      get
      {
        return this._showActionCategoryLabels;
      }
      set
      {
        this._showActionCategoryLabels = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public int keyboardInputFieldCount
    {
      get
      {
        return this._keyboardInputFieldCount;
      }
      set
      {
        this._keyboardInputFieldCount = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public int mouseInputFieldCount
    {
      get
      {
        return this._mouseInputFieldCount;
      }
      set
      {
        this._mouseInputFieldCount = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public int controllerInputFieldCount
    {
      get
      {
        return this._controllerInputFieldCount;
      }
      set
      {
        this._controllerInputFieldCount = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public bool showFullAxisInputFields
    {
      get
      {
        return this._showFullAxisInputFields;
      }
      set
      {
        this._showFullAxisInputFields = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public bool showSplitAxisInputFields
    {
      get
      {
        return this._showSplitAxisInputFields;
      }
      set
      {
        this._showSplitAxisInputFields = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public bool allowElementAssignmentConflicts
    {
      get
      {
        return this._allowElementAssignmentConflicts;
      }
      set
      {
        this._allowElementAssignmentConflicts = value;
        this.InspectorPropertyChanged(false);
      }
    }

    public bool allowElementAssignmentSwap
    {
      get
      {
        return this._allowElementAssignmentSwap;
      }
      set
      {
        this._allowElementAssignmentSwap = value;
        this.InspectorPropertyChanged(false);
      }
    }

    public int actionLabelWidth
    {
      get
      {
        return this._actionLabelWidth;
      }
      set
      {
        this._actionLabelWidth = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public int keyboardColMaxWidth
    {
      get
      {
        return this._keyboardColMaxWidth;
      }
      set
      {
        this._keyboardColMaxWidth = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public int mouseColMaxWidth
    {
      get
      {
        return this._mouseColMaxWidth;
      }
      set
      {
        this._mouseColMaxWidth = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public int controllerColMaxWidth
    {
      get
      {
        return this._controllerColMaxWidth;
      }
      set
      {
        this._controllerColMaxWidth = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public int inputRowHeight
    {
      get
      {
        return this._inputRowHeight;
      }
      set
      {
        this._inputRowHeight = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public int inputColumnSpacing
    {
      get
      {
        return this._inputColumnSpacing;
      }
      set
      {
        this._inputColumnSpacing = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public int inputRowCategorySpacing
    {
      get
      {
        return this._inputRowCategorySpacing;
      }
      set
      {
        this._inputRowCategorySpacing = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public int invertToggleWidth
    {
      get
      {
        return this._invertToggleWidth;
      }
      set
      {
        this._invertToggleWidth = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public int defaultWindowWidth
    {
      get
      {
        return this._defaultWindowWidth;
      }
      set
      {
        this._defaultWindowWidth = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public int defaultWindowHeight
    {
      get
      {
        return this._defaultWindowHeight;
      }
      set
      {
        this._defaultWindowHeight = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public float controllerAssignmentTimeout
    {
      get
      {
        return this._controllerAssignmentTimeout;
      }
      set
      {
        this._controllerAssignmentTimeout = value;
        this.InspectorPropertyChanged(false);
      }
    }

    public float preInputAssignmentTimeout
    {
      get
      {
        return this._preInputAssignmentTimeout;
      }
      set
      {
        this._preInputAssignmentTimeout = value;
        this.InspectorPropertyChanged(false);
      }
    }

    public float inputAssignmentTimeout
    {
      get
      {
        return this._inputAssignmentTimeout;
      }
      set
      {
        this._inputAssignmentTimeout = value;
        this.InspectorPropertyChanged(false);
      }
    }

    public float axisCalibrationTimeout
    {
      get
      {
        return this._axisCalibrationTimeout;
      }
      set
      {
        this._axisCalibrationTimeout = value;
        this.InspectorPropertyChanged(false);
      }
    }

    public bool ignoreMouseXAxisAssignment
    {
      get
      {
        return this._ignoreMouseXAxisAssignment;
      }
      set
      {
        this._ignoreMouseXAxisAssignment = value;
        this.InspectorPropertyChanged(false);
      }
    }

    public bool ignoreMouseYAxisAssignment
    {
      get
      {
        return this._ignoreMouseYAxisAssignment;
      }
      set
      {
        this._ignoreMouseYAxisAssignment = value;
        this.InspectorPropertyChanged(false);
      }
    }

    public bool universalCancelClosesScreen
    {
      get
      {
        return this._universalCancelClosesScreen;
      }
      set
      {
        this._universalCancelClosesScreen = value;
        this.InspectorPropertyChanged(false);
      }
    }

    public bool showInputBehaviorSettings
    {
      get
      {
        return this._showInputBehaviorSettings;
      }
      set
      {
        this._showInputBehaviorSettings = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public bool useThemeSettings
    {
      get
      {
        return this._useThemeSettings;
      }
      set
      {
        this._useThemeSettings = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public LanguageData language
    {
      get
      {
        return this._language;
      }
      set
      {
        this._language = value;
        if (Object.op_Inequality((Object) this._language, (Object) null))
          this._language.Initialize();
        this.InspectorPropertyChanged(true);
      }
    }

    public bool showPlayersGroupLabel
    {
      get
      {
        return this._showPlayersGroupLabel;
      }
      set
      {
        this._showPlayersGroupLabel = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public bool showControllerGroupLabel
    {
      get
      {
        return this._showControllerGroupLabel;
      }
      set
      {
        this._showControllerGroupLabel = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public bool showAssignedControllersGroupLabel
    {
      get
      {
        return this._showAssignedControllersGroupLabel;
      }
      set
      {
        this._showAssignedControllersGroupLabel = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public bool showSettingsGroupLabel
    {
      get
      {
        return this._showSettingsGroupLabel;
      }
      set
      {
        this._showSettingsGroupLabel = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public bool showMapCategoriesGroupLabel
    {
      get
      {
        return this._showMapCategoriesGroupLabel;
      }
      set
      {
        this._showMapCategoriesGroupLabel = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public bool showControllerNameLabel
    {
      get
      {
        return this._showControllerNameLabel;
      }
      set
      {
        this._showControllerNameLabel = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public bool showAssignedControllers
    {
      get
      {
        return this._showAssignedControllers;
      }
      set
      {
        this._showAssignedControllers = value;
        this.InspectorPropertyChanged(true);
      }
    }

    public Action restoreDefaultsDelegate
    {
      get
      {
        return this._restoreDefaultsDelegate;
      }
      set
      {
        this._restoreDefaultsDelegate = value;
      }
    }

    public bool isOpen
    {
      get
      {
        if (this.initialized)
          return this.canvas.get_activeInHierarchy();
        return Object.op_Inequality((Object) this.references.canvas, (Object) null) && ((Component) this.references.canvas).get_gameObject().get_activeInHierarchy();
      }
    }

    private bool isFocused
    {
      get
      {
        return this.initialized && !this.windowManager.isWindowOpen;
      }
    }

    private bool inputAllowed
    {
      get
      {
        return (double) this.blockInputOnFocusEndTime <= (double) Time.get_unscaledTime();
      }
    }

    private int inputGridColumnCount
    {
      get
      {
        int num = 1;
        if (this._showKeyboard)
          ++num;
        if (this._showMouse)
          ++num;
        if (this._showControllers)
          ++num;
        return num;
      }
    }

    private int inputGridWidth
    {
      get
      {
        return this._actionLabelWidth + (!this._showKeyboard ? 0 : this._keyboardColMaxWidth) + (!this._showMouse ? 0 : this._mouseColMaxWidth) + (!this._showControllers ? 0 : this._controllerColMaxWidth) + (this.inputGridColumnCount - 1) * this._inputColumnSpacing;
      }
    }

    private Player currentPlayer
    {
      get
      {
        return ReInput.get_players().GetPlayer(this.currentPlayerId);
      }
    }

    private InputCategory currentMapCategory
    {
      get
      {
        return (InputCategory) ReInput.get_mapping().GetMapCategory(this.currentMapCategoryId);
      }
    }

    private Rewired.UI.ControlMapper.ControlMapper.MappingSet currentMappingSet
    {
      get
      {
        if (this.currentMapCategoryId < 0)
          return (Rewired.UI.ControlMapper.ControlMapper.MappingSet) null;
        for (int index = 0; index < this._mappingSets.Length; ++index)
        {
          if (this._mappingSets[index].mapCategoryId == this.currentMapCategoryId)
            return this._mappingSets[index];
        }
        return (Rewired.UI.ControlMapper.ControlMapper.MappingSet) null;
      }
    }

    private Joystick currentJoystick
    {
      get
      {
        return ReInput.get_controllers().GetJoystick(this.currentJoystickId);
      }
    }

    private bool isJoystickSelected
    {
      get
      {
        return this.currentJoystickId >= 0;
      }
    }

    private GameObject currentUISelection
    {
      get
      {
        return Object.op_Inequality((Object) EventSystem.get_current(), (Object) null) ? EventSystem.get_current().get_currentSelectedGameObject() : (GameObject) null;
      }
    }

    private bool showSettings
    {
      get
      {
        return this._showInputBehaviorSettings && this._inputBehaviorSettings.Length > 0;
      }
    }

    private bool showMapCategories
    {
      get
      {
        return this._mappingSets != null && this._mappingSets.Length > 1;
      }
    }

    private void Awake()
    {
      if (this._dontDestroyOnLoad)
        Object.DontDestroyOnLoad((Object) ((Component) ((Component) this).get_transform()).get_gameObject());
      this.PreInitialize();
      if (!this.isOpen)
        return;
      this.Initialize();
      this.Open(true);
    }

    private void Start()
    {
      if (!this._openOnStart)
        return;
      this.Open(false);
    }

    private void Update()
    {
      if (!this.isOpen || !this.initialized)
        return;
      this.CheckUISelection();
    }

    private void OnDestroy()
    {
      ReInput.remove_ControllerConnectedEvent(new Action<ControllerStatusChangedEventArgs>(this.OnJoystickConnected));
      ReInput.remove_ControllerDisconnectedEvent(new Action<ControllerStatusChangedEventArgs>(this.OnJoystickDisconnected));
      ReInput.remove_ControllerPreDisconnectEvent(new Action<ControllerStatusChangedEventArgs>(this.OnJoystickPreDisconnect));
      this.UnsubscribeMenuControlInputEvents();
    }

    private void PreInitialize()
    {
      if (!ReInput.get_isReady())
        Debug.LogError((object) "Rewired Control Mapper: Rewired has not been initialized! Are you missing a Rewired Input Manager in your scene?");
      else
        this.SubscribeMenuControlInputEvents();
    }

    private void Initialize()
    {
      if (this.initialized || !ReInput.get_isReady())
        return;
      if (Object.op_Equality((Object) this._rewiredInputManager, (Object) null))
      {
        this._rewiredInputManager = (InputManager) Object.FindObjectOfType<InputManager>();
        if (Object.op_Equality((Object) this._rewiredInputManager, (Object) null))
        {
          Debug.LogError((object) "Rewired Control Mapper: A Rewired Input Manager was not assigned in the inspector or found in the current scene! Control Mapper will not function.");
          return;
        }
      }
      if (Object.op_Inequality((Object) Rewired.UI.ControlMapper.ControlMapper.Instance, (Object) null))
      {
        Debug.LogError((object) "Rewired Control Mapper: Only one ControlMapper can exist at one time!");
      }
      else
      {
        Rewired.UI.ControlMapper.ControlMapper.Instance = this;
        if (this.prefabs == null || !this.prefabs.Check())
          Debug.LogError((object) "Rewired Control Mapper: All prefabs must be assigned in the inspector!");
        else if (this.references == null || !this.references.Check())
        {
          Debug.LogError((object) "Rewired Control Mapper: All references must be assigned in the inspector!");
        }
        else
        {
          this.references.inputGridLayoutElement = (LayoutElement) ((Component) this.references.inputGridContainer).GetComponent<LayoutElement>();
          if (Object.op_Equality((Object) this.references.inputGridLayoutElement, (Object) null))
          {
            Debug.LogError((object) "Rewired Control Mapper: InputGridContainer is missing LayoutElement component!");
          }
          else
          {
            if (this._showKeyboard && this._keyboardInputFieldCount < 1)
            {
              Debug.LogWarning((object) "Rewired Control Mapper: Keyboard Input Fields must be at least 1!");
              this._keyboardInputFieldCount = 1;
            }
            if (this._showMouse && this._mouseInputFieldCount < 1)
            {
              Debug.LogWarning((object) "Rewired Control Mapper: Mouse Input Fields must be at least 1!");
              this._mouseInputFieldCount = 1;
            }
            if (this._showControllers && this._controllerInputFieldCount < 1)
            {
              Debug.LogWarning((object) "Rewired Control Mapper: Controller Input Fields must be at least 1!");
              this._controllerInputFieldCount = 1;
            }
            if (this._maxControllersPerPlayer < 0)
            {
              Debug.LogWarning((object) "Rewired Control Mapper: Max Controllers Per Player must be at least 0 (no limit)!");
              this._maxControllersPerPlayer = 0;
            }
            if (this._useThemeSettings && Object.op_Equality((Object) this._themeSettings, (Object) null))
            {
              Debug.LogWarning((object) "Rewired Control Mapper: To use theming, Theme Settings must be set in the inspector! Theming has been disabled.");
              this._useThemeSettings = false;
            }
            if (Object.op_Equality((Object) this._language, (Object) null))
            {
              Debug.LogError((object) "Rawired UI: Language must be set in the inspector!");
            }
            else
            {
              this._language.Initialize();
              this.inputFieldActivatedDelegate = new Action<InputFieldInfo>(this.OnInputFieldActivated);
              this.inputFieldInvertToggleStateChangedDelegate = new Action<ToggleInfo, bool>(this.OnInputFieldInvertToggleStateChanged);
              ReInput.add_ControllerConnectedEvent(new Action<ControllerStatusChangedEventArgs>(this.OnJoystickConnected));
              ReInput.add_ControllerDisconnectedEvent(new Action<ControllerStatusChangedEventArgs>(this.OnJoystickDisconnected));
              ReInput.add_ControllerPreDisconnectEvent(new Action<ControllerStatusChangedEventArgs>(this.OnJoystickPreDisconnect));
              this.playerCount = ReInput.get_players().get_playerCount();
              this.canvas = ((Component) this.references.canvas).get_gameObject();
              this.windowManager = new Rewired.UI.ControlMapper.ControlMapper.WindowManager(this.prefabs.window, this.prefabs.fader, ((Component) this.references.canvas).get_transform());
              this.playerButtons = new List<Rewired.UI.ControlMapper.ControlMapper.GUIButton>();
              this.mapCategoryButtons = new List<Rewired.UI.ControlMapper.ControlMapper.GUIButton>();
              this.assignedControllerButtons = new List<Rewired.UI.ControlMapper.ControlMapper.GUIButton>();
              this.miscInstantiatedObjects = new List<GameObject>();
              this.currentMapCategoryId = this._mappingSets[0].mapCategoryId;
              this.Draw();
              this.CreateInputGrid();
              this.CreateLayout();
              this.SubscribeFixedUISelectionEvents();
              this.initialized = true;
            }
          }
        }
      }
    }

    private void OnJoystickConnected(ControllerStatusChangedEventArgs args)
    {
      if (!this.initialized || !this._showControllers)
        return;
      this.ClearVarsOnJoystickChange();
      this.ForceRefresh();
    }

    private void OnJoystickDisconnected(ControllerStatusChangedEventArgs args)
    {
      if (!this.initialized || !this._showControllers)
        return;
      this.ClearVarsOnJoystickChange();
      this.ForceRefresh();
    }

    private void OnJoystickPreDisconnect(ControllerStatusChangedEventArgs args)
    {
      if (!this.initialized || this._showControllers)
        ;
    }

    public void OnButtonActivated(ButtonInfo buttonInfo)
    {
      if (!this.initialized || !this.inputAllowed)
        return;
      string identifier = buttonInfo.identifier;
      if (identifier == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (Rewired.UI.ControlMapper.ControlMapper.\u003C\u003Ef__switch\u0024map7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Rewired.UI.ControlMapper.ControlMapper.\u003C\u003Ef__switch\u0024map7 = new Dictionary<string, int>(9)
        {
          {
            "PlayerSelection",
            0
          },
          {
            "AssignedControllerSelection",
            1
          },
          {
            "RemoveController",
            2
          },
          {
            "AssignController",
            3
          },
          {
            "CalibrateController",
            4
          },
          {
            "EditInputBehaviors",
            5
          },
          {
            "MapCategorySelection",
            6
          },
          {
            "Done",
            7
          },
          {
            "RestoreDefaults",
            8
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (!Rewired.UI.ControlMapper.ControlMapper.\u003C\u003Ef__switch\u0024map7.TryGetValue(identifier, out num))
        return;
      switch (num)
      {
        case 0:
          this.OnPlayerSelected(buttonInfo.intData, true);
          break;
        case 1:
          this.OnControllerSelected(buttonInfo.intData);
          break;
        case 2:
          this.OnRemoveCurrentController();
          break;
        case 3:
          this.ShowAssignControllerWindow();
          break;
        case 4:
          this.ShowCalibrateControllerWindow();
          break;
        case 5:
          this.ShowEditInputBehaviorsWindow();
          break;
        case 6:
          this.OnMapCategorySelected(buttonInfo.intData, true);
          break;
        case 7:
          this.Close(true);
          break;
        case 8:
          this.OnRestoreDefaults();
          break;
      }
    }

    public void OnInputFieldActivated(InputFieldInfo fieldInfo)
    {
      if (!this.initialized || !this.inputAllowed || this.currentPlayer == null)
        return;
      InputAction action = ReInput.get_mapping().GetAction(fieldInfo.actionId);
      if (action == null)
        return;
      string actionName;
      if (action.get_type() == 1)
      {
        actionName = action.get_descriptiveName();
      }
      else
      {
        if (action.get_type() != null)
          throw new NotImplementedException();
        if (fieldInfo.axisRange == null)
          actionName = action.get_descriptiveName();
        else if (fieldInfo.axisRange == 1)
        {
          actionName = !string.IsNullOrEmpty(action.get_positiveDescriptiveName()) ? action.get_positiveDescriptiveName() : action.get_descriptiveName() + " +";
        }
        else
        {
          if (fieldInfo.axisRange != 2)
            throw new NotImplementedException();
          actionName = !string.IsNullOrEmpty(action.get_negativeDescriptiveName()) ? action.get_negativeDescriptiveName() : action.get_descriptiveName() + " -";
        }
      }
      ControllerMap controllerMap = this.GetControllerMap(fieldInfo.controllerType);
      if (controllerMap == null)
        return;
      ActionElementMap aem = fieldInfo.actionElementMapId < 0 ? (ActionElementMap) null : controllerMap.GetElementMap(fieldInfo.actionElementMapId);
      if (aem != null)
        this.ShowBeginElementAssignmentReplacementWindow(fieldInfo, action, controllerMap, aem, actionName);
      else
        this.ShowCreateNewElementAssignmentWindow(fieldInfo, action, controllerMap, actionName);
    }

    public void OnInputFieldInvertToggleStateChanged(ToggleInfo toggleInfo, bool newState)
    {
      if (!this.initialized || !this.inputAllowed)
        return;
      this.SetActionAxisInverted(newState, toggleInfo.controllerType, toggleInfo.actionElementMapId);
    }

    private void OnPlayerSelected(int playerId, bool redraw)
    {
      if (!this.initialized)
        return;
      this.currentPlayerId = playerId;
      this.ClearVarsOnPlayerChange();
      if (!redraw)
        return;
      this.Redraw(true, true);
    }

    private void OnControllerSelected(int joystickId)
    {
      if (!this.initialized)
        return;
      this.currentJoystickId = joystickId;
      this.Redraw(true, true);
    }

    private void OnRemoveCurrentController()
    {
      if (this.currentPlayer == null || this.currentJoystickId < 0)
        return;
      this.RemoveController(this.currentPlayer, this.currentJoystickId);
      this.ClearVarsOnJoystickChange();
      this.Redraw(false, false);
    }

    private void OnMapCategorySelected(int id, bool redraw)
    {
      if (!this.initialized)
        return;
      this.currentMapCategoryId = id;
      if (!redraw)
        return;
      this.Redraw(true, true);
    }

    private void OnRestoreDefaults()
    {
      if (!this.initialized)
        return;
      this.ShowRestoreDefaultsWindow();
    }

    private void OnScreenToggleActionPressed(InputActionEventData data)
    {
      if (!this.isOpen)
      {
        this.Open();
      }
      else
      {
        if (!this.initialized || !this.isFocused)
          return;
        this.Close(true);
      }
    }

    private void OnScreenOpenActionPressed(InputActionEventData data)
    {
      this.Open();
    }

    private void OnScreenCloseActionPressed(InputActionEventData data)
    {
      if (!this.initialized || !this.isOpen || !this.isFocused)
        return;
      this.Close(true);
    }

    private void OnUniversalCancelActionPressed(InputActionEventData data)
    {
      if (!this.initialized || !this.isOpen)
        return;
      if (this._universalCancelClosesScreen)
      {
        if (this.isFocused)
        {
          this.Close(true);
          return;
        }
      }
      else if (this.isFocused)
        return;
      this.CloseAllWindows();
    }

    private void OnWindowCancel(int windowId)
    {
      if (!this.initialized || windowId < 0)
        return;
      this.CloseWindow(windowId);
    }

    private void OnRemoveElementAssignment(int windowId, ControllerMap map, ActionElementMap aem)
    {
      if (map == null || aem == null)
        return;
      map.DeleteElementMap(aem.get_id());
      this.CloseWindow(windowId);
    }

    private void OnBeginElementAssignment(
      InputFieldInfo fieldInfo,
      ControllerMap map,
      ActionElementMap aem,
      string actionName)
    {
      if (Object.op_Equality((Object) fieldInfo, (Object) null) || map == null)
        return;
      this.pendingInputMapping = new Rewired.UI.ControlMapper.ControlMapper.InputMapping(actionName, fieldInfo, map, aem, fieldInfo.controllerType, fieldInfo.controllerId);
      switch ((int) fieldInfo.controllerType)
      {
        case 0:
          this.ShowElementAssignmentPollingWindow();
          break;
        case 1:
          this.ShowElementAssignmentPollingWindow();
          break;
        case 2:
          this.ShowElementAssignmentPrePollingWindow();
          break;
        default:
          throw new NotImplementedException();
      }
    }

    private void OnControllerAssignmentConfirmed(int windowId, Player player, int controllerId)
    {
      if (windowId < 0 || player == null || controllerId < 0)
        return;
      this.AssignController(player, controllerId);
      this.CloseWindow(windowId);
    }

    private void OnMouseAssignmentConfirmed(int windowId, Player player)
    {
      if (windowId < 0 || player == null)
        return;
      IList<Player> players = ReInput.get_players().get_Players();
      for (int index = 0; index < ((ICollection<Player>) players).Count; ++index)
      {
        if (players[index] != player)
          ((Player.ControllerHelper) players[index].controllers).set_hasMouse(false);
      }
      ((Player.ControllerHelper) player.controllers).set_hasMouse(true);
      this.CloseWindow(windowId);
    }

    private void OnElementAssignmentConflictReplaceConfirmed(
      int windowId,
      Rewired.UI.ControlMapper.ControlMapper.InputMapping mapping,
      ElementAssignment assignment,
      bool skipOtherPlayers,
      bool allowSwap)
    {
      if (this.currentPlayer == null || mapping == null)
        return;
      ElementAssignmentConflictCheck conflictCheck;
      if (!this.CreateConflictCheck(mapping, assignment, out conflictCheck))
      {
        Debug.LogError((object) "Rewired Control Mapper: Error creating conflict check!");
        this.CloseWindow(windowId);
      }
      else
      {
        ElementAssignmentConflictInfo conflict = (ElementAssignmentConflictInfo) null;
        ActionElementMap actionElementMap1 = (ActionElementMap) null;
        ActionElementMap actionElementMap2 = (ActionElementMap) null;
        bool flag1 = false;
        if (allowSwap && mapping.aem != null && this.GetFirstElementAssignmentConflict(conflictCheck, out conflict, skipOtherPlayers))
        {
          flag1 = true;
          actionElementMap2 = new ActionElementMap(mapping.aem);
          actionElementMap1 = new ActionElementMap(((ElementAssignmentConflictInfo) ref conflict).get_elementMap());
        }
        IList<Player> allPlayers = ReInput.get_players().get_AllPlayers();
        for (int index = 0; index < ((ICollection<Player>) allPlayers).Count; ++index)
        {
          Player player = allPlayers[index];
          if (!skipOtherPlayers || player == this.currentPlayer || player == ReInput.get_players().get_SystemPlayer())
            ((Player.ControllerHelper.ConflictCheckingHelper) ((Player.ControllerHelper) player.controllers).conflictChecking).RemoveElementAssignmentConflicts(conflictCheck);
        }
        mapping.map.ReplaceOrCreateElementMap(assignment);
        if (allowSwap && flag1)
        {
          int actionId = actionElementMap1.get_actionId();
          Pole axisContribution = actionElementMap1.get_axisContribution();
          bool flag2 = actionElementMap1.get_invert();
          AxisRange origAxisRange = actionElementMap2.get_axisRange();
          ControllerElementType elementType = actionElementMap2.get_elementType();
          int elementIdentifierId = actionElementMap2.get_elementIdentifierId();
          KeyCode keyCode = actionElementMap2.get_keyCode();
          ModifierKeyFlags modifierKeyFlags = actionElementMap2.get_modifierKeyFlags();
          if (elementType == actionElementMap1.get_elementType() && elementType == null)
          {
            if (origAxisRange != actionElementMap1.get_axisRange())
            {
              if (origAxisRange == null)
                origAxisRange = (AxisRange) 1;
              else if (actionElementMap1.get_axisRange() != null)
                ;
            }
          }
          else if (elementType == null && (actionElementMap1.get_elementType() == 1 || actionElementMap1.get_elementType() == null && actionElementMap1.get_axisRange() != null) && origAxisRange == null)
            origAxisRange = (AxisRange) 1;
          if (elementType != null || origAxisRange != null)
            flag2 = false;
          int num = 0;
          using (IEnumerator<ActionElementMap> enumerator = ((ElementAssignmentConflictInfo) ref conflict).get_controllerMap().ElementMapsWithAction(actionId).GetEnumerator())
          {
            while (((IEnumerator) enumerator).MoveNext())
            {
              ActionElementMap current = enumerator.Current;
              if (this.SwapIsSameInputRange(elementType, origAxisRange, axisContribution, current.get_elementType(), current.get_axisRange(), current.get_axisContribution()))
                ++num;
            }
          }
          if (num < this.GetControllerInputFieldCount(mapping.controllerType))
            ((ElementAssignmentConflictInfo) ref conflict).get_controllerMap().ReplaceOrCreateElementMap(ElementAssignment.CompleteAssignment(mapping.controllerType, elementType, elementIdentifierId, origAxisRange, keyCode, modifierKeyFlags, actionId, axisContribution, flag2));
        }
        this.CloseWindow(windowId);
      }
    }

    private void OnElementAssignmentAddConfirmed(
      int windowId,
      Rewired.UI.ControlMapper.ControlMapper.InputMapping mapping,
      ElementAssignment assignment)
    {
      if (this.currentPlayer == null || mapping == null)
        return;
      mapping.map.ReplaceOrCreateElementMap(assignment);
      this.CloseWindow(windowId);
    }

    private void OnRestoreDefaultsConfirmed(int windowId)
    {
      if (this._restoreDefaultsDelegate == null)
      {
        IList<Player> players = ReInput.get_players().get_Players();
        for (int index = 0; index < ((ICollection<Player>) players).Count; ++index)
        {
          Player player = players[index];
          if (this._showControllers)
            ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) player.controllers).maps).LoadDefaultMaps((ControllerType) 2);
          if (this._showKeyboard)
            ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) player.controllers).maps).LoadDefaultMaps((ControllerType) 0);
          if (this._showMouse)
            ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) player.controllers).maps).LoadDefaultMaps((ControllerType) 1);
        }
      }
      this.CloseWindow(windowId);
      if (this._restoreDefaultsDelegate == null)
        return;
      this._restoreDefaultsDelegate();
    }

    private void OnAssignControllerWindowUpdate(int windowId)
    {
      if (this.currentPlayer == null)
        return;
      Window window = this.windowManager.GetWindow(windowId);
      if (windowId < 0)
        return;
      this.InputPollingStarted();
      if (window.timer.finished)
      {
        this.InputPollingStopped();
        this.CloseWindow(windowId);
      }
      else
      {
        ControllerPollingInfo controllerPollingInfo = ((ReInput.ControllerHelper.PollingHelper) ReInput.get_controllers().polling).PollAllControllersOfTypeForFirstElementDown((ControllerType) 2);
        if (((ControllerPollingInfo) ref controllerPollingInfo).get_success())
        {
          this.InputPollingStopped();
          if (ReInput.get_controllers().IsControllerAssigned((ControllerType) 2, ((ControllerPollingInfo) ref controllerPollingInfo).get_controllerId()) && !((Player.ControllerHelper) this.currentPlayer.controllers).ContainsController((ControllerType) 2, ((ControllerPollingInfo) ref controllerPollingInfo).get_controllerId()))
            this.ShowControllerAssignmentConflictWindow(((ControllerPollingInfo) ref controllerPollingInfo).get_controllerId());
          else
            this.OnControllerAssignmentConfirmed(windowId, this.currentPlayer, ((ControllerPollingInfo) ref controllerPollingInfo).get_controllerId());
        }
        else
          window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
      }
    }

    private void OnElementAssignmentPrePollingWindowUpdate(int windowId)
    {
      if (this.currentPlayer == null)
        return;
      Window window = this.windowManager.GetWindow(windowId);
      if (windowId < 0 || this.pendingInputMapping == null)
        return;
      this.InputPollingStarted();
      if (!window.timer.finished)
      {
        window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
        ControllerPollingInfo controllerPollingInfo;
        switch ((int) this.pendingInputMapping.controllerType)
        {
          case 0:
          case 1:
            controllerPollingInfo = ((ReInput.ControllerHelper.PollingHelper) ReInput.get_controllers().polling).PollControllerForFirstButtonDown(this.pendingInputMapping.controllerType, 0);
            break;
          case 2:
            if (((Player.ControllerHelper) this.currentPlayer.controllers).get_joystickCount() == 0)
              return;
            controllerPollingInfo = ((ReInput.ControllerHelper.PollingHelper) ReInput.get_controllers().polling).PollControllerForFirstButtonDown(this.pendingInputMapping.controllerType, (int) ((Controller) this.currentJoystick).id);
            break;
          default:
            throw new NotImplementedException();
        }
        if (!((ControllerPollingInfo) ref controllerPollingInfo).get_success())
          return;
      }
      this.ShowElementAssignmentPollingWindow();
    }

    private void OnJoystickElementAssignmentPollingWindowUpdate(int windowId)
    {
      if (this.currentPlayer == null)
        return;
      Window window = this.windowManager.GetWindow(windowId);
      if (windowId < 0 || this.pendingInputMapping == null)
        return;
      this.InputPollingStarted();
      if (window.timer.finished)
      {
        this.InputPollingStopped();
        this.CloseWindow(windowId);
      }
      else
      {
        window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
        if (((Player.ControllerHelper) this.currentPlayer.controllers).get_joystickCount() == 0)
          return;
        ControllerPollingInfo pollingInfo = ((ReInput.ControllerHelper.PollingHelper) ReInput.get_controllers().polling).PollControllerForFirstElementDown((ControllerType) 2, (int) ((Controller) this.currentJoystick).id);
        if (!((ControllerPollingInfo) ref pollingInfo).get_success() || !this.IsAllowedAssignment(this.pendingInputMapping, pollingInfo))
          return;
        ElementAssignment elementAssignment = this.pendingInputMapping.ToElementAssignment(pollingInfo);
        if (!this.HasElementAssignmentConflicts(this.currentPlayer, this.pendingInputMapping, elementAssignment, false))
        {
          this.pendingInputMapping.map.ReplaceOrCreateElementMap(elementAssignment);
          this.InputPollingStopped();
          this.CloseWindow(windowId);
        }
        else
        {
          this.InputPollingStopped();
          this.ShowElementAssignmentConflictWindow(elementAssignment, false);
        }
      }
    }

    private void OnKeyboardElementAssignmentPollingWindowUpdate(int windowId)
    {
      if (this.currentPlayer == null)
        return;
      Window window = this.windowManager.GetWindow(windowId);
      if (windowId < 0 || this.pendingInputMapping == null)
        return;
      this.InputPollingStarted();
      if (window.timer.finished)
      {
        this.InputPollingStopped();
        this.CloseWindow(windowId);
      }
      else
      {
        ControllerPollingInfo pollingInfo;
        bool modifierKeyPressed;
        ModifierKeyFlags modifierFlags;
        string label;
        this.PollKeyboardForAssignment(out pollingInfo, out modifierKeyPressed, out modifierFlags, out label);
        if (modifierKeyPressed)
          window.timer.Start(this._inputAssignmentTimeout);
        window.SetContentText(!modifierKeyPressed ? Mathf.CeilToInt(window.timer.remaining).ToString() : string.Empty, 2);
        window.SetContentText(label, 1);
        if (!((ControllerPollingInfo) ref pollingInfo).get_success() || !this.IsAllowedAssignment(this.pendingInputMapping, pollingInfo))
          return;
        ElementAssignment elementAssignment = this.pendingInputMapping.ToElementAssignment(pollingInfo, modifierFlags);
        if (!this.HasElementAssignmentConflicts(this.currentPlayer, this.pendingInputMapping, elementAssignment, false))
        {
          this.pendingInputMapping.map.ReplaceOrCreateElementMap(elementAssignment);
          this.InputPollingStopped();
          this.CloseWindow(windowId);
        }
        else
        {
          this.InputPollingStopped();
          this.ShowElementAssignmentConflictWindow(elementAssignment, false);
        }
      }
    }

    private void OnMouseElementAssignmentPollingWindowUpdate(int windowId)
    {
      if (this.currentPlayer == null)
        return;
      Window window = this.windowManager.GetWindow(windowId);
      if (windowId < 0 || this.pendingInputMapping == null)
        return;
      this.InputPollingStarted();
      if (window.timer.finished)
      {
        this.InputPollingStopped();
        this.CloseWindow(windowId);
      }
      else
      {
        window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
        ControllerPollingInfo pollingInfo;
        if (this._ignoreMouseXAxisAssignment || this._ignoreMouseYAxisAssignment)
        {
          pollingInfo = (ControllerPollingInfo) null;
          using (IEnumerator<ControllerPollingInfo> enumerator = ((ReInput.ControllerHelper.PollingHelper) ReInput.get_controllers().polling).PollControllerForAllElementsDown((ControllerType) 1, 0).GetEnumerator())
          {
            while (((IEnumerator) enumerator).MoveNext())
            {
              ControllerPollingInfo current = enumerator.Current;
              if (((ControllerPollingInfo) ref current).get_elementType() != null || (!this._ignoreMouseXAxisAssignment || ((ControllerPollingInfo) ref current).get_elementIndex() != 0) && (!this._ignoreMouseYAxisAssignment || ((ControllerPollingInfo) ref current).get_elementIndex() != 1))
              {
                pollingInfo = current;
                break;
              }
            }
          }
        }
        else
          pollingInfo = ((ReInput.ControllerHelper.PollingHelper) ReInput.get_controllers().polling).PollControllerForFirstElementDown((ControllerType) 1, 0);
        if (!((ControllerPollingInfo) ref pollingInfo).get_success() || !this.IsAllowedAssignment(this.pendingInputMapping, pollingInfo))
          return;
        ElementAssignment elementAssignment = this.pendingInputMapping.ToElementAssignment(pollingInfo);
        if (!this.HasElementAssignmentConflicts(this.currentPlayer, this.pendingInputMapping, elementAssignment, true))
        {
          this.pendingInputMapping.map.ReplaceOrCreateElementMap(elementAssignment);
          this.InputPollingStopped();
          this.CloseWindow(windowId);
        }
        else
        {
          this.InputPollingStopped();
          this.ShowElementAssignmentConflictWindow(elementAssignment, true);
        }
      }
    }

    private void OnCalibrateAxisStep1WindowUpdate(int windowId)
    {
      if (this.currentPlayer == null)
        return;
      Window window = this.windowManager.GetWindow(windowId);
      if (windowId < 0 || this.pendingAxisCalibration == null || !this.pendingAxisCalibration.isValid)
        return;
      this.InputPollingStarted();
      if (!window.timer.finished)
      {
        window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
        if (((Player.ControllerHelper) this.currentPlayer.controllers).get_joystickCount() == 0)
          return;
        ControllerPollingInfo controllerPollingInfo = ((Controller) this.pendingAxisCalibration.joystick).PollForFirstButtonDown();
        if (!((ControllerPollingInfo) ref controllerPollingInfo).get_success())
          return;
      }
      this.pendingAxisCalibration.RecordZero();
      this.CloseWindow(windowId);
      this.ShowCalibrateAxisStep2Window();
    }

    private void OnCalibrateAxisStep2WindowUpdate(int windowId)
    {
      if (this.currentPlayer == null)
        return;
      Window window = this.windowManager.GetWindow(windowId);
      if (windowId < 0 || this.pendingAxisCalibration == null || !this.pendingAxisCalibration.isValid)
        return;
      if (!window.timer.finished)
      {
        window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
        this.pendingAxisCalibration.RecordMinMax();
        if (((Player.ControllerHelper) this.currentPlayer.controllers).get_joystickCount() == 0)
          return;
        ControllerPollingInfo controllerPollingInfo = ((Controller) this.pendingAxisCalibration.joystick).PollForFirstButtonDown();
        if (!((ControllerPollingInfo) ref controllerPollingInfo).get_success())
          return;
      }
      this.EndAxisCalibration();
      this.InputPollingStopped();
      this.CloseWindow(windowId);
    }

    private void ShowAssignControllerWindow()
    {
      if (this.currentPlayer == null || ReInput.get_controllers().get_joystickCount() == 0)
        return;
      Window window = this.OpenWindow(true);
      if (Object.op_Equality((Object) window, (Object) null))
        return;
      window.SetUpdateCallback(new Action<int>(this.OnAssignControllerWindowUpdate));
      window.CreateTitleText(this.prefabs.windowTitleText, Vector2.get_zero(), this._language.assignControllerWindowTitle);
      window.AddContentText(this.prefabs.windowContentText, UIPivot.get_TopCenter(), UIAnchor.get_TopHStretch(), new Vector2(0.0f, -100f), this._language.assignControllerWindowMessage);
      window.AddContentText(this.prefabs.windowContentText, UIPivot.get_BottomCenter(), UIAnchor.get_BottomHStretch(), Vector2.get_zero(), string.Empty);
      window.timer.Start(this._controllerAssignmentTimeout);
      this.windowManager.Focus(window);
    }

    private void ShowControllerAssignmentConflictWindow(int controllerId)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Rewired.UI.ControlMapper.ControlMapper.\u003CShowControllerAssignmentConflictWindow\u003Ec__AnonStorey1 windowCAnonStorey1 = new Rewired.UI.ControlMapper.ControlMapper.\u003CShowControllerAssignmentConflictWindow\u003Ec__AnonStorey1();
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey1.controllerId = controllerId;
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey1.\u0024this = this;
      if (this.currentPlayer == null || ReInput.get_controllers().get_joystickCount() == 0)
        return;
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey1.window = this.OpenWindow(true);
      // ISSUE: reference to a compiler-generated field
      if (Object.op_Equality((Object) windowCAnonStorey1.window, (Object) null))
        return;
      string otherPlayerName = string.Empty;
      IList<Player> players = ReInput.get_players().get_Players();
      for (int index = 0; index < ((ICollection<Player>) players).Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        if (players[index] != this.currentPlayer && ((Player.ControllerHelper) players[index].controllers).ContainsController((ControllerType) 2, windowCAnonStorey1.controllerId))
        {
          otherPlayerName = players[index].get_descriptiveName();
          break;
        }
      }
      // ISSUE: reference to a compiler-generated field
      Joystick joystick = ReInput.get_controllers().GetJoystick(windowCAnonStorey1.controllerId);
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey1.window.CreateTitleText(this.prefabs.windowTitleText, Vector2.get_zero(), this._language.controllerAssignmentConflictWindowTitle);
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey1.window.AddContentText(this.prefabs.windowContentText, UIPivot.get_TopCenter(), UIAnchor.get_TopHStretch(), new Vector2(0.0f, -100f), this._language.GetControllerAssignmentConflictWindowMessage(((Controller) joystick).get_name(), otherPlayerName, this.currentPlayer.get_descriptiveName()));
      // ISSUE: method pointer
      UnityAction unityAction = new UnityAction((object) windowCAnonStorey1, __methodptr(\u003C\u003Em__0));
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey1.window.cancelCallback = unityAction;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      windowCAnonStorey1.window.CreateButton(this.prefabs.fitButton, UIPivot.get_BottomLeft(), UIAnchor.get_BottomLeft(), Vector2.get_zero(), this._language.yes, new UnityAction((object) windowCAnonStorey1, __methodptr(\u003C\u003Em__1)), unityAction, true);
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey1.window.CreateButton(this.prefabs.fitButton, UIPivot.get_BottomRight(), UIAnchor.get_BottomRight(), Vector2.get_zero(), this._language.no, unityAction, unityAction, false);
      // ISSUE: reference to a compiler-generated field
      this.windowManager.Focus(windowCAnonStorey1.window);
    }

    private void ShowBeginElementAssignmentReplacementWindow(
      InputFieldInfo fieldInfo,
      InputAction action,
      ControllerMap map,
      ActionElementMap aem,
      string actionName)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Rewired.UI.ControlMapper.ControlMapper.\u003CShowBeginElementAssignmentReplacementWindow\u003Ec__AnonStorey2 windowCAnonStorey2 = new Rewired.UI.ControlMapper.ControlMapper.\u003CShowBeginElementAssignmentReplacementWindow\u003Ec__AnonStorey2();
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey2.fieldInfo = fieldInfo;
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey2.map = map;
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey2.aem = aem;
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey2.actionName = actionName;
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey2.\u0024this = this;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Rewired.UI.ControlMapper.ControlMapper.GUIInputField guiInputField = this.inputGrid.GetGUIInputField(this.currentMapCategoryId, action.get_id(), windowCAnonStorey2.fieldInfo.axisRange, windowCAnonStorey2.fieldInfo.controllerType, windowCAnonStorey2.fieldInfo.intData);
      if (guiInputField == null)
        return;
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey2.window = this.OpenWindow(true);
      // ISSUE: reference to a compiler-generated field
      if (Object.op_Equality((Object) windowCAnonStorey2.window, (Object) null))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey2.window.CreateTitleText(this.prefabs.windowTitleText, Vector2.get_zero(), windowCAnonStorey2.actionName);
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey2.window.AddContentText(this.prefabs.windowContentText, UIPivot.get_TopCenter(), UIAnchor.get_TopHStretch(), new Vector2(0.0f, -100f), guiInputField.GetLabel());
      // ISSUE: method pointer
      UnityAction unityAction = new UnityAction((object) windowCAnonStorey2, __methodptr(\u003C\u003Em__0));
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey2.window.cancelCallback = unityAction;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      windowCAnonStorey2.window.CreateButton(this.prefabs.fitButton, UIPivot.get_BottomLeft(), UIAnchor.get_BottomLeft(), Vector2.get_zero(), this._language.replace, new UnityAction((object) windowCAnonStorey2, __methodptr(\u003C\u003Em__1)), unityAction, true);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      windowCAnonStorey2.window.CreateButton(this.prefabs.fitButton, UIPivot.get_BottomCenter(), UIAnchor.get_BottomCenter(), Vector2.get_zero(), this._language.remove, new UnityAction((object) windowCAnonStorey2, __methodptr(\u003C\u003Em__2)), unityAction, false);
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey2.window.CreateButton(this.prefabs.fitButton, UIPivot.get_BottomRight(), UIAnchor.get_BottomRight(), Vector2.get_zero(), this._language.cancel, unityAction, unityAction, false);
      // ISSUE: reference to a compiler-generated field
      this.windowManager.Focus(windowCAnonStorey2.window);
    }

    private void ShowCreateNewElementAssignmentWindow(
      InputFieldInfo fieldInfo,
      InputAction action,
      ControllerMap map,
      string actionName)
    {
      if (this.inputGrid.GetGUIInputField(this.currentMapCategoryId, action.get_id(), fieldInfo.axisRange, fieldInfo.controllerType, fieldInfo.intData) == null)
        return;
      this.OnBeginElementAssignment(fieldInfo, map, (ActionElementMap) null, actionName);
    }

    private void ShowElementAssignmentPrePollingWindow()
    {
      if (this.pendingInputMapping == null)
        return;
      Window window = this.OpenWindow(true);
      if (Object.op_Equality((Object) window, (Object) null))
        return;
      window.CreateTitleText(this.prefabs.windowTitleText, Vector2.get_zero(), this.pendingInputMapping.actionName);
      window.AddContentText(this.prefabs.windowContentText, UIPivot.get_TopCenter(), UIAnchor.get_TopHStretch(), new Vector2(0.0f, -100f), this._language.elementAssignmentPrePollingWindowMessage);
      if (Object.op_Inequality((Object) this.prefabs.centerStickGraphic, (Object) null))
        window.AddContentImage(this.prefabs.centerStickGraphic, UIPivot.get_BottomCenter(), UIAnchor.get_BottomCenter(), new Vector2(0.0f, 40f));
      window.AddContentText(this.prefabs.windowContentText, UIPivot.get_BottomCenter(), UIAnchor.get_BottomHStretch(), Vector2.get_zero(), string.Empty);
      window.SetUpdateCallback(new Action<int>(this.OnElementAssignmentPrePollingWindowUpdate));
      window.timer.Start(this._preInputAssignmentTimeout);
      this.windowManager.Focus(window);
    }

    private void ShowElementAssignmentPollingWindow()
    {
      if (this.pendingInputMapping == null)
        return;
      switch ((int) this.pendingInputMapping.controllerType)
      {
        case 0:
          this.ShowKeyboardElementAssignmentPollingWindow();
          break;
        case 1:
          if (((Player.ControllerHelper) this.currentPlayer.controllers).get_hasMouse())
          {
            this.ShowMouseElementAssignmentPollingWindow();
            break;
          }
          this.ShowMouseAssignmentConflictWindow();
          break;
        case 2:
          this.ShowJoystickElementAssignmentPollingWindow();
          break;
        default:
          throw new NotImplementedException();
      }
    }

    private void ShowJoystickElementAssignmentPollingWindow()
    {
      if (this.pendingInputMapping == null)
        return;
      Window window = this.OpenWindow(true);
      if (Object.op_Equality((Object) window, (Object) null))
        return;
      string text = this.pendingInputMapping.axisRange != null || !this._showFullAxisInputFields || this._showSplitAxisInputFields ? this._language.GetJoystickElementAssignmentPollingWindowMessage(this.pendingInputMapping.actionName) : this._language.GetJoystickElementAssignmentPollingWindowMessage_FullAxisFieldOnly(this.pendingInputMapping.actionName);
      window.CreateTitleText(this.prefabs.windowTitleText, Vector2.get_zero(), this.pendingInputMapping.actionName);
      window.AddContentText(this.prefabs.windowContentText, UIPivot.get_TopCenter(), UIAnchor.get_TopHStretch(), new Vector2(0.0f, -100f), text);
      window.AddContentText(this.prefabs.windowContentText, UIPivot.get_BottomCenter(), UIAnchor.get_BottomHStretch(), Vector2.get_zero(), string.Empty);
      window.SetUpdateCallback(new Action<int>(this.OnJoystickElementAssignmentPollingWindowUpdate));
      window.timer.Start(this._inputAssignmentTimeout);
      this.windowManager.Focus(window);
    }

    private void ShowKeyboardElementAssignmentPollingWindow()
    {
      if (this.pendingInputMapping == null)
        return;
      Window window = this.OpenWindow(true);
      if (Object.op_Equality((Object) window, (Object) null))
        return;
      window.CreateTitleText(this.prefabs.windowTitleText, Vector2.get_zero(), this.pendingInputMapping.actionName);
      window.AddContentText(this.prefabs.windowContentText, UIPivot.get_TopCenter(), UIAnchor.get_TopHStretch(), new Vector2(0.0f, -100f), this._language.GetKeyboardElementAssignmentPollingWindowMessage(this.pendingInputMapping.actionName));
      window.AddContentText(this.prefabs.windowContentText, UIPivot.get_TopCenter(), UIAnchor.get_TopHStretch(), new Vector2(0.0f, (float) -((double) window.GetContentTextHeight(0) + 50.0)), string.Empty);
      window.AddContentText(this.prefabs.windowContentText, UIPivot.get_BottomCenter(), UIAnchor.get_BottomHStretch(), Vector2.get_zero(), string.Empty);
      window.SetUpdateCallback(new Action<int>(this.OnKeyboardElementAssignmentPollingWindowUpdate));
      window.timer.Start(this._inputAssignmentTimeout);
      this.windowManager.Focus(window);
    }

    private void ShowMouseElementAssignmentPollingWindow()
    {
      if (this.pendingInputMapping == null)
        return;
      Window window = this.OpenWindow(true);
      if (Object.op_Equality((Object) window, (Object) null))
        return;
      string text = this.pendingInputMapping.axisRange != null || !this._showFullAxisInputFields || this._showSplitAxisInputFields ? this._language.GetMouseElementAssignmentPollingWindowMessage(this.pendingInputMapping.actionName) : this._language.GetMouseElementAssignmentPollingWindowMessage_FullAxisFieldOnly(this.pendingInputMapping.actionName);
      window.CreateTitleText(this.prefabs.windowTitleText, Vector2.get_zero(), this.pendingInputMapping.actionName);
      window.AddContentText(this.prefabs.windowContentText, UIPivot.get_TopCenter(), UIAnchor.get_TopHStretch(), new Vector2(0.0f, -100f), text);
      window.AddContentText(this.prefabs.windowContentText, UIPivot.get_BottomCenter(), UIAnchor.get_BottomHStretch(), Vector2.get_zero(), string.Empty);
      window.SetUpdateCallback(new Action<int>(this.OnMouseElementAssignmentPollingWindowUpdate));
      window.timer.Start(this._inputAssignmentTimeout);
      this.windowManager.Focus(window);
    }

    private void ShowElementAssignmentConflictWindow(
      ElementAssignment assignment,
      bool skipOtherPlayers)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Rewired.UI.ControlMapper.ControlMapper.\u003CShowElementAssignmentConflictWindow\u003Ec__AnonStorey3 windowCAnonStorey3 = new Rewired.UI.ControlMapper.ControlMapper.\u003CShowElementAssignmentConflictWindow\u003Ec__AnonStorey3();
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey3.assignment = assignment;
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey3.skipOtherPlayers = skipOtherPlayers;
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey3.\u0024this = this;
      if (this.pendingInputMapping == null)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      bool flag = this.IsBlockingAssignmentConflict(this.pendingInputMapping, windowCAnonStorey3.assignment, windowCAnonStorey3.skipOtherPlayers);
      string text = !flag ? this._language.GetElementAlreadyInUseCanReplace(this.pendingInputMapping.elementName, this._allowElementAssignmentConflicts) : this._language.GetElementAlreadyInUseBlocked(this.pendingInputMapping.elementName);
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey3.window = this.OpenWindow(true);
      // ISSUE: reference to a compiler-generated field
      if (Object.op_Equality((Object) windowCAnonStorey3.window, (Object) null))
        return;
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey3.window.CreateTitleText(this.prefabs.windowTitleText, Vector2.get_zero(), this._language.elementAssignmentConflictWindowMessage);
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey3.window.AddContentText(this.prefabs.windowContentText, UIPivot.get_TopCenter(), UIAnchor.get_TopHStretch(), new Vector2(0.0f, -100f), text);
      // ISSUE: method pointer
      UnityAction unityAction = new UnityAction((object) windowCAnonStorey3, __methodptr(\u003C\u003Em__0));
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey3.window.cancelCallback = unityAction;
      if (flag)
      {
        // ISSUE: reference to a compiler-generated field
        windowCAnonStorey3.window.CreateButton(this.prefabs.fitButton, UIPivot.get_BottomCenter(), UIAnchor.get_BottomCenter(), Vector2.get_zero(), this._language.okay, unityAction, unityAction, true);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        windowCAnonStorey3.window.CreateButton(this.prefabs.fitButton, UIPivot.get_BottomLeft(), UIAnchor.get_BottomLeft(), Vector2.get_zero(), this._language.replace, new UnityAction((object) windowCAnonStorey3, __methodptr(\u003C\u003Em__1)), unityAction, true);
        if (this._allowElementAssignmentConflicts)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          windowCAnonStorey3.window.CreateButton(this.prefabs.fitButton, UIPivot.get_BottomCenter(), UIAnchor.get_BottomCenter(), Vector2.get_zero(), this._language.add, new UnityAction((object) windowCAnonStorey3, __methodptr(\u003C\u003Em__2)), unityAction, false);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.ShowSwapButton(windowCAnonStorey3.window.id, this.pendingInputMapping, windowCAnonStorey3.assignment, windowCAnonStorey3.skipOtherPlayers))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: method pointer
            windowCAnonStorey3.window.CreateButton(this.prefabs.fitButton, UIPivot.get_BottomCenter(), UIAnchor.get_BottomCenter(), Vector2.get_zero(), this._language.swap, new UnityAction((object) windowCAnonStorey3, __methodptr(\u003C\u003Em__3)), unityAction, false);
          }
        }
        // ISSUE: reference to a compiler-generated field
        windowCAnonStorey3.window.CreateButton(this.prefabs.fitButton, UIPivot.get_BottomRight(), UIAnchor.get_BottomRight(), Vector2.get_zero(), this._language.cancel, unityAction, unityAction, false);
      }
      // ISSUE: reference to a compiler-generated field
      this.windowManager.Focus(windowCAnonStorey3.window);
    }

    private void ShowMouseAssignmentConflictWindow()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Rewired.UI.ControlMapper.ControlMapper.\u003CShowMouseAssignmentConflictWindow\u003Ec__AnonStorey4 windowCAnonStorey4 = new Rewired.UI.ControlMapper.ControlMapper.\u003CShowMouseAssignmentConflictWindow\u003Ec__AnonStorey4();
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey4.\u0024this = this;
      if (this.currentPlayer == null)
        return;
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey4.window = this.OpenWindow(true);
      // ISSUE: reference to a compiler-generated field
      if (Object.op_Equality((Object) windowCAnonStorey4.window, (Object) null))
        return;
      string otherPlayerName = string.Empty;
      IList<Player> players = ReInput.get_players().get_Players();
      for (int index = 0; index < ((ICollection<Player>) players).Count; ++index)
      {
        if (players[index] != this.currentPlayer && ((Player.ControllerHelper) players[index].controllers).get_hasMouse())
        {
          otherPlayerName = players[index].get_descriptiveName();
          break;
        }
      }
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey4.window.CreateTitleText(this.prefabs.windowTitleText, Vector2.get_zero(), this._language.mouseAssignmentConflictWindowTitle);
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey4.window.AddContentText(this.prefabs.windowContentText, UIPivot.get_TopCenter(), UIAnchor.get_TopHStretch(), new Vector2(0.0f, -100f), this._language.GetMouseAssignmentConflictWindowMessage(otherPlayerName, this.currentPlayer.get_descriptiveName()));
      // ISSUE: method pointer
      UnityAction unityAction = new UnityAction((object) windowCAnonStorey4, __methodptr(\u003C\u003Em__0));
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey4.window.cancelCallback = unityAction;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      windowCAnonStorey4.window.CreateButton(this.prefabs.fitButton, UIPivot.get_BottomLeft(), UIAnchor.get_BottomLeft(), Vector2.get_zero(), this._language.yes, new UnityAction((object) windowCAnonStorey4, __methodptr(\u003C\u003Em__1)), unityAction, true);
      // ISSUE: reference to a compiler-generated field
      windowCAnonStorey4.window.CreateButton(this.prefabs.fitButton, UIPivot.get_BottomRight(), UIAnchor.get_BottomRight(), Vector2.get_zero(), this._language.no, unityAction, unityAction, false);
      // ISSUE: reference to a compiler-generated field
      this.windowManager.Focus(windowCAnonStorey4.window);
    }

    private void ShowCalibrateControllerWindow()
    {
      if (this.currentPlayer == null || ((Player.ControllerHelper) this.currentPlayer.controllers).get_joystickCount() == 0)
        return;
      CalibrationWindow calibrationWindow = this.OpenWindow(this.prefabs.calibrationWindow, "CalibrationWindow", true) as CalibrationWindow;
      if (Object.op_Equality((Object) calibrationWindow, (Object) null))
        return;
      Joystick currentJoystick = this.currentJoystick;
      calibrationWindow.CreateTitleText(this.prefabs.windowTitleText, Vector2.get_zero(), this._language.calibrateControllerWindowTitle);
      calibrationWindow.SetJoystick(this.currentPlayer.get_id(), currentJoystick);
      calibrationWindow.SetButtonCallback(CalibrationWindow.ButtonIdentifier.Done, new Action<int>(this.CloseWindow));
      calibrationWindow.SetButtonCallback(CalibrationWindow.ButtonIdentifier.Calibrate, new Action<int>(this.StartAxisCalibration));
      calibrationWindow.SetButtonCallback(CalibrationWindow.ButtonIdentifier.Cancel, new Action<int>(this.CloseWindow));
      this.windowManager.Focus((Window) calibrationWindow);
    }

    private void ShowCalibrateAxisStep1Window()
    {
      if (this.currentPlayer == null)
        return;
      Window window = this.OpenWindow(false);
      if (Object.op_Equality((Object) window, (Object) null) || this.pendingAxisCalibration == null)
        return;
      Joystick joystick = this.pendingAxisCalibration.joystick;
      if (((ControllerWithAxes) joystick).get_axisCount() == 0)
        return;
      int axisIndex = this.pendingAxisCalibration.axisIndex;
      if (axisIndex < 0 || axisIndex >= ((ControllerWithAxes) joystick).get_axisCount())
        return;
      window.CreateTitleText(this.prefabs.windowTitleText, Vector2.get_zero(), this._language.calibrateAxisStep1WindowTitle);
      window.AddContentText(this.prefabs.windowContentText, UIPivot.get_TopCenter(), UIAnchor.get_TopHStretch(), new Vector2(0.0f, -100f), this._language.GetCalibrateAxisStep1WindowMessage(((ControllerWithAxes) joystick).get_AxisElementIdentifiers()[axisIndex].get_name()));
      if (Object.op_Inequality((Object) this.prefabs.centerStickGraphic, (Object) null))
        window.AddContentImage(this.prefabs.centerStickGraphic, UIPivot.get_BottomCenter(), UIAnchor.get_BottomCenter(), new Vector2(0.0f, 40f));
      window.AddContentText(this.prefabs.windowContentText, UIPivot.get_BottomCenter(), UIAnchor.get_BottomHStretch(), Vector2.get_zero(), string.Empty);
      window.SetUpdateCallback(new Action<int>(this.OnCalibrateAxisStep1WindowUpdate));
      window.timer.Start(this._axisCalibrationTimeout);
      this.windowManager.Focus(window);
    }

    private void ShowCalibrateAxisStep2Window()
    {
      if (this.currentPlayer == null)
        return;
      Window window = this.OpenWindow(false);
      if (Object.op_Equality((Object) window, (Object) null) || this.pendingAxisCalibration == null)
        return;
      Joystick joystick = this.pendingAxisCalibration.joystick;
      if (((ControllerWithAxes) joystick).get_axisCount() == 0)
        return;
      int axisIndex = this.pendingAxisCalibration.axisIndex;
      if (axisIndex < 0 || axisIndex >= ((ControllerWithAxes) joystick).get_axisCount())
        return;
      window.CreateTitleText(this.prefabs.windowTitleText, Vector2.get_zero(), this._language.calibrateAxisStep2WindowTitle);
      window.AddContentText(this.prefabs.windowContentText, UIPivot.get_TopCenter(), UIAnchor.get_TopHStretch(), new Vector2(0.0f, -100f), this._language.GetCalibrateAxisStep2WindowMessage(((ControllerWithAxes) joystick).get_AxisElementIdentifiers()[axisIndex].get_name()));
      if (Object.op_Inequality((Object) this.prefabs.moveStickGraphic, (Object) null))
        window.AddContentImage(this.prefabs.moveStickGraphic, UIPivot.get_BottomCenter(), UIAnchor.get_BottomCenter(), new Vector2(0.0f, 40f));
      window.AddContentText(this.prefabs.windowContentText, UIPivot.get_BottomCenter(), UIAnchor.get_BottomHStretch(), Vector2.get_zero(), string.Empty);
      window.SetUpdateCallback(new Action<int>(this.OnCalibrateAxisStep2WindowUpdate));
      window.timer.Start(this._axisCalibrationTimeout);
      this.windowManager.Focus(window);
    }

    private void ShowEditInputBehaviorsWindow()
    {
      if (this.currentPlayer == null || this._inputBehaviorSettings == null)
        return;
      InputBehaviorWindow inputBehaviorWindow = this.OpenWindow(this.prefabs.inputBehaviorsWindow, "EditInputBehaviorsWindow", true) as InputBehaviorWindow;
      if (Object.op_Equality((Object) inputBehaviorWindow, (Object) null))
        return;
      inputBehaviorWindow.CreateTitleText(this.prefabs.windowTitleText, Vector2.get_zero(), this._language.inputBehaviorSettingsWindowTitle);
      inputBehaviorWindow.SetData(this.currentPlayer.get_id(), this._inputBehaviorSettings);
      inputBehaviorWindow.SetButtonCallback(InputBehaviorWindow.ButtonIdentifier.Done, new Action<int>(this.CloseWindow));
      inputBehaviorWindow.SetButtonCallback(InputBehaviorWindow.ButtonIdentifier.Cancel, new Action<int>(this.CloseWindow));
      this.windowManager.Focus((Window) inputBehaviorWindow);
    }

    private void ShowRestoreDefaultsWindow()
    {
      if (this.currentPlayer == null)
        return;
      this.OpenModal(this._language.restoreDefaultsWindowTitle, this._language.restoreDefaultsWindowMessage, this._language.yes, new Action<int>(this.OnRestoreDefaultsConfirmed), this._language.no, new Action<int>(this.OnWindowCancel), true);
    }

    private void CreateInputGrid()
    {
      this.InitializeInputGrid();
      this.CreateHeaderLabels();
      this.CreateActionLabelColumn();
      this.CreateKeyboardInputFieldColumn();
      this.CreateMouseInputFieldColumn();
      this.CreateControllerInputFieldColumn();
      this.CreateInputActionLabels();
      this.CreateInputFields();
      this.inputGrid.HideAll();
      this.ResetInputGridScrollBar();
    }

    private void InitializeInputGrid()
    {
      if (this.inputGrid == null)
        this.inputGrid = new Rewired.UI.ControlMapper.ControlMapper.InputGrid();
      else
        this.inputGrid.ClearAll();
      for (int index1 = 0; index1 < this._mappingSets.Length; ++index1)
      {
        Rewired.UI.ControlMapper.ControlMapper.MappingSet mappingSet = this._mappingSets[index1];
        if (mappingSet != null && mappingSet.isValid)
        {
          InputMapCategory mapCategory = ReInput.get_mapping().GetMapCategory(mappingSet.mapCategoryId);
          if (mapCategory != null && ((InputCategory) mapCategory).get_userAssignable())
          {
            this.inputGrid.AddMapCategory(mappingSet.mapCategoryId);
            if (mappingSet.actionListMode == Rewired.UI.ControlMapper.ControlMapper.MappingSet.ActionListMode.ActionCategory)
            {
              IList<int> actionCategoryIds = mappingSet.actionCategoryIds;
              for (int index2 = 0; index2 < actionCategoryIds.Count; ++index2)
              {
                int actionCategoryId = actionCategoryIds[index2];
                InputCategory actionCategory = ReInput.get_mapping().GetActionCategory(actionCategoryId);
                if (actionCategory != null && actionCategory.get_userAssignable())
                {
                  this.inputGrid.AddActionCategory(mappingSet.mapCategoryId, actionCategoryId);
                  using (IEnumerator<InputAction> enumerator = ReInput.get_mapping().UserAssignableActionsInCategory(actionCategoryId).GetEnumerator())
                  {
                    while (((IEnumerator) enumerator).MoveNext())
                    {
                      InputAction current = enumerator.Current;
                      if (current.get_type() == null)
                      {
                        if (this._showFullAxisInputFields)
                          this.inputGrid.AddAction(mappingSet.mapCategoryId, current, (AxisRange) 0);
                        if (this._showSplitAxisInputFields)
                        {
                          this.inputGrid.AddAction(mappingSet.mapCategoryId, current, (AxisRange) 1);
                          this.inputGrid.AddAction(mappingSet.mapCategoryId, current, (AxisRange) 2);
                        }
                      }
                      else if (current.get_type() == 1)
                        this.inputGrid.AddAction(mappingSet.mapCategoryId, current, (AxisRange) 1);
                    }
                  }
                }
              }
            }
            else
            {
              IList<int> actionIds = mappingSet.actionIds;
              for (int index2 = 0; index2 < actionIds.Count; ++index2)
              {
                InputAction action = ReInput.get_mapping().GetAction(actionIds[index2]);
                if (action != null)
                {
                  if (action.get_type() == null)
                  {
                    if (this._showFullAxisInputFields)
                      this.inputGrid.AddAction(mappingSet.mapCategoryId, action, (AxisRange) 0);
                    if (this._showSplitAxisInputFields)
                    {
                      this.inputGrid.AddAction(mappingSet.mapCategoryId, action, (AxisRange) 1);
                      this.inputGrid.AddAction(mappingSet.mapCategoryId, action, (AxisRange) 2);
                    }
                  }
                  else if (action.get_type() == 1)
                    this.inputGrid.AddAction(mappingSet.mapCategoryId, action, (AxisRange) 1);
                }
              }
            }
          }
        }
      }
      ((HorizontalOrVerticalLayoutGroup) ((Component) this.references.inputGridInnerGroup).GetComponent<HorizontalLayoutGroup>()).set_spacing((float) this._inputColumnSpacing);
      this.references.inputGridLayoutElement.set_flexibleWidth(0.0f);
      this.references.inputGridLayoutElement.set_preferredWidth((float) this.inputGridWidth);
    }

    private void RefreshInputGridStructure()
    {
      if (this.currentMappingSet == null)
        return;
      this.inputGrid.HideAll();
      this.inputGrid.Show(this.currentMappingSet.mapCategoryId);
      ((RectTransform) ((Component) this.references.inputGridInnerGroup).GetComponent<RectTransform>()).SetSizeWithCurrentAnchors((RectTransform.Axis) 1, this.inputGrid.GetColumnHeight(this.currentMappingSet.mapCategoryId));
    }

    private void CreateHeaderLabels()
    {
      this.references.inputGridHeader1 = this.CreateNewColumnGroup("ActionsHeader", this.references.inputGridHeadersGroup, this._actionLabelWidth).get_transform();
      this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.actionColumnLabel, this.references.inputGridHeader1, Vector2.get_zero());
      if (this._showKeyboard)
      {
        this.references.inputGridHeader2 = this.CreateNewColumnGroup("KeybordHeader", this.references.inputGridHeadersGroup, this._keyboardColMaxWidth).get_transform();
        this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.keyboardColumnLabel, this.references.inputGridHeader2, Vector2.get_zero()).SetTextAlignment((TextAnchor) 4);
      }
      if (this._showMouse)
      {
        this.references.inputGridHeader3 = this.CreateNewColumnGroup("MouseHeader", this.references.inputGridHeadersGroup, this._mouseColMaxWidth).get_transform();
        this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.mouseColumnLabel, this.references.inputGridHeader3, Vector2.get_zero()).SetTextAlignment((TextAnchor) 4);
      }
      if (!this._showControllers)
        return;
      this.references.inputGridHeader4 = this.CreateNewColumnGroup("ControllerHeader", this.references.inputGridHeadersGroup, this._controllerColMaxWidth).get_transform();
      this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.controllerColumnLabel, this.references.inputGridHeader4, Vector2.get_zero()).SetTextAlignment((TextAnchor) 4);
    }

    private void CreateActionLabelColumn()
    {
      this.references.inputGridActionColumn = this.CreateNewColumnGroup("ActionLabelColumn", this.references.inputGridInnerGroup, this._actionLabelWidth).get_transform();
    }

    private void CreateKeyboardInputFieldColumn()
    {
      if (!this._showKeyboard)
        return;
      this.CreateInputFieldColumn("KeyboardColumn", (ControllerType) 0, this._keyboardColMaxWidth, this._keyboardInputFieldCount, true);
    }

    private void CreateMouseInputFieldColumn()
    {
      if (!this._showMouse)
        return;
      this.CreateInputFieldColumn("MouseColumn", (ControllerType) 1, this._mouseColMaxWidth, this._mouseInputFieldCount, false);
    }

    private void CreateControllerInputFieldColumn()
    {
      if (!this._showControllers)
        return;
      this.CreateInputFieldColumn("ControllerColumn", (ControllerType) 2, this._controllerColMaxWidth, this._controllerInputFieldCount, false);
    }

    private void CreateInputFieldColumn(
      string name,
      ControllerType controllerType,
      int maxWidth,
      int cols,
      bool disableFullAxis)
    {
      Transform transform = this.CreateNewColumnGroup(name, this.references.inputGridInnerGroup, maxWidth).get_transform();
      switch ((int) controllerType)
      {
        case 0:
          this.references.inputGridKeyboardColumn = transform;
          break;
        case 1:
          this.references.inputGridMouseColumn = transform;
          break;
        case 2:
          this.references.inputGridControllerColumn = transform;
          break;
        default:
          throw new NotImplementedException();
      }
    }

    private void CreateInputActionLabels()
    {
      Transform gridActionColumn = this.references.inputGridActionColumn;
      for (int index1 = 0; index1 < this._mappingSets.Length; ++index1)
      {
        Rewired.UI.ControlMapper.ControlMapper.MappingSet mappingSet = this._mappingSets[index1];
        if (mappingSet != null && mappingSet.isValid)
        {
          int num1 = 0;
          if (mappingSet.actionListMode == Rewired.UI.ControlMapper.ControlMapper.MappingSet.ActionListMode.ActionCategory)
          {
            int num2 = 0;
            IList<int> actionCategoryIds = mappingSet.actionCategoryIds;
            for (int index2 = 0; index2 < actionCategoryIds.Count; ++index2)
            {
              InputCategory actionCategory = ReInput.get_mapping().GetActionCategory(actionCategoryIds[index2]);
              if (actionCategory != null && actionCategory.get_userAssignable() && this.CountIEnumerable<InputAction>(ReInput.get_mapping().UserAssignableActionsInCategory(actionCategory.get_id())) != 0)
              {
                if (this._showActionCategoryLabels)
                {
                  if (num2 > 0)
                    num1 -= this._inputRowCategorySpacing;
                  Rewired.UI.ControlMapper.ControlMapper.GUILabel label = this.CreateLabel(actionCategory.get_descriptiveName(), gridActionColumn, new Vector2(0.0f, (float) num1));
                  label.SetFontStyle((FontStyle) 1);
                  label.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, (float) this._inputRowHeight);
                  this.inputGrid.AddActionCategoryLabel(mappingSet.mapCategoryId, actionCategory.get_id(), label);
                  num1 -= this._inputRowHeight;
                }
                using (IEnumerator<InputAction> enumerator = ReInput.get_mapping().UserAssignableActionsInCategory(actionCategory.get_id(), true).GetEnumerator())
                {
                  while (((IEnumerator) enumerator).MoveNext())
                  {
                    InputAction current = enumerator.Current;
                    if (current.get_type() == null)
                    {
                      if (this._showFullAxisInputFields)
                      {
                        Rewired.UI.ControlMapper.ControlMapper.GUILabel label = this.CreateLabel(current.get_descriptiveName(), gridActionColumn, new Vector2(0.0f, (float) num1));
                        label.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, (float) this._inputRowHeight);
                        this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, current.get_id(), (AxisRange) 0, label);
                        num1 -= this._inputRowHeight;
                      }
                      if (this._showSplitAxisInputFields)
                      {
                        Rewired.UI.ControlMapper.ControlMapper.GUILabel label1 = this.CreateLabel(string.IsNullOrEmpty(current.get_positiveDescriptiveName()) ? current.get_descriptiveName() + " +" : current.get_positiveDescriptiveName(), gridActionColumn, new Vector2(0.0f, (float) num1));
                        label1.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, (float) this._inputRowHeight);
                        this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, current.get_id(), (AxisRange) 1, label1);
                        num1 -= this._inputRowHeight;
                        Rewired.UI.ControlMapper.ControlMapper.GUILabel label2 = this.CreateLabel(string.IsNullOrEmpty(current.get_negativeDescriptiveName()) ? current.get_descriptiveName() + " -" : current.get_negativeDescriptiveName(), gridActionColumn, new Vector2(0.0f, (float) num1));
                        label2.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, (float) this._inputRowHeight);
                        this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, current.get_id(), (AxisRange) 2, label2);
                        num1 -= this._inputRowHeight;
                      }
                    }
                    else if (current.get_type() == 1)
                    {
                      Rewired.UI.ControlMapper.ControlMapper.GUILabel label = this.CreateLabel(current.get_descriptiveName(), gridActionColumn, new Vector2(0.0f, (float) num1));
                      label.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, (float) this._inputRowHeight);
                      this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, current.get_id(), (AxisRange) 1, label);
                      num1 -= this._inputRowHeight;
                    }
                  }
                }
                ++num2;
              }
            }
          }
          else
          {
            IList<int> actionIds = mappingSet.actionIds;
            for (int index2 = 0; index2 < actionIds.Count; ++index2)
            {
              InputAction action = ReInput.get_mapping().GetAction(actionIds[index2]);
              if (action != null && action.get_userAssignable())
              {
                InputCategory actionCategory = ReInput.get_mapping().GetActionCategory(action.get_categoryId());
                if (actionCategory != null && actionCategory.get_userAssignable())
                {
                  if (action.get_type() == null)
                  {
                    if (this._showFullAxisInputFields)
                    {
                      Rewired.UI.ControlMapper.ControlMapper.GUILabel label = this.CreateLabel(action.get_descriptiveName(), gridActionColumn, new Vector2(0.0f, (float) num1));
                      label.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, (float) this._inputRowHeight);
                      this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.get_id(), (AxisRange) 0, label);
                      num1 -= this._inputRowHeight;
                    }
                    if (this._showSplitAxisInputFields)
                    {
                      Rewired.UI.ControlMapper.ControlMapper.GUILabel label1 = this.CreateLabel(action.get_positiveDescriptiveName(), gridActionColumn, new Vector2(0.0f, (float) num1));
                      label1.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, (float) this._inputRowHeight);
                      this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.get_id(), (AxisRange) 1, label1);
                      int num2 = num1 - this._inputRowHeight;
                      Rewired.UI.ControlMapper.ControlMapper.GUILabel label2 = this.CreateLabel(action.get_negativeDescriptiveName(), gridActionColumn, new Vector2(0.0f, (float) num2));
                      label2.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, (float) this._inputRowHeight);
                      this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.get_id(), (AxisRange) 2, label2);
                      num1 = num2 - this._inputRowHeight;
                    }
                  }
                  else if (action.get_type() == 1)
                  {
                    Rewired.UI.ControlMapper.ControlMapper.GUILabel label = this.CreateLabel(action.get_descriptiveName(), gridActionColumn, new Vector2(0.0f, (float) num1));
                    label.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, (float) this._inputRowHeight);
                    this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.get_id(), (AxisRange) 1, label);
                    num1 -= this._inputRowHeight;
                  }
                }
              }
            }
          }
          this.inputGrid.SetColumnHeight(mappingSet.mapCategoryId, (float) -num1);
        }
      }
    }

    private void CreateInputFields()
    {
      if (this._showControllers)
        this.CreateInputFields(this.references.inputGridControllerColumn, (ControllerType) 2, this._controllerColMaxWidth, this._controllerInputFieldCount, false);
      if (this._showKeyboard)
        this.CreateInputFields(this.references.inputGridKeyboardColumn, (ControllerType) 0, this._keyboardColMaxWidth, this._keyboardInputFieldCount, true);
      if (!this._showMouse)
        return;
      this.CreateInputFields(this.references.inputGridMouseColumn, (ControllerType) 1, this._mouseColMaxWidth, this._mouseInputFieldCount, false);
    }

    private void CreateInputFields(
      Transform columnXform,
      ControllerType controllerType,
      int maxWidth,
      int cols,
      bool disableFullAxis)
    {
      for (int index1 = 0; index1 < this._mappingSets.Length; ++index1)
      {
        Rewired.UI.ControlMapper.ControlMapper.MappingSet mappingSet = this._mappingSets[index1];
        if (mappingSet != null && mappingSet.isValid)
        {
          int fieldWidth = maxWidth / cols;
          int yPos = 0;
          int num = 0;
          if (mappingSet.actionListMode == Rewired.UI.ControlMapper.ControlMapper.MappingSet.ActionListMode.ActionCategory)
          {
            IList<int> actionCategoryIds = mappingSet.actionCategoryIds;
            for (int index2 = 0; index2 < actionCategoryIds.Count; ++index2)
            {
              InputCategory actionCategory = ReInput.get_mapping().GetActionCategory(actionCategoryIds[index2]);
              if (actionCategory != null && actionCategory.get_userAssignable() && this.CountIEnumerable<InputAction>(ReInput.get_mapping().UserAssignableActionsInCategory(actionCategory.get_id())) != 0)
              {
                if (this._showActionCategoryLabels)
                  yPos -= num <= 0 ? this._inputRowHeight : this._inputRowHeight + this._inputRowCategorySpacing;
                using (IEnumerator<InputAction> enumerator = ReInput.get_mapping().UserAssignableActionsInCategory(actionCategory.get_id(), true).GetEnumerator())
                {
                  while (((IEnumerator) enumerator).MoveNext())
                  {
                    InputAction current = enumerator.Current;
                    if (current.get_type() == null)
                    {
                      if (this._showFullAxisInputFields)
                        this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, current, (AxisRange) 0, controllerType, cols, fieldWidth, ref yPos, disableFullAxis);
                      if (this._showSplitAxisInputFields)
                      {
                        this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, current, (AxisRange) 1, controllerType, cols, fieldWidth, ref yPos, false);
                        this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, current, (AxisRange) 2, controllerType, cols, fieldWidth, ref yPos, false);
                      }
                    }
                    else if (current.get_type() == 1)
                      this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, current, (AxisRange) 1, controllerType, cols, fieldWidth, ref yPos, false);
                    ++num;
                  }
                }
              }
            }
          }
          else
          {
            IList<int> actionIds = mappingSet.actionIds;
            for (int index2 = 0; index2 < actionIds.Count; ++index2)
            {
              InputAction action = ReInput.get_mapping().GetAction(actionIds[index2]);
              if (action != null && action.get_userAssignable())
              {
                InputCategory actionCategory = ReInput.get_mapping().GetActionCategory(action.get_categoryId());
                if (actionCategory != null && actionCategory.get_userAssignable())
                {
                  if (action.get_type() == null)
                  {
                    if (this._showFullAxisInputFields)
                      this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, (AxisRange) 0, controllerType, cols, fieldWidth, ref yPos, disableFullAxis);
                    if (this._showSplitAxisInputFields)
                    {
                      this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, (AxisRange) 1, controllerType, cols, fieldWidth, ref yPos, false);
                      this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, (AxisRange) 2, controllerType, cols, fieldWidth, ref yPos, false);
                    }
                  }
                  else if (action.get_type() == 1)
                    this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, (AxisRange) 1, controllerType, cols, fieldWidth, ref yPos, false);
                }
              }
            }
          }
        }
      }
    }

    private void CreateInputFieldSet(
      Transform parent,
      int mapCategoryId,
      InputAction action,
      AxisRange axisRange,
      ControllerType controllerType,
      int cols,
      int fieldWidth,
      ref int yPos,
      bool disableFullAxis)
    {
      GameObject newGuiObject = this.CreateNewGUIObject("FieldLayoutGroup", parent, new Vector2(0.0f, (float) yPos));
      HorizontalLayoutGroup horizontalLayoutGroup = (HorizontalLayoutGroup) newGuiObject.AddComponent<HorizontalLayoutGroup>();
      RectTransform component = (RectTransform) newGuiObject.GetComponent<RectTransform>();
      component.set_anchorMin(new Vector2(0.0f, 1f));
      component.set_anchorMax(new Vector2(1f, 1f));
      component.set_pivot(new Vector2(0.0f, 1f));
      component.set_sizeDelta(Vector2.get_zero());
      component.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, (float) this._inputRowHeight);
      this.inputGrid.AddInputFieldSet(mapCategoryId, action, axisRange, controllerType, newGuiObject);
      for (int fieldIndex = 0; fieldIndex < cols; ++fieldIndex)
      {
        int num = axisRange != null ? 0 : this._invertToggleWidth;
        Rewired.UI.ControlMapper.ControlMapper.GUIInputField inputField = this.CreateInputField(((Component) horizontalLayoutGroup).get_transform(), Vector2.get_zero(), string.Empty, action.get_id(), axisRange, controllerType, fieldIndex);
        inputField.SetFirstChildObjectWidth(Rewired.UI.ControlMapper.ControlMapper.LayoutElementSizeType.PreferredSize, fieldWidth - num);
        this.inputGrid.AddInputField(mapCategoryId, action, axisRange, controllerType, fieldIndex, inputField);
        if (axisRange == null)
        {
          if (!disableFullAxis)
          {
            Rewired.UI.ControlMapper.ControlMapper.GUIToggle toggle = this.CreateToggle(this.prefabs.inputGridFieldInvertToggle, ((Component) horizontalLayoutGroup).get_transform(), Vector2.get_zero(), string.Empty, action.get_id(), axisRange, controllerType, fieldIndex);
            toggle.SetFirstChildObjectWidth(Rewired.UI.ControlMapper.ControlMapper.LayoutElementSizeType.MinSize, num);
            inputField.AddToggle(toggle);
          }
          else
            inputField.SetInteractible(false, false, true);
        }
      }
      yPos -= this._inputRowHeight;
    }

    private void PopulateInputFields()
    {
      this.inputGrid.InitializeFields(this.currentMapCategoryId);
      if (this.currentPlayer == null)
        return;
      this.inputGrid.SetFieldsActive(this.currentMapCategoryId, true);
      foreach (Rewired.UI.ControlMapper.ControlMapper.InputActionSet actionSet in this.inputGrid.GetActionSets(this.currentMapCategoryId))
      {
        if (this._showKeyboard)
        {
          ControllerType controllerType = (ControllerType) 0;
          int controllerId = 0;
          int mapDefaultLayout = this._keyboardMapDefaultLayout;
          int keyboardInputFieldCount = this._keyboardInputFieldCount;
          ControllerMap controllerMapOrCreateNew = this.GetControllerMapOrCreateNew(controllerType, controllerId, mapDefaultLayout);
          this.PopulateInputFieldGroup(actionSet, controllerMapOrCreateNew, controllerType, controllerId, keyboardInputFieldCount);
        }
        if (this._showMouse)
        {
          ControllerType controllerType = (ControllerType) 1;
          int controllerId = 0;
          int mapDefaultLayout = this._mouseMapDefaultLayout;
          int mouseInputFieldCount = this._mouseInputFieldCount;
          ControllerMap controllerMapOrCreateNew = this.GetControllerMapOrCreateNew(controllerType, controllerId, mapDefaultLayout);
          if (((Player.ControllerHelper) this.currentPlayer.controllers).get_hasMouse())
            this.PopulateInputFieldGroup(actionSet, controllerMapOrCreateNew, controllerType, controllerId, mouseInputFieldCount);
        }
        if (this.isJoystickSelected && ((Player.ControllerHelper) this.currentPlayer.controllers).get_joystickCount() > 0)
        {
          ControllerType controllerType = (ControllerType) 2;
          int id = (int) ((Controller) this.currentJoystick).id;
          int mapDefaultLayout = this._joystickMapDefaultLayout;
          int controllerInputFieldCount = this._controllerInputFieldCount;
          ControllerMap controllerMapOrCreateNew = this.GetControllerMapOrCreateNew(controllerType, id, mapDefaultLayout);
          this.PopulateInputFieldGroup(actionSet, controllerMapOrCreateNew, controllerType, id, controllerInputFieldCount);
        }
        else
          this.DisableInputFieldGroup(actionSet, (ControllerType) 2, this._controllerInputFieldCount);
      }
    }

    private void PopulateInputFieldGroup(
      Rewired.UI.ControlMapper.ControlMapper.InputActionSet actionSet,
      ControllerMap controllerMap,
      ControllerType controllerType,
      int controllerId,
      int maxFields)
    {
      if (controllerMap == null)
        return;
      int index = 0;
      this.inputGrid.SetFixedFieldData(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId);
      using (IEnumerator<ActionElementMap> enumerator = controllerMap.ElementMapsWithAction(actionSet.actionId).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          ActionElementMap current = enumerator.Current;
          if (current.get_elementType() == 1)
          {
            if (actionSet.axisRange != null)
            {
              if (actionSet.axisRange == 1)
              {
                if (current.get_axisContribution() == 1)
                  continue;
              }
              else if (actionSet.axisRange == 2 && current.get_axisContribution() == null)
                continue;
              this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, index, current.get_id(), current.get_elementIdentifierName(), false);
            }
            else
              continue;
          }
          else if (current.get_elementType() == null)
          {
            if (actionSet.axisRange == null)
            {
              if (current.get_axisRange() == null)
                this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, index, current.get_id(), current.get_elementIdentifierName(), current.get_invert());
              else
                continue;
            }
            else if (actionSet.axisRange == 1)
            {
              if ((current.get_axisRange() != null || ReInput.get_mapping().GetAction(actionSet.actionId).get_type() == 1) && current.get_axisContribution() != 1)
                this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, index, current.get_id(), current.get_elementIdentifierName(), false);
              else
                continue;
            }
            else if (actionSet.axisRange == 2)
            {
              if (current.get_axisRange() != null && current.get_axisContribution() != null)
                this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, index, current.get_id(), current.get_elementIdentifierName(), false);
              else
                continue;
            }
          }
          ++index;
          if (index > maxFields)
            break;
        }
      }
    }

    private void DisableInputFieldGroup(
      Rewired.UI.ControlMapper.ControlMapper.InputActionSet actionSet,
      ControllerType controllerType,
      int fieldCount)
    {
      for (int fieldIndex = 0; fieldIndex < fieldCount; ++fieldIndex)
        this.inputGrid.GetGUIInputField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, fieldIndex)?.SetInteractible(false, false);
    }

    private void ResetInputGridScrollBar()
    {
      ((RectTransform) ((Component) this.references.inputGridInnerGroup).GetComponent<RectTransform>()).set_anchoredPosition(Vector2.get_zero());
      this.references.inputGridVScrollbar.set_value(1f);
      this.references.inputGridScrollRect.set_verticalScrollbarVisibility((ScrollRect.ScrollbarVisibility) 1);
    }

    private void CreateLayout()
    {
      ((Component) this.references.playersGroup).get_gameObject().SetActive(this.showPlayers);
      ((Component) this.references.controllerGroup).get_gameObject().SetActive(this._showControllers);
      ((Component) this.references.assignedControllersGroup).get_gameObject().SetActive(this._showControllers && this.ShowAssignedControllers());
      ((Component) this.references.settingsAndMapCategoriesGroup).get_gameObject().SetActive(this.showSettings || this.showMapCategories);
      ((Component) this.references.settingsGroup).get_gameObject().SetActive(this.showSettings);
      ((Component) this.references.mapCategoriesGroup).get_gameObject().SetActive(this.showMapCategories);
    }

    private void Draw()
    {
      this.DrawPlayersGroup();
      this.DrawControllersGroup();
      this.DrawSettingsGroup();
      this.DrawMapCategoriesGroup();
      this.DrawWindowButtonsGroup();
    }

    private void DrawPlayersGroup()
    {
      if (!this.showPlayers)
        return;
      this.references.playersGroup.labelText = this._language.playersGroupLabel;
      this.references.playersGroup.SetLabelActive(this._showPlayersGroupLabel);
      for (int index = 0; index < this.playerCount; ++index)
      {
        Player player = ReInput.get_players().GetPlayer(index);
        if (player != null)
        {
          Rewired.UI.ControlMapper.ControlMapper.GUIButton guiButton = new Rewired.UI.ControlMapper.ControlMapper.GUIButton(UITools.InstantiateGUIObject<ButtonInfo>(this.prefabs.button, this.references.playersGroup.content, "Player" + (object) index + "Button"));
          guiButton.SetLabel(player.get_descriptiveName());
          guiButton.SetButtonInfoData("PlayerSelection", player.get_id());
          guiButton.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
          guiButton.buttonInfo.OnSelectedEvent += new Action<GameObject>(this.OnUIElementSelected);
          this.playerButtons.Add(guiButton);
        }
      }
    }

    private void DrawControllersGroup()
    {
      if (!this._showControllers)
        return;
      this.references.controllerSettingsGroup.labelText = this._language.controllerSettingsGroupLabel;
      this.references.controllerSettingsGroup.SetLabelActive(this._showControllerGroupLabel);
      ((Component) this.references.controllerNameLabel).get_gameObject().SetActive(this._showControllerNameLabel);
      ((Component) this.references.controllerGroupLabelGroup).get_gameObject().SetActive(this._showControllerGroupLabel || this._showControllerNameLabel);
      if (this.ShowAssignedControllers())
      {
        this.references.assignedControllersGroup.labelText = this._language.assignedControllersGroupLabel;
        this.references.assignedControllersGroup.SetLabelActive(this._showAssignedControllersGroupLabel);
      }
      ((UIElementInfo) ((Component) this.references.removeControllerButton).GetComponent<ButtonInfo>()).text.set_text(this._language.removeControllerButtonLabel);
      ((UIElementInfo) ((Component) this.references.calibrateControllerButton).GetComponent<ButtonInfo>()).text.set_text(this._language.calibrateControllerButtonLabel);
      ((UIElementInfo) ((Component) this.references.assignControllerButton).GetComponent<ButtonInfo>()).text.set_text(this._language.assignControllerButtonLabel);
      Rewired.UI.ControlMapper.ControlMapper.GUIButton button = this.CreateButton(this._language.none, this.references.assignedControllersGroup.content, Vector2.get_zero());
      button.SetInteractible(false, false, true);
      this.assignedControllerButtonsPlaceholder = button;
    }

    private void DrawSettingsGroup()
    {
      if (!this.showSettings)
        return;
      this.references.settingsGroup.labelText = this._language.settingsGroupLabel;
      this.references.settingsGroup.SetLabelActive(this._showSettingsGroupLabel);
      Rewired.UI.ControlMapper.ControlMapper.GUIButton button = this.CreateButton(this._language.inputBehaviorSettingsButtonLabel, this.references.settingsGroup.content, Vector2.get_zero());
      this.miscInstantiatedObjects.Add(button.gameObject);
      button.buttonInfo.OnSelectedEvent += new Action<GameObject>(this.OnUIElementSelected);
      button.SetButtonInfoData("EditInputBehaviors", 0);
      button.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
    }

    private void DrawMapCategoriesGroup()
    {
      if (!this.showMapCategories || this._mappingSets == null)
        return;
      this.references.mapCategoriesGroup.labelText = this._language.mapCategoriesGroupLabel;
      this.references.mapCategoriesGroup.SetLabelActive(this._showMapCategoriesGroupLabel);
      for (int index = 0; index < this._mappingSets.Length; ++index)
      {
        Rewired.UI.ControlMapper.ControlMapper.MappingSet mappingSet = this._mappingSets[index];
        if (mappingSet != null)
        {
          InputMapCategory mapCategory = ReInput.get_mapping().GetMapCategory(mappingSet.mapCategoryId);
          if (mapCategory != null)
          {
            Rewired.UI.ControlMapper.ControlMapper.GUIButton guiButton = new Rewired.UI.ControlMapper.ControlMapper.GUIButton(UITools.InstantiateGUIObject<ButtonInfo>(this.prefabs.button, this.references.mapCategoriesGroup.content, ((InputCategory) mapCategory).get_name() + "Button"));
            guiButton.SetLabel(((InputCategory) mapCategory).get_descriptiveName());
            guiButton.SetButtonInfoData("MapCategorySelection", ((InputCategory) mapCategory).get_id());
            guiButton.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
            guiButton.buttonInfo.OnSelectedEvent += new Action<GameObject>(this.OnUIElementSelected);
            this.mapCategoryButtons.Add(guiButton);
          }
        }
      }
    }

    private void DrawWindowButtonsGroup()
    {
      ((UIElementInfo) ((Component) this.references.doneButton).GetComponent<ButtonInfo>()).text.set_text(this._language.doneButtonLabel);
      ((UIElementInfo) ((Component) this.references.restoreDefaultsButton).GetComponent<ButtonInfo>()).text.set_text(this._language.restoreDefaultsButtonLabel);
    }

    private void Redraw(bool listsChanged, bool playTransitions)
    {
      this.RedrawPlayerGroup(playTransitions);
      this.RedrawControllerGroup();
      this.RedrawMapCategoriesGroup(playTransitions);
      this.RedrawInputGrid(listsChanged);
      if (!Object.op_Equality((Object) this.currentUISelection, (Object) null) && this.currentUISelection.get_activeInHierarchy())
        return;
      this.RestoreLastUISelection();
    }

    private void RedrawPlayerGroup(bool playTransitions)
    {
      if (!this.showPlayers)
        return;
      for (int index = 0; index < this.playerButtons.Count; ++index)
      {
        bool state = this.currentPlayerId != this.playerButtons[index].buttonInfo.intData;
        this.playerButtons[index].SetInteractible(state, playTransitions);
      }
    }

    private void RedrawControllerGroup()
    {
      int num = -1;
      this.references.controllerNameLabel.set_text(this._language.none);
      UITools.SetInteractable((Selectable) this.references.removeControllerButton, false, false);
      UITools.SetInteractable((Selectable) this.references.assignControllerButton, false, false);
      UITools.SetInteractable((Selectable) this.references.calibrateControllerButton, false, false);
      if (this.ShowAssignedControllers())
      {
        foreach (Rewired.UI.ControlMapper.ControlMapper.GUIButton controllerButton in this.assignedControllerButtons)
        {
          if (!Object.op_Equality((Object) controllerButton.gameObject, (Object) null))
          {
            if (Object.op_Equality((Object) this.currentUISelection, (Object) controllerButton.gameObject))
              num = controllerButton.buttonInfo.intData;
            Object.Destroy((Object) controllerButton.gameObject);
          }
        }
        this.assignedControllerButtons.Clear();
        this.assignedControllerButtonsPlaceholder.SetActive(true);
      }
      Player player = ReInput.get_players().GetPlayer(this.currentPlayerId);
      if (player == null)
        return;
      if (this.ShowAssignedControllers())
      {
        if (((Player.ControllerHelper) player.controllers).get_joystickCount() > 0)
          this.assignedControllerButtonsPlaceholder.SetActive(false);
        using (IEnumerator<Joystick> enumerator = ((IEnumerable<Joystick>) ((Player.ControllerHelper) player.controllers).get_Joysticks()).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
          {
            Joystick current = enumerator.Current;
            Rewired.UI.ControlMapper.ControlMapper.GUIButton button = this.CreateButton(((Controller) current).get_name(), this.references.assignedControllersGroup.content, Vector2.get_zero());
            button.SetButtonInfoData("AssignedControllerSelection", (int) ((Controller) current).id);
            button.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
            button.buttonInfo.OnSelectedEvent += new Action<GameObject>(this.OnUIElementSelected);
            this.assignedControllerButtons.Add(button);
            if (((Controller) current).id == this.currentJoystickId)
              button.SetInteractible(false, true);
          }
        }
        if (((Player.ControllerHelper) player.controllers).get_joystickCount() > 0 && !this.isJoystickSelected)
        {
          this.currentJoystickId = (int) ((Controller) ((Player.ControllerHelper) player.controllers).get_Joysticks()[0]).id;
          this.assignedControllerButtons[0].SetInteractible(false, false);
        }
        if (num >= 0)
        {
          foreach (Rewired.UI.ControlMapper.ControlMapper.GUIButton controllerButton in this.assignedControllerButtons)
          {
            if (controllerButton.buttonInfo.intData == num)
            {
              this.SetUISelection(controllerButton.gameObject);
              break;
            }
          }
        }
      }
      else if (((Player.ControllerHelper) player.controllers).get_joystickCount() > 0 && !this.isJoystickSelected)
        this.currentJoystickId = (int) ((Controller) ((Player.ControllerHelper) player.controllers).get_Joysticks()[0]).id;
      if (this.isJoystickSelected && ((Player.ControllerHelper) player.controllers).get_joystickCount() > 0)
      {
        ((Selectable) this.references.removeControllerButton).set_interactable(true);
        this.references.controllerNameLabel.set_text(((Controller) this.currentJoystick).get_name());
        if (((ControllerWithAxes) this.currentJoystick).get_axisCount() > 0)
          ((Selectable) this.references.calibrateControllerButton).set_interactable(true);
      }
      int joystickCount1 = ((Player.ControllerHelper) player.controllers).get_joystickCount();
      int joystickCount2 = ReInput.get_controllers().get_joystickCount();
      int controllersPerPlayer = this.GetMaxControllersPerPlayer();
      bool flag = controllersPerPlayer == 0;
      if (joystickCount2 <= 0 || joystickCount1 >= joystickCount2 || controllersPerPlayer != 1 && !flag && joystickCount1 >= controllersPerPlayer)
        return;
      UITools.SetInteractable((Selectable) this.references.assignControllerButton, true, false);
    }

    private void RedrawMapCategoriesGroup(bool playTransitions)
    {
      if (!this.showMapCategories)
        return;
      for (int index = 0; index < this.mapCategoryButtons.Count; ++index)
      {
        bool state = this.currentMapCategoryId != this.mapCategoryButtons[index].buttonInfo.intData;
        this.mapCategoryButtons[index].SetInteractible(state, playTransitions);
      }
    }

    private void RedrawInputGrid(bool listsChanged)
    {
      if (listsChanged)
        this.RefreshInputGridStructure();
      this.PopulateInputFields();
      if (!listsChanged)
        return;
      this.ResetInputGridScrollBar();
    }

    private void ForceRefresh()
    {
      if (this.windowManager.isWindowOpen)
        this.CloseAllWindows();
      else
        this.Redraw(false, false);
    }

    private void CreateInputCategoryRow(ref int rowCount, InputCategory category)
    {
      this.CreateLabel(category.get_descriptiveName(), this.references.inputGridActionColumn, new Vector2(0.0f, (float) (rowCount * this._inputRowHeight) * -1f));
      ++rowCount;
    }

    private Rewired.UI.ControlMapper.ControlMapper.GUILabel CreateLabel(
      string labelText,
      Transform parent,
      Vector2 offset)
    {
      return this.CreateLabel(this.prefabs.inputGridLabel, labelText, parent, offset);
    }

    private Rewired.UI.ControlMapper.ControlMapper.GUILabel CreateLabel(
      GameObject prefab,
      string labelText,
      Transform parent,
      Vector2 offset)
    {
      GameObject gameObject = this.InstantiateGUIObject(prefab, parent, offset);
      Text inSelfOrChildren = (Text) UnityTools.GetComponentInSelfOrChildren<Text>(gameObject);
      if (Object.op_Equality((Object) inSelfOrChildren, (Object) null))
      {
        Debug.LogError((object) "Rewired Control Mapper: Label prefab is missing Text component!");
        return (Rewired.UI.ControlMapper.ControlMapper.GUILabel) null;
      }
      inSelfOrChildren.set_text(labelText);
      return new Rewired.UI.ControlMapper.ControlMapper.GUILabel(gameObject);
    }

    private Rewired.UI.ControlMapper.ControlMapper.GUIButton CreateButton(
      string labelText,
      Transform parent,
      Vector2 offset)
    {
      Rewired.UI.ControlMapper.ControlMapper.GUIButton guiButton = new Rewired.UI.ControlMapper.ControlMapper.GUIButton(this.InstantiateGUIObject(this.prefabs.button, parent, offset));
      guiButton.SetLabel(labelText);
      return guiButton;
    }

    private Rewired.UI.ControlMapper.ControlMapper.GUIButton CreateFitButton(
      string labelText,
      Transform parent,
      Vector2 offset)
    {
      Rewired.UI.ControlMapper.ControlMapper.GUIButton guiButton = new Rewired.UI.ControlMapper.ControlMapper.GUIButton(this.InstantiateGUIObject(this.prefabs.fitButton, parent, offset));
      guiButton.SetLabel(labelText);
      return guiButton;
    }

    private Rewired.UI.ControlMapper.ControlMapper.GUIInputField CreateInputField(
      Transform parent,
      Vector2 offset,
      string label,
      int actionId,
      AxisRange axisRange,
      ControllerType controllerType,
      int fieldIndex)
    {
      Rewired.UI.ControlMapper.ControlMapper.GUIInputField inputField = this.CreateInputField(parent, offset);
      inputField.SetLabel(string.Empty);
      inputField.SetFieldInfoData(actionId, axisRange, controllerType, fieldIndex);
      inputField.SetOnClickCallback(this.inputFieldActivatedDelegate);
      inputField.fieldInfo.OnSelectedEvent += new Action<GameObject>(this.OnUIElementSelected);
      return inputField;
    }

    private Rewired.UI.ControlMapper.ControlMapper.GUIInputField CreateInputField(
      Transform parent,
      Vector2 offset)
    {
      return new Rewired.UI.ControlMapper.ControlMapper.GUIInputField(this.InstantiateGUIObject(this.prefabs.inputGridFieldButton, parent, offset));
    }

    private Rewired.UI.ControlMapper.ControlMapper.GUIToggle CreateToggle(
      GameObject prefab,
      Transform parent,
      Vector2 offset,
      string label,
      int actionId,
      AxisRange axisRange,
      ControllerType controllerType,
      int fieldIndex)
    {
      Rewired.UI.ControlMapper.ControlMapper.GUIToggle toggle = this.CreateToggle(prefab, parent, offset);
      toggle.SetToggleInfoData(actionId, axisRange, controllerType, fieldIndex);
      toggle.SetOnSubmitCallback(this.inputFieldInvertToggleStateChangedDelegate);
      toggle.toggleInfo.OnSelectedEvent += new Action<GameObject>(this.OnUIElementSelected);
      return toggle;
    }

    private Rewired.UI.ControlMapper.ControlMapper.GUIToggle CreateToggle(
      GameObject prefab,
      Transform parent,
      Vector2 offset)
    {
      return new Rewired.UI.ControlMapper.ControlMapper.GUIToggle(this.InstantiateGUIObject(prefab, parent, offset));
    }

    private GameObject InstantiateGUIObject(
      GameObject prefab,
      Transform parent,
      Vector2 offset)
    {
      if (!Object.op_Equality((Object) prefab, (Object) null))
        return this.InitializeNewGUIGameObject((GameObject) Object.Instantiate<GameObject>((M0) prefab), parent, offset);
      Debug.LogError((object) "Rewired Control Mapper: Prefab is null!");
      return (GameObject) null;
    }

    private GameObject CreateNewGUIObject(string name, Transform parent, Vector2 offset)
    {
      GameObject gameObject = new GameObject();
      ((Object) gameObject).set_name(name);
      gameObject.AddComponent<RectTransform>();
      return this.InitializeNewGUIGameObject(gameObject, parent, offset);
    }

    private GameObject InitializeNewGUIGameObject(
      GameObject gameObject,
      Transform parent,
      Vector2 offset)
    {
      if (Object.op_Equality((Object) gameObject, (Object) null))
      {
        Debug.LogError((object) "Rewired Control Mapper: GameObject is null!");
        return (GameObject) null;
      }
      RectTransform component = (RectTransform) gameObject.GetComponent<RectTransform>();
      if (Object.op_Equality((Object) component, (Object) null))
      {
        Debug.LogError((object) "Rewired Control Mapper: GameObject does not have a RectTransform component!");
        return gameObject;
      }
      if (Object.op_Inequality((Object) parent, (Object) null))
        ((Transform) component).SetParent(parent, false);
      component.set_anchoredPosition(offset);
      return gameObject;
    }

    private GameObject CreateNewColumnGroup(string name, Transform parent, int maxWidth)
    {
      GameObject newGuiObject = this.CreateNewGUIObject(name, parent, Vector2.get_zero());
      this.inputGrid.AddGroup(newGuiObject);
      LayoutElement layoutElement = (LayoutElement) newGuiObject.AddComponent<LayoutElement>();
      if (maxWidth >= 0)
        layoutElement.set_preferredWidth((float) maxWidth);
      RectTransform component = (RectTransform) newGuiObject.GetComponent<RectTransform>();
      component.set_anchorMin(new Vector2(0.0f, 0.0f));
      component.set_anchorMax(new Vector2(1f, 0.0f));
      return newGuiObject;
    }

    private Window OpenWindow(bool closeOthers)
    {
      return this.OpenWindow(string.Empty, closeOthers);
    }

    private Window OpenWindow(string name, bool closeOthers)
    {
      if (closeOthers)
        this.windowManager.CancelAll();
      Window window = this.windowManager.OpenWindow(name, this._defaultWindowWidth, this._defaultWindowHeight);
      if (Object.op_Equality((Object) window, (Object) null))
        return (Window) null;
      this.ChildWindowOpened();
      return window;
    }

    private Window OpenWindow(GameObject windowPrefab, bool closeOthers)
    {
      return this.OpenWindow(windowPrefab, string.Empty, closeOthers);
    }

    private Window OpenWindow(GameObject windowPrefab, string name, bool closeOthers)
    {
      if (closeOthers)
        this.windowManager.CancelAll();
      Window window = this.windowManager.OpenWindow(windowPrefab, name);
      if (Object.op_Equality((Object) window, (Object) null))
        return (Window) null;
      this.ChildWindowOpened();
      return window;
    }

    private void OpenModal(
      string title,
      string message,
      string confirmText,
      Action<int> confirmAction,
      string cancelText,
      Action<int> cancelAction,
      bool closeOthers)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Rewired.UI.ControlMapper.ControlMapper.\u003COpenModal\u003Ec__AnonStorey5 modalCAnonStorey5 = new Rewired.UI.ControlMapper.ControlMapper.\u003COpenModal\u003Ec__AnonStorey5();
      // ISSUE: reference to a compiler-generated field
      modalCAnonStorey5.\u0024this = this;
      // ISSUE: reference to a compiler-generated field
      modalCAnonStorey5.window = this.OpenWindow(closeOthers);
      // ISSUE: reference to a compiler-generated field
      if (Object.op_Equality((Object) modalCAnonStorey5.window, (Object) null))
        return;
      // ISSUE: reference to a compiler-generated field
      modalCAnonStorey5.window.CreateTitleText(this.prefabs.windowTitleText, Vector2.get_zero(), title);
      // ISSUE: reference to a compiler-generated field
      modalCAnonStorey5.window.AddContentText(this.prefabs.windowContentText, UIPivot.get_TopCenter(), UIAnchor.get_TopHStretch(), new Vector2(0.0f, -100f), message);
      // ISSUE: method pointer
      UnityAction unityAction = new UnityAction((object) modalCAnonStorey5, __methodptr(\u003C\u003Em__0));
      // ISSUE: reference to a compiler-generated field
      modalCAnonStorey5.window.cancelCallback = unityAction;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      modalCAnonStorey5.window.CreateButton(this.prefabs.fitButton, UIPivot.get_BottomLeft(), UIAnchor.get_BottomLeft(), Vector2.get_zero(), confirmText, new UnityAction((object) modalCAnonStorey5, __methodptr(\u003C\u003Em__1)), unityAction, false);
      // ISSUE: reference to a compiler-generated field
      modalCAnonStorey5.window.CreateButton(this.prefabs.fitButton, UIPivot.get_BottomRight(), UIAnchor.get_BottomRight(), Vector2.get_zero(), cancelText, unityAction, unityAction, true);
      // ISSUE: reference to a compiler-generated field
      this.windowManager.Focus(modalCAnonStorey5.window);
    }

    private void CloseWindow(int windowId)
    {
      if (!this.windowManager.isWindowOpen)
        return;
      this.windowManager.CloseWindow(windowId);
      this.ChildWindowClosed();
    }

    private void CloseTopWindow()
    {
      if (!this.windowManager.isWindowOpen)
        return;
      this.windowManager.CloseTop();
      this.ChildWindowClosed();
    }

    private void CloseAllWindows()
    {
      if (!this.windowManager.isWindowOpen)
        return;
      this.windowManager.CancelAll();
      this.ChildWindowClosed();
      this.InputPollingStopped();
    }

    private void ChildWindowOpened()
    {
      if (!this.windowManager.isWindowOpen)
        return;
      this.SetIsFocused(false);
      if (this._PopupWindowOpenedEvent != null)
        this._PopupWindowOpenedEvent();
      if (this._onPopupWindowOpened == null)
        return;
      this._onPopupWindowOpened.Invoke();
    }

    private void ChildWindowClosed()
    {
      if (this.windowManager.isWindowOpen)
        return;
      this.SetIsFocused(true);
      if (this._PopupWindowClosedEvent != null)
        this._PopupWindowClosedEvent();
      if (this._onPopupWindowClosed == null)
        return;
      this._onPopupWindowClosed.Invoke();
    }

    private bool HasElementAssignmentConflicts(
      Player player,
      Rewired.UI.ControlMapper.ControlMapper.InputMapping mapping,
      ElementAssignment assignment,
      bool skipOtherPlayers)
    {
      ElementAssignmentConflictCheck conflictCheck;
      if (player == null || mapping == null || !this.CreateConflictCheck(mapping, assignment, out conflictCheck))
        return false;
      if (!skipOtherPlayers)
        return ((ReInput.ControllerHelper.ConflictCheckingHelper) ReInput.get_controllers().conflictChecking).DoesElementAssignmentConflict(conflictCheck);
      return ((Player.ControllerHelper.ConflictCheckingHelper) ((Player.ControllerHelper) ReInput.get_players().get_SystemPlayer().controllers).conflictChecking).DoesElementAssignmentConflict(conflictCheck) || ((Player.ControllerHelper.ConflictCheckingHelper) ((Player.ControllerHelper) player.controllers).conflictChecking).DoesElementAssignmentConflict(conflictCheck);
    }

    private bool IsBlockingAssignmentConflict(
      Rewired.UI.ControlMapper.ControlMapper.InputMapping mapping,
      ElementAssignment assignment,
      bool skipOtherPlayers)
    {
      ElementAssignmentConflictCheck conflictCheck;
      if (!this.CreateConflictCheck(mapping, assignment, out conflictCheck))
        return false;
      if (skipOtherPlayers)
      {
        using (IEnumerator<ElementAssignmentConflictInfo> enumerator = ((Player.ControllerHelper.ConflictCheckingHelper) ((Player.ControllerHelper) ReInput.get_players().get_SystemPlayer().controllers).conflictChecking).ElementAssignmentConflicts(conflictCheck).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
          {
            ElementAssignmentConflictInfo current = enumerator.Current;
            if (!((ElementAssignmentConflictInfo) ref current).get_isUserAssignable())
              return true;
          }
        }
        using (IEnumerator<ElementAssignmentConflictInfo> enumerator = ((Player.ControllerHelper.ConflictCheckingHelper) ((Player.ControllerHelper) this.currentPlayer.controllers).conflictChecking).ElementAssignmentConflicts(conflictCheck).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
          {
            ElementAssignmentConflictInfo current = enumerator.Current;
            if (!((ElementAssignmentConflictInfo) ref current).get_isUserAssignable())
              return true;
          }
        }
      }
      else
      {
        using (IEnumerator<ElementAssignmentConflictInfo> enumerator = ((ReInput.ControllerHelper.ConflictCheckingHelper) ReInput.get_controllers().conflictChecking).ElementAssignmentConflicts(conflictCheck).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
          {
            ElementAssignmentConflictInfo current = enumerator.Current;
            if (!((ElementAssignmentConflictInfo) ref current).get_isUserAssignable())
              return true;
          }
        }
      }
      return false;
    }

    [DebuggerHidden]
    private IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(
      Player player,
      Rewired.UI.ControlMapper.ControlMapper.InputMapping mapping,
      ElementAssignment assignment,
      bool skipOtherPlayers)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Rewired.UI.ControlMapper.ControlMapper.\u003CElementAssignmentConflicts\u003Ec__Iterator0 conflictsCIterator0 = new Rewired.UI.ControlMapper.ControlMapper.\u003CElementAssignmentConflicts\u003Ec__Iterator0()
      {
        player = player,
        mapping = mapping,
        assignment = assignment,
        skipOtherPlayers = skipOtherPlayers,
        \u0024this = this
      };
      // ISSUE: reference to a compiler-generated field
      conflictsCIterator0.\u0024PC = -2;
      return (IEnumerable<ElementAssignmentConflictInfo>) conflictsCIterator0;
    }

    private bool CreateConflictCheck(
      Rewired.UI.ControlMapper.ControlMapper.InputMapping mapping,
      ElementAssignment assignment,
      out ElementAssignmentConflictCheck conflictCheck)
    {
      if (mapping == null || this.currentPlayer == null)
      {
        conflictCheck = (ElementAssignmentConflictCheck) null;
        return false;
      }
      conflictCheck = ((ElementAssignment) ref assignment).ToElementAssignmentConflictCheck();
      ((ElementAssignmentConflictCheck) ref conflictCheck).set_playerId(this.currentPlayer.get_id());
      ((ElementAssignmentConflictCheck) ref conflictCheck).set_controllerType(mapping.controllerType);
      ((ElementAssignmentConflictCheck) ref conflictCheck).set_controllerId(mapping.controllerId);
      ((ElementAssignmentConflictCheck) ref conflictCheck).set_controllerMapId(mapping.map.get_id());
      ((ElementAssignmentConflictCheck) ref conflictCheck).set_controllerMapCategoryId(mapping.map.get_categoryId());
      if (mapping.aem != null)
        ((ElementAssignmentConflictCheck) ref conflictCheck).set_elementMapId(mapping.aem.get_id());
      return true;
    }

    private void PollKeyboardForAssignment(
      out ControllerPollingInfo pollingInfo,
      out bool modifierKeyPressed,
      out ModifierKeyFlags modifierFlags,
      out string label)
    {
      pollingInfo = (ControllerPollingInfo) null;
      label = string.Empty;
      modifierKeyPressed = false;
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      ^(int&) ref modifierFlags = 0;
      int num = 0;
      ControllerPollingInfo controllerPollingInfo1 = (ControllerPollingInfo) null;
      ControllerPollingInfo controllerPollingInfo2 = (ControllerPollingInfo) null;
      ModifierKeyFlags modifierKeyFlags = (ModifierKeyFlags) 0;
      using (IEnumerator<ControllerPollingInfo> enumerator = ReInput.get_controllers().get_Keyboard().PollForAllKeys().GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          ControllerPollingInfo current = enumerator.Current;
          KeyCode keyboardKey = ((ControllerPollingInfo) ref current).get_keyboardKey();
          if (keyboardKey != 313)
          {
            if (Keyboard.IsModifierKey(((ControllerPollingInfo) ref current).get_keyboardKey()))
            {
              if (num == 0)
                controllerPollingInfo2 = current;
              modifierKeyFlags = modifierKeyFlags | Keyboard.KeyCodeToModifierKeyFlags(keyboardKey);
              ++num;
            }
            else if (((ControllerPollingInfo) ref controllerPollingInfo1).get_keyboardKey() == null)
              controllerPollingInfo1 = current;
          }
        }
      }
      if (((ControllerPollingInfo) ref controllerPollingInfo1).get_keyboardKey() != null)
      {
        if (!ReInput.get_controllers().get_Keyboard().GetKeyDown(((ControllerPollingInfo) ref controllerPollingInfo1).get_keyboardKey()))
          return;
        if (num == 0)
        {
          pollingInfo = controllerPollingInfo1;
        }
        else
        {
          pollingInfo = controllerPollingInfo1;
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          ^(int&) ref modifierFlags = (int) modifierKeyFlags;
        }
      }
      else
      {
        if (num <= 0)
          return;
        modifierKeyPressed = true;
        if (num == 1)
        {
          if ((double) ReInput.get_controllers().get_Keyboard().GetKeyTimePressed(((ControllerPollingInfo) ref controllerPollingInfo2).get_keyboardKey()) > 1.0)
            pollingInfo = controllerPollingInfo2;
          else
            label = Keyboard.GetKeyName(((ControllerPollingInfo) ref controllerPollingInfo2).get_keyboardKey());
        }
        else
          label = Keyboard.ModifierKeyFlagsToString(modifierKeyFlags);
      }
    }

    private bool GetFirstElementAssignmentConflict(
      ElementAssignmentConflictCheck conflictCheck,
      out ElementAssignmentConflictInfo conflict,
      bool skipOtherPlayers)
    {
      if (this.GetFirstElementAssignmentConflict(this.currentPlayer, conflictCheck, out conflict) || this.GetFirstElementAssignmentConflict(ReInput.get_players().get_SystemPlayer(), conflictCheck, out conflict))
        return true;
      if (!skipOtherPlayers)
      {
        IList<Player> players = ReInput.get_players().get_Players();
        for (int index = 0; index < ((ICollection<Player>) players).Count; ++index)
        {
          Player player = players[index];
          if (player != this.currentPlayer && this.GetFirstElementAssignmentConflict(player, conflictCheck, out conflict))
            return true;
        }
      }
      return false;
    }

    private bool GetFirstElementAssignmentConflict(
      Player player,
      ElementAssignmentConflictCheck conflictCheck,
      out ElementAssignmentConflictInfo conflict)
    {
      using (IEnumerator<ElementAssignmentConflictInfo> enumerator = ((Player.ControllerHelper.ConflictCheckingHelper) ((Player.ControllerHelper) player.controllers).conflictChecking).ElementAssignmentConflicts(conflictCheck).GetEnumerator())
      {
        if (((IEnumerator) enumerator).MoveNext())
        {
          ElementAssignmentConflictInfo current = enumerator.Current;
          conflict = current;
          return true;
        }
      }
      conflict = (ElementAssignmentConflictInfo) null;
      return false;
    }

    private void StartAxisCalibration(int axisIndex)
    {
      if (this.currentPlayer == null || ((Player.ControllerHelper) this.currentPlayer.controllers).get_joystickCount() == 0)
        return;
      Joystick currentJoystick = this.currentJoystick;
      if (axisIndex < 0 || axisIndex >= ((ControllerWithAxes) currentJoystick).get_axisCount())
        return;
      this.pendingAxisCalibration = new Rewired.UI.ControlMapper.ControlMapper.AxisCalibrator(currentJoystick, axisIndex);
      this.ShowCalibrateAxisStep1Window();
    }

    private void EndAxisCalibration()
    {
      if (this.pendingAxisCalibration == null)
        return;
      this.pendingAxisCalibration.Commit();
      this.pendingAxisCalibration = (Rewired.UI.ControlMapper.ControlMapper.AxisCalibrator) null;
    }

    private void SetUISelection(GameObject selection)
    {
      if (Object.op_Equality((Object) EventSystem.get_current(), (Object) null))
        return;
      EventSystem.get_current().SetSelectedGameObject(selection);
    }

    private void RestoreLastUISelection()
    {
      if (Object.op_Equality((Object) this.lastUISelection, (Object) null) || !this.lastUISelection.get_activeInHierarchy())
        this.SetDefaultUISelection();
      else
        this.SetUISelection(this.lastUISelection);
    }

    private void SetDefaultUISelection()
    {
      if (!this.isOpen)
        return;
      if (Object.op_Equality((Object) this.references.defaultSelection, (Object) null))
        this.SetUISelection((GameObject) null);
      else
        this.SetUISelection(((Component) this.references.defaultSelection).get_gameObject());
    }

    private void SelectDefaultMapCategory(bool redraw)
    {
      this.currentMapCategoryId = this.GetDefaultMapCategoryId();
      this.OnMapCategorySelected(this.currentMapCategoryId, redraw);
      if (!this.showMapCategories)
        return;
      for (int index = 0; index < this._mappingSets.Length; ++index)
      {
        if (ReInput.get_mapping().GetMapCategory(this._mappingSets[index].mapCategoryId) != null)
        {
          this.currentMapCategoryId = this._mappingSets[index].mapCategoryId;
          break;
        }
      }
      if (this.currentMapCategoryId < 0)
        return;
      for (int index = 0; index < this._mappingSets.Length; ++index)
      {
        bool state = this._mappingSets[index].mapCategoryId != this.currentMapCategoryId;
        this.mapCategoryButtons[index].SetInteractible(state, false);
      }
    }

    private void CheckUISelection()
    {
      if (!this.isFocused || !Object.op_Equality((Object) this.currentUISelection, (Object) null))
        return;
      this.RestoreLastUISelection();
    }

    private void OnUIElementSelected(GameObject selectedObject)
    {
      this.lastUISelection = selectedObject;
    }

    private void SetIsFocused(bool state)
    {
      this.references.mainCanvasGroup.set_interactable(state);
      if (!state)
        return;
      this.Redraw(false, false);
      this.RestoreLastUISelection();
      this.blockInputOnFocusEndTime = Time.get_unscaledTime() + 0.1f;
    }

    public void Toggle()
    {
      if (this.isOpen)
        this.Close(true);
      else
        this.Open();
    }

    public void Open()
    {
      this.Open(false);
    }

    private void Open(bool force)
    {
      if (!this.initialized)
        this.Initialize();
      if (!this.initialized || !force && this.isOpen)
        return;
      this.Clear();
      this.canvas.SetActive(true);
      this.OnPlayerSelected(0, false);
      this.SelectDefaultMapCategory(false);
      this.SetDefaultUISelection();
      this.Redraw(true, false);
      if (this._ScreenOpenedEvent != null)
        this._ScreenOpenedEvent();
      if (this._onScreenOpened == null)
        return;
      this._onScreenOpened.Invoke();
    }

    public void Close(bool save)
    {
      if (!this.initialized || !this.isOpen)
        return;
      if (save && ReInput.get_userDataStore() != null)
        ReInput.get_userDataStore().Save();
      this.Clear();
      this.canvas.SetActive(false);
      this.SetUISelection((GameObject) null);
      if (this._ScreenClosedEvent != null)
        this._ScreenClosedEvent();
      if (this._onScreenClosed == null)
        return;
      this._onScreenClosed.Invoke();
    }

    private void Clear()
    {
      this.windowManager.CancelAll();
      this.lastUISelection = (GameObject) null;
      this.pendingInputMapping = (Rewired.UI.ControlMapper.ControlMapper.InputMapping) null;
      this.pendingAxisCalibration = (Rewired.UI.ControlMapper.ControlMapper.AxisCalibrator) null;
      this.InputPollingStopped();
    }

    private void ClearCompletely()
    {
      this.ClearSpawnedObjects();
      this.ClearAllVars();
    }

    private void ClearSpawnedObjects()
    {
      this.windowManager.ClearCompletely();
      this.inputGrid.ClearAll();
      foreach (Rewired.UI.ControlMapper.ControlMapper.GUIElement playerButton in this.playerButtons)
        Object.Destroy((Object) playerButton.gameObject);
      this.playerButtons.Clear();
      foreach (Rewired.UI.ControlMapper.ControlMapper.GUIElement mapCategoryButton in this.mapCategoryButtons)
        Object.Destroy((Object) mapCategoryButton.gameObject);
      this.mapCategoryButtons.Clear();
      foreach (Rewired.UI.ControlMapper.ControlMapper.GUIElement controllerButton in this.assignedControllerButtons)
        Object.Destroy((Object) controllerButton.gameObject);
      this.assignedControllerButtons.Clear();
      if (this.assignedControllerButtonsPlaceholder != null)
      {
        Object.Destroy((Object) this.assignedControllerButtonsPlaceholder.gameObject);
        this.assignedControllerButtonsPlaceholder = (Rewired.UI.ControlMapper.ControlMapper.GUIButton) null;
      }
      using (List<GameObject>.Enumerator enumerator = this.miscInstantiatedObjects.GetEnumerator())
      {
        while (enumerator.MoveNext())
          Object.Destroy((Object) enumerator.Current);
      }
      this.miscInstantiatedObjects.Clear();
    }

    private void ClearVarsOnPlayerChange()
    {
      this.currentJoystickId = -1;
    }

    private void ClearVarsOnJoystickChange()
    {
      this.currentJoystickId = -1;
    }

    private void ClearAllVars()
    {
      this.initialized = false;
      Rewired.UI.ControlMapper.ControlMapper.Instance = (Rewired.UI.ControlMapper.ControlMapper) null;
      this.playerCount = 0;
      this.inputGrid = (Rewired.UI.ControlMapper.ControlMapper.InputGrid) null;
      this.windowManager = (Rewired.UI.ControlMapper.ControlMapper.WindowManager) null;
      this.currentPlayerId = -1;
      this.currentMapCategoryId = -1;
      this.playerButtons = (List<Rewired.UI.ControlMapper.ControlMapper.GUIButton>) null;
      this.mapCategoryButtons = (List<Rewired.UI.ControlMapper.ControlMapper.GUIButton>) null;
      this.miscInstantiatedObjects = (List<GameObject>) null;
      this.canvas = (GameObject) null;
      this.lastUISelection = (GameObject) null;
      this.currentJoystickId = -1;
      this.pendingInputMapping = (Rewired.UI.ControlMapper.ControlMapper.InputMapping) null;
      this.pendingAxisCalibration = (Rewired.UI.ControlMapper.ControlMapper.AxisCalibrator) null;
      this.inputFieldActivatedDelegate = (Action<InputFieldInfo>) null;
      this.inputFieldInvertToggleStateChangedDelegate = (Action<ToggleInfo, bool>) null;
      this.isPollingForInput = false;
    }

    public void Reset()
    {
      if (!this.initialized)
        return;
      this.ClearCompletely();
      this.Initialize();
      if (!this.isOpen)
        return;
      this.Open(true);
    }

    private void SetActionAxisInverted(
      bool state,
      ControllerType controllerType,
      int actionElementMapId)
    {
      if (this.currentPlayer == null || !(this.GetControllerMap(controllerType) is ControllerMapWithAxes controllerMap))
        return;
      ((ControllerMap) controllerMap).GetElementMap(actionElementMapId)?.set_invert(state);
    }

    private ControllerMap GetControllerMap(ControllerType type)
    {
      if (this.currentPlayer == null)
        return (ControllerMap) null;
      int num = 0;
      switch ((int) type)
      {
        case 0:
        case 1:
          return ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) this.currentPlayer.controllers).maps).GetFirstMapInCategory(type, num, this.currentMapCategoryId);
        case 2:
          if (((Player.ControllerHelper) this.currentPlayer.controllers).get_joystickCount() <= 0)
            return (ControllerMap) null;
          num = (int) ((Controller) this.currentJoystick).id;
          goto case 0;
        default:
          throw new NotImplementedException();
      }
    }

    private ControllerMap GetControllerMapOrCreateNew(
      ControllerType controllerType,
      int controllerId,
      int layoutId)
    {
      ControllerMap controllerMap = this.GetControllerMap(controllerType);
      if (controllerMap == null)
      {
        ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) this.currentPlayer.controllers).maps).AddEmptyMap(controllerType, controllerId, this.currentMapCategoryId, layoutId);
        controllerMap = ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) this.currentPlayer.controllers).maps).GetMap(controllerType, controllerId, this.currentMapCategoryId, layoutId);
      }
      return controllerMap;
    }

    private int CountIEnumerable<T>(IEnumerable<T> enumerable)
    {
      if (enumerable == null)
        return 0;
      IEnumerator<T> enumerator = enumerable.GetEnumerator();
      if (enumerator == null)
        return 0;
      int num = 0;
      while (enumerator.MoveNext())
        ++num;
      return num;
    }

    private int GetDefaultMapCategoryId()
    {
      if (this._mappingSets.Length == 0)
        return 0;
      for (int index = 0; index < this._mappingSets.Length; ++index)
      {
        if (ReInput.get_mapping().GetMapCategory(this._mappingSets[index].mapCategoryId) != null)
          return this._mappingSets[index].mapCategoryId;
      }
      return 0;
    }

    private void SubscribeFixedUISelectionEvents()
    {
      if (this.references.fixedSelectableUIElements == null)
        return;
      foreach (GameObject selectableUiElement in this.references.fixedSelectableUIElements)
      {
        UIElementInfo component = (UIElementInfo) UnityTools.GetComponent<UIElementInfo>(selectableUiElement);
        if (!Object.op_Equality((Object) component, (Object) null))
          component.OnSelectedEvent += new Action<GameObject>(this.OnUIElementSelected);
      }
    }

    private void SubscribeMenuControlInputEvents()
    {
      this.SubscribeRewiredInputEventAllPlayers(this._screenToggleAction, new Action<InputActionEventData>(this.OnScreenToggleActionPressed));
      this.SubscribeRewiredInputEventAllPlayers(this._screenOpenAction, new Action<InputActionEventData>(this.OnScreenOpenActionPressed));
      this.SubscribeRewiredInputEventAllPlayers(this._screenCloseAction, new Action<InputActionEventData>(this.OnScreenCloseActionPressed));
      this.SubscribeRewiredInputEventAllPlayers(this._universalCancelAction, new Action<InputActionEventData>(this.OnUniversalCancelActionPressed));
    }

    private void UnsubscribeMenuControlInputEvents()
    {
      this.UnsubscribeRewiredInputEventAllPlayers(this._screenToggleAction, new Action<InputActionEventData>(this.OnScreenToggleActionPressed));
      this.UnsubscribeRewiredInputEventAllPlayers(this._screenOpenAction, new Action<InputActionEventData>(this.OnScreenOpenActionPressed));
      this.UnsubscribeRewiredInputEventAllPlayers(this._screenCloseAction, new Action<InputActionEventData>(this.OnScreenCloseActionPressed));
      this.UnsubscribeRewiredInputEventAllPlayers(this._universalCancelAction, new Action<InputActionEventData>(this.OnUniversalCancelActionPressed));
    }

    private void SubscribeRewiredInputEventAllPlayers(
      int actionId,
      Action<InputActionEventData> callback)
    {
      if (actionId < 0 || callback == null)
        return;
      if (ReInput.get_mapping().GetAction(actionId) == null)
      {
        Debug.LogWarning((object) ("Rewired Control Mapper: " + (object) actionId + " is not a valid Action id!"));
      }
      else
      {
        using (IEnumerator<Player> enumerator = ((IEnumerable<Player>) ReInput.get_players().get_AllPlayers()).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
            enumerator.Current.AddInputEventDelegate(callback, (UpdateLoopType) 0, (InputActionEventType) 3, actionId);
        }
      }
    }

    private void UnsubscribeRewiredInputEventAllPlayers(
      int actionId,
      Action<InputActionEventData> callback)
    {
      if (actionId < 0 || callback == null || !ReInput.get_isReady())
        return;
      if (ReInput.get_mapping().GetAction(actionId) == null)
      {
        Debug.LogWarning((object) ("Rewired Control Mapper: " + (object) actionId + " is not a valid Action id!"));
      }
      else
      {
        using (IEnumerator<Player> enumerator = ((IEnumerable<Player>) ReInput.get_players().get_AllPlayers()).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
            enumerator.Current.RemoveInputEventDelegate(callback, (UpdateLoopType) 0, (InputActionEventType) 3, actionId);
        }
      }
    }

    private int GetMaxControllersPerPlayer()
    {
      return this._rewiredInputManager.get_userData().get_ConfigVars().autoAssignJoysticks != null ? (int) this._rewiredInputManager.get_userData().get_ConfigVars().maxJoysticksPerPlayer : this._maxControllersPerPlayer;
    }

    private bool ShowAssignedControllers()
    {
      return this._showControllers && (this._showAssignedControllers || this.GetMaxControllersPerPlayer() != 1);
    }

    private void InspectorPropertyChanged(bool reset = false)
    {
      if (!reset)
        return;
      this.Reset();
    }

    private void AssignController(Player player, int controllerId)
    {
      if (player == null || ((Player.ControllerHelper) player.controllers).ContainsController((ControllerType) 2, controllerId))
        return;
      if (this.GetMaxControllersPerPlayer() == 1)
      {
        this.RemoveAllControllers(player);
        this.ClearVarsOnJoystickChange();
      }
      using (IEnumerator<Player> enumerator = ((IEnumerable<Player>) ReInput.get_players().get_Players()).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          Player current = enumerator.Current;
          if (current != player)
            this.RemoveController(current, controllerId);
        }
      }
      ((Player.ControllerHelper) player.controllers).AddController((ControllerType) 2, controllerId, false);
      if (ReInput.get_userDataStore() == null)
        return;
      ReInput.get_userDataStore().LoadControllerData(player.get_id(), (ControllerType) 2, controllerId);
    }

    private void RemoveAllControllers(Player player)
    {
      if (player == null)
        return;
      IList<Joystick> joysticks = ((Player.ControllerHelper) player.controllers).get_Joysticks();
      for (int index = ((ICollection<Joystick>) joysticks).Count - 1; index >= 0; --index)
        this.RemoveController(player, (int) ((Controller) joysticks[index]).id);
    }

    private void RemoveController(Player player, int controllerId)
    {
      if (player == null || !((Player.ControllerHelper) player.controllers).ContainsController((ControllerType) 2, controllerId))
        return;
      if (ReInput.get_userDataStore() != null)
        ReInput.get_userDataStore().SaveControllerData(player.get_id(), (ControllerType) 2, controllerId);
      ((Player.ControllerHelper) player.controllers).RemoveController((ControllerType) 2, controllerId);
    }

    private bool IsAllowedAssignment(
      Rewired.UI.ControlMapper.ControlMapper.InputMapping pendingInputMapping,
      ControllerPollingInfo pollingInfo)
    {
      return pendingInputMapping != null && (pendingInputMapping.axisRange != null || this._showSplitAxisInputFields || ((ControllerPollingInfo) ref pollingInfo).get_elementType() != 1);
    }

    private void InputPollingStarted()
    {
      bool isPollingForInput = this.isPollingForInput;
      this.isPollingForInput = true;
      if (isPollingForInput)
        return;
      if (this._InputPollingStartedEvent != null)
        this._InputPollingStartedEvent();
      if (this._onInputPollingStarted == null)
        return;
      this._onInputPollingStarted.Invoke();
    }

    private void InputPollingStopped()
    {
      bool isPollingForInput = this.isPollingForInput;
      this.isPollingForInput = false;
      if (!isPollingForInput)
        return;
      if (this._InputPollingEndedEvent != null)
        this._InputPollingEndedEvent();
      if (this._onInputPollingEnded == null)
        return;
      this._onInputPollingEnded.Invoke();
    }

    private int GetControllerInputFieldCount(ControllerType controllerType)
    {
      switch ((int) controllerType)
      {
        case 0:
          return this._keyboardInputFieldCount;
        case 1:
          return this._mouseInputFieldCount;
        case 2:
          return this._controllerInputFieldCount;
        default:
          throw new NotImplementedException();
      }
    }

    private bool ShowSwapButton(
      int windowId,
      Rewired.UI.ControlMapper.ControlMapper.InputMapping mapping,
      ElementAssignment assignment,
      bool skipOtherPlayers)
    {
      if (this.currentPlayer == null || !this._allowElementAssignmentSwap || (mapping == null || mapping.aem == null))
        return false;
      ElementAssignmentConflictCheck conflictCheck;
      if (!this.CreateConflictCheck(mapping, assignment, out conflictCheck))
      {
        Debug.LogError((object) "Rewired Control Mapper: Error creating conflict check!");
        return false;
      }
      List<ElementAssignmentConflictInfo> assignmentConflictInfoList = new List<ElementAssignmentConflictInfo>();
      assignmentConflictInfoList.AddRange(((Player.ControllerHelper.ConflictCheckingHelper) ((Player.ControllerHelper) this.currentPlayer.controllers).conflictChecking).ElementAssignmentConflicts(conflictCheck));
      assignmentConflictInfoList.AddRange(((Player.ControllerHelper.ConflictCheckingHelper) ((Player.ControllerHelper) ReInput.get_players().get_SystemPlayer().controllers).conflictChecking).ElementAssignmentConflicts(conflictCheck));
      if (assignmentConflictInfoList.Count == 0)
        return false;
      ActionElementMap aem1 = mapping.aem;
      ElementAssignmentConflictInfo assignmentConflictInfo = assignmentConflictInfoList[0];
      int actionId = ((ElementAssignmentConflictInfo) ref assignmentConflictInfo).get_elementMap().get_actionId();
      Pole axisContribution = ((ElementAssignmentConflictInfo) ref assignmentConflictInfo).get_elementMap().get_axisContribution();
      AxisRange origAxisRange = aem1.get_axisRange();
      ControllerElementType elementType = aem1.get_elementType();
      if (elementType == ((ElementAssignmentConflictInfo) ref assignmentConflictInfo).get_elementMap().get_elementType() && elementType == null)
      {
        if (origAxisRange != ((ElementAssignmentConflictInfo) ref assignmentConflictInfo).get_elementMap().get_axisRange())
        {
          if (origAxisRange == null)
            origAxisRange = (AxisRange) 1;
          else if (((ElementAssignmentConflictInfo) ref assignmentConflictInfo).get_elementMap().get_axisRange() != null)
            ;
        }
      }
      else if (elementType == null && (((ElementAssignmentConflictInfo) ref assignmentConflictInfo).get_elementMap().get_elementType() == 1 || ((ElementAssignmentConflictInfo) ref assignmentConflictInfo).get_elementMap().get_elementType() == null && ((ElementAssignmentConflictInfo) ref assignmentConflictInfo).get_elementMap().get_axisRange() != null) && origAxisRange == null)
        origAxisRange = (AxisRange) 1;
      int num = 0;
      if (assignment.actionId == ((ElementAssignmentConflictInfo) ref assignmentConflictInfo).get_actionId() && mapping.map == ((ElementAssignmentConflictInfo) ref assignmentConflictInfo).get_controllerMap())
      {
        Controller controller = ReInput.get_controllers().GetController(mapping.controllerType, mapping.controllerId);
        if (this.SwapIsSameInputRange(elementType, origAxisRange, axisContribution, (ControllerElementType) controller.GetElementById((int) assignment.elementIdentifierId).type, (AxisRange) assignment.axisRange, (Pole) assignment.axisContribution))
          ++num;
      }
      using (IEnumerator<ActionElementMap> enumerator = ((ElementAssignmentConflictInfo) ref assignmentConflictInfo).get_controllerMap().ElementMapsWithAction(actionId).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          ActionElementMap aem = enumerator.Current;
          if (aem.get_id() != aem1.get_id() && assignmentConflictInfoList.FindIndex((Predicate<ElementAssignmentConflictInfo>) (x => ((ElementAssignmentConflictInfo) ref x).get_elementMapId() == aem.get_id())) < 0 && this.SwapIsSameInputRange(elementType, origAxisRange, axisContribution, aem.get_elementType(), aem.get_axisRange(), aem.get_axisContribution()))
            ++num;
        }
      }
      return num < this.GetControllerInputFieldCount(mapping.controllerType);
    }

    private bool SwapIsSameInputRange(
      ControllerElementType origElementType,
      AxisRange origAxisRange,
      Pole origAxisContribution,
      ControllerElementType conflictElementType,
      AxisRange conflictAxisRange,
      Pole conflictAxisContribution)
    {
      return (origElementType == 1 || origElementType == null && origAxisRange != null) && (conflictElementType == 1 || conflictElementType == null && conflictAxisRange != null) && conflictAxisContribution == origAxisContribution || origElementType == null && origAxisRange == null && (conflictElementType == null && conflictAxisRange == null);
    }

    public static void ApplyTheme(ThemedElement.ElementInfo[] elementInfo)
    {
      if (Object.op_Equality((Object) Rewired.UI.ControlMapper.ControlMapper.Instance, (Object) null) || Object.op_Equality((Object) Rewired.UI.ControlMapper.ControlMapper.Instance._themeSettings, (Object) null) || !Rewired.UI.ControlMapper.ControlMapper.Instance._useThemeSettings)
        return;
      Rewired.UI.ControlMapper.ControlMapper.Instance._themeSettings.Apply(elementInfo);
    }

    public static LanguageData GetLanguage()
    {
      return Object.op_Equality((Object) Rewired.UI.ControlMapper.ControlMapper.Instance, (Object) null) ? (LanguageData) null : Rewired.UI.ControlMapper.ControlMapper.Instance._language;
    }

    private abstract class GUIElement
    {
      public readonly GameObject gameObject;
      protected readonly Text text;
      public readonly Selectable selectable;
      protected readonly UIElementInfo uiElementInfo;
      protected bool permanentStateSet;
      protected readonly List<Rewired.UI.ControlMapper.ControlMapper.GUIElement> children;

      public GUIElement(GameObject gameObject)
      {
        if (Object.op_Equality((Object) gameObject, (Object) null))
        {
          Debug.LogError((object) "Rewired Control Mapper: gameObject is null!");
        }
        else
        {
          this.selectable = (Selectable) gameObject.GetComponent<Selectable>();
          if (Object.op_Equality((Object) this.selectable, (Object) null))
          {
            Debug.LogError((object) "Rewired Control Mapper: Selectable is null!");
          }
          else
          {
            this.gameObject = gameObject;
            this.rectTransform = (RectTransform) gameObject.GetComponent<RectTransform>();
            this.text = (Text) UnityTools.GetComponentInSelfOrChildren<Text>(gameObject);
            this.uiElementInfo = (UIElementInfo) gameObject.GetComponent<UIElementInfo>();
            this.children = new List<Rewired.UI.ControlMapper.ControlMapper.GUIElement>();
          }
        }
      }

      public GUIElement(Selectable selectable, Text label)
      {
        if (Object.op_Equality((Object) selectable, (Object) null))
        {
          Debug.LogError((object) "Rewired Control Mapper: Selectable is null!");
        }
        else
        {
          this.selectable = selectable;
          this.gameObject = ((Component) selectable).get_gameObject();
          this.rectTransform = (RectTransform) this.gameObject.GetComponent<RectTransform>();
          this.text = label;
          this.uiElementInfo = (UIElementInfo) this.gameObject.GetComponent<UIElementInfo>();
          this.children = new List<Rewired.UI.ControlMapper.ControlMapper.GUIElement>();
        }
      }

      public RectTransform rectTransform { get; private set; }

      public virtual void SetInteractible(bool state, bool playTransition)
      {
        this.SetInteractible(state, playTransition, false);
      }

      public virtual void SetInteractible(bool state, bool playTransition, bool permanent)
      {
        for (int index = 0; index < this.children.Count; ++index)
        {
          if (this.children[index] != null)
            this.children[index].SetInteractible(state, playTransition, permanent);
        }
        if (this.permanentStateSet || Object.op_Equality((Object) this.selectable, (Object) null))
          return;
        if (permanent)
          this.permanentStateSet = true;
        if (this.selectable.get_interactable() == state)
          return;
        UITools.SetInteractable(this.selectable, state, playTransition);
      }

      public virtual void SetTextWidth(int value)
      {
        if (Object.op_Equality((Object) this.text, (Object) null))
          return;
        LayoutElement layoutElement = (LayoutElement) ((Component) this.text).GetComponent<LayoutElement>();
        if (Object.op_Equality((Object) layoutElement, (Object) null))
          layoutElement = (LayoutElement) ((Component) this.text).get_gameObject().AddComponent<LayoutElement>();
        layoutElement.set_preferredWidth((float) value);
      }

      public virtual void SetFirstChildObjectWidth(
        Rewired.UI.ControlMapper.ControlMapper.LayoutElementSizeType type,
        int value)
      {
        if (((Transform) this.rectTransform).get_childCount() == 0)
          return;
        Transform child = ((Transform) this.rectTransform).GetChild(0);
        LayoutElement layoutElement = (LayoutElement) ((Component) child).GetComponent<LayoutElement>();
        if (Object.op_Equality((Object) layoutElement, (Object) null))
          layoutElement = (LayoutElement) ((Component) child).get_gameObject().AddComponent<LayoutElement>();
        if (type == Rewired.UI.ControlMapper.ControlMapper.LayoutElementSizeType.MinSize)
        {
          layoutElement.set_minWidth((float) value);
        }
        else
        {
          if (type != Rewired.UI.ControlMapper.ControlMapper.LayoutElementSizeType.PreferredSize)
            throw new NotImplementedException();
          layoutElement.set_preferredWidth((float) value);
        }
      }

      public virtual void SetLabel(string label)
      {
        if (Object.op_Equality((Object) this.text, (Object) null))
          return;
        this.text.set_text(label);
      }

      public virtual string GetLabel()
      {
        return Object.op_Equality((Object) this.text, (Object) null) ? string.Empty : this.text.get_text();
      }

      public virtual void AddChild(Rewired.UI.ControlMapper.ControlMapper.GUIElement child)
      {
        this.children.Add(child);
      }

      public void SetElementInfoData(string identifier, int intData)
      {
        if (Object.op_Equality((Object) this.uiElementInfo, (Object) null))
          return;
        this.uiElementInfo.identifier = identifier;
        this.uiElementInfo.intData = intData;
      }

      public virtual void SetActive(bool state)
      {
        if (Object.op_Equality((Object) this.gameObject, (Object) null))
          return;
        this.gameObject.SetActive(state);
      }

      protected virtual bool Init()
      {
        bool flag = true;
        for (int index = 0; index < this.children.Count; ++index)
        {
          if (this.children[index] != null && !this.children[index].Init())
            flag = false;
        }
        if (Object.op_Equality((Object) this.selectable, (Object) null))
        {
          Debug.LogError((object) "Rewired Control Mapper: UI Element is missing Selectable component!");
          flag = false;
        }
        if (Object.op_Equality((Object) this.rectTransform, (Object) null))
        {
          Debug.LogError((object) "Rewired Control Mapper: UI Element is missing RectTransform component!");
          flag = false;
        }
        if (Object.op_Equality((Object) this.uiElementInfo, (Object) null))
        {
          Debug.LogError((object) "Rewired Control Mapper: UI Element is missing UIElementInfo component!");
          flag = false;
        }
        return flag;
      }
    }

    private class GUIButton : Rewired.UI.ControlMapper.ControlMapper.GUIElement
    {
      public GUIButton(GameObject gameObject)
        : base(gameObject)
      {
        if (this.Init())
          ;
      }

      public GUIButton(Button button, Text label)
        : base((Selectable) button, label)
      {
        if (this.Init())
          ;
      }

      protected Button button
      {
        get
        {
          return this.selectable as Button;
        }
      }

      public ButtonInfo buttonInfo
      {
        get
        {
          return this.uiElementInfo as ButtonInfo;
        }
      }

      public void SetButtonInfoData(string identifier, int intData)
      {
        this.SetElementInfoData(identifier, intData);
      }

      public void SetOnClickCallback(Action<ButtonInfo> callback)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        Rewired.UI.ControlMapper.ControlMapper.GUIButton.\u003CSetOnClickCallback\u003Ec__AnonStorey0 callbackCAnonStorey0 = new Rewired.UI.ControlMapper.ControlMapper.GUIButton.\u003CSetOnClickCallback\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        callbackCAnonStorey0.callback = callback;
        // ISSUE: reference to a compiler-generated field
        callbackCAnonStorey0.\u0024this = this;
        if (Object.op_Equality((Object) this.button, (Object) null))
          return;
        // ISSUE: method pointer
        ((UnityEvent) this.button.get_onClick()).AddListener(new UnityAction((object) callbackCAnonStorey0, __methodptr(\u003C\u003Em__0)));
      }
    }

    private class GUIInputField : Rewired.UI.ControlMapper.ControlMapper.GUIElement
    {
      public GUIInputField(GameObject gameObject)
        : base(gameObject)
      {
        if (this.Init())
          ;
      }

      public GUIInputField(Button button, Text label)
        : base((Selectable) button, label)
      {
        if (this.Init())
          ;
      }

      protected Button button
      {
        get
        {
          return this.selectable as Button;
        }
      }

      public InputFieldInfo fieldInfo
      {
        get
        {
          return this.uiElementInfo as InputFieldInfo;
        }
      }

      public bool hasToggle
      {
        get
        {
          return this.toggle != null;
        }
      }

      public Rewired.UI.ControlMapper.ControlMapper.GUIToggle toggle { get; private set; }

      public int actionElementMapId
      {
        get
        {
          return Object.op_Equality((Object) this.fieldInfo, (Object) null) ? -1 : this.fieldInfo.actionElementMapId;
        }
        set
        {
          if (Object.op_Equality((Object) this.fieldInfo, (Object) null))
            return;
          this.fieldInfo.actionElementMapId = value;
        }
      }

      public int controllerId
      {
        get
        {
          return Object.op_Equality((Object) this.fieldInfo, (Object) null) ? -1 : this.fieldInfo.controllerId;
        }
        set
        {
          if (Object.op_Equality((Object) this.fieldInfo, (Object) null))
            return;
          this.fieldInfo.controllerId = value;
        }
      }

      public void SetFieldInfoData(
        int actionId,
        AxisRange axisRange,
        ControllerType controllerType,
        int intData)
      {
        this.SetElementInfoData(string.Empty, intData);
        if (Object.op_Equality((Object) this.fieldInfo, (Object) null))
          return;
        this.fieldInfo.actionId = actionId;
        this.fieldInfo.axisRange = axisRange;
        this.fieldInfo.controllerType = controllerType;
      }

      public void SetOnClickCallback(Action<InputFieldInfo> callback)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        Rewired.UI.ControlMapper.ControlMapper.GUIInputField.\u003CSetOnClickCallback\u003Ec__AnonStorey0 callbackCAnonStorey0 = new Rewired.UI.ControlMapper.ControlMapper.GUIInputField.\u003CSetOnClickCallback\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        callbackCAnonStorey0.callback = callback;
        // ISSUE: reference to a compiler-generated field
        callbackCAnonStorey0.\u0024this = this;
        if (Object.op_Equality((Object) this.button, (Object) null))
          return;
        // ISSUE: method pointer
        ((UnityEvent) this.button.get_onClick()).AddListener(new UnityAction((object) callbackCAnonStorey0, __methodptr(\u003C\u003Em__0)));
      }

      public virtual void SetInteractable(bool state, bool playTransition, bool permanent)
      {
        if (this.permanentStateSet)
          return;
        if (this.hasToggle && !state)
          this.toggle.SetInteractible(state, playTransition, permanent);
        this.SetInteractible(state, playTransition, permanent);
      }

      public void AddToggle(Rewired.UI.ControlMapper.ControlMapper.GUIToggle toggle)
      {
        if (toggle == null)
          return;
        this.toggle = toggle;
      }
    }

    private class GUIToggle : Rewired.UI.ControlMapper.ControlMapper.GUIElement
    {
      public GUIToggle(GameObject gameObject)
        : base(gameObject)
      {
        if (this.Init())
          ;
      }

      public GUIToggle(Toggle toggle, Text label)
        : base((Selectable) toggle, label)
      {
        if (this.Init())
          ;
      }

      protected Toggle toggle
      {
        get
        {
          return this.selectable as Toggle;
        }
      }

      public ToggleInfo toggleInfo
      {
        get
        {
          return this.uiElementInfo as ToggleInfo;
        }
      }

      public int actionElementMapId
      {
        get
        {
          return Object.op_Equality((Object) this.toggleInfo, (Object) null) ? -1 : this.toggleInfo.actionElementMapId;
        }
        set
        {
          if (Object.op_Equality((Object) this.toggleInfo, (Object) null))
            return;
          this.toggleInfo.actionElementMapId = value;
        }
      }

      public void SetToggleInfoData(
        int actionId,
        AxisRange axisRange,
        ControllerType controllerType,
        int intData)
      {
        this.SetElementInfoData(string.Empty, intData);
        if (Object.op_Equality((Object) this.toggleInfo, (Object) null))
          return;
        this.toggleInfo.actionId = actionId;
        this.toggleInfo.axisRange = axisRange;
        this.toggleInfo.controllerType = controllerType;
      }

      public void SetOnSubmitCallback(Action<ToggleInfo, bool> callback)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        Rewired.UI.ControlMapper.ControlMapper.GUIToggle.\u003CSetOnSubmitCallback\u003Ec__AnonStorey0 callbackCAnonStorey0 = new Rewired.UI.ControlMapper.ControlMapper.GUIToggle.\u003CSetOnSubmitCallback\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        callbackCAnonStorey0.callback = callback;
        // ISSUE: reference to a compiler-generated field
        callbackCAnonStorey0.\u0024this = this;
        if (Object.op_Equality((Object) this.toggle, (Object) null))
          return;
        EventTrigger eventTrigger = (EventTrigger) ((Component) this.toggle).GetComponent<EventTrigger>();
        if (Object.op_Equality((Object) eventTrigger, (Object) null))
          eventTrigger = (EventTrigger) ((Component) this.toggle).get_gameObject().AddComponent<EventTrigger>();
        EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
        // ISSUE: method pointer
        ((UnityEvent<BaseEventData>) triggerEvent).AddListener(new UnityAction<BaseEventData>((object) callbackCAnonStorey0, __methodptr(\u003C\u003Em__0)));
        EventTrigger.Entry entry1 = new EventTrigger.Entry()
        {
          callback = (__Null) triggerEvent,
          eventID = (__Null) 15
        };
        EventTrigger.Entry entry2 = new EventTrigger.Entry()
        {
          callback = (__Null) triggerEvent,
          eventID = (__Null) 4
        };
        if (eventTrigger.get_triggers() != null)
          eventTrigger.get_triggers().Clear();
        else
          eventTrigger.set_triggers(new List<EventTrigger.Entry>());
        eventTrigger.get_triggers().Add(entry1);
        eventTrigger.get_triggers().Add(entry2);
      }

      public void SetToggleState(bool state)
      {
        if (Object.op_Equality((Object) this.toggle, (Object) null))
          return;
        this.toggle.set_isOn(state);
      }
    }

    private class GUILabel
    {
      public GUILabel(GameObject gameObject)
      {
        if (Object.op_Equality((Object) gameObject, (Object) null))
        {
          Debug.LogError((object) "Rewired Control Mapper: gameObject is null!");
        }
        else
        {
          this.text = (Text) UnityTools.GetComponentInSelfOrChildren<Text>(gameObject);
          this.Check();
        }
      }

      public GUILabel(Text label)
      {
        this.text = label;
        if (this.Check())
          ;
      }

      public GameObject gameObject { get; private set; }

      private Text text { get; set; }

      public RectTransform rectTransform { get; private set; }

      public void SetSize(int width, int height)
      {
        if (Object.op_Equality((Object) this.text, (Object) null))
          return;
        this.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 0, (float) width);
        this.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, (float) height);
      }

      public void SetWidth(int width)
      {
        if (Object.op_Equality((Object) this.text, (Object) null))
          return;
        this.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 0, (float) width);
      }

      public void SetHeight(int height)
      {
        if (Object.op_Equality((Object) this.text, (Object) null))
          return;
        this.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, (float) height);
      }

      public void SetLabel(string label)
      {
        if (Object.op_Equality((Object) this.text, (Object) null))
          return;
        this.text.set_text(label);
      }

      public void SetFontStyle(FontStyle style)
      {
        if (Object.op_Equality((Object) this.text, (Object) null))
          return;
        this.text.set_fontStyle(style);
      }

      public void SetTextAlignment(TextAnchor alignment)
      {
        if (Object.op_Equality((Object) this.text, (Object) null))
          return;
        this.text.set_alignment(alignment);
      }

      public void SetActive(bool state)
      {
        if (Object.op_Equality((Object) this.gameObject, (Object) null))
          return;
        this.gameObject.SetActive(state);
      }

      private bool Check()
      {
        bool flag = true;
        if (Object.op_Equality((Object) this.text, (Object) null))
        {
          Debug.LogError((object) "Rewired Control Mapper: Button is missing Text child component!");
          flag = false;
        }
        this.gameObject = ((Component) this.text).get_gameObject();
        this.rectTransform = (RectTransform) ((Component) this.text).GetComponent<RectTransform>();
        return flag;
      }
    }

    [Serializable]
    public class MappingSet
    {
      [SerializeField]
      [Tooltip("The Map Category that will be displayed to the user for remapping.")]
      private int _mapCategoryId;
      [SerializeField]
      [Tooltip("Choose whether you want to list Actions to display for this Map Category by individual Action or by all the Actions in an Action Category.")]
      private Rewired.UI.ControlMapper.ControlMapper.MappingSet.ActionListMode _actionListMode;
      [SerializeField]
      private int[] _actionCategoryIds;
      [SerializeField]
      private int[] _actionIds;
      private IList<int> _actionCategoryIdsReadOnly;
      private IList<int> _actionIdsReadOnly;

      public MappingSet()
      {
        this._mapCategoryId = -1;
        this._actionCategoryIds = new int[0];
        this._actionIds = new int[0];
        this._actionListMode = Rewired.UI.ControlMapper.ControlMapper.MappingSet.ActionListMode.ActionCategory;
      }

      private MappingSet(
        int mapCategoryId,
        Rewired.UI.ControlMapper.ControlMapper.MappingSet.ActionListMode actionListMode,
        int[] actionCategoryIds,
        int[] actionIds)
      {
        this._mapCategoryId = mapCategoryId;
        this._actionListMode = actionListMode;
        this._actionCategoryIds = actionCategoryIds;
        this._actionIds = actionIds;
      }

      public int mapCategoryId
      {
        get
        {
          return this._mapCategoryId;
        }
      }

      public Rewired.UI.ControlMapper.ControlMapper.MappingSet.ActionListMode actionListMode
      {
        get
        {
          return this._actionListMode;
        }
      }

      public IList<int> actionCategoryIds
      {
        get
        {
          if (this._actionCategoryIds == null)
            return (IList<int>) null;
          if (this._actionCategoryIdsReadOnly == null)
            this._actionCategoryIdsReadOnly = (IList<int>) new ReadOnlyCollection<int>((IList<int>) this._actionCategoryIds);
          return this._actionCategoryIdsReadOnly;
        }
      }

      public IList<int> actionIds
      {
        get
        {
          if (this._actionIds == null)
            return (IList<int>) null;
          if (this._actionIdsReadOnly == null)
            this._actionIdsReadOnly = (IList<int>) new ReadOnlyCollection<int>((IList<int>) this._actionIds);
          return (IList<int>) this._actionIds;
        }
      }

      public bool isValid
      {
        get
        {
          return this._mapCategoryId >= 0 && ReInput.get_mapping().GetMapCategory(this._mapCategoryId) != null;
        }
      }

      public static Rewired.UI.ControlMapper.ControlMapper.MappingSet Default
      {
        get
        {
          return new Rewired.UI.ControlMapper.ControlMapper.MappingSet(0, Rewired.UI.ControlMapper.ControlMapper.MappingSet.ActionListMode.ActionCategory, new int[1], new int[0]);
        }
      }

      public enum ActionListMode
      {
        ActionCategory,
        Action,
      }
    }

    [Serializable]
    public class InputBehaviorSettings
    {
      [SerializeField]
      [Tooltip("The Input Behavior that will be displayed to the user for modification.")]
      private int _inputBehaviorId = -1;
      [SerializeField]
      [Tooltip("If checked, a slider will be displayed so the user can change this value.")]
      private bool _showJoystickAxisSensitivity = true;
      [SerializeField]
      [Tooltip("If checked, a slider will be displayed so the user can change this value.")]
      private bool _showMouseXYAxisSensitivity = true;
      [SerializeField]
      [Tooltip("If set to a non-blank value, this key will be used to look up the name in Language to be displayed as the title for the Input Behavior control set. Otherwise, the name field of the InputBehavior will be used.")]
      private string _labelLanguageKey = string.Empty;
      [SerializeField]
      [Tooltip("If set to a non-blank value, this name will be displayed above the individual slider control. Otherwise, no name will be displayed.")]
      private string _joystickAxisSensitivityLabelLanguageKey = string.Empty;
      [SerializeField]
      [Tooltip("If set to a non-blank value, this key will be used to look up the name in Language to be displayed above the individual slider control. Otherwise, no name will be displayed.")]
      private string _mouseXYAxisSensitivityLabelLanguageKey = string.Empty;
      [SerializeField]
      [Tooltip("Maximum value the user is allowed to set for this property.")]
      private float _joystickAxisSensitivityMax = 2f;
      [SerializeField]
      [Tooltip("Maximum value the user is allowed to set for this property.")]
      private float _mouseXYAxisSensitivityMax = 2f;
      [SerializeField]
      [Tooltip("The icon to display next to the slider. Set to none for no icon.")]
      private Sprite _joystickAxisSensitivityIcon;
      [SerializeField]
      [Tooltip("The icon to display next to the slider. Set to none for no icon.")]
      private Sprite _mouseXYAxisSensitivityIcon;
      [SerializeField]
      [Tooltip("Minimum value the user is allowed to set for this property.")]
      private float _joystickAxisSensitivityMin;
      [SerializeField]
      [Tooltip("Minimum value the user is allowed to set for this property.")]
      private float _mouseXYAxisSensitivityMin;

      public int inputBehaviorId
      {
        get
        {
          return this._inputBehaviorId;
        }
      }

      public bool showJoystickAxisSensitivity
      {
        get
        {
          return this._showJoystickAxisSensitivity;
        }
      }

      public bool showMouseXYAxisSensitivity
      {
        get
        {
          return this._showMouseXYAxisSensitivity;
        }
      }

      public string labelLanguageKey
      {
        get
        {
          return this._labelLanguageKey;
        }
      }

      public string joystickAxisSensitivityLabelLanguageKey
      {
        get
        {
          return this._joystickAxisSensitivityLabelLanguageKey;
        }
      }

      public string mouseXYAxisSensitivityLabelLanguageKey
      {
        get
        {
          return this._mouseXYAxisSensitivityLabelLanguageKey;
        }
      }

      public Sprite joystickAxisSensitivityIcon
      {
        get
        {
          return this._joystickAxisSensitivityIcon;
        }
      }

      public Sprite mouseXYAxisSensitivityIcon
      {
        get
        {
          return this._mouseXYAxisSensitivityIcon;
        }
      }

      public float joystickAxisSensitivityMin
      {
        get
        {
          return this._joystickAxisSensitivityMin;
        }
      }

      public float joystickAxisSensitivityMax
      {
        get
        {
          return this._joystickAxisSensitivityMax;
        }
      }

      public float mouseXYAxisSensitivityMin
      {
        get
        {
          return this._mouseXYAxisSensitivityMin;
        }
      }

      public float mouseXYAxisSensitivityMax
      {
        get
        {
          return this._mouseXYAxisSensitivityMax;
        }
      }

      public bool isValid
      {
        get
        {
          if (this._inputBehaviorId < 0)
            return false;
          return this._showJoystickAxisSensitivity || this._showMouseXYAxisSensitivity;
        }
      }
    }

    [Serializable]
    private class Prefabs
    {
      [SerializeField]
      private GameObject _button;
      [SerializeField]
      private GameObject _fitButton;
      [SerializeField]
      private GameObject _inputGridLabel;
      [SerializeField]
      private GameObject _inputGridHeaderLabel;
      [SerializeField]
      private GameObject _inputGridFieldButton;
      [SerializeField]
      private GameObject _inputGridFieldInvertToggle;
      [SerializeField]
      private GameObject _window;
      [SerializeField]
      private GameObject _windowTitleText;
      [SerializeField]
      private GameObject _windowContentText;
      [SerializeField]
      private GameObject _fader;
      [SerializeField]
      private GameObject _calibrationWindow;
      [SerializeField]
      private GameObject _inputBehaviorsWindow;
      [SerializeField]
      private GameObject _centerStickGraphic;
      [SerializeField]
      private GameObject _moveStickGraphic;

      public GameObject button
      {
        get
        {
          return this._button;
        }
      }

      public GameObject fitButton
      {
        get
        {
          return this._fitButton;
        }
      }

      public GameObject inputGridLabel
      {
        get
        {
          return this._inputGridLabel;
        }
      }

      public GameObject inputGridHeaderLabel
      {
        get
        {
          return this._inputGridHeaderLabel;
        }
      }

      public GameObject inputGridFieldButton
      {
        get
        {
          return this._inputGridFieldButton;
        }
      }

      public GameObject inputGridFieldInvertToggle
      {
        get
        {
          return this._inputGridFieldInvertToggle;
        }
      }

      public GameObject window
      {
        get
        {
          return this._window;
        }
      }

      public GameObject windowTitleText
      {
        get
        {
          return this._windowTitleText;
        }
      }

      public GameObject windowContentText
      {
        get
        {
          return this._windowContentText;
        }
      }

      public GameObject fader
      {
        get
        {
          return this._fader;
        }
      }

      public GameObject calibrationWindow
      {
        get
        {
          return this._calibrationWindow;
        }
      }

      public GameObject inputBehaviorsWindow
      {
        get
        {
          return this._inputBehaviorsWindow;
        }
      }

      public GameObject centerStickGraphic
      {
        get
        {
          return this._centerStickGraphic;
        }
      }

      public GameObject moveStickGraphic
      {
        get
        {
          return this._moveStickGraphic;
        }
      }

      public bool Check()
      {
        return !Object.op_Equality((Object) this._button, (Object) null) && !Object.op_Equality((Object) this._fitButton, (Object) null) && (!Object.op_Equality((Object) this._inputGridLabel, (Object) null) && !Object.op_Equality((Object) this._inputGridHeaderLabel, (Object) null)) && (!Object.op_Equality((Object) this._inputGridFieldButton, (Object) null) && !Object.op_Equality((Object) this._inputGridFieldInvertToggle, (Object) null) && (!Object.op_Equality((Object) this._window, (Object) null) && !Object.op_Equality((Object) this._windowTitleText, (Object) null))) && (!Object.op_Equality((Object) this._windowContentText, (Object) null) && !Object.op_Equality((Object) this._fader, (Object) null) && (!Object.op_Equality((Object) this._calibrationWindow, (Object) null) && !Object.op_Equality((Object) this._inputBehaviorsWindow, (Object) null)));
      }
    }

    [Serializable]
    private class References
    {
      [SerializeField]
      private Canvas _canvas;
      [SerializeField]
      private CanvasGroup _mainCanvasGroup;
      [SerializeField]
      private Transform _mainContent;
      [SerializeField]
      private Transform _mainContentInner;
      [SerializeField]
      private UIGroup _playersGroup;
      [SerializeField]
      private Transform _controllerGroup;
      [SerializeField]
      private Transform _controllerGroupLabelGroup;
      [SerializeField]
      private UIGroup _controllerSettingsGroup;
      [SerializeField]
      private UIGroup _assignedControllersGroup;
      [SerializeField]
      private Transform _settingsAndMapCategoriesGroup;
      [SerializeField]
      private UIGroup _settingsGroup;
      [SerializeField]
      private UIGroup _mapCategoriesGroup;
      [SerializeField]
      private Transform _inputGridGroup;
      [SerializeField]
      private Transform _inputGridContainer;
      [SerializeField]
      private Transform _inputGridHeadersGroup;
      [SerializeField]
      private Scrollbar _inputGridVScrollbar;
      [SerializeField]
      private ScrollRect _inputGridScrollRect;
      [SerializeField]
      private Transform _inputGridInnerGroup;
      [SerializeField]
      private Text _controllerNameLabel;
      [SerializeField]
      private Button _removeControllerButton;
      [SerializeField]
      private Button _assignControllerButton;
      [SerializeField]
      private Button _calibrateControllerButton;
      [SerializeField]
      private Button _doneButton;
      [SerializeField]
      private Button _restoreDefaultsButton;
      [SerializeField]
      private Selectable _defaultSelection;
      [SerializeField]
      private GameObject[] _fixedSelectableUIElements;
      [SerializeField]
      private Image _mainBackgroundImage;

      public Canvas canvas
      {
        get
        {
          return this._canvas;
        }
      }

      public CanvasGroup mainCanvasGroup
      {
        get
        {
          return this._mainCanvasGroup;
        }
      }

      public Transform mainContent
      {
        get
        {
          return this._mainContent;
        }
      }

      public Transform mainContentInner
      {
        get
        {
          return this._mainContentInner;
        }
      }

      public UIGroup playersGroup
      {
        get
        {
          return this._playersGroup;
        }
      }

      public Transform controllerGroup
      {
        get
        {
          return this._controllerGroup;
        }
      }

      public Transform controllerGroupLabelGroup
      {
        get
        {
          return this._controllerGroupLabelGroup;
        }
      }

      public UIGroup controllerSettingsGroup
      {
        get
        {
          return this._controllerSettingsGroup;
        }
      }

      public UIGroup assignedControllersGroup
      {
        get
        {
          return this._assignedControllersGroup;
        }
      }

      public Transform settingsAndMapCategoriesGroup
      {
        get
        {
          return this._settingsAndMapCategoriesGroup;
        }
      }

      public UIGroup settingsGroup
      {
        get
        {
          return this._settingsGroup;
        }
      }

      public UIGroup mapCategoriesGroup
      {
        get
        {
          return this._mapCategoriesGroup;
        }
      }

      public Transform inputGridGroup
      {
        get
        {
          return this._inputGridGroup;
        }
      }

      public Transform inputGridContainer
      {
        get
        {
          return this._inputGridContainer;
        }
      }

      public Transform inputGridHeadersGroup
      {
        get
        {
          return this._inputGridHeadersGroup;
        }
      }

      public Scrollbar inputGridVScrollbar
      {
        get
        {
          return this._inputGridVScrollbar;
        }
      }

      public ScrollRect inputGridScrollRect
      {
        get
        {
          return this._inputGridScrollRect;
        }
      }

      public Transform inputGridInnerGroup
      {
        get
        {
          return this._inputGridInnerGroup;
        }
      }

      public Text controllerNameLabel
      {
        get
        {
          return this._controllerNameLabel;
        }
      }

      public Button removeControllerButton
      {
        get
        {
          return this._removeControllerButton;
        }
      }

      public Button assignControllerButton
      {
        get
        {
          return this._assignControllerButton;
        }
      }

      public Button calibrateControllerButton
      {
        get
        {
          return this._calibrateControllerButton;
        }
      }

      public Button doneButton
      {
        get
        {
          return this._doneButton;
        }
      }

      public Button restoreDefaultsButton
      {
        get
        {
          return this._restoreDefaultsButton;
        }
      }

      public Selectable defaultSelection
      {
        get
        {
          return this._defaultSelection;
        }
      }

      public GameObject[] fixedSelectableUIElements
      {
        get
        {
          return this._fixedSelectableUIElements;
        }
      }

      public Image mainBackgroundImage
      {
        get
        {
          return this._mainBackgroundImage;
        }
      }

      public LayoutElement inputGridLayoutElement { get; set; }

      public Transform inputGridActionColumn { get; set; }

      public Transform inputGridKeyboardColumn { get; set; }

      public Transform inputGridMouseColumn { get; set; }

      public Transform inputGridControllerColumn { get; set; }

      public Transform inputGridHeader1 { get; set; }

      public Transform inputGridHeader2 { get; set; }

      public Transform inputGridHeader3 { get; set; }

      public Transform inputGridHeader4 { get; set; }

      public bool Check()
      {
        return !Object.op_Equality((Object) this._canvas, (Object) null) && !Object.op_Equality((Object) this._mainCanvasGroup, (Object) null) && (!Object.op_Equality((Object) this._mainContent, (Object) null) && !Object.op_Equality((Object) this._mainContentInner, (Object) null)) && (!Object.op_Equality((Object) this._playersGroup, (Object) null) && !Object.op_Equality((Object) this._controllerGroup, (Object) null) && (!Object.op_Equality((Object) this._controllerGroupLabelGroup, (Object) null) && !Object.op_Equality((Object) this._controllerSettingsGroup, (Object) null))) && (!Object.op_Equality((Object) this._assignedControllersGroup, (Object) null) && !Object.op_Equality((Object) this._settingsAndMapCategoriesGroup, (Object) null) && (!Object.op_Equality((Object) this._settingsGroup, (Object) null) && !Object.op_Equality((Object) this._mapCategoriesGroup, (Object) null)) && (!Object.op_Equality((Object) this._inputGridGroup, (Object) null) && !Object.op_Equality((Object) this._inputGridContainer, (Object) null) && (!Object.op_Equality((Object) this._inputGridHeadersGroup, (Object) null) && !Object.op_Equality((Object) this._inputGridVScrollbar, (Object) null)))) && (!Object.op_Equality((Object) this._inputGridScrollRect, (Object) null) && !Object.op_Equality((Object) this._inputGridInnerGroup, (Object) null) && (!Object.op_Equality((Object) this._controllerNameLabel, (Object) null) && !Object.op_Equality((Object) this._removeControllerButton, (Object) null)) && (!Object.op_Equality((Object) this._assignControllerButton, (Object) null) && !Object.op_Equality((Object) this._calibrateControllerButton, (Object) null) && (!Object.op_Equality((Object) this._doneButton, (Object) null) && !Object.op_Equality((Object) this._restoreDefaultsButton, (Object) null))) && !Object.op_Equality((Object) this._defaultSelection, (Object) null));
      }
    }

    private class InputActionSet
    {
      private int _actionId;
      private AxisRange _axisRange;

      public InputActionSet(int actionId, AxisRange axisRange)
      {
        this._actionId = actionId;
        this._axisRange = axisRange;
      }

      public int actionId
      {
        get
        {
          return this._actionId;
        }
      }

      public AxisRange axisRange
      {
        get
        {
          return this._axisRange;
        }
      }
    }

    private class InputMapping
    {
      public InputMapping(
        string actionName,
        InputFieldInfo fieldInfo,
        ControllerMap map,
        ActionElementMap aem,
        ControllerType controllerType,
        int controllerId)
      {
        this.actionName = actionName;
        this.fieldInfo = fieldInfo;
        this.map = map;
        this.aem = aem;
        this.controllerType = controllerType;
        this.controllerId = controllerId;
      }

      public string actionName { get; private set; }

      public InputFieldInfo fieldInfo { get; private set; }

      public ControllerMap map { get; private set; }

      public ActionElementMap aem { get; private set; }

      public ControllerType controllerType { get; private set; }

      public int controllerId { get; private set; }

      public ControllerPollingInfo pollingInfo { get; set; }

      public ModifierKeyFlags modifierKeyFlags { get; set; }

      public AxisRange axisRange
      {
        get
        {
          AxisRange axisRange = (AxisRange) 1;
          ControllerPollingInfo pollingInfo1 = this.pollingInfo;
          if (((ControllerPollingInfo) ref pollingInfo1).get_elementType() == null)
          {
            if (this.fieldInfo.axisRange == null)
            {
              axisRange = (AxisRange) 0;
            }
            else
            {
              ControllerPollingInfo pollingInfo2 = this.pollingInfo;
              axisRange = ((ControllerPollingInfo) ref pollingInfo2).get_axisPole() != null ? (AxisRange) 2 : (AxisRange) 1;
            }
          }
          return axisRange;
        }
      }

      public string elementName
      {
        get
        {
          if (this.controllerType == null && this.modifierKeyFlags != null)
          {
            string str = Keyboard.ModifierKeyFlagsToString(this.modifierKeyFlags);
            ControllerPollingInfo pollingInfo = this.pollingInfo;
            string elementIdentifierName = ((ControllerPollingInfo) ref pollingInfo).get_elementIdentifierName();
            return string.Format("{0} + {1}", (object) str, (object) elementIdentifierName);
          }
          ControllerPollingInfo pollingInfo1 = this.pollingInfo;
          string str1 = ((ControllerPollingInfo) ref pollingInfo1).get_elementIdentifierName();
          ControllerPollingInfo pollingInfo2 = this.pollingInfo;
          if (((ControllerPollingInfo) ref pollingInfo2).get_elementType() == null)
          {
            if (this.axisRange == 1)
            {
              ControllerPollingInfo pollingInfo3 = this.pollingInfo;
              str1 = ((ControllerPollingInfo) ref pollingInfo3).get_elementIdentifier().get_positiveName();
            }
            else if (this.axisRange == 2)
            {
              ControllerPollingInfo pollingInfo3 = this.pollingInfo;
              str1 = ((ControllerPollingInfo) ref pollingInfo3).get_elementIdentifier().get_negativeName();
            }
          }
          return str1;
        }
      }

      public ElementAssignment ToElementAssignment(
        ControllerPollingInfo pollingInfo)
      {
        this.pollingInfo = pollingInfo;
        return this.ToElementAssignment();
      }

      public ElementAssignment ToElementAssignment(
        ControllerPollingInfo pollingInfo,
        ModifierKeyFlags modifierKeyFlags)
      {
        this.pollingInfo = pollingInfo;
        this.modifierKeyFlags = modifierKeyFlags;
        return this.ToElementAssignment();
      }

      public ElementAssignment ToElementAssignment()
      {
        ControllerType controllerType = this.controllerType;
        ControllerPollingInfo pollingInfo1 = this.pollingInfo;
        ControllerElementType elementType = ((ControllerPollingInfo) ref pollingInfo1).get_elementType();
        ControllerPollingInfo pollingInfo2 = this.pollingInfo;
        int elementIdentifierId = ((ControllerPollingInfo) ref pollingInfo2).get_elementIdentifierId();
        AxisRange axisRange = this.axisRange;
        ControllerPollingInfo pollingInfo3 = this.pollingInfo;
        KeyCode keyboardKey = ((ControllerPollingInfo) ref pollingInfo3).get_keyboardKey();
        ModifierKeyFlags modifierKeyFlags = this.modifierKeyFlags;
        int actionId = this.fieldInfo.actionId;
        int num1 = this.fieldInfo.axisRange != 2 ? 0 : 1;
        int num2 = this.aem == null ? -1 : this.aem.get_id();
        return new ElementAssignment(controllerType, elementType, elementIdentifierId, axisRange, keyboardKey, modifierKeyFlags, actionId, (Pole) num1, false, num2);
      }
    }

    private class AxisCalibrator
    {
      public AxisCalibrationData data;
      public readonly Joystick joystick;
      public readonly int axisIndex;
      private Controller.Axis axis;
      private bool firstRun;

      public AxisCalibrator(Joystick joystick, int axisIndex)
      {
        this.data = (AxisCalibrationData) null;
        this.joystick = joystick;
        this.axisIndex = axisIndex;
        if (joystick != null && axisIndex >= 0 && ((ControllerWithAxes) joystick).get_axisCount() > axisIndex)
        {
          this.axis = ((ControllerWithAxes) joystick).get_Axes()[axisIndex];
          this.data = ((ControllerWithAxes) joystick).get_calibrationMap().GetAxis(axisIndex).GetData();
        }
        this.firstRun = true;
      }

      public bool isValid
      {
        get
        {
          return this.axis != null;
        }
      }

      public void RecordMinMax()
      {
        if (this.axis == null)
          return;
        float valueRaw = this.axis.get_valueRaw();
        if (this.firstRun || (double) valueRaw < this.data.min)
          this.data.min = (__Null) (double) valueRaw;
        if (this.firstRun || (double) valueRaw > this.data.max)
          this.data.max = (__Null) (double) valueRaw;
        this.firstRun = false;
      }

      public void RecordZero()
      {
        if (this.axis == null)
          return;
        this.data.zero = (__Null) (double) this.axis.get_valueRaw();
      }

      public void Commit()
      {
        if (this.axis == null)
          return;
        AxisCalibration axis = ((ControllerWithAxes) this.joystick).get_calibrationMap().GetAxis(this.axisIndex);
        if (axis == null || (double) Mathf.Abs((float) (this.data.max - this.data.min)) < 0.1)
          return;
        axis.SetData(this.data);
      }
    }

    private class IndexedDictionary<TKey, TValue>
    {
      private List<Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<TKey, TValue>.Entry> list;

      public IndexedDictionary()
      {
        this.list = new List<Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<TKey, TValue>.Entry>();
      }

      public int Count
      {
        get
        {
          return this.list.Count;
        }
      }

      public TValue this[int index]
      {
        get
        {
          return this.list[index].value;
        }
      }

      public TValue Get(TKey key)
      {
        int index = this.IndexOfKey(key);
        if (index < 0)
          throw new Exception("Key does not exist!");
        return this.list[index].value;
      }

      public bool TryGet(TKey key, out TValue value)
      {
        value = default (TValue);
        int index = this.IndexOfKey(key);
        if (index < 0)
          return false;
        value = this.list[index].value;
        return true;
      }

      public void Add(TKey key, TValue value)
      {
        if (this.ContainsKey(key))
          throw new Exception("Key " + key.ToString() + " is already in use!");
        this.list.Add(new Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<TKey, TValue>.Entry(key, value));
      }

      public int IndexOfKey(TKey key)
      {
        int count = this.list.Count;
        for (int index = 0; index < count; ++index)
        {
          if (EqualityComparer<TKey>.Default.Equals(this.list[index].key, key))
            return index;
        }
        return -1;
      }

      public bool ContainsKey(TKey key)
      {
        int count = this.list.Count;
        for (int index = 0; index < count; ++index)
        {
          if (EqualityComparer<TKey>.Default.Equals(this.list[index].key, key))
            return true;
        }
        return false;
      }

      public void Clear()
      {
        this.list.Clear();
      }

      private class Entry
      {
        public TKey key;
        public TValue value;

        public Entry(TKey key, TValue value)
        {
          this.key = key;
          this.value = value;
        }
      }
    }

    private enum LayoutElementSizeType
    {
      MinSize,
      PreferredSize,
    }

    private enum WindowType
    {
      None,
      ChooseJoystick,
      JoystickAssignmentConflict,
      ElementAssignment,
      ElementAssignmentPrePolling,
      ElementAssignmentPolling,
      ElementAssignmentResult,
      ElementAssignmentConflict,
      Calibration,
      CalibrateStep1,
      CalibrateStep2,
    }

    private class InputGrid
    {
      private Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList list;
      private List<GameObject> groups;

      public InputGrid()
      {
        this.list = new Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList();
        this.groups = new List<GameObject>();
      }

      public void AddMapCategory(int mapCategoryId)
      {
        this.list.AddMapCategory(mapCategoryId);
      }

      public void AddAction(int mapCategoryId, InputAction action, AxisRange axisRange)
      {
        this.list.AddAction(mapCategoryId, action, axisRange);
      }

      public void AddActionCategory(int mapCategoryId, int actionCategoryId)
      {
        this.list.AddActionCategory(mapCategoryId, actionCategoryId);
      }

      public void AddInputFieldSet(
        int mapCategoryId,
        InputAction action,
        AxisRange axisRange,
        ControllerType controllerType,
        GameObject fieldSetContainer)
      {
        this.list.AddInputFieldSet(mapCategoryId, action, axisRange, controllerType, fieldSetContainer);
      }

      public void AddInputField(
        int mapCategoryId,
        InputAction action,
        AxisRange axisRange,
        ControllerType controllerType,
        int fieldIndex,
        Rewired.UI.ControlMapper.ControlMapper.GUIInputField inputField)
      {
        this.list.AddInputField(mapCategoryId, action, axisRange, controllerType, fieldIndex, inputField);
      }

      public void AddGroup(GameObject group)
      {
        this.groups.Add(group);
      }

      public void AddActionLabel(
        int mapCategoryId,
        int actionId,
        AxisRange axisRange,
        Rewired.UI.ControlMapper.ControlMapper.GUILabel label)
      {
        this.list.AddActionLabel(mapCategoryId, actionId, axisRange, label);
      }

      public void AddActionCategoryLabel(
        int mapCategoryId,
        int actionCategoryId,
        Rewired.UI.ControlMapper.ControlMapper.GUILabel label)
      {
        this.list.AddActionCategoryLabel(mapCategoryId, actionCategoryId, label);
      }

      public bool Contains(
        int mapCategoryId,
        int actionId,
        AxisRange axisRange,
        ControllerType controllerType,
        int fieldIndex)
      {
        return this.list.Contains(mapCategoryId, actionId, axisRange, controllerType, fieldIndex);
      }

      public Rewired.UI.ControlMapper.ControlMapper.GUIInputField GetGUIInputField(
        int mapCategoryId,
        int actionId,
        AxisRange axisRange,
        ControllerType controllerType,
        int fieldIndex)
      {
        return this.list.GetGUIInputField(mapCategoryId, actionId, axisRange, controllerType, fieldIndex);
      }

      public IEnumerable<Rewired.UI.ControlMapper.ControlMapper.InputActionSet> GetActionSets(
        int mapCategoryId)
      {
        return this.list.GetActionSets(mapCategoryId);
      }

      public void SetColumnHeight(int mapCategoryId, float height)
      {
        this.list.SetColumnHeight(mapCategoryId, height);
      }

      public float GetColumnHeight(int mapCategoryId)
      {
        return this.list.GetColumnHeight(mapCategoryId);
      }

      public void SetFieldsActive(int mapCategoryId, bool state)
      {
        this.list.SetFieldsActive(mapCategoryId, state);
      }

      public void SetFieldLabel(
        int mapCategoryId,
        int actionId,
        AxisRange axisRange,
        ControllerType controllerType,
        int index,
        string label)
      {
        this.list.SetLabel(mapCategoryId, actionId, axisRange, controllerType, index, label);
      }

      public void PopulateField(
        int mapCategoryId,
        int actionId,
        AxisRange axisRange,
        ControllerType controllerType,
        int controllerId,
        int index,
        int actionElementMapId,
        string label,
        bool invert)
      {
        this.list.PopulateField(mapCategoryId, actionId, axisRange, controllerType, controllerId, index, actionElementMapId, label, invert);
      }

      public void SetFixedFieldData(
        int mapCategoryId,
        int actionId,
        AxisRange axisRange,
        ControllerType controllerType,
        int controllerId)
      {
        this.list.SetFixedFieldData(mapCategoryId, actionId, axisRange, controllerType, controllerId);
      }

      public void InitializeFields(int mapCategoryId)
      {
        this.list.InitializeFields(mapCategoryId);
      }

      public void Show(int mapCategoryId)
      {
        this.list.Show(mapCategoryId);
      }

      public void HideAll()
      {
        this.list.HideAll();
      }

      public void ClearLabels(int mapCategoryId)
      {
        this.list.ClearLabels(mapCategoryId);
      }

      private void ClearGroups()
      {
        for (int index = 0; index < this.groups.Count; ++index)
        {
          if (!Object.op_Equality((Object) this.groups[index], (Object) null))
            Object.Destroy((Object) this.groups[index]);
        }
      }

      public void ClearAll()
      {
        this.ClearGroups();
        this.list.Clear();
      }
    }

    private class InputGridEntryList
    {
      private Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<int, Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry> entries;

      public InputGridEntryList()
      {
        this.entries = new Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<int, Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry>();
      }

      public void AddMapCategory(int mapCategoryId)
      {
        if (mapCategoryId < 0 || this.entries.ContainsKey(mapCategoryId))
          return;
        this.entries.Add(mapCategoryId, new Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry());
      }

      public void AddAction(int mapCategoryId, InputAction action, AxisRange axisRange)
      {
        this.AddActionEntry(mapCategoryId, action, axisRange);
      }

      private Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry AddActionEntry(
        int mapCategoryId,
        InputAction action,
        AxisRange axisRange)
      {
        if (action == null)
          return (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry) null;
        Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
        return !this.entries.TryGet(mapCategoryId, out mapCategoryEntry) ? (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry) null : mapCategoryEntry.AddAction(action, axisRange);
      }

      public void AddActionLabel(
        int mapCategoryId,
        int actionId,
        AxisRange axisRange,
        Rewired.UI.ControlMapper.ControlMapper.GUILabel label)
      {
        Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
        if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
          return;
        mapCategoryEntry.GetActionEntry(actionId, axisRange)?.SetLabel(label);
      }

      public void AddActionCategory(int mapCategoryId, int actionCategoryId)
      {
        this.AddActionCategoryEntry(mapCategoryId, actionCategoryId);
      }

      private Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry AddActionCategoryEntry(
        int mapCategoryId,
        int actionCategoryId)
      {
        Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
        return !this.entries.TryGet(mapCategoryId, out mapCategoryEntry) ? (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry) null : mapCategoryEntry.AddActionCategory(actionCategoryId);
      }

      public void AddActionCategoryLabel(
        int mapCategoryId,
        int actionCategoryId,
        Rewired.UI.ControlMapper.ControlMapper.GUILabel label)
      {
        Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
        if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
          return;
        mapCategoryEntry.GetActionCategoryEntry(actionCategoryId)?.SetLabel(label);
      }

      public void AddInputFieldSet(
        int mapCategoryId,
        InputAction action,
        AxisRange axisRange,
        ControllerType controllerType,
        GameObject fieldSetContainer)
      {
        this.GetActionEntry(mapCategoryId, action, axisRange)?.AddInputFieldSet(controllerType, fieldSetContainer);
      }

      public void AddInputField(
        int mapCategoryId,
        InputAction action,
        AxisRange axisRange,
        ControllerType controllerType,
        int fieldIndex,
        Rewired.UI.ControlMapper.ControlMapper.GUIInputField inputField)
      {
        this.GetActionEntry(mapCategoryId, action, axisRange)?.AddInputField(controllerType, fieldIndex, inputField);
      }

      public bool Contains(int mapCategoryId, int actionId, AxisRange axisRange)
      {
        return this.GetActionEntry(mapCategoryId, actionId, axisRange) != null;
      }

      public bool Contains(
        int mapCategoryId,
        int actionId,
        AxisRange axisRange,
        ControllerType controllerType,
        int fieldIndex)
      {
        Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
        return actionEntry != null && actionEntry.Contains(controllerType, fieldIndex);
      }

      public void SetColumnHeight(int mapCategoryId, float height)
      {
        Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
        if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
          return;
        mapCategoryEntry.columnHeight = height;
      }

      public float GetColumnHeight(int mapCategoryId)
      {
        Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
        return !this.entries.TryGet(mapCategoryId, out mapCategoryEntry) ? 0.0f : mapCategoryEntry.columnHeight;
      }

      public Rewired.UI.ControlMapper.ControlMapper.GUIInputField GetGUIInputField(
        int mapCategoryId,
        int actionId,
        AxisRange axisRange,
        ControllerType controllerType,
        int fieldIndex)
      {
        return this.GetActionEntry(mapCategoryId, actionId, axisRange)?.GetGUIInputField(controllerType, fieldIndex);
      }

      private Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry GetActionEntry(
        int mapCategoryId,
        int actionId,
        AxisRange axisRange)
      {
        if (actionId < 0)
          return (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry) null;
        Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
        return !this.entries.TryGet(mapCategoryId, out mapCategoryEntry) ? (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry) null : mapCategoryEntry.GetActionEntry(actionId, axisRange);
      }

      private Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry GetActionEntry(
        int mapCategoryId,
        InputAction action,
        AxisRange axisRange)
      {
        return action == null ? (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry) null : this.GetActionEntry(mapCategoryId, action.get_id(), axisRange);
      }

      [DebuggerHidden]
      public IEnumerable<Rewired.UI.ControlMapper.ControlMapper.InputActionSet> GetActionSets(
        int mapCategoryId)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.\u003CGetActionSets\u003Ec__Iterator0 actionSetsCIterator0 = new Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.\u003CGetActionSets\u003Ec__Iterator0()
        {
          mapCategoryId = mapCategoryId,
          \u0024this = this
        };
        // ISSUE: reference to a compiler-generated field
        actionSetsCIterator0.\u0024PC = -2;
        return (IEnumerable<Rewired.UI.ControlMapper.ControlMapper.InputActionSet>) actionSetsCIterator0;
      }

      public void SetFieldsActive(int mapCategoryId, bool state)
      {
        Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
        if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
          return;
        List<Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry> actionList = mapCategoryEntry.actionList;
        int num = actionList == null ? 0 : actionList.Count;
        for (int index = 0; index < num; ++index)
          actionList[index].SetFieldsActive(state);
      }

      public void SetLabel(
        int mapCategoryId,
        int actionId,
        AxisRange axisRange,
        ControllerType controllerType,
        int index,
        string label)
      {
        this.GetActionEntry(mapCategoryId, actionId, axisRange)?.SetFieldLabel(controllerType, index, label);
      }

      public void PopulateField(
        int mapCategoryId,
        int actionId,
        AxisRange axisRange,
        ControllerType controllerType,
        int controllerId,
        int index,
        int actionElementMapId,
        string label,
        bool invert)
      {
        this.GetActionEntry(mapCategoryId, actionId, axisRange)?.PopulateField(controllerType, controllerId, index, actionElementMapId, label, invert);
      }

      public void SetFixedFieldData(
        int mapCategoryId,
        int actionId,
        AxisRange axisRange,
        ControllerType controllerType,
        int controllerId)
      {
        this.GetActionEntry(mapCategoryId, actionId, axisRange)?.SetFixedFieldData(controllerType, controllerId);
      }

      public void InitializeFields(int mapCategoryId)
      {
        Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
        if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
          return;
        List<Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry> actionList = mapCategoryEntry.actionList;
        int num = actionList == null ? 0 : actionList.Count;
        for (int index = 0; index < num; ++index)
          actionList[index].Initialize();
      }

      public void Show(int mapCategoryId)
      {
        Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
        if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
          return;
        mapCategoryEntry.SetAllActive(true);
      }

      public void HideAll()
      {
        for (int index = 0; index < this.entries.Count; ++index)
          this.entries[index].SetAllActive(false);
      }

      public void ClearLabels(int mapCategoryId)
      {
        Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
        if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
          return;
        List<Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry> actionList = mapCategoryEntry.actionList;
        int num = actionList == null ? 0 : actionList.Count;
        for (int index = 0; index < num; ++index)
          actionList[index].ClearLabels();
      }

      public void Clear()
      {
        this.entries.Clear();
      }

      private class MapCategoryEntry
      {
        private List<Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry> _actionList;
        private Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<int, Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry> _actionCategoryList;
        private float _columnHeight;

        public MapCategoryEntry()
        {
          this._actionList = new List<Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry>();
          this._actionCategoryList = new Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<int, Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry>();
        }

        public List<Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry> actionList
        {
          get
          {
            return this._actionList;
          }
        }

        public Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<int, Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry> actionCategoryList
        {
          get
          {
            return this._actionCategoryList;
          }
        }

        public float columnHeight
        {
          get
          {
            return this._columnHeight;
          }
          set
          {
            this._columnHeight = value;
          }
        }

        public Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry GetActionEntry(
          int actionId,
          AxisRange axisRange)
        {
          int index = this.IndexOfActionEntry(actionId, axisRange);
          return index < 0 ? (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry) null : this._actionList[index];
        }

        public int IndexOfActionEntry(int actionId, AxisRange axisRange)
        {
          int count = this._actionList.Count;
          for (int index = 0; index < count; ++index)
          {
            if (this._actionList[index].Matches(actionId, axisRange))
              return index;
          }
          return -1;
        }

        public bool ContainsActionEntry(int actionId, AxisRange axisRange)
        {
          return this.IndexOfActionEntry(actionId, axisRange) >= 0;
        }

        public Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry AddAction(
          InputAction action,
          AxisRange axisRange)
        {
          if (action == null)
            return (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry) null;
          if (this.ContainsActionEntry(action.get_id(), axisRange))
            return (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry) null;
          this._actionList.Add(new Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry(action, axisRange));
          return this._actionList[this._actionList.Count - 1];
        }

        public Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry GetActionCategoryEntry(
          int actionCategoryId)
        {
          return !this._actionCategoryList.ContainsKey(actionCategoryId) ? (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry) null : this._actionCategoryList.Get(actionCategoryId);
        }

        public Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry AddActionCategory(
          int actionCategoryId)
        {
          if (actionCategoryId < 0)
            return (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry) null;
          if (this._actionCategoryList.ContainsKey(actionCategoryId))
            return (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry) null;
          this._actionCategoryList.Add(actionCategoryId, new Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry(actionCategoryId));
          return this._actionCategoryList.Get(actionCategoryId);
        }

        public void SetAllActive(bool state)
        {
          for (int index = 0; index < this._actionCategoryList.Count; ++index)
            this._actionCategoryList[index].SetActive(state);
          for (int index = 0; index < this._actionList.Count; ++index)
            this._actionList[index].SetActive(state);
        }
      }

      private class ActionEntry
      {
        private Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<int, Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.FieldSet> fieldSets;
        public Rewired.UI.ControlMapper.ControlMapper.GUILabel label;
        public readonly InputAction action;
        public readonly AxisRange axisRange;
        public readonly Rewired.UI.ControlMapper.ControlMapper.InputActionSet actionSet;

        public ActionEntry(InputAction action, AxisRange axisRange)
        {
          this.action = action;
          this.axisRange = axisRange;
          this.actionSet = new Rewired.UI.ControlMapper.ControlMapper.InputActionSet(action.get_id(), axisRange);
          this.fieldSets = new Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<int, Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.FieldSet>();
        }

        public void SetLabel(Rewired.UI.ControlMapper.ControlMapper.GUILabel label)
        {
          this.label = label;
        }

        public bool Matches(int actionId, AxisRange axisRange)
        {
          return this.action.get_id() == actionId && this.axisRange == axisRange;
        }

        public void AddInputFieldSet(ControllerType controllerType, GameObject fieldSetContainer)
        {
          if (this.fieldSets.ContainsKey((int) controllerType))
            return;
          this.fieldSets.Add((int) controllerType, new Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.FieldSet(fieldSetContainer));
        }

        public void AddInputField(
          ControllerType controllerType,
          int fieldIndex,
          Rewired.UI.ControlMapper.ControlMapper.GUIInputField inputField)
        {
          if (!this.fieldSets.ContainsKey((int) controllerType))
            return;
          Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets.Get((int) controllerType);
          if (fieldSet.fields.ContainsKey(fieldIndex))
            return;
          fieldSet.fields.Add(fieldIndex, inputField);
        }

        public Rewired.UI.ControlMapper.ControlMapper.GUIInputField GetGUIInputField(
          ControllerType controllerType,
          int fieldIndex)
        {
          if (!this.fieldSets.ContainsKey((int) controllerType))
            return (Rewired.UI.ControlMapper.ControlMapper.GUIInputField) null;
          return !this.fieldSets.Get((int) controllerType).fields.ContainsKey(fieldIndex) ? (Rewired.UI.ControlMapper.ControlMapper.GUIInputField) null : this.fieldSets.Get((int) controllerType).fields.Get(fieldIndex);
        }

        public bool Contains(ControllerType controllerType, int fieldId)
        {
          return this.fieldSets.ContainsKey((int) controllerType) && this.fieldSets.Get((int) controllerType).fields.ContainsKey(fieldId);
        }

        public void SetFieldLabel(ControllerType controllerType, int index, string label)
        {
          if (!this.fieldSets.ContainsKey((int) controllerType) || !this.fieldSets.Get((int) controllerType).fields.ContainsKey(index))
            return;
          this.fieldSets.Get((int) controllerType).fields.Get(index).SetLabel(label);
        }

        public void PopulateField(
          ControllerType controllerType,
          int controllerId,
          int index,
          int actionElementMapId,
          string label,
          bool invert)
        {
          if (!this.fieldSets.ContainsKey((int) controllerType) || !this.fieldSets.Get((int) controllerType).fields.ContainsKey(index))
            return;
          Rewired.UI.ControlMapper.ControlMapper.GUIInputField guiInputField = this.fieldSets.Get((int) controllerType).fields.Get(index);
          guiInputField.SetLabel(label);
          guiInputField.actionElementMapId = actionElementMapId;
          guiInputField.controllerId = controllerId;
          if (!guiInputField.hasToggle)
            return;
          guiInputField.toggle.SetInteractible(true, false);
          guiInputField.toggle.SetToggleState(invert);
          guiInputField.toggle.actionElementMapId = actionElementMapId;
        }

        public void SetFixedFieldData(ControllerType controllerType, int controllerId)
        {
          if (!this.fieldSets.ContainsKey((int) controllerType))
            return;
          Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets.Get((int) controllerType);
          int count = fieldSet.fields.Count;
          for (int index = 0; index < count; ++index)
            fieldSet.fields[index].controllerId = controllerId;
        }

        public void Initialize()
        {
          for (int index1 = 0; index1 < this.fieldSets.Count; ++index1)
          {
            Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets[index1];
            int count = fieldSet.fields.Count;
            for (int index2 = 0; index2 < count; ++index2)
            {
              Rewired.UI.ControlMapper.ControlMapper.GUIInputField field = fieldSet.fields[index2];
              if (field.hasToggle)
              {
                field.toggle.SetInteractible(false, false);
                field.toggle.SetToggleState(false);
                field.toggle.actionElementMapId = -1;
              }
              field.SetLabel(string.Empty);
              field.actionElementMapId = -1;
              field.controllerId = -1;
            }
          }
        }

        public void SetActive(bool state)
        {
          if (this.label != null)
            this.label.SetActive(state);
          int count = this.fieldSets.Count;
          for (int index = 0; index < count; ++index)
            this.fieldSets[index].groupContainer.SetActive(state);
        }

        public void ClearLabels()
        {
          for (int index1 = 0; index1 < this.fieldSets.Count; ++index1)
          {
            Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets[index1];
            int count = fieldSet.fields.Count;
            for (int index2 = 0; index2 < count; ++index2)
              fieldSet.fields[index2].SetLabel(string.Empty);
          }
        }

        public void SetFieldsActive(bool state)
        {
          for (int index1 = 0; index1 < this.fieldSets.Count; ++index1)
          {
            Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets[index1];
            int count = fieldSet.fields.Count;
            for (int index2 = 0; index2 < count; ++index2)
            {
              Rewired.UI.ControlMapper.ControlMapper.GUIInputField field = fieldSet.fields[index2];
              field.SetInteractible(state, false);
              if (field.hasToggle && (!state || field.toggle.actionElementMapId >= 0))
                field.toggle.SetInteractible(state, false);
            }
          }
        }
      }

      private class FieldSet
      {
        public readonly GameObject groupContainer;
        public readonly Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<int, Rewired.UI.ControlMapper.ControlMapper.GUIInputField> fields;

        public FieldSet(GameObject groupContainer)
        {
          this.groupContainer = groupContainer;
          this.fields = new Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<int, Rewired.UI.ControlMapper.ControlMapper.GUIInputField>();
        }
      }

      private class ActionCategoryEntry
      {
        public readonly int actionCategoryId;
        public Rewired.UI.ControlMapper.ControlMapper.GUILabel label;

        public ActionCategoryEntry(int actionCategoryId)
        {
          this.actionCategoryId = actionCategoryId;
        }

        public void SetLabel(Rewired.UI.ControlMapper.ControlMapper.GUILabel label)
        {
          this.label = label;
        }

        public void SetActive(bool state)
        {
          if (this.label == null)
            return;
          this.label.SetActive(state);
        }
      }
    }

    private class WindowManager
    {
      private List<Window> windows;
      private GameObject windowPrefab;
      private Transform parent;
      private GameObject fader;
      private int idCounter;

      public WindowManager(GameObject windowPrefab, GameObject faderPrefab, Transform parent)
      {
        this.windowPrefab = windowPrefab;
        this.parent = parent;
        this.windows = new List<Window>();
        this.fader = (GameObject) Object.Instantiate<GameObject>((M0) faderPrefab);
        this.fader.get_transform().SetParent(parent, false);
        ((Transform) this.fader.GetComponent<RectTransform>()).set_localScale(Vector2.op_Implicit(Vector2.get_one()));
        this.SetFaderActive(false);
      }

      public bool isWindowOpen
      {
        get
        {
          for (int index = this.windows.Count - 1; index >= 0; --index)
          {
            if (!Object.op_Equality((Object) this.windows[index], (Object) null))
              return true;
          }
          return false;
        }
      }

      public Window topWindow
      {
        get
        {
          for (int index = this.windows.Count - 1; index >= 0; --index)
          {
            if (!Object.op_Equality((Object) this.windows[index], (Object) null))
              return this.windows[index];
          }
          return (Window) null;
        }
      }

      public Window OpenWindow(string name, int width, int height)
      {
        Window window = this.InstantiateWindow(name, width, height);
        this.UpdateFader();
        return window;
      }

      public Window OpenWindow(GameObject windowPrefab, string name)
      {
        if (Object.op_Equality((Object) windowPrefab, (Object) null))
        {
          Debug.LogError((object) "Rewired Control Mapper: Window Prefab is null!");
          return (Window) null;
        }
        Window window = this.InstantiateWindow(name, windowPrefab);
        this.UpdateFader();
        return window;
      }

      public void CloseTop()
      {
        for (int index = this.windows.Count - 1; index >= 0; --index)
        {
          if (Object.op_Equality((Object) this.windows[index], (Object) null))
          {
            this.windows.RemoveAt(index);
          }
          else
          {
            this.DestroyWindow(this.windows[index]);
            this.windows.RemoveAt(index);
            break;
          }
        }
        this.UpdateFader();
      }

      public void CloseWindow(int windowId)
      {
        this.CloseWindow(this.GetWindow(windowId));
      }

      public void CloseWindow(Window window)
      {
        if (Object.op_Equality((Object) window, (Object) null))
          return;
        for (int index = this.windows.Count - 1; index >= 0; --index)
        {
          if (Object.op_Equality((Object) this.windows[index], (Object) null))
            this.windows.RemoveAt(index);
          else if (!Object.op_Inequality((Object) this.windows[index], (Object) window))
          {
            this.DestroyWindow(this.windows[index]);
            this.windows.RemoveAt(index);
            break;
          }
        }
        this.UpdateFader();
        this.FocusTopWindow();
      }

      public void CloseAll()
      {
        this.SetFaderActive(false);
        for (int index = this.windows.Count - 1; index >= 0; --index)
        {
          if (Object.op_Equality((Object) this.windows[index], (Object) null))
          {
            this.windows.RemoveAt(index);
          }
          else
          {
            this.DestroyWindow(this.windows[index]);
            this.windows.RemoveAt(index);
          }
        }
        this.UpdateFader();
      }

      public void CancelAll()
      {
        if (!this.isWindowOpen)
          return;
        for (int index = this.windows.Count - 1; index >= 0; --index)
        {
          if (!Object.op_Equality((Object) this.windows[index], (Object) null))
            this.windows[index].Cancel();
        }
        this.CloseAll();
      }

      public Window GetWindow(int windowId)
      {
        if (windowId < 0)
          return (Window) null;
        for (int index = this.windows.Count - 1; index >= 0; --index)
        {
          if (!Object.op_Equality((Object) this.windows[index], (Object) null) && this.windows[index].id == windowId)
            return this.windows[index];
        }
        return (Window) null;
      }

      public bool IsFocused(int windowId)
      {
        return windowId >= 0 && !Object.op_Equality((Object) this.topWindow, (Object) null) && this.topWindow.id == windowId;
      }

      public void Focus(int windowId)
      {
        this.Focus(this.GetWindow(windowId));
      }

      public void Focus(Window window)
      {
        if (Object.op_Equality((Object) window, (Object) null))
          return;
        window.TakeInputFocus();
        this.DefocusOtherWindows(window.id);
      }

      private void DefocusOtherWindows(int focusedWindowId)
      {
        if (focusedWindowId < 0)
          return;
        for (int index = this.windows.Count - 1; index >= 0; --index)
        {
          if (!Object.op_Equality((Object) this.windows[index], (Object) null) && this.windows[index].id != focusedWindowId)
            this.windows[index].Disable();
        }
      }

      private void UpdateFader()
      {
        if (!this.isWindowOpen)
        {
          this.SetFaderActive(false);
        }
        else
        {
          if (Object.op_Equality((Object) ((Component) this.topWindow).get_transform().get_parent(), (Object) null))
            return;
          this.SetFaderActive(true);
          this.fader.get_transform().SetAsLastSibling();
          this.fader.get_transform().SetSiblingIndex(((Component) this.topWindow).get_transform().GetSiblingIndex());
        }
      }

      private void FocusTopWindow()
      {
        if (Object.op_Equality((Object) this.topWindow, (Object) null))
          return;
        this.topWindow.TakeInputFocus();
      }

      private void SetFaderActive(bool state)
      {
        this.fader.SetActive(state);
      }

      private Window InstantiateWindow(string name, int width, int height)
      {
        if (string.IsNullOrEmpty(name))
          name = "Window";
        GameObject gameObject = UITools.InstantiateGUIObject<Window>(this.windowPrefab, this.parent, name);
        if (Object.op_Equality((Object) gameObject, (Object) null))
          return (Window) null;
        Window component = (Window) gameObject.GetComponent<Window>();
        if (Object.op_Inequality((Object) component, (Object) null))
        {
          component.Initialize(this.GetNewId(), new Func<int, bool>(this.IsFocused));
          this.windows.Add(component);
          component.SetSize(width, height);
        }
        return component;
      }

      private Window InstantiateWindow(string name, GameObject windowPrefab)
      {
        if (string.IsNullOrEmpty(name))
          name = "Window";
        if (Object.op_Equality((Object) windowPrefab, (Object) null))
          return (Window) null;
        GameObject gameObject = UITools.InstantiateGUIObject<Window>(windowPrefab, this.parent, name);
        if (Object.op_Equality((Object) gameObject, (Object) null))
          return (Window) null;
        Window component = (Window) gameObject.GetComponent<Window>();
        if (Object.op_Inequality((Object) component, (Object) null))
        {
          component.Initialize(this.GetNewId(), new Func<int, bool>(this.IsFocused));
          this.windows.Add(component);
        }
        return component;
      }

      private void DestroyWindow(Window window)
      {
        if (Object.op_Equality((Object) window, (Object) null))
          return;
        Object.Destroy((Object) ((Component) window).get_gameObject());
      }

      private int GetNewId()
      {
        int idCounter = this.idCounter;
        ++this.idCounter;
        return idCounter;
      }

      public void ClearCompletely()
      {
        this.CloseAll();
        if (!Object.op_Inequality((Object) this.fader, (Object) null))
          return;
        Object.Destroy((Object) this.fader);
      }
    }
  }
}
