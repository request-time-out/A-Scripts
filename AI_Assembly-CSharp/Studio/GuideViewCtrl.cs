// Decompiled with JetBrains decompiler
// Type: Studio.GuideViewCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Studio
{
  public class GuideViewCtrl : MonoBehaviour
  {
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private LayerMask layerMask;
    private LayerMask layerMaskDefault;
    [SerializeField]
    private DrawLightLine drawLightLine;
    private bool isDefault;

    public GuideViewCtrl()
    {
      base.\u002Ector();
    }

    public void OnClick()
    {
      this.isDefault = !this.isDefault;
      this.camera.set_cullingMask(LayerMask.op_Implicit(!this.isDefault ? this.layerMask : this.layerMaskDefault));
      ((Behaviour) this.drawLightLine).set_enabled(this.isDefault);
    }

    private void Awake()
    {
      ((Behaviour) this.camera).set_enabled(true);
      this.layerMaskDefault = LayerMask.op_Implicit(this.camera.get_cullingMask());
      ((Behaviour) this.drawLightLine).set_enabled(true);
      this.isDefault = true;
    }
  }
}
