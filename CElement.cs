using System;

namespace CRandom
{
    public class CElement
    {
        //--------------------------------------------------------------------
        public      double []   X;//Coordinares
        public      double []   V;//Speeds
        public      double      R;//Radius
        public      double      M;//Radius

        public      int         E;//Element to interact
        public      double      T;//Time to bound
        public      double      t;//Time to element
        //--------------------------------------------------------------------
        public CElement()
        {
            X = null;
            V = null;
            E = -1;
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
                E = -1;
                return true; 
            }

            return false;
        }
        //--------------------------------------------------------------------
    }
}
