﻿using System.Collections;
using UnityEngine;
using XboxCtrlrInput;
public sealed class PlayerController : MonoBehaviour
{
	public Rigidbody2D playerRigidbody;

	public float moveAcceleration = 1.0f;
	public float maxHorizontalVelocity = 10.0f;

	public float frictionVelocity = 1.0f;

	public float groundCheckHeight = 1.0f;

	public float stunTime = 1.0f;
	public float stunImpulse = 10.0f;

	[System.NonSerialized]
	public bool isGrounded;
	[System.NonSerialized]
	public bool isDucking;
	[System.NonSerialized]
	public bool isStunned;
    

	private void Update()
	{
		isDucking = isGrounded && Input.GetAxisRaw( "Vertical" ) < -0.5f;
	}

	private void FixedUpdate()
	{
		int layerMask = LayerMask.GetMask( "Environment" );
		isGrounded = Physics2D.Raycast( transform.position + Vector3.up, Vector2.down, groundCheckHeight, layerMask ).collider != null;

		float horizontalInput = Input.GetAxis( "Horizontal" );
		if( !isDucking && !isStunned && Mathf.Abs( horizontalInput ) > Mathf.Epsilon )
		{
			if( Mathf.Sign( horizontalInput ) * playerRigidbody.velocity.x < maxHorizontalVelocity )
			{
				float move = horizontalInput * moveAcceleration;
				playerRigidbody.AddForce( new Vector2( move, 0.0f ), ForceMode2D.Force );
			}
		}
	}

	private void OnCollisionStay2D( Collision2D collision )
	{
		if( collision.gameObject.layer == LayerMask.NameToLayer( "Environment" ) )
		{
			float horizontalInput = Input.GetAxis( "Horizontal" );
			if( !isStunned && ( isDucking || Mathf.Abs( horizontalInput ) < Mathf.Epsilon ) )
			{
				Vector2 vel = playerRigidbody.velocity;
				vel.x *= frictionVelocity * Time.deltaTime;
				playerRigidbody.velocity = vel;
			}
		}
	}

	private void OnCollisionEnter2D( Collision2D collision )
	{
		if( collision.gameObject.layer == LayerMask.NameToLayer( "Enemy" ) )
		{
			if( !isStunned )
			{
				playerRigidbody.AddForce( collision.contacts[0].normal * stunImpulse, ForceMode2D.Impulse );
				StartCoroutine( StunCoroutine() );
                //vibrar
            }
		}
	}

	private IEnumerator StunCoroutine()
	{
		isStunned = true;
		yield return new WaitForSeconds( stunTime );
		isStunned = false;
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawRay( transform.position + Vector3.up, Vector2.down * groundCheckHeight );
	}
}
