using UnityEngine;
using System.Collections;

// The main class that handles the position tracking and reset logic.
public class FullObjectResetter : MonoBehaviour
{
    // --- Private Fields to Store Start State ---
    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private Rigidbody rb;

    // --- MONOBEHAVIOUR (Initialization) ---
    void Awake()
    {
        // 1. Store the object's initial state (its state "at Start")
        startingPosition = transform.position;
        startingRotation = transform.rotation;

        // 2. Cache components for the reset
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// PUBLIC METHOD: Executes the full reset of the GameObject's state.
    /// This is what's called by the Animator hook.
    /// </summary>
    public void ResetGameObjectState(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 1. Reset Transform (Position and Rotation)
        transform.position = startingPosition;
        transform.rotation = startingRotation;

        // 2. Reset Rigidbody Physics State
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            // Ensure the Rigidbody is active if it was sleeping
            rb.WakeUp();
        }

        // 3. Restart Animation Cycle
        // Uses the stateInfo hash to restart the exact state it just exited from frame 0.
        animator.Play(stateInfo.fullPathHash, layerIndex, 0f);

        Debug.Log($"Object {gameObject.name} fully reset and animation cycle restarted.");
    }

    // ==========================================================
    // !!! NESTED STATE MACHINE BEHAVIOUR CLASS !!!
    // ==========================================================
    // This class hooks into the Animator event when the animation finishes.

    public class AnimatorResetHook : StateMachineBehaviour
    {
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Get the main reset component on the same object as the Animator
            FullObjectResetter resetter = animator.GetComponent<FullObjectResetter>();

            if (resetter != null)
            {
                // Call the main reset method
                resetter.ResetGameObjectState(animator, stateInfo, layerIndex);
            }
        }
    }
}