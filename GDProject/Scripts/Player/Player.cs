using Godot;
using System;

public partial class Player : CharacterBody3D
{

	[ExportCategory("Player Controller")]
	[Export(PropertyHint.Range, "0, 5, ")]
	private float _mouseSensitivity;
	[Export]
	private float _walkSpeed = 5.0f;
	[Export]
	private float _runSpeed = 10.0f;

	private float _moveSpeed = 0;

	[Export]
	public float JumpVelocity = 4.5f;

	[ExportCategory("Camera")]
	[Export]
	private Node3D _cameraPivot;
	[Export]
	private Camera3D _camera;

	[Export]
	public RayCast3D CameraRaycast;

	[Export]
	private Vector3 _cameraOffset;

	[Export]
	private float _defaultCameraOffsetMultiplier;

	[Export]
	private float _maxCameraOffsetMultiplier;

	[Export]
	private float _minCameraOffsetMultiplier;

	[Export]
	private float _cameraOffsetIncrement;

	private float _cameraOffsetMultiplier;

	

	[ExportCategory("Animation")]
	[Export]
	private AnimationTree _animationTree;

	private Vector2 _animationMovementDir;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _Ready()
	{
		base._Ready();
		Input.MouseMode = Input.MouseModeEnum.Captured;
		_cameraOffsetMultiplier = Mathf.Clamp(_defaultCameraOffsetMultiplier,
											 _minCameraOffsetMultiplier,
											  _maxCameraOffsetMultiplier);
		_camera.Position = _cameraOffset * _cameraOffsetMultiplier;
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if(Input.IsActionPressed("Dab"))
		{
			if(!(bool) _animationTree.Get("parameters/Dab_Controller/active"))
			{
				_animationTree.Set("parameters/Dab_Controller/request", (int) AnimationNodeOneShot.OneShotRequest.Fire);
			}
		}

		if(Input.IsActionPressed("ToggleFullscreen"))
		{
			if (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen)
			{
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
			}
			else
			{
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
			}
			
		}

		if (@event is InputEventMouseMotion)
		{
			var @eventMouseMotion = (InputEventMouseMotion) @event;

			if(!Input.IsActionPressed("FreeLook"))
			{
				this.RotateY(-1 * _mouseSensitivity * Mathf.DegToRad(@eventMouseMotion.Relative.X));
				_cameraPivot.Rotate(Vector3.Right, _mouseSensitivity * Mathf.DegToRad(@eventMouseMotion.Relative.Y));
			} else {
				_cameraPivot.Rotate(Vector3.Up, -1 * _mouseSensitivity * Mathf.DegToRad(@eventMouseMotion.Relative.X));
			}
		}

		if(Input.IsActionPressed("CameraZoomIn"))
		{
			_cameraOffsetMultiplier = Mathf.Clamp(_cameraOffsetMultiplier - _cameraOffsetIncrement,
												 _minCameraOffsetMultiplier,
												 _maxCameraOffsetMultiplier);
		}

		if(Input.IsActionPressed("CameraZoomOut"))
		{
			_cameraOffsetMultiplier = Mathf.Clamp(_cameraOffsetMultiplier + _cameraOffsetIncrement,
												 _minCameraOffsetMultiplier,
												  _maxCameraOffsetMultiplier);
		}

		_camera.Position = _cameraOffset * _cameraOffsetMultiplier;


   
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if(!Input.IsActionPressed("FreeLook")){
			_cameraPivot.Rotation = new Vector3(Mathf.Clamp(_cameraPivot.Rotation.X, -MathF.PI/2, MathF.PI/2), 0, 0);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		if (Input.IsActionPressed("Run"))
		{
			_moveSpeed = _runSpeed;
			_animationTree.Set("parameters/MoveSpeed/scale", 1.75);
		}
		else
		{
			_moveSpeed = _walkSpeed;
			_animationTree.Set("parameters/MoveSpeed/scale", 1);
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("StrafeLeft", "StrafeRight", "StrafeBackward", "StrafeForward");
		Vector3 direction = (Transform.Basis * new Vector3(-1*inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{

			velocity.X = direction.X * _moveSpeed; //Mathf.MoveToward(Velocity.X, direction.X * StrafeSpeed, StrafeSpeed);
			velocity.Z = direction.Z * _moveSpeed; //Mathf.MoveToward(Velocity.Z, direction.Z * StrafeSpeed, StrafeSpeed);
			_animationMovementDir.X = Mathf.Lerp(_animationMovementDir.X, inputDir.X, (float) delta * _moveSpeed);
			_animationMovementDir.Y = Mathf.Lerp(_animationMovementDir.Y, inputDir.Y, (float) delta * _moveSpeed);

			_animationTree.Set("parameters/StateMachine/transition_request", "Moving");
			_animationTree.Set("parameters/Moving/blend_position", _animationMovementDir);
			
		}
		else
		{
			_animationTree.Set("parameters/StateMachine/transition_request", "Idle");

			velocity.X = Mathf.MoveToward(Velocity.X, 0, _moveSpeed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, _moveSpeed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
