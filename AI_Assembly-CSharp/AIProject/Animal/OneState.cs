// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.OneState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace AIProject.Animal
{
  public struct OneState
  {
    public AnimalState state;
    public float proportion;

    public OneState(AnimalState _state, float _proportion)
    {
      this.state = _state;
      this.proportion = _proportion;
    }

    public bool Equal(OneState _state)
    {
      return this.state == _state.state && (double) this.proportion == (double) _state.proportion;
    }
  }
}
