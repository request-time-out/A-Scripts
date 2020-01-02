// Decompiled with JetBrains decompiler
// Type: Studio.MPFolderCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class MPFolderCtrl : MonoBehaviour
  {
    [SerializeField]
    private InputField inputName;
    private OCIFolder m_OCIFolder;
    private bool m_Active;
    private bool isUpdateInfo;

    public MPFolderCtrl()
    {
      base.\u002Ector();
    }

    public OCIFolder ociFolder
    {
      get
      {
        return this.m_OCIFolder;
      }
      set
      {
        this.m_OCIFolder = value;
        if (this.m_OCIFolder == null)
          return;
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
          ((Component) this).get_gameObject().SetActive(this.m_OCIFolder != null);
        else
          ((Component) this).get_gameObject().SetActive(false);
      }
    }

    public bool Deselect(OCIFolder _ociFolder)
    {
      if (this.m_OCIFolder != _ociFolder)
        return false;
      this.ociFolder = (OCIFolder) null;
      this.active = false;
      return true;
    }

    private void UpdateInfo()
    {
      if (this.m_OCIFolder == null)
        return;
      this.isUpdateInfo = true;
      this.inputName.set_text(this.m_OCIFolder.name);
      this.isUpdateInfo = false;
    }

    private void OnEndEditName(string _value)
    {
      if (this.isUpdateInfo)
        return;
      this.m_OCIFolder.name = _value;
    }

    private void Start()
    {
      // ISSUE: method pointer
      ((UnityEvent<string>) this.inputName.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditName)));
    }
  }
}
