using System;

namespace RandomHSM
{
    public partial class CRandom
    {
        //--------------------------------------------------------------------
	    public int		        Rn;//Space dimension
        public int		        En;//Number of elements
        public double           Rb;//Bound radius
        public double           Re;//Element radius
        public double           dR;//Element difference %
        public double           Vb;//Bound volume
        public double           dT;//Current time step
        public CEvent           Rs;//Return sequence

        Random                  Rd; //Random generator
        //--------------------------------------------------------------------
        public CRandom(int Dim, int Num, double Rbound, double Relement, double Rdif)
        {
            Rn = Dim; 
	        En = Num;
 
            Rb = Rbound;
            Vb = Vgamma(Rn);  for (int k = 0; k < Rn; k++) Vb *= Rb;
            Re = Relement;
            dR = Rdif;

            Rd = new Random();
            Rs = new CEvent(); Rs.Dim(Rn);
            
            EmtsIni();
            TimeIni();
        }//Object construction
        //--------------------------------------------------------------------
        public CRandom(int Dim, int Num)
        {
            Rn = Dim;
            En = Num;
          
            Re = 1.00D;
            dR = 10.0D;

            Rb = Re * Math.Pow((double)Num * 100D, 1D / Rn);
            Vb = Vgamma(Rn); for (int k = 0; k < Rn; k++) Vb *= Rb;

            Rd = new Random();
            Rs = new CEvent(); Rs.Dim(Rn);

            EmtsIni();
            TimeIni();
        }//Object construction
        //--------------------------------------------------------------------
        public void Next()
        {
            //Select minimal tti  from Ev
            TimeGetStp(); 
            //Change geometry
            EmtsMovAll();
            //Change tti
            TimeDecStp();
            //Interact with bound
            EmtsCollBE();
            //calculate new tti
            TimeCalcBE(Rs.E);
            
            //register event
            EmtsRegEvt();

            //Select two elements
            int Ei = Rs.E < En - 1 ? Rs.E + 1 : 0;
            int Ej = 0 < Rs.E ? Rs.E - 1 : En - 1;
            //Interact selected elements
            EmtsColSel(Ei, Ej);
            //Calculate selected tti
            TimeCalcBE(Ei);
            TimeCalcBE(Ej);
        }
        //--------------------------------------------------------------------
        private double Vgamma(int Dim)
        {
	        double Pk, Pi; int i, k, d, Rh;

	        k = 1; d = 2; Pk = 1D; Pi = 3.141592653589793;
	
	        if (Dim % 2 == 0)
	        {
		        for (i = 1, Rh = (Dim - 0) / 2; i <= Rh; i++)
		        { 
			        Pk *= Pi; k *= i;
		        }

		        return Pk / k;
	        }
	        else
	        {
		        for (i = 1, Rh = (Dim - 1) / 2; i <= Rh; i++)
		        { 
			        Pk *= Pi;  d *= 2; k *= 2 * i + 1;
		        }

		        return Pk * d / k;
	        }
        }
		//--------------------------------------------------------------------
	}
}
