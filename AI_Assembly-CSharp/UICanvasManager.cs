// Decompiled with JetBrains decompiler
// Type: UICanvasManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class UICanvasManager : MonoBehaviour
{
  public static UICanvasManager GlobalAccess;
  public bool MouseOverButton;
  public Text PENameText;
  public Text ToolTipText;
  private RaycastHit rayHit;

  public UICanvasManager()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    UICanvasManager.GlobalAccess = this;
  }

  private void Start()
  {
    if (!Object.op_Inequality((Object) this.PENameText, (Object) null))
      return;
    this.PENameText.set_text(ParticleEffectsLibrary.GlobalAccess.GetCurrentPENameString());
  }

  private void Update()
  {
    if (!this.MouseOverButton && Input.GetMouseButtonUp(0))
      this.SpawnCurrentParticleEffect();
    if (Input.GetKeyUp((KeyCode) 97))
      this.SelectPreviousPE();
    if (!Input.GetKeyUp((KeyCode) 100))
      return;
    this.SelectNextPE();
  }

  public void UpdateToolTip(ButtonTypes toolTipType)
  {
    if (!Object.op_Inequality((Object) this.ToolTipText, (Object) null))
      return;
    if (toolTipType == ButtonTypes.Previous)
    {
      this.ToolTipText.set_text("Select Previous Particle Effect");
    }
    else
    {
      if (toolTipType != ButtonTypes.Next)
        return;
      this.ToolTipText.set_text("Select Next Particle Effect");
    }
  }

  public void ClearToolTip()
  {
    if (!Object.op_Inequality((Object) this.ToolTipText, (Object) null))
      return;
    this.ToolTipText.set_text(string.Empty);
  }

  private void SelectPreviousPE()
  {
    ParticleEffectsLibrary.GlobalAccess.PreviousParticleEffect();
    if (!Object.op_Inequality((Object) this.PENameText, (Object) null))
      return;
    this.PENameText.set_text(ParticleEffectsLibrary.GlobalAccess.GetCurrentPENameString());
  }

  private void SelectNextPE()
  {
    ParticleEffectsLibrary.GlobalAccess.NextParticleEffect();
    if (!Object.op_Inequality((Object) this.PENameText, (Object) null))
      return;
    this.PENameText.set_text(ParticleEffectsLibrary.GlobalAccess.GetCurrentPENameString());
  }

  private void SpawnCurrentParticleEffect()
  {
    if (!Physics.Raycast(Camera.get_main().ScreenPointToRay(Input.get_mousePosition()), ref this.rayHit))
      return;
    ParticleEffectsLibrary.GlobalAccess.SpawnParticleEffect(((RaycastHit) ref this.rayHit).get_point());
  }

  public void UIButtonClick(ButtonTypes buttonTypeClicked)
  {
    if (buttonTypeClicked != ButtonTypes.Previous)
    {
      if (buttonTypeClicked != ButtonTypes.Next)
        return;
      this.SelectNextPE();
    }
    else
      this.SelectPreviousPE();
  }
}
