// Decompiled with JetBrains decompiler
// Type: Rewired.Integration.UnityUI.RewiredPointerInputModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Rewired.UI;
using Rewired.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rewired.Integration.UnityUI
{
  public abstract class RewiredPointerInputModule : BaseInputModule
  {
    public const int kMouseLeftId = -1;
    public const int kMouseRightId = -2;
    public const int kMouseMiddleId = -3;
    public const int kFakeTouchesId = -4;
    private const int customButtonsStartingId = -2147483520;
    private const int customButtonsMaxCount = 128;
    private const int customButtonsLastId = -2147483392;
    private readonly List<IMouseInputSource> m_MouseInputSourcesList;
    private Dictionary<int, Dictionary<int, PlayerPointerEventData>[]> m_PlayerPointerData;
    private ITouchInputSource m_UserDefaultTouchInputSource;
    private RewiredPointerInputModule.UnityInputSource __m_DefaultInputSource;
    private readonly RewiredPointerInputModule.MouseState m_MouseState;

    protected RewiredPointerInputModule()
    {
      base.\u002Ector();
    }

    private RewiredPointerInputModule.UnityInputSource defaultInputSource
    {
      get
      {
        return this.__m_DefaultInputSource != null ? this.__m_DefaultInputSource : (this.__m_DefaultInputSource = new RewiredPointerInputModule.UnityInputSource());
      }
    }

    private IMouseInputSource defaultMouseInputSource
    {
      get
      {
        return (IMouseInputSource) this.defaultInputSource;
      }
    }

    protected ITouchInputSource defaultTouchInputSource
    {
      get
      {
        return (ITouchInputSource) this.defaultInputSource;
      }
    }

    protected bool IsDefaultMouse(IMouseInputSource mouse)
    {
      return this.defaultMouseInputSource == mouse;
    }

    public IMouseInputSource GetMouseInputSource(int playerId, int mouseIndex)
    {
      if (mouseIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (mouseIndex));
      if (this.m_MouseInputSourcesList.Count == 0 && this.IsDefaultPlayer(playerId))
        return this.defaultMouseInputSource;
      int count = this.m_MouseInputSourcesList.Count;
      int num = 0;
      for (int index = 0; index < count; ++index)
      {
        IMouseInputSource mouseInputSources = this.m_MouseInputSourcesList[index];
        if (!UnityTools.IsNullOrDestroyed<IMouseInputSource>((M0) mouseInputSources) && mouseInputSources.get_playerId() == playerId)
        {
          if (mouseIndex == num)
            return mouseInputSources;
          ++num;
        }
      }
      return (IMouseInputSource) null;
    }

    public void RemoveMouseInputSource(IMouseInputSource source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      this.m_MouseInputSourcesList.Remove(source);
    }

    public void AddMouseInputSource(IMouseInputSource source)
    {
      if (UnityTools.IsNullOrDestroyed<IMouseInputSource>((M0) source))
        throw new ArgumentNullException(nameof (source));
      this.m_MouseInputSourcesList.Add(source);
    }

    public int GetMouseInputSourceCount(int playerId)
    {
      if (this.m_MouseInputSourcesList.Count == 0 && this.IsDefaultPlayer(playerId))
        return 1;
      int count = this.m_MouseInputSourcesList.Count;
      int num = 0;
      for (int index = 0; index < count; ++index)
      {
        IMouseInputSource mouseInputSources = this.m_MouseInputSourcesList[index];
        if (!UnityTools.IsNullOrDestroyed<IMouseInputSource>((M0) mouseInputSources) && mouseInputSources.get_playerId() == playerId)
          ++num;
      }
      return num;
    }

    public ITouchInputSource GetTouchInputSource(int playerId, int sourceIndex)
    {
      return !UnityTools.IsNullOrDestroyed<ITouchInputSource>((M0) this.m_UserDefaultTouchInputSource) ? this.m_UserDefaultTouchInputSource : this.defaultTouchInputSource;
    }

    public void RemoveTouchInputSource(ITouchInputSource source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (this.m_UserDefaultTouchInputSource != source)
        return;
      this.m_UserDefaultTouchInputSource = (ITouchInputSource) null;
    }

    public void AddTouchInputSource(ITouchInputSource source)
    {
      if (UnityTools.IsNullOrDestroyed<ITouchInputSource>((M0) source))
        throw new ArgumentNullException(nameof (source));
      this.m_UserDefaultTouchInputSource = source;
    }

    public int GetTouchInputSourceCount(int playerId)
    {
      return this.IsDefaultPlayer(playerId) ? 1 : 0;
    }

    protected void ClearMouseInputSources()
    {
      this.m_MouseInputSourcesList.Clear();
    }

    protected virtual bool isMouseSupported
    {
      get
      {
        int count = this.m_MouseInputSourcesList.Count;
        if (count == 0)
          return this.defaultMouseInputSource.get_enabled();
        for (int index = 0; index < count; ++index)
        {
          if (this.m_MouseInputSourcesList[index].get_enabled())
            return true;
        }
        return false;
      }
    }

    protected abstract bool IsDefaultPlayer(int playerId);

    protected bool GetPointerData(
      int playerId,
      int pointerIndex,
      int pointerTypeId,
      out PlayerPointerEventData data,
      bool create,
      PointerEventType pointerEventType)
    {
      Dictionary<int, PlayerPointerEventData>[] dictionaryArray1;
      if (!this.m_PlayerPointerData.TryGetValue(playerId, out dictionaryArray1))
      {
        dictionaryArray1 = new Dictionary<int, PlayerPointerEventData>[pointerIndex + 1];
        for (int index = 0; index < dictionaryArray1.Length; ++index)
          dictionaryArray1[index] = new Dictionary<int, PlayerPointerEventData>();
        this.m_PlayerPointerData.Add(playerId, dictionaryArray1);
      }
      if (pointerIndex >= dictionaryArray1.Length)
      {
        Dictionary<int, PlayerPointerEventData>[] dictionaryArray2 = new Dictionary<int, PlayerPointerEventData>[pointerIndex + 1];
        for (int index = 0; index < dictionaryArray1.Length; ++index)
          dictionaryArray2[index] = dictionaryArray1[index];
        dictionaryArray2[pointerIndex] = new Dictionary<int, PlayerPointerEventData>();
        dictionaryArray1 = dictionaryArray2;
        this.m_PlayerPointerData[playerId] = dictionaryArray1;
      }
      Dictionary<int, PlayerPointerEventData> dictionary = dictionaryArray1[pointerIndex];
      if (!dictionary.TryGetValue(pointerTypeId, out data) && create)
      {
        data = this.CreatePointerEventData(playerId, pointerIndex, pointerTypeId, pointerEventType);
        dictionary.Add(pointerTypeId, data);
        return true;
      }
      data.mouseSource = pointerEventType != PointerEventType.Mouse ? (IMouseInputSource) null : this.GetMouseInputSource(playerId, pointerIndex);
      data.touchSource = pointerEventType != PointerEventType.Touch ? (ITouchInputSource) null : this.GetTouchInputSource(playerId, pointerIndex);
      return false;
    }

    private PlayerPointerEventData CreatePointerEventData(
      int playerId,
      int pointerIndex,
      int pointerTypeId,
      PointerEventType pointerEventType)
    {
      PlayerPointerEventData pointerEventData1 = new PlayerPointerEventData(this.get_eventSystem());
      pointerEventData1.playerId = playerId;
      pointerEventData1.inputSourceIndex = pointerIndex;
      pointerEventData1.set_pointerId(pointerTypeId);
      pointerEventData1.sourceType = pointerEventType;
      PlayerPointerEventData pointerEventData2 = pointerEventData1;
      switch (pointerEventType)
      {
        case PointerEventType.Mouse:
          pointerEventData2.mouseSource = this.GetMouseInputSource(playerId, pointerIndex);
          break;
        case PointerEventType.Touch:
          pointerEventData2.touchSource = this.GetTouchInputSource(playerId, pointerIndex);
          break;
      }
      switch (pointerTypeId)
      {
        case -3:
          pointerEventData2.buttonIndex = 2;
          break;
        case -2:
          pointerEventData2.buttonIndex = 1;
          break;
        case -1:
          pointerEventData2.buttonIndex = 0;
          break;
        default:
          if (pointerTypeId >= -2147483520 && pointerTypeId <= -2147483392)
          {
            pointerEventData2.buttonIndex = pointerTypeId - -2147483520;
            break;
          }
          break;
      }
      return pointerEventData2;
    }

    protected void RemovePointerData(PlayerPointerEventData data)
    {
      Dictionary<int, PlayerPointerEventData>[] dictionaryArray;
      if (!this.m_PlayerPointerData.TryGetValue(data.playerId, out dictionaryArray) || (uint) data.inputSourceIndex >= (uint) dictionaryArray.Length)
        return;
      dictionaryArray[data.inputSourceIndex].Remove(data.get_pointerId());
    }

    protected PlayerPointerEventData GetTouchPointerEventData(
      int playerId,
      int touchDeviceIndex,
      Touch input,
      out bool pressed,
      out bool released)
    {
      PlayerPointerEventData data;
      bool pointerData = this.GetPointerData(playerId, touchDeviceIndex, ((Touch) ref input).get_fingerId(), out data, true, PointerEventType.Touch);
      ((AbstractEventData) data).Reset();
      pressed = pointerData || ((Touch) ref input).get_phase() == 0;
      released = ((Touch) ref input).get_phase() == 4 || ((Touch) ref input).get_phase() == 3;
      if (pointerData)
        data.set_position(((Touch) ref input).get_position());
      if (pressed)
        data.set_delta(Vector2.get_zero());
      else
        data.set_delta(Vector2.op_Subtraction(((Touch) ref input).get_position(), data.get_position()));
      data.set_position(((Touch) ref input).get_position());
      data.set_button((PointerEventData.InputButton) 0);
      this.get_eventSystem().RaycastAll((PointerEventData) data, (List<RaycastResult>) this.m_RaycastResultCache);
      RaycastResult firstRaycast = BaseInputModule.FindFirstRaycast((List<RaycastResult>) this.m_RaycastResultCache);
      data.set_pointerCurrentRaycast(firstRaycast);
      ((List<RaycastResult>) this.m_RaycastResultCache).Clear();
      return data;
    }

    protected virtual RewiredPointerInputModule.MouseState GetMousePointerEventData(
      int playerId,
      int mouseIndex)
    {
      IMouseInputSource mouseInputSource = this.GetMouseInputSource(playerId, mouseIndex);
      if (mouseInputSource == null)
        return (RewiredPointerInputModule.MouseState) null;
      PlayerPointerEventData data1;
      bool pointerData = this.GetPointerData(playerId, mouseIndex, -1, out data1, true, PointerEventType.Mouse);
      ((AbstractEventData) data1).Reset();
      if (pointerData)
        data1.set_position(mouseInputSource.get_screenPosition());
      Vector2 screenPosition = mouseInputSource.get_screenPosition();
      if (mouseInputSource.get_locked())
      {
        data1.set_position(new Vector2(-1f, -1f));
        data1.set_delta(Vector2.get_zero());
      }
      else
      {
        data1.set_delta(Vector2.op_Subtraction(screenPosition, data1.get_position()));
        data1.set_position(screenPosition);
      }
      data1.set_scrollDelta(mouseInputSource.get_wheelDelta());
      data1.set_button((PointerEventData.InputButton) 0);
      this.get_eventSystem().RaycastAll((PointerEventData) data1, (List<RaycastResult>) this.m_RaycastResultCache);
      RaycastResult firstRaycast = BaseInputModule.FindFirstRaycast((List<RaycastResult>) this.m_RaycastResultCache);
      data1.set_pointerCurrentRaycast(firstRaycast);
      ((List<RaycastResult>) this.m_RaycastResultCache).Clear();
      PlayerPointerEventData data2;
      this.GetPointerData(playerId, mouseIndex, -2, out data2, true, PointerEventType.Mouse);
      this.CopyFromTo((PointerEventData) data1, (PointerEventData) data2);
      data2.set_button((PointerEventData.InputButton) 1);
      PlayerPointerEventData data3;
      this.GetPointerData(playerId, mouseIndex, -3, out data3, true, PointerEventType.Mouse);
      this.CopyFromTo((PointerEventData) data1, (PointerEventData) data3);
      data3.set_button((PointerEventData.InputButton) 2);
      for (int index = 3; index < mouseInputSource.get_buttonCount(); ++index)
      {
        PlayerPointerEventData data4;
        this.GetPointerData(playerId, mouseIndex, index - 2147483520, out data4, true, PointerEventType.Mouse);
        this.CopyFromTo((PointerEventData) data1, (PointerEventData) data4);
        data4.set_button((PointerEventData.InputButton) -1);
      }
      this.m_MouseState.SetButtonState(0, this.StateForMouseButton(playerId, mouseIndex, 0), data1);
      this.m_MouseState.SetButtonState(1, this.StateForMouseButton(playerId, mouseIndex, 1), data2);
      this.m_MouseState.SetButtonState(2, this.StateForMouseButton(playerId, mouseIndex, 2), data3);
      for (int index = 3; index < mouseInputSource.get_buttonCount(); ++index)
      {
        PlayerPointerEventData data4;
        this.GetPointerData(playerId, mouseIndex, index - 2147483520, out data4, false, PointerEventType.Mouse);
        this.m_MouseState.SetButtonState(index, this.StateForMouseButton(playerId, mouseIndex, index), data4);
      }
      return this.m_MouseState;
    }

    protected PlayerPointerEventData GetLastPointerEventData(
      int playerId,
      int pointerIndex,
      int pointerTypeId,
      bool ignorePointerTypeId,
      PointerEventType pointerEventType)
    {
      if (!ignorePointerTypeId)
      {
        PlayerPointerEventData data;
        this.GetPointerData(playerId, pointerIndex, pointerTypeId, out data, false, pointerEventType);
        return data;
      }
      Dictionary<int, PlayerPointerEventData>[] dictionaryArray;
      if (!this.m_PlayerPointerData.TryGetValue(playerId, out dictionaryArray))
        return (PlayerPointerEventData) null;
      if ((uint) pointerIndex >= (uint) dictionaryArray.Length)
        return (PlayerPointerEventData) null;
      using (Dictionary<int, PlayerPointerEventData>.Enumerator enumerator = dictionaryArray[pointerIndex].GetEnumerator())
      {
        if (enumerator.MoveNext())
          return enumerator.Current.Value;
      }
      return (PlayerPointerEventData) null;
    }

    private static bool ShouldStartDrag(
      Vector2 pressPos,
      Vector2 currentPos,
      float threshold,
      bool useDragThreshold)
    {
      if (!useDragThreshold)
        return true;
      Vector2 vector2 = Vector2.op_Subtraction(pressPos, currentPos);
      return (double) ((Vector2) ref vector2).get_sqrMagnitude() >= (double) threshold * (double) threshold;
    }

    protected virtual void ProcessMove(PlayerPointerEventData pointerEvent)
    {
      GameObject gameObject1;
      if (pointerEvent.sourceType == PointerEventType.Mouse)
      {
        GameObject gameObject2;
        if (this.GetMouseInputSource(pointerEvent.playerId, pointerEvent.inputSourceIndex).get_locked())
        {
          gameObject2 = (GameObject) null;
        }
        else
        {
          RaycastResult pointerCurrentRaycast = pointerEvent.get_pointerCurrentRaycast();
          gameObject2 = ((RaycastResult) ref pointerCurrentRaycast).get_gameObject();
        }
        gameObject1 = gameObject2;
      }
      else
      {
        if (pointerEvent.sourceType != PointerEventType.Touch)
          throw new NotImplementedException();
        RaycastResult pointerCurrentRaycast = pointerEvent.get_pointerCurrentRaycast();
        gameObject1 = ((RaycastResult) ref pointerCurrentRaycast).get_gameObject();
      }
      this.HandlePointerExitAndEnter((PointerEventData) pointerEvent, gameObject1);
    }

    protected virtual void ProcessDrag(PlayerPointerEventData pointerEvent)
    {
      if (!pointerEvent.IsPointerMoving() || Object.op_Equality((Object) pointerEvent.get_pointerDrag(), (Object) null) || pointerEvent.sourceType == PointerEventType.Mouse && this.GetMouseInputSource(pointerEvent.playerId, pointerEvent.inputSourceIndex).get_locked())
        return;
      if (!pointerEvent.get_dragging() && RewiredPointerInputModule.ShouldStartDrag(pointerEvent.get_pressPosition(), pointerEvent.get_position(), (float) this.get_eventSystem().get_pixelDragThreshold(), pointerEvent.get_useDragThreshold()))
      {
        ExecuteEvents.Execute<IBeginDragHandler>(pointerEvent.get_pointerDrag(), (BaseEventData) pointerEvent, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_beginDragHandler());
        pointerEvent.set_dragging(true);
      }
      if (!pointerEvent.get_dragging())
        return;
      if (Object.op_Inequality((Object) pointerEvent.get_pointerPress(), (Object) pointerEvent.get_pointerDrag()))
      {
        ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.get_pointerPress(), (BaseEventData) pointerEvent, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_pointerUpHandler());
        pointerEvent.set_eligibleForClick(false);
        pointerEvent.set_pointerPress((GameObject) null);
        pointerEvent.set_rawPointerPress((GameObject) null);
      }
      ExecuteEvents.Execute<IDragHandler>(pointerEvent.get_pointerDrag(), (BaseEventData) pointerEvent, (ExecuteEvents.EventFunction<M0>) ExecuteEvents.get_dragHandler());
    }

    public virtual bool IsPointerOverGameObject(int pointerTypeId)
    {
      foreach (KeyValuePair<int, Dictionary<int, PlayerPointerEventData>[]> keyValuePair in this.m_PlayerPointerData)
      {
        foreach (Dictionary<int, PlayerPointerEventData> dictionary in keyValuePair.Value)
        {
          PlayerPointerEventData pointerEventData;
          if (dictionary.TryGetValue(pointerTypeId, out pointerEventData) && Object.op_Inequality((Object) pointerEventData.get_pointerEnter(), (Object) null))
            return true;
        }
      }
      return false;
    }

    protected void ClearSelection()
    {
      BaseEventData baseEventData = this.GetBaseEventData();
      foreach (KeyValuePair<int, Dictionary<int, PlayerPointerEventData>[]> keyValuePair1 in this.m_PlayerPointerData)
      {
        Dictionary<int, PlayerPointerEventData>[] dictionaryArray = keyValuePair1.Value;
        for (int index = 0; index < dictionaryArray.Length; ++index)
        {
          foreach (KeyValuePair<int, PlayerPointerEventData> keyValuePair2 in dictionaryArray[index])
            this.HandlePointerExitAndEnter((PointerEventData) keyValuePair2.Value, (GameObject) null);
          dictionaryArray[index].Clear();
        }
      }
      this.get_eventSystem().SetSelectedGameObject((GameObject) null, baseEventData);
    }

    public virtual string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder("<b>Pointer Input Module of type: </b>" + (object) ((object) this).GetType());
      stringBuilder.AppendLine();
      foreach (KeyValuePair<int, Dictionary<int, PlayerPointerEventData>[]> keyValuePair1 in this.m_PlayerPointerData)
      {
        stringBuilder.AppendLine("<B>Player Id:</b> " + (object) keyValuePair1.Key);
        Dictionary<int, PlayerPointerEventData>[] dictionaryArray = keyValuePair1.Value;
        for (int index = 0; index < dictionaryArray.Length; ++index)
        {
          stringBuilder.AppendLine("<B>Pointer Index:</b> " + (object) index);
          foreach (KeyValuePair<int, PlayerPointerEventData> keyValuePair2 in dictionaryArray[index])
          {
            stringBuilder.AppendLine("<B>Button Id:</b> " + (object) keyValuePair2.Key);
            stringBuilder.AppendLine(((object) keyValuePair2.Value).ToString());
          }
        }
      }
      return stringBuilder.ToString();
    }

    protected void DeselectIfSelectionChanged(GameObject currentOverGo, BaseEventData pointerEvent)
    {
      if (!Object.op_Inequality((Object) ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGo), (Object) this.get_eventSystem().get_currentSelectedGameObject()))
        return;
      this.get_eventSystem().SetSelectedGameObject((GameObject) null, pointerEvent);
    }

    protected void CopyFromTo(PointerEventData from, PointerEventData to)
    {
      to.set_position(from.get_position());
      to.set_delta(from.get_delta());
      to.set_scrollDelta(from.get_scrollDelta());
      to.set_pointerCurrentRaycast(from.get_pointerCurrentRaycast());
      to.set_pointerEnter(from.get_pointerEnter());
    }

    protected PointerEventData.FramePressState StateForMouseButton(
      int playerId,
      int mouseIndex,
      int buttonId)
    {
      IMouseInputSource mouseInputSource = this.GetMouseInputSource(playerId, mouseIndex);
      if (mouseInputSource == null)
        return (PointerEventData.FramePressState) 3;
      bool buttonDown = mouseInputSource.GetButtonDown(buttonId);
      bool buttonUp = mouseInputSource.GetButtonUp(buttonId);
      if (buttonDown && buttonUp)
        return (PointerEventData.FramePressState) 2;
      if (buttonDown)
        return (PointerEventData.FramePressState) 0;
      return buttonUp ? (PointerEventData.FramePressState) 1 : (PointerEventData.FramePressState) 3;
    }

    protected class MouseState
    {
      private List<RewiredPointerInputModule.ButtonState> m_TrackedButtons = new List<RewiredPointerInputModule.ButtonState>();

      public bool AnyPressesThisFrame()
      {
        for (int index = 0; index < this.m_TrackedButtons.Count; ++index)
        {
          if (this.m_TrackedButtons[index].eventData.PressedThisFrame())
            return true;
        }
        return false;
      }

      public bool AnyReleasesThisFrame()
      {
        for (int index = 0; index < this.m_TrackedButtons.Count; ++index)
        {
          if (this.m_TrackedButtons[index].eventData.ReleasedThisFrame())
            return true;
        }
        return false;
      }

      public RewiredPointerInputModule.ButtonState GetButtonState(
        int button)
      {
        RewiredPointerInputModule.ButtonState buttonState = (RewiredPointerInputModule.ButtonState) null;
        for (int index = 0; index < this.m_TrackedButtons.Count; ++index)
        {
          if (this.m_TrackedButtons[index].button == button)
          {
            buttonState = this.m_TrackedButtons[index];
            break;
          }
        }
        if (buttonState == null)
        {
          buttonState = new RewiredPointerInputModule.ButtonState()
          {
            button = button,
            eventData = new RewiredPointerInputModule.MouseButtonEventData()
          };
          this.m_TrackedButtons.Add(buttonState);
        }
        return buttonState;
      }

      public void SetButtonState(
        int button,
        PointerEventData.FramePressState stateForMouseButton,
        PlayerPointerEventData data)
      {
        RewiredPointerInputModule.ButtonState buttonState = this.GetButtonState(button);
        buttonState.eventData.buttonState = stateForMouseButton;
        buttonState.eventData.buttonData = data;
      }
    }

    public class MouseButtonEventData
    {
      public PointerEventData.FramePressState buttonState;
      public PlayerPointerEventData buttonData;

      public bool PressedThisFrame()
      {
        return this.buttonState == null || this.buttonState == 2;
      }

      public bool ReleasedThisFrame()
      {
        return this.buttonState == 1 || this.buttonState == 2;
      }
    }

    protected class ButtonState
    {
      private int m_Button;
      private RewiredPointerInputModule.MouseButtonEventData m_EventData;

      public RewiredPointerInputModule.MouseButtonEventData eventData
      {
        get
        {
          return this.m_EventData;
        }
        set
        {
          this.m_EventData = value;
        }
      }

      public int button
      {
        get
        {
          return this.m_Button;
        }
        set
        {
          this.m_Button = value;
        }
      }
    }

    private sealed class UnityInputSource : IMouseInputSource, ITouchInputSource
    {
      private int m_LastUpdatedFrame = -1;
      private Vector2 m_MousePosition;
      private Vector2 m_MousePositionPrev;

      int IMouseInputSource.playerId
      {
        get
        {
          this.TryUpdate();
          return 0;
        }
      }

      int ITouchInputSource.playerId
      {
        get
        {
          this.TryUpdate();
          return 0;
        }
      }

      bool IMouseInputSource.enabled
      {
        get
        {
          this.TryUpdate();
          return Input.get_mousePresent();
        }
      }

      bool IMouseInputSource.locked
      {
        get
        {
          this.TryUpdate();
          return Cursor.get_lockState() == 1;
        }
      }

      int IMouseInputSource.buttonCount
      {
        get
        {
          this.TryUpdate();
          return 3;
        }
      }

      bool IMouseInputSource.GetButtonDown(int button)
      {
        this.TryUpdate();
        return Input.GetMouseButtonDown(button);
      }

      bool IMouseInputSource.GetButtonUp(int button)
      {
        this.TryUpdate();
        return Input.GetMouseButtonUp(button);
      }

      bool IMouseInputSource.GetButton(int button)
      {
        this.TryUpdate();
        return Input.GetMouseButton(button);
      }

      Vector2 IMouseInputSource.screenPosition
      {
        get
        {
          this.TryUpdate();
          return Vector2.op_Implicit(Input.get_mousePosition());
        }
      }

      Vector2 IMouseInputSource.screenPositionDelta
      {
        get
        {
          this.TryUpdate();
          return Vector2.op_Subtraction(this.m_MousePosition, this.m_MousePositionPrev);
        }
      }

      Vector2 IMouseInputSource.wheelDelta
      {
        get
        {
          this.TryUpdate();
          return Input.get_mouseScrollDelta();
        }
      }

      bool ITouchInputSource.touchSupported
      {
        get
        {
          this.TryUpdate();
          return Input.get_touchSupported();
        }
      }

      int ITouchInputSource.touchCount
      {
        get
        {
          this.TryUpdate();
          return Input.get_touchCount();
        }
      }

      Touch ITouchInputSource.GetTouch(int index)
      {
        this.TryUpdate();
        return Input.GetTouch(index);
      }

      private void TryUpdate()
      {
        if (Time.get_frameCount() == this.m_LastUpdatedFrame)
          return;
        this.m_LastUpdatedFrame = Time.get_frameCount();
        this.m_MousePositionPrev = this.m_MousePosition;
        this.m_MousePosition = Vector2.op_Implicit(Input.get_mousePosition());
      }
    }
  }
}
