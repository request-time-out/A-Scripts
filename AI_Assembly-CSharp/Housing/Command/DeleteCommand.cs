// Decompiled with JetBrains decompiler
// Type: Housing.Command.DeleteCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Housing.Command
{
  public class DeleteCommand : Housing.ICommand
  {
    private DeleteCommand.Info[] infos;

    public DeleteCommand(ObjectCtrl[] _objectCtrls)
    {
      this.infos = ((IEnumerable<ObjectCtrl>) _objectCtrls).Select<ObjectCtrl, DeleteCommand.Info>((Func<ObjectCtrl, DeleteCommand.Info>) (v => new DeleteCommand.Info(v))).ToArray<DeleteCommand.Info>();
    }

    public void Do()
    {
      foreach (DeleteCommand.Info info in this.infos)
        info.ObjectCtrl.OnDelete();
      Singleton<CraftScene>.Instance.UICtrl.AddUICtrl.Reselect();
    }

    public void Redo()
    {
      this.Do();
      Singleton<CraftScene>.Instance.UICtrl.AddUICtrl.Reselect();
      Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.UpdateUI();
    }

    public void Undo()
    {
      foreach (DeleteCommand.Info info in this.infos)
        Singleton<Manager.Housing>.Instance.RestoreObject(info.ObjectCtrl, info.Parent, info.Index, true);
      Singleton<CraftScene>.Instance.UICtrl.AddUICtrl.Reselect();
      Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.UpdateUI();
    }

    private class Info
    {
      public Info(ObjectCtrl _objectCtrl)
      {
        this.ObjectCtrl = _objectCtrl;
        this.Parent = this.ObjectCtrl.Parent;
        this.Index = this.ObjectCtrl.InfoIndex;
      }

      public ObjectCtrl ObjectCtrl { get; private set; }

      public ObjectCtrl Parent { get; private set; }

      public int Index { get; private set; }
    }
  }
}
