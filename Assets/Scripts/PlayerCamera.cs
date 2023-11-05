using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField, Tag]
    private string playerHeadTag;

    [SerializeField]
    private ParentConstraint parentConstraint;

    private void Reset()
    {
        parentConstraint = GetComponent<ParentConstraint>();
    }

    private void Start()
    {
        var playerHead = GameObject.FindGameObjectWithTag(playerHeadTag).transform;
        var constraintSource = new ConstraintSource()
        {
            sourceTransform = playerHead,
            weight = 1,
        };
        parentConstraint.AddSource(constraintSource);
        parentConstraint.constraintActive = true;
    }
}
