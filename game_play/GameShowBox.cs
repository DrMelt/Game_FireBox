using Godot;
using System;
using GameKernel;

[Tool]
public partial class GameShowBox : Node3D
{
	[ExportGroup("Show")]
	[Export]
	Node3D normalBox;
	[Export]
	Node3D fireBox;

	[ExportGroup("Edit")]
	[Export]
	public BoxState.EBoxType eBoxType = BoxState.EBoxType.Normal;


	[Export]
	public bool canBePush = true;
	[Export]
	public bool canBeFire = true;
	[Export]
	public bool isFire = false;
	public Vector2I mapPos;

	public Vector2I Position2I { get { return new Vector2I((int)Position.X, (int)Position.Z); } set { Position = new Vector3(value.X, 0, value.Y); } }

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			UpdateStateShow();
		}
	}



	internal BoxState GetState()
	{
		BoxState boxState = new BoxState()
		{
			mapPos = mapPos,
			eBoxType = eBoxType,
			canBePush = canBePush,
			canBeFire = canBeFire,
			isFire = isFire,
		};

		return boxState;
	}

	internal void UpdateState(BoxState boxState)
	{
		Position2I = boxState.mapPos;
		mapPos = boxState.mapPos;
		eBoxType = boxState.eBoxType;
		canBePush = boxState.canBePush;
		canBeFire = boxState.canBeFire;
		isFire = boxState.isFire;

		UpdateStateShow();
	}

	void UpdateStateShow(){
		normalBox.Visible = !isFire;
		fireBox.Visible = isFire;
	}

}
