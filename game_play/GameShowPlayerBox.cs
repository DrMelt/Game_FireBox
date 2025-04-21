using GameKernel;
using Godot;
using System;

public partial class GameShowPlayerBox : Node3D
{

    public Vector2I Position2I { get { return new Vector2I((int)Position.X, (int)Position.Z); } set { Position = new Vector3(value.X, 0, value.Y); } }


    public Vector2I mapPos;

    internal void UpdateState(FireBoxGame fireBoxGame)
    {
        Position2I = fireBoxGame.GameData.playerBoxState.MapPos;
    }
}
