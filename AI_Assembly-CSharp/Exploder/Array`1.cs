// Decompiled with JetBrains decompiler
// Type: Exploder.Array`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace Exploder
{
  public class Array<T>
  {
    private T[] array;
    private int size;
    private int index;

    public Array(int size)
    {
      this.array = new T[size];
      this.size = size;
      this.index = 0;
    }

    public void Initialize(int newSize)
    {
      if (newSize > this.size)
      {
        this.array = new T[newSize];
        this.size = newSize;
      }
      this.Clear();
    }

    public int Count
    {
      get
      {
        return this.index;
      }
    }

    public T this[int key]
    {
      get
      {
        return this.array[key];
      }
    }

    public void Clear()
    {
      for (int index = 0; index < this.index; ++index)
        this.array[index] = default (T);
      this.index = 0;
    }

    public void Add(T data)
    {
      this.array[this.index++] = data;
      if (this.index < this.size)
        return;
      T[] objArray = new T[this.size * 2];
      for (int index = 0; index < this.size; ++index)
        objArray[index] = this.array[index];
      this.array = objArray;
    }

    public void Reverse()
    {
      for (int index = 0; index < this.index / 2; ++index)
      {
        T obj = this.array[index];
        this.array[index] = this.array[this.index - index - 1];
        this.array[this.index - index - 1] = obj;
      }
    }
  }
}
