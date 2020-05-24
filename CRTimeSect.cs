using System;

namespace RandomHSM
{
    public partial class CRandom
	{
		//--------------------------------------------------------------------
		double		Tx;
		//--------------------------------------------------------------------
		void TimeIni()
		{
			double Tm; int i, j;

			for (i = 0; i < En; i++)
			{
				TimeCalcBE(i); Ev[i].t = -1; Tm = Ev[i].T;

				for (j = 0; j < En; j++) if(i!=j)
					{
						TimeCalcEE(i, j);
						if(0D <= Tx && Tx < Tm)
						{
							Tm = Tx;
							Ev[i].t = Tx;
							Ev[i].E = j;
						}
					}
			}
		}
		//--------------------------------------------------------------------
		private void TimeCalcEE(int Ei, int Ej)
		{
			double VV, RV, RR, TT, LL, Vv, Rr; long k; CElement ei = Ev[Ei], ej = Ev[Ej];

			RR = 0D; VV = 0D; RV = 0D; for (k = 0; k < Rn; k++)
			{
				Rr = (ej.X[k] - ei.X[k]);
				Vv = (ej.V[k] - ei.V[k]);

				RR += Rr * Rr;
				RV += Rr * Vv;
				VV += Vv * Vv;
			}

			Rr = ei.R + ej.R; Rr = Rr * Rr; LL = Rr - RR;

			if ((LL >= 0D) && (RV < 0D))
			{
				Tx = 0D;
			}
			else
			{
				TT = VV * LL + RV * RV;

				Tx = (TT <= 0D) ? (-1D) : (VV <= 0D) ? (-1D) : ((-RV - Math.Sqrt(TT)) / VV);
			}
		}//Calculate Ei Ej elements  tti
		//--------------------------------------------------------------------
		private void TimeCalcBE(int Ex)
		{
			double VV, RV, RR, TT, LL, Vv, Rr, Ri, Rj; int  k; CElement ex = Ev[Ex];

			RR = 0D; VV = 0D; RV = 0D; for (k = 0; k < Rn; k++)
			{
				Rr = ex.X[k];
				Vv = ex.V[k];

				RR += Rr * Rr;
				RV += Rr * Vv;
				VV += Vv * Vv;
			}

			Ri = ex.R; Rj = Ri - Rb; LL = Rj * Rj - RR;

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
			int i; CElement ei;
			double Tm = Ev[0].T; Ei = 0; Ej = -1; 
			
			for (i = 1; i < En; i++)
			{
				ei = Ev[i]; if (ei.T < Tm)
				{
					Tm = ei.T;
					Ei = i;
					Ej = -1;
				}
			}

			for (i = 0; i < En; i++)
			{
				ei = Ev[i]; if (0D <= ei.t && ei.t < Tm)
				{
					Tm = ei.t;
					Ei = i;
					Ej = ei.E;
				}
			}
			//Set time step
			if (Ej < 0) dT = Ev[Ei].T; else dT = Ev[Ei].t;
		}//Select minimal tti Tm from Ev
		//--------------------------------------------------------------------
		private void TimeDecStp()
        	{
	        for (int i = 0; i < En; i++)
			{
				CElement ei = Ev[i]; 
				
				ei.T -= dT;
				ei.t -= dT;
			}
        	}//Decreases times
		//--------------------------------------------------------------------
		private void TimeIniSts()
		{
			double Tm; int i, j;

			TimeCalcBE(Ei); Ev[Ei].t = -1; Ev[Ei].E = -1; Tm = Ev[Ei].T;

			for (j = 0; j < En; j++) if (Ei != j)
				{
					TimeCalcEE(Ei, j);
					if (0D <= Tx && Tx < Tm)
					{
						Tm = Tx;
						Ev[Ei].t = Tx;
						Ev[Ei].E = j;
					}
				}

			if (Ej < 0) return;

			TimeCalcBE(Ej); Ev[Ej].t = -1; Ev[Ej].E = -1; Tm = Ev[Ej].T;
			
			for (i = 0; i < En; i++) if (Ej != i)
				{
					TimeCalcEE(Ej, i);
					if (0D <= Tx && Tx < Tm)
					{
						Tm = Tx;
						Ev[Ej].t = Tx;
						Ev[Ej].E = i;
					}
				}
		}//Calculate tti for Ei, Ej elements
	}
}
