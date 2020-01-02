// Decompiled with JetBrains decompiler
// Type: Studio.VoicePlayNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class VoicePlayNode : VoiceNode
  {
    [SerializeField]
    private Button buttonDelete;
    private Image m_ImageButton;
    private bool m_Select;

    public UnityAction addOnClickDelete
    {
      set
      {
        ((UnityEvent) this.buttonDelete.get_onClick()).AddListener(value);
      }
    }

    private Image image
    {
      get
      {
        if (Object.op_Equality((Object) this.m_ImageButton, (Object) null))
          this.m_ImageButton = ((Selectable) this.m_Button).get_image();
        return this.m_ImageButton;
      }
    }

    public bool select
    {
      get
      {
        return this.m_Select;
      }
      set
      {
        if (!Utility.SetStruct<bool>(ref this.m_Select, value))
          return;
        ((Graphic) this.image).set_color(!this.m_Select ? Color.get_white() : Color.get_green());
      }
    }

    public void Destroy()
    {
      Object.Destroy((Object) ((Component) this).get_gameObject());
    }
  }
}
