using Godot;
using System;

public partial class SlimeDamage : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void OnBodyEntered(Node2D body)
	{
		if (body is CharacterBody2D)
		{
			if (body is Player)
			{
				Player pc = body as Player;
				pc.TakeDamage();
			}
		}
	}
}
