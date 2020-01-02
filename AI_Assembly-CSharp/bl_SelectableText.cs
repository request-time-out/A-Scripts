// Decompiled with JetBrains decompiler
// Type: bl_SelectableText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class bl_SelectableText : MonoBehaviour
{
  [SerializeField]
  private Color OnEnterColor;
  [SerializeField]
  [Range(0.1f, 3f)]
  private float Duration;
  private Text m_Text;
  private Button m_Button;
  private Color defaultColor;
  private ColorBlock defaultColorBlock;
  private ColorBlock OnSelectColorBlock;

  public bl_SelectableText()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    if (Object.op_Inequality((Object) ((Component) this).GetComponent<Text>(), (Object) null))
    {
      this.m_Text = (Text) ((Component) this).GetComponent<Text>();
      this.defaultColor = ((Graphic) this.m_Text).get_color();
    }
    if (!Object.op_Inequality((Object) ((Component) this).GetComponent<Button>(), (Object) null))
      return;
    this.m_Button = (Button) ((Component) this).GetComponent<Button>();
    this.defaultColorBlock = ((Selectable) this.m_Button).get_colors();
    this.OnSelectColorBlock = this.defaultColorBlock;
    ((ColorBlock) ref this.OnSelectColorBlock).set_normalColor(this.OnEnterColor);
  }

  public void OnEnter()
  {
    if (Object.op_Inequality((Object) this.m_Text, (Object) null))
      ((Graphic) this.m_Text).CrossFadeColor(this.OnEnterColor, this.Duration, true, true);
    if (!Object.op_Inequality((Object) this.m_Button, (Object) null))
      return;
    ((Selectable) this.m_Button).set_colors(this.OnSelectColorBlock);
  }

  public void OnExit()
  {
    if (Object.op_Inequality((Object) this.m_Text, (Object) null))
      ((Graphic) this.m_Text).CrossFadeColor(this.defaultColor, this.Duration, true, true);
    if (!Object.op_Inequality((Object) this.m_Button, (Object) null))
      return;
    ((Selectable) this.m_Button).set_colors(this.defaultColorBlock);
  }
}
