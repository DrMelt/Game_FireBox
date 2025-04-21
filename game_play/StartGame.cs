using Godot;
using System;

public partial class StartGame : Node
{
	[Export]
	GameLogic gameLogic = null;
	[Export]
	ELevelLoad loadLevel = ELevelLoad.Level1;

	public override void _Ready()
	{
		gameLogic.LoadLevel(loadLevel);
	}

}
