using System;

namespace RandomHSM.src
{
    public class CElement
    {
        //--------------------------------------------------------------------
        public double[] X;//Coordinares, current      
        public double[] V;//Speeds
        public double   R;//Radius
        public double   M;//Mass
        public double   T;//Time to bound
        public double   r;// Rb - R; initial placing
        public double   n;//  1 / r; data output
        public double   s;//  r * r; time computtion
        public double   a;//  1 / s; bound interaction
        public double[] x;//coordinares for validation
        public double   t;//time to bound for validation
        //--------------------------------------------------------------------
    }

    public partial class CRandom
    {
        //--------------------------------------------------------------------
        internal CElement[] Ev;//Elements container
        //--------------------------------------------------------------------
        private void EmtsIni()
        {   //allocate resources   
            Ev = new CElement[Bn]; 
           
            for (i = 0L; i < Bn; i++)
            {
                Ev[i] = ei = new CElement();
                ei.X = new double[Rn];
                ei.x = new double[Rn];
                ei.V = new double[Rn];
            }
            //set ensured parameters
            for (Mb = 0D, i = 0L; i < Bn; i++)
            {   //Set Radius of element
                ei = Ev[i]; rr = 0.02D * dR * (Rd.NextDouble() - 0.5D);
                ei.R = rk = Re * (1D + rr); 
                ei.r = rr = Rb - rk; ei.n = 1D / rr; 
                ei.s = rr = rr * rr; ei.a = 1D / rr;
                //Set mass
                Mi = Vg * rk; for (k = 1L; k < Rn; k++) Mi *= rk;
                ei.M = Mi; Mb += Mi;
            }   Mb  /= Bn;
            //  Init RandomSphereXC() arrays and variables
            rn = Rn + 4L; // use Rn + 2 with one pass normalising
            //ra = 1D / Math.Sqrt(2D);        // used in ra -> IC
            ra = 1D / Math.Sqrt((double)rn);  // used in ra -> BC
            Xs = new double[rn]; // multiplier for speed scale
            Vs = new double[rn]; Mp = Math.Sqrt(3D * KT / Mb);
            do  //  Since initial placing may not suit physically
            {   //  iteration, to refine momenta and mass center
                for (i = 0L; i < Bn; i++)
                {   //  Generate  coordinates & speeds on unit sphere surface
                    RandomSphereBC(); // scale random speeds, and positions
                    ei = Ev[i]; Xi = ei.X; Vi = ei.V; Mm = ei.r;         
                    for (k = 0L; k < Rn; k++)
                    {   //Rn projection is distribution in ball
                        Xi[k] = Mm * Xs[k];
                        Vi[k] = Mp * Vs[k];
                    }   //speed preliminarly scaled to given KT              
                }   VV = 0D; i = 0L; // init pass through flags 
                //  Refine initial random placement, normalisers selection
                TestMassCenter(); NormMassCenter();       // <-one pass
                //  reduce mass center normalization to one pass if even
                //  initial distribution used, rn = Rn + 2 for BC and IC
                if (VV != 1D ) continue;   // comment line for one pass
                TestImpulse();    NormImpulse();          // determined
                TestMassCenter(); NormMomenta();          // determined
                TestEnergy();     NormEnergy();   i = 0L; // determined 
            }   while (i != 0L);  // adjust normalisers to used spaces!
        }
        //--------------------------------------------------------------------
        private void RandomSphereBC() 
        {
            do  // coordinates & speeds, first within inscribed cube
            {   // Compressor->wiki->Placing: RandomSphereBC function
                for (rr = rv = vv = 0D, k = 0; k < rn; k++)
                {   // coordinates inscribed, speeds not scaled
                    rk = Xs[k] = 2D * (Rd.NextDouble() - 0.5D) * ra;
                    vk = Vs[k] = 2D * (Rd.NextDouble() - 0.5D);//*1;      
                    rr += rk * rk; // init scalar products 
                    vv += vk * vk; rv += rk * vk;
                }   // enable bouncing, |v| > 0.0 
            }   while (vv <= 0.0); n = rn+Rd.Next()%rn;
            //  Bounce point by sphere surface, repeat e steps
            for (j = 0L; j < n; j++)//( a->v*v != 0D )
            {   // get time dt to reach rn-sphere of radius 1D  
                // centered in origin, no point radius
                a = vv; b = rv; c = rr - 1.0; // Rc = 0
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
                //  Move to sphere surface
                for (k = 0L; k < rn; k++) Xs[k]+= Vs[k] * dt;
                //  reject point by sphere surface, repeat n steps
                for (rv = 0D, k = 0L; k < rn; k++)
                {       rv += Xs[k] * Vs[k];          }
                RV = 2D * rv; rr = rv = vv = 0D;
                for (k = 0L; k < rn; k++)
                {
                    rk = Xs[k]; Vs[k] -= RV*rk; // bounce by sphere surface
                    vk = Vs[k];    rr += rk*rk;
                    rv+= rk*vk;    vv += vk*vk; // refresh scalar products
                }   
            }// RandomSphereBC optimised globals - avoid public usage
        }// Generate  coordinates & speeds on unit sphere surface
        //--------------------------------------------------------------------
        void RandomSphereIC() // PDF tests in wiki and mathmatica file
        {                     // 20% accuracy, on uniformity       
            //  pick one index coordinate, will belong to cube face
            RR = 0D; j = Rd.NextInt64() % rn; // rn -> long -> Int64
            //  generate others coordinates, uniformly distributed
            for (k = 0L; k < rn; k++)
            {
                if (k == j) { rk = Xs[k] = (Rd.Next() % 2==0) ? 1D : -1D; }
                else        { rk = Xs[k] = 2D * (Rd.NextDouble() - 0.5D); }
                RR += rk * rk;     Vs[k] = 2D * (Rd.NextDouble() - 0.5D);
            }   //Vs can be set outside of this function, if needed
            RR = 1D/Math.Sqrt(RR); for (k = 0L; k < rn; k++)   Xs[k] *= RR; 
            //  random rotations of coordinates, to smooth distribution
            for (k = 0; k < rn; k++)
            {
                e = Rd.NextInt64()%rn; do j = Rd.NextInt64()%rn; while (e == j);

                Mi = Xs[e]; Mj = Xs[j];
                Xs[e] = ra * (Mj - Mi);
                Xs[j] = ra * (Mj + Mi); // rr = 1.0 / sqrt(2.0)
            }   //Simple fast random point generation on unit radius rn-sphere
        }// Generate  coordinates & speeds on unit sphere surface
        //--------------------------------------------------------------------
        private void RandomSphereRC()
        {   
            do  // cubic range [-1:+1]
            {   // reject out of sphere positions
                for (rr = 0D, k = 0L; k < rn; k++)
                {   //generate coordinates
                    Xs[k] = rk = 2D * (Rd.NextDouble() - 0.5D); rr += rk * rk;
                }
            }   while (rr >= 1D); // square distance selector
           
                for (k = 0L; k < rn; k++)
                {   //generate speed, simple cubic direction 
                    Vs[k] = 2D * (Rd.NextDouble() - 0.5D);//*1; 
                }
        }// Generate  coordinates & speeds on unit sphere surface
        //--------------------------------------------------------------------
        private void TestImpulse()
        {
            for (k = 0L; k < Rn; k++)
            {
                vk = rk = 0D;          // Pc[k] = Pa[k] = 0D;
                for (i = 0L; i < Bn; i++)
                {             
                    ei = Ev[i];        // P[k] components 
                    vk += rv = ei.M * ei.V[k];
                    rk += Math.Abs(rv);
                }
                Vs[k] = vk;            // Pc[k] summary
                Xs[k] = rk / (2D * Bn);// Pa[k] average
            }
        }// Get current summary inpulse Pc in Vs, average value in Xs
        //--------------------------------------------------------------------
        private void NormImpulse()
        {   //Exclude drift by zoroing summury impulse by coordinate        
            for (k = 0L; k < Rn; k++)
            {   // Dynamically distribute compensation 
                vk = Vs[k]; if (vk == 0D) continue; //done
                // either uniformly across all particles
                // or targeted using j-scaled average unit impulse
                // log_2(j) + 1 = 4 last bits loss on summing
                j = 8L; rk = Xs[k]; j = (j > Bn) ? Bn : j;
                n = ( j  * (long) (rk / Math.Abs(vk))); // scaler
                n = ( (n < 1) ? 1 : (n > Bn) ? Bn : n);
                for (vk /= n, i = 0L; i < n ; i++)
                {
                    ei = Ev[i]; ei.V[k] -= vk / ei.M;
                }
            }
        }// Zero system drift
        //--------------------------------------------------------------------
        private void TestMassCenter()
        {   //  summary elements mass RR = 1D / Mb
            for (Mm = 0D, i = 0L; i < Bn; i++) Mm += Ev[i].M; RR = 1D / Mm; 
            for (k = 0L; k < Rn; k++)
            {
                rk = 0D;         // Xc[k] = 0.0;
                for (i = 0L; i < Bn; i++)
                {                // Xc[k] component
                    ei = Ev[i]; rk += ei.M * ei.X[k];
                }   
                Xs[k] = rk * RR; // next k
            }
        }// Get current mass center vector Xc in Xs
        //--------------------------------------------------------------------
        private void NormMassCenter()
        {   //  mass center vector Xc = Xs, norm of vector, |Xc| = sqrt(vv)                           
            vv = 0D; for (k = 0L; k < Rn; k++)
            {   //  assume v as vector along Xc direction, v = Xc
                vk = Xs[k]; vv += vk * vk;
            }   if (vv <= 0D) return; VV = vv; // done
            //  Try shift elements Xc to the center of bound
            //  measure possible coordinate displacement:
            //  line to sphere equation, with speed & time                               
            for (i = 0L; i < Bn; i++)
            {   //  Possible -> VV = vv <- Required, squared
                ei = Ev[i]; Xi = ei.X; rv = rr = 0D; 
                for (k = 0L; k < Rn; k++)
                {   // dr = v * dt, v = Xc, dt expressions: wiki link 
                    rk = Xi[k]; rr += rk * rk;
                    vk = Xs[k]; rv += rk * vk;
                }   // dr = (sqrt(rv^2 - (rr-RR) * vv) - rv) * v / vv
                rk = rv * rv; vk = (rr - ei.s) * vv;
                if (rk >= vk) // D = rk - vk; rk=b*b, vk=a*c
                {   // available -> RR || VV <- possible
                    RR = Math.Sqrt(rk - vk) - rv; // <- |dr|
                    if (RR >= 0D) // positive time 
                    { RR *= RR; VV = (RR>VV)? VV:RR;}
                    else // get minimal displacement
                    { VV = 0D;  break;              }
                }   // inbound is always  reachable
                else{ VV = 0D;  break;              }
            }   //  shift saves pair distances
            VV = Math.Sqrt(VV / vv); // scale and make shift
            for (i = 0L; i < Bn; i++)
            {
                ei = Ev[i]; Xi = ei.X;
                for (k = 0; k < Rn; k++) Xi[k] -= VV * Xs[k];
            }
        // https://github.com/VasOleMil/Compressor/wiki#time-change-prediction
        }// Try shift elements Xc to the center of bound
        //--------------------------------------------------------------------
        private void NormMomenta()
        {   //NormMassCenter()->NormImpulse()->TestMassCenter()->NormMomenta() 
            for (n = (long)RN, k = 0L; k < n; k++) 
            {
                for (i = 0L; i < Rn; i++)
                {
                    for (j = i + 1; j < Rn; j++)
                    {
                        RR = RV = 0D; // Xc = Xs
                        for (e = 0L; e < Bn; e++)
                        {   //  get angular speed in plane (i, j)
                            ex = Ev[e]; Xe = ex.X; Ve = ex.V; Mm = ex.M;

                            rr = Xe[i] - Xs[i]; // r(i) relative Xc
                            rv = Xe[j] - Xs[j]; // r(j) relative Xc
                           
                            RV += Mm * (rr * Ve[j] - rv * Ve[i]);
                            RR += Mm * (rr * rr + rv * rv); // Inetrtia
                        }   if (RR <= 0D) continue; // vacuum 

                        VV = RV / RR; // result angular speed in 2D plane 

                        for (e = 0L; e < Bn; e++)
                        {   // suppress rotation, one path
                            ex = Ev[e]; Xe = ex.X; Ve = ex.V;

                            rr = Xe[i] - Xs[i]; // r(i) relative Xc
                            rv = Xe[j] - Xs[j]; // r(j) relative Xc

                            Ve[i] += VV * rv; // compensate
                            Ve[j] -= VV * rr; // by subtraction
                        }// use accumulated angular speed VV as indicator 
                    }   
                }         
            }// iterator for rotation supression
        }// suppress system rotation
        //--------------------------------------------------------------------
        public void TestEnergy()
        {
            for (kT = 0D, i = 0L; i < Bn; i++)
            {
                ei = Ev[i]; Vi = ei.V; vv = 0D;

                for (k = 0L; k < Rn; k++)
                {
                    vk = Vi[k]; vk *= vk; vv += vk;
                }
                kT += ei.M * vv;
            }   kT /= (Bn * Rn);
        }// Test all elements energy, kT
        //--------------------------------------------------------------------
        public void NormEnergy()
        {   //  NormMassCenter()->NormImpulse()->NormMomenta()->NormEnergy()
            VV = Math.Sqrt(KT / kT); kT = KT;

            for (i = 0L; i < Bn; i++)
            {
                ei = Ev[i]; Vi = ei.V;

                for (k = 0L; k < Rn; k++) Vi[k] *= VV;
            }
        }// Norm all elements energy, KT
        //--------------------------------------------------------------------
        private void EmtsMovAll()
        {
            for (i = 0L; i < Bn; i++)
            {
                ei = Ev[i]; Xi = ei.X; Vi = ei.V;

                for (k = 0L; k < Rn; k++) 
                {
                    Xi[k] += Vi[k] * dT;
                }
            }
        }// Moves all elements
        //--------------------------------------------------------------------
        private void EmtsCollEE()
        {
            //if (Em % 2L == 0L) return; //P = 1/2, sparsed try

            //Select two adjuscent to Em elements
            Ei = Em < Bn - 1 ? Em + 1 : 0; ei = Ev[Ei]; Vi = ei.V;
            Ej = 0 < Em ? Em - 1 : Bn - 1; ej = Ev[Ej]; Vj = ej.V;
            //Interact selected elements
            Mi = ei.M; Mj = ej.M;
            Mp =  2D / (Mi + Mj);

            for (k = 0L; k < Rn; k++)
            {          
                vv = Vj[k] - Vi[k]; vv *= Mp;

                Vi[k] += Mj * vv;
                Vj[k] -= Mi * vv;
            }
            //Calculate selected tti
            ex = ei; TimeCalcBE();
            ex = ej; TimeCalcBE(); 
        }// interact near em elements, P = 1
        //--------------------------------------------------------------------
        private void EmtsCollBE()
        {
            Xe = em.X; Ve = em.V; rv = 0D;

            for (k = 0L; k < Rn; k++) rv += Xe[k] * Ve[k];
            //rv= (Ka>X)? rv-Ds : (Ka<x)? rv+Ds:rv; 
            rv *= 2D * em.a; // kT regulation place
   
            for (k = 0L; k < Rn; k++) Ve[k] -= rv * Xe[k];
        }// Bound em element interaction
        //--------------------------------------------------------------------
        private void EmtsCollBC()
        {
            Xe = ex.X; Ve = ex.V; rr = vv = 0D;

            for (k = 0L; k < Rn; k++)
            {
                rk = Xe[k]; rr += rk * rk;
                vk = Ve[k]; vv += vk * vk; 
            }

            ra = 1D/((rr==0D)?0D:rr);
            rv = Math.Sqrt(vv * ra) ;

            for (k = 0L; k < Rn; k++) Ve[k] =- rv * Xe[k];
        }// Bound ex element interaction, force speed alignment to center
        //--------------------------------------------------------------------  
    }
}
