// Decompiled with JetBrains decompiler
// Type: Studio.GuideSelect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

namespace Studio
{
  public class GuideSelect : GuideBase, IPointerClickHandler, IEventSystemHandler
  {
    public Color color
    {
      set
      {
        this.colorNormal = this.ConvertColor(value);
        this.colorHighlighted = new Color((float) value.r, (float) value.g, (float) value.b, 1f);
        this.colorNow = this.colorNormal;
      }
    }

    public TreeNodeObject treeNodeObject { get; set; }

    public void OnPointerClick(PointerEventData _eventData)
    {
      if (Object.op_Inequality((Object) this.treeNodeObject, (Object) null))
      {
        this.treeNodeObject.Select(false);
      }
      else
      {
        if (!Singleton<GuideObjectManager>.IsInstance())
          return;
        Singleton<GuideObjectManager>.Instance.selectObject = this.guideObject;
      }
    }

    private void Awake()
    {
      base.Start();
      this.treeNodeObject = (TreeNodeObject) null;
    }

    public override void Start()
    {
      this.colorNow = this.colorNormal;
    }
  }
}
