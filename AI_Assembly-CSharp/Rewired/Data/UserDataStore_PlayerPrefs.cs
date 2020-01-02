// Decompiled with JetBrains decompiler
// Type: Rewired.Data.UserDataStore_PlayerPrefs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Rewired.Utils.Libraries.TinyJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Rewired.Data
{
  public class UserDataStore_PlayerPrefs : UserDataStore
  {
    private const string thisScriptName = "UserDataStore_PlayerPrefs";
    private const string editorLoadedMessage = "\nIf unexpected input issues occur, the loaded XML data may be outdated or invalid. Clear PlayerPrefs using the inspector option on the UserDataStore_PlayerPrefs component.";
    private const string playerPrefsKeySuffix_controllerAssignments = "ControllerAssignments";
    [Tooltip("Should this script be used? If disabled, nothing will be saved or loaded.")]
    [SerializeField]
    private bool isEnabled;
    [Tooltip("Should saved data be loaded on start?")]
    [SerializeField]
    private bool loadDataOnStart;
    [Tooltip("Should Player Joystick assignments be saved and loaded? This is not totally reliable for all Joysticks on all platforms. Some platforms/input sources do not provide enough information to reliably save assignments from session to session and reboot to reboot.")]
    [SerializeField]
    private bool loadJoystickAssignments;
    [Tooltip("Should Player Keyboard assignments be saved and loaded?")]
    [SerializeField]
    private bool loadKeyboardAssignments;
    [Tooltip("Should Player Mouse assignments be saved and loaded?")]
    [SerializeField]
    private bool loadMouseAssignments;
    [Tooltip("The PlayerPrefs key prefix. Change this to change how keys are stored in PlayerPrefs. Changing this will make saved data already stored with the old key no longer accessible.")]
    [SerializeField]
    private string playerPrefsKeyPrefix;
    private bool allowImpreciseJoystickAssignmentMatching;
    private bool deferredJoystickAssignmentLoadPending;
    private bool wasJoystickEverDetected;

    public UserDataStore_PlayerPrefs()
    {
      base.\u002Ector();
    }

    public bool IsEnabled
    {
      get
      {
        return this.isEnabled;
      }
      set
      {
        this.isEnabled = value;
      }
    }

    public bool LoadDataOnStart
    {
      get
      {
        return this.loadDataOnStart;
      }
      set
      {
        this.loadDataOnStart = value;
      }
    }

    public bool LoadJoystickAssignments
    {
      get
      {
        return this.loadJoystickAssignments;
      }
      set
      {
        this.loadJoystickAssignments = value;
      }
    }

    public bool LoadKeyboardAssignments
    {
      get
      {
        return this.loadKeyboardAssignments;
      }
      set
      {
        this.loadKeyboardAssignments = value;
      }
    }

    public bool LoadMouseAssignments
    {
      get
      {
        return this.loadMouseAssignments;
      }
      set
      {
        this.loadMouseAssignments = value;
      }
    }

    public string PlayerPrefsKeyPrefix
    {
      get
      {
        return this.playerPrefsKeyPrefix;
      }
      set
      {
        this.playerPrefsKeyPrefix = value;
      }
    }

    private string playerPrefsKey_controllerAssignments
    {
      get
      {
        return string.Format("{0}_{1}", (object) this.playerPrefsKeyPrefix, (object) "ControllerAssignments");
      }
    }

    private bool loadControllerAssignments
    {
      get
      {
        return this.loadKeyboardAssignments || this.loadMouseAssignments || this.loadJoystickAssignments;
      }
    }

    public virtual void Save()
    {
      if (!this.isEnabled)
        Debug.LogWarning((object) "Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", (Object) this);
      else
        this.SaveAll();
    }

    public virtual void SaveControllerData(
      int playerId,
      ControllerType controllerType,
      int controllerId)
    {
      if (!this.isEnabled)
        Debug.LogWarning((object) "Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", (Object) this);
      else
        this.SaveControllerDataNow(playerId, controllerType, controllerId);
    }

    public virtual void SaveControllerData(ControllerType controllerType, int controllerId)
    {
      if (!this.isEnabled)
        Debug.LogWarning((object) "Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", (Object) this);
      else
        this.SaveControllerDataNow(controllerType, controllerId);
    }

    public virtual void SavePlayerData(int playerId)
    {
      if (!this.isEnabled)
        Debug.LogWarning((object) "Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", (Object) this);
      else
        this.SavePlayerDataNow(playerId);
    }

    public virtual void SaveInputBehavior(int playerId, int behaviorId)
    {
      if (!this.isEnabled)
        Debug.LogWarning((object) "Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", (Object) this);
      else
        this.SaveInputBehaviorNow(playerId, behaviorId);
    }

    public virtual void Load()
    {
      if (!this.isEnabled)
        Debug.LogWarning((object) "Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", (Object) this);
      else
        this.LoadAll();
    }

    public virtual void LoadControllerData(
      int playerId,
      ControllerType controllerType,
      int controllerId)
    {
      if (!this.isEnabled)
        Debug.LogWarning((object) "Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", (Object) this);
      else
        this.LoadControllerDataNow(playerId, controllerType, controllerId);
    }

    public virtual void LoadControllerData(ControllerType controllerType, int controllerId)
    {
      if (!this.isEnabled)
        Debug.LogWarning((object) "Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", (Object) this);
      else
        this.LoadControllerDataNow(controllerType, controllerId);
    }

    public virtual void LoadPlayerData(int playerId)
    {
      if (!this.isEnabled)
        Debug.LogWarning((object) "Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", (Object) this);
      else
        this.LoadPlayerDataNow(playerId);
    }

    public virtual void LoadInputBehavior(int playerId, int behaviorId)
    {
      if (!this.isEnabled)
        Debug.LogWarning((object) "Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", (Object) this);
      else
        this.LoadInputBehaviorNow(playerId, behaviorId);
    }

    protected virtual void OnInitialize()
    {
      if (!this.loadDataOnStart)
        return;
      base.Load();
      if (!this.loadControllerAssignments || ReInput.get_controllers().get_joystickCount() <= 0)
        return;
      this.SaveControllerAssignments();
    }

    protected virtual void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
      if (!this.isEnabled || args.get_controllerType() != 2)
        return;
      this.LoadJoystickData(args.get_controllerId());
      if (this.loadDataOnStart && this.loadJoystickAssignments && !this.wasJoystickEverDetected)
        ((MonoBehaviour) this).StartCoroutine(this.LoadJoystickAssignmentsDeferred());
      if (this.loadJoystickAssignments && !this.deferredJoystickAssignmentLoadPending)
        this.SaveControllerAssignments();
      this.wasJoystickEverDetected = true;
    }

    protected virtual void OnControllerPreDiscconnect(ControllerStatusChangedEventArgs args)
    {
      if (!this.isEnabled || args.get_controllerType() != 2)
        return;
      this.SaveJoystickData(args.get_controllerId());
    }

    protected virtual void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
      if (!this.isEnabled || !this.loadControllerAssignments)
        return;
      this.SaveControllerAssignments();
    }

    private int LoadAll()
    {
      int num = 0;
      if (this.loadControllerAssignments && this.LoadControllerAssignmentsNow())
        ++num;
      IList<Player> allPlayers = ReInput.get_players().get_AllPlayers();
      for (int index = 0; index < ((ICollection<Player>) allPlayers).Count; ++index)
        num += this.LoadPlayerDataNow(allPlayers[index]);
      return num + this.LoadAllJoystickCalibrationData();
    }

    private int LoadPlayerDataNow(int playerId)
    {
      return this.LoadPlayerDataNow(ReInput.get_players().GetPlayer(playerId));
    }

    private int LoadPlayerDataNow(Player player)
    {
      if (player == null)
        return 0;
      int num = 0 + this.LoadInputBehaviors(player.get_id()) + this.LoadControllerMaps(player.get_id(), (ControllerType) 0, 0) + this.LoadControllerMaps(player.get_id(), (ControllerType) 1, 0);
      using (IEnumerator<Joystick> enumerator = ((IEnumerable<Joystick>) ((Player.ControllerHelper) player.controllers).get_Joysticks()).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          Joystick current = enumerator.Current;
          num += this.LoadControllerMaps(player.get_id(), (ControllerType) 2, (int) ((Controller) current).id);
        }
      }
      return num;
    }

    private int LoadAllJoystickCalibrationData()
    {
      int num = 0;
      IList<Joystick> joysticks = ReInput.get_controllers().get_Joysticks();
      for (int index = 0; index < ((ICollection<Joystick>) joysticks).Count; ++index)
        num += this.LoadJoystickCalibrationData(joysticks[index]);
      return num;
    }

    private int LoadJoystickCalibrationData(Joystick joystick)
    {
      return joystick == null || !((ControllerWithAxes) joystick).ImportCalibrationMapFromXmlString(this.GetJoystickCalibrationMapXml(joystick)) ? 0 : 1;
    }

    private int LoadJoystickCalibrationData(int joystickId)
    {
      return this.LoadJoystickCalibrationData(ReInput.get_controllers().GetJoystick(joystickId));
    }

    private int LoadJoystickData(int joystickId)
    {
      int num = 0;
      IList<Player> allPlayers = ReInput.get_players().get_AllPlayers();
      for (int index = 0; index < ((ICollection<Player>) allPlayers).Count; ++index)
      {
        Player player = allPlayers[index];
        if (((Player.ControllerHelper) player.controllers).ContainsController((ControllerType) 2, joystickId))
          num += this.LoadControllerMaps(player.get_id(), (ControllerType) 2, joystickId);
      }
      return num + this.LoadJoystickCalibrationData(joystickId);
    }

    private int LoadControllerDataNow(
      int playerId,
      ControllerType controllerType,
      int controllerId)
    {
      return 0 + this.LoadControllerMaps(playerId, controllerType, controllerId) + this.LoadControllerDataNow(controllerType, controllerId);
    }

    private int LoadControllerDataNow(ControllerType controllerType, int controllerId)
    {
      int num = 0;
      if (controllerType == 2)
        num += this.LoadJoystickCalibrationData(controllerId);
      return num;
    }

    private int LoadControllerMaps(int playerId, ControllerType controllerType, int controllerId)
    {
      int num1 = 0;
      Player player = ReInput.get_players().GetPlayer(playerId);
      if (player == null)
        return num1;
      Controller controller = ReInput.get_controllers().GetController(controllerType, controllerId);
      if (controller == null)
        return num1;
      List<UserDataStore_PlayerPrefs.SavedControllerMapData> controllerMapsXml = this.GetAllControllerMapsXml(player, true, controller);
      if (controllerMapsXml.Count == 0)
        return num1;
      int num2 = num1 + ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) player.controllers).maps).AddMapsFromXml(controllerType, controllerId, UserDataStore_PlayerPrefs.SavedControllerMapData.GetXmlStringList(controllerMapsXml));
      this.AddDefaultMappingsForNewActions(player, controllerMapsXml, controllerType, controllerId);
      return num2;
    }

    private int LoadInputBehaviors(int playerId)
    {
      Player player = ReInput.get_players().GetPlayer(playerId);
      if (player == null)
        return 0;
      int num = 0;
      IList<InputBehavior> inputBehaviors = ReInput.get_mapping().GetInputBehaviors(player.get_id());
      for (int index = 0; index < ((ICollection<InputBehavior>) inputBehaviors).Count; ++index)
        num += this.LoadInputBehaviorNow(player, inputBehaviors[index]);
      return num;
    }

    private int LoadInputBehaviorNow(int playerId, int behaviorId)
    {
      Player player = ReInput.get_players().GetPlayer(playerId);
      if (player == null)
        return 0;
      InputBehavior inputBehavior = ReInput.get_mapping().GetInputBehavior(playerId, behaviorId);
      return inputBehavior == null ? 0 : this.LoadInputBehaviorNow(player, inputBehavior);
    }

    private int LoadInputBehaviorNow(Player player, InputBehavior inputBehavior)
    {
      if (player == null || inputBehavior == null)
        return 0;
      string inputBehaviorXml = this.GetInputBehaviorXml(player, inputBehavior.get_id());
      return inputBehaviorXml == null || inputBehaviorXml == string.Empty || !inputBehavior.ImportXmlString(inputBehaviorXml) ? 0 : 1;
    }

    private bool LoadControllerAssignmentsNow()
    {
      try
      {
        UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo data = this.LoadControllerAssignmentData();
        if (data == null)
          return false;
        if (this.loadKeyboardAssignments || this.loadMouseAssignments)
          this.LoadKeyboardAndMouseAssignmentsNow(data);
        if (this.loadJoystickAssignments)
          this.LoadJoystickAssignmentsNow(data);
      }
      catch
      {
      }
      return true;
    }

    private bool LoadKeyboardAndMouseAssignmentsNow(
      UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo data)
    {
      try
      {
        if (data == null && (data = this.LoadControllerAssignmentData()) == null)
          return false;
        using (IEnumerator<Player> enumerator = ((IEnumerable<Player>) ReInput.get_players().get_AllPlayers()).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
          {
            Player current = enumerator.Current;
            if (data.ContainsPlayer(current.get_id()))
            {
              UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo player = data.players[data.IndexOfPlayer(current.get_id())];
              if (this.loadKeyboardAssignments)
                ((Player.ControllerHelper) current.controllers).set_hasKeyboard(player.hasKeyboard);
              if (this.loadMouseAssignments)
                ((Player.ControllerHelper) current.controllers).set_hasMouse(player.hasMouse);
            }
          }
        }
      }
      catch
      {
      }
      return true;
    }

    private bool LoadJoystickAssignmentsNow(
      UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo data)
    {
      try
      {
        if (ReInput.get_controllers().get_joystickCount() == 0 || data == null && (data = this.LoadControllerAssignmentData()) == null)
          return false;
        using (IEnumerator<Player> enumerator = ((IEnumerable<Player>) ReInput.get_players().get_AllPlayers()).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
            ((Player.ControllerHelper) enumerator.Current.controllers).ClearControllersOfType((ControllerType) 2);
        }
        List<UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo> assignmentHistoryInfoList = !this.loadJoystickAssignments ? (List<UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo>) null : new List<UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo>();
        using (IEnumerator<Player> enumerator = ((IEnumerable<Player>) ReInput.get_players().get_AllPlayers()).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
          {
            Player current = enumerator.Current;
            if (data.ContainsPlayer(current.get_id()))
            {
              UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo player = data.players[data.IndexOfPlayer(current.get_id())];
              for (int index = 0; index < player.joystickCount; ++index)
              {
                UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystick1 = player.joysticks[index];
                if (joystick1 != null)
                {
                  Joystick joystick = this.FindJoystickPrecise(joystick1);
                  if (joystick != null)
                  {
                    if (assignmentHistoryInfoList.Find((Predicate<UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo>) (x => x.joystick == joystick)) == null)
                      assignmentHistoryInfoList.Add(new UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo(joystick, joystick1.id));
                    ((Player.ControllerHelper) current.controllers).AddController((Controller) joystick, false);
                  }
                }
              }
            }
          }
        }
        if (this.allowImpreciseJoystickAssignmentMatching)
        {
          using (IEnumerator<Player> enumerator1 = ((IEnumerable<Player>) ReInput.get_players().get_AllPlayers()).GetEnumerator())
          {
            while (((IEnumerator) enumerator1).MoveNext())
            {
              Player current = enumerator1.Current;
              if (data.ContainsPlayer(current.get_id()))
              {
                UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo player = data.players[data.IndexOfPlayer(current.get_id())];
                for (int index1 = 0; index1 < player.joystickCount; ++index1)
                {
                  UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo = player.joysticks[index1];
                  if (joystickInfo != null)
                  {
                    Joystick joystick = (Joystick) null;
                    int index2 = assignmentHistoryInfoList.FindIndex((Predicate<UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo>) (x => x.oldJoystickId == joystickInfo.id));
                    if (index2 >= 0)
                    {
                      joystick = assignmentHistoryInfoList[index2].joystick;
                    }
                    else
                    {
                      List<Joystick> matches;
                      if (this.TryFindJoysticksImprecise(joystickInfo, out matches))
                      {
                        using (List<Joystick>.Enumerator enumerator2 = matches.GetEnumerator())
                        {
                          while (enumerator2.MoveNext())
                          {
                            Joystick match = enumerator2.Current;
                            if (assignmentHistoryInfoList.Find((Predicate<UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo>) (x => x.joystick == match)) == null)
                            {
                              joystick = match;
                              break;
                            }
                          }
                        }
                        if (joystick != null)
                          assignmentHistoryInfoList.Add(new UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo(joystick, joystickInfo.id));
                        else
                          continue;
                      }
                      else
                        continue;
                    }
                    ((Player.ControllerHelper) current.controllers).AddController((Controller) joystick, false);
                  }
                }
              }
            }
          }
        }
      }
      catch
      {
      }
      if (ReInput.get_configuration().get_autoAssignJoysticks())
        ReInput.get_controllers().AutoAssignJoysticks();
      return true;
    }

    private UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo LoadControllerAssignmentData()
    {
      try
      {
        if (!PlayerPrefs.HasKey(this.playerPrefsKey_controllerAssignments))
          return (UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo) null;
        string str = PlayerPrefs.GetString(this.playerPrefsKey_controllerAssignments);
        if (string.IsNullOrEmpty(str))
          return (UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo) null;
        UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo assignmentSaveInfo = (UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo) JsonParser.FromJson<UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo>(str);
        return assignmentSaveInfo == null || assignmentSaveInfo.playerCount == 0 ? (UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo) null : assignmentSaveInfo;
      }
      catch
      {
        return (UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo) null;
      }
    }

    [DebuggerHidden]
    private IEnumerator LoadJoystickAssignmentsDeferred()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new UserDataStore_PlayerPrefs.\u003CLoadJoystickAssignmentsDeferred\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void SaveAll()
    {
      IList<Player> allPlayers = ReInput.get_players().get_AllPlayers();
      for (int index = 0; index < ((ICollection<Player>) allPlayers).Count; ++index)
        this.SavePlayerDataNow(allPlayers[index]);
      this.SaveAllJoystickCalibrationData();
      if (this.loadControllerAssignments)
        this.SaveControllerAssignments();
      PlayerPrefs.Save();
    }

    private void SavePlayerDataNow(int playerId)
    {
      this.SavePlayerDataNow(ReInput.get_players().GetPlayer(playerId));
      PlayerPrefs.Save();
    }

    private void SavePlayerDataNow(Player player)
    {
      if (player == null)
        return;
      PlayerSaveData saveData = player.GetSaveData(true);
      this.SaveInputBehaviors(player, saveData);
      this.SaveControllerMaps(player, saveData);
    }

    private void SaveAllJoystickCalibrationData()
    {
      IList<Joystick> joysticks = ReInput.get_controllers().get_Joysticks();
      for (int index = 0; index < ((ICollection<Joystick>) joysticks).Count; ++index)
        this.SaveJoystickCalibrationData(joysticks[index]);
    }

    private void SaveJoystickCalibrationData(int joystickId)
    {
      this.SaveJoystickCalibrationData(ReInput.get_controllers().GetJoystick(joystickId));
    }

    private void SaveJoystickCalibrationData(Joystick joystick)
    {
      if (joystick == null)
        return;
      JoystickCalibrationMapSaveData calibrationMapSaveData = joystick.GetCalibrationMapSaveData();
      PlayerPrefs.SetString(this.GetJoystickCalibrationMapPlayerPrefsKey(joystick), ((CalibrationMapSaveData) calibrationMapSaveData).get_map().ToXmlString());
    }

    private void SaveJoystickData(int joystickId)
    {
      IList<Player> allPlayers = ReInput.get_players().get_AllPlayers();
      for (int index = 0; index < ((ICollection<Player>) allPlayers).Count; ++index)
      {
        Player player = allPlayers[index];
        if (((Player.ControllerHelper) player.controllers).ContainsController((ControllerType) 2, joystickId))
          this.SaveControllerMaps(player.get_id(), (ControllerType) 2, joystickId);
      }
      this.SaveJoystickCalibrationData(joystickId);
    }

    private void SaveControllerDataNow(
      int playerId,
      ControllerType controllerType,
      int controllerId)
    {
      this.SaveControllerMaps(playerId, controllerType, controllerId);
      this.SaveControllerDataNow(controllerType, controllerId);
      PlayerPrefs.Save();
    }

    private void SaveControllerDataNow(ControllerType controllerType, int controllerId)
    {
      if (controllerType == 2)
        this.SaveJoystickCalibrationData(controllerId);
      PlayerPrefs.Save();
    }

    private void SaveControllerMaps(Player player, PlayerSaveData playerSaveData)
    {
      using (IEnumerator<ControllerMapSaveData> enumerator = ((PlayerSaveData) ref playerSaveData).get_AllControllerMapSaveData().GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          ControllerMapSaveData current = enumerator.Current;
          this.SaveControllerMap(player, current);
        }
      }
    }

    private void SaveControllerMaps(int playerId, ControllerType controllerType, int controllerId)
    {
      Player player = ReInput.get_players().GetPlayer(playerId);
      if (player == null || !((Player.ControllerHelper) player.controllers).ContainsController(controllerType, controllerId))
        return;
      ControllerMapSaveData[] mapSaveData = ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) player.controllers).maps).GetMapSaveData(controllerType, controllerId, true);
      if (mapSaveData == null)
        return;
      for (int index = 0; index < mapSaveData.Length; ++index)
        this.SaveControllerMap(player, mapSaveData[index]);
    }

    private void SaveControllerMap(Player player, ControllerMapSaveData saveData)
    {
      PlayerPrefs.SetString(this.GetControllerMapPlayerPrefsKey(player, saveData.get_controller(), saveData.get_categoryId(), saveData.get_layoutId(), true), saveData.get_map().ToXmlString());
      PlayerPrefs.SetString(this.GetControllerMapKnownActionIdsPlayerPrefsKey(player, saveData.get_controller(), saveData.get_categoryId(), saveData.get_layoutId(), true), this.GetAllActionIdsString());
    }

    private void SaveInputBehaviors(Player player, PlayerSaveData playerSaveData)
    {
      if (player == null)
        return;
      foreach (InputBehavior inputBehavior in ((PlayerSaveData) ref playerSaveData).get_inputBehaviors())
        this.SaveInputBehaviorNow(player, inputBehavior);
    }

    private void SaveInputBehaviorNow(int playerId, int behaviorId)
    {
      Player player = ReInput.get_players().GetPlayer(playerId);
      if (player == null)
        return;
      InputBehavior inputBehavior = ReInput.get_mapping().GetInputBehavior(playerId, behaviorId);
      if (inputBehavior == null)
        return;
      this.SaveInputBehaviorNow(player, inputBehavior);
      PlayerPrefs.Save();
    }

    private void SaveInputBehaviorNow(Player player, InputBehavior inputBehavior)
    {
      if (player == null || inputBehavior == null)
        return;
      PlayerPrefs.SetString(this.GetInputBehaviorPlayerPrefsKey(player, inputBehavior.get_id()), inputBehavior.ToXmlString());
    }

    private bool SaveControllerAssignments()
    {
      try
      {
        UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo assignmentSaveInfo = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo(ReInput.get_players().get_allPlayerCount());
        for (int index1 = 0; index1 < ReInput.get_players().get_allPlayerCount(); ++index1)
        {
          Player allPlayer = ReInput.get_players().get_AllPlayers()[index1];
          UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo playerInfo = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo();
          assignmentSaveInfo.players[index1] = playerInfo;
          playerInfo.id = allPlayer.get_id();
          playerInfo.hasKeyboard = ((Player.ControllerHelper) allPlayer.controllers).get_hasKeyboard();
          playerInfo.hasMouse = ((Player.ControllerHelper) allPlayer.controllers).get_hasMouse();
          UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo[] joystickInfoArray = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo[((Player.ControllerHelper) allPlayer.controllers).get_joystickCount()];
          playerInfo.joysticks = joystickInfoArray;
          for (int index2 = 0; index2 < ((Player.ControllerHelper) allPlayer.controllers).get_joystickCount(); ++index2)
          {
            Joystick joystick = ((Player.ControllerHelper) allPlayer.controllers).get_Joysticks()[index2];
            joystickInfoArray[index2] = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo()
            {
              instanceGuid = joystick.get_deviceInstanceGuid(),
              id = (int) ((Controller) joystick).id,
              hardwareIdentifier = ((Controller) joystick).get_hardwareIdentifier()
            };
          }
        }
        PlayerPrefs.SetString(this.playerPrefsKey_controllerAssignments, JsonWriter.ToJson((object) assignmentSaveInfo));
        PlayerPrefs.Save();
      }
      catch
      {
      }
      return true;
    }

    private bool ControllerAssignmentSaveDataExists()
    {
      return PlayerPrefs.HasKey(this.playerPrefsKey_controllerAssignments) && !string.IsNullOrEmpty(PlayerPrefs.GetString(this.playerPrefsKey_controllerAssignments));
    }

    private string GetBasePlayerPrefsKey(Player player)
    {
      return this.playerPrefsKeyPrefix + "|playerName=" + player.get_name();
    }

    private string GetControllerMapPlayerPrefsKey(
      Player player,
      Controller controller,
      int categoryId,
      int layoutId,
      bool includeDuplicateIndex)
    {
      string str = this.GetBasePlayerPrefsKey(player) + "|dataType=ControllerMap" + "|controllerMapType=" + controller.get_mapTypeString() + "|categoryId=" + (object) categoryId + "|layoutId=" + (object) layoutId + "|hardwareIdentifier=" + controller.get_hardwareIdentifier();
      if (controller.get_type() == 2)
      {
        str = str + "|hardwareGuid=" + ((Joystick) controller).get_hardwareTypeGuid().ToString();
        if (includeDuplicateIndex)
          str = str + "|duplicate=" + UserDataStore_PlayerPrefs.GetDuplicateIndex(player, controller).ToString();
      }
      return str;
    }

    private string GetControllerMapKnownActionIdsPlayerPrefsKey(
      Player player,
      Controller controller,
      int categoryId,
      int layoutId,
      bool includeDuplicateIndex)
    {
      string str = this.GetBasePlayerPrefsKey(player) + "|dataType=ControllerMap_KnownActionIds" + "|controllerMapType=" + controller.get_mapTypeString() + "|categoryId=" + (object) categoryId + "|layoutId=" + (object) layoutId + "|hardwareIdentifier=" + controller.get_hardwareIdentifier();
      if (controller.get_type() == 2)
      {
        str = str + "|hardwareGuid=" + ((Joystick) controller).get_hardwareTypeGuid().ToString();
        if (includeDuplicateIndex)
          str = str + "|duplicate=" + UserDataStore_PlayerPrefs.GetDuplicateIndex(player, controller).ToString();
      }
      return str;
    }

    private string GetJoystickCalibrationMapPlayerPrefsKey(Joystick joystick)
    {
      return this.playerPrefsKeyPrefix + "|dataType=CalibrationMap" + "|controllerType=" + ((Controller) joystick).get_type().ToString() + "|hardwareIdentifier=" + ((Controller) joystick).get_hardwareIdentifier() + "|hardwareGuid=" + joystick.get_hardwareTypeGuid().ToString();
    }

    private string GetInputBehaviorPlayerPrefsKey(Player player, int inputBehaviorId)
    {
      return this.GetBasePlayerPrefsKey(player) + "|dataType=InputBehavior" + "|id=" + (object) inputBehaviorId;
    }

    private string GetControllerMapXml(
      Player player,
      Controller controller,
      int categoryId,
      int layoutId)
    {
      string mapPlayerPrefsKey = this.GetControllerMapPlayerPrefsKey(player, controller, categoryId, layoutId, true);
      if (!PlayerPrefs.HasKey(mapPlayerPrefsKey))
      {
        if (controller.get_type() != 2)
          return string.Empty;
        mapPlayerPrefsKey = this.GetControllerMapPlayerPrefsKey(player, controller, categoryId, layoutId, false);
        if (!PlayerPrefs.HasKey(mapPlayerPrefsKey))
          return string.Empty;
      }
      return PlayerPrefs.GetString(mapPlayerPrefsKey);
    }

    private List<int> GetControllerMapKnownActionIds(
      Player player,
      Controller controller,
      int categoryId,
      int layoutId)
    {
      List<int> intList = new List<int>();
      string idsPlayerPrefsKey = this.GetControllerMapKnownActionIdsPlayerPrefsKey(player, controller, categoryId, layoutId, true);
      if (!PlayerPrefs.HasKey(idsPlayerPrefsKey))
      {
        if (controller.get_type() != 2)
          return intList;
        idsPlayerPrefsKey = this.GetControllerMapKnownActionIdsPlayerPrefsKey(player, controller, categoryId, layoutId, false);
        if (!PlayerPrefs.HasKey(idsPlayerPrefsKey))
          return intList;
      }
      string str = PlayerPrefs.GetString(idsPlayerPrefsKey);
      if (string.IsNullOrEmpty(str))
        return intList;
      string[] strArray = str.Split(',');
      for (int index = 0; index < strArray.Length; ++index)
      {
        int result;
        if (!string.IsNullOrEmpty(strArray[index]) && int.TryParse(strArray[index], out result))
          intList.Add(result);
      }
      return intList;
    }

    private List<UserDataStore_PlayerPrefs.SavedControllerMapData> GetAllControllerMapsXml(
      Player player,
      bool userAssignableMapsOnly,
      Controller controller)
    {
      List<UserDataStore_PlayerPrefs.SavedControllerMapData> controllerMapDataList = new List<UserDataStore_PlayerPrefs.SavedControllerMapData>();
      IList<InputMapCategory> mapCategories = ReInput.get_mapping().get_MapCategories();
      for (int index1 = 0; index1 < ((ICollection<InputMapCategory>) mapCategories).Count; ++index1)
      {
        InputMapCategory inputMapCategory = mapCategories[index1];
        if (!userAssignableMapsOnly || ((InputCategory) inputMapCategory).get_userAssignable())
        {
          IList<InputLayout> inputLayoutList = ReInput.get_mapping().MapLayouts(controller.get_type());
          for (int index2 = 0; index2 < ((ICollection<InputLayout>) inputLayoutList).Count; ++index2)
          {
            InputLayout inputLayout = inputLayoutList[index2];
            string controllerMapXml = this.GetControllerMapXml(player, controller, ((InputCategory) inputMapCategory).get_id(), inputLayout.get_id());
            if (!(controllerMapXml == string.Empty))
            {
              List<int> mapKnownActionIds = this.GetControllerMapKnownActionIds(player, controller, ((InputCategory) inputMapCategory).get_id(), inputLayout.get_id());
              controllerMapDataList.Add(new UserDataStore_PlayerPrefs.SavedControllerMapData(controllerMapXml, mapKnownActionIds));
            }
          }
        }
      }
      return controllerMapDataList;
    }

    private string GetJoystickCalibrationMapXml(Joystick joystick)
    {
      string mapPlayerPrefsKey = this.GetJoystickCalibrationMapPlayerPrefsKey(joystick);
      return !PlayerPrefs.HasKey(mapPlayerPrefsKey) ? string.Empty : PlayerPrefs.GetString(mapPlayerPrefsKey);
    }

    private string GetInputBehaviorXml(Player player, int id)
    {
      string behaviorPlayerPrefsKey = this.GetInputBehaviorPlayerPrefsKey(player, id);
      return !PlayerPrefs.HasKey(behaviorPlayerPrefsKey) ? string.Empty : PlayerPrefs.GetString(behaviorPlayerPrefsKey);
    }

    private void AddDefaultMappingsForNewActions(
      Player player,
      List<UserDataStore_PlayerPrefs.SavedControllerMapData> savedData,
      ControllerType controllerType,
      int controllerId)
    {
      if (player == null || savedData == null)
        return;
      List<int> allActionIds = this.GetAllActionIds();
      for (int index = 0; index < savedData.Count; ++index)
      {
        UserDataStore_PlayerPrefs.SavedControllerMapData controllerMapData = savedData[index];
        if (controllerMapData != null && controllerMapData.knownActionIds != null && controllerMapData.knownActionIds.Count != 0)
        {
          ControllerMap fromXml = ControllerMap.CreateFromXml(controllerType, savedData[index].xml);
          if (fromXml != null)
          {
            ControllerMap map = ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) player.controllers).maps).GetMap(controllerType, controllerId, fromXml.get_categoryId(), fromXml.get_layoutId());
            if (map != null)
            {
              ControllerMap controllerMapInstance = ReInput.get_mapping().GetControllerMapInstance(ReInput.get_controllers().GetController(controllerType, controllerId), fromXml.get_categoryId(), fromXml.get_layoutId());
              if (controllerMapInstance != null)
              {
                List<int> intList = new List<int>();
                foreach (int num in allActionIds)
                {
                  if (!controllerMapData.knownActionIds.Contains(num))
                    intList.Add(num);
                }
                if (intList.Count != 0)
                {
                  using (IEnumerator<ActionElementMap> enumerator = ((IEnumerable<ActionElementMap>) controllerMapInstance.get_AllMaps()).GetEnumerator())
                  {
                    while (((IEnumerator) enumerator).MoveNext())
                    {
                      ActionElementMap current = enumerator.Current;
                      if (intList.Contains(current.get_actionId()) && !map.DoesElementAssignmentConflict(current))
                      {
                        ElementAssignment elementAssignment;
                        ((ElementAssignment) ref elementAssignment).\u002Ector(controllerType, current.get_elementType(), current.get_elementIdentifierId(), current.get_axisRange(), current.get_keyCode(), current.get_modifierKeyFlags(), current.get_actionId(), current.get_axisContribution(), current.get_invert());
                        map.CreateElementMap(elementAssignment);
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
    }

    private List<int> GetAllActionIds()
    {
      List<int> intList = new List<int>();
      IList<InputAction> actions = ReInput.get_mapping().get_Actions();
      for (int index = 0; index < ((ICollection<InputAction>) actions).Count; ++index)
        intList.Add(actions[index].get_id());
      return intList;
    }

    private string GetAllActionIdsString()
    {
      string empty = string.Empty;
      List<int> allActionIds = this.GetAllActionIds();
      for (int index = 0; index < allActionIds.Count; ++index)
      {
        if (index > 0)
          empty += ",";
        empty += (string) (object) allActionIds[index];
      }
      return empty;
    }

    private Joystick FindJoystickPrecise(
      UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo)
    {
      if (joystickInfo == null)
        return (Joystick) null;
      if (joystickInfo.instanceGuid == Guid.Empty)
        return (Joystick) null;
      IList<Joystick> joysticks = ReInput.get_controllers().get_Joysticks();
      for (int index = 0; index < ((ICollection<Joystick>) joysticks).Count; ++index)
      {
        if (joysticks[index].get_deviceInstanceGuid() == joystickInfo.instanceGuid)
          return joysticks[index];
      }
      return (Joystick) null;
    }

    private bool TryFindJoysticksImprecise(
      UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo,
      out List<Joystick> matches)
    {
      matches = (List<Joystick>) null;
      if (joystickInfo == null || string.IsNullOrEmpty(joystickInfo.hardwareIdentifier))
        return false;
      IList<Joystick> joysticks = ReInput.get_controllers().get_Joysticks();
      for (int index = 0; index < ((ICollection<Joystick>) joysticks).Count; ++index)
      {
        if (string.Equals(((Controller) joysticks[index]).get_hardwareIdentifier(), joystickInfo.hardwareIdentifier, StringComparison.OrdinalIgnoreCase))
        {
          if (matches == null)
            matches = new List<Joystick>();
          matches.Add(joysticks[index]);
        }
      }
      return matches != null;
    }

    private static int GetDuplicateIndex(Player player, Controller controller)
    {
      int num = 0;
      using (IEnumerator<Controller> enumerator = ((Player.ControllerHelper) player.controllers).get_Controllers().GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          Controller current = enumerator.Current;
          if (current.get_type() == controller.get_type())
          {
            bool flag = false;
            if (controller.get_type() == 2)
            {
              if (!((current as Joystick).get_hardwareTypeGuid() != (controller as Joystick).get_hardwareTypeGuid()))
              {
                if ((controller as Joystick).get_hardwareTypeGuid() != Guid.Empty)
                  flag = true;
              }
              else
                continue;
            }
            if (flag || !(current.get_hardwareIdentifier() != controller.get_hardwareIdentifier()))
            {
              if (current == controller)
                return num;
              ++num;
            }
          }
        }
      }
      return num;
    }

    private class SavedControllerMapData
    {
      public string xml;
      public List<int> knownActionIds;

      public SavedControllerMapData(string xml, List<int> knownActionIds)
      {
        this.xml = xml;
        this.knownActionIds = knownActionIds;
      }

      public static List<string> GetXmlStringList(
        List<UserDataStore_PlayerPrefs.SavedControllerMapData> data)
      {
        List<string> stringList = new List<string>();
        if (data == null)
          return stringList;
        for (int index = 0; index < data.Count; ++index)
        {
          if (data[index] != null && !string.IsNullOrEmpty(data[index].xml))
            stringList.Add(data[index].xml);
        }
        return stringList;
      }
    }

    private class ControllerAssignmentSaveInfo
    {
      public UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo[] players;

      public ControllerAssignmentSaveInfo()
      {
      }

      public ControllerAssignmentSaveInfo(int playerCount)
      {
        this.players = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo[playerCount];
        for (int index = 0; index < playerCount; ++index)
          this.players[index] = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo();
      }

      public int playerCount
      {
        get
        {
          return this.players != null ? this.players.Length : 0;
        }
      }

      public int IndexOfPlayer(int playerId)
      {
        for (int index = 0; index < this.playerCount; ++index)
        {
          if (this.players[index] != null && this.players[index].id == playerId)
            return index;
        }
        return -1;
      }

      public bool ContainsPlayer(int playerId)
      {
        return this.IndexOfPlayer(playerId) >= 0;
      }

      public class PlayerInfo
      {
        public int id;
        public bool hasKeyboard;
        public bool hasMouse;
        public UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo[] joysticks;

        public int joystickCount
        {
          get
          {
            return this.joysticks != null ? this.joysticks.Length : 0;
          }
        }

        public int IndexOfJoystick(int joystickId)
        {
          for (int index = 0; index < this.joystickCount; ++index)
          {
            if (this.joysticks[index] != null && this.joysticks[index].id == joystickId)
              return index;
          }
          return -1;
        }

        public bool ContainsJoystick(int joystickId)
        {
          return this.IndexOfJoystick(joystickId) >= 0;
        }
      }

      public class JoystickInfo
      {
        public Guid instanceGuid;
        public string hardwareIdentifier;
        public int id;
      }
    }

    private class JoystickAssignmentHistoryInfo
    {
      public readonly Joystick joystick;
      public readonly int oldJoystickId;

      public JoystickAssignmentHistoryInfo(Joystick joystick, int oldJoystickId)
      {
        if (joystick == null)
          throw new ArgumentNullException(nameof (joystick));
        this.joystick = joystick;
        this.oldJoystickId = oldJoystickId;
      }
    }
  }
}
