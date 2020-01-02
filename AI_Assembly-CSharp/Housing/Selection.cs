// Decompiled with JetBrains decompiler
// Type: Housing.Selection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace Housing
{
  public class Selection : Singleton<Selection>
  {
    public Action<ObjectCtrl[]> onSelectFunc;

    public ObjectCtrl SelectObject
    {
      get
      {
        return this.SelectObjects.SafeGet<ObjectCtrl>(0);
      }
    }

    public ObjectCtrl[] SelectObjects { get; private set; }

    public void SetSelectObjects(ObjectCtrl[] _objectCtrls)
    {
      this.SelectObjects = _objectCtrls;
      if (this.onSelectFunc == null)
        return;
      this.onSelectFunc(this.SelectObjects);
    }

    protected override void Awake()
    {
      if (this.CheckInstance())
        ;
    }
  }
}
