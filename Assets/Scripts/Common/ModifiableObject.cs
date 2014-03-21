using UnityEngine;
using System.Collections;

public class ModifiableObject
{
    public delegate void ModifiedFunction();

    private ModifiedFunction mModifiedFunction;

    public ModifiableObject()
    {
        mModifiedFunction=null;
    }

    protected void modificationMade()
    {        
        if (mModifiedFunction!=null)
        {
            mModifiedFunction();
        }
    }

    public void setModifiedFunction(ModifiedFunction modifiedFunction)
    {
        mModifiedFunction=modifiedFunction;
    }
}
