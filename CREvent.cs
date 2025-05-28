using System;

namespace RandomHSM
{
    public class CEvent
    {
        //--------------------------------------------------------------------
        public      double []   X;//Coordinares
        public      double []   V;//Speeds

        public      long        E;//Element to interact
        //--------------------------------------------------------------------
        public CEvent()
        {
            X = null;
            V = null;
        }
        //--------------------------------------------------------------------
        public void Dim(long Rn)
        {
            X = new double[Rn];
            V = new double[Rn];
        }
        //--------------------------------------------------------------------
    }
}
