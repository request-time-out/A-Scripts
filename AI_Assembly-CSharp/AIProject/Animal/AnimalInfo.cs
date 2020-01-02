// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Animal.Resources;

namespace AIProject.Animal
{
  public struct AnimalInfo
  {
    public AnimalTypes AnimalType;
    public BreedingTypes BreedingType;
    public string Name;
    public string IdentifierName;
    public int AnimalID;
    public AnimalModelInfo ModelInfo;

    public AnimalInfo(
      AnimalTypes _animalType,
      BreedingTypes _breedingType,
      string _name,
      string _identifierName,
      int _animalID,
      int _chunkID,
      AnimalModelInfo _modelInfo)
    {
      this.AnimalType = _animalType;
      this.BreedingType = _breedingType;
      this.Name = _name;
      this.IdentifierName = _identifierName;
      this.AnimalID = _animalID;
      this.ModelInfo = _modelInfo;
    }
  }
}
