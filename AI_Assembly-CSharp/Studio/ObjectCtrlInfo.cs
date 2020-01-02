// Decompiled with JetBrains decompiler
// Type: Studio.ObjectCtrlInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Studio
{
  public abstract class ObjectCtrlInfo
  {
    public ObjectInfo objectInfo;
    public TreeNodeObject treeNodeObject;
    public GuideObject guideObject;
    public ObjectCtrlInfo parentInfo;

    public abstract void OnDelete();

    public abstract void OnAttach(TreeNodeObject _parent, ObjectCtrlInfo _child);

    public abstract void OnDetach();

    public abstract void OnDetachChild(ObjectCtrlInfo _child);

    public abstract void OnSelect(bool _select);

    public abstract void OnLoadAttach(TreeNodeObject _parent, ObjectCtrlInfo _child);

    public abstract void OnVisible(bool _visible);

    public virtual void OnSavePreprocessing()
    {
      if (this.objectInfo != null && Object.op_Inequality((Object) this.treeNodeObject, (Object) null))
        this.objectInfo.treeState = this.treeNodeObject.treeState;
      if (this.objectInfo == null || !Object.op_Inequality((Object) this.treeNodeObject, (Object) null))
        return;
      this.objectInfo.visible = this.treeNodeObject.visible;
    }

    public int kind
    {
      get
      {
        return this.objectInfo != null ? this.objectInfo.kind : -1;
      }
    }

    public int[] kinds
    {
      get
      {
        if (this.objectInfo != null)
          return this.objectInfo.kinds;
        return new int[1]{ -1 };
      }
    }

    public virtual float animeSpeed { get; set; }

    public virtual ObjectCtrlInfo this[int _idx]
    {
      get
      {
        return _idx == 0 ? this : this.parentInfo;
      }
    }
  }
}
