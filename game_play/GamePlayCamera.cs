using Godot;
using System;

public partial class GamePlayCamera : Camera3D
{
	[Export]
	GameLogic gameLogic = null;

	public override void _Process(double delta)
	{
		if (gameLogic.FireBoxGameRef == null)
		{
			return;
		}

        Vector2I mapSize = gameLogic.FireBoxGameRef.MapSize;
        Vector3 pos = new Vector3((mapSize.X - 1) * 0.5f, Position.Y, (mapSize.Y - 1) * 0.5f);
		Position = pos;

		Size = Math.Max(Math.Max(mapSize.Y, mapSize.X / 16.0f * 9.0f), 4);
	}
}
