using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationController : MonoBehaviour {
	public Animator Animator;
	private string currentAnimation;
	private int currentAnimationPriority;

	/// <summary>
	/// Change the animation
	/// </summary>
	/// <param name="animation">An enum which has the same name as the animation that you want to play</param>
	/// <param name="priority">Priority of the animation</param>
	/// <param name="crossfadeDuration">Crossfade duration between the current animation and the new one</param>
	/// <param name="animationSpeed">Animation speed multiplier (reverse with -1)</param>
	public void ChangeAnimation(Enum animation, int priority = 0, float crossfadeDuration = 0.2f, float animationSpeed = 1f) {
		ChangeAnimation(animation.ToString(), priority, crossfadeDuration, animationSpeed);
	}
	public void ChangeAnimation(string animation, int priority = 0, float crossfadeDuration = 0.2f, float animationSpeed = 1f)
	{
		if (currentAnimation == animation) return;
		
		if (!IsAnimationPlaying() || priority >= currentAnimationPriority)
		{
			if (AnimationExists(animation.ToString())) {
				currentAnimation = animation;
				currentAnimationPriority = priority;
				Animator.CrossFade(animation, crossfadeDuration);
				Animator.speed = animationSpeed;
			} else {
				Debug.LogWarning($"Animation '{animation}' does not exist in the Animator on {gameObject.name}.");
			}
		}
	}
	
	/// <summary>
	/// Forcefully changes the animation, bypassing the priority check.
	/// </summary>
	public void ForceAnimation(Enum animation, int priority = 0, float crossfadeDuration = 0.2f, float animationSpeed = 1f)
	{
		ForceAnimation(animation.ToString(), priority, crossfadeDuration, animationSpeed);
	}
	public void ForceAnimation(string animation, int priority = 0, float crossfadeDuration = 0.2f, float animationSpeed = 1f)
	{
		if (AnimationExists(animation))
		{
			currentAnimation = animation;
			currentAnimationPriority = priority;
			Animator.CrossFade(animation, crossfadeDuration);
			Animator.speed = animationSpeed;
		}
		else
		{
			Debug.LogWarning($"Animation '{animation}' does not exist in the Animator on {gameObject.name}.");
		}
	}
	
	private bool AnimationExists(string animationName)
	{
		int hash = Animator.StringToHash(animationName);
		for (int i = 0; i < Animator.layerCount; i++)
		{
			if (Animator.HasState(i, hash)) return true;
		}
		return false;
	}

	public bool IsAnimationPlaying() 
	{
		AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
		return stateInfo.normalizedTime < 1.0f; // && !stateInfo.loop;
	}
}