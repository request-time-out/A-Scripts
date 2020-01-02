// Decompiled with JetBrains decompiler
// Type: Studio.MPCameraCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class MPCameraCtrl : MonoBehaviour
  {
    [SerializeField]
    private TMP_InputField inputName;
    [SerializeField]
    private Toggle toggleActive;
    private OCICamera m_OCICamera;
    private bool m_Active;
    private bool isUpdateInfo;

    public MPCameraCtrl()
    {
      base.\u002Ector();
    }

    public OCICamera ociCamera
    {
      get
      {
        return this.m_OCICamera;
      }
      set
      {
        this.m_OCICamera = value;
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
        if (this.m_Active)
          ((Component) this).get_gameObject().SetActive(this.m_OCICamera != null);
        else
          ((Component) this).get_gameObject().SetActive(false);
      }
    }

    public bool Deselect(OCICamera _ociCamera)
    {
      if (this.m_OCICamera != _ociCamera)
        return false;
      this.ociCamera = (OCICamera) null;
      this.active = false;
      return true;
    }

    public void UpdateInfo()
    {
      if (this.m_OCICamera == null)
        return;
      this.isUpdateInfo = true;
      this.inputName.set_text(this.m_OCICamera.name);
      this.toggleActive.set_isOn(this.m_OCICamera.cameraInfo.active);
      this.isUpdateInfo = false;
    }

    private void OnEndEditName(string _value)
    {
      if (this.isUpdateInfo)
        return;
      this.m_OCICamera.name = _value;
      Singleton<Studio.Studio>.Instance.cameraSelector.Init();
    }

    private void OnValueChangedActive(bool _value)
    {
      if (this.isUpdateInfo)
        return;
      Singleton<Studio.Studio>.Instance.ChangeCamera(this.m_OCICamera, _value, false);
    }

    private void Start()
    {
      // ISSUE: method pointer
      ((UnityEvent<string>) this.inputName.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditName)));
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.toggleActive.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedActive)));
    }
  }
}
