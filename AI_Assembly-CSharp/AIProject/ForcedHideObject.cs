// Decompiled with JetBrains decompiler
// Type: AIProject.ForcedHideObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject
{
  public class ForcedHideObject : MonoBehaviour
  {
    private bool _forcedHideFlag;
    private bool _firstInit;

    public ForcedHideObject()
    {
      base.\u002Ector();
    }

    private bool ForcedHideFlag
    {
      get
      {
        return this._forcedHideFlag;
      }
      set
      {
        this._forcedHideFlag = value;
      }
    }

    public void Init()
    {
      if (!this._firstInit)
        return;
      this._firstInit = false;
    }

    private void OnEnable()
    {
      if (!this._forcedHideFlag)
        return;
      ((Component) this).get_gameObject().SetActive(false);
    }

    private void LateUpdate()
    {
      if (this._firstInit || !this._forcedHideFlag)
        return;
      ((Component) this).get_gameObject().SetActive(false);
    }

    public void SetActive(bool active)
    {
      if (active)
        this.Show();
      else
        this.Hide();
    }

    public void Show()
    {
      this._forcedHideFlag = false;
      if (((Component) this).get_gameObject().get_activeSelf())
        return;
      ((Component) this).get_gameObject().SetActive(true);
    }

    public void Hide()
    {
      this._forcedHideFlag = true;
      if (!((Component) this).get_gameObject().get_activeSelf())
        return;
      ((Component) this).get_gameObject().SetActive(false);
    }

    public bool Active
    {
      get
      {
        return Object.op_Inequality((Object) ((Component) this).get_gameObject(), (Object) null) && ((Component) this).get_gameObject().get_activeSelf();
      }
    }
  }
}
