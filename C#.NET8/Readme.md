# High-Dimensional Asynchronous Hard-Sphere Generator (C# .NET 8)

This repository implements a high-performance, hardware-aware asynchronous event-driven initialization [engine for hard-sphere packaging](https://github.com/VasOleMil/Compressor) and thermalization tasks in multi-dimensional spaces ($3D$ to $12D$). With virtual particle speed interchange linear complexity is reduced to fixed, further simplification - lowering beyond unit ratio of direction/speed interchange events shows non random behaviour.

![7D Sphere surface](SRandom.Rep-07-40000.png)

## Implementation Peculiarities:

* **Precision-Oriented (Mantissa Bit Restoration):** The solver utilizes a full time-predictor equation form combined with a cubic Halley step to restore lost bits of the double-precision (`double`) mantissa after square root operations. This rigorous approach completely eliminates out-of-range position generation and prevents idle time-stepping drift when subtracting the time-span $dT$ near bounding walls. The additional computational load of the high-precision predictor is compensated by the simplicity of linear approximations when calculating time-spans in the immediate vicinity of the boundary.  

* **Dimension-Independent Mass Distribution (Center Reachable):** By scaling the element radius difference inversely to the space dimension ($dR = d/R_n$), the stepping function is regularized against the "curse of dimensionality." This constraints the mass ratio between the heaviest and lightest hard spheres to a strict upper bound: $M_{max}/{M_{min} \to \exp(d)} \approx 1.105,\quad d=0.1$. This prevents the system from generating massive directional mass imbalances, ensuring a stable, contrast-invariant mass distribution that does not polarize or distort as dimensions scale upward.

* **Hyperspace Regularization & Mass-Center Alignment (Center Positioned):** To achieve seamless initial mass-center normalization in high dimensions ($6D+$), the generator initializes coordinates in an expanded hyperspace ($r_n = R_n + 4$). Due to the concentration of measure phenomenon, projecting this higher-dimensional distribution back into the working $R_n$ space naturally shifts the initial mass density away from the bounding wall toward the geometric center. This provides the iterative `NormMassCenter` solver with the necessary spatial clearance (free path buffer `VV`) to align the center of mass into a strict machine zero ($\approx 10^{-16}$ for IEEE 754 double precision). Since the initial expansion wave strictly affects the omitted time span $\tau$ (which is not included in the output), this symmetrical center concentration allows researchers to utilize the spatial distribution immediately following initialization. 

_Note on control long-time memory tails:_  to identify precisely the termination of the thermalization phase (typically $400K+$ asynchronous steps for 100 elements), the system tracks $Ta$ — an Exponential Moving Average (EMA) mean free time $\tau$, calculated over an $Ar = 64$ step window. Furthermore, to prevent smooth, non-ergodic energy drift ($Ka = R_n \cdot kT$), a modified multiplier can be implemented within `EmtsCollBE`: the introduction of $Ds$ (near-LSB bit-doping) keeps the system parameters securely anchored within the corresponding physical range.

_Note on Boundaries:_ Boundary alignment using a static geometric elements probabilty interact/move (`Pbound`) allows for clean analytical derivation. However, to align the boundary directly with a dynamic physical model—where the boundary/element interaction ratio is perfectly balanced — the full Compressor model configuration must be used, accounting for exact particle quantities and geometric radii.

## Acknowledgements:
Special thanks to the Gemini LLM group (Google) for analytical collaboration, verification of mathematical boundaries, and assistance in debugging the thermodynamic phase-space constraints during the development of this C# implementation.
