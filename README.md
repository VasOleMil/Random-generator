# Random-generator (HSM-PRNG)

Simple physical model, fast simplified interaction. Addition to compressor based on hard sphere model, continuous prng.  Random source - interaction events on the sphere surface. Due to the PDF property[1], without two dimensions from initial, may produce even volume distribution. But requires not static projection in high dimensions.

To obtain one dimensional sequence with values from -1 to +1 it is necessary to create three dimensional generator and take only one coordinate. 

Provides simple tests on generated sequence: volume hystogram and bitmap with 2D projection. 

One is slower in low dimesions, but may reach higher than arythmetic prng productivity in random vector generation. Due to sqrt in reflecting function, sequence is unpredictable and not repeatable for the use of partial vector.

1. https://uk.wikipedia.org/wiki/Гіперкульовий_сегмент#Приклади_використання
