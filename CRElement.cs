using System;

namespace RandomHSM
{
    public class CElement
    {
        //--------------------------------------------------------------------
        public      double []   X;//Coordinares
        public      double []   x;//Coordinares
        public      double []   V;//Speeds
        public      double      R;//Radius
        public      double      M;//Radius
        public      double      T;//Time to bound
        //--------------------------------------------------------------------
        public CElement()
        {
            X = null;
            x = null;
            V = null;
        }
        //--------------------------------------------------------------------
        public void Dim(long Rn)
        {
            X = new double[Rn];
            x = new double[Rn];
            V = new double[Rn];          
        }
        //--------------------------------------------------------------------
    }
}
