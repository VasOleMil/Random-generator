# Random-generator
Simple physics model, fast interaction.  For the fun, as addition to compressor based on hard sphere model[1] i wrote a continuous random number generator, actually one generates events on the sphere surface but due to the theorem[2], without two dimensions from initial, acting equal to volume distribution. So i 've managed to obtain continuous volume generation within sphere.

Thanks to this, it is possible to obtain parameters of uniform distribution, such as fluctuation, and accordingly the model is a source of data for calibration of distributions related to reliability.

What is next step? Productivity of generator is comparable with Mathematica 12 RandomReal function. One is quite slow but advantage of generator proposed is theoretically clear base, and cryptographic strength.

1. https://www.facebook.com/groups/249701935475374/permalink/631840700594827/
2. https://www.facebook.com/groups/249701935475374/permalink/640307096414854/
