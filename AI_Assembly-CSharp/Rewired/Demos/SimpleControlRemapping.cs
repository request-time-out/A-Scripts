// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.SimpleControlRemapping
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Rewired.Demos
{
  [AddComponentMenu("")]
  public class SimpleControlRemapping : MonoBehaviour
  {
    private const string category = "Default";
    private const string layout = "Default";
    private InputMapper inputMapper;
    public GameObject buttonPrefab;
    public GameObject textPrefab;
    public RectTransform fieldGroupTransform;
    public RectTransform actionGroupTransform;
    public Text controllerNameUIText;
    public Text statusUIText;
    private ControllerType selectedControllerType;
    private int selectedControllerId;
    private List<SimpleControlRemapping.Row> rows;

    public SimpleControlRemapping()
    {
      base.\u002Ector();
    }

    private Player player
    {
      get
      {
        return ReInput.get_players().GetPlayer(0);
      }
    }

    private ControllerMap controllerMap
    {
      get
      {
        return this.controller == null ? (ControllerMap) null : ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) this.player.controllers).maps).GetMap(this.controller.get_type(), (int) this.controller.id, "Default", "Default");
      }
    }

    private Controller controller
    {
      get
      {
        return ((Player.ControllerHelper) this.player.controllers).GetController(this.selectedControllerType, this.selectedControllerId);
      }
    }

    private void OnEnable()
    {
      if (!ReInput.get_isReady())
        return;
      this.inputMapper.get_options().set_timeout(5f);
      this.inputMapper.get_options().set_ignoreMouseXAxis(true);
      this.inputMapper.get_options().set_ignoreMouseYAxis(true);
      ReInput.add_ControllerConnectedEvent(new Action<ControllerStatusChangedEventArgs>(this.OnControllerChanged));
      ReInput.add_ControllerDisconnectedEvent(new Action<ControllerStatusChangedEventArgs>(this.OnControllerChanged));
      this.inputMapper.add_InputMappedEvent(new Action<InputMapper.InputMappedEventData>(this.OnInputMapped));
      this.inputMapper.add_StoppedEvent(new Action<InputMapper.StoppedEventData>(this.OnStopped));
      this.InitializeUI();
    }

    private void OnDisable()
    {
      this.inputMapper.Stop();
      this.inputMapper.RemoveAllEventListeners();
      ReInput.remove_ControllerConnectedEvent(new Action<ControllerStatusChangedEventArgs>(this.OnControllerChanged));
      ReInput.remove_ControllerDisconnectedEvent(new Action<ControllerStatusChangedEventArgs>(this.OnControllerChanged));
    }

    private void RedrawUI()
    {
      if (this.controller == null)
      {
        this.ClearUI();
      }
      else
      {
        this.controllerNameUIText.set_text(this.controller.get_name());
        for (int index = 0; index < this.rows.Count; ++index)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          SimpleControlRemapping.\u003CRedrawUI\u003Ec__AnonStorey0 redrawUiCAnonStorey0 = new SimpleControlRemapping.\u003CRedrawUI\u003Ec__AnonStorey0();
          // ISSUE: reference to a compiler-generated field
          redrawUiCAnonStorey0.\u0024this = this;
          SimpleControlRemapping.Row row = this.rows[index];
          InputAction action = this.rows[index].action;
          string str = string.Empty;
          // ISSUE: reference to a compiler-generated field
          redrawUiCAnonStorey0.actionElementMapId = -1;
          using (IEnumerator<ActionElementMap> enumerator = this.controllerMap.ElementMapsWithAction(action.get_id()).GetEnumerator())
          {
            while (((IEnumerator) enumerator).MoveNext())
            {
              ActionElementMap current = enumerator.Current;
              if (current.ShowInField(row.actionRange))
              {
                str = current.get_elementIdentifierName();
                // ISSUE: reference to a compiler-generated field
                redrawUiCAnonStorey0.actionElementMapId = current.get_id();
                break;
              }
            }
          }
          row.text.set_text(str);
          ((UnityEventBase) row.button.get_onClick()).RemoveAllListeners();
          // ISSUE: reference to a compiler-generated field
          redrawUiCAnonStorey0.index = index;
          // ISSUE: method pointer
          ((UnityEvent) row.button.get_onClick()).AddListener(new UnityAction((object) redrawUiCAnonStorey0, __methodptr(\u003C\u003Em__0)));
        }
      }
    }

    private void ClearUI()
    {
      if (this.selectedControllerType == 2)
        this.controllerNameUIText.set_text("No joysticks attached");
      else
        this.controllerNameUIText.set_text(string.Empty);
      for (int index = 0; index < this.rows.Count; ++index)
        this.rows[index].text.set_text(string.Empty);
    }

    private void InitializeUI()
    {
      IEnumerator enumerator1 = ((Transform) this.actionGroupTransform).GetEnumerator();
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
      IEnumerator enumerator2 = ((Transform) this.fieldGroupTransform).GetEnumerator();
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
      using (IEnumerator<InputAction> enumerator3 = ((IEnumerable<InputAction>) ReInput.get_mapping().get_Actions()).GetEnumerator())
      {
        while (((IEnumerator) enumerator3).MoveNext())
        {
          InputAction current = enumerator3.Current;
          if (current.get_type() == null)
          {
            this.CreateUIRow(current, (AxisRange) 0, current.get_descriptiveName());
            this.CreateUIRow(current, (AxisRange) 1, string.IsNullOrEmpty(current.get_positiveDescriptiveName()) ? current.get_descriptiveName() + " +" : current.get_positiveDescriptiveName());
            this.CreateUIRow(current, (AxisRange) 2, string.IsNullOrEmpty(current.get_negativeDescriptiveName()) ? current.get_descriptiveName() + " -" : current.get_negativeDescriptiveName());
          }
          else if (current.get_type() == 1)
            this.CreateUIRow(current, (AxisRange) 1, current.get_descriptiveName());
        }
      }
      this.RedrawUI();
    }

    private void CreateUIRow(InputAction action, AxisRange actionRange, string label)
    {
      GameObject gameObject1 = (GameObject) Object.Instantiate<GameObject>((M0) this.textPrefab);
      gameObject1.get_transform().SetParent((Transform) this.actionGroupTransform);
      gameObject1.get_transform().SetAsLastSibling();
      ((Text) gameObject1.GetComponent<Text>()).set_text(label);
      GameObject gameObject2 = (GameObject) Object.Instantiate<GameObject>((M0) this.buttonPrefab);
      gameObject2.get_transform().SetParent((Transform) this.fieldGroupTransform);
      gameObject2.get_transform().SetAsLastSibling();
      this.rows.Add(new SimpleControlRemapping.Row()
      {
        action = action,
        actionRange = actionRange,
        button = (Button) gameObject2.GetComponent<Button>(),
        text = (Text) gameObject2.GetComponentInChildren<Text>()
      });
    }

    private void SetSelectedController(ControllerType controllerType)
    {
      bool flag = false;
      if (controllerType != this.selectedControllerType)
      {
        this.selectedControllerType = controllerType;
        flag = true;
      }
      int selectedControllerId = this.selectedControllerId;
      this.selectedControllerId = this.selectedControllerType != 2 ? 0 : (((Player.ControllerHelper) this.player.controllers).get_joystickCount() <= 0 ? -1 : (int) ((Controller) ((Player.ControllerHelper) this.player.controllers).get_Joysticks()[0]).id);
      if (this.selectedControllerId != selectedControllerId)
        flag = true;
      if (!flag)
        return;
      this.inputMapper.Stop();
      this.RedrawUI();
    }

    public void OnControllerSelected(int controllerType)
    {
      this.SetSelectedController((ControllerType) controllerType);
    }

    private void OnInputFieldClicked(int index, int actionElementMapToReplaceId)
    {
      if (index < 0 || index >= this.rows.Count || this.controller == null)
        return;
      InputMapper inputMapper = this.inputMapper;
      InputMapper.Context context1 = new InputMapper.Context();
      context1.set_actionId(this.rows[index].action.get_id());
      context1.set_controllerMap(this.controllerMap);
      context1.set_actionRange(this.rows[index].actionRange);
      context1.set_actionElementMapToReplace(this.controllerMap.GetElementMap(actionElementMapToReplaceId));
      InputMapper.Context context2 = context1;
      inputMapper.Start(context2);
      this.statusUIText.set_text("Listening...");
    }

    private void OnControllerChanged(ControllerStatusChangedEventArgs args)
    {
      this.SetSelectedController(this.selectedControllerType);
    }

    private void OnInputMapped(InputMapper.InputMappedEventData data)
    {
      this.RedrawUI();
    }

    private void OnStopped(InputMapper.StoppedEventData data)
    {
      this.statusUIText.set_text(string.Empty);
    }

    private class Row
    {
      public InputAction action;
      public AxisRange actionRange;
      public Button button;
      public Text text;
    }
  }
}
