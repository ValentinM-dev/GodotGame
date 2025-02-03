using System.Numerics;
using Godot;
using Vector2 = Godot.Vector2;

public partial class SlimeEnemy : CharacterBody2D
{
    [Export] private AnimatedSprite2D _SlimeEnemy;
    [Export] private RayCast2D _RayCastEnemy;
    [Export] public float Speed = 3000.0f;
    [Export] public float JumpHeight = 250.0f;
    private int _direction = 1;


    public override void _Ready()
    {
        GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("Walk");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        //Gravity
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        if (Input.IsActionPressed("ui_jump") && IsOnFloor())
        {
            velocity.Y = JumpHeight;
        }
        
        velocity.X = Speed * (float)delta;

        Velocity = velocity;
        SetDirection();
        MoveAndSlide();
    }
    
    public void SetDirection()
    {
        if(IsInstanceValid(GetNode<RayCast2D>("RayCast2D").GetCollider()))
        {
            if(GetNode<RayCast2D>("RayCast2D").IsColliding() && GetNode<RayCast2D>("RayCast2D").GetCollider() is Node collider && collider.IsInGroup("Maps"))
            {
                if(_direction == 1)
                {
                    _direction = 1;
                    GetNode<RayCast2D>("RayCast2D").TargetPosition = new Vector2(5, 10);
                    GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = true;
                }
                else
                {
                    _direction = -1;
                    GetNode<RayCast2D>("RayCast2D").TargetPosition = new Vector2(-5, 10);
                    GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = false;
                }
            }
        }
    }
}