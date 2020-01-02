// Decompiled with JetBrains decompiler
// Type: Studio.UndoRedoManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Studio
{
  public class UndoRedoManager : Singleton<UndoRedoManager>
  {
    private Stack<ICommand> undo = new Stack<ICommand>();
    private Stack<ICommand> redo = new Stack<ICommand>();
    private bool m_CanUndo;
    private bool m_CanRedo;

    public event EventHandler CanUndoChange;

    public event EventHandler CanRedoChange;

    public bool CanUndo
    {
      get
      {
        return this.m_CanUndo;
      }
      private set
      {
        if (this.m_CanUndo == value)
          return;
        this.m_CanUndo = value;
        if (this.CanUndoChange == null)
          return;
        this.CanUndoChange((object) this, EventArgs.Empty);
      }
    }

    public bool CanRedo
    {
      get
      {
        return this.m_CanRedo;
      }
      private set
      {
        if (this.m_CanRedo == value)
          return;
        this.m_CanRedo = value;
        if (this.CanRedoChange == null)
          return;
        this.CanRedoChange((object) this, EventArgs.Empty);
      }
    }

    public void Do(ICommand _command)
    {
      if (_command == null)
        return;
      this.undo.Push(_command);
      this.CanUndo = true;
      _command.Do();
      this.redo.Clear();
      this.CanRedo = false;
    }

    public void Do(
      Delegate _doMethod,
      object[] _doParamater,
      Delegate _undoMethod,
      object[] _undoParamater)
    {
      this.Do((ICommand) new UndoRedoManager.Command(_doMethod, _doParamater, _undoMethod, _undoParamater));
    }

    public void Do()
    {
      if (this.undo.Count <= 0)
        return;
      this.Do(this.undo.Peek());
    }

    public void Undo()
    {
      if (this.undo.Count <= 0)
        return;
      ICommand command = this.undo.Pop();
      this.CanUndo = this.undo.Count > 0;
      command.Undo();
      this.redo.Push(command);
      this.CanRedo = true;
    }

    public void Redo()
    {
      if (this.redo.Count <= 0)
        return;
      ICommand command = this.redo.Pop();
      this.CanRedo = this.redo.Count > 0;
      command.Redo();
      this.undo.Push(command);
      this.CanUndo = true;
    }

    public void Push(ICommand _command)
    {
      if (_command == null)
        return;
      this.undo.Push(_command);
      this.CanUndo = true;
      this.redo.Clear();
      this.CanRedo = false;
    }

    public void Clear()
    {
      this.undo.Clear();
      this.redo.Clear();
      this.CanUndo = false;
      this.CanRedo = false;
    }

    protected override void Awake()
    {
      if (!this.CheckInstance())
        return;
      Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
    }

    private class Command : ICommand
    {
      private Delegate doMethod;
      private Delegate undoMethod;
      private object[] doParamater;
      private object[] undoParamater;

      public Command(
        Delegate _doMethod,
        object[] _doParamater,
        Delegate _undoMethod,
        object[] _undoParamater)
      {
        this.doMethod = _doMethod;
        this.doParamater = _doParamater;
        this.undoMethod = _undoMethod;
        this.undoParamater = _undoParamater;
      }

      public void Do()
      {
        this.doMethod.DynamicInvoke(this.doParamater);
      }

      public void Undo()
      {
        this.undoMethod.DynamicInvoke(this.undoParamater);
      }

      public void Redo()
      {
        this.doMethod.DynamicInvoke(this.doParamater);
      }
    }
  }
}
