using Godot;
using System;

public class PlayerDodgeState : State
{

    public PlayerDodgeState(StateMachine stateMaschine) : base(stateMaschine) { }

    public override void Update(float delta)
    {

        Player player = (Player)_stateMachine.Parent;
        int direction = player.GetInputDirection();

        if (player.DodgeTimer.IsStopped())
        {
            if (direction != 0 && player.IsOnFloor() && !player.CeilingRaycast.IsColliding())
            {

                _stateMachine.Change("walk");
                player.WasOnGround = true;
            }
            else if (player.IsOnFloor() && !player.CeilingRaycast.IsColliding())
            {
                _stateMachine.Change("idle");
                player.WasOnGround = true;
            }
            else if (!player.IsOnFloor() && !player.CeilingRaycast.IsColliding())
            {
                _stateMachine.Change("fall");
                player.WasOnGround = false;
            } else {
                _stateMachine.Change("crouch");
                player.WasOnGround = true;
            }
        }
        player.Velocity.x = Mathf.Lerp(player.Velocity.x, 0 , 0.05f);
    }


    public override void Enter(params object[] args)
    {
        Player player = (Player)_stateMachine.Parent;
        player.Velocity.x = player.DodgeSpeed * player.GetInputDirection();
        player.AnimatedSprite.Position = new Vector2(player.AnimatedSprite.Position.x, player.AnimatedSprite.Position.y + 5);
        player.ToggleCrouchCollisionShape();
        player.AnimatedSprite.Play("somersault");
        player.DodgeTimer.Start();

    }
    public override void Exit()
    {
        Player player = (Player)_stateMachine.Parent;
        player.DodgeCoolDownTimer.Start();
        player.ToggleCrouchCollisionShape();
        player.AnimatedSprite.Position = new Vector2(player.AnimatedSprite.Position.x, player.AnimatedSprite.Position.y - 5);
    }
}
