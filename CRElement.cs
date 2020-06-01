using System;

namespace RandomHSM
{
    public class CElement
    {
        //--------------------------------------------------------------------
        public      double []   X;//Coordinares
        public      double []   V;//Speeds
        public      double      R;//Radius
        public      double      M;//Radius

        public      double      T;//Time to bound
        //--------------------------------------------------------------------
        public CElement()
        {
            X = null;
            V = null;
        }
        //--------------------------------------------------------------------
        public bool Dim(int Dimension)
        {
            try
            {
                X = new double[Dimension];
                V = new double[Dimension];
            }
            catch (Exception e) 
            { 
                e.Source = "Dim";
                X = null;
                V = null;
                return true; 
            }

            return false;
        }
        //--------------------------------------------------------------------
    }
}
