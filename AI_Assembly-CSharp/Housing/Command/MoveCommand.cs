// Decompiled with JetBrains decompiler
// Type: Housing.Command.MoveCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Housing.Command
{
  public class MoveCommand : Housing.ICommand
  {
    private MoveCommand.Info[] infos;

    public MoveCommand(ObjectCtrl _objectCtrl, Vector3 _old)
    {
      this.infos = new MoveCommand.Info[1]
      {
        new MoveCommand.Info(_objectCtrl, _old)
      };
    }

    public MoveCommand(MoveCommand.Info[] _infos)
    {
      this.infos = _infos;
    }

    public void Do()
    {
      if (((IList<MoveCommand.Info>) this.infos).IsNullOrEmpty<MoveCommand.Info>())
        return;
      foreach (MoveCommand.Info info in this.infos)
        info.Do();
      foreach (MoveCommand.Info info in this.infos)
        Singleton<Manager.Housing>.Instance.CheckOverlap((ObjectCtrl) (info.ObjectCtrl as OCItem));
      Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.RefreshList();
    }

    public void Redo()
    {
      this.Do();
    }

    public void Undo()
    {
      if (((IList<MoveCommand.Info>) this.infos).IsNullOrEmpty<MoveCommand.Info>())
        return;
      foreach (MoveCommand.Info info in this.infos)
        info.Undo();
      foreach (MoveCommand.Info info in this.infos)
        Singleton<Manager.Housing>.Instance.CheckOverlap((ObjectCtrl) (info.ObjectCtrl as OCItem));
      Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.RefreshList();
    }

    public class Info
    {
      private Vector3 newPos = Vector3.get_zero();
      private Vector3 oldPos = Vector3.get_zero();
      private ObjectCtrl objectCtrl;

      public Info(ObjectCtrl _objectCtrl, Vector3 _old)
      {
        this.objectCtrl = _objectCtrl;
        this.newPos = this.objectCtrl.Position;
        this.oldPos = _old;
      }

      public ObjectCtrl ObjectCtrl
      {
        get
        {
          return this.objectCtrl;
        }
      }

      public void Do()
      {
        this.objectCtrl.Position = this.newPos;
      }

      public void Undo()
      {
        this.objectCtrl.Position = this.oldPos;
      }
    }
  }
}
