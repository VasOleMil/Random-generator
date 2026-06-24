# Random-generator (HSM-PRNG)

Simple physical model, simplified virtual interaction. Addition to compressor based on hard sphere model, continuous PRNG. Due to the [PDF property](https://uk.wikipedia.org/wiki/Гіперкульовий_сегмент#Приклади_використання) without two dimensions from initial, may produce even volume distribution. 

To obtain one dimensional sequence with values from -1 to +1 it is necessary to create three dimensional generator and take only one coordinate. Provides simple tests on generated sequence: volume hystogram and bitmap with 2D projection. One is slower in low dimesions, but may reach higher than arythmetic prng productivity in random vector generation. Due to sqrt in reflecting prediction function, sequence is unpredictable and not repeatable for the use of partial vector. 

### Information-Theoretic Note on the Source of Variance

Even-Unbiased Entropy Source (Beyond the Seed): In addition to the initial pseudo-random seed, the asynchronous direction/speed interchange events draw from an infinite source of variance rooted in the irrational number presentation within the floating-point base.

Unlike purely arithmetic pseudo-random generators that operate on finite, periodic rational fields, this physical engine continuously invokes geometric non-linear operations (such as hyper-spherical normalization and high-precision boundary predictors involving square roots). Even though the time-span computation utilizes a rationalized linear approximation in the boundary vicinity, it fundamentally relies on a much larger, infinite irrational number base. As millions of asynchronous interactions (megaflops) scale and shift these irrational mantissa chains, the system continuously uncovers new, non-periodic bit combinations. This algorithmic non-linearity acts as an information amplifier: it extracts microscopic hardware-level rounding variations across different CPU architectures and transforms them into true, macroscopic thermodynamic entropy. Consequently, the solver achieves true ergodic shuffling where state memory is dissolved not by arithmetic loops, but by the chaotic dynamics of the irrational continuum.

### Practical Applications & Use Cases

Beyond high-dimensional physics simulations, this asynchronous hard-sphere engine serves as a specialized architectural alternative to standard PRNGs:

**Hardware Generator Emulation & Cross-Verification (TRNG Mirroring)**: Because the engine extracts true macroscopic entropy by chaotic "mining" of hardware-level floating-point rounding errors across different CPU generations, it can act as a fully decentralized software-based emulator or a verification shadow for True Random Number Generators (TRNGs).

**Analytical Process Control & Traceability**: In systems where strict auditing of the randomized pipeline is required, traditional arithmetic generators often fail to provide geometric or physical context. This solver provides deep analytical control over the evolution of randomness. Because every step of the phase-space shuffling is constrained by deterministic conservation laws (energy, momenta, and center of mass), the trajectory of the entropy expansion remains structurally verifiable and traceable at any given point in time.
