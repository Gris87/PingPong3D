using UnityEngine;
using System.Collections;

/// <summary>
/// <see cref="ModifiableObject"/> is an object that have any kind of modifiable data.
/// </summary>
public class ModifiableObject
{
    /// <summary>
    /// Function delegate for modificationMade calls.
    /// </summary>
    public delegate void ModifiedFunction();

    private ModifiedFunction mModifiedFunction;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModifiableObject"/> class.
    /// </summary>
    public ModifiableObject()
    {
        mModifiedFunction=null;
    }

    /// <summary>
    /// Call handler for any modification.
    /// </summary>
    protected void modificationMade()
    {
        if (mModifiedFunction!=null)
        {
            mModifiedFunction();
        }
    }

    /// <summary>
    /// Sets the function for modificationMade calls.
    /// </summary>
    /// <param name="modifiedFunction">Function for modificationMade calls.</param>
    public void setModifiedFunction(ModifiedFunction modifiedFunction)
    {
        mModifiedFunction=modifiedFunction;
    }
}
