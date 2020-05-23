using System;
using System.Security.Cryptography.X509Certificates;

namespace RandomHSM
{
    public partial class CRandom
    {
        //--------------------------------------------------------------------
	public int		Rn;//Space dimension
        public int              rn;//output dimension
        public int		En;//Number of elements
        public double           Ee;//Space range (-Ee;+Ee) treated as 0
        public double           Rb;//Bound radius
        public double           Ri;//Element radius
        public double           dR;//Element difference %
        public double           Vb;//Bound volume
        public double           dT;//Current time step
        public double           Te;//Eventual time
        public double           Tc;//Continious time

        public double           Ef;//Ce/(Cb+Ce)*100%
        public double           Pe;//Summary energy
        public double           Pm;//Summary mass
        public double           Pp;//Abs impulse
        public double           Pc;//Abs center of mass
        public double           Pg;//Average geometry deviation
        public double           Pt;//Uniform
        public double           Pv;//Volume

        public CEvent           Rs; 

        Random                  Rd; //Random generator
        int                     Cb; //Number of e-e interactions 
        int                     Ce; //Number of b-e interactions
  
        //--------------------------------------------------------------------
        public CRandom(int Dim, int Num, double Rbound, double Relement, double Rdif)
        {
            Rn = Dim; 
            rn = Rn - 2;
	    En = Num;
 
            Rb = Rbound;
            Vb = Vgamma(Rn);  for (int k = 0; k < Rn; k++) Vb *= Rb;
            Ri = Relement;
            dR = Rdif;
            Te = Tc = dT = 0D;
	        
            Pc = 0.000;
	    Pp = 0.000;
	    Pe = 0.000;
	    Pg = 0.000;
            Ef = 0D; Cb = 0; Ce = 0;

            Rd = new Random();
            Rs = new CEvent(); Rs.Dim(Rn);
            
            EmtsIni();
            TimeIni();
        }//Object construction
        //--------------------------------------------------------------------
        public void RunOne()
        {
            //Select minimal tti  from Ev
            TimeGetStp(); 
            //Change geometry
            EmtsMovAll();
            //Collide selected elements
            EmtsColSel();
            //Change tti
            TimeDecStp();
            //Calculate new tti, return selected elements
            TimeIniSts();
        }
        //--------------------------------------------------------------------
        public void Next()
        {
            double x, xx;
            do
            {
                RunOne();
                xx = 0; for (int k = 0; k < rn; k++) xx += (x = Rs.V[k]) * x;
            }
            while (xx > 1D);
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
