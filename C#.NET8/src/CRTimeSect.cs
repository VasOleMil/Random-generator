using System;

namespace RandomHSM.src
{		
	public class CEvent
    {
		//--------------------------------------------------------------------
        public double[] X;//Coordinares, [0:1]
        public double[] V;//Speeds

        public long		E;//Element to interact
		//--------------------------------------------------------------------
    }
    public partial class CRandom
	{
        //--------------------------------------------------------------------
        void TimeIni()
		{		
            Rs = new CEvent();
            Rs.X = new double[Rn];
            Rs.V = new double[Rn];
            // calculate all bound tti
            for (i = 0L; i < Bn; i++) {	ex = Ev[i]; TimeCalcBE(); }
        }
        //--------------------------------------------------------------------
        private void TimeRegEvt()
        {
            Xe = Rs.X; Ve = Rs.V; Xj = em.X; Vj = em.V; Rs.E = Em; rr = em.n;
            for (k = 0L; k < Rn; k++) { Xe[k] = Xj[k] * rr; Ve[k] = Vj[k]; };   
        }// save event for use
        //--------------------------------------------------------------------
        private void TimeCalcBE()
		{   //  get scalar products, Xi - validation
            Xe = ex.X; Ve = ex.V; Xi = ex.x; 
			for (rr = rv = vv = 0D, k = 0L; k < Rn; k++) 
			{
				rk = Xe[k]; rr += rk*rk;
				vk = Ve[k]; vv += vk*vk; rv += rk*vk;			
			}   if(vv==0D) { ex.T = double.MaxValue; return; } //inf
			//  get time to bound
			a = vv; b = rv; c = rr - ex.s;//Gs = 0D
            VV = a * c; RV = b * b; // wiki -> time
            RR = VV / RV; RV -= VV; // A=RR && D=RV 
            if  (  (c  >= 0D)      &&    (b >= 0D))
            {                  dt = -0D;          }
            else if(RV >= 0D)  // one root tti
            {   //  dA = 2D * sqrt( 2D * Dm ); 
                if (Math.Abs(RR) < dA && (b != 0D))
                {   // Linear Taylor approximation
                    dt = (b > 0D) ? -0.5D * c / b:
                         b / a * (0.5D * RR - 2D);
                }   // first part assumed accurate
                else// Full sqrt computation
                {   dt = (Math.Sqrt(RV) - b) / a; }
                // restore bits by cubic Haley step 
                vv = a*dt + b; VV = dt*(vv + b) +c;
                dt -= vv * VV / (2D*RV + 1.5*a*VV);
            }   else {         dt = -0D;          }     
            //  Save times and position for computational economy
            //  since not all saved for validation data used
            ex.T = ex.t = dt; Ka += Ar * (a * ei.M - Ka);
            for (k = 0L; k < Rn; k++) Xi[k] = Xe[k]; 
        }// Calculate ex element  tti
        //--------------------------------------------------------------------
        private void TimeGetStp()
		{
			Em = 0L; em = Ev[Em]; dT = em.T;
			
			for (i = 1L; i < Bn; i++)
			{
				if ((dt = Ev[i].T) < dT)
				{
					dT = dt; Em = i;
				}
			} 	em = Ev[Em]; Ta += Ar * (dT - Ta);
		}// Select minimal tti from Ev, sets Em,em
		//--------------------------------------------------------------------
		private void TimeDecStp()
        {
	        for (i = 0L; i < Bn; i++) Ev[i].T -= dT;
        }// Decreases times
        //--------------------------------------------------------------------
        private void TimeValStp()
        {   //restoring of saved predicted position 
            dt = em.t; Xe = em.X; Xi = em.x; Vi = em.V;
            for (k = 0L; k < Rn; k++) Xe[k] = Xi[k] + Vi[k] * dt;
        }// Verify position, to prevent Te += dT summing errors
        //--------------------------------------------------------------------
    }
}
