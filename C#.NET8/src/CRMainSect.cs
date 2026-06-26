using System;

namespace RandomHSM.src
{
    public partial class CRandom
    {
        //--------------------------------------------------------------------
	    public long		        Rn;//Space dimension
        public long		        Bn;//Number of elements
        public double           Rb;//Bound radius
        public double           Re;//Element radius
        public double           dR;//Element difference %
        public double           dT;//Current time step
        public CEvent           Rs;//Return sequence
        public double           KT;//initial kT
        public double           kT;//current kT
        public double           Ka;// mean kT * Rn
        public double           Ta;// mean free time, span, tti

        //Service
        internal Random         Rd;//Random generator

        //Misc constants
       
        internal long           rn;// Rn + 2
        internal double         ra;// 1/ Sqrt(rn) 
        internal double         RN;// Sqrt(Rn) 
        internal double         Dm;// double accuracy, Drange()
        internal double         dA;// dA = 2.0 * sqrt(2.0 * Dm)
        internal double         Ar;// Ar = 1.0 / 64.0; 64 steps 15% accuracy 
        internal double         Ds;// -0- , b calculational drift
        internal double         De;// -0- , c calculational drift
        internal double         Vg;// Vgamma()
        internal double         Vb;// Bound volume
        internal double         Mb;// Average element mass/volume

        //optimising by globals, registers

        internal long           k , i , j , e , n , Ei, Ej, Em;
        internal CElement       em, ex, ei, ej;
        internal double         rr, rv, vv, rk, vk, dt, a , b ;
        internal double         RR, RV, VV, Mi, Mj, Mm, Mp, c ;
        internal double []      Xe, Ve, Xi, Xj, Vi, Vj, Xs, Vs;
        //--------------------------------------------------------------------
        public CRandom(long Dim, long Num, double Rbound, double Relement, double Rdif, int Seed)
        {
            Rn = Dim; 
            Bn = Num;
 
            Rb = Rbound;   // calculate volumes: Vb, Mb - estimation
            Re = Relement; Vgamma();   Vb = Vg * Rb; Mb = Vg * Re;
            dR = Rdif; for (k = 1L; k < Rn; k++) { Vb *= Rb; Mb *= Re; }

            Rd = new Random(Seed);

            MainIni();
            EmtsIni();
            TimeIni();
        }// Object construction, R&D constructor
        //--------------------------------------------------------------------
        public 
        CRandom(long Dim, long Num, int Seed)
        {
            Rn = Dim;
            Bn = Num; 

            Re = 1.000D; Pbound();
            dR = 10D/Rn; // d = 10.0 %, Mmax/Mmin = exp(d/100.0)

            Rd = new Random(Seed);

            MainIni();
            EmtsIni();
            TimeIni();
        }// Object construction
        //--------------------------------------------------------------------
        public 
        CRandom(long Dim, long Num)
        { 
            Rn = Dim; 
            Bn = Num;

            Re = 1.000D; Pbound();
            dR = 10D/Rn; // d = 10.0 %, Mmax/Mmin = exp(d/100.0)

            Rd = new Random(); //(int) DateTime.Now.Ticks & 0x0000FFFF

            MainIni();
            EmtsIni();
            TimeIni();
        }// Object construction
        //--------------------------------------------------------------------
        internal void MainIni()
        {                        
            Drange(); 
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
            TimeValStp();
            //Interact with bound
            EmtsCollBE(); ex = em;
            //Calculate new tti
            TimeCalcBE();           
            //register event
            TimeRegEvt();
            //Probably interact elements, P = 1
            EmtsCollEE();
        }
        //--------------------------------------------------------------------
        internal void Vgamma()
        {
	        k = 1L; j = 2L; rk = 1D; vk = 3.141592653589793D;
	
	        if (Rn % 2L == 0L)
	        {
		        for (i = 1L, e = (Rn - 0L) / 2L; i <= e; i++)
		        { 
			        rk *= vk; k *= i;
		        }   Vg = rk / k;
	        }
	        else
	        {
		        for (i = 1L, e = (Rn - 1L) / 2L; i <= e; i++)
		        { 
			        rk *= vk; j *= 2L; k *= 2L * i + 1L;
		        }   Vg = rk * j / k;
	        }
        }
        //--------------------------------------------------------------------
        internal void Drange()
        {
            double  Unit = 1.00D; KT = Rb * Rb * Mb / Rn;
            double  Step = 10.0D; Ka = Rn * KT;
            double  Shift = Unit; Ar = 1D / 64D; Ta = 1D;

            while (Unit + Shift != Unit) Shift /= Step; RN = Math.Sqrt(Rn);

            Dm = Shift * Step; dA = 2D * Math.Sqrt(2D * Dm);
            Ds = 2D * Dm * RN * Rb; De = 32D* Ds;// not used
        }
        //--------------------------------------------------------------------
        internal void Pbound()
        {
            a = 1D / (double)Rn; c = (double)Bn; 
            // Rb = r (1 + 2 (1 - P^(2/((Bn - 1) Bn)))^(-1/Rn))
            // P  = 1/2; approximation for Bn > 10
            // Rb = r (1 + 2 (2 Log[1/P] (Bn+1)/ (Bn^3))^(-(1/Rn)))
            RR = 2D * Math.Log(2D) * (c + 1) / (c * c * c);
            RR = 2D / Math.Pow(RR, a); // outer pow  
            //Fallback to a larger radius if calculation fails
            Rb = Re * ((RR > 2D) ? (1D + RR) : 3D);
            //Calculate volumes: Vb, Mb - estimation for one element
            Vgamma(); Vb = Vg * Rb; Mb = Vg * Re;
            for (k = 1L; k < Rn; k++) { Vb *= Rb; Mb *= Re; }
        }
        //--------------------------------------------------------------------
    }
}
