// Decompiled with JetBrains decompiler
// Type: AIProject.InputModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace AIProject
{
  [AddComponentMenu("AI少女/Event/Input Module")]
  public class InputModule : PointerInputModule
  {
    private float _prevActionTime;
    private Vector2 _lastMoveVector;
    private int _consecutiveMoveCount;
    private Vector2 _mousePosition;
    private Vector2 _lastMousePosition;
    private GameObject _currentFocusedGameObject;
    [SerializeField]
    private float _inputActionPerSecond;
    [SerializeField]
    private float _repeatDelay;
    [FormerlySerializedAs("_allowActivationOnMobileDevice")]
    [SerializeField]
    private bool _forceModuleActive;

    protected InputModule()
    {
      base.\u002Ector();
    }

    public bool ForceModuleActive
    {
      get
      {
        return this._forceModuleActive;
      }
      set
      {
        this._forceModuleActive = value;
      }
    }

    public float InputActionsPerSecond
    {
      get
      {
        return this._inputActionPerSecond;
      }
      set
      {
        this._inputActionPerSecond = value;
      }
    }

    public float RepeatDelay
    {
      get
      {
        return this._repeatDelay;
      }
      set
      {
        this._repeatDelay = value;
      }
    }

    private bool ShouldIgnoreEventsOnNoFocus()
    {
      switch (SystemInfo.get_operatingSystemFamily() - 1)
      {
        case 0:
        case 1:
        case 2:
          return false;
        default:
          return false;
      }
    }

    public virtual void UpdateModule()
    {
      if (!((BaseInputModule) this).get_eventSystem().get_isFocused() && this.ShouldIgnoreEventsOnNoFocus())
        return;
      this._lastMousePosition = this._mousePosition;
      this._mousePosition = ((BaseInputModule) this).get_input().get_mousePosition();
    }

    public virtual bool IsModuleSupported()
    {
      return this._forceModuleActive || ((BaseInputModule) this).get_input().get_mousePresent() || ((BaseInputModule) this).get_input().get_touchSupported();
    }

    public virtual bool ShouldActivateModule()
    {
      bool flag1;
      if (!((BaseInputModule) this).ShouldActivateModule())
      {
        flag1 = false;
      }
      else
      {
        bool flag2 = this._forceModuleActive;
        if (Singleton<Input>.IsInstance())
        {
          Input instance = Singleton<Input>.Instance;
          flag2 |= instance.IsPressedKey((KeyCode) 323);
          if (instance.State == Input.ValidType.UI)
            flag2 = flag2 | instance.IsPressedAction() | !Mathf.Approximately((float) instance.UIAxisRow.x, 0.0f) | !Mathf.Approximately((float) instance.UIAxisRow.y, 0.0f);
        }
        int num1 = flag2 ? 1 : 0;
        Vector2 vector2 = Vector2.op_Subtraction(this._mousePosition, this._lastMousePosition);
        int num2 = (double) ((Vector2) ref vector2).get_sqrMagnitude() > 0.0 ? 1 : 0;
        bool flag3 = (num1 | num2) != 0;
        if (((BaseInputModule) this).get_input().get_touchCount() > 0)
          flag3 = true;
        flag1 = flag3;
      }
      return flag1;
    }

    public virtual void ActivateModule()
    {
      if (!((BaseInputModule) this).get_eventSystem().get_isFocused() && this.ShouldIgnoreEventsOnNoFocus())
        return;
      ((BaseInputModule) this).ActivateModule();
      this._mousePosition = ((BaseInputModule) this).get_input().get_mousePosition();
      this._lastMousePosition = ((BaseInputModule) this).get_input().get_mousePosition();
      GameObject selectedGameObject = ((BaseInputModule) this).get_eventSystem().get_currentSelectedGameObject();
      if (Object.op_Equality((Object) selectedGameObject, (Object) null))
        selectedGameObject = ((BaseInputModule) this).get_eventSystem().get_firstSelectedGameObject();
      ((BaseInputModule) this).get_eventSystem().SetSelectedGameObject(selectedGameObject, ((BaseInputModule) this).GetBaseEventData());
    }

    public virtual void DeactivateModule()
    {
      ((BaseInputModule) this).DeactivateModule();
      this.ClearSelection();
    }

    public virtual void Process()
    {
      if (!((BaseInputModule) this).get_eventSystem().get_isFocused() && this.ShouldIgnoreEventsOnNoFocus())
        return;
      bool selectedObject = this.SendUpdateEventToSelectedObject();
      if (((BaseInputModule) this).get_eventSystem().get_sendNavigationEvents())
      {
        if (!selectedObject)
          selectedObject |= this.SendMoveEventToSelectedObject();
        if (!selectedObject)
          this.SendSubmitEventToSelectedObject();
      }
      if (this.ProcessTouchEvents() || !((BaseInputModule) this).get_input().get_mousePresent())
        return;
      this.ProcessMouseEvent();
    }

    private bool ProcessTouchEvents()
    {
      for (int index = 0; index < ((BaseInputModule) this).get_input().get_touchCount(); ++index)
      {
        Touch touch = ((BaseInputModule) this).get_input().GetTouch(index);
        if (((Touch) ref touch).get_type() != 1)
        {
          bool pressed;
          bool released;
          PointerEventData pointerEventData = this.GetTouchPointerEventData(touch, ref pressed, ref released);
          this.ProcessTouchPress(pointerEventData, pressed, released);
          if (!released)
          {
            this.ProcessMove(pointerEventData);
            this.ProcessDrag(pointerEventData);
          }
          else
            this.RemovePointerData(pointerEventData);
        }
      }
      return ((BaseInputModule) this).get_input().get_touchCount() > 0;
    }

    protected void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
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
          ((BaseInputModule) this).HandlePointerExitAndEnter(pointerEvent, gameObject1);
          pointerEvent.set_pointerEnter(gameObject1);
        }
        GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject1, (BaseEventData) pointerEvent, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_pointerDownHandler());
        if (Object.op_Equality((Object) gameObject2, (Object) null))
          gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
        float unscaledTime = Time.get_unscaledTime();
        if (Object.op_Equality((Object) gameObject2, (Object) pointerEvent.get_lastPress()))
        {
          if ((double) (unscaledTime - pointerEvent.get_clickTime()) < 0.3)
          {
            PointerEventData pointerEventData = pointerEvent;
            pointerEventData.set_clickTime(pointerEventData.get_clickTime() + 1f);
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
      ExecuteEvents.ExecuteHierarchy<IPointerExitHandler>(pointerEvent.get_pointerEnter(), (BaseEventData) pointerEvent, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_pointerExitHandler());
      pointerEvent.set_pointerEnter((GameObject) null);
    }

    protected bool SendSubmitEventToSelectedObject()
    {
      bool flag;
      if (Object.op_Equality((Object) ((BaseInputModule) this).get_eventSystem().get_currentSelectedGameObject(), (Object) null))
      {
        flag = false;
      }
      else
      {
        BaseEventData baseEventData = ((BaseInputModule) this).GetBaseEventData();
        if (Singleton<Input>.IsInstance() && Singleton<Input>.Instance.IsPressedSubmit())
          ExecuteEvents.Execute<ISubmitHandler>(((BaseInputModule) this).get_eventSystem().get_currentSelectedGameObject(), baseEventData, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_submitHandler());
        flag = ((AbstractEventData) baseEventData).get_used();
      }
      return flag;
    }

    private Vector2 GetRawMoveVector()
    {
      Vector2 zero = Vector2.get_zero();
      if (Singleton<Input>.IsInstance())
      {
        Input instance = Singleton<Input>.Instance;
        if (instance.State == Input.ValidType.UI)
        {
          zero.x = instance.UIAxisRow.x;
          zero.y = instance.UIAxisRow.y;
          if (instance.IsPressedHorizontal())
          {
            if (zero.x < 0.0)
              zero.x = (__Null) -1.0;
            if (zero.x > 0.0)
              zero.x = (__Null) 1.0;
          }
          if (instance.IsPressedVertical())
          {
            if (zero.y < 0.0)
              zero.y = (__Null) -1.0;
            if (zero.y > 0.0)
              zero.y = (__Null) 1.0;
          }
        }
      }
      return zero;
    }

    protected bool SendMoveEventToSelectedObject()
    {
      float unscaledTime = Time.get_unscaledTime();
      Vector2 rawMoveVector = this.GetRawMoveVector();
      bool flag1;
      if (Mathf.Approximately((float) rawMoveVector.x, 0.0f) && Mathf.Approximately((float) rawMoveVector.y, 0.0f))
      {
        this._consecutiveMoveCount = 0;
        flag1 = false;
      }
      else
      {
        Input instance = Singleton<Input>.Instance;
        bool flag2 = instance.State == Input.ValidType.UI && (Singleton<Input>.IsInstance() && (instance.IsPressedHorizontal() || instance.IsPressedVertical()));
        bool flag3 = (double) Vector2.Dot(rawMoveVector, this._lastMoveVector) > 0.0;
        if (!flag2)
          flag2 = !flag3 || this._consecutiveMoveCount != 1 ? (double) unscaledTime > (double) this._prevActionTime + 1.0 / (double) this._inputActionPerSecond : (double) unscaledTime > (double) this._prevActionTime + (double) this._repeatDelay;
        bool flag4;
        if (!flag2)
        {
          flag4 = false;
        }
        else
        {
          AxisEventData axisEventData = ((BaseInputModule) this).GetAxisEventData((float) rawMoveVector.x, (float) rawMoveVector.y, 0.6f);
          if (axisEventData.get_moveDir() != 4)
          {
            ExecuteEvents.Execute<IMoveHandler>(((BaseInputModule) this).get_eventSystem().get_currentSelectedGameObject(), (BaseEventData) axisEventData, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_moveHandler());
            if (!flag3)
              this._consecutiveMoveCount = 0;
            ++this._consecutiveMoveCount;
            this._prevActionTime = unscaledTime;
            this._lastMoveVector = rawMoveVector;
          }
          else
            this._consecutiveMoveCount = 0;
          flag4 = ((AbstractEventData) axisEventData).get_used();
        }
        flag1 = false;
      }
      return flag1;
    }

    protected void ProcessMouseEvent()
    {
      this.ProcessMouseEvent(0);
    }

    protected void ProcessMouseEvent(int id)
    {
      PointerInputModule.MouseState pointerEventData = this.GetMousePointerEventData(id);
      PointerInputModule.MouseButtonEventData eventData = pointerEventData.GetButtonState((PointerEventData.InputButton) 0).get_eventData();
      RaycastResult pointerCurrentRaycast1 = ((PointerEventData) eventData.buttonData).get_pointerCurrentRaycast();
      this._currentFocusedGameObject = ((RaycastResult) ref pointerCurrentRaycast1).get_gameObject();
      this.ProcessMousePress(eventData);
      this.ProcessMove((PointerEventData) eventData.buttonData);
      this.ProcessDrag((PointerEventData) eventData.buttonData);
      this.ProcessMousePress(pointerEventData.GetButtonState((PointerEventData.InputButton) 1).get_eventData());
      this.ProcessDrag((PointerEventData) pointerEventData.GetButtonState((PointerEventData.InputButton) 1).get_eventData().buttonData);
      this.ProcessMousePress(pointerEventData.GetButtonState((PointerEventData.InputButton) 2).get_eventData());
      this.ProcessDrag((PointerEventData) pointerEventData.GetButtonState((PointerEventData.InputButton) 2).get_eventData().buttonData);
      Vector2 scrollDelta = ((PointerEventData) eventData.buttonData).get_scrollDelta();
      if (Mathf.Approximately(((Vector2) ref scrollDelta).get_sqrMagnitude(), 0.0f))
        return;
      RaycastResult pointerCurrentRaycast2 = ((PointerEventData) eventData.buttonData).get_pointerCurrentRaycast();
      ExecuteEvents.ExecuteHierarchy<IScrollHandler>(ExecuteEvents.GetEventHandler<IScrollHandler>(((RaycastResult) ref pointerCurrentRaycast2).get_gameObject()), (BaseEventData) eventData.buttonData, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_scrollHandler());
    }

    protected bool SendUpdateEventToSelectedObject()
    {
      bool flag;
      if (Object.op_Equality((Object) ((BaseInputModule) this).get_eventSystem().get_currentSelectedGameObject(), (Object) null))
      {
        flag = false;
      }
      else
      {
        BaseEventData baseEventData = ((BaseInputModule) this).GetBaseEventData();
        ExecuteEvents.Execute<IUpdateSelectedHandler>(((BaseInputModule) this).get_eventSystem().get_currentSelectedGameObject(), baseEventData, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_updateSelectedHandler());
        flag = ((AbstractEventData) baseEventData).get_used();
      }
      return flag;
    }

    protected void ProcessMousePress(PointerInputModule.MouseButtonEventData data)
    {
      PointerEventData buttonData = (PointerEventData) data.buttonData;
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
            PointerEventData pointerEventData = buttonData;
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
      ((BaseInputModule) this).HandlePointerExitAndEnter(buttonData, (GameObject) null);
      ((BaseInputModule) this).HandlePointerExitAndEnter(buttonData, gameObject1);
    }

    protected GameObject GetCurrentFocusedGameObject()
    {
      return this._currentFocusedGameObject;
    }
  }
}
