using System;

namespace RandomHSM
{
    public class CEvent
    {
        //--------------------------------------------------------------------
        public      double []   X;//Coordinares
        public      double []   V;//Speeds

        public      int         E;//Element to interact
        //--------------------------------------------------------------------
        public CEvent()
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
