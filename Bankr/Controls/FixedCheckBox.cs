using System;
using System.Windows.Controls;

namespace CMcG.Bankr.Controls
{
    //Checkbox causes the debugger to throw some exception
    public class FixedCheckBox : CheckBox 
    { 
        public override string ToString() 
        { 
            return "Content: " + Content; 
        } 
    }
}