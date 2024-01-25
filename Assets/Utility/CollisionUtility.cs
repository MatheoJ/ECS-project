using UnityEngine;

/*
   DO NOT CHANGE, THIS WILL BE REPLACED AT CORRECTION
   NE PAS CHANGER, CE FICHIER VA ÊTRE REMPLACÉ A LA CORRECTION
*/

public static class CollisionUtility
{
    /// <summary>
    /// Calculates the resulting position and velocity of two circles colliding with each others.
    /// Based on Keith Peter's Solution in Foundation Actionscript Animation: Making Things Move!
    /// </summary>
    /// <param name="position1">Position of the first circle</param>
    /// <param name="velocity1">Velocity of the first circle</param>
    /// <param name="size1">Size of the first circle. Also its diameter</param>
    /// <param name="position2">Position of the second circle</param>
    /// <param name="velocity2">Velocity of the second circle</param>
    /// <param name="size2">Size of the second circle. Also its diameter</param>
    /// <returns>The collision result of the collisions. Will be null if circles are not touching each other.</returns>
    public static CollisionResult CalculateCollision(Vector2 position1, Vector2 velocity1, float size1, Vector2 position2,
        Vector2 velocity2, float size2)
    {
        var initialPosition1 = position1;
        var initialPosition2 = position2;
        var radius1 = size1 / 2;
        var radius2 = size2 / 2;
        var mass1 = radius1 * 10;
        var mass2 = radius2 * 10;
        
        // Get Magnitudes of initial velocities
        var magnitude1 = velocity1.magnitude;
        var magnitude2 = velocity2.magnitude;
        var oneIsStatic = magnitude1 == 0 || magnitude2 == 0;

        // Get distances between the balls components
        var distanceVector = position2 - position1;

        // Calculate magnitude of the vector separating the balls
        var distanceMagnitude = distanceVector.magnitude;

        // Minimum distance before they are touching
        var minDistance = radius1 + radius2;

        if (distanceMagnitude > minDistance)
            return null;

        var distanceCorrection = (minDistance - distanceMagnitude);
        
        if(!oneIsStatic)
        {
            distanceCorrection /= 2;
        }
            
        var correctionVector = distanceVector.normalized * distanceCorrection;
        position2 += correctionVector;
        position1 -= correctionVector;
        
		// Handling for one static circle
        if (magnitude1 == 0) {
            position1 = initialPosition1;
            distanceVector.Normalize();
            velocity2 -= (2 * Vector2.Dot(distanceVector, velocity2)) * distanceVector;
        }
        else if (magnitude2 == 0)
        {
            position2 = initialPosition2;
            distanceVector = -distanceVector;
            distanceVector.Normalize();
            velocity1 -= (2 * Vector2.Dot(distanceVector, velocity1)) * distanceVector;
        }
        else
        {
            // get angle of distanceVect
            var theta = Mathf.Atan2(distanceVector.y, distanceVector.x);
        
            // precalculate trig values
            var sine = Mathf.Sin(theta);
            var cosine = Mathf.Cos(theta);
        
            Vector2[] bTemp = { new(), new() };
        
            bTemp[1].x = cosine * distanceVector.x + sine * distanceVector.y;
            bTemp[1].y = cosine * distanceVector.y - sine * distanceVector.x;

            // rotate Temporary velocities
            Vector2[] vTemp = { new(), new() };

            vTemp[0].x = cosine * velocity1.x + sine * velocity1.y;
            vTemp[0].y = cosine * velocity1.y - sine * velocity1.x;
            vTemp[1].x = cosine * velocity2.x + sine * velocity2.y;
            vTemp[1].y = cosine * velocity2.y - sine * velocity2.x;
        
            Vector2[] vFinal = { new(), new() };

            // final rotated velocity for b[0]
            vFinal[0].x = ((mass1 - mass2) * vTemp[0].x + 2 * mass2 * vTemp[1].x) / (mass1 + mass2);
            vFinal[0].y = vTemp[0].y;

            // final rotated velocity for b[0]
            vFinal[1].x = ((mass2 - mass1) * vTemp[1].x + 2 * mass1 * vTemp[0].x) / (mass1 + mass2);
            vFinal[1].y = vTemp[1].y;

            // hack to avoid clumping
            bTemp[0].x += vFinal[0].x;
            bTemp[1].x += vFinal[1].x;
        
            // rotate balls
            Vector2[] bFinal = { new(), new() };

            bFinal[0].x = cosine * bTemp[0].x - sine * bTemp[0].y;
            bFinal[0].y = cosine * bTemp[0].y + sine * bTemp[0].x;
            bFinal[1].x = cosine * bTemp[1].x - sine * bTemp[1].y;
            bFinal[1].y = cosine * bTemp[1].y + sine * bTemp[1].x;

            // update velocities
            velocity1.x = cosine * vFinal[0].x - sine * vFinal[0].y;
            velocity1.y = cosine * vFinal[0].y + sine * vFinal[0].x;
            velocity2.x = cosine * vFinal[1].x - sine * vFinal[1].y;
            velocity2.y = cosine * vFinal[1].y + sine * vFinal[1].x;
        }

        return new CollisionResult(position1, velocity1, position2, velocity2);
    }
}