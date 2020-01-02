// Decompiled with JetBrains decompiler
// Type: HAnimationInfoComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class HAnimationInfoComponent : MonoBehaviour
{
  public HScene.AnimationListInfo info;
  private Toggle toggle;
  private ScrollCylinderNode node;

  public HAnimationInfoComponent()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.toggle = (Toggle) ((Component) this).get_gameObject().GetComponent<Toggle>();
    this.node = (ScrollCylinderNode) ((Component) this).get_gameObject().GetComponent<ScrollCylinderNode>();
  }

  private void Update()
  {
    if (Object.op_Inequality((Object) this.toggle, (Object) null) && Vector3.op_Inequality(((Component) this.node.BG).get_transform().get_localScale(), new Vector3(1f, 1f, 1f)))
      ((Selectable) this.toggle).set_interactable(false);
    else
      ((Selectable) this.toggle).set_interactable(true);
  }
}
