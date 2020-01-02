// Decompiled with JetBrains decompiler
// Type: Housing.Command.RotationCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Housing.Command
{
  public class RotationCommand : Housing.ICommand
  {
    private Vector3 newRot = Vector3.get_zero();
    private Vector3 oldRot = Vector3.get_zero();
    private ObjectCtrl objectCtrl;

    public RotationCommand(ObjectCtrl _objectCtrl, Vector3 _old)
    {
      this.objectCtrl = _objectCtrl;
      this.newRot = this.objectCtrl.LocalEulerAngles;
      this.oldRot = _old;
    }

    public void Do()
    {
      this.objectCtrl.LocalEulerAngles = this.newRot;
      Singleton<Manager.Housing>.Instance.CheckOverlap((ObjectCtrl) (this.objectCtrl as OCItem));
      Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.RefreshList();
    }

    public void Redo()
    {
      this.Do();
    }

    public void Undo()
    {
      this.objectCtrl.LocalEulerAngles = this.oldRot;
      Singleton<Manager.Housing>.Instance.CheckOverlap((ObjectCtrl) (this.objectCtrl as OCItem));
      Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.RefreshList();
    }
  }
}
