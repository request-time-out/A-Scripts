// Decompiled with JetBrains decompiler
// Type: ME_AnimatorEvents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class ME_AnimatorEvents : MonoBehaviour
{
  public GameObject EffectPrefab;
  public GameObject SwordPrefab;
  public Transform SwordPosition;
  public Transform StartSwordPosition;
  private GameObject EffectInstance;
  private GameObject SwordInstance;

  public ME_AnimatorEvents()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (Object.op_Inequality((Object) this.SwordInstance, (Object) null))
      Object.Destroy((Object) this.SwordInstance);
    this.SwordInstance = (GameObject) Object.Instantiate<GameObject>((M0) this.SwordPrefab, this.StartSwordPosition.get_position(), this.StartSwordPosition.get_rotation());
    this.SwordInstance.get_transform().set_parent(((Component) this.StartSwordPosition).get_transform());
  }

  public void ActivateEffect()
  {
    if (Object.op_Equality((Object) this.EffectPrefab, (Object) null) || Object.op_Equality((Object) this.SwordInstance, (Object) null))
      return;
    if (Object.op_Inequality((Object) this.EffectInstance, (Object) null))
      Object.Destroy((Object) this.EffectInstance);
    this.EffectInstance = (GameObject) Object.Instantiate<GameObject>((M0) this.EffectPrefab);
    this.EffectInstance.get_transform().set_parent(this.SwordInstance.get_transform());
    this.EffectInstance.get_transform().set_localPosition(Vector3.get_zero());
    this.EffectInstance.get_transform().set_localRotation((Quaternion) null);
    ((PSMeshRendererUpdater) this.EffectInstance.GetComponent<PSMeshRendererUpdater>()).UpdateMeshEffect(this.SwordInstance);
  }

  public void ActivateSword()
  {
    this.SwordInstance.get_transform().set_parent(((Component) this.SwordPosition).get_transform());
    this.SwordInstance.get_transform().set_position(this.SwordPosition.get_position());
    this.SwordInstance.get_transform().set_rotation(this.SwordPosition.get_rotation());
  }

  public void UpdateColor(float HUE)
  {
    if (Object.op_Equality((Object) this.EffectInstance, (Object) null))
      return;
    ME_EffectSettingColor effectSettingColor = (ME_EffectSettingColor) this.EffectInstance.GetComponent<ME_EffectSettingColor>();
    if (Object.op_Equality((Object) effectSettingColor, (Object) null))
      effectSettingColor = (ME_EffectSettingColor) this.EffectInstance.AddComponent<ME_EffectSettingColor>();
    ME_ColorHelper.HSBColor hsv = ME_ColorHelper.ColorToHSV(effectSettingColor.Color);
    hsv.H = HUE;
    effectSettingColor.Color = ME_ColorHelper.HSVToColor(hsv);
  }
}
