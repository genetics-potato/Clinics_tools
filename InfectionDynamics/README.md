### 1. Ebola virus infection modeling and identifiability problems

+ **DOI** [10.3389/fmicb.2015.00257](https://www.doi.org/10.3389/fmicb.2015.00257)
+ **ISSN** 1664-302X

###### ABSTRACT
The recent outbreaks of Ebola virus (EBOV) infections have underlined the impact of the virus as a major threat for human health. Due to the high biosafety classification of EBOV (level 4), basic research is very limited. Therefore, the development of new avenues of thinking to advance quantitative comprehension of the virus and its interaction with the host cells is urgently needed to tackle this lethal disease.

Mathematical modelling of the EBOV dynamics can be instrumental to interpret Ebola infection kinetics on quantitative grounds. To the best of our knowledge, a mathematical modelling approach to unravel the interaction between EBOV and the host cells is still missing. In this paper, a mathematical model based on differential equations is used to represent the basic interactions between EBOV and wild-type Vero cells in vitro. Parameter sets that represent infectivity of pathogens are estimated for EBOV infection and compared with influenza virus infection kinetics. The average infecting time of wild-type Vero cells in EBOV is slower than in influenza infection. Simulation results suggest that the slow infecting time of EBOV could be compensated by its efficient replication. This study reveals several identifiability problems and what kind of experiments are necessary to advance the quantification of EBOV infection. A first mathematical approach of EBOV dynamics and the estimation of standard parameters in viral infections kinetics is the key contribution of this work, paving the way for future modelling work on EBOV infection.

![](./images/Ebola_virus_molecular_structure.png)
> **FIGURE1 | Ebola virus molecular structure.** The Ebola genomeis composed of 3 leader, nucleoprotein(NP), virion protein 35(VP35),VP40,glycoprotein(GP),VP30,VP24,polymerase(L) protein and 5 trailer(adapted from SIB SWISS Institute of Bioinformatics,2014).

![](./images/Schematic_representation_of_the model_for_EBOV_infection.png)
> **Figure 2. Schematic representation of the model for EBOV infection.** Target cells (U) are replenished with rate λ and die with rate ρ. Virus (V) infects target cells (U) with rate β. Infected cells are cleared with rate δ. Once cells are productively infected (I), they release virus at rate p and virus particles are cleared with rate c.

The EBOV infection model is considered as follows:

```vbnet
dU/dt = λ − ρU − βUV    (1)
dI/dt =     βUV − δI    (2)
dV/dt =     pI − cV     (3)
```

###### Parameters

+ **λ = U<sub>0</sub>ρ**

###### Initial Value
