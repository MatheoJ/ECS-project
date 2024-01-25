using UnityEngine;

/*
   DO NOT CHANGE, THIS WILL BE REPLACED AT CORRECTION
   NE PAS CHANGER, CE FICHIER VA ÊTRE REMPLACÉ A LA CORRECTION
*/

/// <summary>
/// Information about the result of a collision between two circles
/// </summary>
public class CollisionResult
{
    /// <summary>
    /// Resulting position of the first circle. Corrects position to make sure the circles are not inside each other.
    /// </summary>
    public readonly Vector2 position1;
    
    /// <summary>
    /// Resulting velocity of the first circle.
    /// </summary>
    public readonly Vector2 velocity1;
    
    /// <summary>
    /// Resulting position of the second circle. Corrects position to make sure the circles are not inside each other.
    /// </summary>
    public readonly Vector2 position2;
    
    /// <summary>
    /// Resulting velocity of the second circle.
    /// </summary>
    public readonly Vector2 velocity2;

    public CollisionResult(Vector2 position1, Vector2 velocity1, Vector2 position2, Vector2 velocity2)
    {
        this.position1 = position1;
        this.position2 = position2;
        this.velocity1 = velocity1;
        this.velocity2 = velocity2;
    }
}