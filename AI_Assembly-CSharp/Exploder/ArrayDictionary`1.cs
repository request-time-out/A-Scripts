// Decompiled with JetBrains decompiler
// Type: Exploder.ArrayDictionary`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace Exploder
{
  internal class ArrayDictionary<T>
  {
    public int Count;
    public int Size;
    private readonly ArrayDictionary<T>.DicItem[] dictionary;

    public ArrayDictionary(int size)
    {
      this.dictionary = new ArrayDictionary<T>.DicItem[size];
      this.Size = size;
    }

    public bool ContainsKey(int key)
    {
      return key < this.Size && this.dictionary[key].valid;
    }

    public T this[int key]
    {
      get
      {
        return this.dictionary[key].data;
      }
      set
      {
        this.dictionary[key].data = value;
      }
    }

    public void Clear()
    {
      for (int index = 0; index < this.Size; ++index)
      {
        this.dictionary[index].data = default (T);
        this.dictionary[index].valid = false;
      }
      this.Count = 0;
    }

    public void Add(int key, T data)
    {
      this.dictionary[key].valid = true;
      this.dictionary[key].data = data;
      ++this.Count;
    }

    public void Remove(int key)
    {
      this.dictionary[key].valid = false;
      --this.Count;
    }

    public T[] ToArray()
    {
      T[] objArray = new T[this.Count];
      int num = 0;
      for (int index = 0; index < this.Size; ++index)
      {
        if (this.dictionary[index].valid)
        {
          objArray[num++] = this.dictionary[index].data;
          if (num == this.Count)
            return objArray;
        }
      }
      return (T[]) null;
    }

    public bool TryGetValue(int key, out T value)
    {
      ArrayDictionary<T>.DicItem dicItem = this.dictionary[key];
      if (dicItem.valid)
      {
        value = dicItem.data;
        return true;
      }
      value = default (T);
      return false;
    }

    public T GetFirstValue()
    {
      for (int index = 0; index < this.Size; ++index)
      {
        ArrayDictionary<T>.DicItem dicItem = this.dictionary[index];
        if (dicItem.valid)
          return dicItem.data;
      }
      return default (T);
    }

    private struct DicItem
    {
      public T data;
      public bool valid;
    }
  }
}
