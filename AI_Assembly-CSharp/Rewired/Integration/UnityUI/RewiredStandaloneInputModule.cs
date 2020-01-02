// Decompiled with JetBrains decompiler
// Type: Rewired.Integration.UnityUI.RewiredStandaloneInputModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Rewired.Components;
using Rewired.UI;
using Rewired.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Rewired.Integration.UnityUI
{
  [AddComponentMenu("Event/Rewired Standalone Input Module")]
  public sealed class RewiredStandaloneInputModule : RewiredPointerInputModule
  {
    [SerializeField]
    [Tooltip("A list of Player Ids that are allowed to control the UI. If Use All Rewired Game Players = True, this list will be ignored.")]
    private int[] rewiredPlayerIds = new int[1];
    [SerializeField]
    [Tooltip("Player Mice allowed to interact with the UI. Each Player that owns a Player Mouse must also be allowed to control the UI or the Player Mouse will not function.")]
    private List<PlayerMouse> playerMice = new List<PlayerMouse>();
    [SerializeField]
    [Tooltip("Id of the horizontal Action for movement (if axis events are used).")]
    private int horizontalActionId = -1;
    [SerializeField]
    [Tooltip("Id of the vertical Action for movement (if axis events are used).")]
    private int verticalActionId = -1;
    [SerializeField]
    [Tooltip("Id of the Action used to submit.")]
    private int submitActionId = -1;
    [SerializeField]
    [Tooltip("Id of the Action used to cancel.")]
    private int cancelActionId = -1;
    [SerializeField]
    [Tooltip("Name of the horizontal axis for movement (if axis events are used).")]
    private string m_HorizontalAxis = "UIHorizontal";
    [SerializeField]
    [Tooltip("Name of the vertical axis for movement (if axis events are used).")]
    private string m_VerticalAxis = "UIVertical";
    [SerializeField]
    [Tooltip("Name of the action used to submit.")]
    private string m_SubmitButton = "UISubmit";
    [SerializeField]
    [Tooltip("Name of the action used to cancel.")]
    private string m_CancelButton = "UICancel";
    [SerializeField]
    [Tooltip("Number of selection changes allowed per second when a movement button/axis is held in a direction.")]
    private float m_InputActionsPerSecond = 10f;
    [SerializeField]
    [Tooltip("Allows the mouse to be used to select elements.")]
    private bool m_allowMouseInput = true;
    [SerializeField]
    [Tooltip("Allows the mouse to be used to select elements if the device also supports touch control.")]
    private bool m_allowMouseInputIfTouchSupported = true;
    [SerializeField]
    [Tooltip("Allows touch input to be used to select elements.")]
    private bool m_allowTouchInput = true;
    [NonSerialized]
    private bool m_HasFocus = true;
    private const string DEFAULT_ACTION_MOVE_HORIZONTAL = "UIHorizontal";
    private const string DEFAULT_ACTION_MOVE_VERTICAL = "UIVertical";
    private const string DEFAULT_ACTION_SUBMIT = "UISubmit";
    private const string DEFAULT_ACTION_CANCEL = "UICancel";
    [Tooltip("(Optional) Link the Rewired Input Manager here for easier access to Player ids, etc.")]
    [SerializeField]
    private InputManager_Base rewiredInputManager;
    [SerializeField]
    [Tooltip("Use all Rewired game Players to control the UI. This does not include the System Player. If enabled, this setting overrides individual Player Ids set in Rewired Player Ids.")]
    private bool useAllRewiredGamePlayers;
    [SerializeField]
    [Tooltip("Allow the Rewired System Player to control the UI.")]
    private bool useRewiredSystemPlayer;
    [SerializeField]
    [Tooltip("Allow only Players with Player.isPlaying = true to control the UI.")]
    private bool usePlayingPlayersOnly;
    [SerializeField]
    [Tooltip("Makes an axis press always move only one UI selection. Enable if you do not want to allow scrolling through UI elements by holding an axis direction.")]
    private bool moveOneElementPerAxisPress;
    [SerializeField]
    [Tooltip("If enabled, Action Ids will be used to set the Actions. If disabled, string names will be used to set the Actions.")]
    private bool setActionsById;
    [SerializeField]
    [Tooltip("Delay in seconds before vertical/horizontal movement starts repeating continouously when a movement direction is held.")]
    private float m_RepeatDelay;
    [SerializeField]
    [FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
    [Tooltip("Forces the module to always be active.")]
    private bool m_ForceModuleActive;
    [NonSerialized]
    private int[] playerIds;
    private bool recompiling;
    [NonSerialized]
    private bool isTouchSupported;
    [NonSerialized]
    private float m_PrevActionTime;
    [NonSerialized]
    private Vector2 m_LastMoveVector;
    [NonSerialized]
    private int m_ConsecutiveMoveCount;

    private RewiredStandaloneInputModule()
    {
    }

    public InputManager_Base RewiredInputManager
    {
      get
      {
        return this.rewiredInputManager;
      }
      set
      {
        this.rewiredInputManager = value;
      }
    }

    public bool UseAllRewiredGamePlayers
    {
      get
      {
        return this.useAllRewiredGamePlayers;
      }
      set
      {
        bool flag = value != this.useAllRewiredGamePlayers;
        this.useAllRewiredGamePlayers = value;
        if (!flag)
          return;
        this.SetupRewiredVars();
      }
    }

    public bool UseRewiredSystemPlayer
    {
      get
      {
        return this.useRewiredSystemPlayer;
      }
      set
      {
        bool flag = value != this.useRewiredSystemPlayer;
        this.useRewiredSystemPlayer = value;
        if (!flag)
          return;
        this.SetupRewiredVars();
      }
    }

    public int[] RewiredPlayerIds
    {
      get
      {
        return (int[]) this.rewiredPlayerIds.Clone();
      }
      set
      {
        this.rewiredPlayerIds = value == null ? new int[0] : (int[]) value.Clone();
        this.SetupRewiredVars();
      }
    }

    public bool UsePlayingPlayersOnly
    {
      get
      {
        return this.usePlayingPlayersOnly;
      }
      set
      {
        this.usePlayingPlayersOnly = value;
      }
    }

    public List<PlayerMouse> PlayerMice
    {
      get
      {
        return new List<PlayerMouse>((IEnumerable<PlayerMouse>) this.playerMice);
      }
      set
      {
        if (value == null)
        {
          this.playerMice = new List<PlayerMouse>();
          this.SetupRewiredVars();
        }
        else
        {
          this.playerMice = new List<PlayerMouse>((IEnumerable<PlayerMouse>) value);
          this.SetupRewiredVars();
        }
      }
    }

    public bool MoveOneElementPerAxisPress
    {
      get
      {
        return this.moveOneElementPerAxisPress;
      }
      set
      {
        this.moveOneElementPerAxisPress = value;
      }
    }

    public bool allowMouseInput
    {
      get
      {
        return this.m_allowMouseInput;
      }
      set
      {
        this.m_allowMouseInput = value;
      }
    }

    public bool allowMouseInputIfTouchSupported
    {
      get
      {
        return this.m_allowMouseInputIfTouchSupported;
      }
      set
      {
        this.m_allowMouseInputIfTouchSupported = value;
      }
    }

    public bool allowTouchInput
    {
      get
      {
        return this.m_allowTouchInput;
      }
      set
      {
        this.m_allowTouchInput = value;
      }
    }

    public bool SetActionsById
    {
      get
      {
        return this.setActionsById;
      }
      set
      {
        if (this.setActionsById == value)
          return;
        this.setActionsById = value;
        this.SetupRewiredVars();
      }
    }

    public int HorizontalActionId
    {
      get
      {
        return this.horizontalActionId;
      }
      set
      {
        if (value == this.horizontalActionId)
          return;
        this.horizontalActionId = value;
        if (!ReInput.get_isReady())
          return;
        this.m_HorizontalAxis = ReInput.get_mapping().GetAction(value) == null ? string.Empty : ReInput.get_mapping().GetAction(value).get_name();
      }
    }

    public int VerticalActionId
    {
      get
      {
        return this.verticalActionId;
      }
      set
      {
        if (value == this.verticalActionId)
          return;
        this.verticalActionId = value;
        if (!ReInput.get_isReady())
          return;
        this.m_VerticalAxis = ReInput.get_mapping().GetAction(value) == null ? string.Empty : ReInput.get_mapping().GetAction(value).get_name();
      }
    }

    public int SubmitActionId
    {
      get
      {
        return this.submitActionId;
      }
      set
      {
        if (value == this.submitActionId)
          return;
        this.submitActionId = value;
        if (!ReInput.get_isReady())
          return;
        this.m_SubmitButton = ReInput.get_mapping().GetAction(value) == null ? string.Empty : ReInput.get_mapping().GetAction(value).get_name();
      }
    }

    public int CancelActionId
    {
      get
      {
        return this.cancelActionId;
      }
      set
      {
        if (value == this.cancelActionId)
          return;
        this.cancelActionId = value;
        if (!ReInput.get_isReady())
          return;
        this.m_CancelButton = ReInput.get_mapping().GetAction(value) == null ? string.Empty : ReInput.get_mapping().GetAction(value).get_name();
      }
    }

    protected override bool isMouseSupported
    {
      get
      {
        if (!base.isMouseSupported || !this.m_allowMouseInput)
          return false;
        return !this.isTouchSupported || this.m_allowMouseInputIfTouchSupported;
      }
    }

    private bool isTouchAllowed
    {
      get
      {
        return this.isTouchSupported && this.m_allowTouchInput;
      }
    }

    [Obsolete("allowActivationOnMobileDevice has been deprecated. Use forceModuleActive instead")]
    public bool allowActivationOnMobileDevice
    {
      get
      {
        return this.m_ForceModuleActive;
      }
      set
      {
        this.m_ForceModuleActive = value;
      }
    }

    public bool forceModuleActive
    {
      get
      {
        return this.m_ForceModuleActive;
      }
      set
      {
        this.m_ForceModuleActive = value;
      }
    }

    public float inputActionsPerSecond
    {
      get
      {
        return this.m_InputActionsPerSecond;
      }
      set
      {
        this.m_InputActionsPerSecond = value;
      }
    }

    public float repeatDelay
    {
      get
      {
        return this.m_RepeatDelay;
      }
      set
      {
        this.m_RepeatDelay = value;
      }
    }

    public string horizontalAxis
    {
      get
      {
        return this.m_HorizontalAxis;
      }
      set
      {
        if (this.m_HorizontalAxis == value)
          return;
        this.m_HorizontalAxis = value;
        if (!ReInput.get_isReady())
          return;
        this.horizontalActionId = ReInput.get_mapping().GetActionId(value);
      }
    }

    public string verticalAxis
    {
      get
      {
        return this.m_VerticalAxis;
      }
      set
      {
        if (this.m_VerticalAxis == value)
          return;
        this.m_VerticalAxis = value;
        if (!ReInput.get_isReady())
          return;
        this.verticalActionId = ReInput.get_mapping().GetActionId(value);
      }
    }

    public string submitButton
    {
      get
      {
        return this.m_SubmitButton;
      }
      set
      {
        if (this.m_SubmitButton == value)
          return;
        this.m_SubmitButton = value;
        if (!ReInput.get_isReady())
          return;
        this.submitActionId = ReInput.get_mapping().GetActionId(value);
      }
    }

    public string cancelButton
    {
      get
      {
        return this.m_CancelButton;
      }
      set
      {
        if (this.m_CancelButton == value)
          return;
        this.m_CancelButton = value;
        if (!ReInput.get_isReady())
          return;
        this.cancelActionId = ReInput.get_mapping().GetActionId(value);
      }
    }

    protected virtual void Awake()
    {
      ((UIBehaviour) this).Awake();
      this.isTouchSupported = this.defaultTouchInputSource.get_touchSupported();
      TouchInputModule component = (TouchInputModule) ((Component) this).GetComponent<TouchInputModule>();
      if (Object.op_Inequality((Object) component, (Object) null))
        ((Behaviour) component).set_enabled(false);
      ReInput.add_InitializedEvent(new Action(this.OnRewiredInitialized));
      this.InitializeRewired();
    }

    public virtual void UpdateModule()
    {
      this.CheckEditorRecompile();
      if (this.recompiling || !ReInput.get_isReady() || (this.m_HasFocus || !this.ShouldIgnoreEventsOnNoFocus()))
        ;
    }

    public virtual bool IsModuleSupported()
    {
      return true;
    }

    public virtual bool ShouldActivateModule()
    {
      if (!base.ShouldActivateModule() || this.recompiling || !ReInput.get_isReady())
        return false;
      bool flag1 = this.m_ForceModuleActive;
      for (int index = 0; index < this.playerIds.Length; ++index)
      {
        Player player = ReInput.get_players().GetPlayer(this.playerIds[index]);
        if (player != null && (!this.usePlayingPlayersOnly || player.get_isPlaying()))
        {
          bool flag2 = flag1 | this.GetButtonDown(player, this.submitActionId) | this.GetButtonDown(player, this.cancelActionId);
          flag1 = !this.moveOneElementPerAxisPress ? flag2 | !Mathf.Approximately(this.GetAxis(player, this.horizontalActionId), 0.0f) | !Mathf.Approximately(this.GetAxis(player, this.verticalActionId), 0.0f) : ((((flag2 ? 1 : 0) | (this.GetButtonDown(player, this.horizontalActionId) ? 1 : (this.GetNegativeButtonDown(player, this.horizontalActionId) ? 1 : 0))) != 0 ? 1 : 0) | (this.GetButtonDown(player, this.verticalActionId) ? 1 : (this.GetNegativeButtonDown(player, this.verticalActionId) ? 1 : 0))) != 0;
        }
      }
      if (this.isMouseSupported)
        flag1 = flag1 | this.DidAnyMouseMove() | this.GetMouseButtonDownOnAnyMouse(0);
      if (this.isTouchAllowed)
      {
        for (int index = 0; index < this.defaultTouchInputSource.get_touchCount(); ++index)
        {
          Touch touch = this.defaultTouchInputSource.GetTouch(index);
          flag1 = ((flag1 ? 1 : 0) | (((Touch) ref touch).get_phase() == null || ((Touch) ref touch).get_phase() == 1 ? 1 : (((Touch) ref touch).get_phase() == 2 ? 1 : 0))) != 0;
        }
      }
      return flag1;
    }

    public virtual void ActivateModule()
    {
      if (!this.m_HasFocus && this.ShouldIgnoreEventsOnNoFocus())
        return;
      base.ActivateModule();
      GameObject selectedGameObject = this.get_eventSystem().get_currentSelectedGameObject();
      if (Object.op_Equality((Object) selectedGameObject, (Object) null))
        selectedGameObject = this.get_eventSystem().get_firstSelectedGameObject();
      this.get_eventSystem().SetSelectedGameObject(selectedGameObject, this.GetBaseEventData());
    }

    public virtual void DeactivateModule()
    {
      base.DeactivateModule();
      this.ClearSelection();
    }

    public virtual void Process()
    {
      if (!ReInput.get_isReady() || !this.m_HasFocus && this.ShouldIgnoreEventsOnNoFocus())
        return;
      bool selectedObject = this.SendUpdateEventToSelectedObject();
      if (this.get_eventSystem().get_sendNavigationEvents())
      {
        if (!selectedObject)
          selectedObject |= this.SendMoveEventToSelectedObject();
        if (!selectedObject)
          this.SendSubmitEventToSelectedObject();
      }
      if (this.ProcessTouchEvents() || !this.isMouseSupported)
        return;
      this.ProcessMouseEvents();
    }

    private bool ProcessTouchEvents()
    {
      if (!this.isTouchAllowed)
        return false;
      for (int index = 0; index < this.defaultTouchInputSource.get_touchCount(); ++index)
      {
        Touch touch = this.defaultTouchInputSource.GetTouch(index);
        if (((Touch) ref touch).get_type() != 1)
        {
          bool pressed;
          bool released;
          PlayerPointerEventData pointerEventData = this.GetTouchPointerEventData(0, 0, touch, out pressed, out released);
          this.ProcessTouchPress((PointerEventData) pointerEventData, pressed, released);
          if (!released)
          {
            this.ProcessMove(pointerEventData);
            this.ProcessDrag(pointerEventData);
          }
          else
            this.RemovePointerData(pointerEventData);
        }
      }
      return this.defaultTouchInputSource.get_touchCount() > 0;
    }

    private void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
    {
      RaycastResult pointerCurrentRaycast = pointerEvent.get_pointerCurrentRaycast();
      GameObject gameObject1 = ((RaycastResult) ref pointerCurrentRaycast).get_gameObject();
      if (pressed)
      {
        pointerEvent.set_eligibleForClick(true);
        pointerEvent.set_delta(Vector2.get_zero());
        pointerEvent.set_dragging(false);
        pointerEvent.set_useDragThreshold(true);
        pointerEvent.set_pressPosition(pointerEvent.get_position());
        pointerEvent.set_pointerPressRaycast(pointerEvent.get_pointerCurrentRaycast());
        this.DeselectIfSelectionChanged(gameObject1, (BaseEventData) pointerEvent);
        if (Object.op_Inequality((Object) pointerEvent.get_pointerEnter(), (Object) gameObject1))
        {
          this.HandlePointerExitAndEnter(pointerEvent, gameObject1);
          pointerEvent.set_pointerEnter(gameObject1);
        }
        GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject1, (BaseEventData) pointerEvent, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_pointerDownHandler());
        if (Object.op_Equality((Object) gameObject2, (Object) null))
          gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
        float unscaledTime = Time.get_unscaledTime();
        if (Object.op_Equality((Object) gameObject2, (Object) pointerEvent.get_lastPress()))
        {
          if ((double) (unscaledTime - pointerEvent.get_clickTime()) < 0.300000011920929)
          {
            PointerEventData pointerEventData = pointerEvent;
            pointerEventData.set_clickCount(pointerEventData.get_clickCount() + 1);
          }
          else
            pointerEvent.set_clickCount(1);
          pointerEvent.set_clickTime(unscaledTime);
        }
        else
          pointerEvent.set_clickCount(1);
        pointerEvent.set_pointerPress(gameObject2);
        pointerEvent.set_rawPointerPress(gameObject1);
        pointerEvent.set_clickTime(unscaledTime);
        pointerEvent.set_pointerDrag(ExecuteEvents.GetEventHandler<IDragHandler>(gameObject1));
        if (Object.op_Inequality((Object) pointerEvent.get_pointerDrag(), (Object) null))
          ExecuteEvents.Execute<IInitializePotentialDragHandler>(pointerEvent.get_pointerDrag(), (BaseEventData) pointerEvent, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_initializePotentialDrag());
      }
      if (!released)
        return;
      ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.get_pointerPress(), (BaseEventData) pointerEvent, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_pointerUpHandler());
      GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
      if (Object.op_Equality((Object) pointerEvent.get_pointerPress(), (Object) eventHandler) && pointerEvent.get_eligibleForClick())
        ExecuteEvents.Execute<IPointerClickHandler>(pointerEvent.get_pointerPress(), (BaseEventData) pointerEvent, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_pointerClickHandler());
      else if (Object.op_Inequality((Object) pointerEvent.get_pointerDrag(), (Object) null) && pointerEvent.get_dragging())
        ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject1, (BaseEventData) pointerEvent, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_dropHandler());
      pointerEvent.set_eligibleForClick(false);
      pointerEvent.set_pointerPress((GameObject) null);
      pointerEvent.set_rawPointerPress((GameObject) null);
      if (Object.op_Inequality((Object) pointerEvent.get_pointerDrag(), (Object) null) && pointerEvent.get_dragging())
        ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.get_pointerDrag(), (BaseEventData) pointerEvent, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_endDragHandler());
      pointerEvent.set_dragging(false);
      pointerEvent.set_pointerDrag((GameObject) null);
      if (Object.op_Inequality((Object) pointerEvent.get_pointerDrag(), (Object) null))
        ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.get_pointerDrag(), (BaseEventData) pointerEvent, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_endDragHandler());
      pointerEvent.set_pointerDrag((GameObject) null);
      ExecuteEvents.ExecuteHierarchy<IPointerExitHandler>(pointerEvent.get_pointerEnter(), (BaseEventData) pointerEvent, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_pointerExitHandler());
      pointerEvent.set_pointerEnter((GameObject) null);
    }

    private bool SendSubmitEventToSelectedObject()
    {
      if (Object.op_Equality((Object) this.get_eventSystem().get_currentSelectedGameObject(), (Object) null) || this.recompiling)
        return false;
      BaseEventData baseEventData = this.GetBaseEventData();
      for (int index = 0; index < this.playerIds.Length; ++index)
      {
        Player player = ReInput.get_players().GetPlayer(this.playerIds[index]);
        if (player != null && (!this.usePlayingPlayersOnly || player.get_isPlaying()))
        {
          if (this.GetButtonDown(player, this.submitActionId))
          {
            ExecuteEvents.Execute<ISubmitHandler>(this.get_eventSystem().get_currentSelectedGameObject(), baseEventData, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_submitHandler());
            break;
          }
          if (this.GetButtonDown(player, this.cancelActionId))
          {
            ExecuteEvents.Execute<ICancelHandler>(this.get_eventSystem().get_currentSelectedGameObject(), baseEventData, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_cancelHandler());
            break;
          }
        }
      }
      return ((AbstractEventData) baseEventData).get_used();
    }

    private Vector2 GetRawMoveVector()
    {
      if (this.recompiling)
        return Vector2.get_zero();
      Vector2 zero = Vector2.get_zero();
      bool flag1 = false;
      bool flag2 = false;
      for (int index = 0; index < this.playerIds.Length; ++index)
      {
        Player player = ReInput.get_players().GetPlayer(this.playerIds[index]);
        if (player != null && (!this.usePlayingPlayersOnly || player.get_isPlaying()))
        {
          if (this.moveOneElementPerAxisPress)
          {
            float num1 = 0.0f;
            if (this.GetButtonDown(player, this.horizontalActionId))
              num1 = 1f;
            else if (this.GetNegativeButtonDown(player, this.horizontalActionId))
              num1 = -1f;
            float num2 = 0.0f;
            if (this.GetButtonDown(player, this.verticalActionId))
              num2 = 1f;
            else if (this.GetNegativeButtonDown(player, this.verticalActionId))
              num2 = -1f;
            ref Vector2 local1 = ref zero;
            local1.x = (__Null) (local1.x + (double) num1);
            ref Vector2 local2 = ref zero;
            local2.y = (__Null) (local2.y + (double) num2);
          }
          else
          {
            ref Vector2 local1 = ref zero;
            local1.x = (__Null) (local1.x + (double) this.GetAxis(player, this.horizontalActionId));
            ref Vector2 local2 = ref zero;
            local2.y = (__Null) (local2.y + (double) this.GetAxis(player, this.verticalActionId));
          }
          flag1 = ((flag1 ? 1 : 0) | (this.GetButtonDown(player, this.horizontalActionId) ? 1 : (this.GetNegativeButtonDown(player, this.horizontalActionId) ? 1 : 0))) != 0;
          flag2 = ((flag2 ? 1 : 0) | (this.GetButtonDown(player, this.verticalActionId) ? 1 : (this.GetNegativeButtonDown(player, this.verticalActionId) ? 1 : 0))) != 0;
        }
      }
      if (flag1)
      {
        if (zero.x < 0.0)
          zero.x = (__Null) -1.0;
        if (zero.x > 0.0)
          zero.x = (__Null) 1.0;
      }
      if (flag2)
      {
        if (zero.y < 0.0)
          zero.y = (__Null) -1.0;
        if (zero.y > 0.0)
          zero.y = (__Null) 1.0;
      }
      return zero;
    }

    private bool SendMoveEventToSelectedObject()
    {
      if (this.recompiling)
        return false;
      float unscaledTime = Time.get_unscaledTime();
      Vector2 rawMoveVector = this.GetRawMoveVector();
      if (Mathf.Approximately((float) rawMoveVector.x, 0.0f) && Mathf.Approximately((float) rawMoveVector.y, 0.0f))
      {
        this.m_ConsecutiveMoveCount = 0;
        return false;
      }
      bool flag1 = (double) Vector2.Dot(rawMoveVector, this.m_LastMoveVector) > 0.0;
      bool downHorizontal;
      bool downVertical;
      this.CheckButtonOrKeyMovement(unscaledTime, out downHorizontal, out downVertical);
      AxisEventData axisEventData = (AxisEventData) null;
      bool flag2 = downHorizontal || downVertical;
      if (flag2)
      {
        axisEventData = this.GetAxisEventData((float) rawMoveVector.x, (float) rawMoveVector.y, 0.6f);
        MoveDirection moveDir = axisEventData.get_moveDir();
        flag2 = (moveDir == 1 || moveDir == 3) && downVertical || (moveDir == null || moveDir == 2) && downHorizontal;
      }
      if (!flag2)
        flag2 = (double) this.m_RepeatDelay <= 0.0 ? (double) unscaledTime > (double) this.m_PrevActionTime + 1.0 / (double) this.m_InputActionsPerSecond : (!flag1 || this.m_ConsecutiveMoveCount != 1 ? (double) unscaledTime > (double) this.m_PrevActionTime + 1.0 / (double) this.m_InputActionsPerSecond : (double) unscaledTime > (double) this.m_PrevActionTime + (double) this.m_RepeatDelay);
      if (!flag2)
        return false;
      if (axisEventData == null)
        axisEventData = this.GetAxisEventData((float) rawMoveVector.x, (float) rawMoveVector.y, 0.6f);
      if (axisEventData.get_moveDir() != 4)
      {
        ExecuteEvents.Execute<IMoveHandler>(this.get_eventSystem().get_currentSelectedGameObject(), (BaseEventData) axisEventData, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_moveHandler());
        if (!flag1)
          this.m_ConsecutiveMoveCount = 0;
        if (this.m_ConsecutiveMoveCount == 0 || !downHorizontal && !downVertical)
          ++this.m_ConsecutiveMoveCount;
        this.m_PrevActionTime = unscaledTime;
        this.m_LastMoveVector = rawMoveVector;
      }
      else
        this.m_ConsecutiveMoveCount = 0;
      return ((AbstractEventData) axisEventData).get_used();
    }

    private void CheckButtonOrKeyMovement(
      float time,
      out bool downHorizontal,
      out bool downVertical)
    {
      downHorizontal = false;
      downVertical = false;
      for (int index = 0; index < this.playerIds.Length; ++index)
      {
        Player player = ReInput.get_players().GetPlayer(this.playerIds[index]);
        if (player != null && (!this.usePlayingPlayersOnly || player.get_isPlaying()))
        {
          downHorizontal = ((downHorizontal ? 1 : 0) | (this.GetButtonDown(player, this.horizontalActionId) ? 1 : (this.GetNegativeButtonDown(player, this.horizontalActionId) ? 1 : 0))) != 0;
          downVertical = ((downVertical ? 1 : 0) | (this.GetButtonDown(player, this.verticalActionId) ? 1 : (this.GetNegativeButtonDown(player, this.verticalActionId) ? 1 : 0))) != 0;
        }
      }
    }

    private void ProcessMouseEvents()
    {
      for (int index = 0; index < this.playerIds.Length; ++index)
      {
        Player player = ReInput.get_players().GetPlayer(this.playerIds[index]);
        if (player != null && (!this.usePlayingPlayersOnly || player.get_isPlaying()))
        {
          int inputSourceCount = this.GetMouseInputSourceCount(this.playerIds[index]);
          for (int pointerIndex = 0; pointerIndex < inputSourceCount; ++pointerIndex)
            this.ProcessMouseEvent(this.playerIds[index], pointerIndex);
        }
      }
    }

    private void ProcessMouseEvent(int playerId, int pointerIndex)
    {
      RewiredPointerInputModule.MouseState pointerEventData = this.GetMousePointerEventData(playerId, pointerIndex);
      if (pointerEventData == null)
        return;
      RewiredPointerInputModule.MouseButtonEventData eventData = pointerEventData.GetButtonState(0).eventData;
      this.ProcessMousePress(eventData);
      this.ProcessMove(eventData.buttonData);
      this.ProcessDrag(eventData.buttonData);
      this.ProcessMousePress(pointerEventData.GetButtonState(1).eventData);
      this.ProcessDrag(pointerEventData.GetButtonState(1).eventData.buttonData);
      this.ProcessMousePress(pointerEventData.GetButtonState(2).eventData);
      this.ProcessDrag(pointerEventData.GetButtonState(2).eventData.buttonData);
      IMouseInputSource mouseInputSource = this.GetMouseInputSource(playerId, pointerIndex);
      for (int button = 3; button < mouseInputSource.get_buttonCount(); ++button)
      {
        this.ProcessMousePress(pointerEventData.GetButtonState(button).eventData);
        this.ProcessDrag(pointerEventData.GetButtonState(button).eventData.buttonData);
      }
      Vector2 scrollDelta = eventData.buttonData.get_scrollDelta();
      if (Mathf.Approximately(((Vector2) ref scrollDelta).get_sqrMagnitude(), 0.0f))
        return;
      RaycastResult pointerCurrentRaycast = eventData.buttonData.get_pointerCurrentRaycast();
      ExecuteEvents.ExecuteHierarchy<IScrollHandler>(ExecuteEvents.GetEventHandler<IScrollHandler>(((RaycastResult) ref pointerCurrentRaycast).get_gameObject()), (BaseEventData) eventData.buttonData, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_scrollHandler());
    }

    private bool SendUpdateEventToSelectedObject()
    {
      if (Object.op_Equality((Object) this.get_eventSystem().get_currentSelectedGameObject(), (Object) null))
        return false;
      BaseEventData baseEventData = this.GetBaseEventData();
      ExecuteEvents.Execute<IUpdateSelectedHandler>(this.get_eventSystem().get_currentSelectedGameObject(), baseEventData, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_updateSelectedHandler());
      return ((AbstractEventData) baseEventData).get_used();
    }

    private void ProcessMousePress(
      RewiredPointerInputModule.MouseButtonEventData data)
    {
      PlayerPointerEventData buttonData = data.buttonData;
      RaycastResult pointerCurrentRaycast = buttonData.get_pointerCurrentRaycast();
      GameObject gameObject1 = ((RaycastResult) ref pointerCurrentRaycast).get_gameObject();
      if (data.PressedThisFrame())
      {
        buttonData.set_eligibleForClick(true);
        buttonData.set_delta(Vector2.get_zero());
        buttonData.set_dragging(false);
        buttonData.set_useDragThreshold(true);
        buttonData.set_pressPosition(buttonData.get_position());
        buttonData.set_pointerPressRaycast(buttonData.get_pointerCurrentRaycast());
        this.DeselectIfSelectionChanged(gameObject1, (BaseEventData) buttonData);
        GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject1, (BaseEventData) buttonData, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_pointerDownHandler());
        if (Object.op_Equality((Object) gameObject2, (Object) null))
          gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
        float unscaledTime = Time.get_unscaledTime();
        if (Object.op_Equality((Object) gameObject2, (Object) buttonData.get_lastPress()))
        {
          if ((double) (unscaledTime - buttonData.get_clickTime()) < 0.300000011920929)
          {
            PlayerPointerEventData pointerEventData = buttonData;
            pointerEventData.set_clickCount(pointerEventData.get_clickCount() + 1);
          }
          else
            buttonData.set_clickCount(1);
          buttonData.set_clickTime(unscaledTime);
        }
        else
          buttonData.set_clickCount(1);
        buttonData.set_pointerPress(gameObject2);
        buttonData.set_rawPointerPress(gameObject1);
        buttonData.set_clickTime(unscaledTime);
        buttonData.set_pointerDrag(ExecuteEvents.GetEventHandler<IDragHandler>(gameObject1));
        if (Object.op_Inequality((Object) buttonData.get_pointerDrag(), (Object) null))
          ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.get_pointerDrag(), (BaseEventData) buttonData, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_initializePotentialDrag());
      }
      if (!data.ReleasedThisFrame())
        return;
      ExecuteEvents.Execute<IPointerUpHandler>(buttonData.get_pointerPress(), (BaseEventData) buttonData, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_pointerUpHandler());
      GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
      if (Object.op_Equality((Object) buttonData.get_pointerPress(), (Object) eventHandler) && buttonData.get_eligibleForClick())
        ExecuteEvents.Execute<IPointerClickHandler>(buttonData.get_pointerPress(), (BaseEventData) buttonData, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_pointerClickHandler());
      else if (Object.op_Inequality((Object) buttonData.get_pointerDrag(), (Object) null) && buttonData.get_dragging())
        ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject1, (BaseEventData) buttonData, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_dropHandler());
      buttonData.set_eligibleForClick(false);
      buttonData.set_pointerPress((GameObject) null);
      buttonData.set_rawPointerPress((GameObject) null);
      if (Object.op_Inequality((Object) buttonData.get_pointerDrag(), (Object) null) && buttonData.get_dragging())
        ExecuteEvents.Execute<IEndDragHandler>(buttonData.get_pointerDrag(), (BaseEventData) buttonData, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_endDragHandler());
      buttonData.set_dragging(false);
      buttonData.set_pointerDrag((GameObject) null);
      if (!Object.op_Inequality((Object) gameObject1, (Object) buttonData.get_pointerEnter()))
        return;
      this.HandlePointerExitAndEnter((PointerEventData) buttonData, (GameObject) null);
      this.HandlePointerExitAndEnter((PointerEventData) buttonData, gameObject1);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
      this.m_HasFocus = hasFocus;
    }

    private bool ShouldIgnoreEventsOnNoFocus()
    {
      return !ReInput.get_isReady() || ReInput.get_configuration().get_ignoreInputWhenAppNotInFocus();
    }

    protected virtual void OnDestroy()
    {
      ((UIBehaviour) this).OnDestroy();
      ReInput.remove_InitializedEvent(new Action(this.OnRewiredInitialized));
    }

    protected override bool IsDefaultPlayer(int playerId)
    {
      if (this.playerIds == null || !ReInput.get_isReady())
        return false;
      for (int index1 = 0; index1 < 3; ++index1)
      {
        for (int index2 = 0; index2 < this.playerIds.Length; ++index2)
        {
          Player player = ReInput.get_players().GetPlayer(this.playerIds[index2]);
          if (player != null && (index1 >= 1 || !this.usePlayingPlayersOnly || player.get_isPlaying()) && (index1 >= 2 || ((Player.ControllerHelper) player.controllers).get_hasMouse()))
            return this.playerIds[index2] == playerId;
        }
      }
      return false;
    }

    private void InitializeRewired()
    {
      if (!ReInput.get_isReady())
      {
        Debug.LogError((object) "Rewired is not initialized! Are you missing a Rewired Input Manager in your scene?");
      }
      else
      {
        ReInput.remove_ShutDownEvent(new Action(this.OnRewiredShutDown));
        ReInput.add_ShutDownEvent(new Action(this.OnRewiredShutDown));
        ReInput.remove_EditorRecompileEvent(new Action(this.OnEditorRecompile));
        ReInput.add_EditorRecompileEvent(new Action(this.OnEditorRecompile));
        this.SetupRewiredVars();
      }
    }

    private void SetupRewiredVars()
    {
      if (!ReInput.get_isReady())
        return;
      this.SetUpRewiredActions();
      if (this.useAllRewiredGamePlayers)
      {
        IList<Player> playerList = !this.useRewiredSystemPlayer ? ReInput.get_players().get_Players() : ReInput.get_players().get_AllPlayers();
        this.playerIds = new int[((ICollection<Player>) playerList).Count];
        for (int index = 0; index < ((ICollection<Player>) playerList).Count; ++index)
          this.playerIds[index] = playerList[index].get_id();
      }
      else
      {
        bool flag = false;
        List<int> intList = new List<int>(this.rewiredPlayerIds.Length + 1);
        for (int index = 0; index < this.rewiredPlayerIds.Length; ++index)
        {
          Player player = ReInput.get_players().GetPlayer(this.rewiredPlayerIds[index]);
          if (player != null && !intList.Contains(player.get_id()))
          {
            intList.Add(player.get_id());
            if (player.get_id() == 9999999)
              flag = true;
          }
        }
        if (this.useRewiredSystemPlayer && !flag)
          intList.Insert(0, ReInput.get_players().GetSystemPlayer().get_id());
        this.playerIds = intList.ToArray();
      }
      this.SetUpRewiredPlayerMice();
    }

    private void SetUpRewiredPlayerMice()
    {
      if (!ReInput.get_isReady())
        return;
      this.ClearMouseInputSources();
      for (int index = 0; index < this.playerMice.Count; ++index)
      {
        PlayerMouse playerMouse = this.playerMice[index];
        if (!UnityTools.IsNullOrDestroyed<PlayerMouse>((M0) playerMouse))
          this.AddMouseInputSource((IMouseInputSource) playerMouse);
      }
    }

    private void SetUpRewiredActions()
    {
      if (!ReInput.get_isReady())
        return;
      if (!this.setActionsById)
      {
        this.horizontalActionId = ReInput.get_mapping().GetActionId(this.m_HorizontalAxis);
        this.verticalActionId = ReInput.get_mapping().GetActionId(this.m_VerticalAxis);
        this.submitActionId = ReInput.get_mapping().GetActionId(this.m_SubmitButton);
        this.cancelActionId = ReInput.get_mapping().GetActionId(this.m_CancelButton);
      }
      else
      {
        InputAction action1 = ReInput.get_mapping().GetAction(this.horizontalActionId);
        this.m_HorizontalAxis = action1 == null ? string.Empty : action1.get_name();
        if (action1 == null)
          this.horizontalActionId = -1;
        InputAction action2 = ReInput.get_mapping().GetAction(this.verticalActionId);
        this.m_VerticalAxis = action2 == null ? string.Empty : action2.get_name();
        if (action2 == null)
          this.verticalActionId = -1;
        InputAction action3 = ReInput.get_mapping().GetAction(this.submitActionId);
        this.m_SubmitButton = action3 == null ? string.Empty : action3.get_name();
        if (action3 == null)
          this.submitActionId = -1;
        InputAction action4 = ReInput.get_mapping().GetAction(this.cancelActionId);
        this.m_CancelButton = action4 == null ? string.Empty : action4.get_name();
        if (action4 != null)
          return;
        this.cancelActionId = -1;
      }
    }

    private bool GetButtonDown(Player player, int actionId)
    {
      return actionId >= 0 && player.GetButtonDown(actionId);
    }

    private bool GetNegativeButtonDown(Player player, int actionId)
    {
      return actionId >= 0 && player.GetNegativeButtonDown(actionId);
    }

    private float GetAxis(Player player, int actionId)
    {
      return actionId < 0 ? 0.0f : player.GetAxis(actionId);
    }

    private void CheckEditorRecompile()
    {
      if (!this.recompiling || !ReInput.get_isReady())
        return;
      this.recompiling = false;
      this.InitializeRewired();
    }

    private void OnEditorRecompile()
    {
      this.recompiling = true;
      this.ClearRewiredVars();
    }

    private void ClearRewiredVars()
    {
      Array.Clear((Array) this.playerIds, 0, this.playerIds.Length);
      this.ClearMouseInputSources();
    }

    private bool DidAnyMouseMove()
    {
      for (int index = 0; index < this.playerIds.Length; ++index)
      {
        int playerId = this.playerIds[index];
        Player player = ReInput.get_players().GetPlayer(playerId);
        if (player != null && (!this.usePlayingPlayersOnly || player.get_isPlaying()))
        {
          int inputSourceCount = this.GetMouseInputSourceCount(playerId);
          for (int mouseIndex = 0; mouseIndex < inputSourceCount; ++mouseIndex)
          {
            IMouseInputSource mouseInputSource = this.GetMouseInputSource(playerId, mouseIndex);
            if (mouseInputSource != null)
            {
              Vector2 screenPositionDelta = mouseInputSource.get_screenPositionDelta();
              if ((double) ((Vector2) ref screenPositionDelta).get_sqrMagnitude() > 0.0)
                return true;
            }
          }
        }
      }
      return false;
    }

    private bool GetMouseButtonDownOnAnyMouse(int buttonIndex)
    {
      for (int index = 0; index < this.playerIds.Length; ++index)
      {
        int playerId = this.playerIds[index];
        Player player = ReInput.get_players().GetPlayer(playerId);
        if (player != null && (!this.usePlayingPlayersOnly || player.get_isPlaying()))
        {
          int inputSourceCount = this.GetMouseInputSourceCount(playerId);
          for (int mouseIndex = 0; mouseIndex < inputSourceCount; ++mouseIndex)
          {
            IMouseInputSource mouseInputSource = this.GetMouseInputSource(playerId, mouseIndex);
            if (mouseInputSource != null && mouseInputSource.GetButtonDown(buttonIndex))
              return true;
          }
        }
      }
      return false;
    }

    private void OnRewiredInitialized()
    {
      this.InitializeRewired();
    }

    private void OnRewiredShutDown()
    {
      this.ClearRewiredVars();
    }

    [Serializable]
    public class PlayerSetting
    {
      public List<PlayerMouse> playerMice = new List<PlayerMouse>();
      public int playerId;

      public PlayerSetting()
      {
      }

      private PlayerSetting(RewiredStandaloneInputModule.PlayerSetting other)
      {
        if (other == null)
          throw new ArgumentNullException(nameof (other));
        this.playerId = other.playerId;
        this.playerMice = new List<PlayerMouse>();
        if (other.playerMice == null)
          return;
        using (List<PlayerMouse>.Enumerator enumerator = other.playerMice.GetEnumerator())
        {
          while (enumerator.MoveNext())
            this.playerMice.Add(enumerator.Current);
        }
      }

      public RewiredStandaloneInputModule.PlayerSetting Clone()
      {
        return new RewiredStandaloneInputModule.PlayerSetting(this);
      }
    }
  }
}
