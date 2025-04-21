using GameKernel;
using Godot;
using System;

public partial class LevelPlayerBox : Node3D
{
	public Vector2I Position2I { get { return new Vector2I((int)Position.X, (int)Position.Z); } }
	internal PlayerBoxState GetBoxPlayerState(Vector2I minCorner)
	{
        PlayerBoxState playerBoxState = new PlayerBoxState(Position2I - minCorner);

		return playerBoxState;
	}

}
