// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.ControlRemappingDemo1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
  [AddComponentMenu("")]
  public class ControlRemappingDemo1 : MonoBehaviour
  {
    private const float defaultModalWidth = 250f;
    private const float defaultModalHeight = 200f;
    private const float assignmentTimeout = 5f;
    private ControlRemappingDemo1.DialogHelper dialog;
    private InputMapper inputMapper;
    private InputMapper.ConflictFoundEventData conflictFoundEventData;
    private bool guiState;
    private bool busy;
    private bool pageGUIState;
    private Player selectedPlayer;
    private int selectedMapCategoryId;
    private ControlRemappingDemo1.ControllerSelection selectedController;
    private ControllerMap selectedMap;
    private bool showMenu;
    private bool startListening;
    private Vector2 actionScrollPos;
    private Vector2 calibrateScrollPos;
    private Queue<ControlRemappingDemo1.QueueEntry> actionQueue;
    private bool setupFinished;
    [NonSerialized]
    private bool initialized;
    private bool isCompiling;
    private GUIStyle style_wordWrap;
    private GUIStyle style_centeredBox;

    public ControlRemappingDemo1()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      this.inputMapper.get_options().set_timeout(5f);
      this.inputMapper.get_options().set_ignoreMouseXAxis(true);
      this.inputMapper.get_options().set_ignoreMouseYAxis(true);
      this.Initialize();
    }

    private void OnEnable()
    {
      this.Subscribe();
    }

    private void OnDisable()
    {
      this.Unsubscribe();
    }

    private void Initialize()
    {
      this.dialog = new ControlRemappingDemo1.DialogHelper();
      this.actionQueue = new Queue<ControlRemappingDemo1.QueueEntry>();
      this.selectedController = new ControlRemappingDemo1.ControllerSelection();
      ReInput.add_ControllerConnectedEvent(new Action<ControllerStatusChangedEventArgs>(this.JoystickConnected));
      ReInput.add_ControllerPreDisconnectEvent(new Action<ControllerStatusChangedEventArgs>(this.JoystickPreDisconnect));
      ReInput.add_ControllerDisconnectedEvent(new Action<ControllerStatusChangedEventArgs>(this.JoystickDisconnected));
      this.ResetAll();
      this.initialized = true;
      ReInput.get_userDataStore().Load();
      if (!ReInput.get_unityJoystickIdentificationRequired())
        return;
      this.IdentifyAllJoysticks();
    }

    private void Setup()
    {
      if (this.setupFinished)
        return;
      this.style_wordWrap = new GUIStyle(GUI.get_skin().get_label());
      this.style_wordWrap.set_wordWrap(true);
      this.style_centeredBox = new GUIStyle(GUI.get_skin().get_box());
      this.style_centeredBox.set_alignment((TextAnchor) 4);
      this.setupFinished = true;
    }

    private void Subscribe()
    {
      this.Unsubscribe();
      this.inputMapper.add_ConflictFoundEvent(new Action<InputMapper.ConflictFoundEventData>(this.OnConflictFound));
      this.inputMapper.add_StoppedEvent(new Action<InputMapper.StoppedEventData>(this.OnStopped));
    }

    private void Unsubscribe()
    {
      this.inputMapper.RemoveAllEventListeners();
    }

    public void OnGUI()
    {
      if (!this.initialized)
        return;
      this.Setup();
      this.HandleMenuControl();
      if (!this.showMenu)
      {
        this.DrawInitialScreen();
      }
      else
      {
        this.SetGUIStateStart();
        this.ProcessQueue();
        this.DrawPage();
        this.ShowDialog();
        this.SetGUIStateEnd();
        this.busy = false;
      }
    }

    private void HandleMenuControl()
    {
      if (this.dialog.enabled || Event.get_current().get_type() != 8 || !ReInput.get_players().GetSystemPlayer().GetButtonDown("Menu"))
        return;
      if (this.showMenu)
      {
        ReInput.get_userDataStore().Save();
        this.Close();
      }
      else
        this.Open();
    }

    private void Close()
    {
      this.ClearWorkingVars();
      this.showMenu = false;
    }

    private void Open()
    {
      this.showMenu = true;
    }

    private void DrawInitialScreen()
    {
      ActionElementMap elementMapWithAction = ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) ReInput.get_players().GetSystemPlayer().controllers).maps).GetFirstElementMapWithAction("Menu", true);
      GUIContent guiContent = elementMapWithAction == null ? new GUIContent("There is no element assigned to open the menu!") : new GUIContent("Press " + elementMapWithAction.get_elementIdentifierName() + " to open the menu.");
      GUILayout.BeginArea(this.GetScreenCenteredRect(300f, 50f));
      GUILayout.Box(guiContent, this.style_centeredBox, new GUILayoutOption[2]
      {
        GUILayout.ExpandHeight(true),
        GUILayout.ExpandWidth(true)
      });
      GUILayout.EndArea();
    }

    private void DrawPage()
    {
      if (GUI.get_enabled() != this.pageGUIState)
        GUI.set_enabled(this.pageGUIState);
      Rect rect;
      ((Rect) ref rect).\u002Ector((float) (((double) Screen.get_width() - (double) Screen.get_width() * 0.899999976158142) * 0.5), (float) (((double) Screen.get_height() - (double) Screen.get_height() * 0.899999976158142) * 0.5), (float) Screen.get_width() * 0.9f, (float) Screen.get_height() * 0.9f);
      GUILayout.BeginArea(rect);
      this.DrawPlayerSelector();
      this.DrawJoystickSelector();
      this.DrawMouseAssignment();
      this.DrawControllerSelector();
      this.DrawCalibrateButton();
      this.DrawMapCategories();
      this.actionScrollPos = GUILayout.BeginScrollView(this.actionScrollPos, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      this.DrawCategoryActions();
      GUILayout.EndScrollView();
      GUILayout.EndArea();
    }

    private void DrawPlayerSelector()
    {
      if (ReInput.get_players().get_allPlayerCount() == 0)
      {
        GUILayout.Label("There are no players.", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      }
      else
      {
        GUILayout.Space(15f);
        GUILayout.Label("Players:", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
        using (IEnumerator<Player> enumerator = ((IEnumerable<Player>) ReInput.get_players().GetPlayers(true)).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
          {
            Player current = enumerator.Current;
            if (this.selectedPlayer == null)
              this.selectedPlayer = current;
            bool flag1 = current == this.selectedPlayer;
            bool flag2 = GUILayout.Toggle((flag1 ? 1 : 0) != 0, !(current.get_descriptiveName() != string.Empty) ? current.get_name() : current.get_descriptiveName(), GUIStyle.op_Implicit("Button"), new GUILayoutOption[1]
            {
              GUILayout.ExpandWidth(false)
            });
            if (flag2 != flag1 && flag2)
            {
              this.selectedPlayer = current;
              this.selectedController.Clear();
              this.selectedMapCategoryId = -1;
            }
          }
        }
        GUILayout.EndHorizontal();
      }
    }

    private void DrawMouseAssignment()
    {
      bool enabled = GUI.get_enabled();
      if (this.selectedPlayer == null)
        GUI.set_enabled(false);
      GUILayout.Space(15f);
      GUILayout.Label("Assign Mouse:", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      bool flag1 = this.selectedPlayer != null && ((Player.ControllerHelper) this.selectedPlayer.controllers).get_hasMouse();
      bool flag2 = GUILayout.Toggle((flag1 ? 1 : 0) != 0, "Assign Mouse", GUIStyle.op_Implicit("Button"), new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      });
      if (flag2 != flag1)
      {
        if (flag2)
        {
          ((Player.ControllerHelper) this.selectedPlayer.controllers).set_hasMouse(true);
          using (IEnumerator<Player> enumerator = ((IEnumerable<Player>) ReInput.get_players().get_Players()).GetEnumerator())
          {
            while (((IEnumerator) enumerator).MoveNext())
            {
              Player current = enumerator.Current;
              if (current != this.selectedPlayer)
                ((Player.ControllerHelper) current.controllers).set_hasMouse(false);
            }
          }
        }
        else
          ((Player.ControllerHelper) this.selectedPlayer.controllers).set_hasMouse(false);
      }
      GUILayout.EndHorizontal();
      if (GUI.get_enabled() == enabled)
        return;
      GUI.set_enabled(enabled);
    }

    private void DrawJoystickSelector()
    {
      bool enabled = GUI.get_enabled();
      if (this.selectedPlayer == null)
        GUI.set_enabled(false);
      GUILayout.Space(15f);
      GUILayout.Label("Assign Joysticks:", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      bool flag1 = this.selectedPlayer == null || ((Player.ControllerHelper) this.selectedPlayer.controllers).get_joystickCount() == 0;
      if (GUILayout.Toggle((flag1 ? 1 : 0) != 0, "None", GUIStyle.op_Implicit("Button"), new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      }) != flag1)
      {
        ((Player.ControllerHelper) this.selectedPlayer.controllers).ClearControllersOfType((ControllerType) 2);
        this.ControllerSelectionChanged();
      }
      if (this.selectedPlayer != null)
      {
        using (IEnumerator<Joystick> enumerator = ((IEnumerable<Joystick>) ReInput.get_controllers().get_Joysticks()).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
          {
            Joystick current = enumerator.Current;
            bool flag2 = ((Player.ControllerHelper) this.selectedPlayer.controllers).ContainsController((Controller) current);
            bool assign = GUILayout.Toggle((flag2 ? 1 : 0) != 0, ((Controller) current).get_name(), GUIStyle.op_Implicit("Button"), new GUILayoutOption[1]
            {
              GUILayout.ExpandWidth(false)
            });
            if (assign != flag2)
              this.EnqueueAction((ControlRemappingDemo1.QueueEntry) new ControlRemappingDemo1.JoystickAssignmentChange(this.selectedPlayer.get_id(), (int) ((Controller) current).id, assign));
          }
        }
      }
      GUILayout.EndHorizontal();
      if (GUI.get_enabled() == enabled)
        return;
      GUI.set_enabled(enabled);
    }

    private void DrawControllerSelector()
    {
      if (this.selectedPlayer == null)
        return;
      bool enabled = GUI.get_enabled();
      GUILayout.Space(15f);
      GUILayout.Label("Controller to Map:", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      if (!this.selectedController.hasSelection)
      {
        this.selectedController.Set(0, (ControllerType) 0);
        this.ControllerSelectionChanged();
      }
      bool flag1 = this.selectedController.type == 0;
      if (GUILayout.Toggle((flag1 ? 1 : 0) != 0, "Keyboard", GUIStyle.op_Implicit("Button"), new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      }) != flag1)
      {
        this.selectedController.Set(0, (ControllerType) 0);
        this.ControllerSelectionChanged();
      }
      if (!((Player.ControllerHelper) this.selectedPlayer.controllers).get_hasMouse())
        GUI.set_enabled(false);
      bool flag2 = this.selectedController.type == 1;
      if (GUILayout.Toggle((flag2 ? 1 : 0) != 0, "Mouse", GUIStyle.op_Implicit("Button"), new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      }) != flag2)
      {
        this.selectedController.Set(0, (ControllerType) 1);
        this.ControllerSelectionChanged();
      }
      if (GUI.get_enabled() != enabled)
        GUI.set_enabled(enabled);
      using (IEnumerator<Joystick> enumerator = ((IEnumerable<Joystick>) ((Player.ControllerHelper) this.selectedPlayer.controllers).get_Joysticks()).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          Joystick current = enumerator.Current;
          bool flag3 = this.selectedController.type == 2 && this.selectedController.id == ((Controller) current).id;
          if (GUILayout.Toggle((flag3 ? 1 : 0) != 0, ((Controller) current).get_name(), GUIStyle.op_Implicit("Button"), new GUILayoutOption[1]
          {
            GUILayout.ExpandWidth(false)
          }) != flag3)
          {
            this.selectedController.Set((int) ((Controller) current).id, (ControllerType) 2);
            this.ControllerSelectionChanged();
          }
        }
      }
      GUILayout.EndHorizontal();
      if (GUI.get_enabled() == enabled)
        return;
      GUI.set_enabled(enabled);
    }

    private void DrawCalibrateButton()
    {
      if (this.selectedPlayer == null)
        return;
      bool enabled = GUI.get_enabled();
      GUILayout.Space(10f);
      Controller controller = !this.selectedController.hasSelection ? (Controller) null : ((Player.ControllerHelper) this.selectedPlayer.controllers).GetController(this.selectedController.type, this.selectedController.id);
      if (controller == null || this.selectedController.type != 2)
      {
        GUI.set_enabled(false);
        GUILayout.Button("Select a controller to calibrate", new GUILayoutOption[1]
        {
          GUILayout.ExpandWidth(false)
        });
        if (GUI.get_enabled() != enabled)
          GUI.set_enabled(enabled);
      }
      else if (GUILayout.Button("Calibrate " + controller.get_name(), new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      }) && controller is Joystick joystick)
      {
        CalibrationMap calibrationMap = ((ControllerWithAxes) joystick).get_calibrationMap();
        if (calibrationMap != null)
          this.EnqueueAction((ControlRemappingDemo1.QueueEntry) new ControlRemappingDemo1.Calibration(this.selectedPlayer, joystick, calibrationMap));
      }
      if (GUI.get_enabled() == enabled)
        return;
      GUI.set_enabled(enabled);
    }

    private void DrawMapCategories()
    {
      if (this.selectedPlayer == null || !this.selectedController.hasSelection)
        return;
      bool enabled = GUI.get_enabled();
      GUILayout.Space(15f);
      GUILayout.Label("Categories:", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      using (IEnumerator<InputMapCategory> enumerator = ReInput.get_mapping().get_UserAssignableMapCategories().GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          InputMapCategory current = enumerator.Current;
          if (!((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) this.selectedPlayer.controllers).maps).ContainsMapInCategory(this.selectedController.type, ((InputCategory) current).get_id()))
            GUI.set_enabled(false);
          else if (this.selectedMapCategoryId < 0)
          {
            this.selectedMapCategoryId = ((InputCategory) current).get_id();
            this.selectedMap = ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) this.selectedPlayer.controllers).maps).GetFirstMapInCategory(this.selectedController.type, this.selectedController.id, ((InputCategory) current).get_id());
          }
          bool flag = ((InputCategory) current).get_id() == this.selectedMapCategoryId;
          if (GUILayout.Toggle((flag ? 1 : 0) != 0, !(((InputCategory) current).get_descriptiveName() != string.Empty) ? ((InputCategory) current).get_name() : ((InputCategory) current).get_descriptiveName(), GUIStyle.op_Implicit("Button"), new GUILayoutOption[1]
          {
            GUILayout.ExpandWidth(false)
          }) != flag)
          {
            this.selectedMapCategoryId = ((InputCategory) current).get_id();
            this.selectedMap = ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) this.selectedPlayer.controllers).maps).GetFirstMapInCategory(this.selectedController.type, this.selectedController.id, ((InputCategory) current).get_id());
          }
          if (GUI.get_enabled() != enabled)
            GUI.set_enabled(enabled);
        }
      }
      GUILayout.EndHorizontal();
      if (GUI.get_enabled() == enabled)
        return;
      GUI.set_enabled(enabled);
    }

    private void DrawCategoryActions()
    {
      if (this.selectedPlayer == null || this.selectedMapCategoryId < 0)
        return;
      bool enabled = GUI.get_enabled();
      if (this.selectedMap == null)
        return;
      GUILayout.Space(15f);
      GUILayout.Label("Actions:", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      InputMapCategory mapCategory = ReInput.get_mapping().GetMapCategory(this.selectedMapCategoryId);
      if (mapCategory == null)
        return;
      InputCategory actionCategory = ReInput.get_mapping().GetActionCategory(((InputCategory) mapCategory).get_name());
      if (actionCategory == null)
        return;
      float num = 150f;
      using (IEnumerator<InputAction> enumerator1 = ReInput.get_mapping().ActionsInCategory(actionCategory.get_id()).GetEnumerator())
      {
        while (((IEnumerator) enumerator1).MoveNext())
        {
          InputAction current1 = enumerator1.Current;
          string str1 = !(current1.get_descriptiveName() != string.Empty) ? current1.get_name() : current1.get_descriptiveName();
          if (current1.get_type() == 1)
          {
            GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
            GUILayout.Label(str1, new GUILayoutOption[1]
            {
              GUILayout.Width(num)
            });
            this.DrawAddActionMapButton(this.selectedPlayer.get_id(), current1, (AxisRange) 1, this.selectedController, this.selectedMap);
            using (IEnumerator<ActionElementMap> enumerator2 = ((IEnumerable<ActionElementMap>) this.selectedMap.get_AllMaps()).GetEnumerator())
            {
              while (((IEnumerator) enumerator2).MoveNext())
              {
                ActionElementMap current2 = enumerator2.Current;
                if (current2.get_actionId() == current1.get_id())
                  this.DrawActionAssignmentButton(this.selectedPlayer.get_id(), current1, (AxisRange) 1, this.selectedController, this.selectedMap, current2);
              }
            }
            GUILayout.EndHorizontal();
          }
          else if (current1.get_type() == null)
          {
            if (this.selectedController.type != null)
            {
              GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
              GUILayout.Label(str1, new GUILayoutOption[1]
              {
                GUILayout.Width(num)
              });
              this.DrawAddActionMapButton(this.selectedPlayer.get_id(), current1, (AxisRange) 0, this.selectedController, this.selectedMap);
              using (IEnumerator<ActionElementMap> enumerator2 = ((IEnumerable<ActionElementMap>) this.selectedMap.get_AllMaps()).GetEnumerator())
              {
                while (((IEnumerator) enumerator2).MoveNext())
                {
                  ActionElementMap current2 = enumerator2.Current;
                  if (current2.get_actionId() == current1.get_id() && current2.get_elementType() != 1 && current2.get_axisType() != 2)
                  {
                    this.DrawActionAssignmentButton(this.selectedPlayer.get_id(), current1, (AxisRange) 0, this.selectedController, this.selectedMap, current2);
                    this.DrawInvertButton(this.selectedPlayer.get_id(), current1, (Pole) 0, this.selectedController, this.selectedMap, current2);
                  }
                }
              }
              GUILayout.EndHorizontal();
            }
            string str2 = !(current1.get_positiveDescriptiveName() != string.Empty) ? current1.get_descriptiveName() + " +" : current1.get_positiveDescriptiveName();
            GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
            GUILayout.Label(str2, new GUILayoutOption[1]
            {
              GUILayout.Width(num)
            });
            this.DrawAddActionMapButton(this.selectedPlayer.get_id(), current1, (AxisRange) 1, this.selectedController, this.selectedMap);
            using (IEnumerator<ActionElementMap> enumerator2 = ((IEnumerable<ActionElementMap>) this.selectedMap.get_AllMaps()).GetEnumerator())
            {
              while (((IEnumerator) enumerator2).MoveNext())
              {
                ActionElementMap current2 = enumerator2.Current;
                if (current2.get_actionId() == current1.get_id() && current2.get_axisContribution() == null && current2.get_axisType() != 1)
                  this.DrawActionAssignmentButton(this.selectedPlayer.get_id(), current1, (AxisRange) 1, this.selectedController, this.selectedMap, current2);
              }
            }
            GUILayout.EndHorizontal();
            string str3 = !(current1.get_negativeDescriptiveName() != string.Empty) ? current1.get_descriptiveName() + " -" : current1.get_negativeDescriptiveName();
            GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
            GUILayout.Label(str3, new GUILayoutOption[1]
            {
              GUILayout.Width(num)
            });
            this.DrawAddActionMapButton(this.selectedPlayer.get_id(), current1, (AxisRange) 2, this.selectedController, this.selectedMap);
            using (IEnumerator<ActionElementMap> enumerator2 = ((IEnumerable<ActionElementMap>) this.selectedMap.get_AllMaps()).GetEnumerator())
            {
              while (((IEnumerator) enumerator2).MoveNext())
              {
                ActionElementMap current2 = enumerator2.Current;
                if (current2.get_actionId() == current1.get_id() && current2.get_axisContribution() == 1 && current2.get_axisType() != 1)
                  this.DrawActionAssignmentButton(this.selectedPlayer.get_id(), current1, (AxisRange) 2, this.selectedController, this.selectedMap, current2);
              }
            }
            GUILayout.EndHorizontal();
          }
        }
      }
      if (GUI.get_enabled() == enabled)
        return;
      GUI.set_enabled(enabled);
    }

    private void DrawActionAssignmentButton(
      int playerId,
      InputAction action,
      AxisRange actionRange,
      ControlRemappingDemo1.ControllerSelection controller,
      ControllerMap controllerMap,
      ActionElementMap elementMap)
    {
      if (GUILayout.Button(elementMap.get_elementIdentifierName(), new GUILayoutOption[2]
      {
        GUILayout.ExpandWidth(false),
        GUILayout.MinWidth(30f)
      }))
      {
        InputMapper.Context context = new InputMapper.Context();
        context.set_actionId(action.get_id());
        context.set_actionRange(actionRange);
        context.set_controllerMap(controllerMap);
        context.set_actionElementMapToReplace(elementMap);
        this.EnqueueAction((ControlRemappingDemo1.QueueEntry) new ControlRemappingDemo1.ElementAssignmentChange(ControlRemappingDemo1.ElementAssignmentChangeType.ReassignOrRemove, context));
        this.startListening = true;
      }
      GUILayout.Space(4f);
    }

    private void DrawInvertButton(
      int playerId,
      InputAction action,
      Pole actionAxisContribution,
      ControlRemappingDemo1.ControllerSelection controller,
      ControllerMap controllerMap,
      ActionElementMap elementMap)
    {
      bool invert = elementMap.get_invert();
      bool flag = GUILayout.Toggle((invert ? 1 : 0) != 0, "Invert", new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      });
      if (flag != invert)
        elementMap.set_invert(flag);
      GUILayout.Space(10f);
    }

    private void DrawAddActionMapButton(
      int playerId,
      InputAction action,
      AxisRange actionRange,
      ControlRemappingDemo1.ControllerSelection controller,
      ControllerMap controllerMap)
    {
      if (GUILayout.Button("Add...", new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      }))
      {
        InputMapper.Context context = new InputMapper.Context();
        context.set_actionId(action.get_id());
        context.set_actionRange(actionRange);
        context.set_controllerMap(controllerMap);
        this.EnqueueAction((ControlRemappingDemo1.QueueEntry) new ControlRemappingDemo1.ElementAssignmentChange(ControlRemappingDemo1.ElementAssignmentChangeType.Add, context));
        this.startListening = true;
      }
      GUILayout.Space(10f);
    }

    private void ShowDialog()
    {
      this.dialog.Update();
    }

    private void DrawModalWindow(string title, string message)
    {
      if (!this.dialog.enabled)
        return;
      GUILayout.Space(5f);
      GUILayout.Label(message, this.style_wordWrap, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUILayout.FlexibleSpace();
      GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      this.dialog.DrawConfirmButton("Okay");
      GUILayout.FlexibleSpace();
      this.dialog.DrawCancelButton();
      GUILayout.EndHorizontal();
    }

    private void DrawModalWindow_OkayOnly(string title, string message)
    {
      if (!this.dialog.enabled)
        return;
      GUILayout.Space(5f);
      GUILayout.Label(message, this.style_wordWrap, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUILayout.FlexibleSpace();
      GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      this.dialog.DrawConfirmButton("Okay");
      GUILayout.EndHorizontal();
    }

    private void DrawElementAssignmentWindow(string title, string message)
    {
      if (!this.dialog.enabled)
        return;
      GUILayout.Space(5f);
      GUILayout.Label(message, this.style_wordWrap, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUILayout.FlexibleSpace();
      if (!(this.actionQueue.Peek() is ControlRemappingDemo1.ElementAssignmentChange assignmentChange))
      {
        this.dialog.Cancel();
      }
      else
      {
        float num;
        if (!this.dialog.busy)
        {
          if (this.startListening && this.inputMapper.get_status() == null)
          {
            this.inputMapper.Start(assignmentChange.context);
            this.startListening = false;
          }
          if (this.conflictFoundEventData != null)
          {
            this.dialog.Confirm();
            return;
          }
          num = this.inputMapper.get_timeRemaining();
          if ((double) num == 0.0)
          {
            this.dialog.Cancel();
            return;
          }
        }
        else
          num = this.inputMapper.get_options().get_timeout();
        GUILayout.Label("Assignment will be canceled in " + ((int) Mathf.Ceil(num)).ToString() + "...", this.style_wordWrap, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      }
    }

    private void DrawElementAssignmentProtectedConflictWindow(string title, string message)
    {
      if (!this.dialog.enabled)
        return;
      GUILayout.Space(5f);
      GUILayout.Label(message, this.style_wordWrap, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUILayout.FlexibleSpace();
      if (!(this.actionQueue.Peek() is ControlRemappingDemo1.ElementAssignmentChange))
      {
        this.dialog.Cancel();
      }
      else
      {
        GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
        this.dialog.DrawConfirmButton(ControlRemappingDemo1.UserResponse.Custom1, "Add");
        GUILayout.FlexibleSpace();
        this.dialog.DrawCancelButton();
        GUILayout.EndHorizontal();
      }
    }

    private void DrawElementAssignmentNormalConflictWindow(string title, string message)
    {
      if (!this.dialog.enabled)
        return;
      GUILayout.Space(5f);
      GUILayout.Label(message, this.style_wordWrap, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUILayout.FlexibleSpace();
      if (!(this.actionQueue.Peek() is ControlRemappingDemo1.ElementAssignmentChange))
      {
        this.dialog.Cancel();
      }
      else
      {
        GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
        this.dialog.DrawConfirmButton(ControlRemappingDemo1.UserResponse.Confirm, "Replace");
        GUILayout.FlexibleSpace();
        this.dialog.DrawConfirmButton(ControlRemappingDemo1.UserResponse.Custom1, "Add");
        GUILayout.FlexibleSpace();
        this.dialog.DrawCancelButton();
        GUILayout.EndHorizontal();
      }
    }

    private void DrawReassignOrRemoveElementAssignmentWindow(string title, string message)
    {
      if (!this.dialog.enabled)
        return;
      GUILayout.Space(5f);
      GUILayout.Label(message, this.style_wordWrap, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUILayout.FlexibleSpace();
      GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      this.dialog.DrawConfirmButton("Reassign");
      GUILayout.FlexibleSpace();
      this.dialog.DrawCancelButton("Remove");
      GUILayout.EndHorizontal();
    }

    private void DrawFallbackJoystickIdentificationWindow(string title, string message)
    {
      if (!this.dialog.enabled)
        return;
      if (!(this.actionQueue.Peek() is ControlRemappingDemo1.FallbackJoystickIdentification joystickIdentification))
      {
        this.dialog.Cancel();
      }
      else
      {
        GUILayout.Space(5f);
        GUILayout.Label(message, this.style_wordWrap, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
        GUILayout.Label("Press any button or axis on \"" + joystickIdentification.joystickName + "\" now.", this.style_wordWrap, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Skip", (GUILayoutOption[]) Array.Empty<GUILayoutOption>()))
        {
          this.dialog.Cancel();
        }
        else
        {
          if (this.dialog.busy || !ReInput.get_controllers().SetUnityJoystickIdFromAnyButtonOrAxisPress(joystickIdentification.joystickId, 0.8f, false))
            return;
          this.dialog.Confirm();
        }
      }
    }

    private void DrawCalibrationWindow(string title, string message)
    {
      if (!this.dialog.enabled)
        return;
      if (!(this.actionQueue.Peek() is ControlRemappingDemo1.Calibration calibration))
      {
        this.dialog.Cancel();
      }
      else
      {
        GUILayout.Space(5f);
        GUILayout.Label(message, this.style_wordWrap, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
        GUILayout.Space(20f);
        GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
        bool enabled = GUI.get_enabled();
        GUILayout.BeginVertical(new GUILayoutOption[1]
        {
          GUILayout.Width(200f)
        });
        this.calibrateScrollPos = GUILayout.BeginScrollView(this.calibrateScrollPos, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
        if (calibration.recording)
          GUI.set_enabled(false);
        IList<ControllerElementIdentifier> elementIdentifiers = ((ControllerWithAxes) calibration.joystick).get_AxisElementIdentifiers();
        for (int index = 0; index < ((ICollection<ControllerElementIdentifier>) elementIdentifiers).Count; ++index)
        {
          ControllerElementIdentifier elementIdentifier = elementIdentifiers[index];
          bool flag1 = calibration.selectedElementIdentifierId == elementIdentifier.get_id();
          bool flag2 = GUILayout.Toggle((flag1 ? 1 : 0) != 0, elementIdentifier.get_name(), GUIStyle.op_Implicit("Button"), new GUILayoutOption[1]
          {
            GUILayout.ExpandWidth(false)
          });
          if (flag1 != flag2)
            calibration.selectedElementIdentifierId = elementIdentifier.get_id();
        }
        if (GUI.get_enabled() != enabled)
          GUI.set_enabled(enabled);
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.BeginVertical(new GUILayoutOption[1]
        {
          GUILayout.Width(200f)
        });
        if (calibration.selectedElementIdentifierId >= 0)
        {
          float axisRawById = ((ControllerWithAxes) calibration.joystick).GetAxisRawById(calibration.selectedElementIdentifierId);
          GUILayout.Label("Raw Value: " + axisRawById.ToString(), (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
          int axisIndexById = ((ControllerWithAxes) calibration.joystick).GetAxisIndexById(calibration.selectedElementIdentifierId);
          AxisCalibration axis = calibration.calibrationMap.GetAxis(axisIndexById);
          GUILayout.Label("Calibrated Value: " + (object) ((ControllerWithAxes) calibration.joystick).GetAxisById(calibration.selectedElementIdentifierId), (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
          GUILayout.Label("Zero: " + (object) axis.get_calibratedZero(), (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
          GUILayout.Label("Min: " + (object) axis.get_calibratedMin(), (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
          GUILayout.Label("Max: " + (object) axis.get_calibratedMax(), (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
          GUILayout.Label("Dead Zone: " + (object) axis.get_deadZone(), (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
          GUILayout.Space(15f);
          bool flag1 = GUILayout.Toggle((axis.get_enabled() ? 1 : 0) != 0, "Enabled", GUIStyle.op_Implicit("Button"), new GUILayoutOption[1]
          {
            GUILayout.ExpandWidth(false)
          });
          if (axis.get_enabled() != flag1)
            axis.set_enabled(flag1);
          GUILayout.Space(10f);
          bool flag2 = GUILayout.Toggle((calibration.recording ? 1 : 0) != 0, "Record Min/Max", GUIStyle.op_Implicit("Button"), new GUILayoutOption[1]
          {
            GUILayout.ExpandWidth(false)
          });
          if (flag2 != calibration.recording)
          {
            if (flag2)
            {
              axis.set_calibratedMax(0.0f);
              axis.set_calibratedMin(0.0f);
            }
            calibration.recording = flag2;
          }
          if (calibration.recording)
          {
            axis.set_calibratedMin(Mathf.Min(new float[3]
            {
              axis.get_calibratedMin(),
              axisRawById,
              axis.get_calibratedMin()
            }));
            axis.set_calibratedMax(Mathf.Max(new float[3]
            {
              axis.get_calibratedMax(),
              axisRawById,
              axis.get_calibratedMax()
            }));
            GUI.set_enabled(false);
          }
          if (GUILayout.Button("Set Zero", new GUILayoutOption[1]
          {
            GUILayout.ExpandWidth(false)
          }))
            axis.set_calibratedZero(axisRawById);
          if (GUILayout.Button("Set Dead Zone", new GUILayoutOption[1]
          {
            GUILayout.ExpandWidth(false)
          }))
            axis.set_deadZone(axisRawById);
          bool flag3 = GUILayout.Toggle((axis.get_invert() ? 1 : 0) != 0, "Invert", GUIStyle.op_Implicit("Button"), new GUILayoutOption[1]
          {
            GUILayout.ExpandWidth(false)
          });
          if (axis.get_invert() != flag3)
            axis.set_invert(flag3);
          GUILayout.Space(10f);
          if (GUILayout.Button("Reset", new GUILayoutOption[1]
          {
            GUILayout.ExpandWidth(false)
          }))
            axis.Reset();
          if (GUI.get_enabled() != enabled)
            GUI.set_enabled(enabled);
        }
        else
          GUILayout.Label("Select an axis to begin.", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        if (calibration.recording)
          GUI.set_enabled(false);
        if (GUILayout.Button("Close", (GUILayoutOption[]) Array.Empty<GUILayoutOption>()))
        {
          this.calibrateScrollPos = (Vector2) null;
          this.dialog.Confirm();
        }
        if (GUI.get_enabled() == enabled)
          return;
        GUI.set_enabled(enabled);
      }
    }

    private void DialogResultCallback(
      int queueActionId,
      ControlRemappingDemo1.UserResponse response)
    {
      foreach (ControlRemappingDemo1.QueueEntry action in this.actionQueue)
      {
        if (action.id == queueActionId)
        {
          if (response != ControlRemappingDemo1.UserResponse.Cancel)
          {
            action.Confirm(response);
            break;
          }
          action.Cancel();
          break;
        }
      }
    }

    private Rect GetScreenCenteredRect(float width, float height)
    {
      return new Rect((float) ((double) Screen.get_width() * 0.5 - (double) width * 0.5), (float) ((double) Screen.get_height() * 0.5 - (double) height * 0.5), width, height);
    }

    private void EnqueueAction(ControlRemappingDemo1.QueueEntry entry)
    {
      if (entry == null)
        return;
      this.busy = true;
      GUI.set_enabled(false);
      this.actionQueue.Enqueue(entry);
    }

    private void ProcessQueue()
    {
      if (this.dialog.enabled || this.busy || this.actionQueue.Count == 0)
        return;
      while (this.actionQueue.Count > 0)
      {
        ControlRemappingDemo1.QueueEntry queueEntry = this.actionQueue.Peek();
        bool flag = false;
        switch (queueEntry.queueActionType)
        {
          case ControlRemappingDemo1.QueueActionType.JoystickAssignment:
            flag = this.ProcessJoystickAssignmentChange((ControlRemappingDemo1.JoystickAssignmentChange) queueEntry);
            break;
          case ControlRemappingDemo1.QueueActionType.ElementAssignment:
            flag = this.ProcessElementAssignmentChange((ControlRemappingDemo1.ElementAssignmentChange) queueEntry);
            break;
          case ControlRemappingDemo1.QueueActionType.FallbackJoystickIdentification:
            flag = this.ProcessFallbackJoystickIdentification((ControlRemappingDemo1.FallbackJoystickIdentification) queueEntry);
            break;
          case ControlRemappingDemo1.QueueActionType.Calibrate:
            flag = this.ProcessCalibration((ControlRemappingDemo1.Calibration) queueEntry);
            break;
        }
        if (!flag)
          break;
        this.actionQueue.Dequeue();
      }
    }

    private bool ProcessJoystickAssignmentChange(
      ControlRemappingDemo1.JoystickAssignmentChange entry)
    {
      if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
        return true;
      Player player = ReInput.get_players().GetPlayer(entry.playerId);
      if (player == null)
        return true;
      if (!entry.assign)
      {
        ((Player.ControllerHelper) player.controllers).RemoveController((ControllerType) 2, entry.joystickId);
        this.ControllerSelectionChanged();
        return true;
      }
      if (((Player.ControllerHelper) player.controllers).ContainsController((ControllerType) 2, entry.joystickId))
        return true;
      if (!ReInput.get_controllers().IsJoystickAssigned(entry.joystickId) || entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
      {
        ((Player.ControllerHelper) player.controllers).AddController((ControllerType) 2, entry.joystickId, true);
        this.ControllerSelectionChanged();
        return true;
      }
      this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.JoystickConflict, new ControlRemappingDemo1.WindowProperties()
      {
        title = "Joystick Reassignment",
        message = "This joystick is already assigned to another player. Do you want to reassign this joystick to " + player.get_descriptiveName() + "?",
        rect = this.GetScreenCenteredRect(250f, 200f),
        windowDrawDelegate = new Action<string, string>(this.DrawModalWindow)
      }, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
      return false;
    }

    private bool ProcessElementAssignmentChange(
      ControlRemappingDemo1.ElementAssignmentChange entry)
    {
      switch (entry.changeType)
      {
        case ControlRemappingDemo1.ElementAssignmentChangeType.Add:
        case ControlRemappingDemo1.ElementAssignmentChangeType.Replace:
          return this.ProcessAddOrReplaceElementAssignment(entry);
        case ControlRemappingDemo1.ElementAssignmentChangeType.Remove:
          return this.ProcessRemoveElementAssignment(entry);
        case ControlRemappingDemo1.ElementAssignmentChangeType.ReassignOrRemove:
          return this.ProcessRemoveOrReassignElementAssignment(entry);
        case ControlRemappingDemo1.ElementAssignmentChangeType.ConflictCheck:
          return this.ProcessElementAssignmentConflictCheck(entry);
        default:
          throw new NotImplementedException();
      }
    }

    private bool ProcessRemoveOrReassignElementAssignment(
      ControlRemappingDemo1.ElementAssignmentChange entry)
    {
      if (entry.context.get_controllerMap() == null)
        return true;
      if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
      {
        this.actionQueue.Enqueue((ControlRemappingDemo1.QueueEntry) new ControlRemappingDemo1.ElementAssignmentChange(entry)
        {
          changeType = ControlRemappingDemo1.ElementAssignmentChangeType.Remove
        });
        return true;
      }
      if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
      {
        this.actionQueue.Enqueue((ControlRemappingDemo1.QueueEntry) new ControlRemappingDemo1.ElementAssignmentChange(entry)
        {
          changeType = ControlRemappingDemo1.ElementAssignmentChangeType.Replace
        });
        return true;
      }
      this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.AssignElement, new ControlRemappingDemo1.WindowProperties()
      {
        title = "Reassign or Remove",
        message = "Do you want to reassign or remove this assignment?",
        rect = this.GetScreenCenteredRect(250f, 200f),
        windowDrawDelegate = new Action<string, string>(this.DrawReassignOrRemoveElementAssignmentWindow)
      }, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
      return false;
    }

    private bool ProcessRemoveElementAssignment(
      ControlRemappingDemo1.ElementAssignmentChange entry)
    {
      if (entry.context.get_controllerMap() == null || entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
        return true;
      if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
      {
        entry.context.get_controllerMap().DeleteElementMap(entry.context.get_actionElementMapToReplace().get_id());
        return true;
      }
      this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.DeleteAssignmentConfirmation, new ControlRemappingDemo1.WindowProperties()
      {
        title = "Remove Assignment",
        message = "Are you sure you want to remove this assignment?",
        rect = this.GetScreenCenteredRect(250f, 200f),
        windowDrawDelegate = new Action<string, string>(this.DrawModalWindow)
      }, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
      return false;
    }

    private bool ProcessAddOrReplaceElementAssignment(
      ControlRemappingDemo1.ElementAssignmentChange entry)
    {
      if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
      {
        this.inputMapper.Stop();
        return true;
      }
      if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
      {
        if (Event.get_current().get_type() != 8)
          return false;
        if (this.conflictFoundEventData != null)
          this.actionQueue.Enqueue((ControlRemappingDemo1.QueueEntry) new ControlRemappingDemo1.ElementAssignmentChange(entry)
          {
            changeType = ControlRemappingDemo1.ElementAssignmentChangeType.ConflictCheck
          });
        return true;
      }
      string str;
      if (entry.context.get_controllerMap().get_controllerType() == null)
      {
        str = Application.get_platform() != null && Application.get_platform() != 1 ? "Press any key to assign it to this action. You may also use the modifier keys Control, Alt, and Shift. If you wish to assign a modifier key itself to this action, press and hold the key for 1 second." : "Press any key to assign it to this action. You may also use the modifier keys Command, Control, Alt, and Shift. If you wish to assign a modifier key itself to this action, press and hold the key for 1 second.";
        if (Application.get_isEditor())
          str += "\n\nNOTE: Some modifier key combinations will not work in the Unity Editor, but they will work in a game build.";
      }
      else
        str = entry.context.get_controllerMap().get_controllerType() != 1 ? "Press any button or axis to assign it to this action." : "Press any mouse button or axis to assign it to this action.\n\nTo assign mouse movement axes, move the mouse quickly in the direction you want mapped to the action. Slow movements will be ignored.";
      this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.AssignElement, new ControlRemappingDemo1.WindowProperties()
      {
        title = "Assign",
        message = str,
        rect = this.GetScreenCenteredRect(250f, 200f),
        windowDrawDelegate = new Action<string, string>(this.DrawElementAssignmentWindow)
      }, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
      return false;
    }

    private bool ProcessElementAssignmentConflictCheck(
      ControlRemappingDemo1.ElementAssignmentChange entry)
    {
      if (entry.context.get_controllerMap() == null)
        return true;
      if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
      {
        this.inputMapper.Stop();
        return true;
      }
      if (this.conflictFoundEventData == null)
        return true;
      if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
      {
        if (entry.response == ControlRemappingDemo1.UserResponse.Confirm)
        {
          ((Action<InputMapper.ConflictResponse>) this.conflictFoundEventData.responseCallback)((InputMapper.ConflictResponse) 1);
        }
        else
        {
          if (entry.response != ControlRemappingDemo1.UserResponse.Custom1)
            throw new NotImplementedException();
          ((Action<InputMapper.ConflictResponse>) this.conflictFoundEventData.responseCallback)((InputMapper.ConflictResponse) 2);
        }
        return true;
      }
      if (this.conflictFoundEventData.isProtected != null)
      {
        string str = ((ElementAssignmentInfo) this.conflictFoundEventData.assignment).get_elementDisplayName() + " is already in use and is protected from reassignment. You cannot remove the protected assignment, but you can still assign the action to this element. If you do so, the element will trigger multiple actions when activated.";
        this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.AssignElement, new ControlRemappingDemo1.WindowProperties()
        {
          title = "Assignment Conflict",
          message = str,
          rect = this.GetScreenCenteredRect(250f, 200f),
          windowDrawDelegate = new Action<string, string>(this.DrawElementAssignmentProtectedConflictWindow)
        }, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
      }
      else
      {
        string str = ((ElementAssignmentInfo) this.conflictFoundEventData.assignment).get_elementDisplayName() + " is already in use. You may replace the other conflicting assignments, add this assignment anyway which will leave multiple actions assigned to this element, or cancel this assignment.";
        this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.AssignElement, new ControlRemappingDemo1.WindowProperties()
        {
          title = "Assignment Conflict",
          message = str,
          rect = this.GetScreenCenteredRect(250f, 200f),
          windowDrawDelegate = new Action<string, string>(this.DrawElementAssignmentNormalConflictWindow)
        }, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
      }
      return false;
    }

    private bool ProcessFallbackJoystickIdentification(
      ControlRemappingDemo1.FallbackJoystickIdentification entry)
    {
      if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled || entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
        return true;
      this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.JoystickConflict, new ControlRemappingDemo1.WindowProperties()
      {
        title = "Joystick Identification Required",
        message = "A joystick has been attached or removed. You will need to identify each joystick by pressing a button on the controller listed below:",
        rect = this.GetScreenCenteredRect(250f, 200f),
        windowDrawDelegate = new Action<string, string>(this.DrawFallbackJoystickIdentificationWindow)
      }, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback), 1f);
      return false;
    }

    private bool ProcessCalibration(ControlRemappingDemo1.Calibration entry)
    {
      if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled || entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
        return true;
      this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.JoystickConflict, new ControlRemappingDemo1.WindowProperties()
      {
        title = "Calibrate Controller",
        message = "Select an axis to calibrate on the " + ((Controller) entry.joystick).get_name() + ".",
        rect = this.GetScreenCenteredRect(450f, 480f),
        windowDrawDelegate = new Action<string, string>(this.DrawCalibrationWindow)
      }, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
      return false;
    }

    private void PlayerSelectionChanged()
    {
      this.ClearControllerSelection();
    }

    private void ControllerSelectionChanged()
    {
      this.ClearMapSelection();
    }

    private void ClearControllerSelection()
    {
      this.selectedController.Clear();
      this.ClearMapSelection();
    }

    private void ClearMapSelection()
    {
      this.selectedMapCategoryId = -1;
      this.selectedMap = (ControllerMap) null;
    }

    private void ResetAll()
    {
      this.ClearWorkingVars();
      this.initialized = false;
      this.showMenu = false;
    }

    private void ClearWorkingVars()
    {
      this.selectedPlayer = (Player) null;
      this.ClearMapSelection();
      this.selectedController.Clear();
      this.actionScrollPos = (Vector2) null;
      this.dialog.FullReset();
      this.actionQueue.Clear();
      this.busy = false;
      this.startListening = false;
      this.conflictFoundEventData = (InputMapper.ConflictFoundEventData) null;
      this.inputMapper.Stop();
    }

    private void SetGUIStateStart()
    {
      this.guiState = true;
      if (this.busy)
        this.guiState = false;
      this.pageGUIState = this.guiState && !this.busy && !this.dialog.enabled && !this.dialog.busy;
      if (GUI.get_enabled() == this.guiState)
        return;
      GUI.set_enabled(this.guiState);
    }

    private void SetGUIStateEnd()
    {
      this.guiState = true;
      if (GUI.get_enabled())
        return;
      GUI.set_enabled(this.guiState);
    }

    private void JoystickConnected(ControllerStatusChangedEventArgs args)
    {
      if (ReInput.get_controllers().IsControllerAssigned(args.get_controllerType(), args.get_controllerId()))
      {
        using (IEnumerator<Player> enumerator = ((IEnumerable<Player>) ReInput.get_players().get_AllPlayers()).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
          {
            Player current = enumerator.Current;
            if (((Player.ControllerHelper) current.controllers).ContainsController(args.get_controllerType(), args.get_controllerId()))
              ReInput.get_userDataStore().LoadControllerData(current.get_id(), args.get_controllerType(), args.get_controllerId());
          }
        }
      }
      else
        ReInput.get_userDataStore().LoadControllerData(args.get_controllerType(), args.get_controllerId());
      if (!ReInput.get_unityJoystickIdentificationRequired())
        return;
      this.IdentifyAllJoysticks();
    }

    private void JoystickPreDisconnect(ControllerStatusChangedEventArgs args)
    {
      if (this.selectedController.hasSelection && args.get_controllerType() == this.selectedController.type && args.get_controllerId() == this.selectedController.id)
        this.ClearControllerSelection();
      if (!this.showMenu)
        return;
      if (ReInput.get_controllers().IsControllerAssigned(args.get_controllerType(), args.get_controllerId()))
      {
        using (IEnumerator<Player> enumerator = ((IEnumerable<Player>) ReInput.get_players().get_AllPlayers()).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
          {
            Player current = enumerator.Current;
            if (((Player.ControllerHelper) current.controllers).ContainsController(args.get_controllerType(), args.get_controllerId()))
              ReInput.get_userDataStore().SaveControllerData(current.get_id(), args.get_controllerType(), args.get_controllerId());
          }
        }
      }
      else
        ReInput.get_userDataStore().SaveControllerData(args.get_controllerType(), args.get_controllerId());
    }

    private void JoystickDisconnected(ControllerStatusChangedEventArgs args)
    {
      if (this.showMenu)
        this.ClearWorkingVars();
      if (!ReInput.get_unityJoystickIdentificationRequired())
        return;
      this.IdentifyAllJoysticks();
    }

    private void OnConflictFound(InputMapper.ConflictFoundEventData data)
    {
      this.conflictFoundEventData = data;
    }

    private void OnStopped(InputMapper.StoppedEventData data)
    {
      this.conflictFoundEventData = (InputMapper.ConflictFoundEventData) null;
    }

    public void IdentifyAllJoysticks()
    {
      if (ReInput.get_controllers().get_joystickCount() == 0)
        return;
      this.ClearWorkingVars();
      this.Open();
      using (IEnumerator<Joystick> enumerator = ((IEnumerable<Joystick>) ReInput.get_controllers().get_Joysticks()).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          Joystick current = enumerator.Current;
          this.actionQueue.Enqueue((ControlRemappingDemo1.QueueEntry) new ControlRemappingDemo1.FallbackJoystickIdentification((int) ((Controller) current).id, ((Controller) current).get_name()));
        }
      }
    }

    protected void CheckRecompile()
    {
    }

    private void RecompileWindow(int windowId)
    {
    }

    private class ControllerSelection
    {
      private int _id;
      private int _idPrev;
      private ControllerType _type;
      private ControllerType _typePrev;

      public ControllerSelection()
      {
        this.Clear();
      }

      public int id
      {
        get
        {
          return this._id;
        }
        set
        {
          this._idPrev = this._id;
          this._id = value;
        }
      }

      public ControllerType type
      {
        get
        {
          return this._type;
        }
        set
        {
          this._typePrev = this._type;
          this._type = value;
        }
      }

      public int idPrev
      {
        get
        {
          return this._idPrev;
        }
      }

      public ControllerType typePrev
      {
        get
        {
          return this._typePrev;
        }
      }

      public bool hasSelection
      {
        get
        {
          return this._id >= 0;
        }
      }

      public void Set(int id, ControllerType type)
      {
        this.id = id;
        this.type = type;
      }

      public void Clear()
      {
        this._id = -1;
        this._idPrev = -1;
        this._type = (ControllerType) 2;
        this._typePrev = (ControllerType) 2;
      }
    }

    private class DialogHelper
    {
      private const float openBusyDelay = 0.25f;
      private const float closeBusyDelay = 0.1f;
      private ControlRemappingDemo1.DialogHelper.DialogType _type;
      private bool _enabled;
      private float _busyTime;
      private bool _busyTimerRunning;
      private Action<int> drawWindowDelegate;
      private GUI.WindowFunction drawWindowFunction;
      private ControlRemappingDemo1.WindowProperties windowProperties;
      private int currentActionId;
      private Action<int, ControlRemappingDemo1.UserResponse> resultCallback;

      public DialogHelper()
      {
        this.drawWindowDelegate = new Action<int>(this.DrawWindow);
        // ISSUE: method pointer
        this.drawWindowFunction = new GUI.WindowFunction((object) this.drawWindowDelegate, __methodptr(Invoke));
      }

      private float busyTimer
      {
        get
        {
          return !this._busyTimerRunning ? 0.0f : this._busyTime - Time.get_realtimeSinceStartup();
        }
      }

      public bool enabled
      {
        get
        {
          return this._enabled;
        }
        set
        {
          if (value)
          {
            if (this._type == ControlRemappingDemo1.DialogHelper.DialogType.None)
              return;
            this.StateChanged(0.25f);
          }
          else
          {
            this._enabled = value;
            this._type = ControlRemappingDemo1.DialogHelper.DialogType.None;
            this.StateChanged(0.1f);
          }
        }
      }

      public ControlRemappingDemo1.DialogHelper.DialogType type
      {
        get
        {
          return !this._enabled ? ControlRemappingDemo1.DialogHelper.DialogType.None : this._type;
        }
        set
        {
          if (value == ControlRemappingDemo1.DialogHelper.DialogType.None)
          {
            this._enabled = false;
            this.StateChanged(0.1f);
          }
          else
          {
            this._enabled = true;
            this.StateChanged(0.25f);
          }
          this._type = value;
        }
      }

      public bool busy
      {
        get
        {
          return this._busyTimerRunning;
        }
      }

      public void StartModal(
        int queueActionId,
        ControlRemappingDemo1.DialogHelper.DialogType type,
        ControlRemappingDemo1.WindowProperties windowProperties,
        Action<int, ControlRemappingDemo1.UserResponse> resultCallback)
      {
        this.StartModal(queueActionId, type, windowProperties, resultCallback, -1f);
      }

      public void StartModal(
        int queueActionId,
        ControlRemappingDemo1.DialogHelper.DialogType type,
        ControlRemappingDemo1.WindowProperties windowProperties,
        Action<int, ControlRemappingDemo1.UserResponse> resultCallback,
        float openBusyDelay)
      {
        this.currentActionId = queueActionId;
        this.windowProperties = windowProperties;
        this.type = type;
        this.resultCallback = resultCallback;
        if ((double) openBusyDelay < 0.0)
          return;
        this.StateChanged(openBusyDelay);
      }

      public void Update()
      {
        this.Draw();
        this.UpdateTimers();
      }

      public void Draw()
      {
        if (!this._enabled)
          return;
        bool enabled = GUI.get_enabled();
        GUI.set_enabled(true);
        GUILayout.Window(this.windowProperties.windowId, this.windowProperties.rect, this.drawWindowFunction, this.windowProperties.title, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
        GUI.FocusWindow(this.windowProperties.windowId);
        if (GUI.get_enabled() == enabled)
          return;
        GUI.set_enabled(enabled);
      }

      public void DrawConfirmButton()
      {
        this.DrawConfirmButton("Confirm");
      }

      public void DrawConfirmButton(string title)
      {
        bool enabled = GUI.get_enabled();
        if (this.busy)
          GUI.set_enabled(false);
        if (GUILayout.Button(title, (GUILayoutOption[]) Array.Empty<GUILayoutOption>()))
          this.Confirm(ControlRemappingDemo1.UserResponse.Confirm);
        if (GUI.get_enabled() == enabled)
          return;
        GUI.set_enabled(enabled);
      }

      public void DrawConfirmButton(ControlRemappingDemo1.UserResponse response)
      {
        this.DrawConfirmButton(response, "Confirm");
      }

      public void DrawConfirmButton(ControlRemappingDemo1.UserResponse response, string title)
      {
        bool enabled = GUI.get_enabled();
        if (this.busy)
          GUI.set_enabled(false);
        if (GUILayout.Button(title, (GUILayoutOption[]) Array.Empty<GUILayoutOption>()))
          this.Confirm(response);
        if (GUI.get_enabled() == enabled)
          return;
        GUI.set_enabled(enabled);
      }

      public void DrawCancelButton()
      {
        this.DrawCancelButton("Cancel");
      }

      public void DrawCancelButton(string title)
      {
        bool enabled = GUI.get_enabled();
        if (this.busy)
          GUI.set_enabled(false);
        if (GUILayout.Button(title, (GUILayoutOption[]) Array.Empty<GUILayoutOption>()))
          this.Cancel();
        if (GUI.get_enabled() == enabled)
          return;
        GUI.set_enabled(enabled);
      }

      public void Confirm()
      {
        this.Confirm(ControlRemappingDemo1.UserResponse.Confirm);
      }

      public void Confirm(ControlRemappingDemo1.UserResponse response)
      {
        this.resultCallback(this.currentActionId, response);
        this.Close();
      }

      public void Cancel()
      {
        this.resultCallback(this.currentActionId, ControlRemappingDemo1.UserResponse.Cancel);
        this.Close();
      }

      private void DrawWindow(int windowId)
      {
        this.windowProperties.windowDrawDelegate(this.windowProperties.title, this.windowProperties.message);
      }

      private void UpdateTimers()
      {
        if (!this._busyTimerRunning || (double) this.busyTimer > 0.0)
          return;
        this._busyTimerRunning = false;
      }

      private void StartBusyTimer(float time)
      {
        this._busyTime = time + Time.get_realtimeSinceStartup();
        this._busyTimerRunning = true;
      }

      private void Close()
      {
        this.Reset();
        this.StateChanged(0.1f);
      }

      private void StateChanged(float delay)
      {
        this.StartBusyTimer(delay);
      }

      private void Reset()
      {
        this._enabled = false;
        this._type = ControlRemappingDemo1.DialogHelper.DialogType.None;
        this.currentActionId = -1;
        this.resultCallback = (Action<int, ControlRemappingDemo1.UserResponse>) null;
      }

      private void ResetTimers()
      {
        this._busyTimerRunning = false;
      }

      public void FullReset()
      {
        this.Reset();
        this.ResetTimers();
      }

      public enum DialogType
      {
        None = 0,
        JoystickConflict = 1,
        ElementConflict = 2,
        KeyConflict = 3,
        DeleteAssignmentConfirmation = 10, // 0x0000000A
        AssignElement = 11, // 0x0000000B
      }
    }

    private abstract class QueueEntry
    {
      private static int uidCounter;

      public QueueEntry(
        ControlRemappingDemo1.QueueActionType queueActionType)
      {
        this.id = ControlRemappingDemo1.QueueEntry.nextId;
        this.queueActionType = queueActionType;
      }

      public int id { get; protected set; }

      public ControlRemappingDemo1.QueueActionType queueActionType { get; protected set; }

      public ControlRemappingDemo1.QueueEntry.State state { get; protected set; }

      public ControlRemappingDemo1.UserResponse response { get; protected set; }

      protected static int nextId
      {
        get
        {
          int uidCounter = ControlRemappingDemo1.QueueEntry.uidCounter;
          ++ControlRemappingDemo1.QueueEntry.uidCounter;
          return uidCounter;
        }
      }

      public void Confirm(ControlRemappingDemo1.UserResponse response)
      {
        this.state = ControlRemappingDemo1.QueueEntry.State.Confirmed;
        this.response = response;
      }

      public void Cancel()
      {
        this.state = ControlRemappingDemo1.QueueEntry.State.Canceled;
      }

      public enum State
      {
        Waiting,
        Confirmed,
        Canceled,
      }
    }

    private class JoystickAssignmentChange : ControlRemappingDemo1.QueueEntry
    {
      public JoystickAssignmentChange(int newPlayerId, int joystickId, bool assign)
        : base(ControlRemappingDemo1.QueueActionType.JoystickAssignment)
      {
        this.playerId = newPlayerId;
        this.joystickId = joystickId;
        this.assign = assign;
      }

      public int playerId { get; private set; }

      public int joystickId { get; private set; }

      public bool assign { get; private set; }
    }

    private class ElementAssignmentChange : ControlRemappingDemo1.QueueEntry
    {
      public ElementAssignmentChange(
        ControlRemappingDemo1.ElementAssignmentChangeType changeType,
        InputMapper.Context context)
        : base(ControlRemappingDemo1.QueueActionType.ElementAssignment)
      {
        this.changeType = changeType;
        this.context = context;
      }

      public ElementAssignmentChange(
        ControlRemappingDemo1.ElementAssignmentChange other)
        : this(other.changeType, other.context.Clone())
      {
      }

      public ControlRemappingDemo1.ElementAssignmentChangeType changeType { get; set; }

      public InputMapper.Context context { get; private set; }
    }

    private class FallbackJoystickIdentification : ControlRemappingDemo1.QueueEntry
    {
      public FallbackJoystickIdentification(int joystickId, string joystickName)
        : base(ControlRemappingDemo1.QueueActionType.FallbackJoystickIdentification)
      {
        this.joystickId = joystickId;
        this.joystickName = joystickName;
      }

      public int joystickId { get; private set; }

      public string joystickName { get; private set; }
    }

    private class Calibration : ControlRemappingDemo1.QueueEntry
    {
      public int selectedElementIdentifierId;
      public bool recording;

      public Calibration(Player player, Joystick joystick, CalibrationMap calibrationMap)
        : base(ControlRemappingDemo1.QueueActionType.Calibrate)
      {
        this.player = player;
        this.joystick = joystick;
        this.calibrationMap = calibrationMap;
        this.selectedElementIdentifierId = -1;
      }

      public Player player { get; private set; }

      public ControllerType controllerType { get; private set; }

      public Joystick joystick { get; private set; }

      public CalibrationMap calibrationMap { get; private set; }
    }

    private struct WindowProperties
    {
      public int windowId;
      public Rect rect;
      public Action<string, string> windowDrawDelegate;
      public string title;
      public string message;
    }

    private enum QueueActionType
    {
      None,
      JoystickAssignment,
      ElementAssignment,
      FallbackJoystickIdentification,
      Calibrate,
    }

    private enum ElementAssignmentChangeType
    {
      Add,
      Replace,
      Remove,
      ReassignOrRemove,
      ConflictCheck,
    }

    public enum UserResponse
    {
      Confirm,
      Cancel,
      Custom1,
      Custom2,
    }
  }
}
