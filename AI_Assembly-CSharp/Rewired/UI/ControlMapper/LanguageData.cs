// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.LanguageData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
  public class LanguageData : ScriptableObject
  {
    [SerializeField]
    private string _yes;
    [SerializeField]
    private string _no;
    [SerializeField]
    private string _add;
    [SerializeField]
    private string _replace;
    [SerializeField]
    private string _remove;
    [SerializeField]
    private string _swap;
    [SerializeField]
    private string _cancel;
    [SerializeField]
    private string _none;
    [SerializeField]
    private string _okay;
    [SerializeField]
    private string _done;
    [SerializeField]
    private string _default;
    [SerializeField]
    private string _assignControllerWindowTitle;
    [SerializeField]
    private string _assignControllerWindowMessage;
    [SerializeField]
    private string _controllerAssignmentConflictWindowTitle;
    [SerializeField]
    [Tooltip("{0} = Joystick Name\n{1} = Other Player Name\n{2} = This Player Name")]
    private string _controllerAssignmentConflictWindowMessage;
    [SerializeField]
    private string _elementAssignmentPrePollingWindowMessage;
    [SerializeField]
    [Tooltip("{0} = Action Name")]
    private string _joystickElementAssignmentPollingWindowMessage;
    [SerializeField]
    [Tooltip("This text is only displayed when split-axis fields have been disabled and the user clicks on the full-axis field. Button/key/D-pad input cannot be assigned to a full-axis field.\n{0} = Action Name")]
    private string _joystickElementAssignmentPollingWindowMessage_fullAxisFieldOnly;
    [SerializeField]
    [Tooltip("{0} = Action Name")]
    private string _keyboardElementAssignmentPollingWindowMessage;
    [SerializeField]
    [Tooltip("{0} = Action Name")]
    private string _mouseElementAssignmentPollingWindowMessage;
    [SerializeField]
    [Tooltip("This text is only displayed when split-axis fields have been disabled and the user clicks on the full-axis field. Button/key/D-pad input cannot be assigned to a full-axis field.\n{0} = Action Name")]
    private string _mouseElementAssignmentPollingWindowMessage_fullAxisFieldOnly;
    [SerializeField]
    private string _elementAssignmentConflictWindowMessage;
    [SerializeField]
    [Tooltip("{0} = Element Name")]
    private string _elementAlreadyInUseBlocked;
    [SerializeField]
    [Tooltip("{0} = Element Name")]
    private string _elementAlreadyInUseCanReplace;
    [SerializeField]
    [Tooltip("{0} = Element Name")]
    private string _elementAlreadyInUseCanReplace_conflictAllowed;
    [SerializeField]
    private string _mouseAssignmentConflictWindowTitle;
    [SerializeField]
    [Tooltip("{0} = Other Player Name\n{1} = This Player Name")]
    private string _mouseAssignmentConflictWindowMessage;
    [SerializeField]
    private string _calibrateControllerWindowTitle;
    [SerializeField]
    private string _calibrateAxisStep1WindowTitle;
    [SerializeField]
    [Tooltip("{0} = Axis Name")]
    private string _calibrateAxisStep1WindowMessage;
    [SerializeField]
    private string _calibrateAxisStep2WindowTitle;
    [SerializeField]
    [Tooltip("{0} = Axis Name")]
    private string _calibrateAxisStep2WindowMessage;
    [SerializeField]
    private string _inputBehaviorSettingsWindowTitle;
    [SerializeField]
    private string _restoreDefaultsWindowTitle;
    [SerializeField]
    [Tooltip("Message for a single player game.")]
    private string _restoreDefaultsWindowMessage_onePlayer;
    [SerializeField]
    [Tooltip("Message for a multi-player game.")]
    private string _restoreDefaultsWindowMessage_multiPlayer;
    [SerializeField]
    private string _actionColumnLabel;
    [SerializeField]
    private string _keyboardColumnLabel;
    [SerializeField]
    private string _mouseColumnLabel;
    [SerializeField]
    private string _controllerColumnLabel;
    [SerializeField]
    private string _removeControllerButtonLabel;
    [SerializeField]
    private string _calibrateControllerButtonLabel;
    [SerializeField]
    private string _assignControllerButtonLabel;
    [SerializeField]
    private string _inputBehaviorSettingsButtonLabel;
    [SerializeField]
    private string _doneButtonLabel;
    [SerializeField]
    private string _restoreDefaultsButtonLabel;
    [SerializeField]
    private string _playersGroupLabel;
    [SerializeField]
    private string _controllerSettingsGroupLabel;
    [SerializeField]
    private string _assignedControllersGroupLabel;
    [SerializeField]
    private string _settingsGroupLabel;
    [SerializeField]
    private string _mapCategoriesGroupLabel;
    [SerializeField]
    private string _calibrateWindow_deadZoneSliderLabel;
    [SerializeField]
    private string _calibrateWindow_zeroSliderLabel;
    [SerializeField]
    private string _calibrateWindow_sensitivitySliderLabel;
    [SerializeField]
    private string _calibrateWindow_invertToggleLabel;
    [SerializeField]
    private string _calibrateWindow_calibrateButtonLabel;
    [SerializeField]
    private LanguageData.CustomEntry[] _customEntries;
    private bool _initialized;
    private Dictionary<string, string> customDict;

    public LanguageData()
    {
      base.\u002Ector();
    }

    public void Initialize()
    {
      if (this._initialized)
        return;
      this.customDict = LanguageData.CustomEntry.ToDictionary(this._customEntries);
      this._initialized = true;
    }

    public string GetCustomEntry(string key)
    {
      string str;
      return string.IsNullOrEmpty(key) || !this.customDict.TryGetValue(key, out str) ? string.Empty : str;
    }

    public bool ContainsCustomEntryKey(string key)
    {
      return !string.IsNullOrEmpty(key) && this.customDict.ContainsKey(key);
    }

    public string yes
    {
      get
      {
        return this._yes;
      }
    }

    public string no
    {
      get
      {
        return this._no;
      }
    }

    public string add
    {
      get
      {
        return this._add;
      }
    }

    public string replace
    {
      get
      {
        return this._replace;
      }
    }

    public string remove
    {
      get
      {
        return this._remove;
      }
    }

    public string swap
    {
      get
      {
        return this._swap;
      }
    }

    public string cancel
    {
      get
      {
        return this._cancel;
      }
    }

    public string none
    {
      get
      {
        return this._none;
      }
    }

    public string okay
    {
      get
      {
        return this._okay;
      }
    }

    public string done
    {
      get
      {
        return this._done;
      }
    }

    public string default_
    {
      get
      {
        return this._default;
      }
    }

    public string assignControllerWindowTitle
    {
      get
      {
        return this._assignControllerWindowTitle;
      }
    }

    public string assignControllerWindowMessage
    {
      get
      {
        return this._assignControllerWindowMessage;
      }
    }

    public string controllerAssignmentConflictWindowTitle
    {
      get
      {
        return this._controllerAssignmentConflictWindowTitle;
      }
    }

    public string elementAssignmentPrePollingWindowMessage
    {
      get
      {
        return this._elementAssignmentPrePollingWindowMessage;
      }
    }

    public string elementAssignmentConflictWindowMessage
    {
      get
      {
        return this._elementAssignmentConflictWindowMessage;
      }
    }

    public string mouseAssignmentConflictWindowTitle
    {
      get
      {
        return this._mouseAssignmentConflictWindowTitle;
      }
    }

    public string calibrateControllerWindowTitle
    {
      get
      {
        return this._calibrateControllerWindowTitle;
      }
    }

    public string calibrateAxisStep1WindowTitle
    {
      get
      {
        return this._calibrateAxisStep1WindowTitle;
      }
    }

    public string calibrateAxisStep2WindowTitle
    {
      get
      {
        return this._calibrateAxisStep2WindowTitle;
      }
    }

    public string inputBehaviorSettingsWindowTitle
    {
      get
      {
        return this._inputBehaviorSettingsWindowTitle;
      }
    }

    public string restoreDefaultsWindowTitle
    {
      get
      {
        return this._restoreDefaultsWindowTitle;
      }
    }

    public string actionColumnLabel
    {
      get
      {
        return this._actionColumnLabel;
      }
    }

    public string keyboardColumnLabel
    {
      get
      {
        return this._keyboardColumnLabel;
      }
    }

    public string mouseColumnLabel
    {
      get
      {
        return this._mouseColumnLabel;
      }
    }

    public string controllerColumnLabel
    {
      get
      {
        return this._controllerColumnLabel;
      }
    }

    public string removeControllerButtonLabel
    {
      get
      {
        return this._removeControllerButtonLabel;
      }
    }

    public string calibrateControllerButtonLabel
    {
      get
      {
        return this._calibrateControllerButtonLabel;
      }
    }

    public string assignControllerButtonLabel
    {
      get
      {
        return this._assignControllerButtonLabel;
      }
    }

    public string inputBehaviorSettingsButtonLabel
    {
      get
      {
        return this._inputBehaviorSettingsButtonLabel;
      }
    }

    public string doneButtonLabel
    {
      get
      {
        return this._doneButtonLabel;
      }
    }

    public string restoreDefaultsButtonLabel
    {
      get
      {
        return this._restoreDefaultsButtonLabel;
      }
    }

    public string controllerSettingsGroupLabel
    {
      get
      {
        return this._controllerSettingsGroupLabel;
      }
    }

    public string playersGroupLabel
    {
      get
      {
        return this._playersGroupLabel;
      }
    }

    public string assignedControllersGroupLabel
    {
      get
      {
        return this._assignedControllersGroupLabel;
      }
    }

    public string settingsGroupLabel
    {
      get
      {
        return this._settingsGroupLabel;
      }
    }

    public string mapCategoriesGroupLabel
    {
      get
      {
        return this._mapCategoriesGroupLabel;
      }
    }

    public string restoreDefaultsWindowMessage
    {
      get
      {
        return ReInput.get_players().get_playerCount() > 1 ? this._restoreDefaultsWindowMessage_multiPlayer : this._restoreDefaultsWindowMessage_onePlayer;
      }
    }

    public string calibrateWindow_deadZoneSliderLabel
    {
      get
      {
        return this._calibrateWindow_deadZoneSliderLabel;
      }
    }

    public string calibrateWindow_zeroSliderLabel
    {
      get
      {
        return this._calibrateWindow_zeroSliderLabel;
      }
    }

    public string calibrateWindow_sensitivitySliderLabel
    {
      get
      {
        return this._calibrateWindow_sensitivitySliderLabel;
      }
    }

    public string calibrateWindow_invertToggleLabel
    {
      get
      {
        return this._calibrateWindow_invertToggleLabel;
      }
    }

    public string calibrateWindow_calibrateButtonLabel
    {
      get
      {
        return this._calibrateWindow_calibrateButtonLabel;
      }
    }

    public string GetControllerAssignmentConflictWindowMessage(
      string joystickName,
      string otherPlayerName,
      string currentPlayerName)
    {
      return string.Format(this._controllerAssignmentConflictWindowMessage, (object) joystickName, (object) otherPlayerName, (object) currentPlayerName);
    }

    public string GetJoystickElementAssignmentPollingWindowMessage(string actionName)
    {
      return string.Format(this._joystickElementAssignmentPollingWindowMessage, (object) actionName);
    }

    public string GetJoystickElementAssignmentPollingWindowMessage_FullAxisFieldOnly(
      string actionName)
    {
      return string.Format(this._joystickElementAssignmentPollingWindowMessage_fullAxisFieldOnly, (object) actionName);
    }

    public string GetKeyboardElementAssignmentPollingWindowMessage(string actionName)
    {
      return string.Format(this._keyboardElementAssignmentPollingWindowMessage, (object) actionName);
    }

    public string GetMouseElementAssignmentPollingWindowMessage(string actionName)
    {
      return string.Format(this._mouseElementAssignmentPollingWindowMessage, (object) actionName);
    }

    public string GetMouseElementAssignmentPollingWindowMessage_FullAxisFieldOnly(string actionName)
    {
      return string.Format(this._mouseElementAssignmentPollingWindowMessage_fullAxisFieldOnly, (object) actionName);
    }

    public string GetElementAlreadyInUseBlocked(string elementName)
    {
      return string.Format(this._elementAlreadyInUseBlocked, (object) elementName);
    }

    public string GetElementAlreadyInUseCanReplace(string elementName, bool allowConflicts)
    {
      return !allowConflicts ? string.Format(this._elementAlreadyInUseCanReplace, (object) elementName) : string.Format(this._elementAlreadyInUseCanReplace_conflictAllowed, (object) elementName);
    }

    public string GetMouseAssignmentConflictWindowMessage(
      string otherPlayerName,
      string thisPlayerName)
    {
      return string.Format(this._mouseAssignmentConflictWindowMessage, (object) otherPlayerName, (object) thisPlayerName);
    }

    public string GetCalibrateAxisStep1WindowMessage(string axisName)
    {
      return string.Format(this._calibrateAxisStep1WindowMessage, (object) axisName);
    }

    public string GetCalibrateAxisStep2WindowMessage(string axisName)
    {
      return string.Format(this._calibrateAxisStep2WindowMessage, (object) axisName);
    }

    [Serializable]
    private class CustomEntry
    {
      public string key;
      public string value;

      public CustomEntry()
      {
      }

      public CustomEntry(string key, string value)
      {
        this.key = key;
        this.value = value;
      }

      public static Dictionary<string, string> ToDictionary(
        LanguageData.CustomEntry[] array)
      {
        if (array == null)
          return new Dictionary<string, string>();
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        for (int index = 0; index < array.Length; ++index)
        {
          if (array[index] != null && !string.IsNullOrEmpty(array[index].key) && !string.IsNullOrEmpty(array[index].value))
          {
            if (dictionary.ContainsKey(array[index].key))
              Debug.LogError((object) ("Key \"" + array[index].key + "\" is already in dictionary!"));
            else
              dictionary.Add(array[index].key, array[index].value);
          }
        }
        return dictionary;
      }
    }
  }
}
