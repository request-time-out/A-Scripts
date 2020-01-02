// Decompiled with JetBrains decompiler
// Type: ChangeHItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class ChangeHItem : MonoBehaviour
{
  [Tooltip("体位アイテムと入れ替わって表示を消すオブジェクト")]
  public GameObject VisibleObj;

  public ChangeHItem()
  {
    base.\u002Ector();
  }

  public void ChangeActive(bool val)
  {
    if (Object.op_Equality((Object) this.VisibleObj, (Object) null) || this.VisibleObj.get_activeSelf() == val)
      return;
    this.VisibleObj.SetActive(val);
  }
}
