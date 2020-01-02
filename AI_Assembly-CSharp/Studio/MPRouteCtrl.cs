// Decompiled with JetBrains decompiler
// Type: Studio.MPRouteCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class MPRouteCtrl : MonoBehaviour
  {
    [SerializeField]
    private TMP_InputField inputName;
    [SerializeField]
    private Button buttonAddPoint;
    [SerializeField]
    private MPRouteCtrl.ToggleGroup toggleOrient;
    [SerializeField]
    private Toggle toggleLoop;
    [SerializeField]
    private Toggle toggleLine;
    [SerializeField]
    private Button buttonColor;
    [SerializeField]
    private RouteControl routeControl;
    private OCIRoute m_OCIRoute;
    private bool m_Active;
    private bool isUpdateInfo;
    private bool isColorFunc;

    public MPRouteCtrl()
    {
      base.\u002Ector();
    }

    public OCIRoute ociRoute
    {
      get
      {
        return this.m_OCIRoute;
      }
      set
      {
        this.m_OCIRoute = value;
        this.UpdateInfo();
      }
    }

    public bool active
    {
      get
      {
        return this.m_Active;
      }
      set
      {
        this.m_Active = value;
        ((Component) this).get_gameObject().SetActive(this.m_Active && this.m_OCIRoute != null);
        this.routeControl.visible = value;
        if (!this.isColorFunc || value)
          return;
        Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
      }
    }

    public bool Deselect(OCIRoute _ociRoute)
    {
      if (this.m_OCIRoute != _ociRoute)
        return false;
      this.ociRoute = (OCIRoute) null;
      this.active = false;
      return true;
    }

    public void UpdateInteractable(OCIRoute _route)
    {
      if (this.m_OCIRoute != _route)
        return;
      bool flag = !this.m_OCIRoute.isPlay;
      ((Selectable) this.buttonAddPoint).set_interactable(flag);
      this.toggleOrient.interactable = flag;
      ((Selectable) this.toggleLoop).set_interactable(flag);
    }

    private void UpdateInfo()
    {
      if (this.m_OCIRoute == null)
        return;
      this.isUpdateInfo = true;
      this.inputName.set_text(this.m_OCIRoute.name);
      this.toggleLoop.set_isOn(this.m_OCIRoute.routeInfo.loop);
      this.toggleLine.set_isOn(this.m_OCIRoute.visibleLine);
      this.toggleOrient.isOn = (int) this.m_OCIRoute.routeInfo.orient;
      ((Graphic) ((Selectable) this.buttonColor).get_image()).set_color(this.m_OCIRoute.routeInfo.color);
      this.isUpdateInfo = false;
      this.UpdateInteractable(this.m_OCIRoute);
    }

    private void OnEndEditName(string _value)
    {
      if (this.isUpdateInfo)
        return;
      this.m_OCIRoute.name = _value;
    }

    private void OnClickAddPoint()
    {
      if (this.m_OCIRoute == null)
        return;
      OCIRoutePoint ociRoutePoint = this.m_OCIRoute.AddPoint();
      if (!Studio.Studio.optionSystem.autoSelect || ociRoutePoint == null)
        return;
      Singleton<Studio.Studio>.Instance.treeNodeCtrl.SelectSingle(ociRoutePoint.treeNodeObject, true);
    }

    private void OnValueChangedPlay(bool _value)
    {
      if (this.m_OCIRoute == null)
        return;
      if (_value)
        this.m_OCIRoute.Play();
      else
        this.m_OCIRoute.Stop(true);
    }

    private void OnValueChangedLoop(bool _value)
    {
      List<OCIRoute> list = ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 4)).Select<ObjectCtrlInfo, OCIRoute>((Func<ObjectCtrlInfo, OCIRoute>) (v => v as OCIRoute)).ToList<OCIRoute>();
      list.Add(this.m_OCIRoute);
      HashSet<OCIRoute> ociRouteSet = new HashSet<OCIRoute>();
      foreach (OCIRoute ociRoute in list)
      {
        ociRoute.routeInfo.loop = _value;
        ociRouteSet.Add(ociRoute);
      }
      foreach (OCIRoute ociRoute in ociRouteSet)
        ociRoute.ForceUpdateLine();
    }

    private void OnValueChangedLine(bool _value)
    {
      List<OCIRoute> list = ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 4)).Select<ObjectCtrlInfo, OCIRoute>((Func<ObjectCtrlInfo, OCIRoute>) (v => v as OCIRoute)).ToList<OCIRoute>();
      list.Add(this.m_OCIRoute);
      foreach (OCIRoute ociRoute in list)
        ociRoute.visibleLine = _value;
    }

    private void OnValueChangedOrient(bool _value, int _idx)
    {
      if (this.isUpdateInfo || !_value)
        return;
      List<OCIRoute> list = ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 4)).Select<ObjectCtrlInfo, OCIRoute>((Func<ObjectCtrlInfo, OCIRoute>) (v => v as OCIRoute)).ToList<OCIRoute>();
      list.Add(this.m_OCIRoute);
      foreach (OCIRoute ociRoute in list)
        ociRoute.routeInfo.orient = (OIRouteInfo.Orient) _idx;
      this.toggleOrient.isOn = _idx;
    }

    private void OnClickColor()
    {
      if (Singleton<Studio.Studio>.Instance.colorPalette.Check("ルートのラインカラー"))
      {
        this.isColorFunc = false;
        Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
      }
      else
      {
        List<OCIRoute> array = ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 4)).Select<ObjectCtrlInfo, OCIRoute>((Func<ObjectCtrlInfo, OCIRoute>) (v => v as OCIRoute)).ToList<OCIRoute>();
        array.Add(this.m_OCIRoute);
        Singleton<Studio.Studio>.Instance.colorPalette.Setup("ルートのラインカラー", this.m_OCIRoute.routeInfo.color, (Action<Color>) (_c =>
        {
          foreach (OCIRoute ociRoute in array)
          {
            ociRoute.routeInfo.color = _c;
            ociRoute.SetLineColor(_c);
          }
          ((Graphic) ((Selectable) this.buttonColor).get_image()).set_color(_c);
        }), false);
        this.isColorFunc = true;
      }
    }

    private void Start()
    {
      // ISSUE: method pointer
      ((UnityEvent<string>) this.inputName.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditName)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonAddPoint.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickAddPoint)));
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.toggleLoop.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedLoop)));
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.toggleLine.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedLine)));
      this.toggleOrient.action = new Action<bool, int>(this.OnValueChangedOrient);
      // ISSUE: method pointer
      ((UnityEvent) this.buttonColor.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickColor)));
    }

    [Serializable]
    private class ToggleGroup
    {
      [SerializeField]
      private Toggle[] toggle;

      public int isOn
      {
        get
        {
          return Array.FindIndex<Toggle>(this.toggle, (Predicate<Toggle>) (_t => _t.get_isOn()));
        }
        set
        {
          for (int index = 0; index < this.toggle.Length; ++index)
            this.toggle[index].set_isOn(index == value);
        }
      }

      public bool interactable
      {
        set
        {
          foreach (Selectable selectable in this.toggle)
            selectable.set_interactable(value);
        }
      }

      public Toggle this[int _idx]
      {
        get
        {
          return this.toggle[_idx];
        }
      }

      public Action<bool, int> action
      {
        set
        {
          MPRouteCtrl.ToggleGroup.\u003C\u003Ec__AnonStorey0 cAnonStorey0 = new MPRouteCtrl.ToggleGroup.\u003C\u003Ec__AnonStorey0();
          cAnonStorey0.value = value;
          for (int index = 0; index < this.toggle.Length; ++index)
            ((UnityEvent<bool>) this.toggle[index].onValueChanged).AddListener(new UnityAction<bool>((object) new MPRouteCtrl.ToggleGroup.\u003C\u003Ec__AnonStorey1()
            {
              \u003C\u003Ef__ref\u00240 = cAnonStorey0,
              no = index
            }, __methodptr(\u003C\u003Em__0)));
        }
      }
    }
  }
}
