using System;

namespace RandomHSM
{
    public partial class CRandom
    {
        //--------------------------------------------------------------------
        CElement[]  Ev;//Elements container
        //--------------------------------------------------------------------
        private void EmtsIni()
        {
            //Allocate resources            
            Ev = new CElement[En]; long i;

            for (i = 0L; i < En; i++)
            {
                Ev[i] = ei = new CElement(); ei.Dim(Rn);
            }

            //init elements
            for (ra = 0.02D * dR, i = 0L; i < En; i++) 
            {
                //Set Radius of element
                ei = Ev[i]; ei.R = rk = Re * (1D + ra * (Rd.NextDouble() - 0.5D));
                //Set mass
                Mi = Vg; for (k = 0L; k < Rn; k++) Mi *= rk; ei.M = Mi;
                //Set random coordinates
                EmtsIniGtb();
                //set random speed
                Vi = 2D * Re / Math.Sqrt(Rn * Mi); //set unit radius max speed
                for (k = 0L; k < Rn; k++) ei.V[k] = Vi * (Rd.NextDouble() - 0.5D);
            }
        }
        //--------------------------------------------------------------------
        private void EmtsIniGtb()
        {
            Mm = 2D * Rb; do
            {
                for (rr = 0D, k = 0L; k < Rn; k++)
                {
                    ei.X[k] = rk = Mm * (Rd.NextDouble() - 0.5D);
                    rr += rk * rk;
                }
            }
            while (rr >= RS);
        }//Set random coordinates of ex element within bound
        //--------------------------------------------------------------------
        private void EmtsMovAll()
        {
            for (long i = 0L; i < En; i++)
            {
                for (ei = Ev[i], k = 0L; k < Rn; k++) 
                {
                    ei.X[k] += ei.V[k] * dT;
                }
            }
        }//Moves all elements
        //--------------------------------------------------------------------
        private void EmtsColSel()
        {
            //if (Em % 3L == 1L) return; //P = 1/3

            //Select two elements
            Ei = Em < En - 1 ? Em + 1 : 0; ei = Ev[Ei];
            Ej = 0 < Em ? Em - 1 : En - 1; ej = Ev[Ej];


            //Interact selected elements
            Mi = ei.M;
            Mj = ej.M;

            Mm = (Mi - Mj);
            Mp = 1D / (Mi + Mj);

            for (k = 0L; k < Rn; k++)
            {
                
                Vi = ei.V[k];
                Vj = ej.V[k];

                ei.V[k] = Mp * (2D * Mj * Vj + Mm * Vi);
                ej.V[k] = Mp * (2D * Mi * Vi - Mm * Vj);
            }
            //Calculate selected tti
            ex = ei; TimeCalcBE();
            ex = ej; TimeCalcBE(); 
        }
        //interact near em elements, P = 1
        //--------------------------------------------------------------------
        private void EmtsCollBE()
        {
            for (rr = 0D, rv = 0D, k = 0L; k < Rn; k++) 
            {
                rk = em.X[k]; rr += rk * rk;
                vk = em.V[k]; rv += rk * vk;
            }

            ra = 2D / ((rr == 0D) ? 0D : rr);//RS? //ra = 2D / RS;
            vv = ra * rv;
   
            for (k = 0L; k < Rn; k++) em.V[k] -= vv * em.X[k];
        }//Bound em element interaction
         //--------------------------------------------------------------------
        private void EmtsCollBS()
        {
            for (rr = 0D, rv = 0D, k = 0L; k < Rn; k++)
            {
                rk = ex.X[k]; rr += rk * rk;
                vk = ex.V[k]; vv += vk * vk; 
            }

            ra = 1D / ((rr == 0D) ? 0D : rr);
            rv = Math.Sqrt(vv * ra);

            for (k = 0L; k < Rn; k++) ex.V[k] -= rv * ex.X[k];
        }//Bound em element interaction, force speed alignment to center
        //--------------------------------------------------------------------  
    }
}
