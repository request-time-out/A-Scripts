// Decompiled with JetBrains decompiler
// Type: SceneAssist.ContentSizeChange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace SceneAssist
{
  public class ContentSizeChange : MonoBehaviour
  {
    [SerializeField]
    private RectTransform target;
    [SerializeField]
    private ContentSizeChange.SizeInfo width;
    [SerializeField]
    private ContentSizeChange.SizeInfo height;

    public ContentSizeChange()
    {
      base.\u002Ector();
    }

    private void OnEnable()
    {
      if (Object.op_Equality((Object) this.target, (Object) null) || !this.width.use && !this.height.use)
        return;
      Vector2 sizeDelta = this.target.get_sizeDelta();
      if (this.width.use)
        sizeDelta.x = (__Null) (double) this.width.enableSize;
      if (this.height.use)
        sizeDelta.y = (__Null) (double) this.height.enableSize;
      this.target.set_sizeDelta(sizeDelta);
    }

    private void OnDisable()
    {
      if (Object.op_Equality((Object) this.target, (Object) null) || !this.width.use && !this.height.use)
        return;
      Vector2 sizeDelta = this.target.get_sizeDelta();
      if (this.width.use)
        sizeDelta.x = (__Null) (double) this.width.disableSize;
      if (this.height.use)
        sizeDelta.y = (__Null) (double) this.height.disableSize;
      this.target.set_sizeDelta(sizeDelta);
    }

    [Serializable]
    public class SizeInfo
    {
      public bool use = true;
      public float enableSize;
      public float disableSize;
    }
  }
}
