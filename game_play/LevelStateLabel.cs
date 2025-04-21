using Godot;
using System;

public partial class LevelStateLabel : Label
{
	[Export]
	GameLogic gameLogic = null;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Visible = gameLogic.FireBoxGameRef.GameData.fireBoxGameState.isLevelCleared;
	}
}
