// Decompiled with JetBrains decompiler
// Type: Studio.InputFieldManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Studio
{
  [AddComponentMenu("Studio/Manager/Input Field", 1000)]
  public class InputFieldManager : Singleton<InputFieldManager>
  {
    private StudioInputField m_StudioInputField;

    public static StudioInputField studioInputField
    {
      set
      {
        if (!Singleton<InputFieldManager>.IsInstance())
          return;
        Singleton<InputFieldManager>.Instance.m_StudioInputField = value;
        ((Behaviour) Singleton<InputFieldManager>.Instance).set_enabled(Object.op_Inequality((Object) value, (Object) null));
      }
    }

    public static bool isFocused
    {
      get
      {
        return Singleton<InputFieldManager>.IsInstance() && Object.op_Implicit((Object) Singleton<InputFieldManager>.Instance.m_StudioInputField) && Singleton<InputFieldManager>.Instance.m_StudioInputField.get_isFocused();
      }
    }

    protected override void Awake()
    {
      if (!this.CheckInstance())
        return;
      Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
      InputFieldManager.studioInputField = (StudioInputField) null;
    }

    private void Update()
    {
      if (!Object.op_Implicit((Object) this.m_StudioInputField) || !this.m_StudioInputField.get_isFocused() || (!Input.get_anyKey() || Input.GetMouseButton(0)))
        return;
      Input.ResetInputAxes();
    }
  }
}
