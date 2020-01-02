// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.IPetAnimal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject.Animal
{
  public interface IPetAnimal
  {
    AnimalTypes AnimalType { get; }

    int AnimalTypeID { get; }

    string Name { get; }

    string Nickname { get; set; }

    int AnimalID { get; }

    Vector3 Position { get; set; }

    Quaternion Rotation { get; set; }

    AIProject.SaveData.AnimalData AnimalData { get; set; }

    PetHomePoint HomePoint { get; }

    void Initialize(PetHomePoint _homePoint);

    void Initialize(AIProject.SaveData.AnimalData animalData);

    AnimalInfo GetAnimalInfo();

    void Release();
  }
}
