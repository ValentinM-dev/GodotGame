using Godot;
using System;

public partial class Player : CharacterBody2D
{
    public const float Speed = 100.0f;
    public const float JumpHeight = 250.0f;

    // add a non-linear acceleration/slow down
    private float friction = 200.0f; // slowing down
    private float acceleration = 400.0f; // speeding up

    // double jump
    private int jumpCount = 0; // number of jumps
    private const int maxJumps = 1; // limit max jump to 2, create a double jump system

    // add dash
    private int dashSpeed = 400;
    private bool isDashAvailable = true;

    // add climb
    private bool canClimb;
    private int climbSpeed = 100;

    // Config Animation
    private bool isInAir = false;

    // Add Effect
    [Export] public PackedScene GhostPlayerInstance;

    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        float inputDirection = Input.GetAxis("ui_left", "ui_right");

        if (inputDirection != 0)
        {
            if (isInAir == false)
            {
                GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("Run");
            }

            velocity.X =
                Mathf.MoveToward(velocity.X, inputDirection * Speed,
                    acceleration * (float)delta); // adjust x velocity with acceleration toward max speed
        }
        else
        {
            velocity.X =
                Mathf.MoveToward(velocity.X, 0,
                    friction * (float)delta); // adjust x velocity with friction toward 0

            if (isInAir == false)
            {
                GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("Idle");
            }
        }

        if (Input.IsActionPressed("ui_left"))
        {
            GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = true;
        }
        else
        {
            GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = false;
        }

        if (Input.IsActionJustPressed("ui_jump"))
        {
            GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("Jump");
            isInAir = true;
            if (jumpCount < maxJumps)
            {
                velocity.Y = -JumpHeight;
                jumpCount++;
            }
        }

        if (IsOnFloor())
        {
            jumpCount = 0;
            isDashAvailable = true;
            canClimb = true;
        }

        if (Input.IsActionJustPressed("ui_jump") && GetNode<RayCast2D>("RayCastLeft").IsColliding())
        {
            velocity.Y = -JumpHeight;
            velocity.X = JumpHeight;
        }

        else if (Input.IsActionJustPressed("ui_jump") && GetNode<RayCast2D>("RayCastRight").IsColliding())
        {
            velocity.Y = -JumpHeight;
            velocity.X = -JumpHeight;
        }

        if (Input.IsActionPressed("ui_climb") && (GetNode<RayCast2D>("RayCastLeftClimb").IsColliding() ||
                                                  GetNode<RayCast2D>("RayCastRightClimb").IsColliding()))
        {
            if (canClimb)
            {
                if (Input.IsActionPressed("ui_up"))
                {
                    velocity.Y = -climbSpeed;
                }
                else if (Input.IsActionPressed("ui_down"))
                {
                    velocity.Y = climbSpeed;
                }
                else
                {
                    velocity = new Vector2(0, 0);
                }
            }
        }

        if (isDashAvailable == true)
        {
            if (Input.IsActionJustPressed("dash"))
            {
                if (Input.IsActionPressed("ui_left"))
                {
                    velocity.X = -dashSpeed;
                }

                if (Input.IsActionPressed("ui_right"))
                {
                    velocity.X = dashSpeed;
                }

                if (Input.IsActionPressed("ui_up"))
                {
                    velocity.Y = -dashSpeed;
                }

                if (Input.IsActionPressed("ui_left") && Input.IsActionPressed("ui_up"))
                {
                    velocity.X = -dashSpeed;
                    velocity.Y = -dashSpeed;
                }

                if (Input.IsActionPressed("ui_right") && Input.IsActionPressed("ui_up"))
                {
                    velocity.X = dashSpeed;
                    velocity.Y = -dashSpeed;
                }
            }

            isDashAvailable = false;
        }


        Velocity = velocity;
        MoveAndSlide();

        //if (velocity.X > Speed + 40)
       // {
            //GhostDash();
       // }
    }

    private void GhostDash()
    {
        //GhostPlayer ghost = GhostPlayerInstance.Instance() as GhostPlayer;
        GhostPlayer ghost = (GhostPlayer)GhostPlayerInstance.Instantiate();
        Owner.AddChild(ghost);
        ghost.GlobalPosition = this.GlobalPosition;
    }
}