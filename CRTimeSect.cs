using System;

namespace RandomHSM
{
    public partial class CRandom
	{
		//--------------------------------------------------------------------
		void TimeIni()
		{
			for (int i = 0; i < En; i++)
			{
				TimeCalcBE(i); 
			}
		}
		//--------------------------------------------------------------------
		private void TimeCalcBE(int Ex)
		{
			double VV, RV, RR, TT, LL, Vv, Rr; int  k; CElement ex = Ev[Ex];

			RR = 0D; VV = 0D; RV = 0D; for (k = 0; k < Rn; k++)
			{
				Rr = ex.X[k];
				Vv = ex.V[k];

				RR += Rr * Rr;
				RV += Rr * Vv;
				VV += Vv * Vv;
			}

			LL = Rb * Rb - RR;

			if ((LL <= 0D) && (RV >= 0D))
			{
				ex.T = 0D;				
			}
			else
			{
				TT = VV * LL + RV * RV;
				ex.T = (TT <= 0D) ? (-1D) : (VV == 0D) ? (-1D) : ((-RV + Math.Sqrt(TT)) / VV);
			}
		}//Calculate Ex element  tti
		//--------------------------------------------------------------------
		private void TimeGetStp()
		{
			int i; CElement ei; double Tm;

			Rs.E = 0; ei = Ev[0]; Tm = ei.T;
			
			for (i = 1; i < En; i++)
			{
				ei = Ev[i]; if (ei.T < Tm)
				{
					Tm = ei.T;
					Rs.E = i;
				}
			}
			
			dT = Ev[Rs.E].T; 
		}//Select minimal tti Tm from Ev
		//--------------------------------------------------------------------
		private void TimeDecStp()
        	{
	        	for (int i = 0; i < En; i++)
			{
				Ev[i].T -= dT;
			}
        	}//Decreases times
		//--------------------------------------------------------------------
	}
}
