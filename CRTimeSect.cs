using System;

namespace RandomHSM
{
    public partial class CRandom
	{
		//--------------------------------------------------------------------
		void TimeIni()
		{
			for (long i = 0L; i < En; i++)
			{
				ex = Ev[i]; TimeCalcBE(); 
			}
		}
		//--------------------------------------------------------------------
		private void TimeCalcBE()
		{
			for (rr = 0D, rv = rr, vv = rr, k = 0L; k < Rn; k++) 
			{
				rk = ex.X[k];
				vk = ex.V[k];

				rr += rk * rk;
				rv += rk * vk;
				vv += vk * vk;
			}
			//Variables for readability
			a = vv; b = rv; c = rr - RS;
			//Theory, eternal rest
            if (a == 0D) { ex.T = Rb * En * En; return; }
			//Return escapers
			if (+Dc <= c && b >= -Ds) 
			{
                EmtsCollBS();	//Use precalced values
                b = -rv * rr;	//Sqrt(vv / rr) * rr
            }
			//Get time change
			if (-Ds <= b && b <= +Ds)
			{
                c = (c < 0D) ? -c : +c;
                dt = Math.Sqrt(-c / a);
			}
			else
			{
				A = (a * c) / (b * b);
				A = Math.Sqrt(1D - A);

				c = (b > 0D) ? +A : -A;
				dt = b / a * (-1D + c);
			}
			//Save final position
            for (k = 0L; k < Rn; k++)
            {
				ex.x[k] = ex.X[k] + ex.V[k] * dt;
            }   ex.T = dt;  //save time

        }//Calculate Ex element  tti
         //--------------------------------------------------------------------
        private void TimeGetStp()
		{
			Em = 0L; em = Ev[Em]; dT = em.T;
			
			for (long i = 1L; i < En; i++)
			{
				ei = Ev[i]; dt = ei.T; if (dt < dT)
				{
					dT = dt; Em = i;
				}
			}
			em = Ev[Em];
		}//Select minimal tti from Ev, sets Em,em
		//--------------------------------------------------------------------
		private void TimeDecStp()
        {
	        for (long i = 0L; i < En; i++)
			{
				Ev[i].T -= dT;
			}
        }//Decreases times
		//--------------------------------------------------------------------
	}
}
