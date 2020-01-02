// Decompiled with JetBrains decompiler
// Type: Manager.Input
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.UI;
using Rewired;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Manager
{
  public class Input : Singleton<Input>
  {
    [SerializeField]
    private float _inputActionPerSecond = 10f;
    [SerializeField]
    private float _repeatDelay = 0.5f;
    private MenuUIBehaviour[] _menuElements = new MenuUIBehaviour[0];
    private Vector2 _lastMoveVector = Vector2.get_zero();
    private Vector2 _subLastMoveVector = Vector2.get_zero();
    private global::ActionTable Action = new global::ActionTable();
    [SerializeField]
    private Input.ValidType _state;
    private Input.ValidType _reservedState;
    private float _prevActionTime;
    private int _consecutiveMoveCount;
    private float _subPrevActionTime;
    private int _subConsecutiveMoveCount;
    protected AxisEventData _axisEventData;
    protected BaseEventData _baseEventData;

    public Input.ValidType State
    {
      get
      {
        return this._state;
      }
    }

    public void ReserveState(Input.ValidType type)
    {
      this._reservedState = type;
    }

    public void SetupState()
    {
      this._state = this._reservedState;
    }

    public float InputActionPerSecond
    {
      get
      {
        return this._inputActionPerSecond;
      }
    }

    public float RepeatDelay
    {
      get
      {
        return this._repeatDelay;
      }
    }

    public List<ISystemCommand> SystemElements { get; set; } = new List<ISystemCommand>();

    public List<IActionCommand> ActionElements { get; set; } = new List<IActionCommand>();

    public MenuUIBehaviour[] MenuElements
    {
      get
      {
        return this._menuElements;
      }
      set
      {
        this._menuElements = ((IEnumerable<MenuUIBehaviour>) value).ToArray<MenuUIBehaviour>();
      }
    }

    private MenuUIBehaviour[] Empty { get; } = new MenuUIBehaviour[0];

    public void ClearMenuElements()
    {
      foreach (MenuUIBehaviour menuElement in this._menuElements)
        menuElement.EnabledInput = false;
      this._menuElements = this.Empty;
    }

    private Player Player0
    {
      get
      {
        return ReInput.get_players().GetPlayer(0);
      }
    }

    public int FocusLevel { get; set; } = -1;

    public Vector2 MoveAxis
    {
      get
      {
        return this.Player0.GetButton(this.Action[ActionID.MouseLeft]) ? new Vector2(0.0f, 1f) : this.Player0.GetAxis2D(this.Action[ActionID.MoveHorizontal], this.Action[ActionID.MoveVertical]);
      }
    }

    public Vector2 LeftStickAxis
    {
      get
      {
        return this.Player0.GetAxis2D(this.Action[ActionID.MoveHorizontal], this.Action[ActionID.MoveVertical]);
      }
    }

    public Vector2 MouseAxis
    {
      get
      {
        Mouse controller = (Mouse) ReInput.get_controllers().GetController<Mouse>(0);
        return controller == null ? Vector2.get_zero() : new Vector2(((ControllerWithAxes) controller).GetAxis(0), ((ControllerWithAxes) controller).GetAxis(1));
      }
    }

    public Mouse Mouse
    {
      get
      {
        return (Mouse) ReInput.get_controllers().GetController<Mouse>(0);
      }
    }

    public Vector2 CameraAxis
    {
      get
      {
        return this.Player0.GetAxis2D(this.Action[ActionID.CameraHorizontal], this.Action[ActionID.CameraVertical]);
      }
    }

    public Vector2 UIAxisRow
    {
      get
      {
        Vector2 axis2D1 = this.Player0.GetAxis2D(this.Action[ActionID.MoveHorizontal], this.Action[ActionID.MoveVertical]);
        Vector2 axis2D2 = this.Player0.GetAxis2D(this.Action[ActionID.SelectHorizontal], this.Action[ActionID.SelectVertical]);
        return new Vector2((double) Mathf.Abs((float) axis2D1.x) >= (double) Mathf.Abs((float) axis2D2.x) ? (float) axis2D1.x : (float) axis2D2.x, (double) Mathf.Abs((float) axis2D1.y) >= (double) Mathf.Abs((float) axis2D2.y) ? (float) axis2D1.y : (float) axis2D2.y);
      }
    }

    public bool IsPressedHorizontal()
    {
      return this.IsPressedAxis(this.Action[ActionID.MoveHorizontal]) || this.IsPressedAxis(this.Action[ActionID.SelectHorizontal]);
    }

    public bool IsPressedVertical()
    {
      return this.IsPressedAxis(this.Action[ActionID.MoveVertical]) || this.IsPressedAxis(this.Action[ActionID.SelectVertical]);
    }

    public bool IsPressedAction()
    {
      return this.Player0.GetButtonDown(this.Action[ActionID.Action]);
    }

    public ActionID GetPressedKeyRewired()
    {
      foreach (KeyValuePair<ActionID, string> keyValuePair in (IEnumerable<KeyValuePair<ActionID, string>>) global::ActionTable.Table)
      {
        switch (keyValuePair.Key)
        {
          case ActionID.MoveHorizontal:
          case ActionID.MoveVertical:
          case ActionID.CameraHorizontal:
          case ActionID.CameraVertical:
          case ActionID.SelectHorizontal:
          case ActionID.SelectVertical:
          case ActionID.MouseWheel:
            if (this.IsPressedAxis(keyValuePair.Value))
              return keyValuePair.Key;
            continue;
          default:
            if (this.IsPressedKey(keyValuePair.Value))
              return keyValuePair.Key;
            continue;
        }
      }
      return ActionID.None;
    }

    public bool GetAnyDown()
    {
      return this.Player0.GetAnyButtonDown();
    }

    public KeyCode GetPressedKey()
    {
      if (!Input.get_anyKeyDown())
        return (KeyCode) 0;
      foreach (KeyCode keyCode in Enum.GetValues(typeof (KeyCode)))
      {
        if (Input.GetKeyDown(keyCode))
          return keyCode;
      }
      return (KeyCode) 0;
    }

    public bool IsPressedSubmit()
    {
      return this.Player0.GetButtonDown(this.Action[ActionID.Submit]);
    }

    public bool IsPressedCancel()
    {
      return this.Player0.GetButtonDown(this.Action[ActionID.Cancel]);
    }

    public bool IsDown(KeyCode key)
    {
      return Input.GetKey(key);
    }

    public bool IsPressedKey(KeyCode key)
    {
      return Input.GetKeyDown(key);
    }

    public bool IsDown(string key)
    {
      return this.Player0.GetButton(key);
    }

    public bool IsDown(ActionID id)
    {
      return this.Player0.GetButton((int) id);
    }

    public bool IsDown(int id)
    {
      return this.Player0.GetButton(id);
    }

    public bool IsPressedKey(string key)
    {
      return this.Player0.GetButtonDown(key);
    }

    public bool IsPressedKey(ActionID id)
    {
      return this.Player0.GetButtonDown((int) id);
    }

    public bool IsPressedKey(int id)
    {
      return this.Player0.GetButtonDown(id);
    }

    public bool IsPressedAxis(string axisName)
    {
      int num = (double) Mathf.Abs(this.Player0.GetAxisPrev(axisName)) <= 0.300000011920929 ? 0 : 1;
      return ((double) Mathf.Abs(this.Player0.GetAxis(axisName)) <= 0.300000011920929 ? 0 : 1) > num;
    }

    public bool IsPressedAxis(ActionID axisID)
    {
      int num = (double) Mathf.Abs(this.Player0.GetAxisPrev((int) axisID)) <= 0.300000011920929 ? 0 : 1;
      return ((double) Mathf.Abs(this.Player0.GetAxis((int) axisID)) <= 0.300000011920929 ? 0 : 1) > num;
    }

    public bool IsPressedAxis(int axisID)
    {
      return Mathf.Abs((int) this.Player0.GetAxisPrev(axisID)) < Mathf.Abs((int) this.Player0.GetAxis(axisID));
    }

    public float GetAxis(string axisName)
    {
      return this.Player0.GetAxis(axisName);
    }

    public float GetAxis(ActionID axisID)
    {
      return this.Player0.GetAxis((int) axisID);
    }

    public float GetAxis(int axisID)
    {
      return this.Player0.GetAxis(axisID);
    }

    public float GetAxisRaw(string axisName)
    {
      return this.Player0.GetAxisRaw(axisName);
    }

    public float GetAxisRaw(ActionID axisID)
    {
      return this.Player0.GetAxisRaw((int) axisID);
    }

    public float GetAxisRaw(int axisID)
    {
      return this.Player0.GetAxisRaw(axisID);
    }

    public float ScrollValue()
    {
      return this.Player0.GetAxis(this.Action[ActionID.MouseWheel]);
    }

    protected virtual void Update()
    {
      if (Singleton<Scene>.Instance.IsNowLoadingFade)
        return;
      Game instance = Singleton<Game>.Instance;
      if (Object.op_Equality((Object) instance.MapShortcutUI, (Object) null) && Object.op_Equality((Object) instance.Config, (Object) null) && (Object.op_Equality((Object) instance.Dialog, (Object) null) && Object.op_Equality((Object) instance.ExitScene, (Object) null)))
        this.SendPressedSystemEvent();
      switch (this._state)
      {
        case Input.ValidType.Action:
          this.SendPressedActionEventToSelectedObject();
          break;
        case Input.ValidType.UI:
          this.SendUpdateEventToSelectedObject();
          this.SendMoveEventToSelectedObject();
          this.SendSubMoveEvent();
          this.SendSubmitEventToSelectedObject();
          break;
      }
    }

    protected void SendPressedSystemEvent()
    {
      foreach (ISystemCommand systemElement in this.SystemElements)
      {
        if (systemElement.EnabledInput)
          systemElement.OnUpdateInput();
      }
    }

    protected void SendPressedActionEventToSelectedObject()
    {
      foreach (IActionCommand actionElement in this.ActionElements)
      {
        if (actionElement.EnabledInput)
          actionElement.OnUpdateInput();
      }
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
        bool flag2 = this._state == Input.ValidType.UI && (this.IsPressedHorizontal() || this.IsPressedVertical());
        bool flag3 = (double) Vector2.Dot(rawMoveVector, this._lastMoveVector) > 0.0;
        if (!flag2)
          flag2 = !flag3 || this._consecutiveMoveCount != 1 ? (double) unscaledTime > (double) this._prevActionTime + 1.0 / (double) this._inputActionPerSecond : (double) unscaledTime > (double) this._prevActionTime + (double) this._repeatDelay;
        if (!flag2)
        {
          flag1 = false;
        }
        else
        {
          AxisEventData axisEventData = this.GetAxisEventData((float) rawMoveVector.x, (float) rawMoveVector.y, 0.6f);
          if (axisEventData.get_moveDir() != 4)
          {
            int focusLevel = this.FocusLevel;
            Game instance = Singleton<Game>.Instance;
            if (Object.op_Inequality((Object) instance.ExitScene, (Object) null))
              instance.ExitScene.OnInputMoveDirection(axisEventData.get_moveDir());
            else if (Object.op_Inequality((Object) instance.Dialog, (Object) null))
              instance.Dialog.OnInputMoveDirection(axisEventData.get_moveDir());
            else if (!Object.op_Inequality((Object) instance.Config, (Object) null) && !Object.op_Inequality((Object) instance.MapShortcutUI, (Object) null))
            {
              foreach (MenuUIBehaviour menuElement in this._menuElements)
              {
                if (menuElement.EnabledInput && menuElement.FocusLevel == focusLevel)
                  menuElement.OnInputMoveDirection(axisEventData.get_moveDir());
              }
            }
            if (!flag3)
              this._consecutiveMoveCount = 0;
            ++this._consecutiveMoveCount;
            this._prevActionTime = unscaledTime;
            this._lastMoveVector = rawMoveVector;
          }
          else
            this._consecutiveMoveCount = 0;
          flag1 = ((AbstractEventData) axisEventData).get_used();
        }
      }
      return flag1;
    }

    private Vector2 GetRawMoveVector()
    {
      Vector2 zero = Vector2.get_zero();
      if (Singleton<Input>.IsInstance() && this._state == Input.ValidType.UI)
      {
        zero.x = this.UIAxisRow.x;
        zero.y = this.UIAxisRow.y;
        if (this.IsPressedHorizontal())
        {
          if (zero.x < 0.0)
            zero.x = (__Null) -1.0;
          if (zero.x > 0.0)
            zero.x = (__Null) 1.0;
        }
        if (this.IsPressedVertical())
        {
          if (zero.y < 0.0)
            zero.y = (__Null) -1.0;
          if (zero.y > 0.0)
            zero.y = (__Null) 1.0;
        }
      }
      return zero;
    }

    protected BaseEventData GetBaseEventData()
    {
      if (this._baseEventData == null)
        this._baseEventData = new BaseEventData(EventSystem.get_current());
      ((AbstractEventData) this._baseEventData).Reset();
      return this._baseEventData;
    }

    protected AxisEventData GetAxisEventData(float x, float y, float moveDeadZone)
    {
      if (this._axisEventData == null)
        this._axisEventData = new AxisEventData(EventSystem.get_current());
      ((AbstractEventData) this._axisEventData).Reset();
      this._axisEventData.set_moveVector(new Vector2(x, y));
      this._axisEventData.set_moveDir(Input.DetermineMoveDirection(x, y, moveDeadZone));
      return this._axisEventData;
    }

    protected static MoveDirection DetermineMoveDirection(
      float x,
      float y,
      float deadZone)
    {
      Vector2 vector2;
      ((Vector2) ref vector2).\u002Ector(x, y);
      return (double) ((Vector2) ref vector2).get_sqrMagnitude() >= (double) deadZone ? ((double) Mathf.Abs(x) <= (double) Mathf.Abs(y) ? ((double) y <= 0.0 ? (MoveDirection) 3 : (MoveDirection) 1) : ((double) x <= 0.0 ? (MoveDirection) 0 : (MoveDirection) 2)) : (MoveDirection) 4;
    }

    protected bool SendSubMoveEvent()
    {
      float unscaledTime = Time.get_unscaledTime();
      Vector2 subRawMoveVector = this.GetSubRawMoveVector();
      bool flag1;
      if (Mathf.Approximately((float) subRawMoveVector.x, 0.0f) && Mathf.Approximately((float) subRawMoveVector.y, 0.0f))
      {
        this._subConsecutiveMoveCount = 0;
        flag1 = false;
      }
      else
      {
        bool flag2 = false;
        bool flag3 = (double) Vector2.Dot(subRawMoveVector, this._subLastMoveVector) > 0.0;
        if (!flag2)
          flag2 = !flag3 || this._subConsecutiveMoveCount != 1 ? (double) unscaledTime > (double) this._subPrevActionTime + 1.0 / (double) this._inputActionPerSecond : (double) unscaledTime > (double) this._subPrevActionTime + (double) this._repeatDelay;
        if (!flag2)
        {
          flag1 = false;
        }
        else
        {
          AxisEventData axisEventData = this.GetAxisEventData((float) subRawMoveVector.x, (float) subRawMoveVector.y, 0.6f);
          if (axisEventData.get_moveDir() != 4)
          {
            int focusLevel = this.FocusLevel;
            Game instance = Singleton<Game>.Instance;
            if (Object.op_Inequality((Object) instance.ExitScene, (Object) null))
              instance.ExitScene.OnInputSubMoveDirection(axisEventData.get_moveDir());
            else if (Object.op_Inequality((Object) instance.Dialog, (Object) null))
              instance.Dialog.OnInputSubMoveDirection(axisEventData.get_moveDir());
            else if (!Object.op_Inequality((Object) instance.Config, (Object) null) && !Object.op_Inequality((Object) instance.MapShortcutUI, (Object) null))
            {
              foreach (MenuUIBehaviour menuElement in this._menuElements)
              {
                if (menuElement.EnabledInput && menuElement.FocusLevel == focusLevel)
                  menuElement.OnInputSubMoveDirection(axisEventData.get_moveDir());
              }
            }
            if (!flag3)
              this._subConsecutiveMoveCount = 0;
            ++this._subConsecutiveMoveCount;
            this._subPrevActionTime = unscaledTime;
            this._subLastMoveVector = subRawMoveVector;
          }
          else
            this._subConsecutiveMoveCount = 0;
          flag1 = ((AbstractEventData) axisEventData).get_used();
        }
      }
      return flag1;
    }

    private Vector2 GetSubRawMoveVector()
    {
      Vector2 zero = Vector2.get_zero();
      if (Singleton<Input>.IsInstance() && this._state == Input.ValidType.UI)
      {
        zero.x = this.CameraAxis.x;
        zero.y = this.CameraAxis.y;
        if (this.IsPressedHorizontal())
        {
          if (zero.x < 0.0)
            zero.x = (__Null) -1.0;
          if (zero.x > 0.0)
            zero.x = (__Null) 1.0;
        }
        if (this.IsPressedVertical())
        {
          if (zero.y < 0.0)
            zero.y = (__Null) -1.0;
          if (zero.y > 0.0)
            zero.y = (__Null) 1.0;
        }
      }
      return zero;
    }

    protected bool SendSubmitEventToSelectedObject()
    {
      BaseEventData baseEventData = this.GetBaseEventData();
      if (this._state == Input.ValidType.UI)
      {
        int focusLevel = this.FocusLevel;
        Game instance = Singleton<Game>.Instance;
        if (Object.op_Inequality((Object) instance.ExitScene, (Object) null))
          instance.ExitScene.OnUpdateInput(this);
        else if (Object.op_Inequality((Object) instance.Dialog, (Object) null))
          instance.Dialog.OnUpdateInput(this);
        else if (!Object.op_Inequality((Object) instance.Config, (Object) null))
        {
          if (Object.op_Inequality((Object) instance.MapShortcutUI, (Object) null))
          {
            if (instance.MapShortcutUI.EnabledInput)
              instance.MapShortcutUI.OnUpdateInput(this);
          }
          else
          {
            foreach (MenuUIBehaviour menuElement in this._menuElements)
            {
              if (!Object.op_Equality((Object) menuElement, (Object) null) && menuElement.EnabledInput && menuElement.FocusLevel == focusLevel)
                menuElement.OnUpdateInput(this);
            }
          }
        }
      }
      return ((AbstractEventData) baseEventData).get_used();
    }

    protected bool SendUpdateEventToSelectedObject()
    {
      return ((AbstractEventData) this.GetBaseEventData()).get_used();
    }

    public enum ValidType
    {
      None,
      Action,
      UI,
    }
  }
}
