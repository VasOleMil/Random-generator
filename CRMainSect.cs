using System;
using System.Security.Cryptography.X509Certificates;

namespace RandomHSM
{
    public partial class CRandom
    {
        //--------------------------------------------------------------------
	    public long		        Rn;//Space dimension
        public long		        En;//Number of elements
        public double           Rb;//Bound radius
        public double           Re;//Element radius
        public double           dR;//Element difference %
        public double           Vb;//Bound volume
        public double           dT;//Current time step
        public CEvent           Rs;//Return sequence
        public double           kT;

        //Service
        internal Random         Rd;//Random generator
        
        //Misc constants
        internal double         RN;//Sqrt(Rn) 
        internal double         RS;//Rb * Rb 
        internal double         Rr;// 1 / Rb
        internal double         De;//double accuracy, Dprec()
        internal double         Ds;// -0- , b calculational drift
        internal double         Dc;// -0- , c calculational drift
        internal double         Vg;//Vgamma(Rn)
        
        //Manual optimising by globals
        internal long           Em;
        internal CElement       em;//Ev[Em]

        internal long           k , Ei, Ej;
        internal CElement       ex, ei, ej;
        internal double         rr, rv, vv, rk, vk, ra, dt, a, b;
        internal double         Vi, Vj, Mi, Mj, Mm, Mp, Ee, A, c;
        internal double[]       sw;//switcher to saved position
        //--------------------------------------------------------------------
        public CRandom(long Dim, long Num, double Rbound, double Relement, double Rdif,int Seed)
        {
            Rn = Dim;
            En = Num;
 
            Rb = Rbound;
            Re = Relement;
            dR = Rdif;

            Rd = new Random(Seed);

            MainIni();
            EmtsIni();
            TimeIni();
        }//Object construction
        //--------------------------------------------------------------------
        public CRandom(long Dim, long Num, int Seed)
        {
            Rn = Dim;
            En = Num;

            Re = 1.00D;
            dR = 20.0D;

            Mm = (En * (En - 1D)) / (2D * Math.Log(2D));
            Rb = Re / 2 * (1.0D + 4.0D * Math.Pow(Mm, 1D / Rn));

            Rd = new Random(Seed);

            MainIni();
            EmtsIni();
            TimeIni();
        }//Object construction
        //--------------------------------------------------------------------
        public CRandom(long Dim, long Num)
        {
            Rn = Dim; 
            En = Num;

            Re = 1.00D;
            dR = 20.0D;

            Mm = (En * (En - 1D)) / (2D * Math.Log(2D));
            Rb = Re / 2 * (1.0D + 4.0D * Math.Pow(Mm, 1D / Rn));

            Rd = new Random();

            MainIni();
            EmtsIni();
            TimeIni();
        }//Object construction
         //--------------------------------------------------------------------
        internal void MainIni()
        {
            RN = Math.Sqrt(Rn);
            Rr = 1D / Rb;
            RS = Rb * Rb;          
            Vg = Vgamma(Rn);
            Vb = Vg; for (k = 0L; k < Rn; k++) Vb *= Rb;

            Dprec(); //De
            Ds = De * (RN * (Rb + Re) + (1D + RN) * (1D + RN) * De);
            Dc = Ds * 20D; //c calculational drift, check in escapers handling

            Rs = new CEvent(); Rs.Dim(Rn);
        }
        //--------------------------------------------------------------------
        public void Next()
        {
            //Select minimal tti  from Ev
            TimeGetStp(); 
            //Change geometry
            EmtsMovAll();           
            //Change tti container
            TimeDecStp(); 
            //Restore saved position
            sw = em.X; em.X = em.x; em.x = sw;
            //Interact with bound
            EmtsCollBE(); ex = em;
            //Calculate new tti
            TimeCalcBE();           
            //register event
            EmtsRegEvt();
            //Probably interact elements, P = 1/2
            EmtsColSel();
        }
        //--------------------------------------------------------------------
        private void EmtsRegEvt()
        {
            for (k = 0L; k < Rn; k++)
            {
                rk = em.X[k];
                Rs.X[k] = rk;
                Rs.V[k] = rk * Rr;
            }
            Rs.E = Em;
        }
        //--------------------------------------------------------------------
        private double Vgamma(long Dim)
        {
	        double Pk, Pi; long i, k, d, Rh;

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
        private void Dprec()
        {
            double  Unit = 1.00D;
            double  Step = 10.0D;
            double  Shift = Unit;

            while (Unit + Shift != Unit) Shift /= Step;

            De = Shift * Step;
        }
        //--------------------------------------------------------------------
        public void TEnergy()
        {
            Ee = 0D;

            for (long i = 0L; i < En; i++)
            {
                ex = Ev[i]; vv = 0D;

                for (k = 0L; k < Rn; k++) 
                {                  
                    vk = ex.V[k]; vk *= vk; vv += vk;
                }
                Ee += ex.M * vv;
            }

            kT = Ee /= (2D * En * Rn);
        }//Test all elements energy, kT
        //--------------------------------------------------------------------
    }
}
