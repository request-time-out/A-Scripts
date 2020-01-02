// Decompiled with JetBrains decompiler
// Type: Studio.MPRoutePointCtrl
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
  public class MPRoutePointCtrl : MonoBehaviour
  {
    [SerializeField]
    private MPRoutePointCtrl.InputCombination inputSpeed;
    [SerializeField]
    private Dropdown dropdownEase;
    [SerializeField]
    private MPRoutePointCtrl.ToggleGroup toggleConnection;
    [SerializeField]
    private Toggle toggleLink;
    private OCIRoutePoint m_OCIRoutePoint;
    private bool m_Active;
    private List<MPRoutePointCtrl.EaseInfo> listEase;
    private bool isUpdateInfo;

    public MPRoutePointCtrl()
    {
      base.\u002Ector();
    }

    public OCIRoutePoint ociRoutePoint
    {
      get
      {
        return this.m_OCIRoutePoint;
      }
      set
      {
        this.m_OCIRoutePoint = value;
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
        ((Component) this).get_gameObject().SetActive(this.m_Active && this.m_OCIRoutePoint != null);
      }
    }

    public bool Deselect(OCIRoutePoint _ociRoutePoint)
    {
      if (this.m_OCIRoutePoint != _ociRoutePoint)
        return false;
      this.ociRoutePoint = (OCIRoutePoint) null;
      this.active = false;
      return true;
    }

    public void UpdateInteractable(OCIRoute _route)
    {
      if (_route == null || !_route.listPoint.Contains(this.m_OCIRoutePoint))
        return;
      bool flag = !_route.isPlay;
      this.inputSpeed.interactable = flag;
      ((Selectable) this.dropdownEase).set_interactable(flag);
      this.toggleConnection.interactable = flag;
      ((Selectable) this.toggleLink).set_interactable(flag);
      if (this.m_OCIRoutePoint.connection != OIRoutePointInfo.Connection.Curve || !this.m_OCIRoutePoint.link)
        return;
      int index = _route.listPoint.FindIndex((Predicate<OCIRoutePoint>) (p => p == this.m_OCIRoutePoint)) - 1;
      OCIRoutePoint ociRoutePoint = _route.listPoint.SafeGet<OCIRoutePoint>(index);
      if (ociRoutePoint == null || ociRoutePoint.connection != OIRoutePointInfo.Connection.Curve)
        return;
      this.inputSpeed.interactable = false;
      ((Selectable) this.dropdownEase).set_interactable(false);
    }

    private void UpdateInfo()
    {
      if (this.m_OCIRoutePoint == null)
        return;
      this.isUpdateInfo = true;
      this.inputSpeed.value = this.m_OCIRoutePoint.routePointInfo.speed;
      int index = this.listEase.FindIndex((Predicate<MPRoutePointCtrl.EaseInfo>) (e => e.ease == this.m_OCIRoutePoint.easeType));
      this.dropdownEase.set_value(index >= 0 ? index : 0);
      this.toggleConnection.isOn = (int) this.m_OCIRoutePoint.connection;
      this.toggleLink.set_isOn(this.m_OCIRoutePoint.link);
      this.isUpdateInfo = false;
      this.UpdateInteractable(this.m_OCIRoutePoint.route);
    }

    private void OnValueChangeSpeed(float _value)
    {
      if (this.isUpdateInfo)
        return;
      foreach (OCIRoutePoint ociRoutePoint in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 6)).Select<ObjectCtrlInfo, OCIRoutePoint>((Func<ObjectCtrlInfo, OCIRoutePoint>) (v => v as OCIRoutePoint)))
        ociRoutePoint.speed = _value;
      this.inputSpeed.value = _value;
    }

    private void OnEndEditSpeed(string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), this.inputSpeed.min, this.inputSpeed.max);
      foreach (OCIRoutePoint ociRoutePoint in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 6)).Select<ObjectCtrlInfo, OCIRoutePoint>((Func<ObjectCtrlInfo, OCIRoutePoint>) (v => v as OCIRoutePoint)))
        ociRoutePoint.speed = num;
      this.inputSpeed.value = num;
    }

    private void OnValueChangedEase(int _value)
    {
      if (this.m_OCIRoutePoint == null)
        return;
      IEnumerable<OCIRoutePoint> ociRoutePoints = ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 6)).Select<ObjectCtrlInfo, OCIRoutePoint>((Func<ObjectCtrlInfo, OCIRoutePoint>) (v => v as OCIRoutePoint));
      StudioTween.EaseType ease = this.listEase[_value].ease;
      foreach (OCIRoutePoint ociRoutePoint in ociRoutePoints)
        ociRoutePoint.easeType = ease;
    }

    private void OnValueChangedConnection(bool _value, int _idx)
    {
      if (this.isUpdateInfo || this.m_OCIRoutePoint == null || !_value)
        return;
      this.toggleConnection.isOn = _idx;
      IEnumerable<OCIRoutePoint> ociRoutePoints = ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 6)).Select<ObjectCtrlInfo, OCIRoutePoint>((Func<ObjectCtrlInfo, OCIRoutePoint>) (v => v as OCIRoutePoint));
      HashSet<OCIRoute> ociRouteSet = new HashSet<OCIRoute>();
      foreach (OCIRoutePoint ociRoutePoint in ociRoutePoints)
      {
        ociRoutePoint.connection = (OIRoutePointInfo.Connection) _idx;
        ociRouteSet.Add(ociRoutePoint.route);
      }
      foreach (OCIRoute ociRoute in ociRouteSet)
        ociRoute.ForceUpdateLine();
      this.UpdateInteractable(this.m_OCIRoutePoint.route);
    }

    private void OnValueChangedLink(bool _value)
    {
      if (this.isUpdateInfo)
        return;
      IEnumerable<OCIRoutePoint> ociRoutePoints = ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 6)).Select<ObjectCtrlInfo, OCIRoutePoint>((Func<ObjectCtrlInfo, OCIRoutePoint>) (v => v as OCIRoutePoint));
      HashSet<OCIRoute> ociRouteSet = new HashSet<OCIRoute>();
      foreach (OCIRoutePoint ociRoutePoint in ociRoutePoints)
      {
        ociRoutePoint.link = _value;
        ociRouteSet.Add(ociRoutePoint.route);
      }
      foreach (OCIRoute ociRoute in ociRouteSet)
        ociRoute.ForceUpdateLine();
      this.UpdateInteractable(this.m_OCIRoutePoint.route);
    }

    private void Awake()
    {
      this.listEase = new List<MPRoutePointCtrl.EaseInfo>();
      this.listEase.Add(new MPRoutePointCtrl.EaseInfo("直線的", StudioTween.EaseType.linear));
      this.listEase.Add(new MPRoutePointCtrl.EaseInfo("徐々に早く", StudioTween.EaseType.easeInQuad));
      this.listEase.Add(new MPRoutePointCtrl.EaseInfo("徐々に遅く", StudioTween.EaseType.easeOutQuad));
      this.listEase.Add(new MPRoutePointCtrl.EaseInfo("急に早く", StudioTween.EaseType.easeInQuart));
      this.listEase.Add(new MPRoutePointCtrl.EaseInfo("急に遅く", StudioTween.EaseType.easeOutQuart));
      this.listEase.Add(new MPRoutePointCtrl.EaseInfo("バウンド", StudioTween.EaseType.easeOutBounce));
      this.dropdownEase.set_options(this.listEase.Select<MPRoutePointCtrl.EaseInfo, Dropdown.OptionData>((Func<MPRoutePointCtrl.EaseInfo, Dropdown.OptionData>) (v => new Dropdown.OptionData(v.name))).ToList<Dropdown.OptionData>());
      // ISSUE: method pointer
      ((UnityEvent<string>) this.inputSpeed.input.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditSpeed)));
      // ISSUE: method pointer
      ((UnityEvent<float>) this.inputSpeed.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangeSpeed)));
      // ISSUE: method pointer
      ((UnityEvent<int>) this.dropdownEase.get_onValueChanged()).AddListener(new UnityAction<int>((object) this, __methodptr(OnValueChangedEase)));
      this.toggleConnection.action = new Action<bool, int>(this.OnValueChangedConnection);
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.toggleLink.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedLink)));
    }

    [Serializable]
    private class InputCombination
    {
      public TMP_InputField input;
      public Slider slider;

      public bool interactable
      {
        set
        {
          ((Selectable) this.input).set_interactable(value);
          ((Selectable) this.slider).set_interactable(value);
        }
      }

      public string text
      {
        get
        {
          return this.input.get_text();
        }
        set
        {
          this.input.set_text(value);
          this.slider.set_value(Utility.StringToFloat(value));
        }
      }

      public float value
      {
        get
        {
          return this.slider.get_value();
        }
        set
        {
          this.slider.set_value(value);
          this.input.set_text(value.ToString("0.0"));
        }
      }

      public float min
      {
        get
        {
          return this.slider.get_minValue();
        }
      }

      public float max
      {
        get
        {
          return this.slider.get_maxValue();
        }
      }
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
          MPRoutePointCtrl.ToggleGroup.\u003C\u003Ec__AnonStorey0 cAnonStorey0 = new MPRoutePointCtrl.ToggleGroup.\u003C\u003Ec__AnonStorey0();
          cAnonStorey0.value = value;
          for (int index = 0; index < this.toggle.Length; ++index)
            ((UnityEvent<bool>) this.toggle[index].onValueChanged).AddListener(new UnityAction<bool>((object) new MPRoutePointCtrl.ToggleGroup.\u003C\u003Ec__AnonStorey1()
            {
              \u003C\u003Ef__ref\u00240 = cAnonStorey0,
              no = index
            }, __methodptr(\u003C\u003Em__0)));
        }
      }
    }

    private class EaseInfo
    {
      public EaseInfo(string _name, StudioTween.EaseType _ease)
      {
        this.name = _name;
        this.ease = _ease;
      }

      public string name { get; private set; }

      public StudioTween.EaseType ease { get; private set; }
    }
  }
}
