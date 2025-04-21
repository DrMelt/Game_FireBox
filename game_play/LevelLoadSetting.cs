using Godot;
using System;

public enum ELevelLoad
{
	None,
	Level1,
	Level2,
	Level3,
	Level4,
	Level5,
}

public partial class LevelLoadSetting : Node
{
	[Export]
	ELevelLoad eLevel = ELevelLoad.None;
	public ELevelLoad ELevel => eLevel;
	[Export]
	PackedScene levelScene = null;
	public PackedScene LevelScene => levelScene;
}
