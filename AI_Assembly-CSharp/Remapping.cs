// Decompiled with JetBrains decompiler
// Type: Remapping
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Remapping : MonoBehaviour
{
  private const Remapping.SettingControllerMode settingControllerMode = Remapping.SettingControllerMode.JoystickKeyboard;
  private Remapping.UpdateMode updateMode;
  private const string categoryName = "Default";
  private const string layoutName = "Default";
  public GameObject actionNameTextPrefab;
  public GameObject inputButtonPrefab;
  public RectTransform actionNameSortArea;
  public RectTransform keyInputButtonSortArea;
  public Text messageText;
  public GameObject settingOrDeleteWindow;
  public Button settingButton;
  public Button deleteButton;
  public Button returnButton;
  public Dropdown controllerSelectDropDown;
  public ControllerSelect controllerSelecter;
  private ControllerType selectedControllerType;
  private int joystickIndexId_;
  private int selectedControllerId;
  private List<Remapping.Row> rows;
  private Remapping.Row row;
  private List<ControllerSetting> controllerList;
  private const string saveFileDirectoryName = "UserData/save/";
  private const string saveFileName = "keyconfig";

  public Remapping()
  {
    base.\u002Ector();
  }

  private bool CheckKeyboardSetting()
  {
    return true;
  }

  private bool CheckMouseSetting()
  {
    return false;
  }

  private int joystickIndexId
  {
    get
    {
      if (0 < this.joystickCount)
      {
        if (this.joystickIndexId_ < 0)
          this.joystickIndexId_ = 0;
        else if (this.joystickCount <= this.joystickIndexId_)
          this.joystickIndexId_ = this.joystickCount - 1;
      }
      return this.joystickIndexId_;
    }
    set
    {
      this.joystickIndexId_ = value;
      if (this.joystickCount <= 0)
        this.joystickIndexId_ = -1;
      else if (this.joystickIndexId_ < 0)
        this.joystickIndexId_ = 0;
      else if (this.joystickCount <= this.joystickIndexId_)
        this.joystickIndexId_ = this.joystickCount - 1;
      this.SetJoystickActiv();
    }
  }

  private IList<ActionElementMap> actionElementMaps
  {
    get
    {
      return this.controllerMap != null ? this.controllerMap.get_AllMaps() : (IList<ActionElementMap>) null;
    }
  }

  private Player player
  {
    get
    {
      return ReInput.get_players().GetPlayer(0);
    }
  }

  private Controller controller
  {
    get
    {
      return this.player != null ? ((Player.ControllerHelper) this.player.controllers).GetController(this.selectedControllerType, this.selectedControllerId) : (Controller) null;
    }
  }

  private int joystickCount
  {
    get
    {
      return this.player != null ? ((Player.ControllerHelper) this.player.controllers).get_joystickCount() : 0;
    }
  }

  private IList<Joystick> joysticks
  {
    get
    {
      return this.player != null ? ((Player.ControllerHelper) this.player.controllers).get_Joysticks() : (IList<Joystick>) null;
    }
  }

  private Joystick joystick
  {
    get
    {
      return this.joysticks != null && 0 <= this.joystickIndexId && this.joystickIndexId < ((ICollection<Joystick>) this.joysticks).Count ? this.joysticks[this.joystickIndexId] : (Joystick) null;
    }
  }

  private ControllerMap controllerMap
  {
    get
    {
      return this.controller == null ? (ControllerMap) null : ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) this.player.controllers).maps).GetMap(this.controller.get_type(), (int) this.controller.id, "Default", "Default");
    }
  }

  private void OnEnable()
  {
    this.updateMode = Remapping.UpdateMode.ButtonSelectMode;
    this.selectedControllerType = (ControllerType) 2;
    this.selectedControllerId = 0;
    this.settingOrDeleteWindow.SetActive(false);
    if (!ReInput.get_isReady())
      return;
    ReInput.add_ControllerConnectedEvent(new Action<ControllerStatusChangedEventArgs>(this.OnControllerChanged));
    ReInput.add_ControllerDisconnectedEvent(new Action<ControllerStatusChangedEventArgs>(this.OnControllerChanged));
    ((UnityEventBase) this.settingButton.get_onClick()).RemoveAllListeners();
    ((UnityEventBase) this.deleteButton.get_onClick()).RemoveAllListeners();
    ((UnityEventBase) this.returnButton.get_onClick()).RemoveAllListeners();
    // ISSUE: method pointer
    ((UnityEvent) this.settingButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnEnable\u003Em__0)));
    // ISSUE: method pointer
    ((UnityEvent) this.deleteButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnEnable\u003Em__1)));
    // ISSUE: method pointer
    ((UnityEvent) this.returnButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnEnable\u003Em__2)));
    this.SetJoystickActiv();
    this.SetControllerToDropdown();
    this.LoadControllerSetting();
    this.InitializeUI();
  }

  private void OnDisable()
  {
    ReInput.remove_ControllerConnectedEvent(new Action<ControllerStatusChangedEventArgs>(this.OnControllerChanged));
    ReInput.remove_ControllerDisconnectedEvent(new Action<ControllerStatusChangedEventArgs>(this.OnControllerChanged));
  }

  private void Start()
  {
    ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.U_Update()));
  }

  private void InitializeUI()
  {
    IEnumerator enumerator1 = ((Transform) this.actionNameSortArea).GetEnumerator();
    try
    {
      while (enumerator1.MoveNext())
        Object.Destroy((Object) ((Component) enumerator1.Current).get_gameObject());
    }
    finally
    {
      if (enumerator1 is IDisposable disposable)
        disposable.Dispose();
    }
    IEnumerator enumerator2 = ((Transform) this.keyInputButtonSortArea).GetEnumerator();
    try
    {
      while (enumerator2.MoveNext())
        Object.Destroy((Object) ((Component) enumerator2.Current).get_gameObject());
    }
    finally
    {
      if (enumerator2 is IDisposable disposable)
        disposable.Dispose();
    }
    this.rows.Clear();
    using (IEnumerator<InputAction> enumerator3 = ReInput.get_mapping().ActionsInCategory("Default").GetEnumerator())
    {
      while (((IEnumerator) enumerator3).MoveNext())
      {
        InputAction current = enumerator3.Current;
        if (current.get_userAssignable())
        {
          if (current.get_type() == 1)
            this.CreateUIRow(current, (AxisRange) 1, current.get_descriptiveName());
          else if (current.get_type() == null)
          {
            this.CreateUIRow(current, (AxisRange) 0, current.get_descriptiveName());
            this.CreateUIRow(current, (AxisRange) 1, string.IsNullOrEmpty(current.get_positiveDescriptiveName()) ? current.get_descriptiveName() + " +" : current.get_positiveDescriptiveName());
            this.CreateUIRow(current, (AxisRange) 2, string.IsNullOrEmpty(current.get_negativeDescriptiveName()) ? current.get_descriptiveName() + " -" : current.get_negativeDescriptiveName());
          }
        }
      }
    }
    this.RedrawUI();
  }

  private void CreateUIRow(InputAction _action, AxisRange _actionRange, string _actionName)
  {
    GameObject gameObject1 = (GameObject) Object.Instantiate<GameObject>((M0) this.actionNameTextPrefab);
    gameObject1.get_transform().SetParent((Transform) this.actionNameSortArea);
    gameObject1.get_transform().SetAsLastSibling();
    ((Text) gameObject1.GetComponent<Text>()).set_text(_actionName);
    GameObject gameObject2 = (GameObject) Object.Instantiate<GameObject>((M0) this.inputButtonPrefab);
    gameObject2.get_transform().SetParent((Transform) this.keyInputButtonSortArea);
    gameObject2.get_transform().SetAsLastSibling();
    this.rows.Add(new Remapping.Row()
    {
      action = _action,
      actionRange = _actionRange,
      button = (Button) gameObject2.GetComponent<Button>(),
      text = (Text) gameObject2.GetComponentInChildren<Text>()
    });
  }

  private void RedrawUI()
  {
    if (this.controller == null)
    {
      this.ClearUI();
    }
    else
    {
      this.ControllerMappingToSetting();
      this.messageText.set_text("編集したいアクションを選択してください");
      for (int index = 0; index < this.rows.Count; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        Remapping.\u003CRedrawUI\u003Ec__AnonStorey0 redrawUiCAnonStorey0 = new Remapping.\u003CRedrawUI\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        redrawUiCAnonStorey0.\u0024this = this;
        Remapping.Row row = this.rows[index];
        InputAction action = row.action;
        string str = string.Empty;
        using (IEnumerator<ActionElementMap> enumerator = this.controllerMap.ElementMapsWithAction(action.get_id()).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
          {
            ActionElementMap current = enumerator.Current;
            if (current.ShowInField(row.actionRange))
            {
              str = current.get_elementIdentifierName();
              break;
            }
          }
        }
        row.text.set_text(str);
        ((UnityEventBase) row.button.get_onClick()).RemoveAllListeners();
        // ISSUE: reference to a compiler-generated field
        redrawUiCAnonStorey0._index = index;
        // ISSUE: method pointer
        ((UnityEvent) row.button.get_onClick()).AddListener(new UnityAction((object) redrawUiCAnonStorey0, __methodptr(\u003C\u003Em__0)));
      }
    }
  }

  private void ClearUI()
  {
    this.messageText.set_text("ボタンのテキストを空にしました");
    for (int index = 0; index < this.rows.Count; ++index)
      this.rows[index].text.set_text(string.Empty);
  }

  private void OnInputFieldClicked(int _buttonIndex)
  {
    if (_buttonIndex < 0 || this.rows.Count <= _buttonIndex || this.controller == null)
      return;
    ((Selectable) this.controllerSelectDropDown).set_interactable(false);
    this.row = this.rows[_buttonIndex];
    this.updateMode = Remapping.UpdateMode.SettingOrDelete;
    this.messageText.set_text(string.Empty);
    this.settingOrDeleteWindow.SetActive(true);
  }

  private void OnInputSetting()
  {
    this.messageText.set_text("キーを押してください");
    this.CloseWindow(Remapping.UpdateMode.InputCheckMode);
  }

  private void OnDeleteInputSetting()
  {
    if (this.row != null)
    {
      if (this.controllerMap != null)
        this.controllerMap.DeleteElementMapsWithAction(this.row.action.get_id());
      this.row = (Remapping.Row) null;
    }
    this.CloseWindow(Remapping.UpdateMode.ButtonSelectMode);
    ((Selectable) this.controllerSelectDropDown).set_interactable(true);
    this.RedrawUI();
  }

  private void SetControllerToDropdown()
  {
    IList<Controller> _controllers = (IList<Controller>) new List<Controller>();
    if (this.CheckKeyboardSetting())
      ((ICollection<Controller>) _controllers).Add(((Player.ControllerHelper) this.player.controllers).GetController((ControllerType) 0, 0));
    if (this.CheckMouseSetting())
      ((ICollection<Controller>) _controllers).Add(((Player.ControllerHelper) this.player.controllers).GetController((ControllerType) 1, 0));
    for (int index = 0; index < this.joystickCount; ++index)
    {
      int id = (int) ((Controller) this.joysticks[index]).id;
      ((ICollection<Controller>) _controllers).Add(((Player.ControllerHelper) this.player.controllers).GetController((ControllerType) 2, id));
    }
    this.controllerSelecter.SetController(_controllers, this.controller == null ? string.Empty : this.controller.get_name());
  }

  public void SetSelectedController(int _controllerType, int _joystickIndexId)
  {
    if (_controllerType == 2)
      this.joystickIndexId = _joystickIndexId;
    this.SetSelectedController((ControllerType) _controllerType);
  }

  private void OnReturnToButtonSelectMode()
  {
    this.row = (Remapping.Row) null;
    this.CloseWindow(Remapping.UpdateMode.ButtonSelectMode);
    ((Selectable) this.controllerSelectDropDown).set_interactable(true);
  }

  private void CloseWindow(Remapping.UpdateMode _updateMode)
  {
    this.updateMode = _updateMode;
    this.settingOrDeleteWindow.SetActive(false);
  }

  private string GetControllerName(ControllerType _controllerType, int _controllerId)
  {
    return string.Copy(((Player.ControllerHelper) ReInput.get_players().GetPlayer(0).controllers).GetController(_controllerType, _controllerId).get_hardwareName());
  }

  private void U_Update()
  {
    switch (this.updateMode)
    {
      case Remapping.UpdateMode.ButtonSelectMode:
        this.UpdateButtonSelectMode();
        break;
      case Remapping.UpdateMode.InputCheckMode:
        this.UpdateInputCheckMode();
        break;
    }
  }

  private void UpdateButtonSelectMode()
  {
    if (Input.GetKeyDown((KeyCode) 115))
    {
      this.SaveControllerSetting();
      this.RedrawUI();
    }
    else if (Input.GetKeyDown((KeyCode) 108))
      this.LoadControllerSetting();
    if (!Input.GetKeyDown((KeyCode) 105))
      return;
    Debug.Log((object) this.joystickCount);
  }

  private void UpdateInputCheckMode()
  {
    ControllerPollingInfo _pollingInfo = ((ReInput.ControllerHelper.PollingHelper) ReInput.get_controllers().polling).PollControllerForFirstElementDown(this.selectedControllerType, this.selectedControllerId);
    if (!((ControllerPollingInfo) ref _pollingInfo).get_success() || ((ControllerPollingInfo) ref _pollingInfo).get_elementType() != 1)
      return;
    InputAction action = this.row.action;
    ActionElementMap _actionElementMap = !this.controllerMap.ContainsAction(action.get_id()) ? (ActionElementMap) null : this.controllerMap.GetElementMapsWithAction(action.get_id())[0];
    ElementAssignment elementAssignment = this.ToElementAssignment(_pollingInfo, (ModifierKeyFlags) 0, this.row.actionRange, action.get_id(), _actionElementMap);
    ElementAssignmentConflictCheck _conflictCheck = (ElementAssignmentConflictCheck) null;
    if (this.CreateConflictCheck(elementAssignment, out _conflictCheck, _actionElementMap) && !((ReInput.ControllerHelper.ConflictCheckingHelper) ReInput.get_controllers().conflictChecking).DoesElementAssignmentConflict(_conflictCheck))
      this.controllerMap.ReplaceOrCreateElementMap(elementAssignment);
    this.row = (Remapping.Row) null;
    this.updateMode = Remapping.UpdateMode.ButtonSelectMode;
    ((Selectable) this.controllerSelectDropDown).set_interactable(true);
    this.RedrawUI();
  }

  private void SetSelectedController(ControllerType _controllerType)
  {
    bool flag = true;
    if (this.selectedControllerType != _controllerType)
    {
      this.selectedControllerType = _controllerType;
      flag = true;
    }
    int selectedControllerId = this.selectedControllerId;
    if (this.selectedControllerType == 2)
    {
      if (this.joystickCount > 0)
      {
        if (this.joystickIndexId < 0)
          this.joystickIndexId = 0;
        else if (this.joystickCount <= this.joystickIndexId)
          this.joystickIndexId = this.joystickCount - 1;
        this.selectedControllerId = (int) ((Controller) this.joysticks[this.joystickIndexId]).id;
      }
      else
      {
        int num = -1;
        this.joystickIndexId = num;
        this.selectedControllerId = num;
      }
    }
    else
      this.selectedControllerId = 0;
    if (this.selectedControllerId != selectedControllerId)
      flag = true;
    this.SetControllerToDropdown();
    if (!flag)
      return;
    this.ControllerSettingToMapping(_controllerType);
    this.RedrawUI();
  }

  private void OnControllerChanged(ControllerStatusChangedEventArgs _args)
  {
    this.SetSelectedController(this.selectedControllerType);
  }

  public void OnControllerSelected(int _controllerType)
  {
    this.SetSelectedController((ControllerType) _controllerType);
  }

  private ElementAssignment ToElementAssignment(
    ControllerPollingInfo _pollingInfo,
    ModifierKeyFlags _modifierKeyFlag,
    AxisRange _axisRange,
    int _actionId,
    ActionElementMap _actionElementMap)
  {
    AxisRange axisRange = (AxisRange) 1;
    if (((ControllerPollingInfo) ref _pollingInfo).get_elementType() == null)
      axisRange = _axisRange != null ? (((ControllerPollingInfo) ref _pollingInfo).get_axisPole() != null ? (AxisRange) 2 : (AxisRange) 1) : (AxisRange) 0;
    return new ElementAssignment(((ControllerPollingInfo) ref _pollingInfo).get_controllerType(), ((ControllerPollingInfo) ref _pollingInfo).get_elementType(), ((ControllerPollingInfo) ref _pollingInfo).get_elementIdentifierId(), axisRange, ((ControllerPollingInfo) ref _pollingInfo).get_keyboardKey(), _modifierKeyFlag, _actionId, _axisRange != 2 ? (Pole) 0 : (Pole) 1, false, _actionElementMap == null ? -1 : _actionElementMap.get_id());
  }

  private bool CreateConflictCheck(
    ElementAssignment _elementAssignment,
    out ElementAssignmentConflictCheck _conflictCheck,
    ActionElementMap _actionElementMap)
  {
    if (this.controllerMap == null || this.player == null)
    {
      _conflictCheck = (ElementAssignmentConflictCheck) null;
      return false;
    }
    _conflictCheck = ((ElementAssignment) ref _elementAssignment).ToElementAssignmentConflictCheck();
    ((ElementAssignmentConflictCheck) ref _conflictCheck).set_playerId(this.player.get_id());
    ((ElementAssignmentConflictCheck) ref _conflictCheck).set_controllerType(this.controllerMap.get_controllerType());
    ((ElementAssignmentConflictCheck) ref _conflictCheck).set_controllerMapId(this.controllerMap.get_id());
    ((ElementAssignmentConflictCheck) ref _conflictCheck).set_controllerMapCategoryId(this.controllerMap.get_categoryId());
    if (_actionElementMap != null)
      ((ElementAssignmentConflictCheck) ref _conflictCheck).set_elementMapId(_actionElementMap.get_id());
    return true;
  }

  private void SetJoystickActiv()
  {
    using (IEnumerator<Joystick> enumerator = ((IEnumerable<Joystick>) ((Player.ControllerHelper) this.player.controllers).get_Joysticks()).GetEnumerator())
    {
      while (((IEnumerator) enumerator).MoveNext())
        ((Controller) enumerator.Current).set_enabled(false);
    }
    if (this.joystick == null)
      return;
    ((Controller) this.joystick).set_enabled(true);
  }

  public int GetJoystickIndexId()
  {
    return this.joystickIndexId;
  }

  private void DeleteControllerSetting(ControllerType _controllerType, string _controllerName)
  {
    for (int index = 0; index < this.controllerList.Count; ++index)
    {
      if (_controllerType == null || _controllerType == 1 ? _controllerType == this.controllerList[index].controllerType : _controllerName == this.controllerList[index].controllerName)
      {
        this.controllerList.RemoveAt(index);
        --index;
      }
    }
  }

  private void ControllerSettingToMapping()
  {
    if (this.CheckKeyboardSetting())
      this.ControllerSettingToMapping((ControllerType) 0);
    if (this.CheckMouseSetting())
      this.ControllerSettingToMapping((ControllerType) 1);
    this.ControllerSettingToMapping((ControllerType) 2);
  }

  private void ControllerSettingToMapping(ControllerType _controllerType)
  {
    if (_controllerType == 2 && this.joystickCount <= 0)
      return;
    int num1 = _controllerType != 2 ? 1 : this.joystickCount;
    for (int index = 0; index < num1; ++index)
    {
      ControllerSetting controllerSetting = (ControllerSetting) null;
      foreach (ControllerSetting controller in this.controllerList)
      {
        if (_controllerType == null || _controllerType == 1)
        {
          if (controller.controllerType == _controllerType)
          {
            controllerSetting = controller;
            break;
          }
        }
        else if (this.joysticks != null)
        {
          if (((Controller) this.joysticks[index]).get_hardwareName() == controller.controllerName)
          {
            controllerSetting = controller;
            break;
          }
        }
        else
          break;
      }
      if (controllerSetting != null)
      {
        int num2 = _controllerType != 2 ? 0 : (int) ((Controller) this.joysticks[index]).id;
        using (IEnumerator<ControllerMap> enumerator = ((IEnumerable<ControllerMap>) ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) this.player.controllers).maps).GetMaps(controllerSetting.controllerType, num2)).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
            enumerator.Current.ClearElementMaps();
        }
        foreach (InputSetting element in controllerSetting.elements)
        {
          int categoryId = element.categoryId;
          int layoutId = element.layoutId;
          ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) this.player.controllers).maps).GetMap(controllerSetting.controllerType, num2, categoryId, layoutId).ReplaceOrCreateElementMap(element.ToElementAssignmentCreateSetting());
        }
      }
    }
  }

  private void ControllerMappingToSetting()
  {
    if (this.CheckKeyboardSetting())
      this.ControllerMappingToSetting((ControllerType) 0);
    if (this.CheckMouseSetting())
      this.ControllerMappingToSetting((ControllerType) 1);
    this.ControllerMappingToSetting((ControllerType) 2);
  }

  private void ControllerMappingToSetting(ControllerType _controllerType)
  {
    List<ControllerSetting> controllerSettingList = new List<ControllerSetting>();
    switch ((int) _controllerType)
    {
      case 0:
      case 1:
        this.DeleteControllerSetting(_controllerType, (string) null);
        ControllerSetting controllerSetting1 = new ControllerSetting(_controllerType, 0);
        if (_controllerType == null)
        {
          controllerSettingList.Add(controllerSetting1);
          break;
        }
        break;
      case 2:
        for (int index = 0; index < this.joystickCount; ++index)
        {
          ControllerSetting controllerSetting2 = new ControllerSetting(_controllerType, (int) ((Controller) this.joysticks[index]).id);
          this.DeleteControllerSetting(_controllerType, controllerSetting2.controllerName);
          controllerSettingList.Add(controllerSetting2);
        }
        break;
    }
    foreach (ControllerSetting controllerSetting2 in controllerSettingList)
    {
      if (controllerSetting2.maps != null)
      {
        this.controllerList.Add(controllerSetting2);
        using (IEnumerator<ControllerMap> enumerator1 = ((IEnumerable<ControllerMap>) controllerSetting2.maps).GetEnumerator())
        {
          while (((IEnumerator) enumerator1).MoveNext())
          {
            using (IEnumerator<ActionElementMap> enumerator2 = ((IEnumerable<ActionElementMap>) enumerator1.Current.get_AllMaps()).GetEnumerator())
            {
              while (((IEnumerator) enumerator2).MoveNext())
              {
                ActionElementMap current = enumerator2.Current;
                controllerSetting2.AddElement(new InputSetting(current));
              }
            }
          }
        }
      }
    }
    controllerSettingList.Clear();
  }

  private void SaveControllerSetting()
  {
    this.ControllerMappingToSetting();
    if (!Directory.Exists("UserData/save/"))
      Directory.CreateDirectory("UserData/save/");
    this.SaveFile("UserData/save/keyconfig");
  }

  public void SaveFile(string _path)
  {
    using (FileStream fileStream = new FileStream(_path, FileMode.Create, FileAccess.Write, FileShare.Write))
      this.SaveFile((Stream) fileStream);
  }

  public void SaveFile(Stream _stream)
  {
    using (BinaryWriter _writer = new BinaryWriter(_stream))
      this.SaveFile(_writer);
  }

  public void SaveFile(BinaryWriter _writer)
  {
    _writer.Write(this.controllerList.Count);
    foreach (ControllerSetting controller in this.controllerList)
    {
      _writer.Write(controller.controllerName);
      _writer.Write((int) controller.controllerType);
      _writer.Write(controller.elements.Count);
      foreach (InputSetting element in controller.elements)
      {
        _writer.Write(element.categoryId);
        _writer.Write(element.layoutId);
        _writer.Write((int) element.controllerType);
        _writer.Write((int) element.elementType);
        _writer.Write(element.elementIdentifierId);
        _writer.Write((int) element.axisRange);
        _writer.Write((int) element.keyboardKey);
        _writer.Write((int) element.modifierKeyFlags);
        _writer.Write(element.actionId);
        _writer.Write((int) element.axisContribution);
        _writer.Write(element.invert);
        _writer.Write(element.elementMapId);
      }
    }
    Debug.Log((object) "セーブ完了：SaveFile(BinaryWriter _writer)");
  }

  private void LoadControllerSetting()
  {
    this.LoadFile("UserData/save/keyconfig");
    this.ControllerSettingToMapping();
    this.RedrawUI();
  }

  public void LoadFile(string _path)
  {
    try
    {
      using (FileStream fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        if (fileStream.Length == 0L)
          Debug.LogError((object) ("開いたファイルは空です：" + _path));
        else
          this.Load((Stream) fileStream);
      }
    }
    catch (Exception ex)
    {
      switch (ex)
      {
        case FileNotFoundException _:
          Debug.LogError((object) ("ファイルが見つかりません：" + _path));
          return;
        case DirectoryNotFoundException _:
          Debug.LogError((object) ("ディレクトリが存在しません：" + Path.GetDirectoryName(_path)));
          break;
      }
      Debug.LogException(ex);
    }
  }

  public void Load(Stream _stream)
  {
    using (BinaryReader _reader = new BinaryReader(_stream))
      this.Load(_reader);
  }

  public void Load(BinaryReader _reader)
  {
    try
    {
      int num1 = _reader.ReadInt32();
      for (int index1 = 0; index1 < num1; ++index1)
      {
        string _controllerName = _reader.ReadString();
        ControllerType _controllerType = (ControllerType) _reader.ReadInt32();
        int num2 = _reader.ReadInt32();
        this.DeleteControllerSetting(_controllerType, _controllerName);
        ControllerSetting controllerSetting = new ControllerSetting(_controllerName, _controllerType);
        for (int index2 = 0; index2 < num2; ++index2)
        {
          InputSetting _inputSetting = new InputSetting(_reader.ReadInt32(), _reader.ReadInt32(), (ControllerType) _reader.ReadInt32(), (ControllerElementType) _reader.ReadInt32(), _reader.ReadInt32(), (AxisRange) _reader.ReadInt32(), (KeyCode) _reader.ReadInt32(), (ModifierKeyFlags) _reader.ReadInt32(), _reader.ReadInt32(), (Pole) _reader.ReadInt32(), _reader.ReadBoolean(), _reader.ReadInt32());
          controllerSetting.AddElement(_inputSetting);
        }
        this.controllerList.Add(controllerSetting);
      }
      Debug.Log((object) "ロードに成功しました：Load(BinaryReader _reader)");
    }
    catch (Exception ex)
    {
      Debug.LogException(ex);
    }
  }

  private void ControllerDefaultSetting()
  {
  }

  private void LogActionMap()
  {
    using (IEnumerator<InputAction> enumerator = ((IEnumerable<InputAction>) ReInput.get_mapping().get_Actions()).GetEnumerator())
    {
      while (((IEnumerator) enumerator).MoveNext())
      {
        InputAction current = enumerator.Current;
        Debug.Log((object) "↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓");
        Debug.Log((object) ("ID → " + (object) current.get_id()));
        Debug.Log((object) ("Name → " + current.get_name()));
        Debug.Log((object) ("Type → " + (object) current.get_type()));
        Debug.Log((object) ("DescriptiveName → " + current.get_descriptiveName()));
        Debug.Log((object) ("PositiveDescriptiveName → " + current.get_positiveDescriptiveName()));
        Debug.Log((object) ("NegativeDescriptiveName → " + current.get_negativeDescriptiveName()));
        Debug.Log((object) ("BehaviorID → " + (object) current.get_behaviorId()));
        Debug.Log((object) ("CategoryID → " + (object) current.get_categoryId()));
        Debug.Log((object) ("UserAssignble → " + (object) current.get_userAssignable()));
        Debug.Log((object) "↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑");
      }
    }
  }

  private void LogControllerAllButton()
  {
    if (this.selectedControllerType == null)
    {
      Keyboard keyboard = ((Player.ControllerHelper) this.player.controllers).get_Keyboard();
      if (keyboard == null)
        return;
      int num = 0;
      Debug.Log((object) "Keyboard ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
      using (IEnumerator<ControllerElementIdentifier> enumerator = ((IEnumerable<ControllerElementIdentifier>) ((Controller) keyboard).get_ButtonElementIdentifiers()).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          this.LogControllerElementIdentifier(enumerator.Current);
          ++num;
        }
      }
      Debug.Log((object) "Count↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓");
      Debug.Log((object) num);
    }
    else if (this.selectedControllerType == 1)
    {
      Mouse mouse = ((Player.ControllerHelper) this.player.controllers).get_Mouse();
      if (mouse == null)
        return;
      int num = 0;
      Debug.Log((object) "Keyboard ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
      using (IEnumerator<ControllerElementIdentifier> enumerator = ((IEnumerable<ControllerElementIdentifier>) ((Controller) mouse).get_ButtonElementIdentifiers()).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          this.LogControllerElementIdentifier(enumerator.Current);
          ++num;
        }
      }
      Debug.Log((object) "Count↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓");
      Debug.Log((object) num);
    }
    else
    {
      if (this.selectedControllerType != 2 || this.joystick == null)
        return;
      int num = 0;
      Debug.Log((object) "Joystick ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
      using (IEnumerator<ControllerElementIdentifier> enumerator = ((IEnumerable<ControllerElementIdentifier>) ((ControllerWithAxes) this.joystick).get_AxisElementIdentifiers()).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          this.LogControllerElementIdentifier(enumerator.Current);
          ++num;
        }
      }
      using (IEnumerator<ControllerElementIdentifier> enumerator = ((IEnumerable<ControllerElementIdentifier>) ((Controller) this.joystick).get_ButtonElementIdentifiers()).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          this.LogControllerElementIdentifier(enumerator.Current);
          ++num;
        }
      }
      Debug.Log((object) "Count↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓");
      Debug.Log((object) num);
    }
  }

  private void LogControllerElementIdentifier(ControllerElementIdentifier _element)
  {
    Debug.Log((object) (_element.get_elementType().ToString() + "↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓"));
    Debug.Log((object) ("ElementType → " + (object) _element.get_elementType()));
    Debug.Log((object) ("ID → " + (object) _element.get_id()));
    Debug.Log((object) ("Name → " + _element.get_name()));
    Debug.Log((object) ("PositiveName → " + _element.get_positiveName()));
    Debug.Log((object) ("NegativeName → " + _element.get_negativeName()));
  }

  private enum UpdateMode
  {
    ButtonSelectMode,
    SettingOrDelete,
    InputCheckMode,
  }

  private enum SettingControllerMode
  {
    Joystick,
    JoystickKeyboard,
    AllController,
  }

  private class Row
  {
    public InputAction action;
    public AxisRange actionRange;
    public Button button;
    public Text text;
  }
}
